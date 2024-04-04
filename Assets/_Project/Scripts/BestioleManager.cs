using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BestioleManager : MonoBehaviour
{
    public FlyweightSettings BestioleSettings;
    public int PopulationGroupSize = 10;
    public int PopulationGroupCount = 3; // so that will be 20 bestioles
    public float MinSpawnRadius = 10f;
    public float MaxSpawnRadius = 12f;
    public int CurrentGeneration = 0;
    public int MaxGeneration = 100;
    public int MaxBestioleToReproducePerPopulation = 5;
    
    private Dictionary<int, List<Bestiole>> _bestiolesPerPopulation = new Dictionary<int, List<Bestiole>>();
    private Storm _storm;

    private void Awake()
    {
        _storm = FindAnyObjectByType<Storm>();
        if (_storm == null)
        {
            Debug.LogError("No Storm found in the scene");
        }
    }

    [Button(ButtonSizes.Large)]
    public void SpawnAllBestioles()
    {
        StartCoroutine(SpawnAllBestiolesCoroutine());
    }

    [Button(ButtonSizes.Large)]
    public void StartLoop()
    {
        StartCoroutine(StartLoopCoroutine());
    }
   
    public IEnumerator StartLoopCoroutine()
    {
        yield return SpawnAllBestiolesCoroutine();

        yield return new WaitForSeconds(3f);
        
        while (CurrentGeneration < MaxGeneration)
        {
            // Wait until there is 1 or less Bestiole active in the entire scene
            yield return new WaitUntil(() => 
                _bestiolesPerPopulation.Values.Sum(population => population.Count(bestiole => bestiole.gameObject.activeSelf)) <= 0);

            // End of generation: calculate fitness for all remaining active Bestioles
            CalculateFitness();

            // Generate the new generation based on fitness
            GenerateNewGeneration();
            
            // Respawn all Bestioles for the new generation
            yield return RespawnAllBestiolesCoroutine();

            // Add a short delay before starting the next generation loop
            yield return new WaitForSeconds(3f);
        }
    }


    private IEnumerator SpawnAllBestiolesCoroutine()
    {
        
        _bestiolesPerPopulation.Clear();
        // Spread the spawn over multiple frames
        for (int i = 0; i < PopulationGroupCount; i++)
        {
            _bestiolesPerPopulation.Add(i, new List<Bestiole>());
            for (int j = 0; j < PopulationGroupSize; j++)
            {
                // Generate a random angle between 0 and 360 degrees (in radians)
                float angle = Random.Range(0, Mathf.PI * 2);

                // Generate a random radius between 10 and 12 meters
                float radius = Random.Range(MinSpawnRadius, MaxSpawnRadius);

                // Convert polar coordinates to Cartesian coordinates
                Vector2 spawnPosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));

                var flyweight = FlyweightFactory.Spawn(BestioleSettings);
                var bestiole = flyweight.GetComponent<Bestiole>();
                flyweight.gameObject.transform.position = spawnPosition;
                _bestiolesPerPopulation[i].Add(bestiole);

                yield return null;
            }
        }
    }
    
    private IEnumerator RespawnAllBestiolesCoroutine()
    {
        // Iterate through each population
        foreach (var populationEntry in _bestiolesPerPopulation)
        {
            List<Bestiole> bestiolesInPopulation = populationEntry.Value;

            // Iterate through each Bestiole in the population
            for (int j = 0; j < bestiolesInPopulation.Count; j++)
            {
                // Generate a random angle between 0 and 360 degrees (in radians)
                float angle = Random.Range(0, Mathf.PI * 2);

                // Generate a random radius between MinSpawnRadius and MaxSpawnRadius
                float radius = Random.Range(MinSpawnRadius, MaxSpawnRadius);

                // Convert polar coordinates to Cartesian coordinates for the new position
                Vector2 spawnPosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));

                // Respawn the Bestiole at the new position
                var bestiole = bestiolesInPopulation[j];
                var flyweight = FlyweightFactory.Spawn(bestiole);
                flyweight.transform.position = spawnPosition;
                yield return null;
            }
        }
    }

    private void CalculateFitness()
    {
        foreach (var bestioles in _bestiolesPerPopulation.Values)
        {
            foreach (var bestiole in bestioles)
            {
                bestiole.Fitness = bestiole.killNumber * 10 + bestiole.lifeTime;
            }
        }
    }

    private void GenerateNewGeneration()
    {
        Dictionary<int, List<Genome>> newPopulationsGenomes = new Dictionary<int, List<Genome>>();

        foreach (var populationEntry in _bestiolesPerPopulation)
        {
            int populationId = populationEntry.Key;
            List<Bestiole> bestioles = populationEntry.Value;

            float totalFitness = bestioles.Sum(b => b.Fitness);

            List<Genome> selectedGenomesForReproduction = new List<Genome>();

            // Select genomes for reproduction
            for (int i = 0; i < MaxBestioleToReproducePerPopulation; i++)
            {
                float randomPoint = Random.value * totalFitness;
                float currentSum = 0;
                foreach (var bestiole in bestioles)
                {
                    currentSum += bestiole.Fitness;
                    if (currentSum >= randomPoint)
                    {
                        var offspringGenome = bestiole.Genome.Clone();
                        offspringGenome.Mutate();
                        selectedGenomesForReproduction.Add(offspringGenome);
                        break; // Found the genome for this iteration
                    }
                }
            }

            // Now, ensure the population size matches PopulationGroupSize by cloning and mutating selected genomes
            List<Genome> newGenomes = new List<Genome>();
            while (newGenomes.Count < PopulationGroupSize)
            {
                foreach (var genome in selectedGenomesForReproduction)
                {
                    if (newGenomes.Count >= PopulationGroupSize) break; // Ensure not to exceed the population size

                    // Clone and potentially mutate again for variation
                    var newGenome = genome.Clone();
                    newGenome.Mutate(); // Optional: You might choose to mutate or not here for additional diversity
                    newGenomes.Add(newGenome);
                }
            }

            // Assign the new list of genomes to the new population
            newPopulationsGenomes[populationId] = newGenomes;
        }

        // Assign the Bestiole.Genome per population
        // Assuming newPopulationsGenomes is filled with new genomes for each population
        foreach (var populationEntry in _bestiolesPerPopulation)
        {
            int populationId = populationEntry.Key;
            List<Bestiole> bestiolesInPopulation = populationEntry.Value;
            List<Genome> genomesForPopulation = newPopulationsGenomes[populationId];

            for (int i = 0; i < bestiolesInPopulation.Count; i++)
            {
                // It's crucial that the size of genomesForPopulation matches bestiolesInPopulation.Count
                // Set the new genome for each Bestiole
                if (bestiolesInPopulation.Count != genomesForPopulation.Count)
                {
                    Debug.LogError("Genome count does not match Bestiole count");
                    return;
                }
                
                bestiolesInPopulation[i].Genome = genomesForPopulation[i];
        
                // Optionally, if the Bestiole needs to be reset or repositioned after getting a new genome,
                // you can call a method here to do that. For example:
                // bestiolesInPopulation[i].ResetToInitialConditions();

                // If Bestioles are deactivated and need to be reactivated, do it here
                // bestiolesInPopulation[i].gameObject.SetActive(true);
            }
        }
        CurrentGeneration++;
    }



    
    private void MutateAllBestioles()
    {
        foreach (var bestioles in _bestiolesPerPopulation.Values)
        {
            foreach (var bestiole in bestioles)
            {
                bestiole.Genome.Mutate();
            }
        }
    }

    private void OnDrawGizmos()
    {
        // show a circle collider the size of min and max spawn radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, MinSpawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, MaxSpawnRadius);
    }
}

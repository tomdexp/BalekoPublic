using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private Dictionary<int, List<Bestiole>> _bestioles = new Dictionary<int, List<Bestiole>>();
    private readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    
    [Button(ButtonSizes.Large)]
    public void SpawnAllBestioles()
    {
        StartCoroutine(SpawnAllBestiolesCoroutine());
    }

    private IEnumerator SpawnAllBestiolesCoroutine()
    {
        _bestioles.Clear();
        // Spread the spawn over multiple frames
        for (int i = 0; i < PopulationGroupCount; i++)
        {
            _bestioles.Add(i, new List<Bestiole>());
            for (int j = 0; j < PopulationGroupSize; j++)
            {
                // Generate a random angle between 0 and 360 degrees (in radians)
                float angle = Random.Range(0, Mathf.PI * 2);

                // Generate a random radius between 10 and 12 meters
                float radius = Random.Range(MinSpawnRadius, MaxSpawnRadius);

                // Convert polar coordinates to Cartesian coordinates
                Vector2 spawnPosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));

                // TODO: Use Object Pooling here
                var flyweight = FlyweightFactory.Spawn(BestioleSettings);
                var bestiole = flyweight.GetComponent<Bestiole>();
                _bestioles[i].Add(bestiole);

                yield return null;
            }
        }
    }

    
    private void MutateAllBestioles()
    {
        foreach (var bestioles in _bestioles.Values)
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

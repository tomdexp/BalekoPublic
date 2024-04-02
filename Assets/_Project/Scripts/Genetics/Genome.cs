using System;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    public List<Gene> Genes = new List<Gene>();
    public List<Tuple<Gene, Gene>> OpposingGenes = new List<Tuple<Gene, Gene>>();
    public uint MutationCount = 0;

    public T GetGene<T>() where T : Gene
    {
        foreach (var gene in Genes)
        {
            if (gene is T geneBase)
            {
                return geneBase;
            }
        }

        return null;
    }
    
    public bool HasGene<T>() where T : Gene
    {
        foreach (var gene in Genes)
        {
            if (gene is T)
            {
                return true;
            }
        }

        return false;
    }
    
    public void Mutate()
    {
        MutationCount++;
        foreach (var gene in Genes)
        {
            gene.Mutate();
        }
        HandleOpposingGenes();
    }
    
    public static Genome CreateFromParents(Genome parent1, Genome parent2)
    {
        var genome = new Genome();
        foreach (var gene1 in parent1.Genes)
        {
            foreach (var gene2 in parent2.Genes)
            {
                if (gene1.GetType() == gene2.GetType())
                {
                    // Use reflection to call the CreateFromParents<T> method with the correct type
                    var method = typeof(Gene).GetMethod(nameof(Gene.CreateFromParents))?.MakeGenericMethod(gene1.GetType());
                    var newGene = method?.Invoke(null, new object[] { gene1, gene2 });
                
                    genome.Genes.Add((Gene)newGene);
                    break;
                }
            }
        }
        return genome;
    }
    
    private void HandleOpposingGenes()
    {
        foreach (var opposingGene in OpposingGenes)
        {
            var gene1 = opposingGene.Item1;
            var gene2 = opposingGene.Item2;
            // apply the deviation of gene1 on gene2, so gene2 mutations are ignored
            gene2.HandleOpposingGene(gene1);
            
            /*// get the one with the most deviation (this technique doesn't work because it tends to go into the extremes and never come back)
            var deviation1 = gene1.GetValueDeviationPercentage();
            var deviation2 = gene2.GetValueDeviationPercentage();
            var absDeviation1 = Math.Abs(deviation1);
            var absDeviation2 = Math.Abs(deviation2);
            if (Math.Abs(absDeviation1 - absDeviation2) < 0.01f)
            {
                continue;
            }
            
            // Special case where on of the gene has already a high deviation
            if (absDeviation1 > 0.85f)
            {
                gene1.HandleOpposingGene(gene2);
                continue;
            }
            if (absDeviation2 > 0.85f)
            {
                gene2.HandleOpposingGene(gene1);
                continue;
            }
            
            if (absDeviation1 > absDeviation2)
            {
                // debug opposing genes and their deviation
                Debug.Log($"Opposing Genes: {gene1.GetType().Name} and {gene2.GetType().Name} with deviation {deviation1} and {deviation2}");
                gene2.HandleOpposingGene(gene1);
            }
            else
            {
                // debug opposing genes and their deviation
                Debug.Log($"Opposing Genes: {gene1.GetType().Name} and {gene2.GetType().Name} with deviation {deviation1} and {deviation2}");
                gene1.HandleOpposingGene(gene2);
            }*/
        }
    }
    
    public void AddOpposingGene<T>(T gene1, T gene2) where T : Gene
    {
        OpposingGenes.Add(new Tuple<Gene, Gene>(gene1, gene2));
    }
    
    public override string ToString()
    {
        var result = "";
        result += $"Mutation Count: {MutationCount}\n";
        foreach (var gene in Genes)
        {
            result += gene + "\n";
        }
        return result;
    }
}

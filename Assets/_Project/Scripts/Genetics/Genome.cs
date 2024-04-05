using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Genome
{
    public List<Gene> Genes = new List<Gene>();
    public List<Tuple<Type, Type>> OpposingGenes = new List<Tuple<Type, Type>>();
    public uint MutationCount = 0;
    public List<Genome> GenomeHistory = new List<Genome>();

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
        SaveInHistory();
    }

    private void SaveInHistory()
    {
        List<Gene> copiedGenes = new List<Gene>();
        foreach (var gene in Genes)
        {
            copiedGenes.Add(gene.Clone());
        }
        GenomeHistory.Add(new Genome
        {
            Genes = copiedGenes,
            MutationCount = MutationCount
        });
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
    
    public Genome Clone()
    {
        var genome = new Genome();
        foreach (var gene in Genes)
        {
            genome.Genes.Add(gene.Clone());
        }
        genome.MutationCount = MutationCount;
        genome.GenomeHistory = new List<Genome>(GenomeHistory);
        genome.OpposingGenes = OpposingGenes;
        return genome;
    }
    
    private void HandleOpposingGenes()
    {
        // Assuming OpposingGenes is now a List<Tuple<Type, Type>>
        foreach (var (geneType1, geneType2) in OpposingGenes)
        {
            // Find genes of the specific types in the Genes list
            var gene1 = Genes.FirstOrDefault(g => g.GetType() == geneType1);
            var gene2 = Genes.FirstOrDefault(g => g.GetType() == geneType2);

            if (gene1 != null && gene2 != null)
            {
                // Apply the deviation of gene1 on gene2, so gene2's mutations are in response to gene1
                gene2.HandleOpposingGene(gene1);
            }
        }
    }

    
    public void AddOpposingGene<T1, T2>() where T1 : Gene where T2 : Gene
    {
        Type geneType1 = typeof(T1);
        Type geneType2 = typeof(T2);
        OpposingGenes.Add(new Tuple<Type, Type>(geneType1, geneType2));
    }

    
    public void BuildDefaultGenome()
    {
        // the data is in Assets/_Project/Resources/GlobalGenesSettings.asset
        GlobalGenesSettings genesSettings = Resources.Load<GlobalGenesSettings>("GlobalGenesSettings");
        
        // BUILD AND ADD ALL DEFAULT GENES
        GeneSize geneSize = GeneBuilder<GeneSize>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneSize>())
            .Build();
        Genes.Add(geneSize);
        
        GeneHealth geneHealth = GeneBuilder<GeneHealth>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneHealth>())
            .Build();
        Genes.Add(geneHealth);
        
        GeneAttackSpeed geneAttackSpeed = GeneBuilder<GeneAttackSpeed>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneAttackSpeed>())
            .Build();
        Genes.Add(geneAttackSpeed);
        
        GenePrecision genePrecision = GeneBuilder<GenePrecision>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GenePrecision>())
            .Build();
        Genes.Add(genePrecision);
        
        GeneProjectileCount geneProjectileCount = GeneBuilder<GeneProjectileCount>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneProjectileCount>())
            .Build();
        Genes.Add(geneProjectileCount);
        
        GeneProjectileSpeed geneProjectileSpeed = GeneBuilder<GeneProjectileSpeed>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneProjectileSpeed>())
            .Build();
        Genes.Add(geneProjectileSpeed);
        
        GeneProjectileSize geneProjectileSize = GeneBuilder<GeneProjectileSize>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneProjectileSize>())
            .Build();
        Genes.Add(geneProjectileSize);
        
        GeneVisionRange geneVisionRange = GeneBuilder<GeneVisionRange>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneVisionRange>())
            .Build();
        Genes.Add(geneVisionRange);
        
        GeneVisionWidth geneVisionWidth = GeneBuilder<GeneVisionWidth>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneVisionWidth>())
            .Build();
        Genes.Add(geneVisionWidth);
        
        GeneRotationSpeed geneRotationSpeed = GeneBuilder<GeneRotationSpeed>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneRotationSpeed>())
            .Build();
        Genes.Add(geneRotationSpeed);
        
        GeneMovementSpeed geneMovementSpeed = GeneBuilder<GeneMovementSpeed>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneMovementSpeed>())
            .Build();
        Genes.Add(geneMovementSpeed);
        
        GeneProjectileLifespan geneProjectileLifespan = GeneBuilder<GeneProjectileLifespan>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneProjectileLifespan>())
            .Build();
        Genes.Add(geneProjectileLifespan);
        
        // PAIR OPPOSING GENES
        AddOpposingGene<GeneSize, GeneHealth>();
        AddOpposingGene<GeneAttackSpeed, GenePrecision>();
        AddOpposingGene<GeneProjectileCount, GeneProjectileLifespan>();
        AddOpposingGene<GeneProjectileSpeed, GeneProjectileSize>();
        AddOpposingGene<GeneVisionRange, GeneVisionWidth>();
        AddOpposingGene<GeneRotationSpeed, GeneMovementSpeed>();
    }
    
    public override string ToString()
    {
        var result = "";
        result += $"Mutation Count: {MutationCount}\n";
        foreach (var gene in Genes)
        {
            result += gene + "\n";
        }
        result += $"Oppositions : \n";
        foreach (var (gene1, gene2) in OpposingGenes)
        {
            result += $"{gene1.Name}<->{gene2.Name}\n";
        }
        return result;
    }
}

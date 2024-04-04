using System;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    public List<Gene> Genes = new List<Gene>();
    public List<Tuple<Gene, Gene>> OpposingGenes = new List<Tuple<Gene, Gene>>();
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
    
    private void HandleOpposingGenes()
    {
        foreach (var (gene1, gene2) in OpposingGenes)
        {
            // apply the deviation of gene1 on gene2, so gene2 mutations are ignored
            gene2.HandleOpposingGene(gene1);
        }
    }
    
    public void AddOpposingGene<T>(T gene1, T gene2) where T : Gene
    {
        OpposingGenes.Add(new Tuple<Gene, Gene>(gene1, gene2));
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
        
        GeneProjectileRange geneProjectileRange = GeneBuilder<GeneProjectileRange>.CreateGene()
            .WithSettings(genesSettings.GetGeneSettings<GeneProjectileRange>())
            .Build();
        Genes.Add(geneProjectileRange);
        
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
        AddOpposingGene<Gene>(geneSize, geneHealth);
        AddOpposingGene<Gene>(geneAttackSpeed, genePrecision);
        AddOpposingGene<Gene>(geneProjectileCount, geneProjectileRange);
        AddOpposingGene<Gene>(geneProjectileSpeed, geneProjectileSize);
        AddOpposingGene<Gene>(geneVisionRange, geneVisionWidth);
        AddOpposingGene<Gene>(geneRotationSpeed, geneMovementSpeed);
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

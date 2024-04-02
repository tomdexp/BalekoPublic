using System.Collections.Generic;

public class Genome
{
    public List<Gene> Genes = new List<Gene>();

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
    
    // create a static method to create a genom from 2 parent genomes
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
}

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class GeneTesterEditorWindow : OdinEditorWindow
{
    [MenuItem("Window/Baleko/Gene Tester")]
    private static void OpenWindow()
    {
        GetWindow<GeneTesterEditorWindow>().Show();
    }
    
    [Button(ButtonSizes.Large)]
    private void TestGenome()
    {
        var parent1 = new Genome();
        parent1.Genes.Add(new GeneMovementSpeed());
        parent1.Genes.Add(new GeneSize());
        
        Debug.Log("Parent 1: \n" + parent1);
    }
    
    [Button(ButtonSizes.Large, ButtonStyle.FoldoutButton)]
    private void TestGenomeWithMutation(int mutationCount = 50)
    {
        var parent1 = new Genome();
        
        // We use the GeneBuilder to create the genes because it allows method like WithMutationRate to be called
        GeneMovementSpeed geneMovementSpeed = GeneBuilder<GeneMovementSpeed>.CreateGene().Build();
        GeneSize geneSize = GeneBuilder<GeneSize>.CreateGene().Build();
        parent1.Genes.Add(geneMovementSpeed);
        parent1.Genes.Add(geneSize);
        parent1.AddOpposingGene<Gene>(geneMovementSpeed, geneSize); // this mean geneSpeed is dominant and we only take the mutation of geneSpeed into account
        
        Debug.Log("Beginning Mutation:");
        Debug.Log("Parent 1: \n" + parent1);
        
        for (var i = 0; i < mutationCount; i++)
        {
            parent1.Mutate();
            Debug.Log("Parent 1: \n" + parent1);
        }
        Debug.Log("Mutation Complete");
    }
    
    [Button(ButtonSizes.Large)]
    private void TestBuildDefaultGenome()
    {
        Genome genome = new Genome();
        genome.BuildDefaultGenome();
        
        Debug.Log("Default Genome: \n" + genome);
    }
}
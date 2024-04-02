using UnityEngine;

public abstract class Gene
{
    public float Value = 0.0f;
    public float MinValue = -1.0f;
    public float MaxValue = 1.0f;
    public float MutationRate = 0.1f;
    public float MinMutationRate = 0.01f;
    public float MaxMutationRate = 0.5f;
    public float MutationChance = 0.1f;
    
    public void Mutate()
    {
        if (Random.value > MutationChance) return;
        var mutation = Random.Range(-MutationRate, MutationRate);
        Value += Value * mutation;
        MutationRate += MutationRate * mutation;
        Clamp();
    }

    private void Clamp()
    {
        Value = Mathf.Clamp(Value, MinValue, MaxValue);
        MutationRate = Mathf.Clamp(MutationRate, MinMutationRate, MaxMutationRate);
    }
    
    public static T CreateFromParents<T>(Gene parent1, Gene parent2) where T : Gene, new()
    {
        var gene = new T
        {
            Value = (parent1.Value + parent2.Value) / 2.0f,
            MutationRate = (parent1.MutationRate + parent2.MutationRate) / 2.0f
        };
        return gene;
    }
}
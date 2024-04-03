using UnityEngine;

public abstract class Gene
{
    public float Value = 0.0f;
    public float MinValue = -1.0f;
    public float MaxValue = 1.0f;
    public float MutationRate = 0.1f;
    public float MinMutationRate = 0.01f;
    public float MaxMutationRate = 0.5f;
    public float MutationChance = 0.5f;
    public uint MutationCount = 0;
    
    public void Mutate()
    {
        if (Random.value > MutationChance) return;
        MutationCount++;
        var mutation = Random.Range(-MutationRate, MutationRate);
        var range = MaxValue - MinValue;
        Value += range * mutation;
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

    public float GetValueDeviationPercentage()
    {
        return (Value / (MaxValue - MinValue)) * 2.0f;
    }
    
    public void HandleOpposingGene(Gene otherGene)
    {
        var oppositeDeviation = -otherGene.GetValueDeviationPercentage();
        Value = ((MaxValue - MinValue) * oppositeDeviation) / 2;
        Clamp();
    }

    public override string ToString()
    {
        return $"{GetType().Name} -> " +
               $"[Value:{Value}|Min:{MinValue}|Max:{MaxValue}] " +
               $" [MutationRate:{MutationRate}|MutationChance:{MutationChance}|MutationCount:{MutationCount}|Min:{MinMutationRate}|Max:{MaxMutationRate}|Deviation:{GetValueDeviationPercentage()}]";
    }
}
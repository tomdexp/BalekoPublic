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
        if (Value == 0) return 0;
        if (Value < 0) // Negative value
        {
            if (MinValue == 0) return 0;
            return -(Value / MinValue);
        }
        else
        {
            return Value / MaxValue;
        }
        //return (Value / (MaxValue - MinValue)) * 2.0f;
    }
    
    public void SetValueFromDeviationPercentage(float deviationPercentage)
    {
        if (deviationPercentage == 0)
        {
            Value = 0;
        }
        else if (deviationPercentage < 0) // Negative deviation
        {
            Value = MinValue * -deviationPercentage;
        }
        else // Positive deviation
        {
            Value = MaxValue * deviationPercentage;
        }
    }
    
    public void HandleOpposingGene(Gene otherGene)
    {
        var oppositeDeviation = -otherGene.GetValueDeviationPercentage();
        SetValueFromDeviationPercentage(oppositeDeviation);
        Clamp();
        var diff = Mathf.Abs(GetValueDeviationPercentage() + otherGene.GetValueDeviationPercentage());
        if (diff > 0.1f)
        {
            Debug.LogWarning($"Opposing genes are too far apart: {GetType().Name} and {otherGene.GetType().Name}");
        }
    }

    public override string ToString()
    {
        return $"{GetType().Name} -> " +
               $"[Value:{Value}|Min:{MinValue}|Max:{MaxValue}] " +
               $" [MutationRate:{MutationRate}|MutationChance:{MutationChance}|MutationCount:{MutationCount}|Min:{MinMutationRate}|Max:{MaxMutationRate}|Deviation:{GetValueDeviationPercentage()}]";
    }
    
    public Gene Clone()
    {
        return (Gene)MemberwiseClone();
    }
}
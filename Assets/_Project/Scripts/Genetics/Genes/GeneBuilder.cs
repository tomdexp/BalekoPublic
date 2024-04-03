using UnityEngine;

public class GeneBuilder<T> where T : Gene, new()
{
    private T _gene = new T();

    public static GeneBuilder<T> CreateGene()
    {
        return new GeneBuilder<T>();
    }

    public GeneBuilder<T> WithValue(float value)
    {
        _gene.Value = value;
        return this;
    }

    public GeneBuilder<T> WithMinValue(float minValue)
    {
        _gene.MinValue = minValue;
        return this;
    }

    public GeneBuilder<T> WithMaxValue(float maxValue)
    {
        _gene.MaxValue = maxValue;
        return this;
    }

    public GeneBuilder<T> WithMutationRate(float mutationRate)
    {
        _gene.MutationRate = mutationRate;
        return this;
    }

    public GeneBuilder<T> WithMinMutationRate(float minMutationRate)
    {
        _gene.MinMutationRate = minMutationRate;
        return this;
    }

    public GeneBuilder<T> WithMaxMutationRate(float maxMutationRate)
    {
        _gene.MaxMutationRate = maxMutationRate;
        return this;
    }

    public GeneBuilder<T> WithMutationChance(float mutationChance)
    {
        _gene.MutationChance = mutationChance;
        return this;
    }
    
    public GeneBuilder<T> WithSettings(GlobalGenesSettings.GeneSetting settings)
    {
        _gene.MinValue = settings.MinValue;
        _gene.MaxValue = settings.MaxValue;
        _gene.MutationRate = settings.MutationRate;
        _gene.MinMutationRate = settings.MinMutationRate;
        _gene.MaxMutationRate = settings.MaxMutationRate;
        _gene.MutationChance = settings.MutationChance;
        return this;
    }

    public T Build()
    {
        return _gene;
    }
}

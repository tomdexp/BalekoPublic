using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Damageable : MonoBehaviour
{
    [HideInInspector] public UnityEvent<float> OnSubstractValue = new UnityEvent<float>();
    [HideInInspector] public UnityEvent<float> OnAddValue = new UnityEvent<float>();
    [HideInInspector] public UnityEvent OnZeroValue;
    public float CurrentValue;
    public float MaxValue;

    [Button]
    public void Substract(float damage)
    {
        if (CurrentValue <= 0) return;
        CurrentValue -= damage;
        if (CurrentValue <= 0) Death();
        OnSubstractValue?.Invoke(damage);
    }

    public void Add(float heal)
    {
        CurrentValue += heal;
        OnSubstractValue?.Invoke(heal);
    }

    public void Death()
    {
        CurrentValue = 0;
        OnZeroValue?.Invoke();
    }
}

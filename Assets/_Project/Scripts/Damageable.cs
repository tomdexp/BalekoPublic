using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Damageable : MonoBehaviour
{
    [HideInInspector] public UnityEvent<float> OnReduceValue = new UnityEvent<float>();
    [HideInInspector] public UnityEvent OnZeroValue;
    public float CurrentValue;
    public float MaxValue;

    [Button]
    public void Damage(float damage)
    {        
        CurrentValue -= damage;
        if (CurrentValue <= 0) Death();
        OnReduceValue?.Invoke(damage);
    }

    public void Death()
    {
        CurrentValue = 0;
        OnZeroValue?.Invoke();
    }
}

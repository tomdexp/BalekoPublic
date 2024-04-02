using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Damageable : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> OnDamage = new UnityEvent<int>();
    [HideInInspector] public UnityEvent OnDeath;
    public float CurrentHealth;

    [Button]
    public void Damage(int damage)
    {        
        CurrentHealth -= damage;
        if (CurrentHealth <= 0) Death();
        OnDamage?.Invoke(damage);
    }

    public void Death()
    {
        CurrentHealth = 0;
        OnDeath?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bestiole : MonoBehaviour
{
    public Damageable Damageable;
    public SpriteRenderer SpriteRenderer;

    public void Awake()
    {
        if (Damageable)
        {
            //Damageable.MaxHealth = _enemyData.MaxHealth; //will be used to setup current life on start
            Damageable.OnDamage.AddListener(OnDamaged);
            Damageable.OnDeath.AddListener(OnDead);
        }
    }

    public void OnDamaged(int damage)
    {
        SpriteRenderer.transform.DOScale(1.5f, .1f).OnComplete(() =>
        {
            SpriteRenderer.transform.DOScale(1f, .1f);
        });
    }

    public void OnDead()
    {
        Destroy(gameObject);
    }
}

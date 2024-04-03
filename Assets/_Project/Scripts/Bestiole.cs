using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bestiole : MonoBehaviour
{
    public Damageable Damageable;
    public Damageable Hungerable;
    public SpriteRenderer SpriteRenderer;

    public void Awake()
    {
        if (Damageable)
        {
            //Damageable.MaxHealth = _enemyData.MaxHealth; //will be used to setup current life on start
            Damageable.OnReduceValue.AddListener(OnDamaged);
            Damageable.OnZeroValue.AddListener(OnDead);
            //Hungerable.MaxValue = _enemyData.MaxHealth; //will be used to setup current life on start
        }
    }

    private void Update()
    {
        Hungerable.Damage(0.1f);
    }

    public void OnDamaged(float damage)
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

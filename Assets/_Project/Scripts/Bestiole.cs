using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bestiole : MonoBehaviour
{
    public Genome Genome;
    public Damageable Damageable;
    public Damageable Hungerable;
    public UI_BarValue HealthBar;
    public UI_BarValue HungerBar;
    public SpriteRenderer SpriteRenderer;

    public void Awake()
    {
        if (Damageable)
        {
            //Damageable.MaxHealth = _enemyData.MaxHealth; //will be used to setup current life on start
            Damageable.OnReduceValue.AddListener(OnDamaged);
            Damageable.OnZeroValue.AddListener(OnDead);
            Hungerable.OnReduceValue.AddListener(OnHungered);
            Hungerable.OnZeroValue.AddListener(OnHungerDead);
            //Hungerable.MaxValue = _enemyData.MaxHealth; //will be used to setup current life on start
        }
    }

    private void Update()
    {
        Hungerable.Damage(0.02f);
    }

    public void OnDamaged(float damage)
    {
        SpriteRenderer.transform.DOScale(0.75f, .1f).OnComplete(() =>
        {
            SpriteRenderer.transform.DOScale(0.5f, .1f);
        });
        HealthBar.SetBarValue(Damageable.CurrentValue, Damageable.MaxValue);
    }

    public void OnDead()
    {
        Destroy(gameObject);
    }

    public void OnHungered(float damage)
    {
        HungerBar.SetBarValue(Hungerable.CurrentValue, Hungerable.MaxValue);

    }

    public void OnHungerDead()
    {
        Destroy(gameObject);
    }
}

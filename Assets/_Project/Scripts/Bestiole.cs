using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bestiole : Flyweight
{
    public new BestioleSettings Settings
    {
        get => (BestioleSettings)base.Settings;
        set => base.Settings = value;
    }
    
    public Genome Genome;
    public Damageable Damageable;
    public Damageable Hungerable;
    public UI_BarValue HealthBar;
    public UI_BarValue HungerBar;
    public SpriteRenderer SpriteRenderer;
    public Transform BulletSpawnPoint;

    [Header("Stats")]
    public float lifeTime;
    public int killNumber;
    
    private Movement _movement;

    public void Awake()
    {
        _movement = GetComponent<Movement>();
        
        if (Damageable)
        {
            Damageable.OnReduceValue.AddListener(OnDamaged);
            Damageable.OnZeroValue.AddListener(OnDead);
            Hungerable.OnReduceValue.AddListener(OnHungered);
            Hungerable.OnZeroValue.AddListener(OnHungerDead);
        }
    }

    public void SetupBestiole()
    {
        if (Genome == null)
        {
            Genome = new Genome();
            Genome.BuildDefaultGenome();
            Genome.Mutate();
        }
        
        lifeTime = 0;
        killNumber = 0;
        Damageable.MaxValue = Settings.DefaultMaxHealth + Settings.DefaultMaxHealth * Genome.GetGene<GeneHealth>().Value;
        Damageable.CurrentValue = Damageable.MaxValue;
        Hungerable.MaxValue = Settings.DefaultMaxHunger; // TODO : GeneHunger
        Hungerable.CurrentValue = Hungerable.MaxValue;
        _movement.Speed = Settings.DefaultMovementSpeed + Settings.DefaultMovementSpeed * Genome.GetGene<GeneMovementSpeed>().Value;
        _movement.RotateSpeed = Settings.DefaultRotationSpeed + Settings.DefaultRotationSpeed * Genome.GetGene<GeneRotationSpeed>().Value;
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
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
        transform.DOKill();
        FlyweightFactory.ReturnToPool(this);
    }

    public void OnHungered(float damage)
    {
        HungerBar.SetBarValue(Hungerable.CurrentValue, Hungerable.MaxValue);
    }

    public void OnHungerDead()
    {
        FlyweightFactory.ReturnToPool(this);
    }
}

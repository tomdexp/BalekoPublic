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
    
    [Header("Scripts")]
    public Genome Genome;
    public Damageable Damageable;
    public Damageable Hungerable;
    public UI_BarValue HealthBar;
    public UI_BarValue HungerBar;
    public Vision Vision;

    [Header("Components")]
    public SpriteRenderer SpriteRenderer;
    public Transform BulletSpawnPoint;
    public SpriteRenderer OutlineSpriteRenderer;

    [Header("Stats")]
    public float lifeTime;
    public int killNumber;
    
    private Movement _movement;

    [Header("Flyweight Factory Settings")]
    public CollectableSettings CollectableSettings;

    public List<Bestiole> targetList = new List<Bestiole>();
    public List<Collectable> collectableList = new List<Collectable>();

    public void Awake()
    {
        _movement = GetComponent<Movement>();
        
        if (Damageable)
        {
            Damageable.OnSubstractValue.AddListener(OnDamaged);
            Damageable.OnZeroValue.AddListener(OnDead);
        }

        if (Hungerable)
        {
            Hungerable.OnSubstractValue.AddListener(OnHungered);
            Hungerable.OnZeroValue.AddListener(OnHungerDead);
        }
        if (Vision)
        {
            Vision.OnEnemySpotted.AddListener(OnEnemySpotted);
            Vision.OnCollectableSpotted.AddListener(OnCollectableSpotted);
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
        
        Vision.VisionRange = Settings.DefaultVisionRange + Settings.DefaultVisionRange * Genome.GetGene<GeneVisionRange>().Value;
        Vision.Fov = Settings.DefaultVisionWidth + Settings.DefaultVisionWidth * Genome.GetGene<GeneVisionWidth>().Value;
        lifeTime = 0;
        killNumber = 0;
        Damageable.MaxValue = Settings.DefaultMaxHealth + Settings.DefaultMaxHealth * Genome.GetGene<GeneHealth>().Value;
        Damageable.CurrentValue = Damageable.MaxValue;
        HealthBar.SetBarValue(Damageable.CurrentValue, Damageable.MaxValue);
        Hungerable.MaxValue = Settings.DefaultMaxHunger; // TODO : GeneHunger
        Hungerable.CurrentValue = Hungerable.MaxValue;
        HungerBar.SetBarValue(Hungerable.CurrentValue, Hungerable.MaxValue);
        _movement.Speed = Settings.DefaultMovementSpeed + Settings.DefaultMovementSpeed * Genome.GetGene<GeneMovementSpeed>().Value;
        _movement.RotateSpeed = Settings.DefaultRotationSpeed + Settings.DefaultRotationSpeed * Genome.GetGene<GeneRotationSpeed>().Value;
        targetList.Clear();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        Hungerable.Substract(0.02f * Time.timeScale);
    }

    public void OnDamaged(float damage)
    {
        if (Damageable.CurrentValue > 0)
        {
            SpriteRenderer.transform.DOScale(0.75f, .1f).OnComplete(() =>
            {
                SpriteRenderer.transform.DOScale(0.5f, .1f);
            });
        }
        HealthBar.SetBarValue(Damageable.CurrentValue, Damageable.MaxValue);
    }

    public void OnDead()
    {
        transform.DOKill();
        SpriteRenderer.transform.DOScale(0.75f, .1f).OnComplete(() =>
        {
            SpriteRenderer.transform.DOScale(0.5f, .1f).OnComplete(() =>
            {
                var collectableFlyweight = FlyweightFactory.Spawn(CollectableSettings);
                collectableFlyweight.transform.position = transform.position;
                FlyweightFactory.ReturnToPool(this);
            });
        });
    }

    public void OnHungered(float damage)
    {
        HungerBar.SetBarValue(Hungerable.CurrentValue, Hungerable.MaxValue);
    }

    public void OnHungerDead()
    {
        SpriteRenderer.transform.DOScale(0f, .1f).OnComplete(() =>
        {
            FlyweightFactory.ReturnToPool(this);
        });
    }

    public void OnEnemySpotted(GameObject go)
    {
        Bestiole bestiole = go.GetComponent<Bestiole>();
        if (bestiole != null && !targetList.Contains(bestiole))
            targetList.Add(bestiole);
    }

    public void OnCollectableSpotted(GameObject go)
    {
        Collectable collectable = go.GetComponent<Collectable>();
        if (collectable != null && !collectableList.Contains(collectable))
            collectableList.Add(collectable);
    }
}

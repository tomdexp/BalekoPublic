﻿using UnityEngine;

[CreateAssetMenu(fileName = nameof(BestioleSettings), menuName = "ScriptableData/" + nameof(BestioleSettings))]
public class BestioleSettings : FlyweightSettings
{
    [Header("Bestiole Settings")] 
    public float DefaultMovementSpeed;
    public float DefaultRotationSpeed;
    public float DefaultMaxHealth;
    public float DefaultMaxHunger;

    public override Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(true);
        go.name = Prefab.name;

        var bestiole = go.GetComponent<Bestiole>();
        bestiole.Settings = this;
        bestiole.SetupBestiole();

        return bestiole;
    }
    
    public override void OnGet(Flyweight flyweight)
    {
        var bestiole = (Bestiole)flyweight;
        bestiole.SetupBestiole();
        flyweight.gameObject.SetActive(true);
    }
}
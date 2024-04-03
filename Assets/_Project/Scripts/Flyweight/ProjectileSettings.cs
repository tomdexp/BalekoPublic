using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProjectileSettings), menuName = "ScriptableData/" + nameof(ProjectileSettings))]
public class ProjectileSettings : FlyweightSettings
{
    [Header("Projectile Settings")]
    public float DefaultLifespan = 10f;
    public float DefaultSpeed = 10f;
    public float DefaultDamage = 30f;
    public float DefaultSize;
    
    public override Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(true);
        go.name = Prefab.name;
        
        var flyweight = go.GetComponent<Bullet>();
        flyweight.Settings = this;
        
        return flyweight;
    }
}

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
        GameObject parent = GameObject.Find(Prefab.name + " Pool");
        if (parent == null)
        {
            parent = new GameObject(Prefab.name + " Pool");
        }
        
        var go = Instantiate(Prefab, parent.transform);
        go.SetActive(true);
        go.name = Prefab.name;
        
        var flyweight = go.GetComponent<Bullet>();
        flyweight.Settings = this;
        
        return flyweight;
    }
}

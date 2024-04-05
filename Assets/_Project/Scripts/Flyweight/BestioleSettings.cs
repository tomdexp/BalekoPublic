using UnityEngine;

[CreateAssetMenu(fileName = nameof(BestioleSettings), menuName = "ScriptableData/" + nameof(BestioleSettings))]
public class BestioleSettings : FlyweightSettings
{
    [Header("Bestiole Settings")] 
    public float DefaultMovementSpeed;
    public float DefaultRotationSpeed;
    public float DefaultMaxHealth;
    public float DefaultMaxHunger;
    public float DefaultVisionRange = 3;
    public float DefaultVisionWidth = 128;
    public float DefaultAttackSpeed = 2;
    [Tooltip("Correspond to the GenePrecision")]
    public float DefaultAttackAngle = 30;
    public float DefaultSize = 1;
    public int DefaultProjectileCount = 1;

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
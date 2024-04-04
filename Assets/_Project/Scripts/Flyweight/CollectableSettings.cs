using UnityEngine;

[CreateAssetMenu(fileName = nameof(CollectableSettings), menuName = "ScriptableData/" + nameof(CollectableSettings))]
public class CollectableSettings : FlyweightSettings
{
    [Header("Collectable Settings")]
    public float HungerRestoreAmount = 10f;
    
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

        var bestiole = go.GetComponent<Collectable>();
        bestiole.Settings = this;
        return bestiole;
    }
}

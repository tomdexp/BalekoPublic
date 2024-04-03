using UnityEngine;

[CreateAssetMenu(fileName = nameof(FlyweightSettings), menuName = "ScriptableData/" + nameof(FlyweightSettings))]
public class FlyweightSettings : ScriptableObject
{
    public FlyweightType Type;
    public GameObject Prefab;
    
    public virtual Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(true);
        go.name = Prefab.name;
        
        var flyweight = go.AddComponent<Flyweight>();
        flyweight.Settings = this;
        
        return flyweight;
    }
    
    public virtual void OnGet(Flyweight flyweight)
    {
        flyweight.gameObject.SetActive(true);
    }
    
    public virtual void OnRelease(Flyweight flyweight)
    {
        flyweight.gameObject.SetActive(false);
    }
    
    public virtual void OnDestroyPoolObject(Flyweight flyweight)
    {
        Destroy(flyweight.gameObject);
    }
}
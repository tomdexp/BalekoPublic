using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FlyweightFactory : MonoBehaviour
{
    [field: SerializeField] public bool CollisionCheck { get; private set; }
    [field: SerializeField] public int DefaultCapacity { get; private set; } = 100;
    [field: SerializeField] public int MaxPoolSize { get; private set; } = 400;
    
    public static FlyweightFactory Instance;
    private readonly Dictionary<FlyweightType, IObjectPool<Flyweight>> _pools = new();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public static Flyweight Spawn(FlyweightSettings settings)
    {
        return Instance.GetPoolForType(settings).Get();
    }
    
    public static void ReturnToPool(Flyweight flyweight)
    {
        Instance.GetPoolForType(flyweight.Settings).Release(flyweight);
    }
    
    IObjectPool<Flyweight> GetPoolForType(FlyweightSettings settings)
    {
        IObjectPool<Flyweight> pool;
        
        if(_pools.TryGetValue(settings.Type, out pool)) return pool;
        
        pool = new ObjectPool<Flyweight>(
            settings.Create,
            settings.OnGet,
            settings.OnRelease,
            settings.OnDestroyPoolObject,
            CollisionCheck,
            DefaultCapacity,
            MaxPoolSize);
        
        _pools.Add(settings.Type, pool);
        return pool;
    }

    
}

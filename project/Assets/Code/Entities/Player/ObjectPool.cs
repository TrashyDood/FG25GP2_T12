using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a generic base for an object-pool. Is used as a parent class
/// for a pool tailored to a specific object type.
/// </summary>
/// <typeparam name="T">The object type to be pooled.</typeparam>
[Serializable]
public abstract class ObjectPool<T>
    where T : IPoolable<T>
{
    [SerializeField]
    T _template;
    
    Queue<T> _objects = new Queue<T>();
    
    public T Template => _template;

    public T FetchObject()
    {
        T result = (_objects.Count > 0) ? _objects.Dequeue() : CreateObject(_template);
        result.OnFetched(this);
        OnFetched(result);

        return result;
    }

    public bool PoolObject(T obj)
    {
        if (obj.IsPooled)
            return false;
        
        _objects.Enqueue(obj);
        obj.OnPooled();
        OnPooled(obj);
        
        return true;
    }

    protected abstract T CreateObject(T template);
    
    protected abstract void OnFetched(T obj);
    protected abstract void OnPooled(T obj);
}

public abstract class PoolableComponent<T> : MonoBehaviour, IPoolable<T>
where T : PoolableComponent<T>
{
    T derived;
    ObjectPool<T> pool;
    bool isPooled;

    public bool IsPooled => pool != null && isPooled;
    public T Derived => derived;

    public PoolableComponent() =>
        derived = (T)this;
    
    public virtual void OnPooled()
    {
        isPooled = true;
        gameObject.SetActive(false);
    }

    public virtual void OnFetched(ObjectPool<T> pool)
    {
        this.pool = pool;
        isPooled = false;
        gameObject.SetActive(true);
    }

    public bool ReturnToPool() => pool.PoolObject(derived);
}

[Serializable]
public class PoolableComponent :
    PoolableComponent<PoolableComponent> { }

/// <summary>
/// Defines an object pool specifically for a given object type that derives from PoolableComponent.
/// </summary>
/// <typeparam name="T">The type which derives from PoolableComponent.</typeparam>
[Serializable]
public class ComponentPool<T> : ObjectPool<T>
where T : PoolableComponent<T>
{
    protected override T CreateObject(T template) =>
        GameObject.Instantiate(template);

    protected override void OnFetched(T obj) =>
        obj.gameObject.SetActive(true);

    protected override void OnPooled(T obj) =>
    obj.gameObject.SetActive(false);
}

[Serializable]
public class ObjectPoolLibrary<TIdentifier, TObject, TPool>
    where TObject : IPoolable<TObject>
    where TPool : ObjectPool<TObject>, new()
{
    [SerializeField]
    List<Pool> pools = new();
    [SerializeField]
    TIdentifier identifier1;

    Dictionary<TIdentifier, int> indices = new();

    public T FetchObject<T>(TIdentifier identifier) where T : class, IPoolable<TObject>
    {
        if (!indices.TryGetValue(identifier, out var index))
            return pools[index].pool.FetchObject() as T;

        indices.Add(identifier, index = pools.Count);
        pools.Add(new());

        return pools[indices[identifier]].pool.FetchObject() as T;
    }

    [Serializable]
    struct Pool
    {
        public TIdentifier identifier;
        public TPool pool;
    }
}

[Serializable]
public class ComponentPoolLibrary<TIdentifier> :
    ObjectPoolLibrary<TIdentifier, PoolableComponent, ComponentPool<PoolableComponent>> { }

public interface IPoolable<TObject>
    where TObject : IPoolable<TObject>
{
    bool IsPooled { get; }
    void OnPooled();
    void OnFetched(ObjectPool<TObject> pool);
    bool ReturnToPool();
}
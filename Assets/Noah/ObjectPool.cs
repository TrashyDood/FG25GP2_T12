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

public abstract class PoolableComponent<T> : MonoBehaviour, IPoolable<T>
where T : PoolableComponent<T>
{
    T _derived;
    ObjectPool<T> _pool;
    bool _isPooled;

    public bool IsPooled => _pool != null && _isPooled;
    public T Derived => _derived;

    public PoolableComponent() =>
        _derived = (T)this;
    
    public virtual void OnPooled()
    {
        _isPooled = true;
        gameObject.SetActive(false);
    }

    public virtual void OnFetched(ObjectPool<T> pool)
    {
        _pool = pool;
        _isPooled = false;
        gameObject.SetActive(true);
    }

    public bool ReturnToPool() => _pool.PoolObject(_derived);
}

public interface IPoolable<TObject>
    where TObject : IPoolable<TObject>
{
    bool IsPooled { get; }
    void OnPooled();
    void OnFetched(ObjectPool<TObject> pool);
    bool ReturnToPool();
}
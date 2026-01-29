using System;
using UnityEngine;

public class Enemy : PoolableComponent<Enemy>
{
    [SerializeField]
    Transform target;
    [SerializeField]
    EnemyData data;

    float health;

    public float Health
    {
        get => health;
        set
        {
            OnHealthChanged(value);
            health = value;
        }
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, data.speed * Time.fixedDeltaTime);
    }

    void OnHealthChanged(float newHealth)
    {
        if (Mathf.Round(newHealth) < 1)
        {
            ReturnToPool();
            foreach (var item in data.dropOnDeath)
                Instantiate(item, transform.position, Quaternion.identity);
        }
    }

    public override void OnFetched(ObjectPool<Enemy> pool)
    {
        base.OnFetched(pool);
    }

    public void SetData(EnemyData data)
    {
        health = data.maxHealth;
    }
}

[Serializable]
public class EnemyData
{
    public GameObject[] dropOnDeath;
    public float maxHealth, speed;

    // max health, damage, range o andra attributer
}
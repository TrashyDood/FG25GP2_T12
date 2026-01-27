using UnityEngine;

public class Enemy : PoolableComponent<Enemy>
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float speed = 5;
    [SerializeField]
    float maxHealth = 20,
        health = 20;

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
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
    }

    void OnHealthChanged(float newHealth)
    {
        if (Mathf.Round(newHealth) < 1)
        {
            ReturnToPool();
        }
    }

    public override void OnFetched(ObjectPool<Enemy> pool)
    {
        base.OnFetched(pool);
    }
}

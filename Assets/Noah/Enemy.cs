using UnityEngine;

public class Enemy : PoolableComponent<Enemy>
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float speed = 5;

    float timer;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            ReturnToPool();
            timer = 5;
        }
        else
            timer -= Time.fixedDeltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
    }

    public override void OnFetched(ObjectPool<Enemy> pool)
    {
        base.OnFetched(pool);
        timer = 5;
    }
}

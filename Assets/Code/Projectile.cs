using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    LayerMask targetLayer;

    Ray ray;
    Vector3 direction;
    uint pierceRemaining;
    Enemy piercedEnemy;
    float speed,
        damage,
        width = 0.1f;

    private void Awake()
    {
        Destroy(gameObject, 10);
    }

    void FixedUpdate()
    {
        ray = new(transform.position, direction * speed * Time.fixedDeltaTime);

        if (Physics.SphereCast(ray, width, out RaycastHit hitInfo, speed * Time.fixedDeltaTime, targetLayer))
        {
            OnHit(hitInfo);
            return;
        }

        transform.SetPositionAndRotation(transform.position + direction * speed * Time.fixedDeltaTime, Quaternion.LookRotation(direction));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
#endif

    public void Initialize(Vector3 direction, float speed, float damage = 10, uint pierceCount = 1)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.pierceRemaining = pierceCount;
    }

    void OnHit(RaycastHit hitInfo)
    {
        Enemy enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();     

        if (enemy != null && enemy != piercedEnemy)
        {
            enemy.CurrentHealth -= damage;
            piercedEnemy = enemy;
        }

        if (enemy == null || pierceRemaining < 1)
            Destroy(gameObject);
    }
}

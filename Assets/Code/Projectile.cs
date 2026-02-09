using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    LayerMask targetLayer;

    Ray ray;
    Vector3 direction;
    int pierceRemaining;
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

        transform.SetLocalPositionAndRotation(transform.position + direction * speed * Time.fixedDeltaTime, Quaternion.LookRotation(transform.position + direction * speed * Time.fixedDeltaTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }

    public void Initialize(Vector3 direction, float speed, float damage = 10, int pierceCount = 1)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.pierceRemaining = pierceCount;
    }

    void OnHit(RaycastHit hitInfo)
    {
        Enemy enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();     

        if (enemy != null)
        {
            enemy.CurrentHealth -= damage;
        }

        Destroy(gameObject);
    }
}

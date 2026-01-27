using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    LayerMask targetLayer;

    Ray ray;
    Vector3 direction;
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
        transform.position += direction * speed * Time.fixedDeltaTime;

        if (Physics.SphereCast(ray, width, out RaycastHit hitInfo, speed * Time.fixedDeltaTime, targetLayer))
            OnHit(hitInfo);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }

    public void Initialize(Vector3 direction, float speed, float damage = 10)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
    }

    void OnHit(RaycastHit hitInfo)
    {
        Enemy enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();

        Destroy(gameObject);

        if (enemy != null)
            enemy.Health -= damage;
    }
}

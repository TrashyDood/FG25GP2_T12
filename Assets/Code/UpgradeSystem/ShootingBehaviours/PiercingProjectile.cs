using UnityEngine;

public class PiercingProjectile : MonoBehaviour
{
    [SerializeField]
    LayerMask targetLayer;

    Ray ray;
    Vector3 direction;

    float speed;
    float damage;
    float width = 0.1f;

    int pierceRemaining;

    private void Awake()
    {
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        // Cast forward
        if (Physics.SphereCast(
            ray,
            width,
            out RaycastHit hitInfo,
            speed * Time.fixedDeltaTime,
            targetLayer))
        {
            OnHit(hitInfo);
        }

        ray = new Ray(transform.position, direction * speed * Time.fixedDeltaTime);
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    public void Initialize(
        Vector3 direction,
        float speed,
        float damage,
        int pierceCount
    )
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.pierceRemaining = pierceCount;
    }

    void OnHit(RaycastHit hitInfo)
    {
        Enemy enemy = hitInfo.collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.Health -= damage;

            pierceRemaining--;

            if (pierceRemaining <= 0)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = hitInfo.point + direction * 0.05f;
        }
        else
        {
            // hit environment
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(ray);
    }
}

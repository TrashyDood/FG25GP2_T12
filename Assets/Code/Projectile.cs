using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    LayerMask targetLayer;

    Vector3 direction;
    float speed,
        damage;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.position + direction, out RaycastHit hitInfo, speed, targetLayer))
            OnHit(hitInfo);
    }

    public void Initialize(Vector3 direction, float speed, float damage = 10)
    {
        this.direction = direction;
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

using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;

    Vector3 velocity = new();
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        UpdateVelocity();
    }
    public void Initialize(Vector3 Velocity, float Speed)
    {
        transform.forward = Velocity;
        velocity = Velocity.normalized * Speed;
    }
    private void UpdateVelocity()
    {
        transform.position += projectileSpeed * Time.deltaTime * velocity;
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            //Destroy(enemy.gameObject);
            // When destroying an enemy, it still targets that removed enemy,
            // maybe do a list for all spawned enemies
            // and remove that enemy from the list so it forgets that enemy
            Destroy(gameObject);
        }
    }
}

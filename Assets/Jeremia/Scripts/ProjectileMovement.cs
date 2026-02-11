using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    Rigidbody rb;
    public float projectileSpeed = 1.0f;
    public Vector3 projectileDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = projectileSpeed * projectileDirection;
    }
}

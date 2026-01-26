using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public GameObject target;
    public float moveSpeed;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, target.transform.position, moveSpeed * Time.deltaTime));
        transform.LookAt(target.transform.position);    
    }
}

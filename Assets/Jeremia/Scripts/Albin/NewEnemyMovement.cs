using UnityEngine;

public class NewEnemyMovement : MonoBehaviour
{
    [HideInInspector] public bool stop = false;

    [Header("Movement")]
    public GameObject target;
    public float moveSpeed;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        if (!stop)  // Only move if not damaging the player
        {
            rb.MovePosition(Vector3.MoveTowards(
                rb.position,
                target.transform.position,
                moveSpeed * Time.deltaTime));
        }

        transform.LookAt(target.transform.position);
    }
}

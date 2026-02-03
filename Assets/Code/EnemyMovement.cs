using UnityEngine;

[RequireComponent(typeof(Enemy), typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    Rigidbody rb;
    Enemy enemy;

    public Enemy Enemy => enemy;
    public Rigidbody Rigidbody => rb;
    public EnemyData Data => enemy?.Data;
    public Transform Target => enemy?.Target;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, Target.transform.position, enemy.Data.speed * Time.deltaTime));
    }
}

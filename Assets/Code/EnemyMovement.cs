using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Enemy), typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    Rigidbody rb;
    Enemy enemy;

    public Enemy Enemy => enemy;
    public Rigidbody Rigidbody => rb;
    public EnemyData Data => enemy?.Data;
    public Transform Target => enemy?.Target;
    
    [SerializeField] private float knockbackDamping = 30f;

    private Vector3 knockbackVelocity;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    private void OnEnable()
    {
        knockbackVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Vector3 toTarget = Target.transform.position;
        Vector3 desiredPos = Vector3.MoveTowards(rb.position, toTarget, enemy.Data.speed * Time.deltaTime);

        if (knockbackVelocity.sqrMagnitude > 0.001f)
        {
            desiredPos += knockbackVelocity * Time.deltaTime;
            knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, Time.deltaTime * knockbackDamping);
            
            if (knockbackVelocity.sqrMagnitude <= 0.0001f)
                knockbackVelocity = Vector3.zero;
        }
        
        rb.MovePosition(Vector3.MoveTowards(rb.position, Target.transform.position, enemy.Data.speed * Time.deltaTime));
    }

    public void ApplyKnockback(Vector3 direction, float speed)
    {
        direction.y = 0f;
        
        if (direction.sqrMagnitude > 0.001f)
            return;

        knockbackVelocity = direction.normalized * speed;
    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMovement), typeof(SphereCollider))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    LayerMask targetLayer;
    [SerializeField]
    UnityEvent onAttackkEvent;
    [SerializeField]
    EnemyData data;

    Ray attackRay;
    EnemyMovement movement;
    SphereCollider collider;
    float currentHealth,
        attackTimer;

    public SphereCollider Collider => collider;
    public EnemyMovement Movement => movement;

    public float Health
    {
        get => currentHealth;
        set
        {
            OnHealthChanged(value);
            currentHealth = value;
        }
    }

    public EnemyData Data
    {
        get => data;
        set
        {
            OnDataChanged(value);
            data = value;
        }
    }

    public Transform Target
    {
        get => target;
        set
        {
            OnTargetChanged(value);
            target = value;
        }
    }

    private void Awake()
    {
        Data = data; // Initialize data.

        collider = GetComponent<SphereCollider>();
        movement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        Target = GameManager.Instance.Player.transform;
    }

    private void FixedUpdate()
    {
        attackRay = new Ray(transform.position, target.position - transform.position);

        if (attackTimer <= 0 && Physics.Raycast(attackRay, out RaycastHit hitInfo, collider.radius + data.attackRange, targetLayer))
        {
            Attack(hitInfo);
            attackTimer = data.attackCooldown;
        }
        else
            attackTimer -= Time.fixedDeltaTime;
    }

    void Attack(RaycastHit hitInfo)
    {
        hitInfo.collider.GetComponent<BodyPart>()?.TakeDamage(data.attackDamage);
    }

    void OnHealthChanged(float newHealth)
    {
        if (Mathf.Round(newHealth) < 1)
        {
            Destroy(gameObject);
            foreach (var item in data.dropOnDeath)
                Instantiate(item, transform.position + Utils.RandomVector3(1, 0, 1, -1, 0, -1), Quaternion.identity);
        }
    }

    void OnDataChanged(EnemyData newData)
    {
        currentHealth = newData.maxHealth;
    }

    void OnTargetChanged(Transform newTarget)
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(attackRay);
    }
#endif
}

[Serializable]
public class EnemyData
{
    public GameObject[] dropOnDeath;
    public float maxHealth = 50,
        speed = 5,
        attackDamage = 20,
        attackRange = 5,
        attackCooldown = 1,
        size;

    // attackSpeed kan vara istället att spelaren har i-frames
}
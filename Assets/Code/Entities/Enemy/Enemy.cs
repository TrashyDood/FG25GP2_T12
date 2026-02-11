using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMovement), typeof(CapsuleCollider))]
public class Enemy : Entity
{
    [SerializeField]
    Transform target;
    [SerializeField]
    LayerMask targetLayer;
    [SerializeField]
    EnemyData data;

    [Header("Events")]
    [SerializeField]
    UnityEvent onAttackEvent;

    Ray attackRay;
    EnemyMovement movement;
    CapsuleCollider collider;
    float attackTimer;

    public CapsuleCollider Collider => collider;
    public EnemyMovement Movement => movement;

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

    protected override void Awake()
    {
        base.Awake();

        Data = data; // Initialize data.

        collider = GetComponent<CapsuleCollider>();
        movement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        Target = GameManager.Instance.Player.transform;
    }

    protected override void FixedUpdate()
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

    void OnDataChanged(EnemyData newData) { }
    void OnTargetChanged(Transform newTarget) { }

    protected override void OnDeath()
    {
        base.OnDeath();

        Destroy(gameObject);
        foreach (var item in data.dropOnDeath)
            Instantiate(item, transform.position + Utils.RandomVector3(1, 0, 1, -1, 0, -1), Quaternion.identity);
    }
}

[Serializable]
public class EnemyData
{
    public GameObject[] dropOnDeath;
    public float speed = 5,
        attackDamage = 20,
        attackRange = 5,
        attackCooldown = 1;

    // attackSpeed kan vara istället att spelaren har i-frames
}
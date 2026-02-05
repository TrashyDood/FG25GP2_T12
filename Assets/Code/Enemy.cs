using System;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : PoolableComponent<Enemy>
{
    [SerializeField]
    Transform target;
    [SerializeField]
    LayerMask targetLayer;
    [SerializeField]
    EnemyData data;

    EnemyMovement movement;
    Material material;
    MeshRenderer meshRenderer;
    float health,
        attackTimer;

    public EnemyMovement Movement => movement;

    public float Health
    {
        get => health;
        set
        {
            OnHealthChanged(value);
            health = value;
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
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        movement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void FixedUpdate()
    {
        if (attackTimer <= 0 && Physics.SphereCast(transform.position, 1, target.position - transform.position, out RaycastHit hitInfo, data.attackRange, targetLayer))
        {
            Attack(hitInfo);
            attackTimer = 1 / data.attackSpeed;
        }
        else
            attackTimer -= Time.fixedDeltaTime;
    }

    void Attack(RaycastHit hitInfo)
    {
        BodyPart bodyPart = hitInfo.collider.GetComponent<BodyPart>();
        if (bodyPart != null)
            bodyPart.TakeDamage(data.attackDamage);
    }

    void OnHealthChanged(float newHealth)
    {
        if (Mathf.Round(newHealth) < 1)
        {
            ReturnToPool();
            foreach (var item in data.dropOnDeath)
                Instantiate(item, transform.position + Utils.RandomVector3(1, 0, 1, -1, 0, -1), Quaternion.identity);
        }
    }

    void OnDataChanged(EnemyData newData)
    {
        health = newData.maxHealth;
        meshRenderer.material = newData.material;
    }

    void OnTargetChanged(Transform newTarget)
    {

    }
}

[Serializable]
public class EnemyData
{
    public GameObject[] dropOnDeath;
    public Mesh model;
    public Material material;
    public float maxHealth = 50,
        speed = 5,
        attackDamage = 20,
        attackRange = 5,
        attackSpeed = 1,
        size;

    // attackSpeed kan vara istället att spelaren har i-frames
}
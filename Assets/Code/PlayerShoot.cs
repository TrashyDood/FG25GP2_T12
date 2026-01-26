using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float attackSpeed;
    private float attackTimer;

    [SerializeField] private float attackRange;
    [SerializeField] private GameObject projectilePrefab;

    public GameObject[] allEnemies;
    public GameObject nearestEnemy;
    private float distance;
    private float closestDistance = float.MaxValue;

    void Start()
    {
        attackTimer = attackSpeed;
    }

    void Update()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemies.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, allEnemies[i].transform.position);

            if (distance < closestDistance)
            {
                nearestEnemy = allEnemies[i];
                closestDistance = distance;
            }
        }

        if (attackSpeed <= 0f)
        {
            ShootClosestEnemy();
            attackSpeed = attackTimer;
        }
        else 
        { 
            attackSpeed -= Time.deltaTime;
        }
    }
    private void ShootClosestEnemy()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector3 closestEnemy = nearestEnemy.transform.position;
        closestEnemy.y = 0f;
        Vector3 direction = (closestEnemy - transform.position).normalized;

        projectile.GetComponent<Projectile>().Initialize(direction, 4);

        closestDistance = float.MaxValue;
        print("closest distance to enemy is: " + nearestEnemy);

    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/Targeted Shot")]
public class TargetedShotBehaviour : ShootingBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float maxRange = 15f;

    public override void Shoot(WeaponInstance weapon)
    {

        Transform firePoint = weapon.FirePoint;

        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint missing for TargetedShot!");
            return;
        }

        Enemy target = FindClosestEnemy(firePoint.position);

        if (target == null)
            return;

        Vector3 direction =
            (target.transform.position - firePoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            rotation
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }

    private Enemy FindClosestEnemy(Vector3 origin)
    {
        Enemy[] enemies = Object.FindObjectsOfType<Enemy>();

        Enemy closest = null;
        float closestDist = maxRange * maxRange;

        foreach (Enemy enemy in enemies)
        {
            float dist =
                (enemy.transform.position - origin).sqrMagnitude;

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }
}

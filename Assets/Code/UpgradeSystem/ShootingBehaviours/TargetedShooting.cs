using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/Targeted Shot")]
public class TargetedShotBehaviour : ShootingBehaviour
{
    [Header("Projectile Settings")]
    public Projectile projectilePrefab;   
    public float projectileSpeed = 24f;  
    public float maxRange = 15f;         

    public override void Shoot(WeaponInstance weapon)
    {
        if (weapon.FirePoint == null || projectilePrefab == null)
        {
            Debug.LogWarning($"{weapon.definition.weaponName} missing FirePoint or projectilePrefab");
            return;
        }

        Enemy target = FindClosestEnemy(weapon.FirePoint.position);
        if (target == null) return;

        Vector3 direction = (target.transform.position - weapon.FirePoint.position).normalized;

        Projectile proj = Instantiate(projectilePrefab, weapon.FirePoint.position, Quaternion.LookRotation(direction));

        proj.Initialize(direction, projectileSpeed, weapon.currentStats.damage);
    }

    private Enemy FindClosestEnemy(Vector3 origin)
    {
        Enemy[] enemies = Object.FindObjectsOfType<Enemy>();
        Enemy closest = null;
        float closestDistSq = maxRange * maxRange;

        foreach (Enemy enemy in enemies)
        {
            float distSq = (enemy.transform.position - origin).sqrMagnitude;
            if (distSq <= closestDistSq)
            {
                closestDistSq = distSq;
                closest = enemy;
            }
        }

        return closest;
    }
}

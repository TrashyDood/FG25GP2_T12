using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/Targeted Shot")]
public class TargetedShotBehaviour : ShootingBehaviour
{
    [Header("Projectile Settings")]
    public Projectile projectilePrefab;   
    public float projectileSpeed = 24f;  
    public float maxRange = 15f;

    public BodyPart BodyPart;

    public override void Shoot(WeaponInstance weapon)
    {

        BodyPart?.OnActivated();

        if (weapon.FirePoint == null || projectilePrefab == null)
        {
            Debug.LogWarning($"{weapon.definition.weaponName} missing FirePoint or projectilePrefab");
            return;
        }

        
        RaycastHit hit;
        float coneAngleRadians = Mathf.Deg2Rad * 90f;
        uint rayCount = 16;

        bool found = Utils.RadiusCast(
            weapon.FirePoint.position,
            weapon.FirePoint.forward,
            maxRange,
            coneAngleRadians,
            rayCount,
            out hit,
            LayerMask.GetMask("Enemy")
        );

        if (!found)
        {
            //Debug.LogError("No enemies found");
            return;
        }

        Debug.Log("Enemies found");

        Enemy target = hit.collider.GetComponentInParent<Enemy>();
        
        if (target == null) return;

        Vector3 direction = (target.transform.position - weapon.FirePoint.position).normalized;

        Projectile proj = Instantiate(projectilePrefab, weapon.FirePoint.position, Quaternion.LookRotation(direction));

        proj.Initialize(direction, projectileSpeed, weapon.currentStats.damage);

        

        /*
        Enemy target = FindClosestEnemy(weapon.FirePoint.position);
        if (target == null) return;

        Vector3 direction = (target.transform.position - weapon.FirePoint.position).normalized;

        Projectile proj = Instantiate(projectilePrefab, weapon.FirePoint.position, Quaternion.LookRotation(direction));

        proj.Initialize(direction, projectileSpeed, weapon.currentStats.damage);
        */
        
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

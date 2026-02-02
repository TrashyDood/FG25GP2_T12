using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/QuillShot")]
public class QuillShot : ShootingBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public int basePierceCount = 3;

    public override void Shoot(WeaponInstance weapon, Transform firePoint)
    {
        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Projectile projectile =
            proj.GetComponent<Projectile>();

        projectile.Initialize(
            firePoint.forward,
            projectileSpeed,
            weapon.currentStats.damage,  
            basePierceCount
        );
    }
}

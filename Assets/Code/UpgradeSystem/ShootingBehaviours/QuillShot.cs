using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/QuillShot")]
public class QuillShot : ShootingBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public int basePierceCount = 3;

    public override void Shoot(WeaponInstance weapon)
    {
        if (weapon.FirePoint == null)
        {
            Debug.LogWarning("FirePoint not found!");
            return;
        }

        GameObject proj = Instantiate(
            projectilePrefab,
            weapon.FirePoint.position,
            weapon.FirePoint.rotation
        );

        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.Initialize(
            weapon.FirePoint.forward,
            projectileSpeed,
            weapon.currentStats.damage,
            basePierceCount
        );
    }
}

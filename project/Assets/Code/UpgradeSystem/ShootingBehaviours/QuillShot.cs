using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/QuillShot")]
public class QuillShot : ShootingBehaviour
{
    public Projectile projectilePrefab;
    public float projectileSpeed = 5f;
    public uint basePierceCount = 3;

    public override void Shoot(WeaponInstance weapon)
    {
        if (weapon.FirePoint == null)
        {
            Debug.LogWarning("FirePoint not found!");
            return;
        }

        Projectile proj = Instantiate(
            projectilePrefab,
            weapon.FirePoint.position,
            weapon.FirePoint.rotation
        );

        
        proj.Initialize(
            weapon.FirePoint.forward,
            projectileSpeed,
            weapon.currentStats.damage,
            basePierceCount
        );
    }
}

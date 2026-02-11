using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/NoShootBehaviour")]
public class NoShootBehaviour : ShootingBehaviour
{
    public override void Shoot(WeaponInstance weapon)
    {
        // Do nothing
    }
}
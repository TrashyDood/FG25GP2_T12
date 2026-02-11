using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/ActivateShield")]
public class ActivateShield : ShootingBehaviour
{
    public override void Shoot(WeaponInstance weapon)
    {
        Shield shield = weapon.weaponObject.GetComponent<Shield>();
        if (shield == null)
        {
            Debug.LogError("Shield component missing on weaponObject");
            return;
        }

        shield.gameObject.SetActive(true);
        Debug.Log("Shield Activated");
    }
}
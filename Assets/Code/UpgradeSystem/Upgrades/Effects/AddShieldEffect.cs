using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Effects/AddShieldEffect")]
public class AddShieldEffect : AddWeaponEffect
{
    public GameObject shieldPrefab;
    public int shieldHP = 100;

    public override void Apply(PlayerBuild build)
    {
        // Use the base AddWeaponEffect logic if you want
        base.Apply(build);

        // Then add your shield-specific setup
        WeaponInstance shieldInstance = build.weapons.Find(w => w.definition.weaponId == weapon.weaponId);
        if (shieldInstance != null)
        {
            // Add Shield component to the weaponObject
            Shield shieldComp = shieldInstance.weaponObject.AddComponent<Shield>();
            shieldComp.maxHP = shieldHP;
            shieldComp.currentHP = shieldHP;
        }
    }
}
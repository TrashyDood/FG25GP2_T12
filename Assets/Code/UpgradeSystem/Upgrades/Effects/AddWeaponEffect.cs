using UnityEngine;


// Upgrade effect that gives the player a new weapon
// or levels up an existing one
[CreateAssetMenu(menuName = "Upgrades/Effects/Add Weapon")]
public class AddWeaponEffect : UpgradeEffect
{
    // Weapon definition to add to the player
    public WeaponDefinition weapon;


    // Adds the weapon to the players build
    public override void Apply(PlayerBuild build)
    {
        if(weapon == null)
        {
            Debug.LogError("AddWeaponEffect has null weaponDef");
        }

        build.AddWeapon(weapon);
    }
}
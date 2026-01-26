using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponLibrary weaponLibrary;
    public string weaponId;

    public void Pickup(PlayerBuild build, Transform playerTransform)
    {
        build.AddWeaponById(weaponId, weaponLibrary, playerTransform);
    }
}
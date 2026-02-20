using UnityEngine;


// THIS SCRIPT IS CURRENTLY NOT IN USE. DONT CALL IT
public class WeaponPickup : MonoBehaviour
{
    public WeaponLibrary weaponLibrary;
    public string weaponId;

    public void Pickup(PlayerBuild build)
    {
        build.AddWeaponById(weaponId, weaponLibrary);
    }
}
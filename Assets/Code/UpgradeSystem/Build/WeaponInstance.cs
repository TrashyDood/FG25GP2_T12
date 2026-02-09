using UnityEngine;
using UnityEngine.Audio;


// Runtime instance of a weapon
[System.Serializable]
public class WeaponInstance
{
    public WeaponDefinition definition;
    public int level;

    public WeaponStats currentStats;

    public float cooldownTimer;

    //public Transform firePoint;
    public BodyPart weaponObject;
    //public Transform firePoint;

    //public Transform FirePoint => firePoint;
    public Transform FirePoint
    {
        get
        {
            if (weaponObject != null)
            {
                // Look for a child named "FirePoint"
                Transform fp = weaponObject.transform.Find("FirePoint");
                return fp != null ? fp : weaponObject.transform; // fallback to root
            }

            // TEMP fallback if no instance exists yet: use prefab transform
            if (definition != null && definition.weaponPrefab != null)
                return definition.weaponPrefab.transform;

            return null;
        }
    }

    public void PlayFireSound()
    {
        if (weaponObject.AudioSource == null || definition.fireClip == null)
            return;

        weaponObject.AudioSource.PlayOneShot(definition.fireClip);
    }
}




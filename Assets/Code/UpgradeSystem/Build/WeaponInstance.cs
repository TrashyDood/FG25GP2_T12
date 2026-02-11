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
            if (weaponObject == null) return null;

            Transform fp = weaponObject.transform.Find("FirePoint");

            /*
            if (fp != null)
               Debug.Log($"{definition.weaponName} using FirePoint at {fp.position}");
               
            else
                Debug.LogWarning($"{definition.weaponName} has no FirePoint, using fallback at {weaponObject.transform.position}");
            */

            return fp != null ? fp : weaponObject.transform;
        }
    }

    public void PlayFireSound()
    {
        if (weaponObject.AudioSource == null || definition.fireClip == null)
            return;

        weaponObject.AudioSource.PlayOneShot(definition.fireClip);
    }
}




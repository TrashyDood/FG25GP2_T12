using UnityEngine;


// Runtime instance of a weapon
[System.Serializable]
public class WeaponInstance
{
    public WeaponDefinition definition;
    public int level;

    public WeaponStats currentStats;

    public float cooldownTimer;

    public Transform firePoint;
    public BodyPart weaponObject;
}

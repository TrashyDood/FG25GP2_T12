using System;
using UnityEngine;

// Container for a weapons stats
// No logic
[Serializable]
public class WeaponStats
{
    public float damage;
    public float fireRate;
    public float range;

    public WeaponStats Clone()
    {
        return new WeaponStats
        {
            fireRate = this.fireRate,
            damage = this.damage,
            range = this.range
        };
    }
}
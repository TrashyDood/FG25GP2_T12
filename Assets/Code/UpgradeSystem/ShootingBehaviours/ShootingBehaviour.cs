using UnityEngine;

// Base class for all shooting behaviours
public abstract class ShootingBehaviour : ScriptableObject
{
    // Called by the weapon runtime
    public abstract void Shoot(WeaponInstance weapon);
}

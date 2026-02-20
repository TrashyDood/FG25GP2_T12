using UnityEngine;

// Abstract base class for all upgrade effects
public abstract class UpgradeEffect : ScriptableObject
{
    // Applies this effect to PlayerBuild
    public abstract void Apply(PlayerBuild build);
}
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

// Scriptable object representing a single upgrade option
// No logic, just data
[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class UpgradeDefinition : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;

    // List of effects that will be applied to the player
    public List<UpgradeEffect> effects;
}


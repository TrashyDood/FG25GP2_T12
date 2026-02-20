using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

// Explanation for how the upgrade and weapon system works:
//
// 1. WeaponDefinition (ScriptableObject) = the blueprint of a weapon.
//    - Stores static information like base stats and name.
//    - Shared across all runs and players.
// 2. WeaponInstance = a runtime instance of a WeaponDefinition.
//    - Created when a player obtains a weapon in a run.
//    - Stores mutable runtime data (current stats, level, etc).
//    - Stored in PlayerBuild.weapons list.
// 3. PlayerBuild = runtime container for the player's state during a run.
//    - Holds Player stats (StatCollection) and owned WeaponInstances.
//    - Discarded at the end of the run; a new instance is created for a new run.
// 4. Upgrades (UpgradeDefinition + UpgradeEffect)
//    - UpgradeDefinitions are ScriptableObjects (data only).
//    - Effects (UpgradeEffect) are applied to PlayerBuild to modify stats or add/level weapons.
//    - Effects do not modify the ScriptableObjects themselves; they only modify runtime data.



// Example script how to upgrade stats etc
public class ExampleCode : MonoBehaviour
{
    // Runtime player build instance
    private PlayerBuild playerBuild;

    // Example: assign some upgrade in the inspector
    public UpgradeDefinition exampleUpgrade;

    private void Awake()
    {
        // Initialize the player build at game start
        playerBuild = new PlayerBuild();

        // Set base stats
        playerBuild.stats.SetBaseStat(StatType.Damage, 10f);
        playerBuild.stats.SetBaseStat(StatType.FireRate, 1f);
        playerBuild.stats.SetBaseStat(StatType.Health, 100f);
        playerBuild.stats.SetBaseStat(StatType.Speed, 5f);
    }

    public void ApplyUpgrade()
    {
        if (exampleUpgrade != null)
        {
            playerBuild.ApplyUpgrade(exampleUpgrade);

            Debug.Log("Applied upgrade: " + exampleUpgrade.upgradeName);
            Debug.Log("Current Damage: " + playerBuild.stats.GetStat(StatType.Damage));
            Debug.Log("Current Speed: " + playerBuild.stats.GetStat(StatType.Speed));
        }
    }

    // Reset the player build for a new run
    public void ResetBuild()
    {
        playerBuild = new PlayerBuild();

        // Reset base stats
        playerBuild.stats.SetBaseStat(StatType.Damage, 10f);
        playerBuild.stats.SetBaseStat(StatType.FireRate, 1f);
        playerBuild.stats.SetBaseStat(StatType.Health, 100f);
        playerBuild.stats.SetBaseStat(StatType.Speed, 5f);

        Debug.Log("Player build reset.");
    }
}

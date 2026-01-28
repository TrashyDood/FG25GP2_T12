using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Library")]

public class UpgradeLibrary : ScriptableObject
{
    public List<UpgradeDefinition> upgrades;

    private Dictionary<string, UpgradeDefinition> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, UpgradeDefinition>();

        foreach (UpgradeDefinition upgrade in upgrades)
        {
            if (upgrade == null || string.IsNullOrEmpty(upgrade.upgradeId))
                continue;

            if (lookup.ContainsKey(upgrade.upgradeId))
            {
                Debug.LogError(
                    $"Duplicate weaponId: {upgrade.upgradeId}"
                );
                continue;
            }

            lookup.Add(upgrade.upgradeId, upgrade);
        }
    }

    public UpgradeDefinition GetUpgrade(string id)
    {
        if (lookup.TryGetValue(id, out var upgrade))
            return upgrade;

        Debug.LogError($"Weapon not found: {id}");
        return null;
    }
}

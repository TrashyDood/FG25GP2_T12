using UnityEngine;

public class LevelTesting : MonoBehaviour
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            ApplyUpgrade();
        }
    }

    public void ApplyUpgrade()
    {
        if (exampleUpgrade != null)
        {
            playerBuild.ApplyUpgrade(exampleUpgrade);

            Debug.Log("Applied upgrade: " + exampleUpgrade.upgradeName);
            Debug.Log("Current Damage: " + playerBuild.stats.GetStat(StatType.Damage));
            Debug.Log("Current Speed: " + playerBuild.stats.GetStat(StatType.Speed));
            Debug.Log("Current Firerate: " + playerBuild.stats.GetStat(StatType.FireRate));
            Debug.Log("Current Health: " + playerBuild.stats.GetStat(StatType.Health));
        }
    }

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

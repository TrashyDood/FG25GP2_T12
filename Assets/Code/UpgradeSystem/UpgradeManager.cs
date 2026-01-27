using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private int choiceCount = 3;

    public List<UpgradeDefinition> UpgradePool = new List<UpgradeDefinition>();

    



    void GetRandomChoices(int choiceCount)
    {
        int upgradeIndex1 = Random.Range(0, UpgradePool.Count);
        int upgradeIndex2 = Random.Range(0, UpgradePool.Count);
        int upgradeIndex3 = Random.Range(0, UpgradePool.Count);

        UpgradeDefinition upgrade1 = UpgradePool[upgradeIndex1];
        UpgradeDefinition upgrade2 = UpgradePool[upgradeIndex2];
        UpgradeDefinition upgrade3 = UpgradePool[upgradeIndex3];


    }
}

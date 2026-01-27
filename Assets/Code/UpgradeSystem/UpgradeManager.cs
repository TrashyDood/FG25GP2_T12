using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private int choiceCount = 3;

    public UpgradeLibrary upgradeLibrary;

    public List<UpgradeDefinition> UpgradePool = new List<UpgradeDefinition>();



    private void Start()
    {
        foreach(UpgradeDefinition upgrade in upgradeLibrary.upgrades)
        {
            UpgradePool.Add(upgrade);
        }
    }



    void GetRandomChoices(int choiceCount)
    {
        int upgradeIndex1 = Random.Range(0, UpgradePool.Count);
 

        UpgradeDefinition upgrade1 = UpgradePool[upgradeIndex1];



    }
}

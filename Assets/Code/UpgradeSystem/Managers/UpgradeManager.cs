using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    private int choiceCount = 3;

    public VoxelBody voxelBody;
    public Player player;
    public UpgradeLibrary upgradeLibrary;
    public WeaponManager weaponManager;

    [Header("Fixed Choice (Left Card)")]
    public UpgradeDefinition fixedChoiceUpgradeDefinition;
    public string fixedChoiceName = "Mass";
    public string fixedChoiceDescription = "";
    public Sprite fixedChoiceIcon;

    [Header("Choice Buttons (children under LevelUpPanel)")]
    [SerializeField] private UpgradeChoiceButton fixedChoiceButton;     // LEFT
    [SerializeField] private UpgradeChoiceButton randomChoiceButton1;   // MIDDLE
    [SerializeField] private UpgradeChoiceButton randomChoiceButton2;   // RIGHT

    public GameObject levelUpPanel;

    public List<UpgradeDefinition> upgradePool = new List<UpgradeDefinition>();

    private void Start()
    {
        foreach (UpgradeDefinition upgrade in upgradeLibrary.upgrades)
        {
            upgradePool.Add(upgrade);
        }

        if (levelUpPanel != null)
            levelUpPanel.SetActive(false);

        GameManager.Instance.GetCBState(GameState.Upgrading).onEnterCallback += (s) => { EnterUpgradeState(); };
    }

    private List<UpgradeDefinition> GetTwoRandomUpgrades()
    {
        if (upgradePool == null || upgradePool.Count < 2)
            throw new InvalidOperationException("UpgradePool must contain at least 2 items.");

        HashSet<int> picked = new HashSet<int>();
        List<UpgradeDefinition> result = new List<UpgradeDefinition>(2);

        while (picked.Count < 2)
        {
            int index = Random.Range(0, upgradePool.Count);
            if (picked.Add(index))
                result.Add(upgradePool[index]);
        }

        return result;
    }

    void ShowChoices()
    {
        if (fixedChoiceUpgradeDefinition == null)
            throw new InvalidOperationException("[UpgradeManager] fixedChoiceUpgradeDefinition is not assigned.");

        if (fixedChoiceButton == null || randomChoiceButton1 == null || randomChoiceButton2 == null)
            throw new InvalidOperationException("[UpgradeManager] Assign the 3 UpgradeChoiceButton references in the Inspector.");

        EnableUi();

        List<UpgradeDefinition> randoms = GetTwoRandomUpgrades();
        UpgradeDefinition upgrade1 = randoms[0];
        UpgradeDefinition upgrade2 = randoms[1];

        // LEFT = fixed choice always
        fixedChoiceButton.Bind(
            payload: fixedChoiceUpgradeDefinition,
            sprite: fixedChoiceIcon != null ? fixedChoiceIcon : fixedChoiceUpgradeDefinition.icon,
            title: fixedChoiceName,
            description: fixedChoiceDescription,
            onClick: OnChoiceClicked
        );

        // other two = random
        randomChoiceButton1.Bind(
            payload: upgrade1,
            sprite: upgrade1.icon,
            title: upgrade1.upgradeName,
            description: "",
            onClick: OnChoiceClicked
        );

        randomChoiceButton2.Bind(
            payload: upgrade2,
            sprite: upgrade2.icon,
            title: upgrade2.upgradeName,
            description: "",
            onClick: OnChoiceClicked
        );
    }

    private void OnChoiceClicked(object payload)
    {
        if (weaponManager == null || weaponManager.Build == null)
        {
            Debug.LogError("WeaponManager or PlayerBuild missing!");
            return;
        }

        if (payload is not UpgradeDefinition upgrade)
        {
            Debug.LogError($"[UpgradeManager] Unexpected payload type: {payload?.GetType().Name ?? "null"}");
            return;
        }

        weaponManager.Build.ApplyUpgrade(upgrade);

        player.Body.EnterEditMode(
            (bodyPart) =>
            {
                GameManager.Instance.StateManager.SetState(GameState.Playing);
            },
            GetWeaponDefinition(upgrade)
        );

        DisableUi();
    }

    public void EnterUpgradeState()
    {
        Debug.Log("Enter upgrade state");
        ShowChoices();
    }

    private void DisableUi()
    {
        if (levelUpPanel != null)
            levelUpPanel.SetActive(false);
    }

    private void EnableUi()
    {
        if (levelUpPanel != null)
            levelUpPanel.SetActive(true);
    }

    public WeaponDefinition GetWeaponDefinition(UpgradeDefinition upgrade)
    {
        if (upgrade == null) return null;
        
        foreach (UpgradeEffect effect in upgrade.effects)
        {
            if (effect is AddWeaponEffect addWeapon)
            {
                return addWeapon.weapon;
            }
        }
        
        return null;
    }
}
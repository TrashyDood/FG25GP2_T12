using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponLibrary weaponLibrary;
    public Player player;

    public PlayerBuild Build => player.Build;
    private List<WeaponInstance> activeWeapons => Build?.weapons;

    public string startingWeaponId = "Quill";

    private void Awake()
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("[WeaponManager] Player not found!");
            return;
        }

        //Build = player.Build; 

        if (Build == null)
        {
            Debug.LogError("[WeaponManager] Player.Build is null!");
        }
    }

    private void Update()
    {
        if (Build == null) return; 

        UpdateCooldowns();


        TryFireWeapons();
        
    }

    private void UpdateCooldowns()
    {
        foreach (var weapon in activeWeapons)
            weapon.cooldownTimer -= Time.deltaTime;
    }

    private void TryFireWeapons()
    {
        foreach (var weapon in activeWeapons)
        {
            if (weapon.FirePoint == null)
            {
                Debug.LogError($"{weapon.definition.weaponName} has no FirePoint!");
                continue;
            }

            if (weapon.cooldownTimer <= 0f)
            {
                weapon.definition.shootingBehaviour.Shoot(weapon);
                weapon.cooldownTimer = weapon.currentStats.fireRate > 0 ? 1f / weapon.currentStats.fireRate : 1f;
            }
        }
    }
}

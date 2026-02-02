using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public WeaponLibrary weaponLibrary;
    public Transform playerTransform;

    private PlayerBuild playerBuild;
    private List<WeaponInstance> activeWeapons;

    public string startingWeaponId = "Quill";

    private void Awake()
    {
        playerBuild = new PlayerBuild();
        activeWeapons = playerBuild.weapons;

        AddDefaultWeapon();
    }

    private void Update()
    {
        UpdateCooldowns();

        if (Input.GetMouseButtonDown(0)) // triggers only on the frame the button is pressed
        {
            TryFireWeapons();
        }
    }

    private void AddDefaultWeapon()
    {
        WeaponDefinition def = weaponLibrary.GetWeapon(startingWeaponId);
        if (def == null)
        {
            Debug.LogError($"Starting weapon not found: {startingWeaponId}");
            return;
        }

        GameObject weaponGO = Instantiate(def.weaponPrefab, playerTransform);
        Transform firePoint = weaponGO.transform.Find("FirePoint");

        WeaponInstance instance = new WeaponInstance
        {
            definition = def,
            level = 1,
            weaponObject = weaponGO,
            firePoint = firePoint,
            currentStats = new WeaponStats
            {
                damage = def.baseStats.damage,
                fireRate = def.baseStats.fireRate,
                range = def.baseStats.range
            }
        };

        playerBuild.weapons.Add(instance);

        Debug.Log($"Default weapon added: {def.weaponName}");
    }

    private void UpdateCooldowns()
    {
        foreach (WeaponInstance weapon in activeWeapons)
        {
            weapon.cooldownTimer -= Time.deltaTime;
        }
    }

    private void TryFireWeapons()
    {
        foreach (WeaponInstance weapon in activeWeapons)
        {
            if (weapon.firePoint == null)
                continue;

            

            if (weapon.cooldownTimer <= 0f)
            {               
                FireWeapon(weapon);
                ResetCooldown(weapon);
                DebugWeaponState(weapon);
            }

        }
    }

    private void FireWeapon(WeaponInstance weapon)
    {
        weapon.definition.shootingBehaviour?.Shoot(
            weapon,
            weapon.firePoint
        );
    }

    private void ResetCooldown(WeaponInstance weapon)
    {
        float fireRate = weapon.currentStats.fireRate;
        weapon.cooldownTimer = fireRate > 0f ? 1f / fireRate : 1f;
    }

    private void DebugWeaponState(WeaponInstance weapon)
    {
        Debug.Log(
            $"[WEAPON FIRED] {weapon.definition.weaponName} | " +
            $"DMG: {weapon.currentStats.damage} | " +
            $"FR: {weapon.currentStats.fireRate} | " +
            $"CD RESET"
        );
    }
}
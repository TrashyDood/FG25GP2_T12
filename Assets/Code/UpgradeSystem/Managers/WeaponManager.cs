using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public WeaponLibrary weaponLibrary;

    public Player player;
    public PlayerBuild build => player.build;

    private List<WeaponInstance> activeWeapons => build.weapons;

    public string startingWeaponId = "Quill";

    private void Awake()
    {
        if (player == null)
            player = FindObjectOfType<Player>();
        //AddDefaultWeapon();
    }

    private void Update()
    {
        UpdateCooldowns();

        if (Input.GetMouseButtonDown(0))
        {
            TryFireWeapons();
        }
    }

    /*
    private void AddDefaultWeapon()
    {
        WeaponDefinition def = weaponLibrary.GetWeapon(startingWeaponId);
        if (def == null)
        {
            Debug.LogError($"Starting weapon not found: {startingWeaponId}");
            return;
        }

        BodyPart weaponGO = Instantiate(def.weaponPrefab, Vector3.zero, Quaternion.identity);

        WeaponInstance instance = new WeaponInstance
        {
            definition = def,
            level = 1,
            weaponObject = weaponGO,
            currentStats = new WeaponStats
            {
                damage = def.baseStats.damage,
                fireRate = def.baseStats.fireRate,
                range = def.baseStats.range
            }
        };

        playerBuild.weapons.Add(instance);
    }
    */

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
                continue;

            if (weapon.cooldownTimer <= 0f)
            {
                weapon.definition.shootingBehaviour.Shoot(weapon);
                weapon.cooldownTimer = weapon.currentStats.fireRate > 0 ? 1f / weapon.currentStats.fireRate : 1f;
            }
        }
    }
}

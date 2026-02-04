using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


// Represents runtime build
// This includes:
// - Player stats
// - Owned weapons and their levels
public class PlayerBuild
{
    // Collection of thats and modifiers
    public StatCollection stats;

    // Weapons currently owned by the player
    public List<WeaponInstance> weapons;

    public PlayerBuild() 
    {
        stats = new StatCollection();
        weapons = new List<WeaponInstance>();
    }

    // Applies all effects contained in an upgrade definition
    // Param being the upgrade that was selected
    public void ApplyUpgrade(UpgradeDefinition upgrade)
    {
        foreach (var effect in upgrade.effects) effect.Apply(this);
    }

    // Adds a weapon to the players build
    // If the weapon already exists, level up weapon instead
    public void AddWeapon(WeaponDefinition def)
    {
        // Check if player already has the weapon
        WeaponInstance existing = weapons.Find(w => w.definition == def);
        if (existing != null)
        {
            existing.level++; // increase level if duplicate
            return;
        }

        // Add new weapon
        weapons.Add(new WeaponInstance
        {
            definition = def,
            level = 1,
            currentStats = new WeaponStats
            {
                damage = def.baseStats.damage,
                fireRate = def.baseStats.fireRate,
                range = def.baseStats.range
            }
        });
    }

    public void AddWeaponById(string weaponId, WeaponLibrary library, Transform playerTransform)
    {
        WeaponDefinition def = library.GetWeapon(weaponId);

        if (def == null)
            return;

        AddWeapon(def, playerTransform);
    }

    public void AddWeapon(WeaponDefinition def, Transform playerTransform)
    {
        WeaponInstance existing = weapons.Find(w => w.definition == def);
        if (existing != null)
        {
            existing.level++;
            return;
        }

        BodyPart weaponGO = Object.Instantiate(def.weaponPrefab, playerTransform);
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

        weapons.Add(instance);
    }
}

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
        stats.SetBaseStat(StatType.Speed, 10f);
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
        /*
        // Check if player already has the weapon
        WeaponInstance existing = weapons.Find(w => w.definition == def);
        if (existing != null)
        {
            existing.level++; // increase level if duplicate
            return;
        }
        */

        if (def == null)
        {
            Debug.LogError("PlayerBuild.AddWeapon called with null WeaponDef");
        }

        // Add new weapon (logical only, no prefab)
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
            // weaponObject stays null until placed in EditMode
        });
    }

    public void AddWeaponById(string weaponId, WeaponLibrary library)
    {
        WeaponDefinition def = library.GetWeapon(weaponId);

        if (def == null)
            return;

        AddWeapon(def);
    }


}

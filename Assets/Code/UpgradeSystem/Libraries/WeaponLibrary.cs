using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Weapons/Weapon Library")]
public class WeaponLibrary : ScriptableObject
{
    public List<WeaponDefinition> weapons;

    private Dictionary<string, WeaponDefinition> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, WeaponDefinition>();

        foreach (WeaponDefinition weapon in weapons)
        {
            if (weapon == null || string.IsNullOrEmpty(weapon.weaponId))
                continue;

            if (lookup.ContainsKey(weapon.weaponId))
            {
                Debug.LogError(
                    $"Duplicate weaponId: {weapon.weaponId}"
                );
                continue;
            }

            lookup.Add(weapon.weaponId, weapon);
        }
    }

    public WeaponDefinition GetWeapon(string id)
    {
        if (lookup.TryGetValue(id, out var weapon))
            return weapon;

        Debug.LogError($"Weapon not found: {id}");
        return null;
    }
}
using UnityEngine;
using UnityEngine.Serialization;

// ScriptableObject describing a weapon template
[CreateAssetMenu(fileName = "WeaponDefinition", menuName = "Weapons/Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    public string weaponName;
    public string weaponId;
    public Sprite icon;

    public WeaponStats baseStats;

    public BodyPart weaponPrefab;
    //public GameObject projectilePrefab;

    public ShootingBehaviour shootingBehaviour;
}
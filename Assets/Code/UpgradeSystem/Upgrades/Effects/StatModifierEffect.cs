using UnityEngine;

// Upgrade effect that modifies one of the players stats
[CreateAssetMenu(
    fileName = "StatModifierEffect",
    menuName = "Upgrades/Effects/Stat Modifier"
)]
public class StatModifierEffect : UpgradeEffect
{
    // which stat it modifies (Damage, speed, etc)
    public StatType stat;

    // Amount the stat is modified
    public float amount;

    // Type of modifier (flat or percentage)
    public ModifierType modifierType;

    // Applies this stat to the players build
    public override void Apply(PlayerBuild build)
    {
        build.stats.AddModifier(stat, amount, modifierType);
    }
}

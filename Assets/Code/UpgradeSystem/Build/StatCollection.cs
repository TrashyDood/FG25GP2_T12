using System.Collections.Generic;

[System.Serializable]
public enum StatType
{
    Damage,
    FireRate,
    Health,
    Speed
}

[System.Serializable]
public enum ModifierType
{
    Additive,
    Multiplicative
}


// Manages player stats and their modifiers
// Stats are calculated as ((base + additive) * multiplicative)
[System.Serializable]
public class StatCollection
{
    private Dictionary<StatType, float> baseStats = new();
    private Dictionary<StatType, float> additive = new();
    private Dictionary<StatType, float> multiplicative = new();

    public void SetBaseStat(StatType stat, float value)
    {
        baseStats[stat] = value;
    }

    public void AddModifier(StatType stat, float amount, ModifierType type)
    {
        if (type == ModifierType.Additive)
        {
            additive[stat] = additive.TryGetValue(stat, out var v) ? v + amount : amount;
        }
        else // multiplicative
        {
            multiplicative[stat] = multiplicative.TryGetValue(stat, out var v) ? v * amount : amount;
        }
    }

    public float GetStat(StatType stat)
    {
        float baseValue = baseStats.TryGetValue(stat, out var b) ? b : 0f;
        float add = additive.TryGetValue(stat, out var a) ? a : 0f;
        float mult = multiplicative.TryGetValue(stat, out var m) ? m : 1f;
        return (baseValue + add) * mult;
    }
}

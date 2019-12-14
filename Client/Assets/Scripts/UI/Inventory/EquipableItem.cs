using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Coat,
    Pants,
    Neck,
    Belt,
    Shoes,
    Bracelet,
    Necklace,
    Ring,
    Support,
    MagicStone,
    EarRing,
}

[CreateAssetMenu]
public class EquipableItem : Item
{
    public int strengthBonus;
    public int agilityBonus;
    public int intelligenceBonus;
    public int vitalityBonus;
    [Space]
    public float strengthPercentBonus;
    public float agilityPercentBonus;
    public float intelligencePercentBonus;
    public float vitalityPercentBonus;
    [Space]
    public EquipmentType equipmentType;
}

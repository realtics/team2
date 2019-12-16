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
    public int physicalAttackBonus;
    public int magicAttackBonus;
    [Space]
    public int physicalDefenseBonus;
    public int magicDefenseBonus;
    [Space]
    public int strengthBonus;
    public int intelligenceBonus;
    public int healthBonus;
    public int mentalityBonus;
    public int hangmaBonus;
    [Space]
    public float strengthPercentBonus;
    public float intelligencePercentBonus;
    public float healthPercentBonus;
    public float mentalityPercentBonus;
    [Space]
    public string Information;
    public EquipmentType equipmentType;
}

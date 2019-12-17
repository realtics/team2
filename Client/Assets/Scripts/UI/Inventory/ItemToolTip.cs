using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] Text _itemNameText;
    [SerializeField] Text _itemSlotText;
    [SerializeField] Text _itemStatsText;
    [SerializeField] Text _itemInfoText;
	[SerializeField] Text _equipUequipText;
	[SerializeField] GameObject _BlockRayCast;
	[SerializeField] Image _ItemImage;

	[SerializeField]
	private ItemSlot _itemSlot; 
    private StringBuilder _sb = new StringBuilder();

    public void ShowToolTip(EquipableItem item, ItemSlot itemSlot)
    {
		_BlockRayCast.SetActive(true);

		_itemSlot = itemSlot;
		if (_itemSlot is EquipmentSlot)
			_equipUequipText.text = "해제";
		else
			_equipUequipText.text = "장착";

		_ItemImage.sprite = _itemSlot.Item.icon;

		_itemNameText.text = item.itemName;
        _itemSlotText.text = item.equipmentType.ToString();
        _itemInfoText.text = item.Information;

        _sb.Length = 0;
        AddTopStat(item);
        AddMainStat(item);
        //AddPercentStat(item);
        _itemStatsText.text = _sb.ToString();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
		_BlockRayCast.SetActive(false);
		gameObject.SetActive(false);
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if (value != 0)
        {
            if (_sb.Length > 0)
                _sb.AppendLine();

            _sb.Append(statName);

            if (value > 0)
                _sb.Append(" +");

            if (isPercent)
            {
                _sb.Append(value * 100);
                _sb.Append("%");
            }
            else
            {
                _sb.Append(value);
                _sb.Append(" ");
            }
        }
    }
    private void AddTopStat(EquipableItem item)
    {
        AddStat(item.physicalAttackBonus, "물리 공격력");
        AddStat(item.magicAttackBonus, "마법 공격력");
		AddStat(item.physicalDefenseBonus, "물리 방어력");
		AddStat(item.magicDefenseBonus, "마법 방어력");

		if (_sb.Length > 0)
            _sb.AppendLine();
    }

    private void AddMainStat(EquipableItem item)
    {
        AddStat(item.strengthBonus, "힘 ");
        AddStat(item.intelligenceBonus, "지능");
        AddStat(item.healthBonus, "체력");
        AddStat(item.mentalityBonus, "정신력");
        AddStat(item.hangmaBonus, "항마력");

        if (_sb.Length > 0)
            _sb.AppendLine();
    }

    private void AddPercentStat(EquipableItem item)
    {
        AddStat(item.strengthPercentBonus, "힘", isPercent: true);
        AddStat(item.intelligencePercentBonus, "지능", isPercent: true);
        AddStat(item.healthPercentBonus, "체력", isPercent: true);
        AddStat(item.mentalityPercentBonus, "정신력", isPercent: true);

        if (_sb.Length > 0)
            _sb.AppendLine();
    }

	public void ClickButton()
	{
		_itemSlot.ClickEvent();
		HideToolTip();
	}
}

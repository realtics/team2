using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] Text _itemNameText;
    [SerializeField] Text _itemSlotText;
    [SerializeField] Text _itemStatsText;

    private StringBuilder _sb = new StringBuilder();

    public void ShowToolTip(EquipableItem item)
    {
        _itemNameText.text = item.itemName;
        _itemSlotText.text = item.equipmentType.ToString();

        _sb.Length = 0;
        AddAttackStat(item);
        AddMainStat(item);
        //AddPercentStat(item);
        AddInformation(item.Information);

        _itemStatsText.text = _sb.ToString();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
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
    private void AddAttackStat(EquipableItem item)
    {
        AddStat(item.physicalAttackBonus, "물리 공격력");
        AddStat(item.magicAttackBonus, "마법 공격력");

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

    private void AddInformation(string information)
    {
        if(information != null)
        {
            if (_sb.Length > 0)
                _sb.AppendLine();

            _sb.Append(information);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ChracterStatInfo : MonoBehaviour
{
	[SerializeField]
	private EquipmentPanel _equipmentPanel;

	[SerializeField] private Text _physicalAttackBonusText;
	[SerializeField] private Text _magicAttackBonusText;
	[SerializeField] private Text _physicalDefenseBonusText;
	[SerializeField] private Text _magicDefenseBonusText;
	[SerializeField] private Text _strengthBonusText;
	[SerializeField] private Text _intelligenceBonusText;
	[SerializeField] private Text _healthBonusText;
	[SerializeField] private Text _mentalityBonusText;
	[SerializeField] private Text _hangmaBonusText;

	//private CharacterStat _stat;

	public void PopUpStatInfo()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}

	private void OnValidate()
	{
		SetCharacterInfo();
	}

	private void UpdateStat()
	{
		
        CharacterBaseStat _totalEquipStat = new CharacterBaseStat();

		for (int i = 0; i < _equipmentPanel.EquipmentSlots.Length; i++)
		{
			if (_equipmentPanel.EquipmentSlots[i].Item != null)
			{
				EquipableItem equipableItem = (EquipableItem)_equipmentPanel.EquipmentSlots[i].Item;
                _totalEquipStat.physicalAttack += equipableItem.physicalAttackBonus;
                _totalEquipStat.magicAttack += equipableItem.magicAttackBonus;
                _totalEquipStat.physicalDefense += equipableItem.physicalDefenseBonus;
                _totalEquipStat.magicDefense += equipableItem.magicDefenseBonus;
                _totalEquipStat.strength += equipableItem.strengthBonus;
                _totalEquipStat.intelligence += equipableItem.intelligenceBonus;
                _totalEquipStat.health += equipableItem.healthBonus;
                _totalEquipStat.mentality += equipableItem.intelligenceBonus;
                _totalEquipStat.hangma += equipableItem.hangmaBonus;
			}
		}
        PlayerManager.Instance.SetEquipmentStat(_totalEquipStat);
  
        _physicalAttackBonusText.text = PlayerManager.Instance.Stat.TotalStat.physicalAttack.ToString();
        _magicAttackBonusText.text = PlayerManager.Instance.Stat.TotalStat.magicAttack.ToString();
		_physicalDefenseBonusText.text = PlayerManager.Instance.Stat.TotalStat.physicalDefense.ToString();
		_magicDefenseBonusText.text = PlayerManager.Instance.Stat.TotalStat.magicDefense.ToString();
		_strengthBonusText.text = PlayerManager.Instance.Stat.TotalStat.strength.ToString();
		_intelligenceBonusText.text = PlayerManager.Instance.Stat.TotalStat.intelligence.ToString();
		_healthBonusText.text = PlayerManager.Instance.Stat.TotalStat.health.ToString();
		_mentalityBonusText.text = PlayerManager.Instance.Stat.TotalStat.mentality.ToString();
		_hangmaBonusText.text = PlayerManager.Instance.Stat.TotalStat.hangma.ToString();

    }

	public void SetCharacterInfo()
	{
		UpdateStat();
	}
}

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
		//FIXME : 스탯연동할때 지역변수 다 제거 일단은 확인용
		int physicalAttackBonus = 0;
		int magicAttackBonus = 0;
		int physicalDefenseBonus = 0;
		int magicDefenseBonus = 0;
		int strengthBonus = 0;
		int intelligenceBonus = 0;
		int healthBonus = 0;
		int mentalityBonus = 0;
		int hangmaBonus = 0;

		for (int i = 0; i < _equipmentPanel.EquipmentSlots.Length; i++)
		{
			if (_equipmentPanel.EquipmentSlots[i].Item != null)
			{
				EquipableItem equipableItem = (EquipableItem)_equipmentPanel.EquipmentSlots[i].Item;
				physicalAttackBonus += equipableItem.physicalAttackBonus;
				magicAttackBonus += equipableItem.magicAttackBonus;
				physicalDefenseBonus += equipableItem.physicalDefenseBonus;
				magicDefenseBonus += equipableItem.magicDefenseBonus;
				strengthBonus += equipableItem.strengthBonus;
				intelligenceBonus += equipableItem.intelligenceBonus;
				healthBonus += equipableItem.healthBonus;
				mentalityBonus += equipableItem.intelligenceBonus;
				hangmaBonus += equipableItem.hangmaBonus;
			}
		}
		_physicalAttackBonusText.text = physicalAttackBonus.ToString();
		_magicAttackBonusText.text = magicAttackBonus.ToString();
		_physicalDefenseBonusText.text = physicalDefenseBonus.ToString();
		_magicDefenseBonusText.text = magicDefenseBonus.ToString();
		_strengthBonusText.text = strengthBonus.ToString();
		_intelligenceBonusText.text = intelligenceBonus.ToString();
		_healthBonusText.text = healthBonus.ToString();
		_mentalityBonusText.text = mentalityBonus.ToString();
		_hangmaBonusText.text = hangmaBonus.ToString();

		//_stat.physicalAttackBonus = physicalAttackBonus;
		//_stat.magicAttackBonus = magicAttackBonus;
		//_stat.physicalDefenseBonus = physicalDefenseBonus;
		//_stat.magicDefenseBonus = magicDefenseBonus;
		//_stat.strengthBonus = strengthBonus;
		//_stat.intelligenceBonus = intelligenceBonus;
		//_stat.healthBonus = healthBonus;
		//_stat.mentalityBonus = mentalityBonus;
		//_stat.hangmaBonus = hangmaBonus;
	}

	public void SetCharacterInfo()
	{
		UpdateStat();
	}
}

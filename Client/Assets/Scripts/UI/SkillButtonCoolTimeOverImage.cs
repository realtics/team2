using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonCoolTimeOverImage : MonoBehaviour
{
	[SerializeField]
	private SwordmanSkillIndex _skillIndex;
	[SerializeField]
	private Image _overImage;
	[SerializeField]
	private Text _coolTimeText;
	[SerializeField]
	private CharacterSkill _skill;

	private void Start()
	{
		if (PlayerManager.Instance.PlayerCharacter != null)
			_skill = PlayerManager.Instance.PlayerCharacter.Movement.GetEquipSkill(_skillIndex);
	}

	private void Update()
	{
		if (_skill == null)
			_skill = PlayerManager.Instance.PlayerCharacter.Movement.GetEquipSkill(_skillIndex);

		_overImage.fillAmount = _skill.CurrentCoolTime / _skill.InitCoolTime;

		if (_skill.CurrentCoolTime > 0.0f)
			_coolTimeText.text = ((int)_skill.CurrentCoolTime+1.0f).ToString();
		else
			_coolTimeText.text = "";
	}
}

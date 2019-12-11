using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CharacterSkill", menuName = "Skill")]
public class CharacterSkillMetaObject : ScriptableObject
{
	public AttackInfoSender AttackInfo;
	public GameObject skillPrefab;
	public int motion;
	public float coolTime;
}

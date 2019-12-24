using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordmanSkillIndex
{
    None = 0,
    Jingongcham,
    Hadouken,
    Blache,
    ClearCircle,
    CutIn,
}

public class SwordmanSkillManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterSkillMetaObject> _skillMetas;

    private static SwordmanSkillManager _instance;
    public static SwordmanSkillManager Instance
    {
        get
        {
            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;
    }

	private void Start()
	{
		//ObjectPoolManager.Instance.CreatePool(FindSkillEffect(SwordmanSkillIndex.Jingongcham));
	}

    private void Update()
    {
        
    }

    public CharacterSkill GetSkill(CharacterStat stat, SwordmanSkillIndex index)
    {
        CharacterSkill skill = null;
		CharacterSkillMetaObject meta = FindSkillMeta(index);
		AttackInfoSender sender = meta.AttackInfo;
		sender.Attacker = stat.transform;

		switch (index)
        {
            case SwordmanSkillIndex.Jingongcham:
                SwordmanSkillJingongcham jingongcham = new SwordmanSkillJingongcham();
				sender.Damage = stat.TotalStat.strength * sender.Damage / 10.0f;
				skill = jingongcham;
                break;
            case SwordmanSkillIndex.Hadouken:
                SwordmanSkillHadouken hadouken = new SwordmanSkillHadouken();
				sender.Damage = stat.TotalStat.intelligence * sender.Damage / 10.0f;
				skill = hadouken;
                break;
            case SwordmanSkillIndex.Blache:
                SwordmanSkillBlache blache = new SwordmanSkillBlache();
                skill = blache;
                break;
        }

		skill.SetCreateEffect(stat, sender, meta.skillPrefab, meta.coolTime, meta.motion);
		ObjectPoolManager.Instance.CreatePool(meta.skillPrefab, 1);

        return skill;
    }

	public CharacterSkillMetaObject FindSkillMeta(SwordmanSkillIndex inedx)
	{
		CharacterSkillMetaObject skill = null;

		foreach (CharacterSkillMetaObject meta in _skillMetas)
		{
			if (meta.index != inedx)
				continue;

			skill = meta;
		}

		return skill;
	}
}

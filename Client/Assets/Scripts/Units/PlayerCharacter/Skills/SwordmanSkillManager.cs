using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordmanSkillType
{
    None = 0,
    Jingongcham = 1,
}

[Serializable]
public struct SwordmanSkillBody
{
    public SwordmanSkillType type;
    public GameObject effect;
}

public class SwordmanSkillManager : MonoBehaviour
{
    [SerializeField]
    private List<SwordmanSkillBody> _skillBodys;

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
        ObjectPoolManager.Instance.CreatePool(ObjectPoolType.Jingongcham, FindSkillEffect(SwordmanSkillType.Jingongcham));
    }

    private void Update()
    {
        
    }

    public CharacterSkill GetSkill(CharacterStat stat, SwordmanSkillType type)
    {
        CharacterSkill skill = null;

        switch (type)
        {
            case SwordmanSkillType.Jingongcham:
                SwordmanSkillJingongcham jingongcham = new SwordmanSkillJingongcham();
                jingongcham.SetCreateEffect(stat, FindSkillEffect(type), 10.0f, 2);
                skill = jingongcham;
                break;
        }

        return skill;
    }

    private GameObject FindSkillEffect(SwordmanSkillType type)
    {
        GameObject effect = null;

        foreach (SwordmanSkillBody body in _skillBodys)
        {
            if (body.type != type)
                continue;

            effect = body.effect;
        }

        return effect;
    }
}

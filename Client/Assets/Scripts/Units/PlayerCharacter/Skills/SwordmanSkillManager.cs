using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordmanSkillIndex
{
    None = 0,
    Jingongcham = 1,
}

[Serializable]
public struct SwordmanSkillBody
{
    public SwordmanSkillIndex type;
    public GameObject effectPrefab;
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
        //ObjectPoolManager.Instance.CreatePool(FindSkillEffect(SwordmanSkillIndex.Jingongcham));
    }

    private void Update()
    {
        
    }

    public CharacterSkill GetSkill(CharacterStat stat, SwordmanSkillIndex type)
    {
        CharacterSkill skill = null;

        switch (type)
        {
            case SwordmanSkillIndex.Jingongcham:
                SwordmanSkillJingongcham jingongcham = new SwordmanSkillJingongcham();
                jingongcham.SetCreateEffect(stat, FindSkillEffect(type), 10.0f, 1);
                skill = jingongcham;
                break;
        }

        return skill;
    }

    public GameObject FindSkillEffect(SwordmanSkillIndex type)
    {
        GameObject effect = null;

        foreach (SwordmanSkillBody body in _skillBodys)
        {
            if (body.type != type)
                continue;

            effect = body.effectPrefab;
        }

        return effect;
    }
}

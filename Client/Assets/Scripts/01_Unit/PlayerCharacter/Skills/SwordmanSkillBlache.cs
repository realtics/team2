using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanSkillBlache : CharacterSkill
{
    public override bool OnSkill()
    {
        if (!base.OnSkill())
            return false;

        _instanceEffect = ObjectPoolManager.Instance.GetRestObject(_effectPrefab);
        _instanceEffect.transform.position = _sender.Attacker.position;
        SetSpriteFlipForward();

        Transform canvas = GameObject.Find("CutinParent").transform;
        GameObject cutin = ObjectPoolManager.Instance.GetRestObject(SwordmanSkillManager.Instance.FindSkillEffect(SwordmanSkillIndex.CutIn), canvas);
        cutin.transform.localScale = Vector3.one;
        cutin.transform.localPosition = Vector3.zero;
        return true;
    }

    protected override void SetAttackInfo(CharacterStat stat)
    {
        base.SetAttackInfo(stat);
    }
}

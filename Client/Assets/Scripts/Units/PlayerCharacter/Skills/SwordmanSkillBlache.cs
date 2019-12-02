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

        return true;
    }

    protected override void SetAttackInfo(CharacterStat stat)
    {
        base.SetAttackInfo(stat);
    }
}

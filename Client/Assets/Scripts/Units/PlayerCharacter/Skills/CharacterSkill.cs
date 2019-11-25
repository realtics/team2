using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill
{
    protected GameObject _effect;
    private int _motionIndex;
    protected CharacterStat _stat;
    protected AttackInfoSender _sender;

    public int MotionIndex { get { return _motionIndex; } }

    public virtual void OnSkill()
    {

    }

    public void SetCreateEffect(CharacterStat stat, GameObject effect, int motion = 1)
    {
        _stat = stat;
        _motionIndex = motion;
        _effect = effect;
        SetAttackInfo(_stat);
    }

    protected virtual void SetAttackInfo(CharacterStat stat)
    {
        _sender.Attacker = stat.transform;
    }
}

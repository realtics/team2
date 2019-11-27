using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill
{
    protected GameObject _effectPrefab;
    protected GameObject _instanceEffect;
    private int _motionIndex;
    protected CharacterStat _stat;
    protected AttackInfoSender _sender;
    protected float _coolTime;
    protected float _initCoolTime;

    public int MotionIndex { get { return _motionIndex; } }
    public bool UsableSkill { get { return _coolTime <= 0.0f; } }
    public float CurrentCoolTime { get { return _coolTime; } }

    public void UpdateCoolTime()
    {
        if (_coolTime <= 0.0f)
            return;

        _coolTime -= Time.deltaTime;
    }

    public virtual bool OnSkill()
    {
        if (!UsableSkill)
            return false;

        _coolTime = _initCoolTime;
        return true;
    }

    public void SetCreateEffect(CharacterStat stat, GameObject effect, float coolTime, int motion = 1)
    {
        _stat = stat;
        _motionIndex = motion;
        _effectPrefab = effect;
        _initCoolTime = coolTime;
        SetAttackInfo(_stat);
    }

    protected virtual void SetAttackInfo(CharacterStat stat)
    {
        _sender.Attacker = stat.transform;
    }

    protected void SetSpriteFlipForward()
    {
        Vector3 flipScale = _effectPrefab.transform.localScale;
        flipScale.x *= _stat.Unit.Forward;
        _instanceEffect.transform.localScale = flipScale;
    }
}

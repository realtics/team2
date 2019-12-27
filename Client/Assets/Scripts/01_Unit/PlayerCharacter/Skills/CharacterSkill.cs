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
	protected CharacterMovement _movement;
    protected AttackInfoSender _sender;
    protected float _coolTime;
    protected float _initCoolTime;

    public int MotionIndex { get { return _motionIndex; } }
    public bool UsableSkill { get { return _coolTime <= 0.0f; } }
    public float CurrentCoolTime { get { return _coolTime; } }
	public float InitCoolTime { get { return _initCoolTime; } }

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
		_movement = (CharacterMovement)_stat.Unit;
		_motionIndex = motion;
		_effectPrefab = effect;
		_initCoolTime = coolTime;
		SetAttackInfo(_stat);
	}

	public void SetCreateEffect(CharacterStat stat, AttackInfoSender sender, GameObject effectPrefab, float coolTime = 1.0f, int motion = 1)
	{
		_stat = stat;
		_movement = (CharacterMovement)_stat.Unit;
		_sender = sender;
		_motionIndex = motion;
		_effectPrefab = effectPrefab;
		_initCoolTime = coolTime;
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

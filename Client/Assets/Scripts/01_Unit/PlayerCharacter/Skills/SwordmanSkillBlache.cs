﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanSkillBlache : CharacterSkill
{
    private BlacheAttackBox _attackBox;
    public override bool OnSkill()
    {
        if (!base.OnSkill())
            return false;

        _instanceEffect = ObjectPoolManager.Instance.GetRestObject(_effectPrefab);
        _instanceEffect.transform.position = _sender.Attacker.position;
        SetSpriteFlipForward();
		EffectManager.Instance.ShowCutin();

        _attackBox = _instanceEffect.GetComponent<BlacheAttackBox>();
        _attackBox.SetAttackInfo(_sender);
        //GameObject canvas = GameObject.Find("CutinParent");
        //if (canvas != null)
        //{
        //	GameObject cutin = ObjectPoolManager.Instance.GetRestObject(SwordmanSkillManager.Instance.FindSkillEffect(SwordmanSkillIndex.CutIn), canvas.transform);
        //	cutin.transform.localScale = Vector3.one;
        //	cutin.transform.localPosition = Vector3.zero;
        //}
        return true;
    }

    protected override void SetAttackInfo(CharacterStat stat)
    {
        base.SetAttackInfo(stat);
    }
}

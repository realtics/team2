using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanSkillHadouken : CharacterSkill
{
    private HadoukenUpperAttackBox _attackBox;

    public override bool OnSkill()
    {
        if (!base.OnSkill())
            return false;

        _instanceEffect = ObjectPoolManager.Instance.GetRestObject(_effectPrefab);
        _attackBox = _instanceEffect.GetComponent<HadoukenUpperAttackBox>();

        Vector3 pos = _effectPrefab.transform.position;
        pos.x *= _stat.Unit.Forward;

        _instanceEffect.transform.position = _sender.Attacker.transform.position + pos;

        SetSpriteFlipForward();

        _attackBox.InfoSender = _sender;
        _attackBox.Unit = _stat.Unit;

        return true;
    }

    protected override void SetAttackInfo(CharacterStat stat)
    {
        base.SetAttackInfo(stat);
        _sender.Damage = stat.AttackDamage * 1.0f;
        _sender.HorizontalExtraMoveDuration = 0.1f;
        _sender.HorizontalExtraMoveValue = -1.0f;
        _sender.ExtraHeightValue = 0.2f;
        _sender.StunDuration = 1.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanSkillJingongcham : CharacterSkill
{
    private JingongchamAttackBox _attackBox;

    public override void OnSkill()
    {
        GameObject effect = ObjectPoolManager.Instance.GetRestObject(ObjectPoolType.Jingongcham);
        _attackBox = effect.GetComponent<JingongchamAttackBox>();
        Vector3 effectPos = _sender.Attacker.transform.position;
        effectPos.y += 2.0f;
        effect.transform.position = effectPos;

        Vector3 flipScale = effect.transform.localScale;

        if (_stat.Unit.Forward > 0.0f)
        {
            if (effect.transform.localScale.x < 0.0f)
                flipScale.x = -flipScale.x;

        }
        else
        {
            if (effect.transform.localScale.x > 0.0f)
                flipScale.x = -flipScale.x;
        }

        effect.transform.localScale = flipScale;

        _attackBox.InfoSender = _sender;
    }

    protected override void SetAttackInfo(CharacterStat stat)
    {
        base.SetAttackInfo(stat);
        _sender.Damage = stat.AttackDamage * 2.0f;
        _sender.HorizontalExtraMoveDuration = 0.1f;
        _sender.HorizontalExtraMoveValue = -10.0f;
        _sender.StunDuration = 1.0f;
    }
}

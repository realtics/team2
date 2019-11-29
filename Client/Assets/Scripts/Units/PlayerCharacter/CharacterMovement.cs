﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : BaseUnit
{
    private bool _nextAttack;
    private CharacterAnimController _animController;
    private Dictionary<SwordmanSkillIndex, CharacterSkill> _equiredSkills;
    private SwordmanSkillIndex _usedSkill;

    public CharacterSkill UsedSkill { get { return _equiredSkills[_usedSkill]; } }

    protected override void Start()
    {
        base.Start();
        _nextAttack = false;
        _animator.SetBool("IsGround", true);
        _animator.SetBool("NextAttack", false);
        _animController = GetComponentInChildren<CharacterAnimController>();

        _equiredSkills = new Dictionary<SwordmanSkillIndex, CharacterSkill>();
        _equiredSkills.Add(SwordmanSkillIndex.Jingongcham, SwordmanSkillManager.Instance.GetSkill(_stat, SwordmanSkillIndex.Jingongcham));
        _equiredSkills.Add(SwordmanSkillIndex.Hadouken, SwordmanSkillManager.Instance.GetSkill(_stat, SwordmanSkillIndex.Hadouken));
    }

    protected override void Update()
    {
        CheckAttackEnd();
        SkillCoolTimeUpdate();
    }

    private void SkillCoolTimeUpdate()
    {
        foreach (CharacterSkill skill in _equiredSkills.Values)
        {
            skill.UpdateCoolTime();
        }
    }

    private void CheckAttackEnd()
    {
        //if (!IsAttack)
        //    return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!animStateInfo.IsTag("Attack"))
            return;

        if (IsAnimationPlaying())
            return;

        StopAttack();
    }

    public override void StopAttack()
    {
        _animController.OffAttackBox();

        if (IsInTranstion)
            return;

        base.StopAttack();
        StopSkill();

        _animator.SetBool("IsAttack", false);
        _animator.SetBool("NextAttack", false);
    }

    public override bool SetAttack()
    {
        if (!base.SetAttack())
            return false;

        _animator.SetBool("IsAttack", true);
        SetNextAttack();

        return true;
    }

    public void SetNextAttack()
    {
        if (!IsAttack)
            return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (!animStateInfo.IsTag("Attack"))
            return;

        if (animStateInfo.IsName("Attack3"))
            return;

        if (CurAnimTime < 0.5f)
            return;

        if (CurAnimTime >= 0.95f)
            return;

        if (IsInTranstion)
            return;

        MoveUnit(1.0f, 0.3f, ExtraMoveDirection.Horizontal);

        _animator.SetBool("NextAttack", true);
        StartCoroutine(CoSetFalseNextAttack());
		_animController.OffAttackBox();
	}

    public override bool SetJump()
    {
        if (!base.SetJump())
            return false;

        _animator.SetBool("IsJumpUp", true);
        _animator.SetBool("IsGround", false);

        return true;
    }

    public override bool SetAirHitHeight(float power)
    {
        if (!base.SetAirHitHeight(power))
            return false;

        _animator.SetBool("IsGround", false);
        _animator.SetBool("IsAirHit", true);
        return true;
    }

    protected override bool SetJumpDown()
    {
        if (!base.SetJumpDown())
            return false;

        _animator.SetBool("IsJumpUp", false);
        _animator.SetBool("IsJumpDown", true);

        return true;
    }

    public override bool SetRun()
    {
        if (!base.SetRun())
            return false;

        _animator.SetBool("IsRun", true);

        return true;
    }


    public override bool StopRun()
    {
        if (!base.StopRun())
            return false;

        _animator.SetBool("IsRun", false);

        return true;
    }

    protected override bool CheckGroundAfterJump()
    {
        if (!base.CheckGroundAfterJump())
            return false;

        _animator.SetBool("IsJumpDown", false);
        _animator.SetBool("IsGround", true);

        return true;
    }

    public override bool SetAxis(float horizontal, float vertical)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
            return false;

        if (!base.SetAxis(horizontal, vertical))
            return false;

        _animator.SetBool("IsMoving", IsMoving);

        return true;
    }

    public override bool OnHit(AttackInfoSender sender)
    {
        if (!base.OnHit(sender))
            return false;

        _animator.SetBool("IsHit", true);

        if (sender.Attacker.position.x > transform.position.x)
            SetFlipX(false);
        else
            SetFlipX(true);

        if (sender.HorizontalExtraMoveDuration > 0.0f)
            MoveUnit(sender.HorizontalExtraMoveValue, sender.HorizontalExtraMoveDuration, ExtraMoveDirection.Horizontal);

        if (sender.VerticalExtraMoveDuration > 0.0f)
            MoveUnit(sender.VerticalExtraMoveValue, sender.VerticalExtraMoveDuration, ExtraMoveDirection.Vertical);

        if (sender.ExtraHeightValue > 0.0f)
            SetAirHitHeight(sender.ExtraHeightValue);

        return true;
    }

    public override void SetHit(float stunDuration)
    {
        base.SetHit(stunDuration);
        SetRecoveryTime(stunDuration);
    }

    public override void StopHit()
    {
        base.StopHit();
        _animator.SetBool("IsHit", false);
        _animator.SetBool("IsAirHit", false);
        _animator.SetBool("IsAttack", false);
    }

    public override bool SetSkill(SwordmanSkillIndex skill)
    {
        _usedSkill = skill;

        if (!UsedSkill.UsableSkill)
        {
            Debug.Log("현재 쿨타임입니다. : " + UsedSkill.CurrentCoolTime + "초 남았습니다.");

            _usedSkill = SwordmanSkillIndex.None;
            return false;
        }

        if (!base.SetSkill(skill))
            return false;

        if (!CheckIsAttack())
            return false;

        _animator.SetBool("OnSkill", true);
        _animator.SetInteger("SkillMotion", UsedSkill.MotionIndex);

        return true;
    }

    public override void StopSkill()
    {
        base.StopSkill();
        _animator.SetBool("OnSkill", false);
    }

    IEnumerator CoSetFalseNextAttack()
    {
        yield return new WaitForEndOfFrame();
        _animator.SetBool("NextAttack", false);
    }

    public override void OnSkill()
    {
        UsedSkill.OnSkill();
    }

    private bool CheckIsAttack()
    {
        if (!IsAttack)
            return true;

        //if (CurAnimTime < 0.2f)
        //    return false;

        if (CurAnimTime >= 0.95f)
            return false;

        StopAttack();

        return true;
    }
}

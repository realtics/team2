using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : BaseUnit
{
    private bool _nextAttack;
    private CharacterAnimController _animController;

    protected override void Start()
    {
        base.Start();
        _nextAttack = false;
        _animator.SetBool("IsGround", true);
        _animator.SetBool("NextAttack", false);
        _animController = GetComponentInChildren<CharacterAnimController>();
    }

    override protected void Update()
    {
        CheckAttackEnd();
    }

    private void CheckAttackEnd()
    {
        if (!IsAttack)
            return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!animStateInfo.IsTag("Attack"))
            return;

        if (IsAnimationPlaying())
            return;

        StopAttack();
    }

    public override void StopAttack()
    {
        if (IsInTranstion)
            return;

        base.StopAttack();
        _animator.SetBool("IsAttack", false);
        _animator.SetBool("NextAttack", false);
        _animController.OffAttackBox();
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
        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (!animStateInfo.IsTag("Attack"))
            return;

        if (animStateInfo.IsName("Attack3"))
            return;

        if (CurAnimTime < 0.5f)
            return;

        if (IsInTranstion)
            return;

        MoveUnit(1.0f, 0.3f, ExtraMoveDirection.Horizontal);

        _animator.SetBool("NextAttack", true);
        StartCoroutine(CoSetFalseNextAttack());
    }

    public override bool SetJump()
    {
        if (!base.SetJump())
            return false;

        _animator.SetBool("IsJumpUp", true);
        _animator.SetBool("IsGround", false);

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
        if (!base.SetAxis(horizontal, vertical))
            return false;

        _animator.SetBool("IsMoving", IsMoving);

        return true;
    }

    public override void OnHit(float damage)
    {
        base.OnHit(damage);
        SetHit();
        _animator.SetBool("IsHit", true);
    }

    public override void OnHit(float damage, Transform attacker)
    {
        OnHit(damage);

        if (attacker.position.x > transform.position.x)
            SetFlipX(false);
        else
            SetFlipX(true);

        MoveUnit(-1.0f, 0.1f, ExtraMoveDirection.Horizontal);
    }

    public override void OnHit(AttackInfoSender sender)
    {
        base.OnHit(sender);
        _animator.SetBool("IsHit", true);

        if (sender.Attacker.position.x > transform.position.x)
            SetFlipX(false);
        else
            SetFlipX(true);

        if (sender.HorizontalExtraMoveDuration > 0.0f)
            MoveUnit(sender.HorizontalExtraMoveValue, sender.HorizontalExtraMoveDuration, ExtraMoveDirection.Horizontal);

        if (sender.VerticalExtraMoveDuration > 0.0f)
            MoveUnit(sender.VerticalExtraMoveValue, sender.VerticalExtraMoveDuration, ExtraMoveDirection.Vertical);
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
        _animator.SetBool("IsAttack", false);
    }

    IEnumerator CoSetFalseNextAttack()
    {
        yield return new WaitForEndOfFrame();
        _animator.SetBool("NextAttack", false);
    }
}

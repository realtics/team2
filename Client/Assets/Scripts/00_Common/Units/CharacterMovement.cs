using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : BaseUnit
{
    private int _maxAttackIndex;
    [SerializeField]
    private int _attackIndex;

    protected override void Start()
    {
        base.Start();

        _attackIndex = 0;
        _maxAttackIndex = 3;
        _animator.SetBool("IsGround", true);
    }

    override protected void Update()
    {
        CheckAttackEnd();
        CheckJumpAttackEnd();
    }

    private void CheckJumpAttackEnd()
    {
        if (IsGround)
            return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!animStateInfo.IsName("JumpAttack"))
            return;

        if (CurAnimTime < 0.95f)
            return;

        base.StopAttack();
        _animator.SetBool("IsAttack", false);
    }

    private void CheckAttackEnd()
    {
        if (!IsAttack)
            return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!animStateInfo.IsTag("Attack"))
            return;

        if (CurAnimTime < 0.95f)
            return;

        StopAttack();
    }

    public override void StopAttack()
    {
        base.StopAttack();
        _attackIndex = 0;
        _animator.SetBool("IsAttack", false);
        _animator.SetInteger("AttackIndex", _attackIndex);
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

        if (_attackIndex >= _maxAttackIndex - 1)
            return;

        if (CurAnimTime < 0.5f)
            return;

        if (_animator.IsInTransition(0))
            return;

        MoveUnit(1.0f, 0.3f, 1);
        _attackIndex++;
        _animator.SetInteger("AttackIndex", _attackIndex);
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

}

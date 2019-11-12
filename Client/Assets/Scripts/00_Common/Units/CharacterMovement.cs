using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : BaseUnit
{
    private bool _nextAttack;
    private int _maxAttackIndex;
    [SerializeField]
    private int _attackIndex;

    protected override void Start()
    {
        base.Start();

        _nextAttack = false;
        _attackIndex = 1;
        _maxAttackIndex = 3;
    }

    override protected void Update()
    {
        SetAnimParameter();
        CheckAttackEnd();
        CheckJumpAttackEnd();
    }

    private void CheckJumpAttackEnd()
    {
        if (IsGround)
            return;
        if (!IsAttack)
            return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!animStateInfo.IsName("JumpAttack"))
            return;

        if (animStateInfo.normalizedTime < 0.40f)
            return;

        _nextAttack = true;

        if (animStateInfo.normalizedTime < 0.95f)
            return;

        base.StopAttack();
    }

    private void CheckAttackEnd()
    {
        if (!IsAttack)
            return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!animStateInfo.IsName("Attack" + _attackIndex))
            return;

        if (animStateInfo.normalizedTime < 0.40f)
            return;

        _nextAttack = true;

        if (animStateInfo.normalizedTime < 0.95f)
            return;

        StopAttack();
    }

    public override void StopAttack()
    {
        base.StopAttack();
        _nextAttack = false;
        _attackIndex = 1;
    }

    public override void SetAttack()
    {
        base.SetAttack();

        if (!_nextAttack)
            return;

        if (_attackIndex < _maxAttackIndex)
            _attackIndex++;


        _nextAttack = false;
    }

    private void SetAnimParameter()
    {
        _animator.SetBool("IsGround", IsGround);
        _animator.SetBool("IsRun", IsRun);
        _animator.SetBool("IsAttack", IsAttack);
        _animator.SetBool("IsMoving", IsMoving);
        _animator.SetBool("IsJumpUp", IsJumpUp);
        _animator.SetBool("IsJumpDown", IsJumpDown);
        _animator.SetBool("IsHit", IsHit);
        _animator.SetInteger("AttackIndex", _attackIndex);
    }


}

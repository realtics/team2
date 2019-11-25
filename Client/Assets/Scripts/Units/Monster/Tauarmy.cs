using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tauarmy : BaseMonster
{
    private enum TauAttackMotion
    {
        AttackMotion1 = 0,
        AttackMotion2 = 1,
        AttacknMotionEnd = 2
    }
    private TauAttackMotion _currentAttackMotion;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //AttackState
    public override void EnterAttackState()
    {
        if (_target == null)
        {
            return;
        }

        IsAttack = true;

        FlipImage();
        _animator.SetBool("isAttacking", true);

        base.EnterAttackState();
    }

    public override void UpdateAttackState()
    {
        base.UpdateAttackState();
    }

    public override void ExitAttackState()
    {
        base.ExitAttackState();
    }

    //MoveState
    public override void EnterMoveState()
    {
        base.EnterMoveState();
    }

    public override void UpdateMoveState()
    {
        base.UpdateMoveState();
    }

    public override void ExitMoveState()
    {
        base.ExitMoveState();
    }

    //HitState
    public override void EnterHitState()
    {
        base.EnterHitState();
    }

    public override void UpdateHitState()
    {
        base.UpdateHitState();
    }

    public override void ExitHitState()
    {
        base.ExitHitState();
    }

    //DieState
    public override void EnterDieState()
    {
        base.EnterDieState();
    }

    public override void UpdateDieState()
    {
        base.UpdateDieState();
    }


    public override void ExitDieState()
    {
        base.ExitDieState();
    }
}

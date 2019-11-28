using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calvary : BaseMonster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //MonsterManager.Instance.ReceiveMonsterDie(this);
            ChangeDieState();
        }
    }

    //AttackState
    public override void EnterAttackState()
    {
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

    protected override void SetAerialValue(AttackInfoSender sender)
    {
        //FIXME : 컴포넌트화?
        //this monster don't have aerialvalue
        return;
    }

    public override void NoticeDie()
    {
        MonsterManager.Instance.ReceiveBossMonsterDie(this);
    }
}


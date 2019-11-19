using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState<Monster>
{
    static readonly AttackState instance = new AttackState();
    public static AttackState GetInstance
    {
        get
        {
            return instance;
        }
    }

    static AttackState() { }
    private AttackState() { }


    public override void EnterState(Monster monster)
    {
        if (monster.target == null)
        {
            return;
        }

        monster.FlipImage();
        monster.animator.SetBool("isAttacking", true);
    }

    public override void UpdateState(Monster monster)
    {
        if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            monster.animator.SetBool("isAttacking", false);
           
            monster.ChangeState(MoveState.GetInstance);
        }
    }

    public override void ExitState(Monster monster)
    {
        monster.animator.SetBool("isAttacking", false);
        monster.InactiveSmashHitBox();
    }
}

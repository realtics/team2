using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState<Monster>
{
    public override void EnterState(Monster monster)
    {
        monster.EnterAttackState();
    }

    public override void UpdateState(Monster monster)
    {
        monster.UpdateAttackState();
    }

    public override void ExitState(Monster monster)
    {
        monster.ExitAttackState();
    }
}

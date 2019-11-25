using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState<BaseMonster>
{
    public override void EnterState(BaseMonster monster)
    {
        monster.EnterAttackState();
    }

    public override void UpdateState(BaseMonster monster)
    {
        monster.UpdateAttackState();
    }

    public override void ExitState(BaseMonster monster)
    {
        monster.ExitAttackState();
    }
}

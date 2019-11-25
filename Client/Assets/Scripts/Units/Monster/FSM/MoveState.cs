using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState<BaseMonster>
{
   
    private const float InitialResetTime = 3.0f;

    public override void EnterState(BaseMonster monster)
    {
        monster.EnterMoveState();
    }

    public override void UpdateState(BaseMonster monster)
    {
        monster.UpdateMoveState();
    }

    public override void ExitState(BaseMonster monster)
    {
        monster.ExitMoveState();
    }
}

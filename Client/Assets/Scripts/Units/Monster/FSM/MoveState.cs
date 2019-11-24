using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState<Monster>
{
   
    private const float InitialResetTime = 3.0f;

    public override void EnterState(Monster monster)
    {
        monster.EnterMoveState();
    }

    public override void UpdateState(Monster monster)
    {
        monster.UpdateMoveState();
    }

    public override void ExitState(Monster monster)
    {
        monster.ExitMoveState();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : FSMState<BaseMonster>
{
    public override void EnterState(BaseMonster monster)
    {
        monster.EnterDieState();
    }

    public override void UpdateState(BaseMonster monster)
    {
        monster.UpdateDieState();
    }

    public override void ExitState(BaseMonster monster)
    {
        monster.ExitDieState();
    }
}

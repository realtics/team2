using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : FSMState<Monster>
{
    public override void EnterState(Monster monster)
    {
        monster.EnterDieState();
    }

    public override void UpdateState(Monster monster)
    {
        monster.UpdateDieState();
    }

    public override void ExitState(Monster monster)
    {
        monster.EnterDieState();
    }
}

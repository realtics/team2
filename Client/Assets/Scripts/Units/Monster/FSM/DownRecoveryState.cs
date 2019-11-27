using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownRecoveryState : FSMState<BaseMonster>
{
    public override void EnterState(BaseMonster monster)
    {
        monster.EnterDownRecoveryState();
    }

    public override void UpdateState(BaseMonster monster)
    {
        monster.UpdateDownRecoveryState();
    }

    public override void ExitState(BaseMonster monster)
    {
        monster.ExitDownRecoveryState();
    }
}

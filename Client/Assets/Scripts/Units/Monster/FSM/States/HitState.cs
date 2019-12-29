using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitState : FSMState<BaseMonster>
{
    public override void EnterState(BaseMonster monster)
    {
        monster.EnterHitState();
    }

    public override void UpdateState(BaseMonster monster)
    {
        monster.UpdateHitState();
    }

    public override void ExitState(BaseMonster monster)
    {
        monster.ExitHitState();
    }
}

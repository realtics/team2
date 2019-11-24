using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitState : FSMState<Monster>
{
    public override void EnterState(Monster monster)
    {
        monster.EnterHitState();
    }

    public override void UpdateState(Monster monster)
    {
        monster.UpdateHitState();
    }

    public override void ExitState(Monster monster)
    {
        monster.ExitHitState();
    }
}

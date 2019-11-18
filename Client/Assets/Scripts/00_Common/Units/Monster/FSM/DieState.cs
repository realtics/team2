using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : FSMState<Monster>
{
    static readonly DieState instance = new DieState();
    public static DieState GetInstance
    {
        get
        {
            return instance;
        }
    }

    static DieState() { }
    private DieState() { }

    //TODO: Monster.cs로 이전

    public override void EnterState(Monster monster)
    {
      
    }

    public override void UpdateState(Monster monster)
    {
       
    }

    public override void ExitState(Monster monster)
    {
       
    }
}

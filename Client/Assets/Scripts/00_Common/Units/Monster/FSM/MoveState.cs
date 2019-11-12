using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState<Monster>
{
    static readonly MoveState   instance = new MoveState();
    public static MoveState  GetInstance
    {
        get
        {
            return instance;
        }
    }

    private float _resetTime = 3f;
    private float _currentTime;

    static MoveState() { }
    private MoveState() { }

    public override void EnterState(Monster monster)
    {
        _currentTime = _resetTime;
    }

    public override void UpdateState(Monster monster)
    {
        //타겟 유무 확인
        if (monster.target != null)
        {
            if (!monster.CheckRange())
            {
                //추적시간을 초과하면 타겟을 잃음

            }


        }
    }

    public override void ExitState(Monster monster)
    {
        throw new System.NotImplementedException();
    }

    
}

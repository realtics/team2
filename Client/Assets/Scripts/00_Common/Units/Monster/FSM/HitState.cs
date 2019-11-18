using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitState : FSMState<Monster>
{
    static readonly HitState instance = new HitState();
    public static HitState GetInstance
    {
        get
        {
            return instance;
        }
    }

    static HitState() { }
    private HitState() { }

    //TODO: Monster.cs로 이전
    private float _hitRecoveryTime = 2.0f;
    private float _currentTime = 0.0f;


    public override void EnterState(Monster monster)
    {
        //Debug.Log("Enter HitState");

        monster.animator.SetBool("isHit", true);
    }

    public override void UpdateState(Monster monster)
    {
        if (IsHitRecoveryTimeEnd())
        {
            monster.animator.SetBool("isHit", false);
            monster.ChangeState(MoveState.GetInstance);
        }
      
        
        //TODO : 피격에서 죽는것 구현
    }

    public override void ExitState(Monster monster)
    {
        //Debug.Log("Exit AttacState");
        _currentTime = 0.0f;
    }

    private bool IsHitRecoveryTimeEnd()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _hitRecoveryTime)
        {
            return true;
        }
        return false;
    }
}

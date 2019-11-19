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
        monster.animator.SetBool("isHit", true);
    }

    public override void UpdateState(Monster monster)
    {
        if (IsHitRecoveryTimeEnd())
        {
            monster.animator.SetBool("isHit", false);
            monster.ChangeState(MoveState.GetInstance);
        }
    }

    public override void ExitState(Monster monster)
    {
        _currentTime = 0.0f;
        monster.animator.SetBool("isHit", false);
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

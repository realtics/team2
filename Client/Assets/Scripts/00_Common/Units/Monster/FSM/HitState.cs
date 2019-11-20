using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitState : FSMState<Monster>
{
    static readonly HitState instance = new HitState();
    public static HitState Instance
    {
        get
        {
            return instance;
        }
    }

    static HitState() { }
    private HitState() { }

    private enum HitMotion
    {
        HitMotion0 = 0,
        HitMotion1 = 1,
        HitMotionEnd = 2
    }

    //TODO: Monster.cs로 이전
    private float _hitRecoveryTime = 1.0f;
    private float _currentTime = 0.0f;

    private HitMotion _currentHitMotion;

    public override void EnterState(Monster monster)
    {
        _currentTime = 0.0f;
        monster.IsHit = true;
        monster.animator.SetBool("isHit", true);

        SetHitMotion(monster);
    }

    public override void UpdateState(Monster monster)
    {
        if (IsHitRecoveryTimeEnd())
        {
            monster.ChangeState(MoveState.Instance);
        }
    }

    public override void ExitState(Monster monster)
    {
        //_currentTime = 0.0f;
        monster.IsHit = false;
        monster.animator.SetBool("isHit", false);
        monster.animator.SetInteger("hitMotion", (int)HitMotion.HitMotionEnd);
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

    private void SetHitMotion(Monster monster)
    {
        if ((HitMotion)monster.animator.GetInteger("hitMotion") == HitMotion.HitMotionEnd)
            _currentHitMotion = HitMotion.HitMotion0;

        else if ((HitMotion)monster.animator.GetInteger("hitMotion") == HitMotion.HitMotion0)
            _currentHitMotion = HitMotion.HitMotion1;

        else if ((HitMotion)monster.animator.GetInteger("hitMotion") == HitMotion.HitMotion1)
            _currentHitMotion = HitMotion.HitMotion0;

        monster.animator.SetInteger("hitMotion", (int)_currentHitMotion);
    }
}

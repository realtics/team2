using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitState : FSMState<Monster>
{
    //static readonly HitState instance = new HitState();
    //public static HitState Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    //static HitState() { }
    //private HitState() { }

    public override void EnterState(Monster monster)
    {
        monster.HitRecoveryCurrentTime = 0.0f;
        monster.IsHit = true;
        monster.animator.SetBool("isHit", true);

        SetHitMotion(monster);
    }

    public override void UpdateState(Monster monster)
    {
        if (IsHitRecoveryTimeEnd(monster))
        {
            //monster.ChangeState(MoveState.Instance);
            monster.ChangeState(monster._moveState);
        }
    }

    public override void ExitState(Monster monster)
    {
        monster.IsHit = false;
        monster.animator.SetBool("isHit", false);
        monster.animator.SetInteger("hitMotion", (int)Monster.HitMotion.HitMotionEnd);
    }

    //FIXME : 이함수가 여기있는 것이 옳은지 판단 후 위치 이동해야함
    private bool IsHitRecoveryTimeEnd(Monster monster)
    {
        monster.HitRecoveryCurrentTime += Time.deltaTime;
        if (monster.HitRecoveryCurrentTime >= monster.HitRecoveryResetTime)
        {
            return true;
        }
        return false;
    }

    //FIXME : 이함수가 여기있는 것이 옳은지 판단 후 위치 이동해야함
    private void SetHitMotion(Monster monster)
    {
        if ((Monster.HitMotion)monster.animator.GetInteger("hitMotion") == Monster.HitMotion.HitMotionEnd)
            monster.CurrentHitMotion = Monster.HitMotion.HitMotion0;

        else if ((Monster.HitMotion)monster.animator.GetInteger("hitMotion") == Monster.HitMotion.HitMotion0)
            monster.CurrentHitMotion = Monster.HitMotion.HitMotion1;

        else if ((Monster.HitMotion)monster.animator.GetInteger("hitMotion") == Monster.HitMotion.HitMotion1)
            monster.CurrentHitMotion = Monster.HitMotion.HitMotion0;

        monster.animator.SetInteger("hitMotion", (int)monster.CurrentHitMotion);
    }
}

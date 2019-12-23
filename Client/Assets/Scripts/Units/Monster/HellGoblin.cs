using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGoblin : Goblin
{

    protected override void Awake()
    {
        base.Awake();
        OnSuperArmor();
    }

    //DieState
    public override void EnterDieState()
    {


        StartCoroutine(TimeScaleSlow());
        EffectManager.Instance.SpawnClearCircle(HitBoxCenter);

        base.EnterDieState();
    }

    public override void UpdateDieState()
    {
        base.UpdateDieState();
    }


    public override void ExitDieState()
    {
        base.ExitDieState();
    }

    public override void NoticeDie()
    {
        MonsterManager.Instance.ReceiveBossMonsterDie(this);
    }

    private IEnumerator TimeScaleSlow()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }
}

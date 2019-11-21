using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : FSMState<Monster>
{
    //static readonly DieState instance = new DieState();
    //public static DieState Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    //static DieState() { }
    //private DieState() { }

    public override void EnterState(Monster monster)
    {
        monster.animator.SetBool("isDie", true);
        monster.InactiveHitBox();

        //FIXME : 보스몬스터 구분시 변경
        if (monster.name == "testMonster")
            GameManager.Instance.NoticeGameClear();
        //TODO : 아이템 드랍
    }

    public override void UpdateState(Monster monster)
    {
       
    }

    public override void ExitState(Monster monster)
    {
       
    }
}

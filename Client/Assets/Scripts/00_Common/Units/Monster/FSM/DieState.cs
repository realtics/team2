﻿using System.Collections;
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


    public override void EnterState(Monster monster)
    {
        monster.animator.SetBool("isDie", true);
        monster.InactiveHitBox();
      
        //TODO : 아이템 드랍
    }

    public override void UpdateState(Monster monster)
    {
       
    }

    public override void ExitState(Monster monster)
    {
       
    }
}

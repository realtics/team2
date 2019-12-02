﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calvary : BaseMonster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //MonsterManager.Instance.ReceiveMonsterDie(this);
            ChangeDieState();
        }
    }

	protected override void AddHitEffect()
	{
		Vector3 newPos = new Vector3(0, 4.53f+1, 0);
		HitEffectManager.Instance.AddHitEffect(_avatar.position + _hitBox.right / 2 + _hitBox.up / 2 + newPos, 4.2f);
	}

	//AttackState
	public override void EnterAttackState()
    {
        base.EnterAttackState();
		_animator.SetFloat("animSpeed", 1.5f);
	}

    public override void UpdateAttackState()
    {
        base.UpdateAttackState();
	}

    public override void ExitAttackState()
    {
        base.ExitAttackState();
		_animator.SetFloat("animSpeed", 1.0f);
	}

    //MoveState
    public override void EnterMoveState()
    {
        base.EnterMoveState();
    }

    public override void UpdateMoveState()
    {
        base.UpdateMoveState();
    }

    public override void ExitMoveState()
    {
        base.ExitMoveState();
    }

    //HitState
    public override void EnterHitState()
    {
        base.EnterHitState();	
	}

    public override void UpdateHitState()
    {
        base.UpdateHitState();
    }

    public override void ExitHitState()
    {
        base.ExitHitState();
	}

    //DieState
    public override void EnterDieState()
    {
        // FIXME HACK : (안병욱) 수정해
        StartCoroutine(TimeScaleSlow());
        GameObject circle = ObjectPoolManager.Instance.GetRestObject(SwordmanSkillManager.Instance.FindSkillEffect(SwordmanSkillIndex.ClearCircle));
        Vector3 newPos = transform.root.position;
        newPos.y += 1.2f;
        circle.transform.position = newPos;

        print(newPos + " / " + newPos);

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

    protected override void SetAerialValue(AttackInfoSender sender)
    {
        //FIXME : 컴포넌트화?
        //this monster don't have aerialvalue
        return;
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


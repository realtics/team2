using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tauarmy : BaseMonster
{
    private enum TauarmyAttackMotion
    {
        AttackMotion1 = 0,
        AttackMotion2 = 1,
        AttacknMotionEnd = 2
    }
    private TauarmyAttackMotion _currentAttackMotion;

    [SerializeField]
    private float _rushAttackResetTime;
    [SerializeField]
    private float _rushAttackCurrentTime;

    [SerializeField]
    private Transform _rushAttackBox;

    protected override void SetInitialState()
    {
        base.SetInitialState();
        _rushAttackCurrentTime = _rushAttackResetTime;
    }

    protected override void Awake()
	{
		base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void ActiveRushAttackBox()
    {
        _rushAttackBox.gameObject.SetActive(true);
    }

    public void InactiveRushAttackBox()
    {
        _rushAttackBox.gameObject.SetActive(false);
    }

    //AttackState
    public override void EnterAttackState()
    { 
        base.EnterAttackState();
		OnSuperArmor();
        CanRushAttack();
		//_currentAttackMotion = (TauarmyAttackMotion)Random.Range((int)TauarmyAttackMotion.AttackMotion1, (int)TauarmyAttackMotion.AttacknMotionEnd);
        _animator.SetInteger("attackMotion", (int)_currentAttackMotion);
        SetForwardDirection();
	}

    public override void UpdateAttackState()
    {
        if (_currentAttackMotion == TauarmyAttackMotion.AttackMotion2)
        {
            transform.position += _forwardDirection * Time.deltaTime * (_moveSpeed*3);
        }
        base.UpdateAttackState();
    }

    public override void ExitAttackState()
    {
        base.ExitAttackState();
		OffSuperArmor();
		InactiveRushAttackBox();
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

    private void CanRushAttack()
    {
        if (_rushAttackCurrentTime >= _rushAttackResetTime)
        {
            _currentAttackMotion = TauarmyAttackMotion.AttackMotion2;
            StartCheckRushAttackTime();
        }
        else
        {
            _currentAttackMotion = TauarmyAttackMotion.AttackMotion1;
        }
    }

    IEnumerator CheckRushAttackTime()
    {
        _rushAttackCurrentTime += Time.deltaTime;

        if (_rushAttackCurrentTime >= _rushAttackResetTime)
        {
            StopCoroutine("CheckRushAttackTime");
            yield break;
        }
        yield return new WaitForFixedUpdate();
        StartCoroutine("CheckRushAttackTime");
    }

    protected void StartCheckRushAttackTime()
    {
        _rushAttackCurrentTime = 0.0f;
        StartCoroutine("CheckRushAttackTime");
    }
}

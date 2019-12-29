using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calvary : BaseMonster
{
    private enum CalvaryAttackMotion
    {
        AttackMotion1 = 0,
        AttackMotion2 = 1,
        AttacknMotionEnd = 2
    }
    private CalvaryAttackMotion _currentAttackMotion;

    [SerializeField]
    private float _smashAttackResetTime;
    [SerializeField]
    private float _smashAttackCurrentTime;

    [SerializeField]
    private Transform _smashAttackBox;

    private AudioSource _audioSource;
	[SerializeField]
	private AudioClip _meet;

    private float _originalRange;

    protected override void SetInitialState()
    {
        base.SetInitialState();
        _originalRange = _attackRange;
        _attackRange = _originalRange * 3;
        _smashAttackCurrentTime = _smashAttackResetTime;
    }

    protected override void Awake()
	{
		base.Awake();
		_audioSource = GetComponent<AudioSource>();
		OnSuperArmor();
	}

	private void Start()
	{
		_audioSource.clip = _meet;
		_audioSource.Play();
	}

	protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ChangeAttackRange();

        if (Input.GetKeyDown(KeyCode.F2))
        {
            //MonsterManager.Instance.ReceiveMonsterDie(this);
            ChangeDieState();
        }
    }

    public void ActiveSmashAttackBox()
    {
        _smashAttackBox.gameObject.SetActive(true);
    }

    public void InactiveSmashAttackBox()
    {
        _smashAttackBox.gameObject.SetActive(false);
    }

    //AttackState
    public override void EnterAttackState()
    {
        base.EnterAttackState();
        CanSmashAttack();
        _animator.SetInteger("attackMotion", (int)_currentAttackMotion);
	}

    public override void UpdateAttackState()
    {
        base.UpdateAttackState();
	}

    public override void ExitAttackState()
    {
        base.ExitAttackState();
		_animator.SetFloat("animSpeed", 1.0f);
        _attackRange = _originalRange;
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

    private void CanSmashAttack()
    {
        if (_smashAttackCurrentTime >= _smashAttackResetTime)
        {
            _currentAttackMotion = CalvaryAttackMotion.AttackMotion2;
            StartCheckSmashAttackTime();
        }
        else
        {
            _currentAttackMotion = CalvaryAttackMotion.AttackMotion1;
            _animator.SetFloat("animSpeed", 1.5f);
        }
    }

    IEnumerator CheckSmashAttackTime()
    {
        _smashAttackCurrentTime += Time.deltaTime;

        if (_smashAttackCurrentTime >= _smashAttackResetTime)
        {
            StopCoroutine("CheckSmashAttackTime");
            yield break;
        }
        yield return new WaitForFixedUpdate();
        StartCoroutine("CheckSmashAttackTime");
    }

    protected void StartCheckSmashAttackTime()
    {
        _smashAttackCurrentTime = 0.0f;
        StartCoroutine("CheckSmashAttackTime");
    }

    private void ChangeAttackRange()
    {
        if (_smashAttackCurrentTime >= _smashAttackResetTime)
        {
            _attackRange = _originalRange * 3;
        }
    }
}


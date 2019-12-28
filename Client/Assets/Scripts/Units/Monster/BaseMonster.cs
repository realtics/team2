using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{

    //values for UIHelper 
    public enum MonsterTypeInfo
	{
		Goblin,
		Tau,
		Calvary
	}
	[SerializeField]
	protected MonsterTypeInfo _monsterType;
	[SerializeField]
	protected string _monsterName;
	[SerializeField]
	protected int _monsterLevel;
	[SerializeField]
	protected float _currentHp;
	[SerializeField]
	protected float _maxHp;
    [SerializeField]
    protected float _attackDamage;
    [SerializeField]
    protected float _defensePercent;

	//values for MonsterControl
	[SerializeField]
	protected float _attackRange;
	[SerializeField]
	protected float _chaseCancleTime;
	[SerializeField]
	protected float _chaseTime = 0.0f;
	[SerializeField]
	protected float _moveSpeed;
	[SerializeField]
	protected Transform _target = null;

	//values for BaseAttack
	[SerializeField]
	protected float _baseAttackResetTime;
	[SerializeField]
	protected float _baseAttackCurrentTime;
	[SerializeField]
	protected Transform _baseAttackBox;

	//values for HitEffect
	[SerializeField]
	protected Transform _hitBox;
	protected BoxCollider2D _hitBoxCenter;
	protected float _hitEffectSize;

	//values for sprite & animator
	protected Animator _animator;
	protected SpriteOutline _superArmorLine;
	protected Vector3 _forwardDirection;

	private StateMachine<BaseMonster> _state = null;
	private FSMState<BaseMonster> _attackState = new AttackState();
	private FSMState<BaseMonster> _moveState = new MoveState();
	private FSMState<BaseMonster> _hitState = new HitState();
	private FSMState<BaseMonster> _dieState = new DieState();
	private FSMState<BaseMonster> _downRecoveryState = new DownRecoveryState();

	private bool _isDead;
	private bool _isAttack;
	private bool _isHit;
	private bool _isAerialHit;
	private bool _hasAerialPower;
	private bool _isDown;
	private bool _isDownRecovery;

	[SerializeField]
	protected Transform _avatar;
	protected Vector3 _originPos;
	private float _height;
	private float _jumpValue;

	//values for MoveState 
	private enum MovementStateInfo
	{
		Idle = 0,
		Left = 1,
		Right = 2,
	}
	private MovementStateInfo _movementState;
	private float _randomMoveResetTime;
	private float _randomMoveCurrentTime;
	private const float InitialResetTime = 3.0f;

	//values for HitState
	private enum HitMotion
	{
		HitMotion0 = 0,
		HitMotion1 = 1,
		HitMotionEnd = 2
	}
	private HitMotion _currentHitMotion;
	private float _hitRecoveryResetTime;
	private float _hitRecoveryCurrentTime;

	//values for KnockBack
	private Vector3 _knockBackDirection;
	private float _knockBackSpeed;
	private float _knockBackDuration;

	//properties
	public MonsterTypeInfo MonsterType { get { return _monsterType; } }
	public string MonsterName { get { return _monsterName; } }
	public int MonsterLevel { get { return _monsterLevel; } }
	public float CurrentHp { get { return _currentHp; } }
	public float MaxHp { get { return _maxHp; } }
    public float AttackDamage { get { return _attackDamage; } }

    public bool IsGround { get { return !(_height > 0.0f); } }
	public bool IsAttack { get { return _isAttack; } set { _isAttack = value; } }
	public bool IsHit { get { return _isHit; } set { _isHit = value; } }
	public bool IsSuperArmor { get { return _superArmorLine.enabled; }}

	public Vector3 HitBoxCenter { get { return _hitBoxCenter.bounds.center; } }

	protected virtual void Awake()
	{
		SetInitialState();
	}

	protected virtual void FixedUpdate()
	{
		if (_currentHp <= 0 && !_isDead && !_isAerialHit)
		{
			_state.ChangeState(_dieState);
			_isDead = true;
		}
		_state.Update();

		//치트키
		if (Input.GetKeyDown(KeyCode.F1))
		{
			ChangeState(_dieState);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			_target = other.transform.root;
		}
		else
		{
			return;
		}
	}

	protected void ChangeState(FSMState<BaseMonster> state)
	{
		_state.ChangeState(state);
	}

	protected virtual void SetInitialState()
	{
		_animator = GetComponentInChildren<Animator>();
		_currentHp = MaxHp;
		_baseAttackCurrentTime = _baseAttackResetTime;
		_originPos = _avatar.localPosition;
		_hitBoxCenter = _hitBox.GetComponent<BoxCollider2D>();
		_hitEffectSize = _hitBoxCenter.size.y + _hitBoxCenter.size.x;
		_superArmorLine = _avatar.GetComponent<SpriteOutline>();
       
        _state = new StateMachine<BaseMonster>();
		_state.InitialSetting(this, _moveState);

		_target = null;
	}

	protected bool CheckRange()
	{
		if ((Mathf.Abs(_target.position.x - transform.position.x) < _attackRange) &&
			(Mathf.Abs(_target.position.y - transform.position.y) < _attackRange / 4))
		{
			return true;
		}
		return false;
	}

	public void ActiveBaseAttackBox()
	{
		_baseAttackBox.gameObject.SetActive(true);
	}

	public void InactiveBaseAttackBox()
	{
		_baseAttackBox.gameObject.SetActive(false);
	}

	public void ActiveHitBox()
	{
		_hitBox.gameObject.SetActive(true);
	}

	public void InactiveHitBox()
	{
		_hitBox.gameObject.SetActive(false);
	}

	protected void SetForwardDirection()
	{
		if (transform.localScale.x > 0)
			_forwardDirection = Vector3.left;
		else
			_forwardDirection = Vector3.right;
	}

	public void FlipImage()
	{
		if (_target == null)
			return;

		if ((_target.position.x - transform.position.x) > 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}

		else
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
	}

	public void OnHit(AttackInfoSender sender)
	{
        float realDmage = sender.Damage * (1.0f - _defensePercent);

        AddHitEffect();
		AddHitDamageEffect(realDmage);

        _currentHp -= realDmage;
		UIHelper.Instance.SetMonster(this);

		if (!IsSuperArmor)
		{
			if (sender.HorizontalExtraMoveDuration > 0)
				SetKnockbackValue(sender);
			if (sender.ExtraHeightValue > 0)
				SetAerialValue(sender);

			if (_isHit)
				_state.RestartState();
			else
				_state.ChangeState(_hitState);
		}
	}
	
	protected void AddHitEffect()
	{
		EffectManager.Instance.AddHitEffect(HitBoxCenter, _hitEffectSize);
	}

	protected void AddHitDamageEffect(float damage)
	{
		EffectManager.Instance.AddHitDamageEffect(HitBoxCenter, damage);
	}

	//FIXME: AttackInofoSender 의 값중 넉백관련만 인자로 받게 고쳐야함
	protected virtual void SetKnockbackValue(AttackInfoSender sender)
	{
		_hasAerialPower = false;
		Vector3 direction = Vector3.zero;

		if ((sender.Attacker.position.x - transform.position.x) < 0)
			direction = Vector3.left;

		else
			direction = Vector3.right;

		_knockBackDirection = direction;
		_knockBackSpeed = sender.HorizontalExtraMoveValue;
		_knockBackDuration = sender.HorizontalExtraMoveDuration;
		_hitRecoveryResetTime = sender.StunDuration;

		StartCoroutine("Knockback");
	}

	IEnumerator Knockback()
	{
		_knockBackDuration -= Time.deltaTime;

		if (_knockBackDuration <= 0.0f)
		{
			StopCoroutine("Knockback");
			yield break;
		}

		else
		{
			transform.position += _knockBackDirection * Time.deltaTime * _knockBackSpeed;
		}

		yield return new WaitForFixedUpdate();
		StartCoroutine("Knockback");
	}

	protected virtual void SetAerialValue(AttackInfoSender sender)
	{
		_hasAerialPower = true;
		_isAerialHit = true;
		_animator.SetBool("isAaerial", true);

		_jumpValue = sender.ExtraHeightValue;

		if (IsGround)
			StartCoroutine("AerialProcess");
	}

	protected virtual void SetAerialValue(float sender)
	{
		StopCoroutine("AerialProcess");
		_jumpValue = sender;
		StartCoroutine("AerialProcess");
	}

	IEnumerator AerialProcess()
	{
		Vector3 groundPos = _originPos;
		groundPos.y += _height;
		_avatar.localPosition = groundPos;


		_height += _jumpValue;
		_jumpValue -= Time.deltaTime * 0.7f;

		if (_height <= 0.0f)
		{
			_isAerialHit = false;
			_animator.SetBool("isAaerial", false);

			_animator.SetInteger("hitMotion", (int)HitMotion.HitMotionEnd);

			_isDown = true;
			_animator.SetBool("isDown", true);

			_height = 0.0f;
			_avatar.localPosition = _originPos;
			_jumpValue = 0.0f;
			StopCoroutine("AerialProcess");
			yield break;
		}

		yield return new WaitForFixedUpdate();
		StartCoroutine("AerialProcess");
	}

	protected void OnSuperArmor()
	{
		_superArmorLine.enabled = true;
	}

	protected void OffSuperArmor()
	{
		_superArmorLine.enabled = false;
	}

	//AttackState
	public virtual void EnterAttackState()
	{
		if (_target == null)
		{
			return;
		}
		IsAttack = true;

		FlipImage();
		_animator.SetBool("isAttacking", true);
	}

	public virtual void UpdateAttackState()
	{
		if (!_animator.IsInTransition(0))
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				ChangeState(_moveState);
				StartCheckBaseAttackTime();
			}
		}
	}

	public virtual void ExitAttackState()
	{
		_animator.SetBool("isAttacking", false);
		IsAttack = false;
		InactiveBaseAttackBox();
	}

	protected virtual bool IsAttackable()
	{
		if (_baseAttackCurrentTime >= _baseAttackResetTime)
			return true;

		else
			return false;
	}

	IEnumerator CheckBaseAttackTime()
	{
		_baseAttackCurrentTime += Time.deltaTime;

		if (_baseAttackCurrentTime >= _baseAttackResetTime)
		{
			StopCoroutine("CheckBaseAttackTime");
			yield break;
		}
		yield return new WaitForFixedUpdate();
		StartCoroutine("CheckBaseAttackTime");
	}

	protected void StartCheckBaseAttackTime()
	{
		_baseAttackCurrentTime = 0.0f;
		StartCoroutine("CheckBaseAttackTime");
	}

	//DieState
	public virtual void EnterDieState()
	{
		_animator.SetBool("isDie", true);
		InactiveHitBox();
	}

	public virtual void UpdateDieState()
	{
		if (_animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Die"))
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.5f)
			{
				//gameObject.SetActive(false);
				NoticeDie();
			}
		}
	}

	public virtual void ExitDieState()
	{
		//nothing
	}

	public void ChangeDieState()
	{
		ChangeState(_dieState);
	}

	//HitState
	public virtual void EnterHitState()
	{
		_hitRecoveryCurrentTime = 0.0f;
		IsHit = true;
		_animator.SetBool("isHit", true);

		if (!_hasAerialPower && _isAerialHit)
		{
			//FIXME : 매직넘버
			SetAerialValue(0.1f);
		}
		SetHitMotion();
	}

	public virtual void UpdateHitState()
	{
		if (_isAerialHit)
		{


		}
		else
		{
			if (IsHitRecoveryTimeEnd())
			{
				if (_isDown)
				{
					_isDown = false;
					_animator.SetBool("isDown", false);
					ChangeState(_downRecoveryState);
				}
				else
				{
					ChangeState(_moveState);
				}
			}
		}
	}

	public virtual void ExitHitState()
	{
		IsHit = false;
		_animator.SetBool("isHit", false);
		_animator.SetInteger("hitMotion", (int)HitMotion.HitMotionEnd);
	}

	protected bool IsHitRecoveryTimeEnd()
	{
		_hitRecoveryCurrentTime += Time.deltaTime;
		if (_hitRecoveryCurrentTime >= _hitRecoveryResetTime)
		{
			return true;
		}
		return false;
	}

	protected void SetHitMotion()
	{
		if ((HitMotion)_animator.GetInteger("hitMotion") == HitMotion.HitMotionEnd)
			_currentHitMotion = HitMotion.HitMotion0;

		else if ((HitMotion)_animator.GetInteger("hitMotion") == HitMotion.HitMotion0)
			_currentHitMotion = HitMotion.HitMotion1;

		else if ((HitMotion)_animator.GetInteger("hitMotion") == HitMotion.HitMotion1)
			_currentHitMotion = HitMotion.HitMotion0;

		_animator.SetInteger("hitMotion", (int)_currentHitMotion);
	}

	//DownRecoveryState
	public virtual void EnterDownRecoveryState()
	{
		_isDownRecovery = true;
		_animator.SetBool("isDownRecovery", true);
	}

	public virtual void UpdateDownRecoveryState()
	{
		if (!_animator.IsInTransition(0))
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				ChangeState(_moveState);
			}
		}
	}

	public virtual void ExitDownRecoveryState()
	{
		_isDownRecovery = false;
		_animator.SetBool("isDownRecovery", false);
	}

	//MoveState
	public virtual void EnterMoveState()
	{
		_randomMoveResetTime = InitialResetTime;
		_randomMoveCurrentTime = _randomMoveResetTime;
	}

	public virtual void UpdateMoveState()
	{
		if (_target != null)
		{
			_animator.SetBool("isMoving", true);
			if (!CheckRange())
			{
				//FIXME : 추적시간 초과 함수화
				_chaseTime += Time.deltaTime;
				if (_chaseTime >= _chaseCancleTime)
				{
					_target = null;
					_chaseTime = 0.0f;
					return;
				}

				//FIXME : 추적 함수화
				Vector3 direction = _target.position - transform.position;
				direction.Normalize();

				FlipImage();

				direction.x *= _moveSpeed;
				direction.y *= (_moveSpeed / 2);

				if ((Mathf.Abs(_target.position.x - transform.position.x) <= _attackRange))
					direction.x = 0;

				transform.position += direction * Time.smoothDeltaTime;
			}
			else
			{
				if (IsAttackable())
					ChangeState(_attackState);
				else
					_animator.SetBool("isMoving", false);
			}
		}
		else
		{
			SetRandDirection();

			//FIXME :: 함수화
			Vector3 direction = Vector3.zero;

			if (_movementState == MovementStateInfo.Left)
			{
				direction = Vector3.left;
				transform.localScale = new Vector3(1, 1, 1);
			}

			else if (_movementState == MovementStateInfo.Right)
			{
				direction = Vector3.right;
				transform.localScale = new Vector3(-1, 1, 1);
			}

			transform.position += direction * Time.smoothDeltaTime * (_moveSpeed / 3f);
		}
	}

	public virtual void ExitMoveState()
	{
		_movementState = MovementStateInfo.Idle;
		_animator.SetBool("isMoving", false);
	}

	protected void SetRandDirection()
	{
		_randomMoveCurrentTime += Time.smoothDeltaTime;
		if (_randomMoveCurrentTime >= _randomMoveResetTime)
		{
			_movementState = (MovementStateInfo)Random.Range((int)MovementStateInfo.Idle, (int)MovementStateInfo.Right + 1);

			if (_movementState == MovementStateInfo.Idle)
				_animator.SetBool("isMoving", false);
			else
				_animator.SetBool("isMoving", true);

			_randomMoveResetTime = Random.Range(1f, 4f);
			_randomMoveCurrentTime = 0f;
		}
	}

	//For MonsterManager
	public virtual void NoticeDie()
	{
		MonsterManager.Instance.ReceiveMonsterDie(this);
	}

	public void ResetMonster()
	{
		_isDead = false;
		_currentHp = MaxHp;
		_baseAttackCurrentTime = _baseAttackResetTime;
		_chaseTime = 0.0f;
		ActiveHitBox();
		ChangeState(_moveState);
	}

	public void InactiveMonster()
	{
		gameObject.SetActive(false);
	}

}

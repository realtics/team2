using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
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

    //values for MonsterControl
    protected float _attackRange;
    [SerializeField]
    protected float _chaseCancleTime;
    [SerializeField]
    protected float _chaseTime = 0.0f;
    [SerializeField]
    public float _moveSpeed;

    [SerializeField]
    protected Transform _target = null;
    [SerializeField]
    protected Transform _smashHitBox;
    [SerializeField]
    protected Transform _hitBox;
    protected Animator _animator;
   

    private StateMachine<Monster> _state = null;
    private FSMState<Monster> _attackState = new AttackState();
    private FSMState<Monster> _moveState = new MoveState();
    private FSMState<Monster> _hitState = new HitState();
    private FSMState<Monster> _dieState = new DieState();

    private bool _isDead = false;
    private bool _isAttack;
    private bool _isHit;

    //values for MoveState 
    public enum MovementStateInfo
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
    public enum HitMotion
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

    public bool IsAttack { get { return _isAttack; } set { _isAttack = value; } }
    public bool IsHit { get { return _isHit; } set { _isHit = value; } }

    protected virtual void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _currentHp = MaxHp;
        SetInitialState();
    }

    protected virtual void FixedUpdate()
    {
        if (_currentHp <= 0 && !_isDead)
        {
            _state.ChangeState(_dieState);
            _isDead = true;
        }
        _state.Update();
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

    protected void ChangeState(FSMState<Monster> state)
    {
        _state.ChangeState(state);
    }

    protected void SetInitialState()
    {
        _state = new StateMachine<Monster>();
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

    public void OnHit(AttackInfoSender sender)
    {
        SetKnockbackValue(sender);
        _currentHp -= sender.Damage;

        UIHelper.Instance.SetMonster(this);
        UIHelper.Instance.SetMonsterHp(_currentHp, _maxHp);

        if (!_isHit)
            _state.ChangeState(_hitState);
        else
            _state.RestartState();
    }

    public void ActiveSmashHitBox()
    {
        _smashHitBox.gameObject.SetActive(true);
    }

    public void InactiveSmashHitBox()
    {
        _smashHitBox.gameObject.SetActive(false);
    }

    public void InactiveHitBox()
    {
        _hitBox.gameObject.SetActive(false);
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

    //FIXME: AttackInofoSender 의 값중 넉백관련만 인자로 받게 고쳐야함
    protected void SetKnockbackValue(AttackInfoSender sender)
    {
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

        yield return null;
        StartCoroutine("Knockback");
    }

    //TODO : 공중공격피격시 띄움 판정함수


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
        BaseAttack();
    }

    public virtual void ExitAttackState()
    {
        _animator.SetBool("isAttacking", false);
        IsAttack = false;
        InactiveSmashHitBox();
    }

    protected void BaseAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ChangeState(_moveState);
        }
    }

    //DieState
    public virtual void EnterDieState()
    {
        _animator.SetBool("isDie", true);
        InactiveHitBox();
    }

    public virtual void UpdateDieState()
    {
        //nothing
    }

    public virtual void ExitDieState()
    {
        //nothing
    }

    //HitState
    public virtual void EnterHitState()
    {
        _hitRecoveryCurrentTime = 0.0f;
        IsHit = true;
        _animator.SetBool("isHit", true);

        SetHitMotion();
    }

    public virtual void UpdateHitState()
    {
        if (IsHitRecoveryTimeEnd())
        {
            ChangeState(_moveState);
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
                ChangeState(_attackState);
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

}

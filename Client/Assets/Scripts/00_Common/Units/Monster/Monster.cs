using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string monsterName = "testMonster";
    public int monsterLevel = 99;

    public float currentHp = 200.0f;
    public float maxHp = 200.0f;

    public float attackRange = 3.8f;

    public float chaseCancleTime = 5.0f;
    public float chaseTime = 0;

    public float moveSpeed = 2.5f;

    public Transform target = null;
    public Animator animator = null;

    private StateMachine<Monster> _state = null;

    private Transform _smashHitBox;
    private Transform _hitBox;

    private bool _isDead = false;

    private bool _isAttack;
    private bool _isHit;

    //MoveState Value
    public enum MovementStateInfo
    {
        Idle = 0,
        Left = 1,
        Right = 2,
    }
    private float _randomMoveResetTime;
    private float _randomMoveCurrentTime;
    private MovementStateInfo _moveMentState;

    //KnockBakc Value
    private Vector3 _knockBackDirection;
    private float _knockBackSpeed;
    private float _knockBackDuration;
       
    //properties
    public bool IsAttack { get { return _isAttack; } set { _isAttack = value; } }
    public bool IsHit { get { return _isHit; } set { _isHit = value; } }
    public float RandomMoveResetTime { get { return _randomMoveResetTime; } set { _randomMoveResetTime = value; } }
    public float RandomMoveCurrentTime { get { return _randomMoveCurrentTime; } set { _randomMoveCurrentTime = value; } }
    public MovementStateInfo MovementState { get { return _moveMentState; } set { _moveMentState = value;  } }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _smashHitBox = transform.Find("SmashHitBox");
        _hitBox = transform.Find("HitBox");
        ResetState();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (currentHp <= 0 && !_isDead)
        {
            _state.ChangeState(DieState.Instance);
            _isDead = true;
        }

        if (_isHit)
        {
            
        }

        _state.Update();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            target = other.transform.root;
        }
        else
        {
            return;
        }
    }

    // 상태변경
    public void ChangeState(FSMState<Monster> state)
    {
        _state.ChangeState(state);
    }

    public bool CheckRange()
    {
        if ((Mathf.Abs(target.position.x - transform.position.x) < attackRange) &&
            (Mathf.Abs(target.position.y - transform.position.y) < attackRange / 4))
        {
            return true;
        }
        return false;
    }

    public void ResetState()
    {
        _state = new StateMachine<Monster>();
        _state.InitialSetting(this, MoveState.Instance);

        target = null;
    }

    public void OnHit(AttackInfoSender sender)
    {
        //SetHit(sender.StunDuration);
        //StopAttack();

        SetKnockbackValue(sender);
        currentHp -= sender.Damage;
        UIHelper.Instance.SetMonster(this);
        UIHelper.Instance.SetMonsterHp(currentHp, maxHp);

        if (!_isHit)
            _state.ChangeState(HitState.Instance);

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
        if (target == null)
            return;

        if ((target.position.x - transform.position.x) > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //FIXME: 경직도 생성시 이름변경 혹은 새함수 만들어야함
    private void SetKnockbackValue(AttackInfoSender sender)
    {
        Vector3 direction = Vector3.zero;

        if ((sender.Attacker.position.x - transform.position.x) < 0)
            direction = Vector3.left;

        else
            direction = Vector3.right;

        _knockBackDirection = direction;
        _knockBackSpeed = sender.HorizontalExtraMoveValue;
        _knockBackDuration = sender.HorizontalExtraMoveDuration;

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
}

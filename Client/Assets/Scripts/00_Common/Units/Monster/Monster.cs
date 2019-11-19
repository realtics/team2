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

    //FIXME: 피격시 밀림 변수 테스트중
    private Vector3 _hitDirection;
    private float _hitSpeed;
    private float _hitDuration;
    private float _hitTime;
       


    //properties
    public bool IsAttack { get { return _isAttack; } set { _isAttack = value; } }
    public bool IsHit { get { return _isHit; } set { _isHit = value; } }

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
            _state.ChangeState(DieState.GetInstance);
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
        _state.InitialSetting(this, MoveState.GetInstance);

        target = null;
    }

    public void OnHit(AttackInfoSender sender)
    {
        //SetHit(sender.StunDuration);
        //StopAttack();

        MovedOnHit(sender);
        currentHp -= sender.Damage;
        UIHelper.Instance.SetMonster(this);
        UIHelper.Instance.SetMonsterHp(currentHp, maxHp);

        if (!_isHit)
            _state.ChangeState(HitState.GetInstance);

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

    private void MovedOnHit(AttackInfoSender sender)
    {
        Vector3 direction = Vector3.zero;

        if ((sender.Attacker.position.x - transform.position.x) < 0)
            direction = Vector3.left;

        else
            direction = Vector3.right;

        _hitDirection = direction;
        _hitSpeed = sender.HorizontalExtraMoveValue;
        _hitDuration = sender.HorizontalExtraMoveDuration;
    }
}

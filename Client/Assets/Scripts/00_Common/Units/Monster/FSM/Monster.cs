using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string monsterName = "testMonser";
    public int monsterLevel = 99;

    public float currentHp = 200.0f;
    public float maxHp = 200.0f;

    public float attackRange =3.8f;
    public float attackSpeed = 1.5f;

    public float chaseCancleTime = 5.0f;
    public float chaseTime = 0;

    public float moveSpeed = 2.5f;

    public Transform target = null;
    public Animator animator = null;

    private StateMachine<Monster> _state = null;

    private Transform _smashHitBox;
    private float _smashAttackTimer;
    private float _smashAttackTime = 0.5f;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _smashHitBox = transform.Find("SmashHitBox");

        ResetState();
    }

    private void Update()
    {
        //FIXME : 플레이어 공격 판정 완료시 삭제
        TestHitKeyInput();
    }

    private void FixedUpdate()
    {
        _state.Update();

        if (IsActiveSmashHitBox())
        {
            _smashAttackTimer += Time.deltaTime;
            if (_smashAttackTimer >= _smashAttackTime)
            {
                InactiveSmashHitBox();
            }
        }
    }

    // 상태변경
    public void ChangeState(FSMState<Monster> state)
    {
        _state.ChangeState(state);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            //Debug.Log("Player Detect.");
            target = other.transform;

            //TODO: 피격
           
        }
        else
        {
            return;
        }
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

    public void OnHit(float damage)
    {
        Debug.Log("OnHit");

        //TODO : UI와 상호작용
        //FIXME : 간단한 테스트후 수정

        UIHelper.Instance.SetMonster(this);
        UIHelper.Instance.AddMonsterHp(-damage);
        currentHp -= damage;

        _state.ChangeState(HitState.GetInstance);
    }

    public void ActiveSmashHitBox()
    {
        _smashAttackTimer = 0.0f;
        _smashHitBox.gameObject.SetActive(true);
    }

    public void InactiveSmashHitBox()
    {
        _smashHitBox.gameObject.SetActive(false);
    }

    private bool IsActiveSmashHitBox()
    {
        if (_smashHitBox.gameObject.activeSelf == true)
            return true;
        else
            return false;
    }

    //FIXME : 플레이어 공격판정 완료시, 삭제
    private void TestHitKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            OnHit(5.0f);
    }
}

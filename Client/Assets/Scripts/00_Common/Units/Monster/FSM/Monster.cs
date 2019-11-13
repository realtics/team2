using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackSpeed = 1.5f;

    public float chaseCancleTime = 5.0f;
    public float chaseTime = 0;

    public float moveSpeed = 2.5f;

    public Transform target = null;
    public Animator animator = null;

    private StateMachine<Monster> _state = null;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetState();
    }

    // Update is called once per frame
    void Update()
    {
        _state.Update();
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
            Debug.Log("Player Detect.");
            target = other.transform;

            //if (CheckRange())
            //{
            //    Debug.Log("Attack.");
            //    ChangeState(AttackState.GetInstance);
              
            //}
            //else
            //{
            //    Debug.Log("Chsing.");
            //    animator.SetBool("isMoving", true);
            //    ChangeState(MoveState.GetInstance);
            //}
        }
        else
        {
            return;
        }
    }

    public bool CheckRange()
    {
        if ((Mathf.Abs(target.position.x - transform.position.x) < attackRange) && (Mathf.Abs(target.position.y - transform.position.y) < attackRange / 4))
        {
            return true;
        }
        return false;
    }

    public void ResetState()
    {
        _state = new StateMachine<Monster>();
        // 초기 상태 설정
        _state.InitialSetting(this, MoveState.GetInstance);
        // 타겟 null 설정
        target = null;
    }
}

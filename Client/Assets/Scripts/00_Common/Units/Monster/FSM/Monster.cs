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
    public Animation anim = null;

    private StateMachine<Monster> _state = null;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 상태변경
    public void ChangeState(FSMState<Monster> state)
    {
        _state.ChangeState(state);
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어 접근시 move_state로 변경
        if (other.transform.tag == "Player")
        {
            Debug.Log("Player Detect.");

            target = other.transform.Find("Player");

            if (CheckRange())
            {
                // ChangeState(State_Attack.Instance);
                Debug.Log("Attack.");
            }
            else
            {
                ChangeState(MoveState.GetInstance);
            }
        }
        else
        {
            return;
        }

    }

    public bool CheckRange()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        return false;
    }

    public bool CheckAngle()
    {
        if (Vector3.Dot(target.transform.position, transform.position) >= 0.5f)
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

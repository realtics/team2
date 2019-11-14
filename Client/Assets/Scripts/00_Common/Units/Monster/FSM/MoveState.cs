using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState<Monster>
{
    static readonly MoveState instance = new MoveState();
    public static MoveState GetInstance
    {
        get
        {
            return instance;
        }
    }

    static MoveState() { }
    private MoveState() { }

    private enum MovementState
    {
        Idle = 0,
        Left = 1,
        Right = 2,
    }

    private float _resetTime = 3f;
    private float _currentTime;

    private MovementState _moveMentState;
    
    public override void EnterState(Monster monster)
    {
        Debug.Log("Enter MoveState");
        _currentTime = _resetTime;
    }

    public override void UpdateState(Monster monster)
    {
        //타겟 유무 확인
        if (monster.target != null)
        {
            monster.animator.SetBool("isMoving", true);
            if (!monster.CheckRange())
            {
                //추적시간을 초과하면 타겟을 잃음
                monster.chaseTime += Time.deltaTime;
                if (monster.chaseTime >= monster.chaseCancleTime)
                {
                    monster.target = null;
                    monster.chaseTime = 0.0f;
                    return;
                }

                //방향설정
                Vector3 direction = monster.target.position - monster.transform.position;
                direction.Normalize();

                if (direction.x > 0)
                    monster.transform.localScale = new Vector3(-1, 1, 1);
                else
                    monster.transform.localScale = new Vector3(1, 1, 1);

                //monster.transform.position += direction * Time.smoothDeltaTime * monster.moveSpeed;
                direction.x *= monster.moveSpeed;
                direction.y *= (monster.moveSpeed / 2);
                monster.transform.position += direction * Time.smoothDeltaTime;
            }
            else
            {
               monster.ChangeState(AttackState.GetInstance);
            }
        }
        else
        {
            //타겟이 없을 때는 임의의 방향으로 방황함
            //방향재설정
            SetRandDirection(monster);
            Vector3 direction = Vector3.zero;

            if (_moveMentState == MovementState.Left)
            {
                direction = Vector3.left;
                monster.transform.localScale = new Vector3(1, 1, 1);
            }

            else if (_moveMentState == MovementState.Right)
            {
                direction = Vector3.right;
                monster.transform.localScale = new Vector3(-1, 1, 1);
            }

            monster.transform.position += direction * Time.smoothDeltaTime * (monster.moveSpeed / 3f);
        }
    }

    public override void ExitState(Monster monster)
    {
        Debug.Log("Exit AttacState");
        _moveMentState = MovementState.Idle;
        monster.animator.SetBool("isMoving", false);
    }


    // ResetTime 때 마다 임의의 방향으로 설정
    void SetRandDirection(Monster monster)
    {
        _currentTime += Time.smoothDeltaTime;
        if (_currentTime >= _resetTime)
        {
            _moveMentState = (MovementState) Random.Range((int)MovementState.Idle, (int)MovementState.Right+1);

            if (_moveMentState == MovementState.Idle)
                monster.animator.SetBool("isMoving", false);
            else
                monster.animator.SetBool("isMoving", true);

            // 시간 재설정
            _resetTime = Random.Range(1f, 4f);
            _currentTime = 0f;
        }
    }
}

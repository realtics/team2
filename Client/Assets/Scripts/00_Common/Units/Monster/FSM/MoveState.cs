/*
 * MoveState.cs
 * 몬스터이동 상태
 */

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

    private float _resetTime = 3f;
    private float _currentTime;

    private int   _movementFlag = 0; //0:Idle , 1:Left, 2:Right, 3:Up, 4:Down

    static MoveState() { }
    private MoveState() { }

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

            if (_movementFlag == 1)
            {
                direction = Vector3.left;
                monster.transform.localScale = new Vector3(1, 1, 1);
            }

            else if (_movementFlag == 2)
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
        _movementFlag = 0;
        monster.animator.SetBool("isMoving", false);
    }


    // ResetTime 때 마다 임의의 방향으로 설정
    void SetRandDirection(Monster monster)
    {
        _currentTime += Time.smoothDeltaTime;
        if (_currentTime >= _resetTime)
        {
            _movementFlag = Random.Range(0, 3);

            if (_movementFlag == 0)
                monster.animator.SetBool("isMoving", false);
            else
                monster.animator.SetBool("isMoving", true);

            // 시간 재설정
            _resetTime = Random.Range(1f, 4f);
            _currentTime = 0f;
        }
    }
}

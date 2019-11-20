using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState<Monster>
{
    static readonly MoveState instance = new MoveState();
    public static MoveState Instance
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
        _currentTime = _resetTime;
    }

    public override void UpdateState(Monster monster)
    {
        if (monster.target != null)
        {
            monster.animator.SetBool("isMoving", true);
            if (!monster.CheckRange())
            {
                //FIXME : 추적시간 초과 함수화
                monster.chaseTime += Time.deltaTime;
                if (monster.chaseTime >= monster.chaseCancleTime)
                {
                    monster.target = null;
                    monster.chaseTime = 0.0f;
                    return;
                }

                //FIXME : 추적 함수화
                Vector3 direction = monster.target.position - monster.transform.position;
                direction.Normalize();

                monster.FlipImage();

                direction.x *= monster.moveSpeed;
                direction.y *= (monster.moveSpeed / 2);

                if ((Mathf.Abs(monster.target.position.x - monster.transform.position.x) <= monster.attackRange))
                    direction.x = 0;

                monster.transform.position += direction * Time.smoothDeltaTime;
            }
            else
            {
               monster.ChangeState(AttackState.Instance);
            }
        }
        else
        {
            SetRandDirection(monster);

            //FIXME :: 함수화
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
        _moveMentState = MovementState.Idle;
        monster.animator.SetBool("isMoving", false);
    }

    private void SetRandDirection(Monster monster)
    {
        _currentTime += Time.smoothDeltaTime;
        if (_currentTime >= _resetTime)
        {
            _moveMentState = (MovementState) Random.Range((int)MovementState.Idle, (int)MovementState.Right+1);
            Debug.Log(monster.name + "  : " + _moveMentState);

            if (_moveMentState == MovementState.Idle)
                monster.animator.SetBool("isMoving", false);
            else
                monster.animator.SetBool("isMoving", true);

            _resetTime = Random.Range(1f, 4f);
            _currentTime = 0f;
        }
    }
}

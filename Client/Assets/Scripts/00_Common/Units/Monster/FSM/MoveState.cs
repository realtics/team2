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

    private const float InitialResetTime = 3.0f;

    public override void EnterState(Monster monster)
    {
        monster.RandomMoveResetTime = InitialResetTime;
        monster.RandomMoveCurrentTime = monster.RandomMoveResetTime;
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
           
            if (monster.MovementState == Monster.MovementStateInfo.Left)
            {
                direction = Vector3.left;
                monster.transform.localScale = new Vector3(1, 1, 1);
            }

            else if (monster.MovementState == Monster.MovementStateInfo.Right)
            {
                direction = Vector3.right;
                monster.transform.localScale = new Vector3(-1, 1, 1);
            }

            monster.transform.position += direction * Time.smoothDeltaTime * (monster.moveSpeed / 3f);
        }
    }

    public override void ExitState(Monster monster)
    {
        monster.MovementState = Monster.MovementStateInfo.Idle;
        monster.animator.SetBool("isMoving", false);
    }

    private void SetRandDirection(Monster monster)
    {
        monster.RandomMoveCurrentTime += Time.smoothDeltaTime;
        if (monster.RandomMoveCurrentTime >= monster.RandomMoveResetTime)
        {
            monster.MovementState = (Monster.MovementStateInfo) Random.Range((int)Monster.MovementStateInfo.Idle, (int)Monster.MovementStateInfo.Right+1);
            Debug.Log(monster.name + "  : " + monster.MovementState);

            if (monster.MovementState == Monster.MovementStateInfo.Idle)
                monster.animator.SetBool("isMoving", false);
            else
                monster.animator.SetBool("isMoving", true);

            monster.RandomMoveCurrentTime = Random.Range(1f, 4f);
            monster.RandomMoveCurrentTime = 0f;
        }
    }
}

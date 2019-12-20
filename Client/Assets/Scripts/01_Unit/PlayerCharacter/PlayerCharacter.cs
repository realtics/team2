using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private CharacterMovement _movement;
    private Vector3 _oldDirection;

	public CharacterMovement Movement { get { return _movement; } }

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _oldDirection = Vector3.zero;
		PlayerManager.Instance.PlayerCharacter = this;
    }

    public void SetJump()
    {
        if (_movement.IsGround)
            _movement.SetJump();
    }

    public void SetAxis(float horizontal, float vertical)
    {
        if (horizontal < 0.0f)
            SetFlipX(true);
        else if (horizontal > 0.0f)
            SetFlipX(false);

        if (horizontal == 0.0f && vertical == 0.0f)
        {
            if (CheckDirection(horizontal, vertical))
            {
                SendMoveEnd(_movement.Forward, vertical);
            }
        }
        else
        {
            if (CheckDirection(horizontal, vertical))
            {
                SendMoveStart(horizontal, vertical);
            }
        }

        _movement.SetAxis(horizontal, vertical);
    }

    private void SetFlipX(bool flip)
    {
        if (!_movement.IsMovable())
            return;

        if (_movement.IsInTranstion)
            return;

        _movement.SetFlipX(flip);
    }

    public void SetRun()
    {
        _movement.SetRun();
    }

    public void StopRun()
    {
        _movement.StopRun();
    }

    public void SetAttack()
    {
        _movement.SetAttack();
    }

    public void SetSkill(SwordmanSkillIndex skill)
    {
        _movement.SetSkill(skill);
    }

    public void SetTest(AttackInfoSender sender)
    {
        _movement.OnHit(sender);
    }

    private bool CheckDirection(float horizontal, float vertical)
    {
        Vector3 dir = new Vector3(horizontal, vertical, 0);

        if (dir != _oldDirection)
        {
            _oldDirection = dir;
            return true;
        }

        return false;
    }

    private void SendMoveStart(float horizontal, float vertical)
    {
        if (NetworkManager.Instance == null)
            return;

        Vector3 dir = new Vector3(horizontal, vertical, 0.0f);
        //NetworkManager.Instance.MoveStart(transform.position, dir);
    }

    private void SendMoveEnd(float horizontal, float vertical)
    {
		if (NetworkManager.Instance == null)
			return;

		Vector3 dir = new Vector3(horizontal, vertical, 0.0f);
        NetworkManager.Instance.MoveEnd(transform.position, dir);
    }

    public void FindMovement()
    {
        _movement = GetComponent<CharacterMovement>();
    }
}

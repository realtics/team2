using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private CharacterMovement _movement;

    private void Start()
    {
        _movement = GetComponent<CharacterMovement>();
    }

    public void SetJump()
    {
        if (_movement.IsGround)
            _movement.SetJump();
    }

    public void SetAxis(float horizontal, float vertical)
    {
        if (horizontal < 0.0f)
            _movement.SetFlipX(true);
        else if (horizontal > 0.0f)
            _movement.SetFlipX(false);

        _movement.SetAxis(horizontal, vertical);
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
}

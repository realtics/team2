using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : BaseUnit
{
    private void Update()
    {
        SetAnimParameter();
    }
    private void SetAnimParameter()
    {
        _animator.SetBool("IsGround", IsGround);
        _animator.SetBool("IsRun", IsRun);
        _animator.SetBool("IsAttack", IsAttack);
        _animator.SetBool("IsMoving", IsMoving);
        _animator.SetBool("IsJumpUp", IsJumpUp);
        _animator.SetBool("IsJumpDown", IsJumpDown);
        _animator.SetBool("IsHit", IsHit);
    }


}

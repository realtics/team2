using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackEndChecker : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerManager.Instance.PlayerCharacter.Movement.StopAttack();
	}
}

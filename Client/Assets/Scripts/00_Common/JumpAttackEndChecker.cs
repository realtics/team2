using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackEndChecker : StateMachineBehaviour
{
	private BaseUnit _unit;
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_unit != null)
			return;

		_unit = animator.transform.root.GetComponent<BaseUnit>();
	}
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_unit.StopAttack();
	}
}

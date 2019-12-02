using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHitEffect : MonoBehaviour
{
	public void ClipEvent_DeadEffect()
	{
		gameObject.SetActive(false);
	}
}

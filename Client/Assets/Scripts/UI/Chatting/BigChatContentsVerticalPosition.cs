using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigChatContentsVerticalPosition : MonoBehaviour
{
	public void AddNetChatting(int count)
	{
		if (count <= 21)
			return;

		RectTransform rtTrf = (RectTransform)transform;
		Vector3 pos = rtTrf.localPosition;
		pos.y = (count - 21) * 37.0f + 5.0f;
		rtTrf.localPosition = pos;
	}
}

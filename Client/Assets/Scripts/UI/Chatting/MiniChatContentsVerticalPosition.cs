using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniChatContentsVerticalPosition : MonoBehaviour
{
	public void AddNetChatting(int count)
	{
		if (count <= 5)
			return;

		RectTransform rtTrf = (RectTransform)transform;
		Vector3 pos = rtTrf.localPosition;
		pos.y = (count - 4) * 40.0f + 5.0f;
		rtTrf.localPosition = pos;
	}
}

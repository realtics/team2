using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigChatContentsVerticalPosition : MonoBehaviour
{
	public void AddNetChatting(int count)
	{
		if (count <= 22)
			return;

		RectTransform rtTrf = (RectTransform)transform;
		Vector3 pos = rtTrf.localPosition;
		pos.y = (count - 22) * 35.0f + 5.0f;
		rtTrf.localPosition = pos;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
	public Text messageText;

	public void SetToastMessage(string message)
	{
		messageText.text = message;
	}

	public void AnimEvent_OffActiveSelf()
	{
		gameObject.SetActive(false);
	}
}

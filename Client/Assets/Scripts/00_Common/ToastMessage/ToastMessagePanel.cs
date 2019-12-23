using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastMessagePanel : MonoBehaviour
{
	private static ToastMessagePanel _instance;
	public static ToastMessagePanel Instance
	{
		get
		{
			return _instance;
		}
	}

	public GameObject toastMessagePrefab;

	private void Awake()
	{
		_instance = this;
	}

	public void SetToastMessage(string message)
	{
		GameObject toastObject = ObjectPoolManager.Instance.GetRestObject(toastMessagePrefab);
		ToastMessage toastText = toastObject.GetComponent<ToastMessage>();

		toastText.SetToastMessage(message);

		toastObject.transform.SetParent(transform);
		toastObject.transform.localScale = Vector3.one;
		toastObject.transform.localPosition = Vector3.zero;
		toastObject.transform.SetAsFirstSibling();
	}
}

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

	private List<string> _messages;

	private void Awake()
	{
		_instance = this;
		_messages = new List<string>();
	}

	private void Update()
	{
		ShowToastMessage();
	}

	private void ShowToastMessage()
	{
		if (_messages.Count <= 0)
			return;

		string toastMessage = _messages[0];

		GameObject toastObject = ObjectPoolManager.Instance.GetRestObject(toastMessagePrefab);
		ToastMessage toastText = toastObject.GetComponent<ToastMessage>();

		toastText.SetToastMessage(toastMessage);

		toastObject.transform.SetParent(transform);
		toastObject.transform.localScale = Vector3.one;
		toastObject.transform.localPosition = Vector3.zero;
		toastObject.transform.SetAsFirstSibling();

		_messages.Remove(toastMessage);
	}

	public void SetToastMessage(string message)
	{
		_messages.Add(message);
	}
}

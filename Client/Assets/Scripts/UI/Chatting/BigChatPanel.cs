using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigChatPanel : MonoBehaviour
{
	public BigChatContentsVerticalPosition bigChatContent;
	public Text bigChatText;

	private int _chatCount = 0;


	public void AddNewChatting(string nickName, string chatting)
	{
		bigChatText.text += $"[{nickName}] {chatting}\n";
		bigChatText.text.Replace("\\n", "\n");

		Vector3 chatHeight = ((RectTransform)bigChatText.transform).sizeDelta;
		chatHeight.y = 35.0f * (++_chatCount) + 5.0f;
		((RectTransform)bigChatText.transform).sizeDelta = chatHeight;

		bigChatContent.AddNetChatting(_chatCount);
	}
}

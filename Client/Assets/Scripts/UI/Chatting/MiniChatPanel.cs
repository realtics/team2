using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniChatPanel : MonoBehaviour
{
	public MiniChatContentsVerticalPosition miniChatContent;
	public Text miniChatText;

	private int _chatCount = 0;


	public void AddNewChatting(string nickName, string chatting)
	{
		miniChatText.text += $"[{nickName}] {chatting}\n";
		miniChatText.text.Replace("\\n", "\n");

		Vector3 chatHeight = ((RectTransform)miniChatText.transform).sizeDelta;
		chatHeight.y = 40.0f * (++_chatCount);
		((RectTransform)miniChatText.transform).sizeDelta = chatHeight;

		miniChatContent.AddNetChatting(_chatCount);
	}
}

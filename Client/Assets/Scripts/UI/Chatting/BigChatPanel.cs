using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigChatPanel : MonoBehaviour
{
	public ChattingPanel chattingPanel;
	public BigChatContentsVerticalPosition bigChatContent;
	public Text bigChatText;
	public InputField chattingField;

	private int _chatCount = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			SendChatButton();
		}
	}

	public void AddNewChatting(string nickName, string chatting)
	{
		bigChatText.text += $"[{nickName}] {chatting}\n";
		bigChatText.text.Replace("\\n", "\n");

		Vector3 chatHeight = ((RectTransform)bigChatText.transform).sizeDelta;
		chatHeight.y = 35.0f * (++_chatCount) + 5.0f;
		((RectTransform)bigChatText.transform).sizeDelta = chatHeight;

		bigChatContent.AddNetChatting(_chatCount);
	}

	public void SendChatButton()
	{
		if (chattingField.text == "")
			return;

		string chatting = chattingField.text;

		chattingPanel.AddNewChatting(PlayerManager.Instance.NickName, chatting);
		chattingField.text = "";
		NetworkManager.Instance.SendChat(chatting);
	}
}

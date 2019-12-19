using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ChattingData
{
	public string nickName;
	public string chatting;
}
public class ChattingPanel : MonoBehaviour
{
	private List<ChattingData> _chattings;

	private static ChattingPanel _instance;
	public static ChattingPanel Instance
	{
		get
		{
			return _instance;
		}
	}

	public MiniChatPanel minichatPanel;
	public BigChatPanel bigChatPanel;

	private void Awake()
	{
		_instance = this;
	}
	private void Update()
	{
		NewChatProcess();
	}

	private void NewChatProcess()
	{
		if (_chattings.Count <= 0)
			return;

		ChattingData chatData = _chattings[0];

		minichatPanel.AddNewChatting(chatData.nickName, chatData.chatting);
		bigChatPanel.AddNewChatting(chatData.nickName, chatData.chatting);
	}
	public void AddNewChatting(string nickName, string chatting)
	{
		ChattingData newChat = new ChattingData();
		newChat.nickName = nickName;
		newChat.chatting = chatting;
		_chattings.Add(newChat);
	}
}

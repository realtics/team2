using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChattingPanel : MonoBehaviour
{
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
	}
	public void AddNewChatting(string nickName, string chatting)
	{
		minichatPanel.AddNewChatting(nickName, chatting);
		bigChatPanel.AddNewChatting(nickName, chatting);
	}
}

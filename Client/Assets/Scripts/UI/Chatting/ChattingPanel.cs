using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChattingPanel : MonoBehaviour
{
	public MiniChatPanel minichatPanel;
	public BigChatPanel bigChatPanel;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			AddNewChatting("Wook", "AASSDASD");
		}
	}
	public void AddNewChatting(string nickName, string chatting)
	{
		minichatPanel.AddNewChatting(nickName, chatting);
		bigChatPanel.AddNewChatting(nickName, chatting);
	}
}

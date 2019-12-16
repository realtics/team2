using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    private static ChatManager _instance;
    public static ChatManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject ChatPrefab;

    private List<string> _chatList = new List<string>();

    public ChatScale miniChat;
    public ChatScale bigChat;
    public InputField chatField;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
    }

    public void AddNewChat(string nickname, string chat)
    {
        string newChat = $"{nickname} : {chat}";

        AddChatToPanel(miniChat, newChat);
        AddChatToPanel(bigChat, newChat);
    }

    private void AddChatToPanel(ChatScale parnet, string newChat)
    {
        Text chatText = Instantiate(ChatPrefab).GetComponent<Text>();
        chatText.text = newChat;

        chatText.transform.parent = parnet.transform;
        parnet.ResizeMinichatPanel();
    }

    public void SendChat()
    {
        if (chatField.text.Equals(""))
            return;

        AddNewChat("Me", chatField.text);
        NetworkManager.Instance.SendChat(chatField.text);
		chatField.text = "";
	}
}

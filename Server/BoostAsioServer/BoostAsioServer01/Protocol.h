#pragma once 

const unsigned short PORT_NUMBER = 31452;
const int MAX_RECEIVE_BUFFER_LEN = 512;
const int MAX_NAME_LEN = 13;
const int MAX_MESSAGE_LEN = 129;

const int MAX_PLAYER_MOVE_LEN = 50;

const int FIRST_SESSION_INDEX = 100;	// Ŭ���̾�Ʈ�� �˷��� ù��° ���� session Index

const int MAX_USER_ID = 50;
const int MAX_USER_PW = 200;
const int MAX_USER_NAME = 50;

enum PACKET_INDEX : short
{
	// REQ : Ŭ��->����, Ŭ�󿡼� ������ � ���� ��û
	// RES : ����->Ŭ��, ������ � ������ ����

	REQ_IN = 1,
	RES_IN = 2,
	REQ_CHAT = 5,
	NOTICE_CHAT = 6,
	
	REQ_CHECK_BEFORE_LOGIN = 105, 
	RES_CHECK_BEFORE_LOGIN = 106,

	REQ_NEW_LOGIN = 107,
	RES_NEW_LOGIN_SUCSESS = 108,
	
	REQ_CONCURRENT_USER = 110,
	RES_CONCURRENT_USER_LIST = 111,
	REQ_USER_EXIT = 112,
	RES_USER_EXIT = 113,

	REQ_CHATTING = 120,
	RES_CHATTING = 121,

	REQ_PLAYER_MOVE_START = 130,
	RES_PLAYER_MOVE_START = 131,
	REQ_PLAYER_MOVE_END = 132,
	RES_PLAYER_MOVE_END = 133,
};

enum CHECK_BEFORE_LOGIN_RESULT : int
{
	RESULT_SUCCESS = 1,
	RESULT_NO_ID = 2,
	RESULT_IS_WRONG_PASSWORD = 3,
};

struct PACKET_HEADER
{
	short packetIndex;
	short packetSize;
};

struct PACKET_HEADER_BODY
{
	PACKET_HEADER header;
};

struct PKT_REQ_IN : public PACKET_HEADER
{
	char characterName[MAX_NAME_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_IN;
		packetSize = sizeof(PKT_REQ_IN);
		memset(characterName, 0, MAX_NAME_LEN);
	}
};

struct PKT_RES_IN : public PACKET_HEADER
{
	bool isSuccess;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_IN;
		packetSize = sizeof(PKT_RES_IN);
		isSuccess = false;
	}
};

struct PKT_REQ_CHAT : public PACKET_HEADER
{
	char userMessage[MAX_MESSAGE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_CHAT;
		packetSize = sizeof(PKT_REQ_CHAT);
		memset(userMessage, 0, MAX_MESSAGE_LEN);
	}
};

struct PKT_NOTICE_CHAT : public PACKET_HEADER
{
	char characterName[MAX_NAME_LEN];
	char userMessage[MAX_MESSAGE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::NOTICE_CHAT;
		packetSize = sizeof(PKT_NOTICE_CHAT);
		memset(characterName, 0, MAX_NAME_LEN);
		memset(userMessage, 0, MAX_MESSAGE_LEN);
	}
};

struct PKT_REQ_CHECK_BEFORE_LOGIN : public PACKET_HEADER
{
	char userID[MAX_USER_ID];
	char userPW[MAX_USER_PW];
	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_CHECK_BEFORE_LOGIN;
		packetSize = sizeof(PKT_REQ_CHECK_BEFORE_LOGIN);
		memset(userID, 0, MAX_USER_ID);
		memset(userPW, 0, MAX_USER_PW);
	}
};

struct PKT_RES_CHECK_BEFORE_LOGIN : public PACKET_HEADER
{
	int checkResult;
	void Init()
	{
		packetIndex = PACKET_INDEX::RES_CHECK_BEFORE_LOGIN;
		packetSize = sizeof(PKT_RES_CHECK_BEFORE_LOGIN);
		checkResult = 0;
	}
};

struct PKT_REQ_NEW_LOGIN : public PACKET_HEADER
{
	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_NEW_LOGIN;
		packetSize = sizeof(PKT_REQ_NEW_LOGIN);
	}
};

struct PKT_RES_NEW_LOGIN_SUCSESS : public PACKET_HEADER
{
	bool isSuccess;
	int sessionID;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_NEW_LOGIN_SUCSESS;
		packetSize = sizeof(PKT_RES_NEW_LOGIN_SUCSESS);
		isSuccess = true;
		sessionID = FIRST_SESSION_INDEX;
	}
};

struct PKT_REQ_CONCURRENT_USER : public PACKET_HEADER
{
	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_CONCURRENT_USER;
		packetSize = sizeof(PKT_REQ_CONCURRENT_USER);
	}
};

struct PKT_RES_CONCURRENT_USER_LIST : public PACKET_HEADER
{
	int totalUser;
	std::string concurrentUserList;
	std::string userPos;
	std::string userDir;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_CONCURRENT_USER_LIST;
		packetSize = sizeof(PKT_RES_CONCURRENT_USER_LIST);
		totalUser = 0;
	}
};

struct PKT_REQ_USER_EXIT : public PACKET_HEADER
{
	int sessionID;

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_USER_EXIT;
		packetSize = sizeof(PKT_REQ_USER_EXIT);
		sessionID = FIRST_SESSION_INDEX;
	}
};

struct PKT_RES_USER_EXIT : public PACKET_HEADER
{
	int sessionID;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_USER_EXIT;
		packetSize = sizeof(PKT_RES_USER_EXIT);
		sessionID = FIRST_SESSION_INDEX;
	}
};

struct PKT_REQ_CHATTING : public PACKET_HEADER
{
	int userID;
	char chatMessage[MAX_MESSAGE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_CHATTING;
		packetSize = sizeof(PKT_REQ_CHATTING);
		userID = FIRST_SESSION_INDEX;
		memset(chatMessage, 0, MAX_MESSAGE_LEN);
	}
};

struct PKT_RES_CHATTING : public PACKET_HEADER
{
	int userID;
	char userName[MAX_NAME_LEN];
	char chatMessage[MAX_MESSAGE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_CHATTING;
		packetSize = sizeof(PKT_REQ_CHATTING);
		userID = FIRST_SESSION_INDEX;
		memset(userName, 0, MAX_NAME_LEN);
		memset(chatMessage, 0, MAX_MESSAGE_LEN);
	}
};

struct PKT_REQ_PLAYER_MOVE_START : public PACKET_HEADER
{
	int sessionID;
	char userPos[MAX_PLAYER_MOVE_LEN];
	char userDir[MAX_PLAYER_MOVE_LEN];
	
	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_PLAYER_MOVE_START;
		packetSize = sizeof(PKT_REQ_PLAYER_MOVE_START);
		sessionID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
		memset(userDir, 0, MAX_PLAYER_MOVE_LEN);
	}
};

struct PKT_RES_PLAYER_MOVE_START : public PACKET_HEADER
{
	int sessionID;
	char userPos[MAX_PLAYER_MOVE_LEN];
	char userDir[MAX_PLAYER_MOVE_LEN];
	
	void Init()
	{
		packetIndex = PACKET_INDEX::RES_PLAYER_MOVE_START;
		packetSize = sizeof(PKT_RES_PLAYER_MOVE_START);
		sessionID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
		memset(userDir, 0, MAX_PLAYER_MOVE_LEN);
	}
};

struct PKT_REQ_PLAYER_MOVE_END : public PACKET_HEADER
{
	int sessionID;
	char userPos[MAX_PLAYER_MOVE_LEN];
	char userDir[MAX_PLAYER_MOVE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_PLAYER_MOVE_END;
		packetSize = sizeof(PKT_REQ_PLAYER_MOVE_START);
		sessionID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
		memset(userDir, 0, MAX_PLAYER_MOVE_LEN);
	}
};

struct PKT_RES_PLAYER_MOVE_END : public PACKET_HEADER
{
	int sessionID;
	char userPos[MAX_PLAYER_MOVE_LEN];
	char userDir[MAX_PLAYER_MOVE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_PLAYER_MOVE_END;
		packetSize = sizeof(PKT_RES_PLAYER_MOVE_END);
		sessionID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
		memset(userDir, 0, MAX_PLAYER_MOVE_LEN);
	}
};
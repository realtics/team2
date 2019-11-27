#pragma once 

const unsigned short PORT_NUMBER = 31452;
const int MAX_RECEIVE_BUFFER_LEN = 512;
const int MAX_NAME_LEN = 13;
const int MAX_MESSAGE_LEN = 129;

const int MAX_PLAYER_MOVE_LEN = 50;

const int FIRST_USER_INDEX = 100;		// 첫번째 유저 Index (클라이언트에 알려줄 index)

enum PACKET_INDEX
{
	// REQ : 클라->서버, 클라에서 서버에 어떤 값을 요청
	// RES : 서버->클라, 서버가 어떤 값으로 응답

	REQ_IN = 1,
	RES_IN = 2,
	REQ_CHAT = 5,
	NOTICE_CHAT = 6,
	
	REQ_NEW_LOGIN = 100,
	RES_NEW_LOGIN_SUCSESS = 101,

	REQ_CONCURRENT_USER = 110,
	RES_CONCURRENT_USER_LIST = 111,

	JOIN_PLAYER = 120,

	REQ_PLAYER_MOVE_START = 130,
	RES_PLAYER_MOVE_START = 131,
	REQ_PLAYER_MOVE_END = 132,
	RES_PLAYER_MOVE_END = 133,
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
	int userID;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_NEW_LOGIN_SUCSESS;
		packetSize = sizeof(PKT_RES_NEW_LOGIN_SUCSESS);
		isSuccess = false;
		userID = 0;
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

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_CONCURRENT_USER_LIST;
		packetSize = sizeof(PKT_RES_CONCURRENT_USER_LIST);
		totalUser = 0;
	}
};

struct PKT_REQ_PLAYER_MOVE_START : public PACKET_HEADER
{
	int userID;
	char userPos[MAX_PLAYER_MOVE_LEN];
	char userDir[MAX_PLAYER_MOVE_LEN];
	
	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_PLAYER_MOVE_START;
		packetSize = sizeof(PKT_REQ_PLAYER_MOVE_START);
		userID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
		memset(userDir, 0, MAX_PLAYER_MOVE_LEN);
	}
};

struct PKT_RES_PLAYER_MOVE_START : public PACKET_HEADER
{
	int userID;
	char userPos[MAX_PLAYER_MOVE_LEN];
	char userDir[MAX_PLAYER_MOVE_LEN];
	
	void Init()
	{
		packetIndex = PACKET_INDEX::RES_PLAYER_MOVE_START;
		packetSize = sizeof(PKT_RES_PLAYER_MOVE_START);
		userID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
		memset(userDir, 0, MAX_PLAYER_MOVE_LEN);
	}
};

struct PKT_REQ_PLAYER_MOVE_END : public PACKET_HEADER
{
	int userID;
	char userPos[MAX_PLAYER_MOVE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_PLAYER_MOVE_END;
		packetSize = sizeof(PKT_REQ_PLAYER_MOVE_START);
		userID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
	}
};

struct PKT_RES_PLAYER_MOVE_END : public PACKET_HEADER
{
	int userID;
	char userPos[MAX_PLAYER_MOVE_LEN];

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_PLAYER_MOVE_END;
		packetSize = sizeof(PKT_RES_PLAYER_MOVE_END);
		userID = 0;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
	}
};
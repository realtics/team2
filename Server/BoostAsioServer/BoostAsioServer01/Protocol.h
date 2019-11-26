#pragma once 

const unsigned short PORT_NUMBER = 31452;
const int MAX_RECEIVE_BUFFER_LEN = 512;
const int MAX_NAME_LEN = 13;
const int MAX_MESSAGE_LEN = 129;

const int FIRST_USER_INDEX = 100;		// 첫번째 유저 Index (클라이언트에 알려줄 index)

enum PACKET_INDEX
{
	REQ_IN = 1,
	RES_IN = 2,
	REQ_CHAT = 5,
	NOTICE_CHAT = 6,
	
	NEW_LOGIN = 7,
	NEW_LOGIN_SUCSESS = 8,

	CONCURRENT_USERS = 10,

	JOIN_PLAYER = 20,

	PLAYER_MOVE_START = 30,
	PLAYER_MOVE_END = 31,
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


struct PACKET_NEW_LOGIN : public PACKET_HEADER
{
	void Init()
	{
		packetIndex = PACKET_INDEX::NEW_LOGIN;
		packetSize = sizeof(PACKET_NEW_LOGIN);
	}
};

struct PACKET_NEW_LOGIN_SUCSESS : public PACKET_HEADER
{
	bool isSuccess;
	int userID;

	void Init()
	{
		packetIndex = PACKET_INDEX::NEW_LOGIN_SUCSESS;
		packetSize = sizeof(PACKET_NEW_LOGIN_SUCSESS);
	}
};

struct PACKET_CONCURRENT_USERS : public PACKET_HEADER
{
	int totalUsers;
	std::string concurrentUsersList;

	void Init()
	{
		packetIndex = PACKET_INDEX::CONCURRENT_USERS;
		packetSize = sizeof(PACKET_CONCURRENT_USERS);
	}
};
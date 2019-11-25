#pragma once 
const unsigned short PORT_NUMBER = 31452;
const int MAX_RECEIVE_BUFFER_LEN = 512;
const int MAX_NAME_LEN = 13;
const int MAX_MESSAGE_LEN = 129;

enum PACKET_INDEX
{
	REQ_IN = 1,
	RES_IN = 2,
	REQ_CHAT = 5,
	NOTICE_CHAT = 6,
	
	NEW_LOGIN = 7,
	NEW_LOGIN_SUCSESS = 8,

	JOIN_PLAYER = 2001,

	PLAYER_MOVE_START = 3001,
	PLAYER_MOVE_END = 3002,
};

struct PACKET_HEADER
{
	short packetIndex;
	short packetSize;
};

struct PACKET_CHARACTER_MOVE
{
	PACKET_HEADER header;
	float characterMoveX;
	float characterMoveY;
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
	int playerID;

	void Init()
	{
		packetIndex = PACKET_INDEX::NEW_LOGIN_SUCSESS;
		packetSize = sizeof(PACKET_NEW_LOGIN_SUCSESS);
	}
};
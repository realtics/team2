#pragma once 
const unsigned short PORT_NUMBER = 31452;
const int MAX_RECEIVE_BUFFER_LEN = 512;
const int MAX_NAME_LEN = 13;
const int MAX_MESSAGE_LEN = 129;

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

//  ÆÐÅ¶
const short REQ_IN = 1;
// PKT_REQ_IN

const short RES_IN = 2;
// PKT_RES_IN

const short REQ_CHAT = 6;
// PKT_REQ_CHAT

const short NOTICE_CHAT = 7;
// PKT_NOTICE_CHAT

struct PKT_REQ_IN : public PACKET_HEADER
{
	char characterName[MAX_NAME_LEN];

	void Init()
	{
		packetIndex = REQ_IN;
		packetSize = sizeof(PKT_REQ_IN);
		memset(characterName, 0, MAX_NAME_LEN);
	}
};

struct PKT_RES_IN : public PACKET_HEADER
{
	bool isSuccess;

	void Init()
	{
		packetIndex = RES_IN;
		packetSize = sizeof(PKT_RES_IN);
		isSuccess = false;
	}
};

struct PKT_REQ_CHAT : public PACKET_HEADER
{
	char userMessage[MAX_MESSAGE_LEN];

	void Init()
	{
		packetIndex = REQ_CHAT;
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
		packetIndex = NOTICE_CHAT;
		packetSize = sizeof(PKT_NOTICE_CHAT);
		memset(characterName, 0, MAX_NAME_LEN);
		memset(userMessage, 0, MAX_MESSAGE_LEN);
	}
};
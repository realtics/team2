#pragma once 

#include <string>
#include <vector>
#include <list>

const unsigned short PORT_NUMBER = 31452;
const int MAX_RECEIVE_BUFFER_LEN = 512;

const int MAX_MESSAGE_LEN = 129;

const int MAX_PLAYER_MOVE_LEN = 50;

const int FIRST_SESSION_INDEX = 100;	// 클라이언트에 알려줄 첫번째 유저 session Index

const int MAX_USER_ID = 50;
const int MAX_USER_PW = 200;
const int MAX_USER_NAME = 50;

const int MAX_RESULT_ITEM_ID = 50;

const int MAX_USER_ITEM_LEN = 5;
const int MAX_USER_EQUIP = 12;
const int MAX_USER_INVENTORY = 25;

const int MAX_INVENTORY_COLUMN = 15;

enum DB_INDEX : int
{
	DB_INDEX_RESULT_ITEMS = 1000,
	DB_INDEX_RESULT_ITEMS_START = 1001,

	DB_INDEX_HELL_ITEMS = 6000,
	DB_INDEX_HELL_ITEMS_START = 6001,
};

enum PACKET_INDEX : short
{
	// REQ : 클라->서버, 클라에서 서버에 어떤 값을 요청
	// RES : 서버->클라, 서버가 어떤 값으로 응답
	
	REQ_SIGN_UP = 103,
	RES_SIGN_UP = 104,

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

	REQ_DUNGEON_CLEAR_RESULT_ITEM = 301,
	RES_DUNGEON_CLEAR_RESULT_ITEM = 302,

	REQ_DUNGEON_HELL_RESULT_ITEM = 351,
	RES_DUNGEON_HELL_RESULT_ITEM = 352,

	REQ_DUNGEON_HELL_ITEM_PICK_UP = 361,

	REQ_INVENTORY_OPEN = 501,
	RES_INVENTORY_OPEN = 502,

	REQ_INVENTORY_CLOSE = 511,
	RES_INVENTORY_CLOSE = 512,
};

enum WORLD_ZONE
{
	WORLD_ZONE_LOBBY,
	WORLD_ZONE_DUNGEON,
};

enum RESULT_SIGN_UP_CHECK : int
{
	RESULT_SIGN_UP_CHECK_SUCCESS = 1,
	RESULT_SIGN_UP_OVERLAP_ID = 2,

	RESULT_SIGN_UP_UNKNOWN = -1,
};

enum RESULT_INVENTORY_CHECK : int
{
	RESULT_INVENTORY_CHECK_SUCCESS = 1,

	RESULT_INVENTORY_CHECK_UNKNOWN = -1,
};

enum RESULT_BEFORE_LOGIN_CHECK : int
{
	RESULT_BEFORE_LOGIN_CHECK_SUCCESS = 1,
	RESULT_BEFORE_LOGIN_CHECK_NO_ID = 2,
	RESULT_BEFORE_LOGIN_CHECK_IS_WRONG_PASSWORD = 3,

	RESULT_BEFORE_LOGIN_CHECK_UNKNOWN = -1,
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

struct PKT_REQ_SIGN_UP : public PACKET_HEADER
{
	char userID[MAX_USER_ID];
	char userPW[MAX_USER_PW];
	char userName[MAX_USER_NAME];
	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_SIGN_UP;
		packetSize = sizeof(PKT_REQ_SIGN_UP);
		memset(userID, 0, MAX_USER_ID);
		memset(userPW, 0, MAX_USER_PW);
		memset(userName, 0, MAX_USER_NAME);
	}
};

struct PKT_RES_SIGN_UP : public PACKET_HEADER
{
	int checkResult;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_SIGN_UP;
		packetSize = sizeof(PKT_RES_SIGN_UP);
		checkResult = 0;
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
	char userID[MAX_USER_ID];
	int sessionID;
	std::string userName;
	void Init()
	{
		packetIndex = PACKET_INDEX::RES_CHECK_BEFORE_LOGIN;
		packetSize = sizeof(PKT_RES_CHECK_BEFORE_LOGIN);
		checkResult = 0;
		memset(userID, 0, MAX_USER_ID);
		sessionID = FIRST_SESSION_INDEX;
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
	std::string userName;
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
	int sessionID;
	std::string chatMessage;

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_CHATTING;
		packetSize = sizeof(PKT_REQ_CHATTING);
		sessionID = FIRST_SESSION_INDEX;
	}
};

struct PKT_RES_CHATTING : public PACKET_HEADER
{
	int sessionID;
	std::string userName;
	std::string chatMessage;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_CHATTING;
		packetSize = sizeof(PKT_RES_CHATTING);
		sessionID = FIRST_SESSION_INDEX;
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
		sessionID = FIRST_SESSION_INDEX;
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
		sessionID = FIRST_SESSION_INDEX;
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
		sessionID = FIRST_SESSION_INDEX;
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
		sessionID = FIRST_SESSION_INDEX;
		memset(userPos, 0, MAX_PLAYER_MOVE_LEN);
		memset(userDir, 0, MAX_PLAYER_MOVE_LEN);
	}
};

struct PKT_REQ_DUNGEON_CLEAR_RESULT_ITEM : PACKET_HEADER
{
	char userID[MAX_USER_ID];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_DUNGEON_CLEAR_RESULT_ITEM;
		packetSize = sizeof(PKT_REQ_DUNGEON_CLEAR_RESULT_ITEM);
		memset(userID, 0, MAX_USER_ID);
	}
};

struct PKT_RES_DUNGEON_CLEAR_RESULT_ITEM : PACKET_HEADER
{
	std::string itemID;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_DUNGEON_CLEAR_RESULT_ITEM;
		packetSize = sizeof(PKT_RES_DUNGEON_CLEAR_RESULT_ITEM);
	}
};

struct PKT_REQ_DUNGEON_HELL_RESULT_ITEM : PACKET_HEADER
{
	char userID[MAX_USER_ID];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_DUNGEON_HELL_RESULT_ITEM;
		packetSize = sizeof(PKT_REQ_DUNGEON_HELL_RESULT_ITEM);
		memset(userID, 0, MAX_USER_ID);
	}
};

struct PKT_RES_DUNGEON_HELL_RESULT_ITEM : PACKET_HEADER
{
	std::string itemID;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_DUNGEON_HELL_RESULT_ITEM;
		packetSize = sizeof(PKT_RES_DUNGEON_HELL_RESULT_ITEM);
	}
};

struct PKT_REQ_DUNGEON_HELL_ITEM_PICK_UP : PACKET_HEADER
{
	char userID[MAX_USER_ID];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_DUNGEON_HELL_ITEM_PICK_UP;
		packetSize = sizeof(PKT_REQ_DUNGEON_HELL_ITEM_PICK_UP);
		memset(userID, 0, MAX_USER_ID);
	}
};

struct PKT_REQ_INVENTORY_OPEN : PACKET_HEADER
{
	char userID[MAX_USER_ID];

	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_INVENTORY_OPEN;
		packetSize = sizeof(PKT_REQ_INVENTORY_OPEN);
		memset(userID, 0, MAX_USER_ID);
	}
};

struct PKT_RES_INVENTORY_OPEN : PACKET_HEADER
{
	char equip[MAX_USER_EQUIP][MAX_USER_ITEM_LEN];
	char inventory[MAX_USER_INVENTORY][MAX_USER_ITEM_LEN];
		
	void Init()
	{
		packetIndex = PACKET_INDEX::RES_INVENTORY_OPEN;
		packetSize = sizeof(PKT_RES_INVENTORY_OPEN);
		memset(equip, 0, sizeof(equip));
		memset(inventory, 0, sizeof(inventory));
	}
};

struct PKT_REQ_INVENTORY_CLOSE : PACKET_HEADER
{
	char userID[MAX_USER_ID];
	int userExo;
	char equip[MAX_USER_EQUIP][MAX_USER_ITEM_LEN];
	char inventory[MAX_USER_INVENTORY][MAX_USER_ITEM_LEN];
	
	void Init()
	{
		packetIndex = PACKET_INDEX::REQ_INVENTORY_CLOSE;
		packetSize = sizeof(PKT_REQ_INVENTORY_CLOSE);
		memset(userID, 0, MAX_USER_ID);
		memset(equip, 0, sizeof(equip));
		memset(inventory, 0, sizeof(inventory));
		userExo = 0;
	}
};

struct PKT_RES_INVENTORY_CLOSE : public PACKET_HEADER
{
	int checkResult;

	void Init()
	{
		packetIndex = PACKET_INDEX::RES_INVENTORY_CLOSE;
		packetSize = sizeof(PKT_RES_INVENTORY_CLOSE);
		checkResult = 0;
	}
};
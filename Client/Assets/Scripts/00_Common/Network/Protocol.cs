using System.Collections;
using System.Collections.Generic;
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

enum RESULT_SIGN_UP_CHECK : int
{
	RESULT_SIGN_UP_CHECK_SUCCESS = 1,
	RESULT_SIGN_UP_OVERLAP_ID = 2,

	RESULT_SIGN_UP_UNKNOWN = -1,
}
enum CHECK_BEFORE_LOGIN_RESULT : int
{
	RESULT_BEFORE_LOGIN_CHECK_SUCCESS = 1,
	RESULT_BEFORE_LOGIN_CHECK_NO_ID = 2,
	RESULT_BEFORE_LOGIN_CHECK_IS_WRONG_PASSWORD = 3,

	RESULT_BEFORE_LOGIN_CHECK_UNKNOWN = -1,
}

public struct PACKET_HEADER
{
	public short packetIndex;
	public short packetSize;
};

public struct PACKET_HEADER_BODY
{
	public PACKET_HEADER header;
};

public struct PKT_REQ_SIGN_UP
{
	public PACKET_HEADER header;
	public string userID;
	public string userPW;
	public string userName;
}
public struct PKT_RES_SIGN_UP
{
	public PACKET_HEADER header;
	public int checkResult;
}

public struct PKT_REQ_CHECK_BEFORE_LOGIN
{
	public PACKET_HEADER header;
	public string userID;
	public string userPW;
}
public struct PKT_RES_CHECK_BEFORE_LOGIN
{
	public PACKET_HEADER header;
	public int checkResult;
	public int sessionID;
	public string userName;
}

public struct PKT_REQ_NEW_LOGIN
{
	public PACKET_HEADER header;
};

public struct PKT_RES_NEW_LOGIN_SUCSESS
{
	public PACKET_HEADER header;
	public bool isSuccess;
	public int sessionID;
};

public struct PKT_REQ_CONCURRENT_USER
{
	public PACKET_HEADER header;
};

public struct PKT_RES_CONCURRENT_USER_LIST
{
	public PACKET_HEADER header;
	public int totalUser;
	public string concurrentUser;
	public string userName;
	public string userPos;
	public string userDir;
};

public struct PKT_REQ_USER_EXIT
{
	public PACKET_HEADER header;
	public int sessionID;
}
public struct PKT_RES_USER_EXIT
{
	public PACKET_HEADER header;
	public int sessionID;
}

public struct PKT_REQ_CHATTING
{
	public PACKET_HEADER header;
	public int sessionID;
	public string chatMessage;
}

public struct PKT_RES_CHATTING
{
	public PACKET_HEADER header;
	public int sessionID;
	public string userName;
	public string chatMessage;
}

public struct PKT_REQ_PLAYER_MOVE_START
{
	public PACKET_HEADER header;
	public int sessionID;
	public string userPos;
	public string userDir;
}

public struct PKT_RES_PLAYER_MOVE_START
{
	public PACKET_HEADER header;
	public int sessionID;
	public string userPos;
	public string userDir;
}

public struct PKT_REQ_PLAYER_MOVE_END
{
	public PACKET_HEADER header;
	public int sessionID;
	public string userPos;
	public string userDir;
}

public struct PKT_RES_PLAYER_MOVE_END
{
	public PACKET_HEADER header;
	public int sessionID;
	public string userPos;
	public string userDir;
}

public struct PKT_REQ_DUNGEON_CLEAR_RESULT_ITEM
{
	public PACKET_HEADER header;
	public string userID;
}

public struct PKT_RES_DUNGEON_CLEAR_RESULT_ITEM
{
	public PACKET_HEADER header;
	public string itemID;
}

public struct PKT_REQ_DUNGEON_HELL_RESULT_ITEM
{
	public PACKET_HEADER header;
	public string userID;
}

public struct PKT_RES_DUNGEON_HELL_RESULT_ITEM
{
	public PACKET_HEADER header;
	public string itemID;
}

public struct PKT_REQ_DUNGEON_HELL_ITEM_PICK_UP
{
	public PACKET_HEADER header;
	public string userID;
}
public struct PKT_REQ_INVENTORY_OPEN
{
	public PACKET_HEADER header;
	public string userID;
}

public struct PKT_RES_INVENTORY_OPEN
{
	public PACKET_HEADER header;
	public List<string> equip;
	public List<string> inventory;
}

public struct PKT_REQ_INVENTORY_CLOSE
{
	public PACKET_HEADER header;
	public string userID;
	public List<string> equip;
	public List<string> inventory;
	public int userExo;
}

public struct PKT_RES_INVENTORY_CLOSE
{
	public PACKET_HEADER header;
	public int checkResult;
}



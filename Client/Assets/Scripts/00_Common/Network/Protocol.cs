enum PACKET_INDEX : short
{
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
	public int userID;
	public string chatMessage;
}

public struct PKT_RES_CHATTING
{
	public PACKET_HEADER header;
	public int userID;
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


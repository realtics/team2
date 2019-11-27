enum PACKET_INDEX
{
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

public struct PACKET_HEADER
{
    public short packetIndex;
    public short packetSize;
};

public struct PACKET_HEADER_BODY
{
    public PACKET_HEADER header;
};

public struct PKT_REQ_NEW_LOGIN
{
    public PACKET_HEADER header;
};

public struct PKT_RES_NEW_LOGIN_SUCSESS
{
    public PACKET_HEADER header;
    public bool isSuccess;
    public int userID;
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
};

public struct PKT_REQ_PLAYER_MOVE_START
{
    public PACKET_HEADER header;
    public int userID;
    public string userPos;
    public string userDir;
}

public struct PKT_RES_PLAYER_MOVE_START
{
    public PACKET_HEADER header;
    public int userID;
    public string userPos;
    public string userDir;
}

public struct PKT_REQ_PLAYER_MOVE_END
{
    public PACKET_HEADER header;
    public int userID;
    public string userPos;
}

public struct PKT_RES_PLAYER_MOVE_END
{
    public PACKET_HEADER header;
    public int userID;
    public string userPos;
}


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

public struct PACKET_HEADER
{
    public short packetIndex;
    public short packetSize;
};

public struct PACKET_HEADER_BODY
{
    public PACKET_HEADER header;
};

public struct PACKET_NEW_LOGIN
{
    public PACKET_HEADER header;
};

public struct PACKET_NEW_LOGIN_SUCSESS
{
    public PACKET_HEADER header;
    public bool isSuccess;
    public int userID;
}

public struct PACKET_CONCURRENT_USERS
{
    public PACKET_HEADER header;
    public int totalUsers;
    public string concurrentUsers;
}
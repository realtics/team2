using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System;

using Newtonsoft.Json;


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

public struct PACKET_HEADER
{
    public short packetIndex;
    public short packetSize;
};

public struct PACKET_NEW_LOGIN
{
    public PACKET_HEADER header;
};


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject playerPrefab;

    private Socket _sock = null;
    private bool _isLogin = false;

    private int _myId;
    private Dictionary<int,Character> _characters;

    public bool IsLogin { get { return _isLogin; } }
    public bool LoginSuccess { set { _isLogin = true; } }
    public int MyId { get { return _myId; } }

    void Start()
    {
        _instance = this;
        _characters = new Dictionary<int, Character>();

        CreateSocket();

        if(IsLogin == false)
        {
            NewLogin();
        }

        // 내 캐릭터를 생성하는 로직
        // 서버에서 내가 접속했다고 알려주면 Id를 받고 내 Id로 설정한다.
        JoinNewPlayer(1);
        SetMyId(1);
    }

    void Update()
    {
        // 서버에서 새로운 플레이어가 접속했다고 알려주는 역할임
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 새로운 플레이어의 Id를 넣어주고 생성함.
            JoinNewPlayer(2);
        }

        // 서버가 어떤 플레이어가 움직였다 라고 알림
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ReceivedPacketHandler();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ThisIsStopPaket();
        }
    }

    private void CreateSocket()
    {
        _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        if (_sock == null)
        {
            Debug.Log("소켓 생성 실패");
        }
        _sock.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 31452));
    }

    private void NewLogin()
    {
        string _jsonData;
        char _endNullValue = '\0';

        var _packHeader = new PACKET_HEADER { packetIndex = (short)PACKET_INDEX.NEW_LOGIN, 
                                             packetSize = 45 };
        var _packData = new PACKET_NEW_LOGIN { header = _packHeader };
        _jsonData = JsonConvert.SerializeObject(_packData);
        _jsonData += _endNullValue;

        byte[] _sendByte = new byte[128];
        _sendByte = Encoding.UTF8.GetBytes(_jsonData);
        _sock.Send(_sendByte);
    }

    public void SetMyId(int id)
    {
        _myId = id;
    }

    public void JoinNewPlayer(int id)
    {
        Character newPlayer;

        // Instantiate는 게임 오브젝트를 씬에 생성하는 함수
        // 코스트가 커서 자주 사용하면 좋지 않아 보통은 ObjectPool을 사용한다.
        // 생성 된 캐릭터에게 자신의 Id를 알려준다.
        // 패킷을 보낼 때 어떤 플레이어가 보냈는지 알려주기 위해 고유 Id를 함께 넘겨줘야한다.

        newPlayer = Instantiate(playerPrefab).GetComponent<Character>();
        newPlayer.transform.position = Vector3.zero;
        newPlayer.SetId(id);

        _characters.Add(id, newPlayer);
    }

    // 원래는 패킷마다 핸들러를 만들어 사용해야함
    private void ReceivedPacketHandler()
    {
        // id가 2인 플레이어가 1,0,0 방향벡터로 움직였다고 가정
        Character movePlayer;
        _characters.TryGetValue(2, out movePlayer);

        // 무언가 받아온 다음 null 체크는 필수
        // Dictinary에서 받아오는 것이기 때문에
        if (!_characters.ContainsKey(2))
            return;
        // 같은 방식도 괜찮다 둘중에 아무거나 하면 됨
        if (movePlayer == null)
        {
            Debug.LogError("그런 플레이어는 존재하지 않아요! id = " + 1);
            return;
        }

        movePlayer.SetMoveDirectionAndMove(Vector3.left);
    }

    private void ThisIsStopPaket()
    {
        // 이것도 위의 함수와 동일함
        // MoveEnd 패킷이 왔고 마지막 좌표가 담겨있다고 가정

        Character movePlayer;
        _characters.TryGetValue(2, out movePlayer);

        if (!_characters.ContainsKey(2))
            return;

        movePlayer.StopMove(movePlayer.transform.position);
    }
}

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

public struct PACKET_NEW_LOGIN_SUCSESS
{
    public PACKET_HEADER header;
    public bool isSuccess;
    public int sessionID;
}


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
    private byte[] _buffer;

    private bool _isLogin = false;
    private int _myId;
    private Dictionary<int,Character> _characters;

    public bool IsLogin { get { return _isLogin; } }
    public bool LoginSuccess { set { _isLogin = true; } }
    public int MyId { get { return _myId; } }

    void Start()
    {
        Screen.SetResolution(960, 540, false);

        _instance = this;
        _characters = new Dictionary<int, Character>();

        CreateSocket();

        if (IsLogin == false)
        {
            NewLogin();

            NewLoginSucsess();

            StartReceiving();
        }

        // 내 캐릭터를 생성하는 로직
        // 서버에서 내가 접속했다고 알려주면 Id를 받고 내 Id로 설정한다.
        JoinNewPlayer(MyId);
    }

    void Update()
    {


        // 서버에서 새로운 플레이어가 접속했다고 알려주는 역할임
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 새로운 플레이어의 Id를 넣어주고 생성함.
            JoinNewPlayer(MyId);
        }

        // 서버가 어떤 플레이어가 움직였다 라고 알림
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ReceivedPacketHandler();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ThisIsStopPacket();
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
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER { packetIndex = (short)PACKET_INDEX.NEW_LOGIN, 
                                             packetSize = 45 };
        var packData = new PACKET_NEW_LOGIN { header = packHeader };
        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;
        byte[] sendByte = new byte[128];
        sendByte = Encoding.UTF8.GetBytes(jsonData);
        //TODO 1-0: JSON 헤더에 패킷 사이즈 체크 하는것을 foreach로 하고 있는데, 더 좋은 방법 있다면 개선
        //TODO 1-1: 패킷 사이즈를 담아 보내는 것이 현재 상태에선 크게 중요하진 않으므로, 코드만 남겨두고 나중에 활용
        //int jsonDataSize = 0;
        //foreach (byte b in _sendByte)
        //{
        //    jsonDataSize++;
        //    if (b == '\0')
        //        break;
        //}
        //Debug.Log(jsonData);
        //Debug.Log(jsonDataSize);
        int resultSize = _sock.Send(sendByte);
    }

    private void NewLoginSucsess()
    {
        byte[] recvBuf = new byte[128];
        int socketReceive = _sock.Receive(recvBuf);
        Debug.Log(socketReceive);

        string recvData = Encoding.UTF8.GetString(recvBuf, 0, socketReceive);
        int bufLen = recvBuf.Length;
        Debug.Log(recvData);

        var Jsondata = JsonConvert.DeserializeObject<PACKET_NEW_LOGIN_SUCSESS>(recvData);
        if (Jsondata.header.packetIndex == (short)PACKET_INDEX.NEW_LOGIN_SUCSESS)
        {
            Debug.Log("접속 성공 여부 : " + Jsondata.isSuccess);
            Debug.Log("접속 ID : " + Jsondata.sessionID);

            SetIsLogin(Jsondata.isSuccess);
            SetMyId(Jsondata.sessionID);
        }
        
    }

    public void StartReceiving()
    {
        try
        {
            _buffer = new byte[128];
            _sock.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
        }
        catch { }
    }

    private void ReceiveCallback(IAsyncResult AR)
    {
        try
        {
            // 클라이언트가 서버에서 연결을 끊을 때 바이트가 1보다 작은 경우 발생합니다.
            // 현재 클라이언트에서 연결 끊기 기능을 실행합니다.
            if (_sock.EndReceive(AR) > 1)
            {
                //// 수신 한 처음 4 바이트 (int 32)를 Int32 (이것은 다가오는 데이터의 크기)로 변환합니다.
                //_buffer = new byte[BitConverter.ToInt32(_buffer, 0)];
                //// 다음으로 이 데이터를 이전에 받은 크기로 버퍼에 수신합니다.
                //_sock.Receive(_buffer, _buffer.Length, SocketFlags.None);
                //// 전송 한 데이터로 변환하기 위해 모든 것을 수신했을 때.
                //// 예를 들어 string, int 등 ...이 예제에서는 문자열을 보내고 받는 구현 만 사용합니다.

                //// 바이트를 문자열로 변환하여 메시지 상자에 출력
                //string data = Encoding.Default.GetString(_buffer);
                //Debug.Log(data);
                //// 이제 소켓에서 데이터가 올 때까지 기다렸다가 다시 시작해야합니다.
                //StartReceiving();



                //◆ 이 부분 코드 잘못 됨 수정 (byte 문제)
                //ader":{"packetIndex":"8","packetSize":"10"},"isSuccess":"true","sessionID":"1002"}
                _buffer = new byte[128];
                int socketReceive = _sock.Receive(_buffer, _buffer.Length, SocketFlags.None);
                Debug.Log(socketReceive);

                //string data = Encoding.Default.GetString(_buffer);
                string recvData = Encoding.UTF8.GetString(_buffer, 0, socketReceive);
                Debug.Log(recvData);
                int bufLen = _buffer.Length;
                Debug.Log(bufLen);

                var JsonData = JsonConvert.DeserializeObject<PACKET_HEADER>(recvData);
                Debug.Log(JsonData);
                
                ProcessPacket(JsonData.packetIndex, recvData);


                StartReceiving();
            }
            else
            {
                //Disconnect();
            }
        }
        catch
        {
            // 실행이 발생하면 소켓이 연결되어 있는지 확인한 다음 다시 '수신 시작' 할 수 있습니다.
            if (!_sock.Connected)
            {
                //Disconnect();
            }
            else
            {
                StartReceiving();
            }
        }
    }

    private void ProcessPacket(short packetIndex, string JsonData)
    {
        switch(packetIndex)
        {
        case (short)PACKET_INDEX.NEW_LOGIN:
            {
                    Debug.Log("packet Index NEW_LOGIN 7");
            }
            break;
        case (short)PACKET_INDEX.NEW_LOGIN_SUCSESS:
            {
                    Debug.Log("packet Index NEW_LOGIN_SUCSESS 8");
                }
            break;
        default:
            {

            }
            break;
        }
    }


    public void SetMyId(int id)
    {
        _myId = id;
    }
    public void SetIsLogin(bool isLogin)
    {
        _isLogin = isLogin;
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

    private void ThisIsStopPacket()
    {
        // 이것도 위의 함수와 동일함
        // MoveEnd 패킷이 왔고 마지막 좌표가 담겨있다고 가정

        Character movePlayer;
        _characters.TryGetValue(2, out movePlayer);

        if (!_characters.ContainsKey(2))
            return;

        movePlayer.StopMove(movePlayer.transform.position);
    }


    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), _isLogin.ToString() + ", " + MyId.ToString());
    }
}

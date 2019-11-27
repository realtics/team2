using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

using Newtonsoft.Json;

public class StateObject    // 데이터를 수신하기 위한 상태 객체
{
    public Socket WorkSocket = null;                    // 클라이언트 소켓
    public const int BufferSize = 256;                  // 수신 버퍼의 크기
    public byte[] RecvBuffer = new byte[BufferSize];    // 수신 버퍼
    public void ClearRecvBuffer()
    {
        Array.Clear(RecvBuffer, 0, RecvBuffer.Length);
    }
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

    //// 신호를 받을 때 수동으로 재설정 되어야 하는 스레드 동기화 이벤트
    //private static ManualResetEvent _connectDone = new ManualResetEvent(false);
    //private static ManualResetEvent _sendDone = new ManualResetEvent(false);
    //private static ManualResetEvent _receiveDone = new ManualResetEvent(false);
    //private static String _response = String.Empty; // 서버 응답
    
    private Socket _sock = null;


    private int _myId;      // 실행한 클라이언트의 ID
    public int GetMyId { get { return _myId; } }
    public void SetMyId(int id) { _myId = id; }


    private bool _isLogin = false;      // 로그인 여부
    public bool GetIsLogin { get { return _isLogin; } }
    public void SetIsLogin(bool isLogin) { _isLogin = isLogin; }
    public bool LoginSuccess { set { _isLogin = true; } }


    private int _totalUser;     // 전체 유저 수
    public void SetTotalUser(int totalUser) { _totalUser = totalUser; }

    private string _UserList;       // 유저 리스트
    public void SetUserList(string userList) { _UserList = userList; }


    private bool _isConcurrentUserList = false;     // 유저리스트 여부
    public bool GetIsConcurrentUserList { get { return _isConcurrentUserList; } }
    public void SetIsConcurrentUserList(bool isConcurrentUserList) { _isConcurrentUserList = isConcurrentUserList; }


    public string DebugMsg01;
    public int DebugMsg02;
    public int DebugMsg03;


    private Dictionary<int,Character> _characters;
    private List<CharacterSpawnData> _spawnCharacters;
    
    void Start()
    {
        Screen.SetResolution(960, 540, false);
        
        _instance = this;
        _spawnCharacters = new List<CharacterSpawnData>();
        _characters = new Dictionary<int, Character>();
        
        CreateSocket();

        if (GetIsLogin == false)
        {
            NewLogin();

            Receive(_sock);
        }

    }

    void Update()
    {
        if (GetIsConcurrentUserList == false)
        {
            ConcurrentUser();
        }


        //// 서버에서 새로운 플레이어가 접속했다고 알려주는 역할임
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    // 새로운 플레이어의 Id를 넣어주고 생성함.
        //    JoinNewPlayer(GetMyId);
        //}

        //// 서버가 어떤 플레이어가 움직였다 라고 알림
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    ReceivedPacketHandler();
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    ThisIsStopPacket();
        //}

        if (_spawnCharacters.Count > 0)
        {
            GameObject newPlayer = Instantiate(playerPrefab);
            Character spawnedPlayer = newPlayer.GetComponent<Character>();
            spawnedPlayer.SetId(_spawnCharacters[0].id);
            newPlayer.transform.position = Vector3.zero;
            _characters.Add(_spawnCharacters[0].id, spawnedPlayer);
            _spawnCharacters.RemoveAt(0);
        }
    }

    private void CreateSocket()
    {
        try
        {
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            if (_sock == null)
            {
                Debug.Log("소켓 생성 실패");
            }
            _sock.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 31452));
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            _sock = (Socket)ar.AsyncState;
            _sock.EndConnect(ar);

            Debug.Log("소켓 연결 : " + _sock.RemoteEndPoint.ToString());
            //_connectDone.Set();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private void Receive(Socket sock)
    {
        try
        {
            StateObject state = new StateObject();
            state.WorkSocket = sock;

            // 데이터 수신 시작
            sock.BeginReceive(state.RecvBuffer, 0, StateObject.BufferSize, 0,
                                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            StateObject state = (StateObject)ar.AsyncState;
            _sock = state.WorkSocket;
            
            int bytesRead = 0;
            
            try
            {
                bytesRead = _sock.EndReceive(ar);
            } catch
            {
                return;
            }
            
            if (bytesRead > 0)
            {
                string recvData = Encoding.UTF8.GetString(state.RecvBuffer, 0, bytesRead);
                int bufLen = recvData.Length;
                Debug.Log("recvData[" + bufLen + "]= " + recvData);

                var JsonData = JsonConvert.DeserializeObject<PACKET_HEADER_BODY>(recvData);
                ProcessPacket(JsonData.header.packetIndex, recvData);

                // 수신 대기
                _sock.BeginReceive(state.RecvBuffer, 0, StateObject.BufferSize, 0,
                                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                //_receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private void ProcessPacket(short packetIndex, string jsonData)
    {
        switch(packetIndex)
        {
            case (short)PACKET_INDEX.RES_NEW_LOGIN_SUCSESS:
                {
                    var desJson = JsonConvert.DeserializeObject<PKT_RES_NEW_LOGIN_SUCSESS>(jsonData);

                    // 내 캐릭터를 생성하는 로직
                    // 서버에서 내가 접속했다고 알려주면 Id를 받고 내 Id로 설정한다.
                    if (GetMyId == 0)
                    {
                        SetIsLogin(desJson.isSuccess);
                        SetMyId(desJson.userID);
                        JoinNewPlayer(desJson.userID);
                        Debug.Log("접속 ID : " + desJson.userID + ", 접속 성공 여부 : " + desJson.isSuccess);
                    }
                    else
                    {
                        Debug.LogError("접속 ID : " + desJson.userID + ", 접속 성공 여부 : " + desJson.isSuccess);
                    }
                }
                break;
            case (short)PACKET_INDEX.RES_CONCURRENT_USER_LIST:
                {
                    var desJson = JsonConvert.DeserializeObject<PKT_RES_CONCURRENT_USER_LIST>(jsonData);
                    Debug.Log(desJson.totalUser);
                    Debug.Log(desJson.concurrentUser);
                    SetTotalUser(desJson.totalUser);
                    //SetUserList(Json.concurrentUserList);
                    DebugMsg01 = desJson.concurrentUser;
                    DebugMsg02 = desJson.totalUser;

                    string[] splitText = desJson.concurrentUser.Split(',');
                    for(int i = 0; i < splitText.Length; i++)
                    {
                        Debug.Log(splitText[i]);
                        
                        int userId = Int32.Parse(splitText[i]);
                        
                        if (GetMyId == userId)
                            continue;

                        if (_characters.ContainsKey(userId))
                            continue;

                        JoinNewPlayer(userId);
                    }

                    SetIsConcurrentUserList(true);
                }
                break;
            case (short)PACKET_INDEX.RES_PLAYER_MOVE_START:
                {
                    var desJson = JsonConvert.DeserializeObject<PKT_RES_PLAYER_MOVE_START>(jsonData);

                    var userID = desJson.userID;
                    var userPos = desJson.userPos;
                    var userDir = desJson.userDir;

                    Vector3 vecPos = StringToVector3(userPos);
                    Vector3 vecDir = StringToVector3(userDir);

                    ReceivedPacketHandler(userID, vecPos, vecDir);
                }
                break;
            case (short)PACKET_INDEX.RES_PLAYER_MOVE_END:
                {
                    var desJson = JsonConvert.DeserializeObject<PKT_RES_PLAYER_MOVE_END>(jsonData);

                    var userID = desJson.userID;
                    var userPos = desJson.userPos;

                    Vector3 vecPos = StringToVector3(userPos);

                    ThisIsStopPacket(userID, vecPos);
                }
                break;
            default:
                {
                    Debug.LogError("Index가 존재 하지 않는 Packet");
                }
            break;
        }
    }

    private void Send(Socket client, String data)
    {
        try
        { 
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0,
                            new AsyncCallback(SendCallback), client);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            _sock = (Socket)ar.AsyncState;

            int bytesSend = _sock.EndSend(ar);
            Debug.Log("서버에 "+ bytesSend + " bytes 보냄");

            //_sendDone.Set();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private void NewLogin()
    {
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_NEW_LOGIN,
            packetSize = 8
        };
        var packData = new PKT_REQ_NEW_LOGIN { header = packHeader };

        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;

        byte[] sendByte = new byte[256];
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

        SetMyId(0);
        int resultSize = _sock.Send(sendByte);
        Debug.Log("NewLogin() = " + resultSize);
    }

    private void ConcurrentUser()
    {
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_CONCURRENT_USER,
            packetSize = 8
        };
        var packData = new PKT_REQ_CONCURRENT_USER { header = packHeader };

        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;

        byte[] sendByte = new byte[256];
        sendByte = Encoding.UTF8.GetBytes(jsonData);
        
        SetIsConcurrentUserList(true);

        int resultSize = _sock.Send(sendByte);
        Debug.Log("ConcurrentUser() = " + resultSize);
    }

    public void MoveStart(Vector3 dir, Vector3 pos)
    {
        string startDir = dir.ToString("N5");
        string startPos = pos.ToString("N5");
        
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_PLAYER_MOVE_START,
            packetSize = 100
        };
        var packData = new PKT_REQ_PLAYER_MOVE_START
        {
            header = packHeader,
            userID = GetMyId,
            userPos = startPos,
            userDir = startDir
        };

        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;

        byte[] sendByte = new byte[256];
        sendByte = Encoding.UTF8.GetBytes(jsonData);

        int resultSize = _sock.Send(sendByte);
        Debug.Log("MoveStart() = " + resultSize);
    }

    public void MoveEnd(Vector3 pos)
    {
        string EndPos = pos.ToString("N5");

        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_PLAYER_MOVE_END,
            packetSize = 100
        };
        var packData = new PKT_REQ_PLAYER_MOVE_END
        {
            header = packHeader,
            userID = GetMyId,
            userPos = EndPos
        };

        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;

        byte[] sendByte = new byte[256];
        sendByte = Encoding.UTF8.GetBytes(jsonData);

        int resultSize = _sock.Send(sendByte);
        Debug.Log("MoveEnd() = " + resultSize);
    }

    public void JoinNewPlayer(int id)
    {
        CharacterSpawnData newPlayer = new CharacterSpawnData();

        // Instantiate는 게임 오브젝트를 씬에 생성하는 함수
        // 코스트가 커서 자주 사용하면 좋지 않아 보통은 ObjectPool을 사용한다.
        // 생성 된 캐릭터에게 자신의 Id를 알려준다.
        // 패킷을 보낼 때 어떤 플레이어가 보냈는지 알려주기 위해 고유 Id를 함께 넘겨줘야한다.

        //newPlayer = Instantiate(playerPrefab).GetComponent<Character>();

        newPlayer.position = Vector3.zero; // 접속한 클라 위치
        newPlayer.id = id;

        //_characters.Add(id, newPlayer);
        _spawnCharacters.Add(newPlayer);
    }

    // 원래는 패킷마다 핸들러를 만들어 사용해야함
    private void ReceivedPacketHandler(int userID, Vector3 pos, Vector3 dir)
    {
        // id가 2인 플레이어가 1,0,0 방향벡터로 움직였다고 가정
        Character movePlayer;
        //_characters.TryGetValue(2, out movePlayer);
        _characters.TryGetValue(userID, out movePlayer);

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

        //movePlayer.SetMoveDirectionAndMove(Vector3.zero, Vector3.left);
        movePlayer.SetMoveDirectionAndMove(pos, dir);
    }

    private void ThisIsStopPacket(int userID, Vector3 pos)
    {
        // 이것도 위의 함수와 동일함
        // MoveEnd 패킷이 왔고 마지막 좌표가 담겨있다고 가정

        Character movePlayer;
        //_characters.TryGetValue(2, out movePlayer);
        _characters.TryGetValue(userID, out movePlayer);

        if (!_characters.ContainsKey(userID))
            return;

        //movePlayer.StopMove(movePlayer.transform.position);
        movePlayer.StopMove(pos);
    }


    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 500, 100), "접속여부:" + _isLogin.ToString() + ", 유저:" + GetMyId.ToString());
        GUI.Label(new Rect(0, 15, 300, 100), "동시접속자 수 : " + DebugMsg02);
        GUI.Label(new Rect(0, 30, 960, 100), "접속자 리스트 : " + DebugMsg01);

    }
    
    public Vector3 StringToVector3(string vector)
    {
        if (vector.StartsWith("(") && vector.EndsWith(")"))
        {
            vector = vector.Substring(1, vector.Length - 2);
        }

        string[] arrayData = vector.Split(',');

        Vector3 result = new Vector3(
                                    float.Parse(arrayData[0]),
                                    float.Parse(arrayData[1]),
                                    float.Parse(arrayData[2]));

        return result;
    }
}

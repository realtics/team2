using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

using Newtonsoft.Json;
using System.IO;
using Cinemachine;
using UnityEngine.SceneManagement;

enum DefineDefaultValue : short
{
    packetSize = 100
}

public struct CharacterSpawnData
{
    public int id;
    public string userName;
    public Vector3 position;
    public Vector3 direction;
}

public class StateObject    // 데이터를 수신하기 위한 상태 객체
{

	public Socket WorkSocket = null;                    // 클라이언트 소켓
    public const int BufferSize = 512;                  // 수신 버퍼의 크기
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
			//if (_instance._sock == null)
			//    return null;

			return _instance;
		}
	}

	public GameObject playerPrefab;

	//// 신호를 받을 때 수동으로 재설정 되어야 하는 스레드 동기화 이벤트
	//private static ManualResetEvent _connectDone = new ManualResetEvent(false);
	//private static ManualResetEvent _sendDone = new ManualResetEvent(false);
	//private static ManualResetEvent _receiveDone = new ManualResetEvent(false);
	//private static String _response = String.Empty; // 서버 응답

	public string appDataPath;
	public string appDataPathParent;

	private Socket _sock = null;
	public bool IsConnect { get { return _sock == null ? false : true; } }

	[SerializeField]
	private int _myId = 0;      // 실행한 클라이언트의 ID

	private bool _isSingle;
	private string _accountId;
	public int MyId { get { return _myId; } set { _myId = value; } }


	private bool _isLogin = false;      // 로그인 여부
	public bool IsLogin { get { return _isLogin; } set { _isLogin = true; } }


	private int _totalUser;     // 전체 유저 수
	public int TotalUser { get { return _totalUser; } set { _totalUser = value; } }

    private string _userList;       // 유저 리스트
    public string UserList { get { return _userList; } set { _userList = value; } }
	public bool IsSingle { get { return _isSingle; } set { _isSingle = value; } }


	private bool _isConcurrentUserList = false;     // 유저리스트 여부
    public bool IsConcurrentUserList { get { return _isConcurrentUserList; } set { _isConcurrentUserList = value; } }
	public string AccountId { get { return _accountId; } set { _accountId = value; } }
	private bool _login;
	public bool Login { get { return _login; } }

    public string DebugMsg01;
    public int DebugMsg02;
    public int DebugMsg03;

    public bool DebugMsg10;
    public int DebugMsg11;

    public bool DebugMsg12;
    public int DebugMsg13;
    public List<string> _DebugMsgList01;
    public bool DebugText = false;

    private Dictionary<int, CharacterMovement> _characters;
    private List<CharacterSpawnData> _spawnCharacters;
    private List<int> _exitCharacters;
	
	private bool _successSignup;
	public bool SuccessSignup { get { return _successSignup; } set { _successSignup = value; } }
	

    private void Awake()
    {
        _instance = this;
		DontDestroyOnLoad(this);
        appDataPath = Application.dataPath;
        appDataPathParent = System.IO.Directory.GetParent(appDataPath).ToString();
        _exitCharacters = new List<int>();
        _spawnCharacters = new List<CharacterSpawnData>();
        _characters = new Dictionary<int, CharacterMovement>();
		//Debug.Log(appDataPathParent);
		_isSingle = true;

	}

    void Start()
    {
		//Screen.SetResolution(960, 540, false);

		DebugLogList("start() start");
		//ConnectToServer();

        DebugLogList("start() end");
    }

	public void ConnectToServer()
	{
		if (IsConnect)
			return;

		CreateSocket();
		//NewLogin();
		//NewLoginSucsess();
		Receive(_sock);
		_isSingle = false;
	}

    void FixedUpdate()
    {
        if (_isLogin)
        {
            //if (!IsConcurrentUserList)
            //{
            //    ConcurrentUser();
            //}


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
				CharacterSpawnData spawnData = _spawnCharacters[0];

				if (spawnData.id == _myId)
                {
                    PlayerCharacter pc = newPlayer.AddComponent<PlayerCharacter>();
                    pc.FindMovement();

                    FindObjectOfType<CinemachineVirtualCamera>().Follow = pc.transform;

                    MovementJoystick joystick = FindObjectOfType<MovementJoystick>();
                    if (joystick != null)
                        joystick.PC = pc;

                    InputSystem input = FindObjectOfType<InputSystem>();
                    if (input != null)
                        input.PC = pc;

                    NetworkInventoryInfoSaver.Instance.InventoryInitValue = false;
                    OpenInventory();
                   
                }
                else
					newPlayer.transform.GetChild(0).tag = "UserPlayer";

				CharacterMovement spawnedPlayer = newPlayer.GetComponent<CharacterMovement>();
                spawnedPlayer.Id = spawnData.id;
                spawnedPlayer.NickName = spawnData.userName;
                spawnedPlayer.SetFlipX(spawnData.direction.x < 0 ? true : false);
                newPlayer.transform.position = spawnData.position;
                _characters.Add(spawnData.id, spawnedPlayer);
				_spawnCharacters.RemoveAt(0);
			}

			ExitUserOff();
        }
    }

    private void ExitUserOff()
    {
        if (_exitCharacters.Count == 0)
            return;

        int id = _exitCharacters[0];

        CharacterMovement exitUser;
        _characters.TryGetValue(id, out exitUser);

        if (exitUser == null)
            return;

        // FIXME(안병욱) : 오브젝트 풀로 수정

        _characters.Remove(id);
		_exitCharacters.RemoveAt(0);

		Destroy(exitUser.gameObject);
    }

    private void CreateSocket()
    {
        try
        {
            DebugLogList("socket() start");
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            if (_sock == null)
            {
                Debug.Log("소켓 생성 실패");
            }
            //_sock.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 31452));
            _sock.Connect(new IPEndPoint(IPAddress.Parse("192.168.200.141"), 31452));
           // _sock.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.105"), 31452));

            DebugLogList("socket() end");
        }
        catch (Exception e)
        {
			_sock = null;
			Debug.LogError(e.ToString());
            DebugLogList("[!!Exception!!] " + e.ToString());

		}

    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            DebugLogList("ConnectCallback()");
            _sock = (Socket)ar.AsyncState;
            _sock.EndConnect(ar);
            Debug.Log("소켓 연결 : " + _sock.RemoteEndPoint.ToString());
            //_connectDone.Set();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            DebugLogList("[!!Exception!!] " + e.ToString());
        }
    }

    private void Receive(Socket sock)
    {
		try
        {
            DebugLogList("Receive() start");
            StateObject state = new StateObject();
            state.ClearRecvBuffer();
            state.WorkSocket = sock;
            // 데이터 수신 시작
            sock.BeginReceive(state.RecvBuffer, 0, StateObject.BufferSize, 0,
                                new AsyncCallback(ReceiveCallback), state);
			DebugLogList("Receive() end");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            DebugLogList("[!!Exception!!] " + e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            DebugLogList("ReceiveCallback() start");
            StateObject state = (StateObject)ar.AsyncState;
            _sock = state.WorkSocket;

            int bytesRead = 0;

            try
            {
                bytesRead = _sock.EndReceive(ar);
                DebugLogList("bytesRead : " + bytesRead.ToString());
            }
            catch
            {
                return;
            }

            if (bytesRead > 0)
            {
				DebugLogList("bytesRead > 0");
				// TODO : 인코딩 해결 할 것
				//string recvData = Encoding.UTF8.GetString(state.RecvBuffer);

				string recvData = Encoding.UTF8.GetString(state.RecvBuffer, 0, bytesRead);
				//string recvData = Encoding.GetEncoding("EUC-KR").GetString(state.RecvBuffer, 0, bytesRead);
				DebugLogList(recvData);
				var JsonData = JsonConvert.DeserializeObject<PACKET_HEADER_BODY>(recvData);

				//int bufLen = recvData.Length;
				//Debug.Log("recvData[" + bufLen + "]= " + recvData);

				//// TODO : EUC-KR로 한글이 들어 올 경우, 패킷 자르기 시, 서버에서 잰 길이와 클라에서 받는 길이가 다르게 판정됨
				//// 패킷 자르기
				//string recvDataSubstring = recvData.Substring(45, 3);
				//string[] recvDataSplit = recvDataSubstring.Split('"');
				//int recvDataSize = int.Parse(recvDataSplit[0]);

				//string recvDataSubstring2 = recvData.Substring(0, recvDataSize);
				//DebugLogList(recvDataSubstring2);
				//var JsonData = JsonConvert.DeserializeObject<PACKET_HEADER_BODY>(recvDataSubstring2);

				DebugLogList("ReceiveCallback - ProcessPacket - start");
				ProcessPacket(JsonData.header.packetIndex, recvData);
				DebugLogList("ReceiveCallback - ProcessPacket - end");
				// 수신 대기
				_sock.BeginReceive(state.RecvBuffer, 0, StateObject.BufferSize, 0,
									new AsyncCallback(ReceiveCallback), state);
				DebugLogList("ReceiveCallback end");
			}
            else
            {
                //_receiveDone.Set();
                DebugLogList("bytesRead <= 0");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            DebugLogList("[!!Exception!!] " + e.ToString());
        }
    }

    private void ProcessPacket(short packetIndex, string jsonData)
    {

		DebugLogList("ProcessPacket() start");
        try
        {
            switch (packetIndex)
            {
				case (short)PACKET_INDEX.RES_CHECK_BEFORE_LOGIN:
					{
						DebugLogList("PACKET_INDEX.RES_CHECK_BEFORE_LOGIN start");
						var desJson = JsonConvert.DeserializeObject<PKT_RES_CHECK_BEFORE_LOGIN>(jsonData);
						Debug.Log(desJson.checkResult);

						int checkResult = desJson.checkResult;

						switch (checkResult)
						{
							case (int)CHECK_BEFORE_LOGIN_RESULT.RESULT_BEFORE_LOGIN_CHECK_SUCCESS:
								{
									var sessionID = desJson.sessionID;
									var userName = desJson.userName;

									_myId = sessionID;
									PlayerManager.Instance.NickName = userName;
									_login = true;
								}
								break;
							case (int)CHECK_BEFORE_LOGIN_RESULT.RESULT_BEFORE_LOGIN_CHECK_NO_ID:
								ToastMessagePanel.Instance.SetToastMessage("존재하지 않는 아이디입니다.");
								_accountId = "";
								break;
							case (int)CHECK_BEFORE_LOGIN_RESULT.RESULT_BEFORE_LOGIN_CHECK_IS_WRONG_PASSWORD:
								ToastMessagePanel.Instance.SetToastMessage("비밀번호가 틀렸습니다.");
								_accountId = "";
								break;
						}

						
					}
					break;
				case (short)PACKET_INDEX.RES_CONCURRENT_USER_LIST:
                    {
                        DebugLogList("PACKET_INDEX.RES_CONCURRENT_USER_LIST start");
                        var desJson = JsonConvert.DeserializeObject<PKT_RES_CONCURRENT_USER_LIST>(jsonData);

                        DebugLogList(desJson.ToString());
                        DebugLogList(desJson.header.ToString());
                        DebugLogList(desJson.header.packetIndex.ToString());
                        DebugLogList(desJson.header.packetSize.ToString());

                        //Debug.Log(desJson.totalUser);
                        //Debug.Log(desJson.concurrentUser);
                        //Debug.Log(desJson.userPos);
                        //Debug.Log(desJson.userDir);
                        _totalUser = desJson.totalUser;
                        //SetUserList(Json.concurrentUserList);
                        DebugMsg01 = desJson.concurrentUser;
                        DebugMsg02 = desJson.totalUser;

                        string[] splitConcurrentUser = desJson.concurrentUser.Split(',');
                        string[] splitUserName = desJson.userName.Split(',');
                        string[] splitUserPos = desJson.userPos.Split('|');
                        string[] splitUserDir = desJson.userDir.Split('|');
                        for (int i = 0; i < splitConcurrentUser.Length; i++)
                        {
                            //Debug.Log(splitConcurrentUser[i]);
                            //Debug.Log(splitUserPos[i]);
                            //Debug.Log(splitUserDir[i]);
                            int sessionID = Int32.Parse(splitConcurrentUser[i]);
                            string userName = splitUserName[i];
                            string userPos = splitUserPos[i];
                            string userDir = splitUserDir[i];

                            if (MyId == sessionID)
                                continue;

                            if (_characters.ContainsKey(sessionID))
                                continue;

                            JoinNewPlayer(sessionID, userName, StringToVector3(userPos), StringToVector3(userDir));
                        }
                        DebugLogList("PACKET_INDEX.RES_CONCURRENT_USER_LIST end");
                        _isConcurrentUserList = true;
                    }
                    break;
                case (short)PACKET_INDEX.RES_USER_EXIT:
                    {
                        var desJson = JsonConvert.DeserializeObject<PKT_RES_USER_EXIT>(jsonData);

                        var sessionID = desJson.sessionID;

						if (_myId == sessionID)
							break;

						_exitCharacters.Add(sessionID);
                    }
                    break;
                case (short)PACKET_INDEX.RES_PLAYER_MOVE_START:
                    {
                        var desJson = JsonConvert.DeserializeObject<PKT_RES_PLAYER_MOVE_START>(jsonData);

                        var sessionID = desJson.sessionID;
                        var userPos = desJson.userPos;
                        var userDir = desJson.userDir;

                        Vector3 vecPos = StringToVector3(userPos);
                        Vector3 vecDir = StringToVector3(userDir);

                        //Debug.Log("userPos:" + vecPos.ToString("N5") + ", userDir:" + vecDir.ToString("N5"));

                        ReceivedPacketHandler(sessionID, vecPos, vecDir);
                    }
                    break;
                case (short)PACKET_INDEX.RES_PLAYER_MOVE_END:
                    {
                        var desJson = JsonConvert.DeserializeObject<PKT_RES_PLAYER_MOVE_END>(jsonData);

                        var sessionID = desJson.sessionID;
                        var userPos = desJson.userPos;

                        Vector3 vecPos = StringToVector3(userPos);
                        //Debug.Log("userPos:" + vecPos.ToString("N5"));

                        ThisIsStopPacket(sessionID, vecPos);
                    }
                    break;
				case (short)PACKET_INDEX.RES_CHATTING:
					{
						var desJson = JsonConvert.DeserializeObject<PKT_RES_CHATTING>(jsonData);

						var sessionID = desJson.sessionID;
						var userName = desJson.userName;
						var newChat = desJson.chatMessage;

						ChattingPanel.Instance.AddNewChatting(userName, newChat);
					}
					break;
				case (short)PACKET_INDEX.RES_SIGN_UP:
					{
						var desJson = JsonConvert.DeserializeObject<PKT_RES_SIGN_UP>(jsonData);

						RESULT_SIGN_UP_CHECK checkResult = (RESULT_SIGN_UP_CHECK)desJson.checkResult;

						switch (checkResult)
						{
                            case RESULT_SIGN_UP_CHECK.RESULT_SIGN_UP_OVERLAP_ID:
                                ToastMessagePanel.Instance.SetToastMessage("이미 사용중인 아이디입니다.");
                                break;
                            case RESULT_SIGN_UP_CHECK.RESULT_SIGN_UP_CHECK_SUCCESS:
                                ToastMessagePanel.Instance.SetToastMessage("회원가입 완료 !");
                                // FIXME: (안병욱) 파인드 수정
								_successSignup = true;

								break;
                        }
					}
					break;
				case (short)PACKET_INDEX.RES_DUNGEON_CLEAR_RESULT_ITEM:
					{
						var desJson = JsonConvert.DeserializeObject<PKT_RES_DUNGEON_CLEAR_RESULT_ITEM>(jsonData);

						var itemID = desJson.itemID;

						NetworkInventoryInfoSaver.Instance.ItemID = itemID;

                        DungeonGameManager.Instance.RES_DUNGEON_CLEAR_RESULT_ITEM = true;
                    }
					break;
                case (short)PACKET_INDEX.RES_DUNGEON_HELL_RESULT_ITEM:
                    {
                        var desJson = JsonConvert.DeserializeObject<PKT_RES_DUNGEON_HELL_RESULT_ITEM>(jsonData);

                        var itemID = desJson.itemID;

                        NetworkInventoryInfoSaver.Instance.ItemID = itemID;

                        MapLoader.instance.RES_DUNGEON_HELL_RESULT_ITEM = true;
                    }
                    break;
                case (short)PACKET_INDEX.RES_INVENTORY_CLOSE:
                    {
                        var desJson = JsonConvert.DeserializeObject<PKT_RES_INVENTORY_CLOSE>(jsonData);

                        Debug.Log(desJson.checkResult);
                    }
                    break;
                case (short)PACKET_INDEX.RES_INVENTORY_OPEN:
                    {
                        var desJson = JsonConvert.DeserializeObject<PKT_RES_INVENTORY_OPEN>(jsonData);

                        NetworkInventoryInfoSaver.Instance.SaveItemIDs(desJson.equip, desJson.inventory);

                        NetworkInventoryInfoSaver.Instance.RES_INVENTORY_OPEN = true;
                    }
                    break;
                default:
                    {
                        Debug.LogError("Index가 존재 하지 않는 Packet");
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            DebugLogList("[!!Exception!!] " + e.ToString());
        }
    }

    private void Send(Socket client, String data)
    {
        DebugLogList("Send(Socket client, String data)");
        try
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0,
                            new AsyncCallback(SendCallback), client);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            DebugLogList("[!!Exception!!] " + e.ToString());
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        DebugLogList("SendCallback()");
        try
        {
            _sock = (Socket)ar.AsyncState;

            int bytesSend = _sock.EndSend(ar);
            Debug.Log("서버에 " + bytesSend + " bytes 보냄");

            //_sendDone.Set();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            DebugLogList("[!!Exception!!] " + e.ToString());
        }
    }


    public void NewLogin()
    {
        DebugLogList("NewLogin() start");
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_NEW_LOGIN,
            packetSize = (short)DefineDefaultValue.packetSize
		};
        var packData = new PKT_REQ_NEW_LOGIN { header = packHeader };
        DebugLogList(packData.ToString());
        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;
        DebugLogList(jsonData.ToString());
        byte[] sendByte = new byte[512];
        sendByte = Encoding.UTF8.GetBytes(jsonData);
		//TODO 1-0: JSON 헤더에 패킷 사이즈 체크 하는것을 foreach로 하고 있는데, 더 좋은 방법 있다면 개선
		//TODO 1-1: 패킷 사이즈를 담아 보내는 것이 현재 상태에선 크게 중요하진 않으므로, 코드만 남겨두고 나중에 활용
		short jsonDataSize = 0;
		foreach (byte b in sendByte)
		{
			jsonDataSize++;
			if (b == '\0')
				break;
		}
		//Debug.Log(jsonData);
		//Debug.Log(jsonDataSize);
		var packHeader2 = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_NEW_LOGIN,
			packetSize = jsonDataSize
		};
		var packData2 = new PKT_REQ_NEW_LOGIN { header = packHeader2 };
		string jsonData2;
		jsonData2 = JsonConvert.SerializeObject(packData2);
		jsonData2 += endNullValue;
		byte[] sendByte2 = new byte[512];
		sendByte2 = Encoding.UTF8.GetBytes(jsonData2);

		_myId = 0;
        int resultSize = _sock.Send(sendByte2);
        //Debug.Log("NewLogin() = " + resultSize);
        DebugLogList("NewLogin() end");
    }

    private void NewLoginSucsess()
    {
        DebugLogList("NewLoginSucsess() start");
        byte[] recvBuf = new byte[512];
        int socketReceive = _sock.Receive(recvBuf);
        //Debug.Log(socketReceive);

        string recvData = Encoding.UTF8.GetString(recvBuf, 0, socketReceive);
        int bufLen = recvBuf.Length;
        //Debug.Log(recvData);
        DebugLogList(recvData);

        var desJson = JsonConvert.DeserializeObject<PKT_RES_NEW_LOGIN_SUCSESS>(recvData);
        if (desJson.header.packetIndex == (short)PACKET_INDEX.RES_NEW_LOGIN_SUCSESS)
        {
            //Debug.Log("접속 성공 여부 : " + desJson.isSuccess);
            //Debug.Log("접속 ID : " + desJson.sessionID);

            if (MyId == 0)
            {
                _isLogin = desJson.isSuccess;
                _myId = desJson.sessionID;
                JoinNewPlayer(desJson.sessionID, PlayerManager.Instance.NickName, Vector3.zero, Vector3.right);
            }
        }
        DebugLogList("NewLoginSucsess() end");
    }

    public void ConcurrentUser()
    {
        DebugLogList("ConcurrentUser() start");
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_CONCURRENT_USER,
            packetSize = (short)DefineDefaultValue.packetSize
		};
        var packData = new PKT_REQ_CONCURRENT_USER { header = packHeader };
        DebugLogList(packData.ToString());
        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;
        DebugLogList(jsonData.ToString());
        byte[] sendByte = new byte[512];
        sendByte = Encoding.UTF8.GetBytes(jsonData);

		_isConcurrentUserList = true;

		int resultSize = _sock.Send(sendByte);
        //Debug.Log("ConcurrentUser() = " + resultSize);
        DebugLogList("resultSize :" + resultSize.ToString());
        DebugLogList("ConcurrentUser() end");
    }

    public void MoveStart(Vector3 pos, Vector3 dir)
    {
        if (_sock == null)
            return;

        string startPos = pos.ToString("N4");
        string startDir = dir.ToString("N1");

        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_PLAYER_MOVE_START,
            packetSize = (short)DefineDefaultValue.packetSize
		};
        var packData = new PKT_REQ_PLAYER_MOVE_START
        {
            header = packHeader,
			sessionID = MyId,
            userPos = startPos,
            userDir = startDir
        };

        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;

        byte[] sendByte = new byte[512];
        sendByte = Encoding.UTF8.GetBytes(jsonData);

        int resultSize = _sock.Send(sendByte);
        //Debug.Log("MoveStart - Send");
    }

    public void MoveEnd(Vector3 pos, Vector3 dir)
    {
        if (_sock == null)
            return;

        string EndPos = pos.ToString("N4");
        string EndDir = dir.ToString("N1");

        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_PLAYER_MOVE_END,
            packetSize = (short)DefineDefaultValue.packetSize
		};
        var packData = new PKT_REQ_PLAYER_MOVE_END
        {
            header = packHeader,
			sessionID = MyId,
            userPos = EndPos,
            userDir = EndDir
        };

        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;

        byte[] sendByte = new byte[512];
        sendByte = Encoding.UTF8.GetBytes(jsonData);

        int resultSize = _sock.Send(sendByte);
        //Debug.Log("MoveEnd - Send");
    }

    public void JoinNewPlayer(int id, string userName, Vector3 pos, Vector3 dir)
    {
        DebugLogList("JoinNewPlayer start");
        DebugLogList("ID : " + id.ToString());
        CharacterSpawnData newPlayer = new CharacterSpawnData();

        // Instantiate는 게임 오브젝트를 씬에 생성하는 함수
        // 코스트가 커서 자주 사용하면 좋지 않아 보통은 ObjectPool을 사용한다.
        // 생성 된 캐릭터에게 자신의 Id를 알려준다.
        // 패킷을 보낼 때 어떤 플레이어가 보냈는지 알려주기 위해 고유 Id를 함께 넘겨줘야한다.

        //newPlayer = Instantiate(playerPrefab).GetComponent<Character>();

        newPlayer.position = pos; //Vector3.zero; // 접속한 클라 위치
        newPlayer.direction = dir;
        newPlayer.id = id;
        newPlayer.userName = userName;

        //_characters.Add(id, newPlayer);
        _spawnCharacters.Add(newPlayer);
        DebugLogList("JoinNewPlayer end");
    }

    // 원래는 패킷마다 핸들러를 만들어 사용해야함
    private void ReceivedPacketHandler(int sessionID, Vector3 pos, Vector3 dir)
    {
        // id가 2인 플레이어가 1,0,0 방향벡터로 움직였다고 가정
        CharacterMovement movePlayer;
        //_characters.TryGetValue(2, out movePlayer);
        _characters.TryGetValue(sessionID, out movePlayer);

        // 무언가 받아온 다음 null 체크는 필수
        // Dictinary에서 받아오는 것이기 때문에
        if (!_characters.ContainsKey(sessionID))
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

    private void ThisIsStopPacket(int sessionID, Vector3 pos)
    {
        // 이것도 위의 함수와 동일함
        // MoveEnd 패킷이 왔고 마지막 좌표가 담겨있다고 가정

        CharacterMovement movePlayer;
        //_characters.TryGetValue(2, out movePlayer);
        _characters.TryGetValue(sessionID, out movePlayer);

        if (!_characters.ContainsKey(sessionID))
            return;

        //movePlayer.StopMove(movePlayer.transform.position);
        movePlayer.StopMove(pos);
    }

	//private void OnGUI()
	//{
	//	//GUI.Label(new Rect(0, 0, 500, 100), "접속여부:" + _isLogin.ToString() + ", 유저:" + GetMyId.ToString());
	//	//GUI.Label(new Rect(0, 15, 300, 100), "동시접속자 수 : " + DebugMsg02);
	//	//GUI.Label(new Rect(0, 30, 960, 100), "접속자 리스트 : " + DebugMsg01);

	//	//GUI.Label(new Rect(0, 60, 960, 100), "10 : " + DebugMsg10 + ", 11 : " + DebugMsg11);

	//}

	public int DebugLogListIndex = 0;
    public void DebugLogList(string logData)
    {
        string addLogIndex = "[" + DebugLogListIndex + "]["
                                + _isLogin.ToString() + "]["
                                + MyId.ToString() + "] "
                                + logData;

        _DebugMsgList01.Add(addLogIndex);
        DebugLogListIndex++;
        //Debug.LogError(DebugLogListIndex + "] " + logData);
        //DebugLogFileSave();
    }

    public void DebugLogFileSave()
    {
        if (Directory.Exists(appDataPathParent + "/log") == false)
        {
            Directory.CreateDirectory(appDataPathParent + "/log");
        }
        string dateTime = DateTime.Now.ToString("yyMMdd-HHmmss");
        string fileName = appDataPathParent + "/Log/Debug-" + dateTime + "-" + MyId.ToString() + ".txt";
        //Debug.Log(fileName);

        FileStream fs = new FileStream(fileName, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);

        for (int i = 0; i < _DebugMsgList01.Count; i++)
        {
            sw.WriteLine(_DebugMsgList01[i].ToString());
        }

        sw.Close();
        fs.Close();
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

    public void DisconnectServer()
    {
		if (IsConnect)
			_sock.Close();
    }

	public void CheckBeforeLogin(string id, string pw)
	{
		if (!IsConnect)
			return;

		DebugLogList("CheckBeforeLogin() start");
		string jsonData;
		char endNullValue = '\0';

		var packHeader = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_CHECK_BEFORE_LOGIN,
			packetSize = (short)DefineDefaultValue.packetSize
		};
		var packData = new PKT_REQ_CHECK_BEFORE_LOGIN
		{
			header = packHeader,
			userID = id,
			userPW = pw
		};
		DebugLogList(packData.ToString());
		jsonData = JsonConvert.SerializeObject(packData);
		jsonData += endNullValue;
		DebugLogList(jsonData.ToString());
		byte[] sendByte = new byte[512];
		sendByte = Encoding.UTF8.GetBytes(jsonData);

		short jsonDataSize = 0;
		foreach (byte b in sendByte)
		{
			jsonDataSize++;
			if (b == '\0')
				break;
		}
		//Debug.Log(jsonData);
		//Debug.Log(jsonDataSize);
		var packHeader2 = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_CHECK_BEFORE_LOGIN,
			packetSize = jsonDataSize
		};
		var packData2 = new PKT_REQ_CHECK_BEFORE_LOGIN
		{
			header = packHeader2,
			userID = id,
			userPW = pw
		};
		string jsonData2;
		jsonData2 = JsonConvert.SerializeObject(packData2);
		jsonData2 += endNullValue;
		byte[] sendByte2 = new byte[512];
		sendByte2 = Encoding.UTF8.GetBytes(jsonData2);

		_myId = 0;
		int resultSize = _sock.Send(sendByte2);
		DebugLogList("CheckBeforeLogin() end");
		_accountId = id;
	}

	public void LoginToTown()
	{
		NewLogin();
		NewLoginSucsess();
		ConcurrentUser();
	}

	public void SendChat(string chat)
	{
		if (!IsConnect)
			return;

		string jsonData;
		char endNullValue = '\0';

		var packHeader = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_CHATTING,
			packetSize = (short)DefineDefaultValue.packetSize
		};
		var packData = new PKT_REQ_CHATTING
		{
			header = packHeader,
			sessionID = MyId,
			chatMessage = chat
		};

		jsonData = JsonConvert.SerializeObject(packData);
		jsonData += endNullValue;

		byte[] sendByte = new byte[512];
		sendByte = Encoding.UTF8.GetBytes(jsonData);

		short jsonDataSize = 0;
		foreach (byte b in sendByte)
		{
			jsonDataSize++;
			if (b == '\0')
				break;
		}

		var packHeader2 = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_CHATTING,
			packetSize = jsonDataSize
		};
		var packData2 = new PKT_REQ_CHATTING
		{
			header = packHeader2,
			sessionID = MyId,
			chatMessage = chat
		};
		string jsonData2;
		jsonData2 = JsonConvert.SerializeObject(packData2);
		jsonData2 += endNullValue;
		byte[] sendByte2 = new byte[512];
		sendByte2 = Encoding.UTF8.GetBytes(jsonData2);
		int resultSize = _sock.Send(sendByte2);

		//int euckrCodepage = 51949;
		//Encoding euckr = Encoding.GetEncoding(euckrCodepage);

		//byte[] sendByte3 = euckr.GetBytes(jsonData2);
		//int resultSize = _sock.Send(sendByte3, 0, sendByte3.Length, SocketFlags.None);

	}

	public void UserExit()
	{
		if (!IsConnect)
			return;

		//DebugLogList("UserExit() start");
		string jsonData;
		char endNullValue = '\0';

		var packHeader = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_USER_EXIT,
			packetSize = 8
		};
		var packData = new PKT_REQ_USER_EXIT
		{
			header = packHeader,
			sessionID = _myId
		};
		DebugLogList(packData.ToString());
		jsonData = JsonConvert.SerializeObject(packData);
		jsonData += endNullValue;
		DebugLogList(jsonData.ToString());
		byte[] sendByte = new byte[512];
		sendByte = Encoding.UTF8.GetBytes(jsonData);

		int resultSize = _sock.Send(sendByte);
		DebugLogList("UserExit() end");

		_characters.Clear();
		_exitCharacters.Clear();
	}

	public void SignUpUser(string id, string password, string nickName)
	{
		if (!IsConnect)
			return;

		string jsonData;
		char endNullValue = '\0';

		var packHeader = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_SIGN_UP,
			packetSize = 8
		};
		var packData = new PKT_REQ_SIGN_UP
		{
			header = packHeader,
			userID = id,
			userPW = password,
			userName = nickName
		};
		DebugLogList(packData.ToString());
		jsonData = JsonConvert.SerializeObject(packData);
		jsonData += endNullValue;
		DebugLogList(jsonData.ToString());
		byte[] sendByte = new byte[512];
		sendByte = Encoding.UTF8.GetBytes(jsonData);

		int resultSize = _sock.Send(sendByte);
	}

	public void DungeonClearResultItem()
	{
		DebugLogList("DungeonClearResultItem() start");
		string jsonData;
		char endNullValue = '\0';

		var packHeader = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_DUNGEON_CLEAR_RESULT_ITEM,
			packetSize = (short)DefineDefaultValue.packetSize
		};
		var packData = new PKT_REQ_DUNGEON_CLEAR_RESULT_ITEM
        {
            header = packHeader,
            userID = _accountId
        };
		DebugLogList(packData.ToString());
		jsonData = JsonConvert.SerializeObject(packData);
		jsonData += endNullValue;
		DebugLogList(jsonData.ToString());
		byte[] sendByte = new byte[512];
		sendByte = Encoding.UTF8.GetBytes(jsonData);
		
		int resultSize = _sock.Send(sendByte);
		DebugLogList("DungeonClearResultItem() end");
	}

    public void DungeonHellResultItem()
    {
        DebugLogList("DungeonHellResultItem() start");
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_DUNGEON_HELL_RESULT_ITEM,
            packetSize = (short)DefineDefaultValue.packetSize
        };
        var packData = new PKT_REQ_DUNGEON_HELL_RESULT_ITEM
        {
            header = packHeader,
            userID = _accountId
        };
        DebugLogList(packData.ToString());
        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;
        DebugLogList(jsonData.ToString());
        byte[] sendByte = new byte[512];
        sendByte = Encoding.UTF8.GetBytes(jsonData);

        int resultSize = _sock.Send(sendByte);
        DebugLogList("DungeonHellResultItem() end");
    }

    public void DungeonHellItemPickUp()
    {
        DebugLogList("DungeonHellItemPickUp() start");
        string jsonData;
        char endNullValue = '\0';

        var packHeader = new PACKET_HEADER
        {
            packetIndex = (short)PACKET_INDEX.REQ_DUNGEON_HELL_ITEM_PICK_UP,
            packetSize = (short)DefineDefaultValue.packetSize
        };
        var packData = new PKT_REQ_DUNGEON_HELL_ITEM_PICK_UP
        {
            header = packHeader,
            userID = _accountId
        };
        DebugLogList(packData.ToString());
        jsonData = JsonConvert.SerializeObject(packData);
        jsonData += endNullValue;
        DebugLogList(jsonData.ToString());
        byte[] sendByte = new byte[512];
        sendByte = Encoding.UTF8.GetBytes(jsonData);

        int resultSize = _sock.Send(sendByte);
        DebugLogList("DungeonHellItemPickUp() end");
    }

    public void OpenInventory()
	{
		DebugLogList("OpenInventory() start");
		string jsonData;
		char endNullValue = '\0';

		var packHeader = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_INVENTORY_OPEN,
			packetSize = (short)DefineDefaultValue.packetSize
		};
		var packData = new PKT_REQ_INVENTORY_OPEN
		{ 
			header = packHeader,
			userID = _accountId
		};
		DebugLogList(packData.ToString());
		jsonData = JsonConvert.SerializeObject(packData);
		jsonData += endNullValue;
		DebugLogList(jsonData.ToString());
		byte[] sendByte = new byte[512];
		sendByte = Encoding.UTF8.GetBytes(jsonData);

		int resultSize = _sock.Send(sendByte);
		DebugLogList("OpenInventory() end");
	}

	public void CloseInventory()
	{
		DebugLogList("CloseInventory() start");
		string jsonData;
		char endNullValue = '\0';

		var packHeader = new PACKET_HEADER
		{
			packetIndex = (short)PACKET_INDEX.REQ_INVENTORY_CLOSE,
			packetSize = (short)DefineDefaultValue.packetSize
		};
		var packData = new PKT_REQ_INVENTORY_CLOSE
		{
			header = packHeader,
			userID = _accountId,
			equip = NetworkInventoryInfoSaver.Instance.EquipIDs,
			inventory = NetworkInventoryInfoSaver.Instance.InventoryIDs
		};
		DebugLogList(packData.ToString());
		jsonData = JsonConvert.SerializeObject(packData);
		jsonData += endNullValue;
		DebugLogList(jsonData.ToString());
		byte[] sendByte = new byte[512];
		sendByte = Encoding.UTF8.GetBytes(jsonData);

		int resultSize = _sock.Send(sendByte);
		DebugLogList("CloseInventory() end");
	}
}

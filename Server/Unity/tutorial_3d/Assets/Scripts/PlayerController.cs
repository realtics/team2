using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System;

using Newtonsoft.Json;

public struct PACKET_HEADER
{
    public int packetIndex;
    public int packetSize;
}
struct PACKET_CHARACTER_MOVE
{
    public PACKET_HEADER header;
    public float characterMoveX;
    public float characterMoveY;
};

public struct TempPacket
{
    public PACKET_HEADER header;
}

[System.Serializable]
public class Data
{
    public int _nLevel;
    public Vector3 _vecPositon;

    public void printData()
    {
        Debug.Log("Level : " + _nLevel);
        Debug.Log("Position : " + _vecPositon);
    }
}


public class PlayerController : MonoBehaviour
{
    private Socket sock = null;

    public float speed;
    public Text countText;
    public Text winText;

    Data data = new Data();

    Rigidbody rb;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;
        SetCountText();
        winText.text = "";



        ////////////////
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        if (sock == null)
        {
            Debug.Log("소켓생성 실패");
        }
        sock.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 31452));





    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal"); //좌우 방향키 체크
        float vertical = Input.GetAxis("Vertical");     //위아래 방향키 체크
        bool playerStop = Input.GetKey(KeyCode.Keypad0);

        if (playerStop == true)
        {
            //Vector3 movement = new Vector3(0f, 0f, 0f);
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;

            data._vecPositon = new Vector3(0.0f, 0.0f, 0.0f);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            string str = JsonUtility.ToJson(data);
            Debug.Log("ToJson : " + str);



        }
        else
        {
            //Vector3 movement = new Vector3(horizontal, 0f, vertical);//입력받은 방향대로 벡터값을 저장
            //rb.AddForce(movement * speed);  //rigidbody에 힘을 가함

            data._vecPositon = new Vector3(horizontal, 0.0f, vertical);
            rb.AddForce(data._vecPositon * speed);  //rigidbody에 힘을 가함

            string str = JsonUtility.ToJson(data);
            Debug.Log("ToJson : " + str);

            //var data2 = JsonUtility.FromJson(str);
            //data2.printData();




            //서버에 보내기
            Vector3 objectPos = this.gameObject.transform.position;

            var packckHeader = new PACKET_HEADER { packetIndex = 3, packetSize = 10 };
            var packckCharacterMove = new PACKET_CHARACTER_MOVE { header = packckHeader, characterMoveX = objectPos.x, characterMoveY = objectPos.y };
            string json;
            json = JsonConvert.SerializeObject(packckCharacterMove);
            char charNull = '\0';
            json += charNull;
            byte[] sendBuf = new byte[128];
            sendBuf = Encoding.UTF8.GetBytes(json);
            sock.Send(sendBuf);


            //서버에서 받는것 처리
            byte[] recvBuf = new byte[128];
            int socketRecive = sock.Receive(recvBuf);
            Debug.Log("recv");
            Debug.Log(socketRecive);    //91

            string recvData = Encoding.UTF8.GetString(recvBuf, 0, socketRecive);
            //int bufLen = Encoding.Default.GetBytes(bufRecv);
            int bufLen = recvBuf.Length;
            Debug.Log(recvData);    //{"header":{"packetIndex":"3","packetSize":"12"},"characterMoveX":"2","characterMoveY":"1"}

            var data2 = JsonConvert.DeserializeObject<TempPacket>(recvData);
            if (data2.header.packetIndex == 3)
            {
                var packetTemp = JsonConvert.DeserializeObject<PACKET_CHARACTER_MOVE>(recvData);
                Debug.Log(packetTemp.characterMoveX);
                Debug.Log(packetTemp.characterMoveY);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            other.gameObject.SetActive(false);

            data._nLevel++;

            count += 1;
            SetCountText();
        }
        
        if(count >= 8)
        {
            winText.text = "Game End";
        }
    }

    void SetCountText()
    {
        countText.text = "Count : " + count.ToString();
    }
}

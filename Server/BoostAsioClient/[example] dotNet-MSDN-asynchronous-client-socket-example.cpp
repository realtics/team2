using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Threading;  
using System.Text;  
  
// 원격 장치에서 데이터를 수신하기위한 상태 객체.
public class StateObject {  
	// 클라이언트 소켓.
    public Socket workSocket = null;  
	// 수신 버퍼의 크기.
    public const int BufferSize = 256;  
	// 수신 버퍼.
    public byte[] buffer = new byte[BufferSize];  
	//받은 데이터 문자열.
    public StringBuilder sb = new StringBuilder();  
}  
  
public class AsynchronousClient {  
	// 원격 장치의 포트 번호입니다.
    private const int port = 11000;
  
	// ManualResetEvent 인스턴스 신호 완료.
    private static ManualResetEvent connectDone = new ManualResetEvent(false);  
    private static ManualResetEvent sendDone = new ManualResetEvent(false);  
    private static ManualResetEvent receiveDone = new ManualResetEvent(false);  
  
	// 원격 장치의 응답입니다.
    private static String response = String.Empty;  
  
    private static void StartClient() {  
		// 원격 장치에 연결합니다.
        try {  
			// 소켓의 원격 엔드 포인트를 설정하십시오.
			// 이름
			// 원격 장치는 "host.contoso.com"입니다.
            IPHostEntry ipHostInfo = Dns.GetHostEntry("host.contoso.com");  
            IPAddress ipAddress = ipHostInfo.AddressList[0];  
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);  
  
			// TCP / IP 소켓을 만듭니다.
            Socket client = new Socket(ipAddress.AddressFamily,  
                SocketType.Stream, ProtocolType.Tcp);  
  
			// 원격 엔드 포인트에 연결합니다.
            client.BeginConnect( remoteEP,   
                new AsyncCallback(ConnectCallback), client);  
            connectDone.WaitOne();  
  
			// 테스트 데이터를 원격 장치로 보냅니다.
            Send(client,"This is a test<EOF>");  
            sendDone.WaitOne();  
  
			// 원격 장치로부터 응답을 받습니다.
            Receive(client);  
            receiveDone.WaitOne();  
  
			// 콘솔에 응답을 작성합니다.
            Console.WriteLine("Response received : {0}", response);  
  
			// 소켓을 놓습니다.
            client.Shutdown(SocketShutdown.Both);  
            client.Close();  
  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void ConnectCallback(IAsyncResult ar) {  
        try {  
			// 상태 객체에서 소켓을 검색합니다.
            Socket client = (Socket) ar.AsyncState;  
  
			// 연결을 완료하십시오.
            client.EndConnect(ar);  
  
            Console.WriteLine("Socket connected to {0}",  
                client.RemoteEndPoint.ToString());  
  
			// 연결되었음을 알립니다.
            connectDone.Set();  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void Receive(Socket client) {  
        try {  
			// 상태 객체를 만듭니다.
            StateObject state = new StateObject();  
            state.workSocket = client;  
  
			// 원격 장치에서 데이터 수신을 시작합니다.
            client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,  
                new AsyncCallback(ReceiveCallback), state);  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void ReceiveCallback( IAsyncResult ar ) {  
        try {  
			// 상태 객체와 클라이언트 소켓을 검색
			// 비동기 상태 객체에서.
            StateObject state = (StateObject) ar.AsyncState;  
            Socket client = state.workSocket;  
  
			// 원격 장치에서 데이터를 읽습니다. 
            int bytesRead = client.EndReceive(ar);  
  
            if (bytesRead > 0) {  
				// 더 많은 데이터가있을 수 있으므로 지금까지 수신 한 데이터를 저장하십시오.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));  
  
				// 나머지 데이터를 가져옵니다.
                client.BeginReceive(state.buffer,0,StateObject.BufferSize,0,  
                    new AsyncCallback(ReceiveCallback), state);  
            } else {  
				// 모든 데이터가 도착했습니다. 응답으로 넣습니다.
                if (state.sb.Length > 1) {  
                    response = state.sb.ToString();  
                }  
				// 모든 바이트가 수신되었음을 알립니다.
                receiveDone.Set();  
            }  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void Send(Socket client, String data) {  
		// ASCII 인코딩을 사용하여 문자열 데이터를 바이트 데이터로 변환합니다.
        byte[] byteData = Encoding.ASCII.GetBytes(data);  
  
		// 원격 장치로 데이터 전송을 시작합니다.
        client.BeginSend(byteData, 0, byteData.Length, 0,  
            new AsyncCallback(SendCallback), client);  
    }  
  
    private static void SendCallback(IAsyncResult ar) {  
        try {  
			// 상태 객체에서 소켓을 검색합니다.
            Socket client = (Socket) ar.AsyncState;  
  
			// 원격 장치로 데이터 전송을 완료합니다.
            int bytesSent = client.EndSend(ar);  
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);  
  
			// 모든 바이트가 전송되었음을 알립니다.
            sendDone.Set();  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    public static int Main(String[] args) {  
        StartClient();  
        return 0;  
    }  
}  
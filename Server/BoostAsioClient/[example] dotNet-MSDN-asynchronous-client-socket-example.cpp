using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Threading;  
using System.Text;  
  
// ���� ��ġ���� �����͸� �����ϱ����� ���� ��ü.
public class StateObject {  
	// Ŭ���̾�Ʈ ����.
    public Socket workSocket = null;  
	// ���� ������ ũ��.
    public const int BufferSize = 256;  
	// ���� ����.
    public byte[] buffer = new byte[BufferSize];  
	//���� ������ ���ڿ�.
    public StringBuilder sb = new StringBuilder();  
}  
  
public class AsynchronousClient {  
	// ���� ��ġ�� ��Ʈ ��ȣ�Դϴ�.
    private const int port = 11000;
  
	// ManualResetEvent �ν��Ͻ� ��ȣ �Ϸ�.
    private static ManualResetEvent connectDone = new ManualResetEvent(false);  
    private static ManualResetEvent sendDone = new ManualResetEvent(false);  
    private static ManualResetEvent receiveDone = new ManualResetEvent(false);  
  
	// ���� ��ġ�� �����Դϴ�.
    private static String response = String.Empty;  
  
    private static void StartClient() {  
		// ���� ��ġ�� �����մϴ�.
        try {  
			// ������ ���� ���� ����Ʈ�� �����Ͻʽÿ�.
			// �̸�
			// ���� ��ġ�� "host.contoso.com"�Դϴ�.
            IPHostEntry ipHostInfo = Dns.GetHostEntry("host.contoso.com");  
            IPAddress ipAddress = ipHostInfo.AddressList[0];  
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);  
  
			// TCP / IP ������ ����ϴ�.
            Socket client = new Socket(ipAddress.AddressFamily,  
                SocketType.Stream, ProtocolType.Tcp);  
  
			// ���� ���� ����Ʈ�� �����մϴ�.
            client.BeginConnect( remoteEP,   
                new AsyncCallback(ConnectCallback), client);  
            connectDone.WaitOne();  
  
			// �׽�Ʈ �����͸� ���� ��ġ�� �����ϴ�.
            Send(client,"This is a test<EOF>");  
            sendDone.WaitOne();  
  
			// ���� ��ġ�κ��� ������ �޽��ϴ�.
            Receive(client);  
            receiveDone.WaitOne();  
  
			// �ֿܼ� ������ �ۼ��մϴ�.
            Console.WriteLine("Response received : {0}", response);  
  
			// ������ �����ϴ�.
            client.Shutdown(SocketShutdown.Both);  
            client.Close();  
  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void ConnectCallback(IAsyncResult ar) {  
        try {  
			// ���� ��ü���� ������ �˻��մϴ�.
            Socket client = (Socket) ar.AsyncState;  
  
			// ������ �Ϸ��Ͻʽÿ�.
            client.EndConnect(ar);  
  
            Console.WriteLine("Socket connected to {0}",  
                client.RemoteEndPoint.ToString());  
  
			// ����Ǿ����� �˸��ϴ�.
            connectDone.Set();  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void Receive(Socket client) {  
        try {  
			// ���� ��ü�� ����ϴ�.
            StateObject state = new StateObject();  
            state.workSocket = client;  
  
			// ���� ��ġ���� ������ ������ �����մϴ�.
            client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,  
                new AsyncCallback(ReceiveCallback), state);  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void ReceiveCallback( IAsyncResult ar ) {  
        try {  
			// ���� ��ü�� Ŭ���̾�Ʈ ������ �˻�
			// �񵿱� ���� ��ü����.
            StateObject state = (StateObject) ar.AsyncState;  
            Socket client = state.workSocket;  
  
			// ���� ��ġ���� �����͸� �н��ϴ�. 
            int bytesRead = client.EndReceive(ar);  
  
            if (bytesRead > 0) {  
				// �� ���� �����Ͱ����� �� �����Ƿ� ���ݱ��� ���� �� �����͸� �����Ͻʽÿ�.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));  
  
				// ������ �����͸� �����ɴϴ�.
                client.BeginReceive(state.buffer,0,StateObject.BufferSize,0,  
                    new AsyncCallback(ReceiveCallback), state);  
            } else {  
				// ��� �����Ͱ� �����߽��ϴ�. �������� �ֽ��ϴ�.
                if (state.sb.Length > 1) {  
                    response = state.sb.ToString();  
                }  
				// ��� ����Ʈ�� ���ŵǾ����� �˸��ϴ�.
                receiveDone.Set();  
            }  
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  
  
    private static void Send(Socket client, String data) {  
		// ASCII ���ڵ��� ����Ͽ� ���ڿ� �����͸� ����Ʈ �����ͷ� ��ȯ�մϴ�.
        byte[] byteData = Encoding.ASCII.GetBytes(data);  
  
		// ���� ��ġ�� ������ ������ �����մϴ�.
        client.BeginSend(byteData, 0, byteData.Length, 0,  
            new AsyncCallback(SendCallback), client);  
    }  
  
    private static void SendCallback(IAsyncResult ar) {  
        try {  
			// ���� ��ü���� ������ �˻��մϴ�.
            Socket client = (Socket) ar.AsyncState;  
  
			// ���� ��ġ�� ������ ������ �Ϸ��մϴ�.
            int bytesSent = client.EndSend(ar);  
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);  
  
			// ��� ����Ʈ�� ���۵Ǿ����� �˸��ϴ�.
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
#include "Session.h"
#include "AsioServer.h"

Session::Session(int sessionID, boost::asio::io_context& io_context, AsioServer* pServer)
	: _socket(io_context)
	, _sessionID(sessionID)
	, _pServer(pServer)
{
}

Session::~Session()
{
	while (_sendDataQueue.empty() == false)
	{
		delete[] _sendDataQueue.front();
		_sendDataQueue.pop_front();
	}
}

void Session::Init()
{
	_packetBufferMark = 0;
}

void Session::PostReceive()
{
	_socket.async_read_some
	(
		boost::asio::buffer(_receiveBuffer),
							boost::bind(&Session::HandleReceive,
							this,
							boost::asio::placeholders::error,
							boost::asio::placeholders::bytes_transferred)

	);
}

void Session::PostSend(const bool bImmediately, const int packetSize, char* pData)
{
	char* pSendData = nullptr;

	if (bImmediately == false)
	{
		pSendData = new char[packetSize];
		memcpy(pSendData, pData, packetSize);

		_sendDataQueue.push_back(pSendData);
	}
	else
	{
		pSendData = pData;
	}

	if (bImmediately == false && _sendDataQueue.size() > 1)
	{
		return;
	}
	
	boost::asio::async_write(_socket,
		boost::asio::buffer(pSendData, packetSize),
							boost::bind(&Session::HandleWrite,
								this,
								boost::asio::placeholders::error,
								boost::asio::placeholders::bytes_transferred)
	);
}

void Session::HandleWrite(const boost::system::error_code& error, size_t bytes_transferred)
{
	delete[] _sendDataQueue.front();
	_sendDataQueue.pop_front();

	if (_sendDataQueue.empty() == false)
	{
		char* pData = _sendDataQueue.front();

		PACKET_HEADER* pHeader = (PACKET_HEADER*)pData;

		PostSend(true, pHeader->packetSize, pData);
	}
}

void Session::HandleReceive(const boost::system::error_code& error, size_t bytes_transferred)
{
	if (error)
	{
		if (error == boost::asio::error::eof)
		{
			std::cout << "클라이언트와 연결이 끊어졌습니다. ";
		}
		else
		{
			std::cout << "error No: " << error.value() << " error Message: " << error.message() << std::endl;
		}

		_pServer->CloseSession(_sessionID);
	}
	else
	{
		_packetBufferMark = 0;
		Deserialization(_receiveBuffer.data());
		//memcpy(&_packetBuffer[_packetBufferMark], _receiveBuffer.data(), bytes_transferred);
		
		int packetData = _packetBufferMark + bytes_transferred;
		int readData = 0;
//		std::cout << "packetData : " << packetData << std::endl;
		PACKET_HEADER* pHeader = (PACKET_HEADER*)&_packetBuffer[readData];

		while (packetData > 0)
		{
			if (packetData < sizeof(PACKET_HEADER))
			{
				break;
			}

			if (pHeader->packetSize <= packetData)
			{
				_pServer->ProcessPacket(_sessionID, &_packetBuffer[readData]);

				packetData -= pHeader->packetSize;
				readData += pHeader->packetSize;
			}
			else
			{
				Sleep(100);
				packetData = 0;
				_packetBufferMark = 0;
				break;
			}
		}

		if (packetData > 0)
		{
			char TempBuffer[MAX_RECEIVE_BUFFER_LEN] = { 0, };
			memcpy(&TempBuffer[0], &_packetBuffer[readData], packetData);
			memcpy(&_packetBuffer[0], &TempBuffer[0], packetData);
		}

		_packetBufferMark = packetData;

		PostReceive();
	}
}

void Session::Deserialization(char* jsonData)
{
	short packetSize = strlen(jsonData) + 1;

	//std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;

	//std::string stringJsonData1 = jsonData;
	//std::string stringJsonData2 = stringJsonData1.substr(25, 3);
	//int packetIndex = atoi(stringJsonData2.c_str());

	boost::property_tree::ptree ptRecv;
	std::istringstream iss(jsonData);
	boost::property_tree::read_json(iss, ptRecv);

	boost::property_tree::ptree& children = ptRecv.get_child("header");
	short packetIndex = children.get<short>("packetIndex");

	//short packetSize = children.get<short>("packetSize");
	
	switch (packetIndex)
	{
	case PACKET_INDEX::REQ_SIGN_UP:
	{
		std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;
		PKT_REQ_SIGN_UP packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		strcpy_s(packet.userID, MAX_USER_ID, ptRecv.get<std::string>("userID").c_str());
		strcpy_s(packet.userPW, MAX_USER_PW, ptRecv.get<std::string>("userPW").c_str());
		strcpy_s(packet.userName, MAX_USER_NAME, ptRecv.get<std::string>("userName").c_str());
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	case PACKET_INDEX::REQ_CHECK_BEFORE_LOGIN:
	{
		std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;
		PKT_REQ_CHECK_BEFORE_LOGIN packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		strcpy_s(packet.userID, MAX_USER_ID, ptRecv.get<std::string>("userID").c_str());
		strcpy_s(packet.userPW, MAX_USER_PW, ptRecv.get<std::string>("userPW").c_str());
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	case PACKET_INDEX::REQ_NEW_LOGIN:
	{
		std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;
		PKT_REQ_NEW_LOGIN packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	case PACKET_INDEX::REQ_CONCURRENT_USER:
	{
		std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;
		PKT_REQ_CONCURRENT_USER packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	case PACKET_INDEX::REQ_USER_EXIT:
	{
		std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;
		PKT_REQ_USER_EXIT packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		packet.sessionID = ptRecv.get<int>("sessionID");
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	case PACKET_INDEX::REQ_CHATTING:
	{
		std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;
		PKT_REQ_CHATTING packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		packet.sessionID = ptRecv.get<int>("sessionID");
		strcpy_s(packet.chatMessage, MAX_MESSAGE_LEN, ptRecv.get<std::string>("chatMessage").c_str());
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	case PACKET_INDEX::REQ_PLAYER_MOVE_START:
	{
		PKT_REQ_PLAYER_MOVE_START packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		packet.sessionID = ptRecv.get<int>("sessionID");
		strcpy_s(packet.userPos, MAX_PLAYER_MOVE_LEN, ptRecv.get<std::string>("userPos").c_str());
		strcpy_s(packet.userDir, MAX_PLAYER_MOVE_LEN, ptRecv.get<std::string>("userDir").c_str());
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	case PACKET_INDEX::REQ_PLAYER_MOVE_END:
	{
		std::cout << "[클라->서버][Size:" << packetSize << "] " << jsonData << std::endl;
		PKT_REQ_PLAYER_MOVE_END packet;
		packet.Init();
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		packet.sessionID = ptRecv.get<int>("sessionID");
		strcpy_s(packet.userPos, MAX_PLAYER_MOVE_LEN, ptRecv.get<std::string>("userPos").c_str());
		strcpy_s(packet.userDir, MAX_PLAYER_MOVE_LEN, ptRecv.get<std::string>("userDir").c_str());
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
	}
	break;
	default:
	{
		std::cout << "Index가 존재 하지 않는 Packet : " << packetIndex << std::endl;
	}
	break;
	}
}

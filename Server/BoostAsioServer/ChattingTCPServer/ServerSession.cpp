#include "ServerSession.h"
#include "ChatServer.h"


Session::Session(int nSessionID, boost::asio::io_context& io_service, ChatServer* pServer)
	: _Socket(io_service)
	, _nSessionID(nSessionID)
	, _pServer(pServer)
{
}

Session::~Session()
{
	while (_SendDataQueue.empty() == false)
	{
		delete[] _SendDataQueue.front();
		_SendDataQueue.pop_front();
	}
}

void Session::Init()
{
	_nPacketBufferMark = 0;
}

void Session::PostReceive()
{
	_Socket.async_read_some
	(
		boost::asio::buffer(_ReceiveBuffer),
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

		_SendDataQueue.push_back(pSendData);
	}
	else
	{
		pSendData = pData;
	}
	
	if (bImmediately == false && _SendDataQueue.size() > 1)
	{
		return;
	}

	boost::asio::async_write(_Socket,
							boost::asio::buffer(pSendData, packetSize),
							boost::bind(&Session::HandleWrite,
										this,
										boost::asio::placeholders::error,
										boost::asio::placeholders::bytes_transferred)
	);
}

void Session::HandleWrite(const boost::system::error_code& error, size_t bytes_transferred)
{
	delete[] _SendDataQueue.front();
	_SendDataQueue.pop_front();

	if (_SendDataQueue.empty() == false)
	{
		char* pData = _SendDataQueue.front();

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
			std::cout << "클라이언트와 연결이 끊어졌습니다" << std::endl;
		}
		else
		{
			std::cout << "error No: " << error.value() << " error Message: " << error.message() << std::endl;
		}

		_pServer->CloseSession(_nSessionID);
	}
	else
	{
		memcpy(&_PacketBuffer[_nPacketBufferMark], _ReceiveBuffer.data(), bytes_transferred);

		int nPacketData = _nPacketBufferMark + bytes_transferred;
		int nReadData = 0;


///////////////////
		//std::wstring strUni = L"유니코드";
		//int len = WideCharToMultiByte(CP_ACP, 0, &strUni[0], -1, NULL, 0, NULL, NULL);
		//std::string strMulti(len, 0);
		//WideCharToMultiByte(CP_ACP, 0, &strUni[0], -1, &strMulti[0], len, NULL, NULL);

		

		std::string stringReceiveBuffer;
		stringReceiveBuffer = _ReceiveBuffer.data();
		
		std::cout << stringReceiveBuffer << std::endl;

		boost::property_tree::ptree ptRecv;
		std::istringstream is(stringReceiveBuffer);
		boost::property_tree::read_json(is, ptRecv);

		

		boost::property_tree::ptree& children = ptRecv.get_child("header");
		int headerIndex = children.get<int>("packetIndex");
		int packetSize = children.get<int>("packetSize");

		std::string jsonString = ptRecv.get<std::string>("han");

		std::cout << "클라에서 보낸 json 값 추출 : ";
		std::cout << "headerIndex:" << headerIndex << ", ";
		std::cout << "packetSize:" << packetSize << ", ";
		std::cout << "han:" << jsonString << std::endl;

		PKT_RES_CHATTING packet;
		packet.chatMsg = jsonString;

		char _packetBuffer[MAX_RECEIVE_BUFFER_LEN * 2];
		memcpy(&_packetBuffer[0], (char*)&packet, sizeof(packet));

		const char* pData = &_packetBuffer[0];

		PKT_RES_CHATTING* pPacket = (PKT_RES_CHATTING*)pData;
		

////////////////클라에 json 형태로 보내기

		//{"header":{"packetIndex":1,"packetSize":??},"characterMoveX":1,"characterMoveY":2}
		//{"header":{"packetIndex":1,"packetSize":??},"characterMoveX":1,"String":"string"}
		
		boost::property_tree::ptree ptSend;
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<int>("packetIndex", 199);
		ptSendHeader.put<int>("packetSize", 10);
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<std::string>("han", pPacket->chatMsg);
		//ptSend.put<std::string>("String", packetCharacterMove.string);
		

		std::string stringRecv;
		std::ostringstream os(stringRecv);

		//boost::property_tree::json_parser::create_escapes(stringRecv);

		boost::property_tree::write_json(os, ptSend, false);
		std::string sendStr = os.str();
		std::cout << sendStr << std::endl;
		PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());



		while (nPacketData > 0)
		{
			if (nPacketData < sizeof(PACKET_HEADER))
			{
				break;
			}

			PACKET_HEADER* pHeader = (PACKET_HEADER*)&_PacketBuffer[nReadData];

			if (pHeader->packetSize <= nPacketData)
			{
				_pServer->ProcessPacket(_nSessionID, &_PacketBuffer[nReadData]);

				nPacketData -= pHeader->packetSize;
				nReadData += pHeader->packetSize;
			}
			else
			{
				nPacketData = 0;
				_nPacketBufferMark = 0;
				break;
			}
		}

		if (nPacketData > 0)
		{
			char TempBuffer[MAX_RECEIVE_BUFFER_LEN] = { 0, };
			memcpy(&TempBuffer[0], &_PacketBuffer[nReadData], nPacketData);
			memcpy(&_PacketBuffer[0], &TempBuffer[0], nPacketData);
		}

		_nPacketBufferMark = nPacketData;

		PostReceive();
	}
}


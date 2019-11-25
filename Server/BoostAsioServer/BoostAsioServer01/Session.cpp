#include "Session.h"
#include "AsioServer.h"


Session::Session(int nSessionID, boost::asio::io_context& io_context, AsioServer* pServer)
	: _socket(io_context)
	, _sessionID(nSessionID)
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
			std::cout << "클라이언트와 연결이 끊어졌습니다" << std::endl;
		}
		else
		{
			std::cout << "error No: " << error.value() << " error Message: " << error.message() << std::endl;
		}

		_pServer->CloseSession(_sessionID);
	}
	else
	{
		Deserialization(_receiveBuffer.data());
		//memcpy(&_packetBuffer[_packetBufferMark], _receiveBuffer.data(), bytes_transferred);
		
		int nPacketData = _packetBufferMark + bytes_transferred;
		int nReadData = 0;

		while (nPacketData > 0)
		{
			if (nPacketData < sizeof(PACKET_HEADER))
			{
				break;
			}

			PACKET_HEADER* pHeader = (PACKET_HEADER*)&_packetBuffer[nReadData];

			if (pHeader->packetSize <= nPacketData)
			{
				_pServer->ProcessPacket(_sessionID, &_packetBuffer[nReadData]);

				nPacketData -= pHeader->packetSize;
				nReadData += pHeader->packetSize;
			}
			else
			{
				nPacketData = 0;
				_packetBufferMark = 0;
				break;
			}
		}

		if (nPacketData > 0)
		{
			char TempBuffer[MAX_RECEIVE_BUFFER_LEN] = { 0, };
			memcpy(&TempBuffer[0], &_packetBuffer[nReadData], nPacketData);
			memcpy(&_packetBuffer[0], &TempBuffer[0], nPacketData);
		}

		_packetBufferMark = nPacketData;

		PostReceive();
	}
}

void Session::Deserialization(char* jsonData)
{
	std::cout << "[클라->서버 JSON] " << jsonData << std::endl;

	boost::property_tree::ptree ptRecv;
	std::istringstream iss(jsonData);
	boost::property_tree::read_json(iss, ptRecv);

	boost::property_tree::ptree& children = ptRecv.get_child("header");
	short packetIndex = children.get<short>("packetIndex");
	short packetSize = children.get<short>("packetSize");

	switch (packetIndex)
	{
	case PACKET_INDEX::NEW_LOGIN:
		PACKET_HEADER packet;
		packet.packetIndex = packetIndex;
		packet.packetSize = packetSize;
		memcpy(&_packetBuffer[_packetBufferMark], (char*)&packet, sizeof(packet));
		
	break;

	}

}


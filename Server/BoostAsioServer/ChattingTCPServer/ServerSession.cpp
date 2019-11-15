#include "ServerSession.h"
#include "ChattingServer.h"


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
							boost::bind(&Session::handle_receive,
										this,
										boost::asio::placeholders::error,
										boost::asio::placeholders::bytes_transferred)

	);
}

void Session::PostSend(const bool bImmediately, const int nSize, char* pData)
{
	char* pSendData = nullptr;

	if (bImmediately == false)
	{
		pSendData = new char[nSize];
		memcpy(pSendData, pData, nSize);

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
							boost::asio::buffer(pSendData, nSize),
							boost::bind(&Session::handle_write,
										this,
										boost::asio::placeholders::error,
										boost::asio::placeholders::bytes_transferred)
	);
}

void Session::handle_write(const boost::system::error_code& error, size_t bytes_transferred)
{
	delete[] _SendDataQueue.front();
	_SendDataQueue.pop_front();

	if (_SendDataQueue.empty() == false)
	{
		char* pData = _SendDataQueue.front();

		PACKET_HEADER* pHeader = (PACKET_HEADER*)pData;

		PostSend(true, pHeader->nSize, pData);
	}
}

void Session::handle_receive(const boost::system::error_code& error, size_t bytes_transferred)
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

		while (nPacketData > 0)
		{
			if (nPacketData < sizeof(PACKET_HEADER))
			{
				break;
			}

			PACKET_HEADER* pHeader = (PACKET_HEADER*)&_PacketBuffer[nReadData];

			if (pHeader->nSize <= nPacketData)
			{
				_pServer->ProcessPacket(_nSessionID, &_PacketBuffer[nReadData]);

				nPacketData -= pHeader->nSize;
				nReadData += pHeader->nSize;
			}
			else
			{
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


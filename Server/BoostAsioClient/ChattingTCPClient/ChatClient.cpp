#include "ChatClient.h"

ChatClient::ChatClient(boost::asio::io_context& io_context) : _IOContext(io_context), _Socket(io_context)
{
	_nPacketBufferMark = 0;
	_PacketBuffer[0] = { 0, };
	_ReceiveBuffer = { 0 };

	_bIsLogin = false;
	InitializeCriticalSectionAndSpinCount(&_lock, 4000);
}

ChatClient::~ChatClient()
{
	EnterCriticalSection(&_lock);

	while (_SendDataQueue.empty() == false)
	{
		delete[] _SendDataQueue.front();
		_SendDataQueue.pop_front();
	}

	LeaveCriticalSection(&_lock);

	DeleteCriticalSection(&_lock);
}

void ChatClient::PostReceive()
{
	memset(&_ReceiveBuffer, '\0', sizeof(_ReceiveBuffer));

	_Socket.async_read_some
	(
		boost::asio::buffer(_ReceiveBuffer),
							boost::bind(&ChatClient::handle_receive,
										this,
										boost::asio::placeholders::error,
										boost::asio::placeholders::bytes_transferred)
	);
}

void ChatClient::handle_connect(const boost::system::error_code& error)
{
	if (!error)
	{
		std::cout << "서버 접속 성공" << std::endl;
		std::cout << "이름을 입력하세요!!" << std::endl;

		PostReceive();
	}
	else
	{
		std::cout << "서버 접속 실패. error No: " << error.value() << " error Message: " << error.message() << std::endl;
	}
}

void ChatClient::handle_write(const boost::system::error_code& error, size_t bytes_transferred)
{
	EnterCriticalSection(&_lock);			// 락 시작

	delete[] _SendDataQueue.front();
	_SendDataQueue.pop_front();

	char* pData = nullptr;

	if (_SendDataQueue.empty() == false)
	{
		pData = _SendDataQueue.front();
	}

	LeaveCriticalSection(&_lock);			// 락 완료

	if (pData != nullptr)
	{
		PACKET_HEADER* pHeader = (PACKET_HEADER*)pData;
		PostSend(true, pHeader->packetSize, pData);
	}
}

void ChatClient::handle_receive(const boost::system::error_code& error, size_t bytes_transferred)
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

		Close();
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

			if (pHeader->packetSize <= nPacketData)
			{
				ProcessPacket(&_PacketBuffer[nReadData]);

				nPacketData -= pHeader->packetSize;
				nReadData += pHeader->packetSize;
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

void ChatClient::ProcessPacket(const char* pData)
{
	PACKET_HEADER* pheader = (PACKET_HEADER*)pData;

	switch (pheader->packetIndex)
	{
		case RES_IN:
		{
			PKT_RES_IN* pPacket = (PKT_RES_IN*)pData;

			LoginOK();

			std::cout << "클라이언트 로그인 성공 ?: " << pPacket->isSuccess << std::endl;
		}
		break;
		case NOTICE_CHAT:
		{
			PKT_NOTICE_CHAT* pPacket = (PKT_NOTICE_CHAT*)pData;

			std::cout << pPacket->characterName << ": " << pPacket->userMessage << std::endl;
		}
		break;
	}
}

void ChatClient::Connect(boost::asio::ip::tcp::endpoint endpoint)
{
	_nPacketBufferMark = 0;

	_Socket.async_connect(endpoint,
							boost::bind(&ChatClient::handle_connect,
										this,
										boost::asio::placeholders::error)
	);
}

void ChatClient::Close()
{
	if (_Socket.is_open())
	{
		_Socket.close();
	}
}

void ChatClient::PostSend(const bool bImmediately, const int packetSize, char* pData)
{
	char* pSendData = nullptr;

	EnterCriticalSection(&_lock);		// 락 시작

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

	if (bImmediately || _SendDataQueue.size() < 2)
	{
		boost::asio::async_write(_Socket,
								boost::asio::buffer(pSendData, packetSize),
								boost::bind(&ChatClient::handle_write,
											this,
											boost::asio::placeholders::error,
											boost::asio::placeholders::bytes_transferred)
		);
	}

	LeaveCriticalSection(&_lock);		// 락 완료
}
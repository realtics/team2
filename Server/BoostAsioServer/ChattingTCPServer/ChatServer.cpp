#include "ChatServer.h"

bool ChatServer::PostAccept()
{
	if (_SessionQueue.empty())
	{
		_bIsAccepting = false;
		return false;
	}

	_bIsAccepting = true;
	int nSessionID = _SessionQueue.front();

	_SessionQueue.pop_front();

	_acceptor.async_accept(_SessionList[nSessionID]->Socket(),
		boost::bind(&ChatServer::HandleAccept,
			this,
			_SessionList[nSessionID],
			boost::asio::placeholders::error)
	);

	return true;
}

void ChatServer::HandleAccept(Session* pSession, const boost::system::error_code& error)
{
	if (!error)
	{
		std::cout << "클라이언트 접속 성공. SessionID: " << pSession->SessionID() << std::endl;

		pSession->Init();
		pSession->PostReceive();

		PostAccept();
	}
	else
	{
		std::cout << "error No: " << error.value() << " error Message: " << error.message() << std::endl;
	}
}

ChatServer::ChatServer(boost::asio::io_context& io_service)
	: _acceptor(io_service, boost::asio::ip::tcp::endpoint(boost::asio::ip::tcp::v4(), PORT_NUMBER))
{
	_bIsAccepting = false;
}

ChatServer::~ChatServer()
{
	for (size_t i = 0; i < _SessionList.size(); ++i)
	{
		if (_SessionList[i]->Socket().is_open())
		{
			_SessionList[i]->Socket().close();
		}

		delete _SessionList[i];
	}
}

void ChatServer::Init(const int nMaxSessionCount)
{
	for (int i = 0; i < nMaxSessionCount; ++i)
	{
		Session* pSession = new Session(i, (boost::asio::io_context&)_acceptor.get_executor().context(), this);
		_SessionList.push_back(pSession);
		_SessionQueue.push_back(i);
	}
}

void ChatServer::Start()
{
	std::cout << "서버 시작....." << std::endl;

	PostAccept();
}

void ChatServer::CloseSession(const int nSessionID)
{
	std::cout << "클라이언트 접속 종료. 세션 ID: " << nSessionID << std::endl;

	_SessionList[nSessionID]->Socket().close();

	_SessionQueue.push_back(nSessionID);

	if (_bIsAccepting == false)
	{
		PostAccept();
	}
}

void ChatServer::ProcessPacket(const int nSessionID, const char* pData)
{
	PACKET_HEADER* pheader = (PACKET_HEADER*)pData;
	std::cout << "패킷사이즈 : " << pheader->packetSize << std::endl;

	switch (pheader->packetIndex)
	{
	case REQ_IN:
	{
		PKT_REQ_IN* pPacket = (PKT_REQ_IN*)pData;
		_SessionList[nSessionID]->SetName(pPacket->characterName);

		std::cout << "클라이언트 로그인 성공 Name: " << _SessionList[nSessionID]->GetName() << std::endl;

		PKT_RES_IN SendPkt;
		SendPkt.Init();
		SendPkt.isSuccess = true;

		_SessionList[nSessionID]->PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
	}
	break;
	case REQ_CHAT:
	{
		PKT_REQ_CHAT* pPacket = (PKT_REQ_CHAT*)pData;

		PKT_NOTICE_CHAT SendPkt;
		SendPkt.Init();
		strncpy_s(SendPkt.characterName, MAX_NAME_LEN, _SessionList[nSessionID]->GetName(), MAX_NAME_LEN - 1);
		strncpy_s(SendPkt.userMessage, MAX_MESSAGE_LEN, pPacket->userMessage, MAX_MESSAGE_LEN - 1);

		size_t nTotalSessionCount = _SessionList.size();

		for (size_t i = 0; i < nTotalSessionCount; ++i)
		{
			if (_SessionList[i]->Socket().is_open())
			{
				_SessionList[i]->PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
			}
		}
	}
	break;
	}

	return;
}
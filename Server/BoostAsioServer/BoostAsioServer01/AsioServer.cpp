#include "AsioServer.h"

AsioServer::AsioServer(boost::asio::io_context& io_context)
	: _acceptor(io_context, boost::asio::ip::tcp::endpoint(boost::asio::ip::tcp::v4(), PORT_NUMBER))
{
	_isAccepting = false;
	_PlayerID = 0;
}

AsioServer::~AsioServer()
{
	for (size_t i = 0; i < _sessionList.size(); ++i)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			_sessionList[i]->Socket().close();
		}

		delete _sessionList[i];
	}
}

void AsioServer::Init(const int nMaxSessionCount)
{
	for (int i = 0; i < nMaxSessionCount; ++i)
	{
		Session* pSession = new Session(i, (boost::asio::io_context&)_acceptor.get_executor().context(), this);
		_sessionList.push_back(pSession);
		_sessionQueue.push_back(i);
	}
}

void AsioServer::Start()
{
	std::cout << "서버 시작....." << std::endl;

	PostAccept();
}

bool AsioServer::PostAccept()
{
	if (_sessionQueue.empty())
	{
		_isAccepting = false;
		return false;
	}

	_isAccepting = true;
	int nSessionID = _sessionQueue.front();
	
	_PlayerID = nSessionID + 1000; // PlayerID는 1000부터 시작

	_sessionQueue.pop_front();

	_acceptor.async_accept(_sessionList[nSessionID]->Socket(),
							boost::bind(&AsioServer::HandleAccept,
								this,
								_sessionList[nSessionID],
								boost::asio::placeholders::error)
	);

	return true;
}

void AsioServer::HandleAccept(Session* pSession, const boost::system::error_code& error)
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

void AsioServer::CloseSession(const int nSessionID)
{
	std::cout << "클라이언트 접속 종료. 세션 ID: " << nSessionID << std::endl;

	_sessionList[nSessionID]->Socket().close();

	_sessionQueue.push_back(nSessionID);

	if (_isAccepting == false)
	{
		PostAccept();
	}
}

void AsioServer::ProcessPacket(const int nSessionID, const char* pData)
{
	PACKET_HEADER* pheader = (PACKET_HEADER*)pData;

	switch (pheader->packetIndex)
	{
	case PACKET_INDEX::REQ_IN:
	{
		PKT_REQ_IN* pPacket = (PKT_REQ_IN*)pData;
		_sessionList[nSessionID]->SetName(pPacket->characterName);

		std::cout << "클라이언트 로그인 성공 Name: " << _sessionList[nSessionID]->GetName() << std::endl;

		PKT_RES_IN SendPkt;
		SendPkt.Init();
		SendPkt.isSuccess = true;

		_sessionList[nSessionID]->PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
	}
	break;
	case PACKET_INDEX::REQ_CHAT:
	{
		PKT_REQ_CHAT* pPacket = (PKT_REQ_CHAT*)pData;

		PKT_NOTICE_CHAT SendPkt;
		SendPkt.Init();
		strncpy_s(SendPkt.characterName, MAX_NAME_LEN, _sessionList[nSessionID]->GetName(), MAX_NAME_LEN - 1);
		strncpy_s(SendPkt.userMessage, MAX_MESSAGE_LEN, pPacket->userMessage, MAX_MESSAGE_LEN - 1);

		size_t nTotalSessionCount = _sessionList.size();

		for (size_t i = 0; i < nTotalSessionCount; ++i)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				_sessionList[i]->PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
			}
		}
	}
	break;

	case PACKET_INDEX::NEW_LOGIN:
	{
		PACKET_NEW_LOGIN* pPacket = (PACKET_NEW_LOGIN*)pData;
		PACKET_NEW_LOGIN_SUCSESS SendPkt;

		std::cout << _PlayerID << "번 클라이언트 로그인 성공" << std::endl;

		SendPkt.packetIndex = PACKET_INDEX::NEW_LOGIN_SUCSESS;
		SendPkt.packetSize = 10;
		SendPkt.isSuccess = true;
		SendPkt.playerID = _PlayerID;

		boost::property_tree::ptree ptSend;
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<bool>("isSuccess", SendPkt.isSuccess);
		ptSend.put<int>("playerID", SendPkt.playerID);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		std::cout << "[서버->클라 JSON] " << sendStr << std::endl;

		size_t nTotalSessionCount = _sessionList.size();

		for (size_t i = 0; i < nTotalSessionCount; ++i)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				_sessionList[i]->PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
			}
		}

		//////
		//_sessionList[nSessionID]->PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());

		//////
		//SendPkt.Init();
		//SendPkt.isSuccess = true;
		//SendPkt.sessionID = nSessionID;

		//_sessionList[nSessionID]->PostSend(false, SendPkt.packetSize, (char*)&SendPkt);

	}
	break;

	}

	return;
}
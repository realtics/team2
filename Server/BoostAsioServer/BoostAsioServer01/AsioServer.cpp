#include "AsioServer.h"

AsioServer::AsioServer(boost::asio::io_context& io_context)
	: _acceptor(io_context, boost::asio::ip::tcp::endpoint(boost::asio::ip::tcp::v4(), PORT_NUMBER))
{
	_isAccepting = false;
	_userID = 0;
}

AsioServer::~AsioServer()
{
	for (size_t i = 0; i < _sessionList.size(); i++)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			_sessionList[i]->Socket().close();
		}

		delete _sessionList[i];
	}
}

void AsioServer::Init(const int maxSessionCount)
{
	for (int i = 0; i < maxSessionCount; i++)
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
	int sessionID = _sessionQueue.front();
	
	_sessionQueue.pop_front();

	_acceptor.async_accept(_sessionList[sessionID]->Socket(),
							boost::bind(&AsioServer::HandleAccept,
								this,
								_sessionList[sessionID],
								boost::asio::placeholders::error)
	);

	return true;
}

void AsioServer::HandleAccept(Session* pSession, const boost::system::error_code& error)
{
	_userID = pSession->SessionID() + FIRST_USER_INDEX;

	if (!error)
	{
		//std::cout << "클라이언트 접속 성공. SessionID: " << pSession->SessionID() << std::endl;
		std::cout << "\"" << _userID << "\"번 클라이언트 서버 접속 성공" << std::endl;

		pSession->Init();
		pSession->PostReceive();

		PostAccept();
	}
	else
	{
		std::cout << "error No: " << error.value() << " error Message: " << error.message() << std::endl;
	}
}

void AsioServer::CloseSession(const int sessionID)
{
	_userID = sessionID + FIRST_USER_INDEX;
	std::cout << "\"" << _userID << "\"번 클라이언트 접속 종료" << std::endl;

	_sessionList[sessionID]->Socket().close();

	_sessionQueue.push_back(sessionID);

	if (_isAccepting == false)
	{
		PostAccept();
	}

	ConcurrentUser();	//유저가 접속 종료 하면 클라이언트에게 갱신된 정보를 보냄
}

void AsioServer::ProcessPacket(const int sessionID, const char* pData)
{
	PACKET_HEADER* pHeader = (PACKET_HEADER*)pData;

	switch (pHeader->packetIndex)
	{
	case PACKET_INDEX::REQ_IN:
	{
		PKT_REQ_IN* pPacket = (PKT_REQ_IN*)pData;
		_sessionList[sessionID]->SetName(pPacket->characterName);

		std::cout << "클라이언트 로그인 성공 Name: " << _sessionList[sessionID]->GetName() << std::endl;

		PKT_RES_IN SendPkt;
		SendPkt.Init();
		SendPkt.isSuccess = true;

		_sessionList[sessionID]->PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
	}
	break;
	case PACKET_INDEX::REQ_CHAT:
	{
		PKT_REQ_CHAT* pPacket = (PKT_REQ_CHAT*)pData;

		PKT_NOTICE_CHAT SendPkt;
		SendPkt.Init();
		strncpy_s(SendPkt.characterName, MAX_NAME_LEN, _sessionList[sessionID]->GetName(), MAX_NAME_LEN - 1);
		strncpy_s(SendPkt.userMessage, MAX_MESSAGE_LEN, pPacket->userMessage, MAX_MESSAGE_LEN - 1);

		size_t totalSessionCount = _sessionList.size();

		for (size_t i = 0; i < totalSessionCount; i++)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				_sessionList[i]->PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
			}
		}
	}
	break;
	case PACKET_INDEX::REQ_NEW_LOGIN:
	{
		PKT_REQ_NEW_LOGIN* pPacket = (PKT_REQ_NEW_LOGIN*)pData;

		PKT_RES_NEW_LOGIN_SUCSESS SendPkt;
		SendPkt.Init();
		std::cout << "\"" << _userID << "\"번 클라이언트 로그인 성공" << std::endl;

		SendPkt.isSuccess = true;
		SendPkt.userID = _userID;

		boost::property_tree::ptree ptSend;
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<bool>("isSuccess", SendPkt.isSuccess);
		ptSend.put<int>("userID", SendPkt.userID);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		std::cout << "[서버->클라 JSON] " << sendStr << std::endl;

		size_t totalSessionCount = _sessionList.size();

		for (size_t i = 0; i < totalSessionCount; i++)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				_sessionList[i]->PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
			}
		}
	}
	break;
	case PACKET_INDEX::REQ_CONCURRENT_USER:
	{
		ConcurrentUser();
	}
	break;
	}

	return;
}

void AsioServer::ConcurrentUser()
{
	PKT_RES_CONCURRENT_USER_LIST concurrentUser;
	concurrentUser.packetIndex = PACKET_INDEX::RES_CONCURRENT_USER_LIST;
	concurrentUser.packetSize = sizeof(PKT_RES_CONCURRENT_USER_LIST);

	int totalUser = 0;
	std::string userList = "";

	for (size_t i = 0; i < _sessionList.size(); i++)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			totalUser++;

			int playerNum = i + FIRST_USER_INDEX;

			userList += std::to_string(playerNum);
			userList += ",";
		}
	}

	if (totalUser > 0)
	{
		// 마지막 , 없애기
		int endPlayerList = userList.size() - 1;

		userList.replace(endPlayerList, endPlayerList, "");
	}
	concurrentUser.Init();

	concurrentUser.totalUser = totalUser;
	concurrentUser.concurrentUserList = userList;

	boost::property_tree::ptree ptSendHeader;
	ptSendHeader.put<short>("packetIndex", concurrentUser.packetIndex);
	ptSendHeader.put<short>("packetSize", concurrentUser.packetSize);

	boost::property_tree::ptree ptSend;
	ptSend.add_child("header", ptSendHeader);
	ptSend.put<int>("totalUser", concurrentUser.totalUser);
	ptSend.put<std::string>("concurrentUser", concurrentUser.concurrentUserList);

	std::cout << "접속 유저 : " << concurrentUser.totalUser << std::endl;
	std::cout << "유저 리스트 : " << concurrentUser.concurrentUserList << std::endl;

	std::string stringRecv;
	std::ostringstream oss(stringRecv);
	boost::property_tree::write_json(oss, ptSend, false);
	std::string sendStr = oss.str();
	std::cout << sendStr << std::endl;

	size_t totalSessionCount = _sessionList.size();

	for (size_t i = 0; i < totalSessionCount; i++)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			_sessionList[i]->PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
		}
	}
}
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
	std::cout << "���� ����....." << std::endl;

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
		//std::cout << "Ŭ���̾�Ʈ ���� ����. SessionID: " << pSession->SessionID() << std::endl;
		std::cout << "\"" << _userID << "\"�� Ŭ���̾�Ʈ ���� ���� ����" << std::endl;

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
	std::cout << "\"" << _userID << "\"�� Ŭ���̾�Ʈ ���� ����" << std::endl;

	_sessionList[sessionID]->Socket().close();

	_sessionQueue.push_back(sessionID);

	if (_isAccepting == false)
	{
		PostAccept();
	}

	ConcurrentUser();	//������ ���� ���� �ϸ� Ŭ���̾�Ʈ���� ���ŵ� ������ ����
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

		std::cout << "Ŭ���̾�Ʈ �α��� ���� Name: " << _sessionList[sessionID]->GetName() << std::endl;

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
		std::cout << "\"" << _userID << "\"�� Ŭ���̾�Ʈ �α��� ����" << std::endl;

		SendPkt.isSuccess = true;
		SendPkt.userID = _userID;

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<bool>("isSuccess", SendPkt.isSuccess);
		ptSend.put<int>("userID", SendPkt.userID);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		std::cout << "[����->Ŭ��] " << sendStr << std::endl;

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
	case PACKET_INDEX::REQ_PLAYER_MOVE_START:
	{
		PKT_REQ_PLAYER_MOVE_START* pPacket = (PKT_REQ_PLAYER_MOVE_START*)pData;

		PKT_RES_PLAYER_MOVE_START playerMove;
		playerMove.Init();

		playerMove.userID = pPacket->userID;
		strcpy_s(playerMove.userPos, MAX_PLAYER_MOVE_LEN, pPacket->userPos);
		strcpy_s(playerMove.userDir, MAX_PLAYER_MOVE_LEN, pPacket->userDir);
		
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", playerMove.packetIndex);
		ptSendHeader.put<short>("packetSize", playerMove.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("userID", playerMove.userID);
		ptSend.put<std::string>("userPos", playerMove.userPos);
		ptSend.put<std::string>("userDir", playerMove.userDir);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		std::cout << "[����->Ŭ��]" << sendStr << std::endl;

		size_t totalSessionCount = _sessionList.size();

		for (size_t i = 0; i < totalSessionCount; i++)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				if (_sessionList[i]->SessionID() == (playerMove.userID - FIRST_USER_INDEX))
					continue;

				_sessionList[i]->PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
			}
		}
	}
	break;
	case PACKET_INDEX::REQ_PLAYER_MOVE_END:
	{
		PKT_REQ_PLAYER_MOVE_END* pPacket = (PKT_REQ_PLAYER_MOVE_END*)pData;

		PKT_RES_PLAYER_MOVE_END playerMove;
		playerMove.Init();

		playerMove.userID = pPacket->userID;
		strcpy_s(playerMove.userPos, MAX_PLAYER_MOVE_LEN, pPacket->userPos);
		strcpy_s(playerMove.userDir, MAX_PLAYER_MOVE_LEN, pPacket->userDir);

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", playerMove.packetIndex);
		ptSendHeader.put<short>("packetSize", playerMove.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("userID", playerMove.userID);
		ptSend.put<std::string>("userPos", playerMove.userPos);
		ptSend.put<std::string>("userDir", playerMove.userDir);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		std::cout << "[����->Ŭ��]" << sendStr << std::endl;

		size_t totalSessionCount = _sessionList.size();

		for (size_t i = 0; i < totalSessionCount; i++)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				if (_sessionList[i]->SessionID() == (playerMove.userID - FIRST_USER_INDEX))
					continue;

				_sessionList[i]->PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
			}
		}
	}
	break;
	}

	return;
}

void AsioServer::ConcurrentUser()
{
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
		// ������ , ���ֱ�
		int endPlayerList = userList.size() - 1;

		userList.replace(endPlayerList, endPlayerList, "");
	}

	PKT_RES_CONCURRENT_USER_LIST concurrentUser;
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

	std::cout << "���� ���� : " << concurrentUser.totalUser << std::endl;
	std::cout << "���� ����Ʈ : " << concurrentUser.concurrentUserList << std::endl;

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

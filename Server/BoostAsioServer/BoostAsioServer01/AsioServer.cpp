#include "AsioServer.h"

AsioServer::AsioServer(boost::asio::io_context& io_context)
	: _acceptor(io_context, boost::asio::ip::tcp::endpoint(boost::asio::ip::tcp::v4(), PORT_NUMBER))
{
	_isAccepting = false;
	_sessionID = 0;
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
	//setlocale(LC_ALL, "");
	//std::locale::global(std::locale("ko_KR.UTF-8"));

	std::cout << "port : " << PORT_NUMBER << std::endl;
	_DBMysql.Init();
	//_DBMysql.DBDataLoginSelectAll();
	_DBMysql.DBMySQLVersion();

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
	_sessionID = pSession->SessionID() + FIRST_SESSION_INDEX;

	if (!error)
	{
		//std::cout << "Ŭ���̾�Ʈ ���� ����. SessionID: " << pSession->SessionID() << std::endl;
		std::cout << "\"" << _sessionID << "\"�� Ŭ���̾�Ʈ ���� ���� ����" << std::endl;

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
	_sessionID = sessionID + FIRST_SESSION_INDEX;
	std::cout << "\"" << _sessionID << "\"�� Ŭ���̾�Ʈ ���� ����" << std::endl;

	_sessionList[sessionID]->Socket().close();

	_sessionQueue.push_back(sessionID);

	if (_isAccepting == false)
	{
		PostAccept();
	}

	_totalUserPos.erase(_sessionID);
	_totalUserDir.erase(_sessionID);

	UserExit(_sessionID);
	//ConcurrentUser();	//������ ���� ���� �ϸ� Ŭ���̾�Ʈ���� ���ŵ� ������ ����
}

void AsioServer::ProcessPacket(const int sessionID, const char* pData)
{
	_sessionID = sessionID + FIRST_SESSION_INDEX;

	PACKET_HEADER* pHeader = (PACKET_HEADER*)pData;

	switch (pHeader->packetIndex)
	{
	case PACKET_INDEX::REQ_SIGN_UP:
	{
		PKT_REQ_SIGN_UP* pPacket = (PKT_REQ_SIGN_UP*)pData;

		PKT_RES_SIGN_UP SendPkt;
		SendPkt.Init();
		
		SendPkt.checkResult = _DBMysql.DBSignUpCreate(pPacket->userID, pPacket->userPW, pPacket->userName);
		
		if(SendPkt.checkResult == RESULT_SIGN_UP_CHECK::RESULT_SIGN_UP_CHECK_SUCCESS)
			_DBMysql.DBInventoryCreate(pPacket->userID);

		// json
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("checkResult", SendPkt.checkResult);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();

		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<int>("checkResult", SendPkt.checkResult);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		_sessionList[sessionID]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
	}
	break;
	case PACKET_INDEX::REQ_CHECK_BEFORE_LOGIN:
	{
		PKT_REQ_CHECK_BEFORE_LOGIN* pPacket = (PKT_REQ_CHECK_BEFORE_LOGIN*)pData;
		
		PKT_RES_CHECK_BEFORE_LOGIN SendPkt;
		SendPkt.Init();

		SendPkt.sessionID = _sessionID;

		// DB üũ
		SendPkt.checkResult = _DBMysql.DBLoginCheckUserID(pPacket->userID);

		std::cout << "ID checkResult = " << SendPkt.checkResult << std::endl;

		if (SendPkt.checkResult == RESULT_BEFORE_LOGIN_CHECK::RESULT_BEFORE_LOGIN_CHECK_SUCCESS)
		{
			SendPkt.checkResult = _DBMysql.DBLoginCheckUserPW(pPacket->userID, pPacket->userPW);
			std::cout << "PW checkResult = " << SendPkt.checkResult << std::endl;
			SendPkt.userName = _DBMysql.DBLoginGetUserName(pPacket->userID);

			_sessionList[sessionID]->SetName(SendPkt.userName.c_str());

			strcpy_s(SendPkt.userID, pPacket->userID);
		}

		// json
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("checkResult", SendPkt.checkResult);
		ptSend.put<std::string>("userID", SendPkt.userID);
		ptSend.put<int>("sessionID", SendPkt.sessionID);
		ptSend.put<std::string>("userName", _sessionList[sessionID]->GetName());

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		
		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<int>("checkResult", SendPkt.checkResult);
		ptSend2.put<std::string>("userID", SendPkt.userID);
		ptSend2.put<int>("sessionID", SendPkt.sessionID);
		ptSend2.put<std::string>("userName", _sessionList[sessionID]->GetName());

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		_sessionList[sessionID]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
	}
	break;
	case PACKET_INDEX::REQ_NEW_LOGIN:
	{
		_sessionList[sessionID]->SetZone(WORLD_ZONE::WORLD_ZONE_LOBBY);

		PKT_REQ_NEW_LOGIN* pPacket = (PKT_REQ_NEW_LOGIN*)pData;

		PKT_RES_NEW_LOGIN_SUCSESS SendPkt;
		SendPkt.Init();
		
		std::cout << "\"" << _sessionID << "\"�� Ŭ���̾�Ʈ �α��� ����" << std::endl;
		
		SendPkt.isSuccess = true;
		SendPkt.sessionID = _sessionID;
		
		_totalUserPos.erase(_sessionID);
		_totalUserDir.erase(_sessionID);
		_totalUserPos.insert(std::make_pair(_sessionID, "(0.0000, 0.0000, 0.0000)"));
		_totalUserDir.insert(std::make_pair(_sessionID, "(0.0, 0.0, 0.0)"));

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<bool>("isSuccess", SendPkt.isSuccess);
		ptSend.put<int>("sessionID", SendPkt.sessionID);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		//std::cout << "[����->Ŭ��] " << sendStr << std::endl;
		
		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<bool>("isSuccess", SendPkt.isSuccess);
		ptSend2.put<int>("sessionID", SendPkt.sessionID);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		_sessionList[sessionID]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
	}
	break;
	case PACKET_INDEX::REQ_CONCURRENT_USER:
	{
		ConcurrentUser();
	}
	break;
	case PACKET_INDEX::REQ_USER_EXIT:
	{
		PKT_REQ_USER_EXIT* pPacket = (PKT_REQ_USER_EXIT*)pData;

		int exitUser = pPacket->sessionID;

		_sessionList[sessionID]->SetZone(WORLD_ZONE::WORLD_ZONE_DUNGEON);

		UserExit(exitUser);
	}
	break;
	case PACKET_INDEX::REQ_CHATTING:
	{
		PKT_REQ_CHATTING* pPacket = (PKT_REQ_CHATTING*)pData;

		PKT_RES_CHATTING SendPkt;
		SendPkt.Init();
		SendPkt.sessionID = pPacket->sessionID;
		SendPkt.userName = _sessionList[sessionID]->GetName();
		SendPkt.chatMessage = pPacket->chatMessage;

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("sessionID", SendPkt.sessionID);
		ptSend.put<std::string>("userName", SendPkt.userName);
		ptSend.put<std::string>("chatMessage", SendPkt.chatMessage);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		//std::cout << "[����->Ŭ��]" << sendStr << std::endl;

		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<int>("sessionID", SendPkt.sessionID);
		ptSend2.put<std::string>("userName", SendPkt.userName);
		ptSend2.put<std::string>("chatMessage", SendPkt.chatMessage);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		size_t totalSessionCount = _sessionList.size();

		for (size_t i = 0; i < totalSessionCount; i++)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				if (_sessionList[i]->SessionID() == (SendPkt.sessionID - FIRST_SESSION_INDEX))
					continue;

				_sessionList[i]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
			}
		}
	}
	break;
	case PACKET_INDEX::REQ_PLAYER_MOVE_START:
	{
		PKT_REQ_PLAYER_MOVE_START* pPacket = (PKT_REQ_PLAYER_MOVE_START*)pData;
		
		PKT_RES_PLAYER_MOVE_START playerMove;
		playerMove.Init();

		playerMove.sessionID = pPacket->sessionID;
		strcpy_s(playerMove.userPos, MAX_PLAYER_MOVE_LEN, pPacket->userPos);
		strcpy_s(playerMove.userDir, MAX_PLAYER_MOVE_LEN, pPacket->userDir);

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", playerMove.packetIndex);
		ptSendHeader.put<short>("packetSize", playerMove.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("sessionID", playerMove.sessionID);
		ptSend.put<std::string>("userPos", playerMove.userPos);
		ptSend.put<std::string>("userDir", playerMove.userDir);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		//std::cout << "[����->Ŭ��]" << sendStr << std::endl;

		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", playerMove.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<int>("sessionID", playerMove.sessionID);
		ptSend2.put<std::string>("userPos", playerMove.userPos);
		ptSend2.put<std::string>("userDir", playerMove.userDir);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
//		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		size_t totalSessionCount = _sessionList.size();

		for (size_t i = 0; i < totalSessionCount; i++)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				if (_sessionList[i]->SessionID() == (playerMove.sessionID - FIRST_SESSION_INDEX))
					continue;

				if (_sessionList[i]->GetZone() == WORLD_ZONE::WORLD_ZONE_LOBBY)
					_sessionList[i]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
			}
		}
	}
	break;
	case PACKET_INDEX::REQ_PLAYER_MOVE_END:
	{
		PKT_REQ_PLAYER_MOVE_END* pPacket = (PKT_REQ_PLAYER_MOVE_END*)pData;

		_totalUserPos.erase(pPacket->sessionID);
		_totalUserDir.erase(pPacket->sessionID);
		_totalUserPos.insert(std::make_pair(pPacket->sessionID, pPacket->userPos));
		_totalUserDir.insert(std::make_pair(pPacket->sessionID, pPacket->userDir));
		

		//for (auto it = _totalUserPos.begin(); it != _totalUserPos.end(); it++)
		//{
		//	std::cout << "[pos] " << it->first << " " << it->second << std::endl;
		//}
		//for (auto it = _totalUserDir.begin(); it != _totalUserDir.end(); it++)
		//{
		//	std::cout << "[dir] " << it->first << " " << it->second << std::endl;
		//}

		PKT_RES_PLAYER_MOVE_END playerMove;
		playerMove.Init();

		playerMove.sessionID = pPacket->sessionID;
		strcpy_s(playerMove.userPos, MAX_PLAYER_MOVE_LEN, pPacket->userPos);
		strcpy_s(playerMove.userDir, MAX_PLAYER_MOVE_LEN, pPacket->userDir);

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", playerMove.packetIndex);
		ptSendHeader.put<short>("packetSize", playerMove.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("sessionID", playerMove.sessionID);
		ptSend.put<std::string>("userPos", playerMove.userPos);
		ptSend.put<std::string>("userDir", playerMove.userDir);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		//std::cout << "[����->Ŭ��]" << sendStr << std::endl;

		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", playerMove.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<int>("sessionID", playerMove.sessionID);
		ptSend2.put<std::string>("userPos", playerMove.userPos);
		ptSend2.put<std::string>("userDir", playerMove.userDir);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
//		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		size_t totalSessionCount = _sessionList.size();

		for (size_t i = 0; i < totalSessionCount; i++)
		{
			if (_sessionList[i]->Socket().is_open())
			{
				if (_sessionList[i]->SessionID() == (playerMove.sessionID - FIRST_SESSION_INDEX))
					continue;

				if (_sessionList[i]->GetZone() == WORLD_ZONE::WORLD_ZONE_LOBBY)
					_sessionList[i]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
			}
		}
	}
	break;
	case PACKET_INDEX::REQ_DUNGEON_CLEAR_RESULT_ITEM:
	{
		PKT_REQ_DUNGEON_CLEAR_RESULT_ITEM* pPacket = (PKT_REQ_DUNGEON_CLEAR_RESULT_ITEM*)pData;

		PKT_RES_DUNGEON_CLEAR_RESULT_ITEM SendPkt;
		SendPkt.Init();

		int resultItemSize = _DBMysql.DBDungeonClearResultItemSize();

		boost::random::mt19937 engine((unsigned int)time(NULL));
		boost::random::uniform_int_distribution<> dist(DB_INDEX_RESULT_ITEMS_START, (DB_INDEX_RESULT_ITEMS + resultItemSize));
		
		int resultRandom = dist(engine);
		
		// DB üũ
		SendPkt.itemID = _DBMysql.DBDungeonClearResultItem(resultRandom);
		
		_DBMysql.DBDungeonClearResultItemInventoryAdd(pPacket->userID, resultRandom);

		// json
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<std::string>("itemID", SendPkt.itemID);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();

		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<std::string>("itemID", SendPkt.itemID);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		_sessionList[sessionID]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
	}
	break;
	case PACKET_INDEX::REQ_DUNGEON_HELL_RESULT_ITEM:
	{
		PKT_REQ_DUNGEON_HELL_RESULT_ITEM* pPacket = (PKT_REQ_DUNGEON_HELL_RESULT_ITEM*)pData;

		PKT_RES_DUNGEON_HELL_RESULT_ITEM SendPkt;
		SendPkt.Init();

		int resultItemSize = _DBMysql.DBDungeonHellResultItemSize();

		boost::random::mt19937 engine((unsigned int)time(NULL));
		boost::random::uniform_int_distribution<> dist(DB_INDEX_HELL_ITEMS_START, (DB_INDEX_HELL_ITEMS + resultItemSize));

		int resultRandom = dist(engine);

		_sessionList[sessionID]->SetHellRandom(0);
		_sessionList[sessionID]->SetHellRandom(resultRandom);

		// DB üũ
		SendPkt.itemID = _DBMysql.DBDungeonHellResultItem(resultRandom);
		_DBMysql.DBLogHellResultItem(pPacket->userID, resultRandom);

		// json
		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader.put<short>("packetSize", SendPkt.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<std::string>("itemID", SendPkt.itemID);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();

		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", SendPkt.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<std::string>("itemID", SendPkt.itemID);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		_sessionList[sessionID]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
	}
	break;
	case PACKET_INDEX::REQ_DUNGEON_HELL_ITEM_PICK_UP:
	{
		PKT_REQ_DUNGEON_HELL_ITEM_PICK_UP* pPacket = (PKT_REQ_DUNGEON_HELL_ITEM_PICK_UP*)pData;

		PKT_REQ_DUNGEON_HELL_ITEM_PICK_UP hellPickUp;
		hellPickUp.Init();
		
		_DBMysql.DBDungeonHellResultItemInventoryAdd(pPacket->userID, _sessionList[sessionID]->GetHellRandom());
		_DBMysql.DBLogHellResultItem(pPacket->userID, _sessionList[sessionID]->GetHellRandom());

	}

	case PACKET_INDEX::REQ_INVENTORY_OPEN:
	{
		PKT_REQ_INVENTORY_OPEN* pPacket = (PKT_REQ_INVENTORY_OPEN*)pData;

		PKT_RES_INVENTORY_OPEN userInven;
		userInven.Init();

		std::array<std::string, MAX_INVENTORY_COLUMN> userDBInventory = _DBMysql.DBInventorySelect(pPacket->userID);

		int columnCorrection = MAX_INVENTORY_COLUMN - MAX_USER_EQUIP - 1;
		for (int i = 0; i < MAX_USER_EQUIP; i++)
		{
			strcpy_s(userInven.equip[i], MAX_USER_ITEM_LEN, userDBInventory[(i + columnCorrection)].c_str());
		}

		std::string inven = userDBInventory[MAX_INVENTORY_COLUMN - 1];
		for (int i = 0; i < MAX_USER_INVENTORY; i++)
		{
			if (!strcmp(inven.c_str(), ""))
				break;

			if (inven.find(",") != -1)
			{
				std::string invenData;
				invenData = inven.substr(0, MAX_USER_ITEM_LEN - 1);

				strcpy_s(userInven.inventory[i], MAX_USER_ITEM_LEN, invenData.c_str());

				inven.erase(0, MAX_USER_ITEM_LEN);
			}
			else
			{
				if (strcmp(inven.c_str(), "") != 0)
				{
					strcpy_s(userInven.inventory[i], MAX_USER_ITEM_LEN, inven.c_str());
					inven.erase(0, MAX_USER_ITEM_LEN);
				}
				else
					break;
			}
		}

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", userInven.packetIndex);
		ptSendHeader.put<short>("packetSize", userInven.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);

		boost::property_tree::ptree ptChild1[MAX_USER_EQUIP];
		boost::property_tree::ptree ptEquipChildren;
		boost::property_tree::ptree ptChild2[MAX_USER_INVENTORY];
		boost::property_tree::ptree ptInventoryChildren;

		int equipNum = 0;
		for (int i = 0; i < MAX_USER_EQUIP; i++)
		{
			ptChild1[equipNum].put("", userInven.equip[i]);
			ptEquipChildren.push_back(std::make_pair("", ptChild1[equipNum]));
			equipNum++;
		}
		ptSend.add_child("equip", ptEquipChildren);

		int inventoryNum = 0;
		for (int i = 0; i < MAX_USER_INVENTORY; i++)
		{
			if (!strcmp(userInven.inventory[i], ""))
				break;

			ptChild2[inventoryNum].put("", userInven.inventory[i]);
			ptInventoryChildren.push_back(std::make_pair("", ptChild2[inventoryNum]));
			inventoryNum++;
		}
		ptSend.add_child("inventory", ptInventoryChildren);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();
		std::cout << "[����->Ŭ��] " << sendStr << std::endl;

		_sessionList[sessionID]->PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
	}
	break;
	case PACKET_INDEX::REQ_INVENTORY_CLOSE:
	{
		PKT_REQ_INVENTORY_CLOSE* pPacket = (PKT_REQ_INVENTORY_CLOSE*)pData;

		PKT_RES_INVENTORY_CLOSE userInven;
		userInven.Init();

		userInven.checkResult = _DBMysql.DBInventoryUpdate(pPacket->userID, pPacket->equip, pPacket->inventory);

		boost::property_tree::ptree ptSendHeader;
		ptSendHeader.put<short>("packetIndex", userInven.packetIndex);
		ptSendHeader.put<short>("packetSize", userInven.packetSize);

		boost::property_tree::ptree ptSend;
		ptSend.add_child("header", ptSendHeader);
		ptSend.put<int>("checkResult", userInven.checkResult);

		std::string stringRecv;
		std::ostringstream oss(stringRecv);
		boost::property_tree::write_json(oss, ptSend, false);
		std::string sendStr = oss.str();

		short JsonDataAllPacketSize = JsonDataSize(sendStr);

		boost::property_tree::ptree ptSendHeader2;
		ptSendHeader2.put<short>("packetIndex", userInven.packetIndex);
		ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

		boost::property_tree::ptree ptSend2;
		ptSend2.add_child("header", ptSendHeader2);
		ptSend2.put<int>("checkResult", userInven.checkResult);

		std::string stringRecv2;
		std::ostringstream oss2(stringRecv2);
		boost::property_tree::write_json(oss2, ptSend2, false);
		std::string sendStr2 = oss2.str();
		std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

		_sessionList[sessionID]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
	}
	break;

	}

	return;
}

// TODO : ������ �۾�
short AsioServer::JsonDataSize(std::string jsonData)
{
	short JsonAllSize = (short)jsonData.size();
//	std::cout << "Json ������ : " << JsonAllSize << std::endl;
	return JsonAllSize;
}

void AsioServer::ConcurrentUser()
{
	int totalUser = 0;
	std::string userList = "";
	std::string userName = "";
	std::string userPos = "";
	std::string userDir = "";

	for (size_t i = 0; i < _sessionList.size(); i++)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			if (_sessionList[i]->GetZone() == WORLD_ZONE::WORLD_ZONE_LOBBY)
			{
				totalUser++;

				int userNum = i + FIRST_SESSION_INDEX;

				userList += std::to_string(userNum);
				userList += ",";

				userName += _sessionList[i]->GetName();
				userName += ",";

				if (_totalUserPos.empty() == false)
				{
					userPos += _totalUserPos.find(userNum)->second;
					userPos += "|";

					userDir += _totalUserDir.find(userNum)->second;
					userDir += "|";
				}
			}
		}
	}

	if (totalUser > 0)
	{
		// ������ , ���ֱ�
		int endUserList = userList.size() - 1;
		userList.replace(endUserList, endUserList, "");

		int endUserName = userName.size() - 1;
		userName.replace(endUserName, endUserName, "");

		if (_totalUserPos.empty() == false)
		{
			int endUserPos = userPos.size() - 1;
			userPos.replace(endUserPos, endUserPos, "");

			int endUserDir = userDir.size() - 1;
			userDir.replace(endUserDir, endUserDir, "");
		}
	}

	PKT_RES_CONCURRENT_USER_LIST concurrentUser;
	concurrentUser.Init();

	concurrentUser.totalUser = totalUser;
	concurrentUser.concurrentUserList = userList;
	concurrentUser.userName = userName;
	concurrentUser.userPos = userPos;
	concurrentUser.userDir = userDir;

	boost::property_tree::ptree ptSendHeader;
	ptSendHeader.put<short>("packetIndex", concurrentUser.packetIndex);
	ptSendHeader.put<short>("packetSize", concurrentUser.packetSize);

	boost::property_tree::ptree ptSend;
	ptSend.add_child("header", ptSendHeader);
	ptSend.put<int>("totalUser", concurrentUser.totalUser);
	ptSend.put<std::string>("concurrentUser", concurrentUser.concurrentUserList);
	ptSend.put<std::string>("userName", concurrentUser.userName);
	ptSend.put<std::string>("userPos", concurrentUser.userPos);
	ptSend.put<std::string>("userDir", concurrentUser.userDir);

	std::cout << "���� ���� : " << concurrentUser.totalUser << std::endl;
	std::cout << "���� ����Ʈ : " << concurrentUser.concurrentUserList << std::endl;
	std::cout << "���� �̸� : " << concurrentUser.userName << std::endl;
	//std::cout << "���� Pos : " << concurrentUser.userPos << std::endl;
	//std::cout << "���� Dir : " << concurrentUser.userDir << std::endl;

	std::string stringRecv;
	std::ostringstream oss(stringRecv);
	boost::property_tree::write_json(oss, ptSend, false);
	std::string sendStr = oss.str();
	//std::cout << [����->Ŭ��] " << sendStr << std::endl;

	short JsonDataAllPacketSize = JsonDataSize(sendStr);

	boost::property_tree::ptree ptSendHeader2;
	ptSendHeader2.put<short>("packetIndex", concurrentUser.packetIndex);
	ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

	boost::property_tree::ptree ptSend2;
	ptSend2.add_child("header", ptSendHeader2);
	ptSend2.put<int>("totalUser", concurrentUser.totalUser);
	ptSend2.put<std::string>("concurrentUser", concurrentUser.concurrentUserList);
	ptSend2.put<std::string>("userName", concurrentUser.userName);
	ptSend2.put<std::string>("userPos", concurrentUser.userPos);
	ptSend2.put<std::string>("userDir", concurrentUser.userDir);

	std::string stringRecv2;
	std::ostringstream oss2(stringRecv2);
	boost::property_tree::write_json(oss2, ptSend2, false);
	std::string sendStr2 = oss2.str();
	std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

	size_t totalSessionCount = _sessionList.size();

	for (size_t i = 0; i < totalSessionCount; i++)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			//if(exitUser != 0)
			//{
			//	if (_sessionList[i]->SessionID() == (exitUser - FIRST_SESSION_INDEX))
			//		continue;
			//}

			if(_sessionList[i]->GetZone() == WORLD_ZONE::WORLD_ZONE_LOBBY)
				_sessionList[i]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
		}
	}
}

void AsioServer::UserExit(int sessionID)
{
	PKT_RES_USER_EXIT userExit;
	userExit.Init();

	userExit.sessionID = sessionID;

	boost::property_tree::ptree ptSendHeader;
	ptSendHeader.put<short>("packetIndex", userExit.packetIndex);
	ptSendHeader.put<short>("packetSize", userExit.packetSize);

	boost::property_tree::ptree ptSend;
	ptSend.add_child("header", ptSendHeader);
	ptSend.put<int>("sessionID", userExit.sessionID);

	std::string stringRecv;
	std::ostringstream oss(stringRecv);
	boost::property_tree::write_json(oss, ptSend, false);
	std::string sendStr = oss.str();
	//std::cout << [����->Ŭ��] " << sendStr << std::endl;

	short JsonDataAllPacketSize = JsonDataSize(sendStr);

	boost::property_tree::ptree ptSendHeader2;
	ptSendHeader2.put<short>("packetIndex", userExit.packetIndex);
	ptSendHeader2.put<short>("packetSize", JsonDataAllPacketSize);

	boost::property_tree::ptree ptSend2;
	ptSend2.add_child("header", ptSendHeader2);
	ptSend2.put<int>("sessionID", userExit.sessionID);

	std::string stringRecv2;
	std::ostringstream oss2(stringRecv2);
	boost::property_tree::write_json(oss2, ptSend2, false);
	std::string sendStr2 = oss2.str();
	std::cout << "[����->Ŭ��] " << sendStr2 << std::endl;

	size_t totalSessionCount = _sessionList.size();

	for (size_t i = 0; i < totalSessionCount; i++)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			if (_sessionList[i]->SessionID() == (userExit.sessionID - FIRST_SESSION_INDEX))
				continue;

			if (_sessionList[i]->GetZone() == WORLD_ZONE::WORLD_ZONE_LOBBY)
				_sessionList[i]->PostSend(false, std::strlen(sendStr2.c_str()), (char*)sendStr2.c_str());
		}
	}
}

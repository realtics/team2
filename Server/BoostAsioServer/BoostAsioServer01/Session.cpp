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
			std::cout << "클라이언트와 연결이 끊어졌습니다. ";
		}
		else
		{
			std::cout << "error No: " << error.value() << " error Message: " << error.message() << std::endl;
		}

		_pServer->CloseSession(_sessionID);

		ConcurrentUsers();
	}
	else
	{
		Deserialization(_receiveBuffer.data());
		//memcpy(&_packetBuffer[_packetBufferMark], _receiveBuffer.data(), bytes_transferred);
		
		int nPacketData = _packetBufferMark + bytes_transferred;
		int nReadData = 0;

		// TODO : ConcurrentUsers 위치 수정 할 것
		ConcurrentUsers();

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

void Session::ConcurrentUsers()
{
	PACKET_CONCURRENT_USERS concurrentUsers;
	concurrentUsers.packetIndex = PACKET_INDEX::CONCURRENT_USERS;
	concurrentUsers.packetSize = sizeof(PACKET_CONCURRENT_USERS);

	std::vector< Session* > _sessionList = _pServer->GetSessionList();
	int totalUsers = 0;
	std::string userList = "";

	for (size_t i = 0; i < _sessionList.size(); ++i)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			totalUsers++;
			
			int PlayerNum = i + FIRST_USER_INDEX;

			userList += std::to_string(PlayerNum);
			userList += ",";
		}
	}
	
	if (totalUsers > 0)
	{
		// 마지막 , 없애기
		int endPlayerList = userList.size() - 1;
		
		userList.replace(endPlayerList, endPlayerList, "");
	}
	concurrentUsers.Init();

	concurrentUsers.totalUsers = totalUsers;
	concurrentUsers.concurrentUsersList = userList;
	
	boost::property_tree::ptree ptSendHeader;
	ptSendHeader.put<short>("packetIndex", concurrentUsers.packetIndex);
	ptSendHeader.put<short>("packetSize", concurrentUsers.packetSize);
	
	boost::property_tree::ptree ptSend;
	ptSend.add_child("header", ptSendHeader);
	ptSend.put<int>("totalUsers", concurrentUsers.totalUsers);
	ptSend.put<std::string>("concurrentUsers", concurrentUsers.concurrentUsersList);

	std::cout << "접속 유저 : " << concurrentUsers.totalUsers << std::endl;
	std::cout << "유저 리스트 : " << concurrentUsers.concurrentUsersList << std::endl;

	std::string stringRecv;
	std::ostringstream oss(stringRecv);
	boost::property_tree::write_json(oss, ptSend, false);
	std::string sendStr = oss.str();
	std::cout << sendStr << std::endl;
	//PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());

	for (size_t i = 0; i < _sessionList.size(); ++i)
	{
		if (_sessionList[i]->Socket().is_open())
		{
			PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
		}
	}


	//ptSend.put<std::string>("String", packetCharacterMove.string);

	//std::string stringRecv;
	//std::ostringstream os(stringRecv);
	//boost::property_tree::write_json(os, ptSend, false);
	//std::string sendStr = os.str();
	//std::cout << sendStr << std::endl;
	//PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());

	//PACKET_CHARACTER_MOVE  packetCharacterMove;
	//	packetCharacterMove.header.packetIndex = PACKET_INDEX::RES_IN;
	//	packetCharacterMove.header.packetSize = sizeof(PACKET_CHARACTER_MOVE);
	//	packetCharacterMove.characterMoveX = 2;
	//	packetCharacterMove.characterMoveY = 1;
	//	//packetCharacterMove.characterMoveY = "string";

	//	//{"header":{"packetIndex":1,"packetSize":??},"characterMoveX":1,"characterMoveY":2}
	//	//{"header":{"packetIndex":1,"packetSize":??},"characterMoveX":1,"String":"string"}

	//	boost::property_tree::ptree ptSend;
	//	boost::property_tree::ptree ptSendHeader;
	//	ptSendHeader.put<int>("packetIndex", packetCharacterMove.header.packetIndex);
	//	ptSendHeader.put<int>("packetSize", packetCharacterMove.header.packetSize);
	//	ptSend.add_child("header", ptSendHeader);
	//	ptSend.put<float>("characterMoveX", packetCharacterMove.characterMoveX);
	//	ptSend.put<float>("characterMoveY", packetCharacterMove.characterMoveY);
	//	//ptSend.put<std::string>("String", packetCharacterMove.string);

	//	std::string stringRecv;
	//	std::ostringstream os(stringRecv);
	//	boost::property_tree::write_json(os, ptSend, false);
	//	std::string sendStr = os.str();
	//	std::cout << sendStr << std::endl;
	//	PostSend(false, std::strlen(sendStr.c_str()), (char*)sendStr.c_str());
}
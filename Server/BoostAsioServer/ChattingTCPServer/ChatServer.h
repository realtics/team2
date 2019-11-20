#pragma once 

#include <iostream>
#include <vector>
#include <deque>
#include <algorithm>
#include <string>
#include <vector>

#include "ServerSession.h"
#include "Protocol.h"

class ChatServer
{
private:
	int _nSeqNumber;
	bool _bIsAccepting;

	boost::asio::ip::tcp::acceptor _acceptor;

	std::vector< Session* > _SessionList;
	std::deque< int > _SessionQueue;
private:
	bool PostAccept();
	void HandleAccept(Session* pSession, const boost::system::error_code& error);

public:
	ChatServer(boost::asio::io_context& io_service);
	~ChatServer();

	void Init(const int nMaxSessionCount);
	void Start();
	void CloseSession(const int nSessionID);
	void ProcessPacket(const int nSessionID, const char* pData);
};




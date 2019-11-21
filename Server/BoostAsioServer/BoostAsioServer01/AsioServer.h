#pragma once 

#include <iostream>
#include <vector>
#include <deque>
#include <algorithm>
#include <string>
#include <vector>
#include <sstream>

#include "Session.h"
#include "Protocol.h"

class AsioServer
{
private:
	bool _isAccepting;

	boost::asio::ip::tcp::acceptor _acceptor;

	std::vector< Session* > _sessionList;
	std::deque< int > _sessionQueue;

	bool PostAccept();
	void HandleAccept(Session* pSession, const boost::system::error_code& error);

public:
	AsioServer(boost::asio::io_context& io_context);
	~AsioServer();

	void Init(const int nMaxSessionCount);
	void Start();
	void CloseSession(const int nSessionID);
	void ProcessPacket(const int nSessionID, const char* pData);
};

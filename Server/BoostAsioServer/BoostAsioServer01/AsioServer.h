#pragma once 

#include <iostream>
#include <vector>
#include <deque>
#include <algorithm>
#include <string>
#include <vector>
#include <sstream>

#include "DBMySQL.h"
#include "Session.h"
#include "Protocol.h"

class AsioServer
{
private:
	DBMySQL _DBMysql;

	bool _isAccepting;

	boost::asio::ip::tcp::acceptor _acceptor;

	std::vector< Session* > _sessionList;
	std::deque< int > _sessionQueue;

	std::map<int, std::string> _totalUserPos;
	std::map<int, std::string> _totalUserDir;
	
	bool PostAccept();
	void HandleAccept(Session* pSession, const boost::system::error_code& error);

	int _sessionID;
public:
	AsioServer(boost::asio::io_context& io_context);
	~AsioServer();

	void Init(const int maxSessionCount);
	void Start();
	void CloseSession(const int sessionID);
	void ProcessPacket(const int sessionID, const char* pData);
	
	short JsonDataSize(std::string jsonData);

	void ConcurrentUser();
	void UserExit(int sessionID);

	std::vector< Session* > GetSessionList() { return _sessionList; }
};

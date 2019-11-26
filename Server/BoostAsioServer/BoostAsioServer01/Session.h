#pragma once
#include <deque>
#include <boost/bind.hpp>
#include <boost/asio.hpp>
#include "Protocol.h"

//json
#include <boost/property_tree/ptree.hpp> 
#include <boost/property_tree/json_parser.hpp>

class AsioServer;
class Session
{
private:
	int _sessionID;
	boost::asio::ip::tcp::socket _socket;

	std::array<char, MAX_RECEIVE_BUFFER_LEN> _receiveBuffer = { 0 };

	int _packetBufferMark;
	char _packetBuffer[MAX_RECEIVE_BUFFER_LEN * 2];
	
	std::deque< char* > _sendDataQueue;

	std::string _name;

	AsioServer* _pServer;

	void HandleWrite(const boost::system::error_code& error, size_t bytes_transferred);
	void HandleReceive(const boost::system::error_code& error, size_t bytes_transferred);

	void Deserialization(char* jsonData);
	//std::string Serialization(PACKET_INDEX packetIndex);

	void ConcurrentUsers();
public:
	Session(int nSessionID, boost::asio::io_context& io_context, AsioServer* pServer);
	~Session();

	void Init();
	void PostReceive();
	void PostSend(const bool bImmediately, const int packetSize, char* pData);

	int SessionID() { return _sessionID; }
	boost::asio::ip::tcp::socket& Socket() { return _socket; }
	void SetName(const char* pszName) { _name = pszName; }
	const char* GetName() { return _name.c_str(); }
};
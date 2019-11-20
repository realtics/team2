#pragma once
#include <deque>
#include <boost/bind.hpp>
#include <boost/asio.hpp>
#include "Protocol.h"

#include <boost/property_tree/ptree.hpp> 
#include <boost/property_tree/json_parser.hpp>

class ChatServer;
class Session
{
private:
	void HandleWrite(const boost::system::error_code& error, size_t bytes_transferred);
	void HandleReceive(const boost::system::error_code& error, size_t bytes_transferred);

	int _nSessionID;
	boost::asio::ip::tcp::socket _Socket;

	std::array<char, MAX_RECEIVE_BUFFER_LEN> _ReceiveBuffer = { 0 };

	int _nPacketBufferMark;
	char _PacketBuffer[MAX_RECEIVE_BUFFER_LEN * 2];

	//bool m_bCompletedWrite;

	std::deque< char* > _SendDataQueue;

	std::string _Name;

	ChatServer* _pServer;
public:
	Session(int nSessionID, boost::asio::io_context& io_service, ChatServer* pServer);
	~Session();

	void Init();
	void PostReceive();
	void PostSend(const bool bImmediately, const int packetSize, char* pData);

	int SessionID() { return _nSessionID; }
	boost::asio::ip::tcp::socket& Socket() { return _Socket; }
	void SetName(const char* pszName) { _Name = pszName; }
	const char* GetName() { return _Name.c_str(); }
};
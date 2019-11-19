#include <deque>
#include <iostream>

#include <boost/bind.hpp>
#include <boost/asio.hpp>
#include <boost/thread.hpp>

#include "..\..\BoostAsioServer\ChattingTCPServer\Protocol.h"

#include <boost/property_tree/ptree.hpp> 
#include <boost/property_tree/json_parser.hpp>

class ChatClient
{
private:
	boost::asio::io_context& _IOContext;
	boost::asio::ip::tcp::socket _Socket;

	std::array<char, 512> _ReceiveBuffer = { 0 };

	int _nPacketBufferMark;
	char _PacketBuffer[MAX_RECEIVE_BUFFER_LEN * 2];

	CRITICAL_SECTION _lock;
	std::deque< char* > _SendDataQueue;

	bool _bIsLogin;
private:
	void PostReceive();
	void handle_connect(const boost::system::error_code& error);
	void handle_write(const boost::system::error_code& error, size_t bytes_transferred);
	void handle_receive(const boost::system::error_code& error, size_t bytes_transferred);
	void ProcessPacket(const char* pData);
public:
	ChatClient(boost::asio::io_context& io_context);
	~ChatClient();

	void Connect(boost::asio::ip::tcp::endpoint endpoint);
	void Close();
	void PostSend(const bool bImmediately, const int packetSize, char* pData);

	bool IsConnecting() { return _Socket.is_open(); }
	void LoginOK() { _bIsLogin = true; }
	bool IsLogin() { return _bIsLogin; }
};
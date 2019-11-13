#define _CRT_SECURE_NO_WARNINGS

#include <ctime>
#include <iostream>
#include <string>
#include <boost/bind.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/enable_shared_from_this.hpp>
#include <boost/asio.hpp>

using boost::asio::ip::tcp;
using namespace std;

std::string make_daytime_string()
{
	time_t now = time(0);
	return ctime(&now);
}

class tcp_connection :public boost::enable_shared_from_this<tcp_connection>
{
private:
	tcp::socket socket_;
	std::string message_;
	tcp_connection(boost::asio::io_context& io_context) :socket_(io_context) {}
	//클라이언트와의 커넥션에 대한 나머지 일에 대한 책임은 handle_write() 함수에 있음
	void handle_write(const boost::system::error_code& /*e*/, size_t) {}
public:
	typedef boost::shared_ptr<tcp_connection> pointer;
	static pointer create(boost::asio::io_context& io_context)
	{
		return pointer(new tcp_connection(io_context));
	}
	tcp::socket& socket()
	{
		return socket_;
	}
	void start()
	{
		//보내질 데이터들은 비동기적인 오퍼레이션이 완료 되는 동안 message_ 멤버변수에 저장해둠
		message_ = make_daytime_string();
		//클라이언트에게 데이터 제공을 위해 async_write 함수 호출
		//async_write 함수와, async_write_some 함수는 다른 함수
		//async_write 함수는 버퍼의 내용을 모두 전송 했을때 완료 함수를 호출
		//async_write_some 함수는 버퍼의 내용을 일부라도 보냈다면 완료 함수를 호출(바이트수가 일치하는지 확인 해야함)
		
		//bind()를 사용해서 비동기적인 오퍼레이션이 초기화 될 때, 반드시 핸들러의 매게변수 목록에 적합한 인자를 지정 해줘야 함
		boost::asio::async_write(socket_,
								boost::asio::buffer(message_),
								boost::bind(&tcp_connection::handle_write,
								shared_from_this(),
								boost::asio::placeholders::error,
								boost::asio::placeholders::bytes_transferred));

		std::cout << message_ << std::endl;
	}
};

class tcp_server
{
private:
	boost::asio::io_context& io_context_;
	tcp::acceptor acceptor_;
	
	//소켓을 하나 생성, 새로운 커넥션을 기다리는 비동기적인 accept 수행
	void start_accept() 
	{
		tcp_connection::pointer new_connection = tcp_connection::create(io_context_);
		acceptor_.async_accept(new_connection->socket(),
								boost::bind(&tcp_server::handle_accept,
								this,
								new_connection,
								boost::asio::placeholders::error));
	}

	//비동기적인 accept 수행이 start_accept()함수의 종료로 인해 초기화 될때 호출
	//클라이언트 요청에 응답하고, 다음 accept 수행을 위해 start_accept() 함수 호출
	void handle_accept(tcp_connection::pointer new_connection, const boost::system::error_code& error)
	{
		if (!error)
		{
			new_connection->start();
		}
		start_accept();
	}
public:
	tcp_server(boost::asio::io_context& io_context) :
												io_context_(io_context),
												acceptor_(io_context, tcp::endpoint(tcp::v4(), 13))
	{
		start_accept();
	}
};

int main()
{
	try
	{
		//서버 객체 생성, 예전 버전에는 io_context가 io_service로 되어 있음
		boost::asio::io_context io_context;
		tcp_server server(io_context);
		//객체 실행
		io_context.run();
	}
	catch (std::exception & e)	//예외 처리
	{
		std::cerr << e.what() << std::endl;
	}
	return 0;
}
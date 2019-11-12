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
	//Ŭ���̾�Ʈ���� Ŀ�ؼǿ� ���� ������ �Ͽ� ���� å���� handle_write() �Լ��� ����
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
		//������ �����͵��� �񵿱����� ���۷��̼��� �Ϸ� �Ǵ� ���� message_ ��������� �����ص�
		message_ = make_daytime_string();
		//Ŭ���̾�Ʈ���� ������ ������ ���� async_write �Լ� ȣ��
		//async_write �Լ���, async_write_some �Լ��� �ٸ� �Լ�
		//async_write �Լ��� ������ ������ ��� ���� ������ �Ϸ� �Լ��� ȣ��
		//async_write_some �Լ��� ������ ������ �Ϻζ� ���´ٸ� �Ϸ� �Լ��� ȣ��(����Ʈ���� ��ġ�ϴ��� Ȯ�� �ؾ���)
		
		//bind()�� ����ؼ� �񵿱����� ���۷��̼��� �ʱ�ȭ �� ��, �ݵ�� �ڵ鷯�� �ŰԺ��� ��Ͽ� ������ ���ڸ� ���� ����� ��
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
	
	//������ �ϳ� ����, ���ο� Ŀ�ؼ��� ��ٸ��� �񵿱����� accept ����
	void start_accept() 
	{
		tcp_connection::pointer new_connection = tcp_connection::create(io_context_);
		acceptor_.async_accept(new_connection->socket(),
								boost::bind(&tcp_server::handle_accept,
								this,
								new_connection,
								boost::asio::placeholders::error));
	}

	//�񵿱����� accept ������ start_accept()�Լ��� ����� ���� �ʱ�ȭ �ɶ� ȣ��
	//Ŭ���̾�Ʈ ��û�� �����ϰ�, ���� accept ������ ���� start_accept() �Լ� ȣ��
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
		//���� ��ü ����, ���� �������� io_context�� io_service�� �Ǿ� ����
		boost::asio::io_context io_context;
		tcp_server server(io_context);
		//��ü ����
		io_context.run();
	}
	catch (std::exception & e)	//���� ó��
	{
		std::cerr << e.what() << std::endl;
	}
	return 0;
}
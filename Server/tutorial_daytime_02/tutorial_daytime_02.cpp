#define _CRT_SECURE_NO_WARNINGS

#include <ctime>
#include <iostream>
#include <string>
#include <boost/asio.hpp>

using boost::asio::ip::tcp;
using namespace std;

//1. make_daytime_string()�Լ� ����
// ���� ��ǻ���� ��¥ �� �ð� ������ ��ȯ�մϴ�.
string make_daytime_string()
{
	time_t now = time(0);
	return ctime(&now);
}

int main()
{
	try {
		// �⺻������ Boost Asio ���α׷��� �ϳ��� IO Service ��ü�� �����ϴ�.
		boost::asio::io_context io_context;
		//2. acceptor ��ü
		// TCP ���������� 13�� ��Ʈ�� ������ �޴� ���� ������ �����մϴ�.
		tcp::acceptor acceptor(io_context, tcp::endpoint(tcp::v4(), 13));
		//3. ���� ����
		// ��� Ŭ���̾�Ʈ�� ���� ������ �ݺ� �����մϴ�.
		for (;;)
		{
			std::cout << std::endl;
			// ���� ��ü�� ������ ������ ��ٸ��ϴ�.
			tcp::socket socket(io_context);
			acceptor.accept(socket);
			std::cout << "socket, accept" << std::endl;
			//4. Ŭ���̾�Ʈ�� ������ ����
			// ������ �Ϸ�Ǹ� �ش� Ŭ���̾�Ʈ���� ���� �޽����� �����մϴ�.
			string message = make_daytime_string();
			std::cout << "message : " << message << std::endl;

			// �ش� Ŭ���̾�Ʈ���� �޽����� ��� �����մϴ�.
			boost::system::error_code ignored_error;
			std::cout << "error_code : " << ignored_error << std::endl;
			boost::asio::write(socket, boost::asio::buffer(message), ignored_error);
			std::cout << "����" << std::endl;
		}
	}
	//5. ���� ó��
	catch (exception & e)
	{
		cerr << e.what() << endl;
	}
	return 0;
}
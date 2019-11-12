#define _CRT_SECURE_NO_WARNINGS

#include <ctime>
#include <iostream>
#include <string>
#include <boost/asio.hpp>

using boost::asio::ip::tcp;
using namespace std;

//1. make_daytime_string()함수 선언
// 현재 컴퓨터의 날짜 및 시간 정보를 반환합니다.
string make_daytime_string()
{
	time_t now = time(0);
	return ctime(&now);
}

int main()
{
	try {
		// 기본적으로 Boost Asio 프로그램은 하나의 IO Service 객체를 가집니다.
		boost::asio::io_context io_context;
		//2. acceptor 객체
		// TCP 프로토콜의 13번 포트로 연결을 받는 수동 소켓을 생성합니다.
		tcp::acceptor acceptor(io_context, tcp::endpoint(tcp::v4(), 13));
		//3. 소켓 생성
		// 모든 클라이언트에 대해 무한정 반복 수행합니다.
		for (;;)
		{
			std::cout << std::endl;
			// 소켓 객체를 생성해 연결을 기다립니다.
			tcp::socket socket(io_context);
			acceptor.accept(socket);
			std::cout << "socket, accept" << std::endl;
			//4. 클라이언트에 데이터 전송
			// 연결이 완료되면 해당 클라이언트에게 보낼 메시지를 생성합니다.
			string message = make_daytime_string();
			std::cout << "message : " << message << std::endl;

			// 해당 클라이언트에게 메시지를 담아 전송합니다.
			boost::system::error_code ignored_error;
			std::cout << "error_code : " << ignored_error << std::endl;
			boost::asio::write(socket, boost::asio::buffer(message), ignored_error);
			std::cout << "전송" << std::endl;
		}
	}
	//5. 예외 처리
	catch (exception & e)
	{
		cerr << e.what() << endl;
	}
	return 0;
}
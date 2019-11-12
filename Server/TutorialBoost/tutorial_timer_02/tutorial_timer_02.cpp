
//비동기식 타이머를 세팅 하고, 출력 하는 튜토리얼

#include <iostream>
#include <boost/asio.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

//콜백 함수 선언
void print(const boost::system::error_code&)
{
	std::cout << "Hello, world!" << std::endl;
}

int main()
{
	boost::asio::io_context io;
	boost::asio::deadline_timer t(io, boost::posix_time::seconds(4));

	std::cout << "4초뒤 메세지가 출력 됩니다." << std::endl;
	
	//비동기 함수 async_wait() 호출
	t.async_wait(&print);

	//콜백 호출
	io.run();

	return 0;
}
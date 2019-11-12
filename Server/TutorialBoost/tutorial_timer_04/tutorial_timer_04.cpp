//
////멤버 함수를 핸들러로 사용
//
//#include <iostream>
//#include <boost/asio.hpp>
//#include <boost/bind.hpp>
//#include <boost/date_time/posix_time/posix_time.hpp>
//
//class printer
//{
//private:
//	boost::asio::deadline_timer timer;	//타이머
//	int count;							//시간 체크용
//public:  
//	printer(boost::asio::io_context& io) : timer(io, boost::posix_time::seconds(1)),count(0)
//	{
//		//비동기 대기
//		timer.async_wait(boost::bind(&printer::print, this));
//	}
//	~printer()
//	{
//		std::cout << "마지막 카운트 : " << count << std::endl;
//	}
//	void print()
//	{
//		if (count < 5)
//		{
//			std::cout << "시간 : " << count << std::endl;
//			++count;
//
//			timer.expires_at(timer.expires_at() + boost::posix_time::seconds(1));
//			timer.async_wait(boost::bind(&printer::print, this));
//		}
//	}
//};

#include "Timer.h"

int main()
{
	boost::asio::io_context io;
	Timer p(io);
	io.run();

	return 0;
}
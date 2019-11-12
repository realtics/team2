
//멀티 스레드 프로그램에서 핸들러 동기화
//strand 사용 (동기화)

//#include <iostream>
//#include <boost/asio.hpp>
//#include <boost/thread/thread.hpp>
//#include <boost/bind.hpp>
//#include <boost/date_time/posix_time/posix_time.hpp>
//
//class Timer
//{
//public:
//	Timer(boost::asio::io_context& io) :
//		strand(io),
//		timer1(io, boost::posix_time::seconds(1)),
//		timer2(io, boost::posix_time::seconds(1)),
//		count(0)
//	{
//		timer2.async_wait(boost::asio::bind_executor(strand,
//			boost::bind(&Timer::Print2, this)));
//
//		timer1.async_wait(boost::asio::bind_executor(strand,
//			boost::bind(&Timer::Print1, this)));
//
//	}
//
//	~Timer()
//	{
//		std::cout << "마지막 카운트 : " << count << std::endl;
//	}
//
//	void Print1()
//	{
//		if (count < 10)
//		{
//			std::cout << "타이머 1: " << count << std::endl;
//			++count;
//
//			timer1.expires_at(timer1.expires_at() + boost::posix_time::seconds(1));
//
//			timer1.async_wait(boost::asio::bind_executor(strand,
//				boost::bind(&Timer::Print1, this)));
//		}
//	}
//
//	void Print2()
//	{
//		if (count < 10)
//		{
//			std::cout << "타이머 2: " << count << std::endl;
//			++count;
//
//			timer2.expires_at(timer2.expires_at() + boost::posix_time::seconds(1));
//
//			timer2.async_wait(boost::asio::bind_executor(strand,
//				boost::bind(&Timer::Print2, this)));
//		}
//	}
//
//private:
//	boost::asio::io_context::strand strand;
//	boost::asio::deadline_timer timer1;
//	boost::asio::deadline_timer timer2;
//	int count;
//};

#include "Timer.h"

int main()
{
	boost::asio::io_context io;
	Timer p(io);
	boost::thread t(boost::bind(&boost::asio::io_context::run, &io));
	io.run();
	t.join();

	return 0;
}
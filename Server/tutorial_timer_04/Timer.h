#pragma once

//멤버 함수를 핸들러로 사용

#include <iostream>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

class Timer
{
private:
	boost::asio::deadline_timer timer;	//타이머
	int count;							//시간 체크용
public:
	Timer(boost::asio::io_context& io);
	~Timer();
	void Print();
};

#pragma once
#include <iostream>
#include <boost/asio.hpp>
#include <boost/thread/thread.hpp>
#include <boost/bind.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

class Timer
{
private:
	boost::asio::io_context::strand strand;
	boost::asio::deadline_timer timer1;
	boost::asio::deadline_timer timer2;
	int count;
public:
	Timer(boost::asio::io_context& io);
	~Timer();
	void Print1();
	void Print2();
};
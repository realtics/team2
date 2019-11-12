#include "Timer.h"


Timer::Timer(boost::asio::io_context& io) : timer(io, boost::posix_time::seconds(1)), count(0)
{
	//�񵿱� ���
	timer.async_wait(boost::bind(&Timer::Print, this));
}

Timer::~Timer()
{
	std::cout << "������ ī��Ʈ : " << count << std::endl;
}


void Timer::Print()
{
	if (count < 5)
	{
		std::cout << "�ð� : " << count << std::endl;
		++count;

		timer.expires_at(timer.expires_at() + boost::posix_time::seconds(1));
		timer.async_wait(boost::bind(&Timer::Print, this));
	}
}

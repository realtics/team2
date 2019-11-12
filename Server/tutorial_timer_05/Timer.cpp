#include "Timer.h"

Timer::Timer(boost::asio::io_context& io) :
	strand(io),
	timer1(io, boost::posix_time::seconds(1)),
	timer2(io, boost::posix_time::seconds(1)),
	count(0)
{
	//�񵿱� ��� 2��
	timer1.async_wait(boost::asio::bind_executor(strand, boost::bind(&Timer::Print1, this)));

	timer2.async_wait(boost::asio::bind_executor(strand, boost::bind(&Timer::Print2, this)));
}

Timer::~Timer()
{
	std::cout << "������ ī��Ʈ : " << count << std::endl;
}

void Timer::Print1()
{
	if (count < 10)
	{
		std::cout << "Ÿ�̸� 1: " << count << std::endl;
		++count;

		timer1.expires_at(timer1.expires_at() + boost::posix_time::seconds(1));

		timer1.async_wait(boost::asio::bind_executor(strand, boost::bind(&Timer::Print1, this)));
	}
}

void Timer::Print2()
{
	if (count < 10)
	{
		std::cout << "Ÿ�̸� 2: " << count << std::endl;
		++count;

		timer2.expires_at(timer2.expires_at() + boost::posix_time::seconds(1));

		timer2.async_wait(boost::asio::bind_executor(strand, boost::bind(&Timer::Print2, this)));
	}
}
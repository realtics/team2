//
////��� �Լ��� �ڵ鷯�� ���
//
//#include <iostream>
//#include <boost/asio.hpp>
//#include <boost/bind.hpp>
//#include <boost/date_time/posix_time/posix_time.hpp>
//
//class printer
//{
//private:
//	boost::asio::deadline_timer timer;	//Ÿ�̸�
//	int count;							//�ð� üũ��
//public:  
//	printer(boost::asio::io_context& io) : timer(io, boost::posix_time::seconds(1)),count(0)
//	{
//		//�񵿱� ���
//		timer.async_wait(boost::bind(&printer::print, this));
//	}
//	~printer()
//	{
//		std::cout << "������ ī��Ʈ : " << count << std::endl;
//	}
//	void print()
//	{
//		if (count < 5)
//		{
//			std::cout << "�ð� : " << count << std::endl;
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
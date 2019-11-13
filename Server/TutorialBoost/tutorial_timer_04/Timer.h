#pragma once

//��� �Լ��� �ڵ鷯�� ���

#include <iostream>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

class Timer
{
private:
	boost::asio::deadline_timer timer;	//Ÿ�̸�
	int count;							//�ð� üũ��
public:
	Timer(boost::asio::io_context& io);
	~Timer();
	void Print();
};

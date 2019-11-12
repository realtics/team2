
//Ÿ�̸Ӱ� 1�ʿ� �ѹ� �߻� �ϵ��� �ϰ�, �ڵ鷯�� �߰� �Ű������� ����
//�μ��� �ڵ鷯�� ���ε�

#include <iostream>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

void print(const boost::system::error_code& e, boost::asio::steady_timer& timer, int& count)
{
	if (count < 5)
	{
		std::cout << "time : " << count << "\n";
		++(count);

		//�ð� ������ expires ������ �Լ��� ���ؼ�.
		timer.expires_from_now(boost::asio::chrono::seconds(1));
		timer.async_wait(boost::bind(print, boost::asio::placeholders::error, boost::ref(timer), boost::ref(count)));
	}
}

bool boostAsioTimer()
{
	boost::asio::io_service io_service;

	int count = 0;
	boost::asio::steady_timer timer(io_service);

	timer.async_wait(boost::bind(
								print,
								boost::asio::placeholders::error,
								boost::ref(timer),
								boost::ref(count)));
	io_service.run();

	std::cout << "count = " << count << std::endl;
	return true;
}

int main()
{
	boostAsioTimer();

	return 0;
}
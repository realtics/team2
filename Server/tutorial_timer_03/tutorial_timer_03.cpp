
//타이머가 1초에 한번 발생 하도록 하고, 핸들러에 추가 매개변수를 전달
//인수를 핸들러에 바인딩

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

		//시간 설정은 expires 종류의 함수를 통해서.
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
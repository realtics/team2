
//����� Ÿ�̸Ӹ� ���� �ϰ�, ��� �ϴ� Ʃ�丮��

#include <iostream>
#include <boost/asio.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

int main()
{
	//io_context ��ü ����
	boost::asio::io_service io;

	while (true)
	{
		boost::asio::deadline_timer t(io, boost::posix_time::seconds(1));
		t.wait();
		std::cout << "Hello, world! 1" << std::endl;

		boost::asio::steady_timer t2(io, boost::asio::chrono::seconds(1));
		t2.wait();
		std::cout << "Hello, world! 2" << std::endl;
	}
	system("pause");

	return 0;
}
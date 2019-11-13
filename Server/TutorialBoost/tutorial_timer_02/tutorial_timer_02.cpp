
//�񵿱�� Ÿ�̸Ӹ� ���� �ϰ�, ��� �ϴ� Ʃ�丮��

#include <iostream>
#include <boost/asio.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

//�ݹ� �Լ� ����
void print(const boost::system::error_code&)
{
	std::cout << "Hello, world!" << std::endl;
}

int main()
{
	boost::asio::io_context io;
	boost::asio::deadline_timer t(io, boost::posix_time::seconds(4));

	std::cout << "4�ʵ� �޼����� ��� �˴ϴ�." << std::endl;
	
	//�񵿱� �Լ� async_wait() ȣ��
	t.async_wait(&print);

	//�ݹ� ȣ��
	io.run();

	return 0;
}
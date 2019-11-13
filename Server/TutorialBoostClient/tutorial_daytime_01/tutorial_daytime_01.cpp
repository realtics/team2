#include <iostream>
#include <boost/array.hpp>
#include <boost/asio.hpp>

using boost::asio::ip::tcp;
using namespace std;
//1. ����� �Է�
int main(/*int argc, char* argv[]*/)
{
	try {
		/*if (argc != 2) {
		  std::cerr << "Usage: client <host>" << std::endl;
		  return 1;
		}*/
		//2. io_context ��ü
		// �⺻������ Boost Asio ���α׷��� �ϳ��� IO Service ��ü�� �����ϴ�.
		boost::asio::io_context io_context;
		//3. resolver ��ü
		// ������ �̸��� TCP ���������� �ٲٱ� ���� Resolver�� ����մϴ�.
		tcp::resolver resolver(io_context);
		//4. query ���� �� endpoint ���ϱ�
		// �����δ� ���� ����, ���񽺴� Daytime ���������� �����ݴϴ�.
		tcp::resolver::query query("127.0.0.1", "daytime");
		// DNS�� ���� IP �ּ� �� ��Ʈ ��ȣ�� ���ɴϴ�.
		tcp::resolver::iterator endpoint = resolver.resolve(query);
		/*tcp::resolver::results_type endpoint = resolver.resolve(argv[1], "daytime");*/
		//5. ���� ���� �� ����
		// ���� ��ü�� �ʱ�ȭ�Ͽ� ������ �����մϴ�.
		tcp::socket socket(io_context);
		boost::asio::connect(socket, endpoint);
		//6. ���� �Ϸ�
		for (;;)
		{
			// ���� �� ���� ó�� ������ �����մϴ�.
			boost::array<char, 128> buf;
			boost::system::error_code error;
			//7. ���ŵ� ������ ����
			// ���۸� �̿��� �����κ��� �����͸� �޾ƿɴϴ�.
			size_t len = socket.read_some(boost::asio::buffer(buf), error);
			if (error == boost::asio::error::eof)
				break;
			else if (error)
				throw boost::system::system_error(error);
			//8. ���
			// ���ۿ� ��� �����͸� ȭ�鿡 ����մϴ�.
			cout.write(buf.data(), len);

		}
	}
	//9. ���� ó��
	catch (exception & e)
	{
		cerr << e.what() << endl;
	}
	return 0;
}
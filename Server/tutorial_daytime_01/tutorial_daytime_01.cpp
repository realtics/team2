#include <iostream>
#include <boost/array.hpp>
#include <boost/asio.hpp>

using boost::asio::ip::tcp;
using namespace std;
//1. 사용자 입력
int main(/*int argc, char* argv[]*/)
{
	try {
		/*if (argc != 2) {
		  std::cerr << "Usage: client <host>" << std::endl;
		  return 1;
		}*/
		//2. io_context 객체
		// 기본적으로 Boost Asio 프로그램은 하나의 IO Service 객체를 가집니다.
		boost::asio::io_context io_context;
		//3. resolver 객체
		// 도메인 이름을 TCP 종단점으로 바꾸기 위해 Resolver를 사용합니다.
		tcp::resolver resolver(io_context);
		//4. query 생성 및 endpoint 구하기
		// 서버로는 로컬 서버, 서비스는 Daytime 프로토콜을 적어줍니다.
		tcp::resolver::query query("127.0.0.1", "daytime");
		// DNS를 거쳐 IP 주소 및 포트 번호를 얻어옵니다.
		tcp::resolver::iterator endpoint = resolver.resolve(query);
		/*tcp::resolver::results_type endpoint = resolver.resolve(argv[1], "daytime");*/
		//5. 소켓 생성 및 연결
		// 소켓 객체를 초기화하여 서버에 연결합니다.
		tcp::socket socket(io_context);
		boost::asio::connect(socket, endpoint);
		//6. 연결 완료
		for (;;)
		{
			// 버퍼 및 오류 처리 변수를 선언합니다.
			boost::array<char, 128> buf;
			boost::system::error_code error;
			//7. 수신된 데이터 저장
			// 버퍼를 이용해 서버로부터 데이터를 받아옵니다.
			size_t len = socket.read_some(boost::asio::buffer(buf), error);
			if (error == boost::asio::error::eof)
				break;
			else if (error)
				throw boost::system::system_error(error);
			//8. 출력
			// 버퍼에 담긴 데이터를 화면에 출력합니다.
			cout.write(buf.data(), len);

		}
	}
	//9. 예외 처리
	catch (exception & e)
	{
		cerr << e.what() << endl;
	}
	return 0;
}
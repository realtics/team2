#include "AsioServer.h"

const int MAX_SESSION_COUNT = 40;

int main()
{
	boost::asio::io_context io_context;

	AsioServer server(io_context);
	server.Init(MAX_SESSION_COUNT);
	server.Start();

	io_context.run();

	std::cout << "네트워크 접속 종료" << std::endl;

	getchar();
	return 0;
}

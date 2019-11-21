#include "AsioServer.h"

const int MAX_SESSION_COUNT = 50;

int main()
{
	boost::asio::io_context io_context;

	AsioServer server(io_context);
	server.Init(MAX_SESSION_COUNT);
	server.Start();

	io_context.run();

	std::cout << "匙飘况农 立加 辆丰" << std::endl;

	getchar();
	return 0;
}

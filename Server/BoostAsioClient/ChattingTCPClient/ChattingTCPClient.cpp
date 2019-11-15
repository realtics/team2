#include "ChatClient.h"

#define IPAddress "127.0.0.1"

int main()
{
	boost::asio::io_context io_context;

	auto endpoint = boost::asio::ip::tcp::endpoint(boost::asio::ip::make_address(IPAddress), PORT_NUMBER);

	ChatClient Cliet(io_context);
	Cliet.Connect(endpoint);

	boost::thread thread(boost::bind(&boost::asio::io_context::run, &io_context));

	char inputMessage[MAX_MESSAGE_LEN * 2] = { 0, };

	while (std::cin.getline(inputMessage, MAX_MESSAGE_LEN))
	{
		if (strnlen_s(inputMessage, MAX_MESSAGE_LEN) == 0)
		{
			break;
		}

		if (Cliet.IsConnecting() == false)
		{
			std::cout << "서버와 연결되지 않았습니다" << std::endl;
			continue;
		}

		if (Cliet.IsLogin() == false)
		{
			PKT_REQ_IN SendPkt;
			SendPkt.Init();
			strncpy_s(SendPkt.szName, MAX_NAME_LEN, inputMessage, MAX_NAME_LEN - 1);

			Cliet.PostSend(false, SendPkt.nSize, (char*)&SendPkt);
		}
		else
		{
			PKT_REQ_CHAT SendPkt;
			SendPkt.Init();
			strncpy_s(SendPkt.szMessage, MAX_MESSAGE_LEN, inputMessage, MAX_MESSAGE_LEN - 1);

			Cliet.PostSend(false, SendPkt.nSize, (char*)&SendPkt);
		}
	}

	io_context.stop();

	Cliet.Close();

	thread.join();

	std::cout << "클라이언트를 종료해 주세요" << std::endl;

	return 0;
}
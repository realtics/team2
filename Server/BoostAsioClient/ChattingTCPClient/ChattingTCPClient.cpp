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
			continue;
		}

		if (Cliet.IsConnecting() == false)
		{
			std::cout << "������ ������� �ʾҽ��ϴ�" << std::endl;
			continue;
		}

		if (Cliet.IsLogin() == false)
		{
			PKT_REQ_IN SendPkt;
			SendPkt.Init();
			strncpy_s(SendPkt.characterName, MAX_NAME_LEN, inputMessage, MAX_NAME_LEN - 1);

			Cliet.PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
		}
		else
		{
			PKT_REQ_CHAT SendPkt;
			SendPkt.Init();
			strncpy_s(SendPkt.userMessage, MAX_MESSAGE_LEN, inputMessage, MAX_MESSAGE_LEN - 1);

			Cliet.PostSend(false, SendPkt.packetSize, (char*)&SendPkt);
		}
	}

	io_context.stop();

	Cliet.Close();

	thread.join();

	std::cout << "Ŭ���̾�Ʈ�� ������ �ּ���" << std::endl;

	return 0;
}
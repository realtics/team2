#include "DBMySQL.h"

DBMySQL::DBMySQL()
{
	
}

DBMySQL::~DBMySQL()
{
	mysql_close(_pConnection);
}

void DBMySQL::Init()
{
	if (mysql_init(&_mysql) == NULL)
	{
		std::cout << "MySQL Init() error" << std::endl;
	}

	_pConnection = mysql_real_connect(&_mysql, _host, _user, _password, _dbName, _port, (const char*)NULL, 0);
	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		std::cout << "DB 연결 성공" << std::endl;
	}

	//한글 깨짐 처리
	mysql_query(&_mysql, "set names euckr");
}

void DBMySQL::DBDataLoginSelectAll()
{
	mysql_select_db(&_mysql, _dbName);

	const char* tempQuery = "SELECT * FROM login";

	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		int tempState = mysql_query(_pConnection, tempQuery);
		if (tempState == 0)
		{
			_pSqlResult = mysql_store_result(_pConnection);

			int fields = mysql_num_fields(_pSqlResult);

			while ((_SqlRow = mysql_fetch_row(_pSqlResult)) != NULL)
			{
				std::cout << "idx" << "\t" << "userid" << "\t" << "pw" << "\t"
					<< "name" << "\t" << "lastConnect" << std::endl;

				for (int i = 0; i < fields; i++)
				{
					if (_SqlRow[i] != NULL)
						std::cout << _SqlRow[i];
					else
						std::cout << "\t";

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}
}

void DBMySQL::DBMySQLVersion()	
{
	std::cout << "Connect MySQL version : " << mysql_get_client_info() << std::endl;
}

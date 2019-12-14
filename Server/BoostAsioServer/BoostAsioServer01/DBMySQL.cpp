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

void DBMySQL::DBMySQLVersion()
{
	std::cout << "Connect MySQL version : " << mysql_get_client_info() << std::endl;
}

void DBMySQL::DBDataLoginSelectAll()
{
	mysql_select_db(&_mysql, _dbName);

	const char* DBQuery = "SELECT * FROM login";

	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		int DBState = mysql_query(_pConnection, DBQuery);
		if (DBState == 0)
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

int DBMySQL::DBLoginCheckUserID(std::string checkID)
{
	mysql_select_db(&_mysql, _dbName);
	
	const char* DBQuery1 = "SELECT user_id FROM login WHERE user_ID = \"";
	const char* DBQuery2 = checkID.c_str();
	const char* DBQuery3 = "\";";

	char DBQuery[256] = "";

	strcat_s(DBQuery, DBQuery1);
	strcat_s(DBQuery, DBQuery2);
	strcat_s(DBQuery, DBQuery3);

	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		int DBState = mysql_query(_pConnection, DBQuery);
		if (DBState == 0)
		{
			_pSqlResult = mysql_store_result(_pConnection);

			int fields = mysql_num_fields(_pSqlResult);

			while ((_SqlRow = mysql_fetch_row(_pSqlResult)) != NULL)
			{
				for (int i = 0; i < fields; i++)
				{
					if (_SqlRow[i] != NULL)
					{
						std::cout << _SqlRow[i] << " = " << checkID.c_str() << " 비교" << std::endl;

						if (!strcmp(_SqlRow[i], checkID.c_str()))
						{
							return CHECK_BEFORE_LOGIN_RESULT::RESULT_SUCCESS;
						}
					}
					else
					{
						return CHECK_BEFORE_LOGIN_RESULT::RESULT_NO_ID;
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return CHECK_BEFORE_LOGIN_RESULT::RESULT_NO_ID;
}

int DBMySQL::DBLoginCheckUserPW(std::string checkID, std::string checkPW)
{
	mysql_select_db(&_mysql, _dbName);

	const char* DBQuery1 = "SELECT user_password FROM login WHERE user_id = \"";
	const char* DBQuery2 = checkID.c_str();
	const char* DBQuery3 = "\";";

	char DBQuery[256] = "";

	strcat_s(DBQuery, DBQuery1);
	strcat_s(DBQuery, DBQuery2);
	strcat_s(DBQuery, DBQuery3);

	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		int DBState = mysql_query(_pConnection, DBQuery);
		if (DBState == 0)
		{
			_pSqlResult = mysql_store_result(_pConnection);

			int fields = mysql_num_fields(_pSqlResult);

			while ((_SqlRow = mysql_fetch_row(_pSqlResult)) != NULL)
			{
				for (int i = 0; i < fields; i++)
				{
					if (_SqlRow[i] != NULL)
					{
						std::cout << _SqlRow[i] << " = " << checkPW.c_str() << " 비교" << std::endl;

						if (!strcmp(_SqlRow[i], checkPW.c_str()))
						{
							return CHECK_BEFORE_LOGIN_RESULT::RESULT_SUCCESS;
						}
					}
					else
					{
						return CHECK_BEFORE_LOGIN_RESULT::RESULT_IS_WRONG_PASSWORD;
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return CHECK_BEFORE_LOGIN_RESULT::RESULT_IS_WRONG_PASSWORD;
}

std::string DBMySQL::DBLoginGetUserName(std::string userID)
{
	mysql_select_db(&_mysql, _dbName);

	const char* DBQuery1 = "SELECT user_name FROM login WHERE user_id = \"";
	const char* DBQuery2 = userID.c_str();
	const char* DBQuery3 = "\";";

	char DBQuery[256] = "";

	strcat_s(DBQuery, DBQuery1);
	strcat_s(DBQuery, DBQuery2);
	strcat_s(DBQuery, DBQuery3);

	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		int DBState = mysql_query(_pConnection, DBQuery);
		if (DBState == 0)
		{
			_pSqlResult = mysql_store_result(_pConnection);

			int fields = mysql_num_fields(_pSqlResult);

			while ((_SqlRow = mysql_fetch_row(_pSqlResult)) != NULL)
			{
				for (int i = 0; i < fields; i++)
				{
					if (_SqlRow[i] != NULL)
					{
						std::cout << userID << "의 캐릭터 명 : " << _SqlRow[i] << std::endl;

						return _SqlRow[i];
					}
					else
					{
						return "캐릭터 명 없음";
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return "캐릭터 명 없음";
}

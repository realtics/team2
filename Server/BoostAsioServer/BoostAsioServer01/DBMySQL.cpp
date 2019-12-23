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
				std::cout << "index" << "\t" << "user_id" << "\t" << "user_pw" << "\t"
					<< "user_name" << "\t" << "sign_date" << std::endl;

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

int DBMySQL::DBSignUp(std::string inputID, std::string inputPW, std::string inputName)
{
	mysql_select_db(&_mysql, _dbName);
	//INSERT INTO login(user_id, user_password, user_name, sign_date) VALUES ('dddd', 'dddd', '디디디디',CURRENT_TIMESTAMP);
	const char* DBQuery1 = "INSERT INTO login(user_id, user_password, user_name, sign_date) VALUES('";
	const char* DBQuery2 = inputID.c_str();
	const char* DBQuery3 = "', '";
	const char* DBQuery4 = inputPW.c_str();
	const char* DBQuery5 = "', '";
	const char* DBQuery6 = inputName.c_str();
	const char* DBQuery7 = "', ";
	const char* DBQuery8 = "CURRENT_TIMESTAMP);";

	char DBQuery[256] = "";
	strcat_s(DBQuery, DBQuery1);
	strcat_s(DBQuery, DBQuery2);
	strcat_s(DBQuery, DBQuery3);
	strcat_s(DBQuery, DBQuery4);
	strcat_s(DBQuery, DBQuery5);
	strcat_s(DBQuery, DBQuery6);
	strcat_s(DBQuery, DBQuery7);
	strcat_s(DBQuery, DBQuery8);

	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		int DBState = mysql_query(_pConnection, DBQuery);
		if (DBState == 0)
		{
			std::cout << "insert 성공 " << std::endl;

			_pSqlResult = mysql_store_result(_pConnection);

			mysql_free_result(_pSqlResult);

			return RESULT_SIGN_UP_CHECK::RESULT_SIGN_UP_CHECK_SUCCESS;
		}
		else
		{
			std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;

			if (mysql_errno(&_mysql) == 1062)	// 1062 : Duplicate entry '????' for key 'user_id'
			{
				return RESULT_SIGN_UP_CHECK::RESULT_SIGN_UP_OVERLAP_ID;
			}

			return RESULT_SIGN_UP_CHECK::RESULT_SIGN_UP_UNKNOWN;
		}
	}

	return RESULT_SIGN_UP_CHECK::RESULT_SIGN_UP_UNKNOWN;
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
							return RESULT_BEFORE_LOGIN_CHECK::RESULT_BEFORE_LOGIN_CHECK_SUCCESS;
						}
					}
					else
					{
						return RESULT_BEFORE_LOGIN_CHECK::RESULT_BEFORE_LOGIN_CHECK_NO_ID;
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return RESULT_BEFORE_LOGIN_CHECK::RESULT_BEFORE_LOGIN_CHECK_NO_ID;
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
							return RESULT_BEFORE_LOGIN_CHECK::RESULT_BEFORE_LOGIN_CHECK_SUCCESS;
						}
					}
					else
					{
						return RESULT_BEFORE_LOGIN_CHECK::RESULT_BEFORE_LOGIN_CHECK_IS_WRONG_PASSWORD;
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return RESULT_BEFORE_LOGIN_CHECK::RESULT_BEFORE_LOGIN_CHECK_IS_WRONG_PASSWORD;
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

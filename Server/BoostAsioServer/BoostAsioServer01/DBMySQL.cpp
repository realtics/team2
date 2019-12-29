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

			std::cout << "index" << "\t\t" << "user_id" << "\t\t" << "user_pw" << "\t\t\t\t\t"
				<< "user_name" << "\t" << "sign_date" << std::endl;
			while ((_SqlRow = mysql_fetch_row(_pSqlResult)) != NULL)
			{
				for (int i = 0; i < fields; i++)
				{
					if (_SqlRow[i] != NULL)
						std::cout << _SqlRow[i];
					else
						std::cout << "\t";

					std::cout << "\t\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
		std::cout << std::endl;
	}
}

int DBMySQL::DBSignUpCreate(std::string inputID, std::string inputPW, std::string inputName)
{
	mysql_select_db(&_mysql, _dbName);
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
			_pSqlResult = mysql_store_result(_pConnection);

			mysql_free_result(_pSqlResult);

			std::cout << "[DB][ID] " << inputID << " [Name] " << inputName << " [회원가입 성공] " << std::endl;

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

int DBMySQL::DBInventoryCreate(std::string inputID)
{
	mysql_select_db(&_mysql, _dbName);
	const char* DBQuery1 = "INSERT INTO inventory(user_id) VALUES('";
	const char* DBQuery2 = inputID.c_str();
	const char* DBQuery3 = "');";

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

			mysql_free_result(_pSqlResult);

			std::cout << "[DB][ID] " << inputID << " [Inventory 생성 성공]" << std::endl;

			return RESULT_INVENTORY_CHECK::RESULT_INVENTORY_CHECK_SUCCESS;
		}
		else
		{
			std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;

			return RESULT_INVENTORY_CHECK::RESULT_INVENTORY_CHECK_UNKNOWN;
		}
	}

	return RESULT_INVENTORY_CHECK::RESULT_INVENTORY_CHECK_UNKNOWN;
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

int DBMySQL::DBDungeonClearResultItemSize()
{
	mysql_select_db(&_mysql, _dbName);

	char DBQuery[256] = "SELECT COUNT(item_id) AS cnt FROM result_items";

	int result = 0;

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
						result = boost::lexical_cast<int>(_SqlRow[i]);
					}
				}
			}
		}
		mysql_free_result(_pSqlResult);
	}

	return result;
}

std::string DBMySQL::DBDungeonClearResultItem(int resultRandom)
{
	mysql_select_db(&_mysql, _dbName);
	
	const char* DBQuery1 = "SELECT item_id FROM result_items WHERE item_id = \"";
	std::string tostring = std::to_string(resultRandom);
	const char* DBQuery2 = tostring.c_str();
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
						std::cout << "[DB][결과 아이템 ID]" << _SqlRow[i] << std::endl;

						return _SqlRow[i];
					}
					else
					{
						return "no_id";
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return "no_id";
}

int DBMySQL::DBDungeonHellResultItemSize()
{
	mysql_select_db(&_mysql, _dbName);

	char DBQuery[256] = "SELECT COUNT(item_id) AS cnt FROM hell_items";

	int result = 0;

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
						result = boost::lexical_cast<int>(_SqlRow[i]);
					}
				}
			}
		}
		mysql_free_result(_pSqlResult);
	}

	return result;
}

std::string DBMySQL::DBDungeonHellResultItem(int resultRandom)
{
	mysql_select_db(&_mysql, _dbName);

	const char* DBQuery1 = "SELECT item_id FROM hell_items WHERE item_id = \"";
	std::string tostring = std::to_string(resultRandom);
	const char* DBQuery2 = tostring.c_str();
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
						std::cout << "[DB][헬 아이템 ID]" << _SqlRow[i] << std::endl;

						return _SqlRow[i];
					}
					else
					{
						return "no_id";
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return "no_id";
}

std::array<std::string, MAX_INVENTORY_COLUMN> DBMySQL::DBInventorySelect(std::string inputID)
{
	mysql_select_db(&_mysql, _dbName);

	const char* DBQuery1 = "SELECT* FROM inventory WHERE user_id = '";
	const char* DBQuery2 = inputID.c_str();
	const char* DBQuery3 = "';";

	char DBQuery[256] = "";

	strcat_s(DBQuery, DBQuery1);
	strcat_s(DBQuery, DBQuery2);
	strcat_s(DBQuery, DBQuery3);

	std::array<std::string, MAX_INVENTORY_COLUMN> userInventory;

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

			std::cout << "[DB][" << inputID << "의 Inventory] ";
			while ((_SqlRow = mysql_fetch_row(_pSqlResult)) != NULL)
			{
				for (int i = 0; i < fields; i++)
				{
					if (_SqlRow[i] != NULL)
					{
						std::cout << _SqlRow[i];
						userInventory[i] = _SqlRow[i];
						//return _SqlRow[i];
					}
					else
					{
						std::cout << "null" << std::endl;
						userInventory[i] = "";
					}

					std::cout << ", ";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return userInventory;
}

int DBMySQL::DBInventoryUpdate(std::string inputID, char (*arr1)[MAX_USER_ITEM_LEN], char (*arr2)[MAX_USER_ITEM_LEN])
{
	mysql_select_db(&_mysql, _dbName);
	
	// 기본 형태 : UPDATE inventory SET equip_00='6012', equip_01='0', equip_02='1006', equip_03='1005', equip_04='0', equip_05='0', equip_06='6002', equip_07='0', equip_08='0', equip_09='6011', equip_10='0', equip_11='6004', user_inventory='6002,1005,1006' WHERE user_id="test";

	std::string inventoryString;
	for (int i = 0; i < MAX_USER_INVENTORY; i++)
	{
		if (strcmp(arr2[0], "") == 0)
			break;

		if (strcmp(arr2[i], "") != 0)
		{
			inventoryString += arr2[i];
			inventoryString += ",";
		}
		else
		{
			int endInventoryString = inventoryString.size() - 1;
			inventoryString.replace(endInventoryString, endInventoryString, "");

			break;
		}
	}

	std::string DBQuery;
	DBQuery += "UPDATE inventory SET ";

	for (int i = 0; i < MAX_USER_EQUIP; i++)
	{
		if (i < 10)
		{
			DBQuery += "equip_0";
			DBQuery += std::to_string(i);
			DBQuery += "='";
			DBQuery += arr1[i];
			DBQuery += "', ";
		}
		else if (i == (MAX_USER_EQUIP - 1))
		{
			DBQuery += "equip_";
			DBQuery += std::to_string(i);
			DBQuery += "='";
			DBQuery += arr1[i];
			DBQuery += "', user_inventory='";
			DBQuery += inventoryString;
			DBQuery += "' WHERE user_id=\"";
			DBQuery += inputID.c_str();
			DBQuery += "\";";
		}
		else
		{
			DBQuery += "equip_";
			DBQuery += std::to_string(i);
			DBQuery += "='";
			DBQuery += arr1[i];
			DBQuery += "', ";
		}

	}

	if (_pConnection == NULL)
	{
		std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;
	}
	else
	{
		int DBState = mysql_query(_pConnection, DBQuery.c_str());
		if (DBState == 0)
		{
			_pSqlResult = mysql_store_result(_pConnection);

			mysql_free_result(_pSqlResult);

			std::cout << "[DB][ID] " << inputID << " [인벤토리 갱신]" << std::endl;

			return RESULT_INVENTORY_CHECK::RESULT_INVENTORY_CHECK_SUCCESS;
		}
		else
		{
			std::cout << mysql_errno(&_mysql) << " 에러 : " << mysql_error(&_mysql) << std::endl;

			return RESULT_INVENTORY_CHECK::RESULT_INVENTORY_CHECK_UNKNOWN;
		}
	}

	return RESULT_INVENTORY_CHECK::RESULT_INVENTORY_CHECK_UNKNOWN;
}

std::string DBMySQL::DBLoginGetUserName(std::string inputID)
{
	mysql_select_db(&_mysql, _dbName);

	const char* DBQuery1 = "SELECT user_name FROM login WHERE user_id = \"";
	const char* DBQuery2 = inputID.c_str();
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
						std::cout << inputID << "의 캐릭터 명 : " << _SqlRow[i] << std::endl;

						return _SqlRow[i];
					}
					else
					{
						return "no_character_name";
					}

					std::cout << "\t";
				}
				std::cout << std::endl;
			}
			mysql_free_result(_pSqlResult);
		}
	}

	return "no_character_name";
}

#pragma once

#pragma comment(lib, "libmysql.lib")

#include <iostream>
#include <winsock2.h>
#include <mysql.h>

class DBMySQL
{
private:
	const char* _host = "localhost";
	const char* _user = "mdnf";
	const char* _password = "mdnf1";
	const char* _dbName = "mdnf";
	unsigned int _port = 3306;

	MYSQL* _pConnection = NULL;
	MYSQL _mysql;
	MYSQL_RES* _pSqlResult;
	MYSQL_ROW _SqlRow;
public:
	DBMySQL();
	~DBMySQL();

	void Init();

	void DBDataLoginSelectAll();

	void DBMySQLVersion();
};
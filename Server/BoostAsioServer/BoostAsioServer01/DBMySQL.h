#pragma once

#include <winsock2.h>
#include <mysql.h>

class DBMySQL
{
private:
	MYSQL _mysql;
public:
	DBMySQL();
	~DBMySQL();
};
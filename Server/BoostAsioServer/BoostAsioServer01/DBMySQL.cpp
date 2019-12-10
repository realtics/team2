#include "DBMySQL.h"

DBMySQL::DBMySQL()
{
	std::cout << "Connect MySQL version : " << mysql_get_client_info() << std::endl;
}

DBMySQL::~DBMySQL()
{

}

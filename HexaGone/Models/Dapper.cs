using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;



namespace HexaGone.Models
{
    public static class Dapper
    {
        public static string connectionString = "server=remotemysql.com;port=3306;user id=fWMQMukiaJ; password=Zhc52KVhNT; Allow User Variables=true; database=fWMQMukiaJ; SslMode=none";

        public static MySqlConnection sqlConnection = new MySqlConnection(connectionString);

    }
}
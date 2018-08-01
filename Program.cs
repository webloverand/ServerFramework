
using MySql.Data.MySqlClient;
using ServerFramework.Tool;
using System;

namespace ServerFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            MySQLHelper mySqlHelper = new MySQLHelper();
            MySqlDataReader dataReader = mySqlHelper.ExecuteReader("select * from servertest");
            while (dataReader.Read())
            {
                Console.WriteLine(dataReader.GetString("ID") + " " + dataReader.GetString("Name"));
            }
            Console.ReadKey();

        }
    }
}

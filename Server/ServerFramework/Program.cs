
using MySql.Data.MySqlClient;
using ServerFramework.Servers;
using ServerFramework.Tool;
using System;

namespace ServerFramework
{
    class Program
    {
        static void Main(string[] args)
        {

            //MySqlDataReader dataReader = MySQLHelper.ExecuteReader(MySQLHelper.Connect(), "select * from servertest");
            //while (dataReader.Read())
            //{
            //    Console.WriteLine(dataReader.GetString("ID") + " " + dataReader.GetString("Name"));
            //}
            //Console.ReadKey();

            Server server = new Server("192.168.31.68", 6688);
            server.Start();

            Console.ReadKey();
        }
    }
}

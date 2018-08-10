using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using ServerFramework.Tool.Singleton;

namespace ServerFramework.Tool
{
    class MySQLHelper : Singleton<MySQLHelper>
    {
        public const string ConnectionString = "datasource=127.0.0.1;port=3306;database=servicefw;user=root;pwd=134855";

        public MySQLHelper()
        {
            Connect();
        }
        /// <summary>
        /// 连接
        /// </summary>
        public MySqlConnection Connect()
        {
            MySqlConnection SqlConn = new MySqlConnection(ConnectionString);
            try
            {
                SqlConn.Open();
                return SqlConn;
            }
            catch(Exception e)
            {
                Console.WriteLine("[MySQLHelper]Connect:" + e.Message);
                return null;
            }
        }
        //关闭数据库连接
        public void CloseConnection(MySqlConnection SqlConn)
        {
            if (SqlConn != null)
                SqlConn.Close();
            else
            {
                Console.WriteLine("MySqlConnection不能为空");
            }
        }
        //判定安全字符串
        public bool IsSafeStr(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
    }
}

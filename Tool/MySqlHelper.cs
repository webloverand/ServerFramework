using System;
using MySql.Data.MySqlClient;

namespace ServerFramework.Tool
{
    class MySQLHelper
    {
        public const string ConnectionString = "datasource=127.0.0.1;port=3306;database=servicefw;user=root;pwd=134855";
        public MySqlConnection SqlConn;

        public MySQLHelper()
        {
            Connect();
        }
        /// <summary>
        /// 连接
        /// </summary>
        public void Connect()
        {
            SqlConn = new MySqlConnection(ConnectionString);
            try
            {
                SqlConn.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("Connect数据库的时候出现异常:" + e);
            }
        }

        public void DisConnect()
        {
            if (SqlConn != null)
                SqlConn.Close();
            else
            {
                Console.WriteLine("DisConnect MySqlConnection为空");
            }
        }
        //查询调用
        public MySqlDataReader ExecuteReader(string sqlC)
        {
            MySqlCommand cmd = new MySqlCommand(sqlC, SqlConn);
            return cmd.ExecuteReader();
        }
        //插入/删除/更新等无返回值命令调用
        public void ExecuteNonQuery(string sqlC)
        {
            MySqlCommand cmd = new MySqlCommand(sqlC, SqlConn);
            cmd.ExecuteNonQuery();
        }
        
    }
}

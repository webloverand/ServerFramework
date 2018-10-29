using MySql.Data.MySqlClient;
using Protocol;
using ServerFramework.Controller;
using ServerFramework.Model;
using ServerFramework.Tool;
using System;

//数据访问对象（Data Access Object）
namespace ServerFramework.DAO
{
    class UserDataDAO
    {
        public UserData VerifyUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                if (!MySQLHelper.Instance.IsSafeStr(username))
                {
                    ControllerManager.reasonCode = ReasonCode.IllegalCharacter;
                    return null;
                }
                if (!MySQLHelper.Instance.IsSafeStr(password))
                {
                    ControllerManager.reasonCode = ReasonCode.IllegalCharacter;
                    return null;
                }
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ControllerManager.reasonCode = ReasonCode.None;
                    UserData user = new UserData(username, password);
                    return user;
                }
                else
                {
                    ControllerManager.reasonCode = ReasonCode.UsernameOrPwdError;
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public bool GetUserByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                if (!MySQLHelper.Instance.IsSafeStr(username))
                {
                    ControllerManager.reasonCode = ReasonCode.IllegalCharacter;
                    return false;
                }
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
        }

        public bool AddUser(MySqlConnection conn, string username, string password)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username = @username , password = @password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                ControllerManager.reasonCode = ReasonCode.DatabaseException;
                Console.WriteLine("在AddUser的时候出现异常：" + e);
                return false;
            }
        }
    }
}

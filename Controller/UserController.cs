using Common;
using ServerFramework.DAO;
using ServerFramework.Model;
using ServerFramework.Servers;
using System;

namespace ServerFramework.Controller
{
    class UserController : BaseController
    {
        private UserDataDAO userDAO = new UserDataDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            UserData user = userDAO.VerifyUser(client.MySQLConn, strs[0], strs[1]);
            if (user == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                client.SetUserData(user);
                return string.Format("{0},{1}", ((int)ReturnCode.Success).ToString(), user.Username);
            }
        }
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0]; string password = strs[1];
            bool res = userDAO.GetUserByUsername(client.MySQLConn, username);
            if (res)
            {
                ControllerManager.reasonCode = ReasonCode.RepeatRegister;
                return ((int)ReturnCode.Fail).ToString();
            }
            if(userDAO.AddUser(client.MySQLConn, username, password))
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}

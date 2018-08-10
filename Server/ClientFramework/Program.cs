using ClientFramework.Request;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientManage clientManage = new ClientManage();
            clientManage.OnInit();

            LoginRequest loginRequest = new LoginRequest();
            RegisterRequest registerRequest = new RegisterRequest();

            while(true)
            {
                if (Console.ReadKey().KeyChar == 'l')
                {
                    loginRequest.SendRequest(clientManage, "张三", "123");
                }
                if (Console.ReadKey().KeyChar == 'r')
                {
                    registerRequest.SendRequest(clientManage, "张三", "123");
                }
            }
        }
    }
}

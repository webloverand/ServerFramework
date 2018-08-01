using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//需要导入的
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

namespace ServerFramework.Servers
{
    class Server
    {
        private IPEndPoint iPEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();

        public Server() { }
        public Server(string ipStr,int port)
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port); //设置IP与port
        }

    }
}

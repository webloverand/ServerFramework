using Protocol;
using MySql.Data.MySqlClient;
using ServerFramework.DAO;
using ServerFramework.Model;
using ServerFramework.Tool;
using System;
using System.Net.Sockets;

namespace ServerFramework.Servers
{
    class Client
    {
        private UserData userdata;
        private UserTempData userTempData;
        private Server server;
        public MySqlConnection MySQLConn;
        private MessageHandle msg = new MessageHandle();

        //常量
        public const int BUFFER_SIZE = 1024;
        //Socket
        public Socket clientSocket;
        //是否使用
        public bool isUse = false;
        //Buff
        public byte[] readBuff = new byte[BUFFER_SIZE];
        public int buffCount = 0;
        //沾包分包
        public byte[] lenBytes = new byte[sizeof(UInt32)];
        public Int32 msgLength = 0;
        //心跳时间
        public long lastTickTime = long.MinValue;

        public Client() {
            readBuff = new byte[BUFFER_SIZE];
        }
        public void Init(Socket socket,Server server)
        {
            //数据初始化
            readBuff = new byte[BUFFER_SIZE];
            this.clientSocket = socket;
            this.server = server;
            isUse = true;
            buffCount = 0;
            //心跳处理
            lastTickTime = Sys.GetTimeStamp();
            MySQLConn = MySQLHelper.Instance.Connect();
            //开始接收数据
            Start();
        }
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Console.WriteLine("收到 [" + GetAdress() + "] 断开链接");
                    Close();
                    return;
                }
                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("收到 [" + GetAdress() + "] 断开链接 " + e.Message);
                Close();
            }
        }
        private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode, actionCode, data, this);
        }
        //剩余的Buff
        public int BuffRemain()
        {
            return BUFFER_SIZE - buffCount;
        }
        //获取客户端地址
        public string GetAdress()
        {
            if (!isUse)
                return "无法获取地址";
            return clientSocket.RemoteEndPoint.ToString();
        }

        public void SetUserData(UserData user)
        {
            this.userdata = user;
        }
      
        public void Send(ActionCode actionCode,ReasonCode reasonCode, string data)
        {
            try
            {
                byte[] bytes = MessageHandle.PackData(actionCode,reasonCode, data);
                clientSocket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }
        }
        //关闭
        public void Close()
        {
            if (!isUse)
                return;
            Console.WriteLine("[断开链接]" + GetAdress());
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            isUse = false;
        }
    }
}

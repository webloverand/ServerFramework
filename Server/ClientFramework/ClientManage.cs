using ClientFramework.Request;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientFramework
{
    class ClientManage
    {
        private const string IP = "192.168.31.68";
        private const int PORT = 6688;

        private Socket clientSocket;
        //消息处理器
        private MessageHandle msg = new MessageHandle();
        //连接服务器
        public void OnInit()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(IP, PORT);
                Console.WriteLine("连接到服务器");
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("无法连接到服务器端，请检查您的网络！！" + e);
            }
        }
        //开始接收
        private void Start()
        {
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCB, null);
        }
        //接收回调函数
        private void ReceiveCB(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(ar);

                msg.ReadMessage(count, OnProcessDataCallback);

                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ReceiveCB]:" + e.Message);
            }
        }
        //消息分发
        private void OnProcessDataCallback(ActionCode actionCode,ReasonCode reasonCode, string data)
        {
            RequestManager.Instance.HandleReponse(actionCode, reasonCode,data);
        }
        //像服务端发送消息
        public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
        {
            byte[] bytes = MessageHandle.PackData(requestCode, actionCode, data);
            
            clientSocket.Send(bytes);
        }
    }
}

using Protocol;
using ServerFramework.Controller;
using ServerFramework.Tool;
using System;
using System.Collections.Generic;
//需要导入的
using System.Net;
using System.Net.Sockets;

/// <summary>
/// 服务器监听脚本
/// </summary>
namespace ServerFramework.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        //连接的客户端列表
        private Client[] clientConn;
        //最大链接数
        public int maxConn = 50;
        //分发器
        private ControllerManager controllerManager;

        //主定时器
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        //心跳时间
        public long heartBeatTime = 180;

        public Server() { }
        public Server(string ipStr,int port)
        {
            controllerManager = new ControllerManager(this);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port); //设置IP与port
        }
        #region Listen方法解析
        /*      1.返回值：
        　　      成功返回0；
        　　      失败返回-1。
                2.参数：
          　　   sockfd：套接字，成功返回后进入监听模式，当有新连接并accept后会再建立一个套接字保存新的连接；
          　　   backlog：暂且翻译为后备连接吧！下面详细介绍此参数：

        　　     1）  当TCP接收一个连接后（三次握手通过）会将此连接存在连接请求队列里面，并对队列个数+1，而backlog为此队列允许的最大个数，超过此值，则直接将新的连接删除，即不在接收新的连接。将这些处于请求队列里面的连接暂记为后备连接，这些都在底层自动完成，底层将连接添加到队列后等待上层来处理（一般是调用accept函数接收连接）；
        　　     2）  当上层调用accept函数接收一个连接（处于请求队列里面的后备连接）,队列个数会-1；
        　　     3）  那么这样一个加一个减，只要底层提交的速度小于上层接收的速度(一般是这样)，很明显backlog就不能限制连接的个数，只能限制后备连接的个数。那为啥要用这个backlog呢？主要用于并发处理，当上层没来的及接收时，底层可以提交多个连接；
        　　     4）  backlog的取值范围 ,一般为0-5。
                
               3.那么，如何才能限制连接个数，而不是后备的连接个数呢？如下：
                  我们可以关闭处于监听状态的sock。假设我想限制3个连接，在应用层每当accept到一个连接时，定义一个变量var让其+1，当判断有三个连接时关闭sock。然后动态的检测当前的计数值var，当小于3时，再打开此sock，当然这样操作必须使能SO_REUSEPORT(允许重用本地地址)，可以通过调用setsockopt函数来使能，问题解决。
        */
        #endregion
        //开启服务器
        public void Start()
        {
            //定时器
            timer.Elapsed += new System.Timers.ElapsedEventHandler(HandleMainTimer);
            timer.AutoReset = false;
            timer.Enabled = true;
            //链接池
            clientConn = new Client[maxConn];
            for (int i = 0; i < maxConn; i++)
            {
                clientConn[i] = new Client();
            }

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint); //绑定IP与端口号
            serverSocket.Listen(maxConn);  
            serverSocket.BeginAccept(AcceptCB, null); //开始监听客户端的连接
            Console.WriteLine("[服务器]启动成功!");
        }
        private void AcceptCB(IAsyncResult ar)
        {
            try
            {
                Socket socket = serverSocket.EndAccept(ar);
                int index = NewIndex();

                if (index < 0)
                {
                    socket.Close();
                    Console.Write("[AcceptCB警告]链接已满");
                }
                else
                {
                    Client client = clientConn[index];
                    client.Init(socket,this);
                    Console.WriteLine("客户端连接 [" + client.GetAdress() + "] conn池ID：" + index);
                }
                serverSocket.BeginAccept(AcceptCB, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("AcceptCb失败:" + e.Message);
            }
        }
        //获取链接池索引，返回负数表示获取失败
        public int NewIndex()
        {
            if (clientConn == null)
                return -1;
            for (int i = 0; i < clientConn.Length; i++)
            {
                if (clientConn[i] == null)
                {
                    clientConn[i] = new Client();
                    return i;
                }
                else if (clientConn[i].isUse == false)
                {
                    return i;
                }
            }
            return -1;
        }
        //分发消息
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
        //向客户端发送消息
        public void SendResponse(Client client, ActionCode actionCode,ReasonCode reasonCode, string data)
        {
            Console.WriteLine("[SendResponse]回复客户端:" + data);
            client.Send(actionCode, reasonCode, data);
        }
        //主定时器
        public void HandleMainTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            //处理心跳
            HeartBeat();
            timer.Start();
        }
        //心跳
        public void HeartBeat()
        {
            long timeNow = Sys.GetTimeStamp();

            for (int i = 0; i < clientConn.Length; i++)
            {
                Client conn = clientConn[i];
                if (conn == null) continue;
                if (!conn.isUse) continue;

                if (conn.lastTickTime < timeNow - heartBeatTime)
                {
                    Console.WriteLine("[心跳引起断开连接]" + conn.GetAdress());
                    lock (conn)
                        conn.Close();
                }
            }
        }
    }
}

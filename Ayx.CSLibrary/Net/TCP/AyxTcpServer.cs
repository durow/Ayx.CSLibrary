/**************************************************************************************
 * 文件名：AyxTcpServer.cs
 * 主要功能：TCP通信中的服务端，使用方法简介：
            1、创建一个AyxTcpServer对象
            2、监听NewClientAccepted事件处理新接入的连接
            3、对新接入的AyxTcpClient连接监听MessageReceived事件
            4、通过StopServer停止服务
 * 作者：durow
 * 时间：2015.08.21
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ayx.CSLibrary.Net.TCP
{
    public class AyxTcpServer
    {
        #region 私有字段

        /// <summary>
        /// 用于监听连接的监听器
        /// </summary>
        private TcpListener _listener;

        /// <summary>
        /// 监听器线程
        /// </summary>
        private Thread _listeningThread;

        /// <summary>
        /// 接收消息线程
        /// </summary>
        private Thread _receivingThread;

        #endregion

        #region 公开属性

        /// <summary>
        /// 服务绑定的IP地址
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// 服务绑定的端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 是否正在监听
        /// </summary>
        public bool IsListening { get; private set; }

        /// <summary>
        /// 客户端列表
        /// </summary>
        public List<AyxTcpClient> ClientList { get; private set; }

        #endregion

        #region 委托和事件

        /// <summary>
        /// 接受新连接时触发
        /// </summary>
        public event EventHandler<TcpClientEventArgs> NewClientConnected;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建一个AyxTcpServer对象
        /// </summary>
        public AyxTcpServer()
        {
            IsListening = false;
            ClientList = new List<AyxTcpClient>();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 开始服务
        /// </summary>
        /// <param name="ip">绑定的IP</param>
        /// <param name="port">服务端口</param>
        public void StartServer(string ip, int port)
        {
            IP = ip;
            Port = port;

            StartListening();
            StartReceiving();
        }

        /// <summary>
        /// 停止服务，同时关闭所有已接受的连接和接收线程
        /// </summary>
        public void StopServer()
        {
            IsListening = false;
            _listener.Stop();
            if (_listeningThread != null)
                _listeningThread.Abort();
            foreach (var item in ClientList)
            {
                item.StopAndClose();
            }
            ClientList.Clear();
        }

        /// <summary>
        /// 根据Tag选择AyxTcpClient
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>返回选中的AyxTcpClient对象，没选中返回null</returns>
        public AyxTcpClient GetClientByTag(string tag)
        {
            try
            {
                var client = ClientList.SingleOrDefault(p => p.Tag.ToString() == tag);
                return client;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 停止接收并移除客户端连接列表
        /// </summary>
        /// <param name="client">要移除的客户端</param>
        public void StopAndRemoveClient(AyxTcpClient client)
        {
            if (client == null) return;
            client.StopAndClose();
            ClientList.Remove(client);
        }

        /// <summary>
        /// 移除连接
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(AyxTcpClient client)
        {
            ClientList.Remove(client);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 开始监听连接
        /// </summary>
        private void StartListening()
        {
            ClientList.Clear();
            var iep = new IPEndPoint(IPAddress.Parse(IP), Port);
            _listener = new TcpListener(iep);
            _listener.Start();
            IsListening = true;
            _listeningThread = new Thread(ListeningThreadFunction)
            {IsBackground = true};
            _listeningThread.Start();
        }

        /// <summary>
        /// 监听线程的方法
        /// </summary>
        private void ListeningThreadFunction()
        {
            while (IsListening)
            {
                var client = _listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(s =>
                {
                    var ayxClient = AyxTcpClient.FromTcpClient(client);
                    //处理加密连接
                    ayxClient.TcpMessageReceived += SecurityMessage;
                    ClientList.Add(ayxClient);
                });
            }
        }

        private void SecurityMessage(object sender, TcpMsgEventArgs e)
        {
            var client = e.AyxClient;
            client.SecurityMsgFunction(e.Message);
            if (e.Message.Command == "SetDES")
            {
                //加密连接结束后取消事件订阅
                client.TcpMessageReceived -= SecurityMessage;
                //加密连接完成后触发接收到新连接的事件
                if (NewClientConnected != null)
                    NewClientConnected(this, new TcpClientEventArgs(client));
            }
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        private void StartReceiving()
        {
            _receivingThread = new Thread(ReceivingThreadFunction)
            {IsBackground = true};
            _receivingThread.Start();
        }


        /// <summary>
        /// 接收线程的方法
        /// </summary>
        private void ReceivingThreadFunction()
        {
            while (IsListening)
            {
                Parallel.ForEach(ClientList, new Action<AyxTcpClient>(c =>
                {
                    if (c.IsBusy) return;
                    c.ReceiveAndDispatchMsg();
                }));
                Thread.Sleep(50);
            }
        }

        #endregion
    }
}
/**************************************************************************************
 * 文件名：AyxTcpClient.cs
 * 主要功能：TCP通信中的客户终端，使用方法简介：
            1、创建一个AyxTcpClient对象
            2、通过ConnectServer方法连接服务端
            3、监听MessageReceived事件处理收到的消息
            4、通过StartReceive方法开启一个新线程开始接收消息
            5、通过StopAndClose方法断开连接并停止接收
            其中FromTcpCliet主要用于Listener Accept新连接时创建一个AyxTcpClient对象
 * 作者：durow
 * 时间：2015.08.21
**************************************************************************************/

using Ayx.CSLibrary.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ayx.CSLibrary.Net.TCP
{
    public class AyxTcpClient
    {
        #region 私有字段

        /// <summary>
        /// 消息拆分器，防止出现粘包的问题
        /// </summary>
        private readonly TcpMsgSplitter _msgSplitter;

        /// <summary>
        /// 接收状态
        /// </summary>
        private bool _isReceiving = false;

        /// <summary>
        /// 接收消息的线程
        /// </summary>
        private Thread _receiveThread;

        /// <summary>
        /// RSA加密解密助手
        /// </summary>
        private RsaHelper rsa;

        /// <summary>
        /// DES加密解密助手
        /// </summary>
        private DesHelper des;

        /// <summary>
        /// 正在加密连接
        /// </summary>
        private bool isConnecting = false;

        /// <summary>
        /// 是否已开启加密
        /// </summary>
        private bool isInSecurity = false;

        /// <summary>
        /// 是否处于忙碌状态
        /// </summary>
        public bool IsBusy { get; private set; } = false;

        #endregion

        #region 公开属性

        /// <summary>
        /// TcpClient对象，用于处理TCP连接和收发数据
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// 标识该对象的一个Tag，可自由填充
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 接收数据循环等待时间
        /// </summary>
        public int SleepTime { get; set; } = 100;

        /// <summary>
        /// 连接是否已通过验证
        /// </summary>
        public bool IsValidated { get; set; } = false;

        /// <summary>
        /// 是否已经连接
        /// </summary>
        public bool IsConnected { get; private set; } = false;

        #endregion

        #region 事件和委托

        /// <summary>
        /// 收到TCP消息事件
        /// </summary>
        public event EventHandler<TcpMsgEventArgs> TcpMessageReceived;

        /// <summary>
        /// 连接成功事件
        /// </summary>
        public event EventHandler Connected;

        #endregion

        #region 构造函数和工厂方法

        /// <summary>
        /// 创建一个空的AyxTcpClient对象
        /// </summary>
        public AyxTcpClient()
        {
            _msgSplitter = new TcpMsgSplitter();
        }

        /// <summary>
        /// 从TcpClient创建一个AyxTcpClient对象
        /// </summary>
        /// <param name="client">TcpClient对象</param>
        /// <returns></returns>
        public static AyxTcpClient FromTcpClient(TcpClient client)
        {
            return new AyxTcpClient
            {
                Client = client
            };
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <param name="ip">服务端地址</param>
        /// <param name="port">端口</param>
        public void ConnectServer(string ip, int port, int timeOut = 10000)
        {
            isConnecting = true;

            //连接
            if (Client == null)
                Client = new TcpClient();
            Client.Connect(ip, port);

            //加密连接
            var timer = 0;
            var secMsg = new AyxTcpMessage("GetPK");
            SendMessage(secMsg);
            while (isConnecting)
            {
                if (timer > timeOut)
                {
                    throw new Exception("连接加密超时!");
                }
                var msgList = ReceiveMessage();
                foreach (var msg in msgList)
                {
                    SecurityMsgFunction(msg);
                }
                Thread.Sleep(SleepTime);
                timer += SleepTime;
            }
        }

        /// <summary>
        /// 异步连接服务端，连接结束后触发Connected事件
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        public void BeginConnectServer(string ip, int port)
        {
            ThreadPool.QueueUserWorkItem(s =>
            {
                ConnectServer(ip, port);
                if (Connected != null)
                    Connected(this, new EventArgs());
            });
        }

        public Task ConnectServerTask(string ip, int port)
        {
            var t = new Task(new Action(() => { ConnectServer(ip, port); }));
            return t;
        }

        /// <summary>
        /// 异步连接服务端
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        //public async void ConnectServerAsync(string ip, int port)
        //{
        //    var task = ConnectServerTask(ip, port);
        //    task.Start();
        //    await task;
        //}

        /// <summary>
        /// 开始接收数据，收到数据后触发TcpMessageReceived事件
        /// </summary>
        public void StartReceiveAsync(int sleepTime = 100)
        {
            if (!IsConnected)
            {
                throw new Exception("Not Connected!Please connect to the server first!");
            }
            SleepTime = sleepTime;
            _isReceiving = true;
            _receiveThread = new Thread(ReceiveThreadFunction)
            {IsBackground = true};
            _receiveThread.Start();
        }

        /// <summary>
        /// 停止并关闭连接
        /// </summary>
        public void StopAndClose()
        {
            if (_isReceiving)
            {
                _isReceiving = false;
            }
            Client.Close();
        }

        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="msg">字符串信息</param>
        public void SendMessage(string msg)
        {
            if (isInSecurity && des != null)
                msg = des.Encrypt(msg);
            var sendMsg = TcpMsgSplitter.PackMessage(msg);
            var sendData = Encoding.Default.GetBytes(sendMsg);
            SendMessage(sendData);
        }

        /// <summary>
        /// 发送AyxTcpMessage消息
        /// </summary>
        /// <param name="msg">AyxTcpMessage消息</param>
        public void SendMessage(AyxTcpMessage msg)
        {
            SendMessage(msg.ToString());
        }

        /// <summary>
        /// 发送Byte[]
        /// </summary>
        /// <param name="data">要发送的内容</param>
        public void SendMessage(byte[] data)
        {
            Client.Client.Send(data);
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessageAsync(AyxTcpMessage msg)
        {
            SendMessageAsync(msg.ToString());
        }

        /// <summary>
        /// 异步发送字符串
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessageAsync(string msg)
        {
            if (isInSecurity && des != null)
                msg = des.Encrypt(msg);
            var sendMsg = TcpMsgSplitter.PackMessage(msg);
            var sendData = Encoding.Default.GetBytes(sendMsg);
            SendMessageAsync(sendData);
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data"></param>
        public void SendMessageAsync(byte[] data)
        {
            ThreadPool.QueueUserWorkItem(s => { Client.Client.Send(data); });
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <returns>接收到的消息列表</returns>
        internal IEnumerable<AyxTcpMessage> ReceiveMessage()
        {
            if (IsBusy) yield break;
            IsBusy = true;

            var len = Client.Available;
            if (len <= 0)
            {
                IsBusy = false;
                yield break;
            }
            var buff = new byte[len];
            Client.Client.Receive(buff);
            var msg = Encoding.Default.GetString(buff);
            //拆解消息
            var msgList = _msgSplitter.Split(msg);
            foreach (var msgString in msgList)
            {
                var temp = msgString;
                //解密
                if (isInSecurity && des != null)
                    temp = des.Decrypt(msgString);
                //Parse消息对象
                var tcpMsg = AyxTcpMessage.Parse(temp);
                yield return tcpMsg;
            }
            IsBusy = false;
        }

        /// <summary>
        /// 接收消息并分发
        /// </summary>
        internal void ReceiveAndDispatchMsg()
        {
            var msgList = ReceiveMessage();
            if (TcpMessageReceived != null)
            {
                foreach (var msg in msgList)
                {
                    TcpMessageReceived(this, new TcpMsgEventArgs(this, msg));
                }
            }
        }

        #endregion

        #region 私有方法

        //接收线程函数
        private void ReceiveThreadFunction()
        {
            while (_isReceiving)
            {
                if (!Client.Connected)
                {
                    _isReceiving = false;
                    Client.Close();
                }
                ReceiveAndDispatchMsg();
                Thread.Sleep(SleepTime);
            }
        }

        /// <summary>
        /// 加密连接时的消息处理
        /// </summary>
        /// <param name="msg"></param>
        internal void SecurityMsgFunction(AyxTcpMessage msg)
        {
            switch (msg.Command)
            {
                case "GetPK":
                    GetPK_Function();
                    break;
                case "SetPK":
                    SetPK_Function(msg);
                    break;
                case "SetDES":
                    SetDES_Function(msg);
                    break;
                case "SecOK":
                    SecOK_Function();
                    break;
                default:
                    break;
            }
        }

        //GetPK命令
        private void GetPK_Function()
        {
            rsa = new RsaHelper();
            var pk = rsa.GetPkXmlString();
            var msg = new AyxTcpMessage("SetPK");
            msg.AddKeyValue("PK", pk);
            SendMessage(msg);
        }

        //SetPK命令
        private void SetPK_Function(AyxTcpMessage msg)
        {
            var pk = msg.GetKeyValue("PK");
            rsa = new RsaHelper();
            rsa.InitPkFromXmlString(pk);
            des = new DesHelper();
            var sendMsg = new AyxTcpMessage("SetDES");
            var key = rsa.Encrypt(des.Key);
            var iv = rsa.Encrypt(des.Iv);
            sendMsg.AddKeyValue("Key", key);
            sendMsg.AddKeyValue("IV", iv);
            SendMessage(sendMsg);
        }

        //SetDES命令
        private void SetDES_Function(AyxTcpMessage msg)
        {
            try
            {
                var key = msg.GetKeyValue("Key");
                key = rsa.Decrypt(key);
                var iv = msg.GetKeyValue("IV");
                iv = rsa.Decrypt(iv);
                des = new DesHelper(key, iv);
                var sendMsg = new AyxTcpMessage("SecOK");
                SendMessage(sendMsg);
                isInSecurity = true;
                IsConnected = true;
            }
            catch
            {
                throw;
            }
        }

        //SecOK命令
        private void SecOK_Function()
        {
            isInSecurity = true; //进入加密传输
            isConnecting = false; //连接状态结束
            IsConnected = true; //已连接
        }

        #endregion
    }
}
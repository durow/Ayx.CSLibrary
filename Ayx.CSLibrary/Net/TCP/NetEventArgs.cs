using System;
using System.Collections.Generic;

namespace Ayx.CSLibrary.Net.TCP
{

    /// <summary>
    /// 接收消息后触发事件的参数
    /// </summary>
    //public class TcpMsgEventArgs : EventArgs
    //{
    //    public string MessageString { get; private set; }

    //    public TcpMsgEventArgs(string msg)
    //    {
    //        MessageString = msg;
    //    }
    //}

    /// <summary>
    /// 接受新连接事件的参数
    /// </summary>
    public class TcpClientEventArgs : EventArgs
    {
        public AyxTcpClient AyxClient { get; private set; }

        public TcpClientEventArgs(AyxTcpClient ayxClient)
        {
            AyxClient = ayxClient;
        }
    }

    /// <summary>
    /// 接收到数据时事件的参数
    /// </summary>
    public class TcpMsgEventArgs : EventArgs
    {
        public AyxTcpClient AyxClient { get; private set; }
        public AyxTcpMessage Message { get; private set; }

        public TcpMsgEventArgs(AyxTcpClient ayxClient, AyxTcpMessage msg)
        {
            AyxClient = ayxClient;
            Message = msg;
        }
    }
}

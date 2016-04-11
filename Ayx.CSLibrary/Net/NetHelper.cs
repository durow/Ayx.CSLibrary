/**************************************************************************************
 * 文件名：NetHelper.cs
 * 主要功能：网络相关的一些辅助方法：
 * 作者：durow
 * 时间：2015.08.21
**************************************************************************************/

using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Ayx.CSLibrary.Net
{
    public static class NetHelper
    {
        /// <summary>
        /// 获取本地IPv4地址列表
        /// </summary>
        /// <returns>地址列表</returns>
        public static IEnumerable<string> GetLocalIpv4List()
        {
            var hostName = Dns.GetHostName();
            var ipList = Dns.GetHostAddresses(hostName);
            foreach (var ip in ipList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    yield return ip.ToString();
            }
        }
    }
}

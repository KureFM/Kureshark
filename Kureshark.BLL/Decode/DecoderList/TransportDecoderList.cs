using Kureshark.BLL.InsideDecoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.BLL.DecoderList
{
    /// <summary>
    /// 解码器列表只允许有1个实例
    /// 请使用Instance方法获取该类的实例
    /// </summary>
    public sealed class TransportDecoderList:DecoderList
    {
        private static TransportDecoderList _instance;

        private static object _lock = new object();

        public static TransportDecoderList Instance()
        {
            if (_instance == null)
                lock (_lock)
                {
                    return _instance = new TransportDecoderList();
                }
            return _instance;
        }

        private TransportDecoderList()
        {
            //内置解码器
            AddDecoder(new TcpDecoder());
            AddDecoder(new UdpDecoder());
        }
    }
}

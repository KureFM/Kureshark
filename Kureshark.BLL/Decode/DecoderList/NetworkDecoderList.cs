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
    public sealed class NetworkDecoderList:DecoderList
    {
        private static NetworkDecoderList _instance;

        private static object _lock = new object();

        public static NetworkDecoderList Instance()
        {
            if (_instance == null)
                lock (_lock)
                {
                    return _instance = new NetworkDecoderList();
                }
            return _instance;
        }

        private NetworkDecoderList()
        {
            //内置解码器
            AddDecoder(new ArpDecoder());
            AddDecoder(new IpV4Decoder());
        }
    }
}

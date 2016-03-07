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
    public class AppDecoderList : DecoderList
    {
        private static AppDecoderList _instance;

        private static object _lock = new object();

        public static AppDecoderList Instance()
        {
            if (_instance == null)
                lock (_lock)
                {
                    return _instance = new AppDecoderList();
                }
            return _instance;
        }

        public AppDecoderList()
        {
            AddDecoder(new DnsDecoder());
        }
    }
}

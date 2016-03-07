using Kureshark.Model;
using Kureshark.Model.DataLink;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.BLL.InsideDecoder;

namespace Kureshark.BLL.DecoderList
{
    /// <summary>
    /// 解码器列表只允许有1个实例
    /// 请使用Instance方法获取该类的实例
    /// </summary>
    public sealed class DataLinkDecoderList : DecoderList
    {
        private static DataLinkDecoderList _instance;

        private static object _lock = new object();

        public static DataLinkDecoderList Instance()
        {
            if (_instance == null)
                lock (_lock)
                {
                    return _instance = new DataLinkDecoderList();
                }
            return _instance;
        }

        private DataLinkDecoderList()
        {
            //内置解码器
            AddDecoder(new EthernetDecoder());
        }
    }
}

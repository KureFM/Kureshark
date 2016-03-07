using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.BLL
{
    /// <summary>
    /// 解码器类型
    /// </summary>
    public enum DecoderType
    {
        /// <summary>
        /// 对数据链路的帧进行解码
        /// 根据LinkType解码
        /// </summary>
        LinkType=1,

        /// <summary>
        /// 根据EtherType解码
        /// </summary>
        EtherType=2,

        /// <summary>
        /// 根据Protocol Number解码
        /// </summary>
        Protocol=3,

        /// <summary>
        /// 根据Port Number解码
        /// </summary>
        Port=4
    }
}

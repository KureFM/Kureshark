using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.DataLink
{
    /// <summary>
    /// 数据链路类型
    /// http://www.tcpdump.org/linktypes.html
    /// </summary>
    public enum LinkType:ushort
    {
        NULL=0,
        Ethernet_II=1,
        IEEE802_5=2,
    }
}

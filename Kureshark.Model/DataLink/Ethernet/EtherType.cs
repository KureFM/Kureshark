using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.DataLink
{
    /// <summary>
    /// <url>
    /// http://standards.ieee.org/develop/regauth/ethertype/eth.txt
    /// </url>
    /// </summary>
    [Flags]
    public enum EtherType : ushort
    {
        /// <summary>
        /// Internet Protocol version 4
        /// </summary>
        IpV4=0x0800,
        /// <summary>
        /// Address Resolution Protocol
        /// </summary>
        Arp=0x0806,
        /// <summary>
        /// Internet Protocol Version 6
        /// </summary>
        Rarp=0x86DD

    }
}

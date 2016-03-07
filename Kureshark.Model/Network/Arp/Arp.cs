using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.DataLink;
using Kureshark.Model.Extend;

namespace Kureshark.Model.Network
{
    /// <summary>
    /// Address Resolution Protocol
    /// RFC 826
    /// <url>
    /// https://www.ietf.org/rfc/rfc826.txt
    /// </url>
    /// <pre>
    /// +------+----------+----------+----------+----------+-----------+----------+----------+----------+----------+
    /// | byte |    2     |    2     |     1    |     1    |     2     |     6    |     4    |     6    |     4    |
    /// +------+----------+----------+----------+----------+-----------+----------+----------+----------+----------+
    /// |      |          |          | Hardware | Protocol |           |  Sender  |  Sender  |  Target  |  Target  |
    /// |      | Hardware | Protocol |  address |  address | Operation | hardware | protocol | hardware | protocol |
    /// |      |   type   |   type   |  length  |  length  |           |  address |  address |  address |  address |
    /// +------+----------+----------+----------+----------+-----------+----------+----------+----------+----------+
    /// </pre>
    /// </summary>
    public class Arp : FramePayload
    {
        public Arp(byte[] rawData)
        {
            _rawData = rawData;
        }

        public LinkType HardwareType
        {
            get
            {
                return (LinkType)BitConverter.ToUInt16(_rawData.Sub(0, 2).Reverse(),0);
            }
        }

        public EtherType ProtocolType
        {
            get
            {
                return (EtherType)BitConverter.ToUInt16(_rawData.Sub(2, 2).Reverse(), 0);
            }
        }

        public ushort HardwareLength
        {
            get
            {
                return Convert.ToUInt16(_rawData[4]);
            }
        }

        public ushort ProtocolLength
        {
            get
            {
                return Convert.ToUInt16(_rawData[5]);
            }
        }

        public ArpOperation Operation
        {
            get
            {
                return (ArpOperation)BitConverter.ToUInt16(_rawData.Sub(6, 2).Reverse(), 0);
            }
        }

        public MacAddress SenderMac
        {
            get
            {
                return new MacAddress(_rawData.Sub(8, 6));
            }
        }

        public IpV4Address SenderIp
        {
            get
            {
                return new IpV4Address(_rawData.Sub(14, 4));
            }
        }

        public MacAddress TargetMac
        {
            get
            {
                return new MacAddress(_rawData.Sub(18, 6));
            }
        }

        public IpV4Address TargetIp
        {
            get
            {
                return new IpV4Address(_rawData.Sub(24, 4));
            }
        }

    }
}

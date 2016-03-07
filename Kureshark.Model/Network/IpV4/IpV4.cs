using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.DataLink;
using Kureshark.Model.Extend;
using Kureshark.Model.Network;

namespace Kureshark.Model.Network
{
    /// <summary>
    /// Internet Protocol version 4
    /// RFC 760
    /// <url>
    /// https://www.ietf.org/rfc/rfc760.txt
    /// </url>
    /// <pre>
    ///  0                   1                   2                   3
    ///  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  0
    /// |Version|  IHL  |Type of Service|          Total Length         |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  4
    /// |         Identification        |Flags|      Fragment Offset    |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  8
    /// |  Time to Live |    Protocol   |         Header Checksum       |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  12
    /// |                       Source Address                          |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  16
    /// |                    Destination Address                        |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  20
    /// |                    Options                    |    Padding    |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// </pre>
    /// </summary>
    public class Ipv4 : FramePayload
    {
        public Ipv4(byte[] rawData)
        {
            if (rawData.Length < 20)
            {
                throw new ArgumentException("IPv4报文长度至少为20byte");
            }
            _rawData = rawData;
        }

        /// <summary>
        /// Version: 4 bits(1/2byte)
        /// </summary>
        public ushort Version
        {
            get
            {
                //不要直接修改源数据
                byte temp = _rawData[0];
                //第一个byte的高4位
                return Convert.ToUInt16(temp >> 4);
            }
        }

        /// <summary>
        /// Internet Header Length: 4bit
        /// How many 4 bytes(32bits)
        /// </summary>
        public ushort IHL
        {
            get
            {
                byte temp = _rawData[0];
                //第一个byte的低4位           & 00001111
                return Convert.ToUInt16(temp & 15);
            }
        }

        /// <summary>
        /// Differentiated Services Code Point: 8bit
        /// Originally defined as the Type of service (ToS) field. This field is now defined by RFC 2474 for Differentiated services (DiffServ)
        /// <rfc>
        /// RFC 791
        /// RFC 2474
        /// RFC 3168
        /// </rfc>
        /// <url>
        /// https://www.ietf.org/rfc/rfc791.txt
        /// https://www.ietf.org/rfc/rfc2474.txt
        /// https://www.ietf.org/rfc/rfc3168.txt
        /// </url>
        /// <rfc791>
        /// Type of Service:  8 bits
        ///    0     1     2     3     4     5     6     7
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// |                 |     |     |     |     |     |
        /// |   PRECEDENCE    |  D  |  T  |  R  |  0  |  0  |
        /// |                 |     |     |     |     |     |
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// Precedence
        ///   111 - Network Control
        ///   110 - Internetwork Control
        ///   101 - CRITIC/ECP
        ///   100 - Flash Override
        ///   011 - Flash
        ///   010 - Immediate
        ///   001 - Priority
        ///   000 - Routine
        /// The use of the Delay, Throughput, and Reliability indications may increase the cost (in some sense) of the service.
        /// </rfc791>
        /// <rfc2474>
        /// A replacement header field, called the DS field, is defined, which is intended to supersede the existing definitions of the IPv4 TOS octet [RFC791] and the IPv6 Traffic Class octet [IPv6].
        /// The DS field structure is presented below:
        ///   0   1   2   3   4   5   6   7
        /// +---+---+---+---+---+---+---+---+
        /// |         DSCP          |  CU   |
        /// +---+---+---+---+---+---+---+---+
        /// DSCP: differentiated services codepoint
        /// CU:   currently unused
        /// </rfc2474>
        /// <rfc3168>
        ///    0     1     2     3     4     5     6     7
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// |          DS FIELD, DSCP           | ECN FIELD |
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// DSCP: differentiated services codepoint
        /// ECN:  Explicit Congestion Notification
        /// </rfc3168>
        /// 参考上述描述，将此字段命名为DiffServ
        /// </summary>
        public IpV4DiffServ DiffServ
        {
            get
            {
                return new IpV4DiffServ(_rawData[1]);
            }
        }

        public new ushort Length
        {
            get
            {
                byte[] temp = _rawData.Sub(2, 2);
                //反转序列
                Array.Reverse(temp);
                return BitConverter.ToUInt16(temp, 0);
            }
        }

        /// <summary>
        /// Identification: 16bits
        /// 标识符
        /// </summary>
        public byte[] Identification
        {
            get
            {
                return _rawData.Sub(4, 2);
            }
        }

        /// <summary>
        /// Flags:  3 bits
        /// Various Control Flags.
        ///   Bit 0: reserved, must be zero
        ///   Bit 1: Don't Fragment This Datagram (DF).
        ///   Bit 2: More Fragments Flag (MF).
        /// </summary>
        public IpV4Flags Flags
        {
            get
            {
                byte temp = _rawData[6];
                //8位取高3位，故右移5位
                temp >>= 5;
                return new IpV4Flags(temp);
            }
        }

        /// <summary>
        /// Fragment Offset:  13 bits
        /// This field indicates where in the datagram this fragment belongs.
        /// The fragment offset is measured in units of 8 octets (64 bits).
        /// The first fragment has offset zero.
        /// </summary>
        public ushort FragmentOffset
        {
            get
            {
                //取出第7，8个byte的低13位
                byte[] temp = _rawData.Sub(6, 2);
                //         0b00011111
                temp[0] &= 31;
                return BitConverter.ToUInt16(temp, 0);
            }
        }

        /// <summary>
        /// Time to Live:  8 bits
        /// </summary>
        public ushort TTL
        {
            get
            {
                return Convert.ToUInt16(_rawData[8]);
            }
        }

        /// <summary>
        /// Protocol:  8 bits
        /// This field defines the protocol used in the data portion of the IP datagram. The Internet Assigned Numbers Authority maintains a list of IP protocol numbers which was originally defined in RFC 790.
        /// </summary>
        public IpV4ProtocolField Protocol
        {
            get
            {
                return (IpV4ProtocolField)_rawData[9];
            }
        }
 
        /// <summary>
        /// Header Checksum:  16 bits
        /// </summary>
        public byte[] HeaderChecksum
        {
            get
            {
                return _rawData.Sub(10, 2);
            }
        }

        public IpV4Address SourceIp
        {
            get
            {
                return new IpV4Address(_rawData.Sub(12, 4));
            }
        }

        public IpV4Address DestinationIp
        {
            get
            {
                return new IpV4Address(_rawData.Sub(16, 4));
            }
        }

        /// <summary>
        /// 如果数据字节长度小于20则Payload返回NULL
        /// </summary>
        public PacketPayload Payload
        {
            get
            {
                int offset = IHL * 4;

                if (Length > offset)
                {
                    return new PacketPayload(_rawData.Sub(offset, Length - offset));
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

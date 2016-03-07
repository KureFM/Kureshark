using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.Extend;
using Kureshark.Model.Network;

namespace Kureshark.Model.Transport
{
    /// <summary>
    /// Transmission Control Protocol
    /// RFC 675 – Specification of Internet Transmission Control Program, December 1974 Version
    /// RFC 793 – TCP v4
    /// RFC 1122 – includes some error corrections for TCP
    /// RFC 1323 – TCP Extensions for High Performance [Obsoleted by RFC 7323]
    /// RFC 1379 – Extending TCP for Transactions -- Concepts [Obsoleted by RFC 6247]
    /// RFC 1948 – Defending Against Sequence Number Attacks
    /// RFC 2018 – TCP Selective Acknowledgment Options
    /// RFC 5681 – TCP Congestion Control
    /// RFC 6247 - Moving the Undeployed TCP Extensions RFC 1072, RFC 1106, RFC 1110, RFC 1145, RFC 1146, RFC 1379, RFC 1644, and RFC 1693 to Historic Status
    /// RFC 6298 – Computing TCP's Retransmission Timer
    /// RFC 6824 - TCP Extensions for Multipath Operation with Multiple Addresses
    /// RFC 7323 - TCP Extensions for High Performance
    /// RFC 7414 – A Roadmap for TCP Specification Documents
    /// <pre>
    /// TCP Header Format
    ///  0                   1                   2                   3
    ///  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  0
    /// |          Source Port          |       Destination Port        |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  4
    /// |                        Sequence Number                        |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  8
    /// |                    Acknowledgment Number                      |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  12
    /// |  Data | Rese|N|C|E|U|A|P|R|S|F|                               |
    /// | Offset| rved|S|W|C|R|C|S|S|Y|I|            Window             |
    /// |       |     | |R|E|G|K|H|T|N|N|                               |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  16
    /// |           Checksum            |         Urgent Pointer        |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  20
    /// |                    Options                    |    Padding    |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |                             data                              |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// </pre>
    /// </summary>
    public class Tcp : PacketPayload
    {
        public Tcp()
        {

        }

        public Tcp(byte[] rawData)
        {
            if (rawData.Length < 20)
            {
                throw new ArgumentException("");
            }
            _rawData = rawData;

        }

        /// <summary>
        /// Source Port: 16 bits
        /// </summary>
        public ushort SourcePort
        {
            get
            {
                byte[] temp = _rawData.Sub(0, 2);
                Array.Reverse(temp);
                return BitConverter.ToUInt16(temp, 0);
            }
        }

        /// <summary>
        /// Destination Port: 16 bits
        /// </summary>
        public ushort DestinationPort
        {
            get
            {
                byte[] temp = _rawData.Sub(2, 2);
                Array.Reverse(temp);
                return BitConverter.ToUInt16(temp, 0);
            }
        }

        public uint SeqNum
        {
            get
            {
                byte[] temp = _rawData.Sub(4, 4);
                Array.Reverse(temp);
                return BitConverter.ToUInt32(temp, 0);
            }
        }

        public uint AckNum
        {
            get
            {
                byte[] temp = _rawData.Sub(8, 4);
                Array.Reverse(temp);
                return BitConverter.ToUInt32(temp, 0);
            }
        }

        /// <summary>
        /// Data Offset: 4 bits(1/2byte)
        /// How many 4 bytes(32bits)
        /// </summary>
        public ushort DataOffest
        {
            get
            {
                byte temp = _rawData[12];
                temp >>= 4;
                return Convert.ToUInt16(temp);
            }
        }

        /// <summary>
        /// Flags: 9 bits(9/16bytes)
        /// </summary>
        public TcpFlags Flags
        {
            get
            {
                return new TcpFlags(_rawData.Sub(12, 2).Reverse());
            }
        }

        public ushort WindowSize
        {
            get
            {
                byte[] temp = _rawData.Sub(14, 2);
                Array.Reverse(temp);
                return BitConverter.ToUInt16(temp, 0);
            }
        }

        public byte[] Checksum
        {
            get
            {
                return _rawData.Sub(16, 2);
            }
        }

        public byte[] UrgentPointer
        {
            get
            {
                return _rawData.Sub(18, 2);
            }
        }

        public DatagramPayload Payload
        {
            get
            {
                int offset = DataOffest * 4;

                if (Length > offset)
                {
                    return new DatagramPayload(_rawData.Sub(offset, Length - offset));
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

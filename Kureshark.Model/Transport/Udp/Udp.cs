using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.Network;
using Kureshark.Model.Extend;

namespace Kureshark.Model.Transport
{
    /// <summary>
    /// User Datagram Protocol
    /// RFC 768 – User Datagram Protocol
    /// RFC 2460 – Internet Protocol, Version 6 (IPv6) Specification
    /// RFC 2675 – IPv6 Jumbograms
    /// RFC 4113 – Management Information Base for the UDPRFC 5405 – Unicast UDP Usage Guidelines for Application Designers
    /// <pre>
    ///  0      7 8     15 16    23 24    31
    /// +--------+--------+--------+--------+
    /// |     Source      |   Destination   |
    /// |      Port       |      Port       |
    /// +--------+--------+--------+--------+
    /// |                 |                 |
    /// |     Length      |    Checksum     |
    /// +--------+--------+--------+--------+
    /// |
    /// |          data octets ...
    /// +---------------- ...
    /// </pre>
    /// </summary>
    public class Udp : PacketPayload
    {
        public Udp()
        {

        }

        public Udp(byte[] rawData)
        {
            if (rawData.Length < 8)
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

        public new ushort Length
        {
            get
            {
                byte[] temp = _rawData.Sub(4, 2);
                //反转序列
                Array.Reverse(temp);
                return BitConverter.ToUInt16(temp, 0);
            }
        }

        public byte[] Checksum
        {
            get
            {
                return _rawData.Sub(6, 2);
            }
        }

        public DatagramPayload Payload
        {
            get
            {
                if (_rawData.Length > 8)
                {
                    return new DatagramPayload(_rawData.Sub(8, Length - 8));
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.Extend;

namespace Kureshark.Model.DataLink
{
    /// <summary>
    /// Ethernet_II
    /// RFC 894
    /// <url>
    /// https://www.ietf.org/rfc/rfc894.txt
    /// </url>
    /// <pre>
    /// +------+-------------+-------------+-----------+-----------------------------+-----+
    /// | byte |      6      |      6      |     2     |           46~1500           |  4  |
    /// +------+-------------+-------------+-----------+-----------------------------+-----+
    /// |      | Destination |   Source    |           |                             |     |
    /// |      |     MAC     |     MAC     | EtherType |           Payload           | CRC |
    /// |      |   Address   |   Address   |           |                             |     |
    /// +------+-------------+-------------+-----------+-----------------------------+-----+
    /// </pre>
    /// </summary>
    public class Ethernet : Frame
    {
        public Ethernet(byte[] rawData)
        {
            //为了保证每一部分都有数值，RAW的长度至少为15
            if (rawData.Length < 15)
            {
                throw new ArgumentException("数组的长度至少为15才足以构建Ethernet帧，实际长度为" + rawData.Length);
            }
            _rawData = rawData;
        }
        public Ethernet(Frame frame)
        {
            _rawData = frame.RawData;
        }

        public MacAddress Destination
        {
            get
            {
                return new MacAddress(_rawData.Sub(0, 6));
            }
        }

        public MacAddress Source
        {
            get
            {
                return new MacAddress(_rawData.Sub(6, 6));
            }
        }

        public EtherType Type
        {
            get
            {
                byte[] temp = _rawData.Sub(12, 2);
                Array.Reverse(temp);
                return (EtherType)BitConverter.ToUInt16(temp,0);
            }
        }

        public FramePayload Payload
        {
            get
            {
                return new FramePayload(_rawData.Sub(14, Length - 14));
            }
        }
    }
}

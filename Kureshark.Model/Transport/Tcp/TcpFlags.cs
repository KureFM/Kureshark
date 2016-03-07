using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.Extend;

namespace Kureshark.Model.Transport
{
    /// <summary>
    /// 根据RFC 793，标志位为6位，分别为URG、ACK、PSH、RST、SYN、FIN
    /// 在RFC 3168中增加了2位，分别为CWR、ECE
    /// 在RFC 3540中增加了1位，为NS
    /// </summary>
    public sealed class TcpFlags
    {
        private byte[] _flagsValue;

        //只用到低9位，只接受2个byte
        public TcpFlags(byte[] flagsValue)
        {
            if (flagsValue.Length != 2)
            {
                throw new ArgumentException(String.Format("Flags需要2个byte，实际上收到{0}个", flagsValue.Length));
            }
            ////                 & 0b11111110
            //if ((flagsValue[1] & 254) != 0)
            //{
            //    throw new ArgumentException("Flags只用到低9位，构造时请确保最高7位的值为0");
            //}
            flagsValue[1] &= 1;
            _flagsValue = flagsValue;
        }

        /// <summary>
        /// Nonce Sum
        /// ECN-nonce concealment protection (experimental: see RFC 3540[http://tools.ietf.org/html/rfc3540]).
        /// </summary>
        public bool NS
        {
            get
            {
                return Convert.ToBoolean(_flagsValue[1]);
            }
        }

        /// <summary>
        /// Congestion Window Reduced (CWR) flag is set by the sending host to indicate that it received a TCP segment with the ECE flag set and had responded in congestion control mechanism (added to header by RFC 3168[http://tools.ietf.org/html/rfc3168]).
        /// </summary>
        public bool CWR
        {
            get
            {
                return GetBit(_flagsValue[0], 8);
            }
        }
        
        /// <summary>
        /// ECN-Echo has a dual role, depending on the value of the SYN flag. It indicates:
        /// If the SYN flag is set (1), that the TCP peer is ECN capable.
        /// If the SYN flag is clear (0), that a packet with Congestion Experienced flag in IP header set is received during normal transmission (added to header by RFC 3168[http://tools.ietf.org/html/rfc3168]).
        /// </summary>
        public bool ECE
        {
            get
            {
                return GetBit(_flagsValue[0], 7);
            }
        }

        /// <summary>
        /// indicates that the Urgent pointer field is significant
        /// </summary>
        public bool URG
        {
            get
            {
                return GetBit(_flagsValue[0], 6);
            }
        }

        /// <summary>
        /// indicates that the Acknowledgment field is significant. All packets after the initial SYN packet sent by the client should have this flag set.
        /// </summary>
        public bool ACK
        {
            get
            {
                return GetBit(_flagsValue[0], 5);
            }
        }

        /// <summary>
        /// Push function. Asks to push the buffered data to the receiving application.
        /// </summary>
        public bool PSH
        {
            get
            {
                return GetBit(_flagsValue[0], 4);
            }
        }

        /// <summary>
        /// Reset the connection
        /// </summary>
        public bool RST
        {
            get
            {
                return GetBit(_flagsValue[0], 3);
            }
        }

        /// <summary>
        /// Synchronize sequence numbers. Only the first packet sent from each end should have this flag set. Some other flags and fields change meaning based on this flag, and some are only valid for when it is set, and others when it is clear.
        /// </summary>
        public bool SYN
        {
            get
            {
                return GetBit(_flagsValue[0], 2);
            }
        }

        /// <summary>
        /// No more data from sender
        /// </summary>
        public bool FIN
        {
            get
            {
                return GetBit(_flagsValue[0], 1);
            }
        }

        public ushort GetValue()
        {
            return BitConverter.ToUInt16(_flagsValue,0);
        }

        /// <summary>
        /// 取出byte中某一位(位数按低到高的顺序从1开始计)的值(bit)
        /// </summary>
        /// <param name="aByte">要取出值得byte</param>
        /// <param name="bitNum">要取出byte值的位数(位数按低到高的顺序从1开始计)</param>
        /// <returns>因为每个bit取值只有0或1，故返回值为bool</returns>
        private bool GetBit(byte aByte, int bitNum)
        {
            if (bitNum>8)
            {
                throw new ArgumentException("bitNum must be < 8.");
            }
            aByte >>= (bitNum - 1);
            aByte &= 1;
            return Convert.ToBoolean(aByte);
        }
    }
}

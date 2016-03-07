using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Network
{
    public sealed class IpV4Flags
    {
        private byte _flagsValue;

        //一个byte为8位，Flags只使用最低3位，构造时请确保最高5位的值为0
        public IpV4Flags(byte flagsValue)
        {
            //              & 0b11111000
            if ((flagsValue & 248) != 0)
            {
                throw new ArgumentException("Flags只使用最低3位，构造时请确保最高5位的值为0");
            }
            //              & 0b00000100
            if ((flagsValue & 4) != 0)
            {
                throw new ArgumentException("根据RFC760的定义，Flags的最高位必须为0（3位中的最高位）");
            }
            _flagsValue = flagsValue;
        }

        /// <summary>
        /// Don't Fragment (DF)
        /// 禁止分片
        /// </summary>
        public bool DF
        {
            get
            {
                return Convert.ToBoolean(_flagsValue >> 1);
            }
        }

        /// <summary>
        /// More Fragments ()
        /// 更多分片
        /// </summary>
        public bool MF
        {
            get
            {
                //                                   & 00000001
                return Convert.ToBoolean(_flagsValue & 1);
            }
        }

        public byte Value
        {
            get
            {
                return _flagsValue;
            }
        }
    }
}

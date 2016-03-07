using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.Extend;

namespace Kureshark.Model.Network
{
    public class IpV4Address : IEquatable<IpV4Address>
    {
        private byte[] _ipAddress;

        public IpV4Address(byte[] ipAddress)
        {
            if (ipAddress.Length == 4)
            {
                _ipAddress = ipAddress;
            }
            else
            {
                throw new ArgumentException("参数错误，参数byte的长度应为4，实际为" + ipAddress.Length);
            }
        }

        /// <summary>
        /// 解析点以分十进制法标记的ip字符串
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static IpV4Address Parse(string ipAddress)
        {
            if (String.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentNullException("参数为空，无法解析");
            }
            string[] hexs = ipAddress.Split(':');
            if (hexs.Length != 6)
            {
                throw new ArgumentException("解析应得到4组参数，实际得到" + hexs.Length + "组");
            }
            byte[] value = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                value[i] = Convert.ToByte(hexs[i], 10);
            }
            return new IpV4Address(value);
        }

        public byte[] ByteAddress
        {
            get
            {
                return this._ipAddress;
            }
        }

        /// <summary>
        /// 判断2个IpAddress对象是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IpV4Address other)
        {
            bool isEqual = true;

            for (int i = 0; i < 4; i++)
            {
                if (this.ByteAddress[i] !=
                    other.ByteAddress[i])
                {
                    isEqual = false;
                    break;
                }
            }

            return isEqual;
        }

        public override bool Equals(object obj)
        {
            return (obj is IpV4Address &&
                Equals((IpV4Address)obj));
        }
        

        #region 重载运算符

        #endregion

        public override string ToString()
        {
            return _ipAddress.ToString("D", ".");
        }

        public override int GetHashCode()
        {
            return _ipAddress.GetHashCode();
        }
    }
}

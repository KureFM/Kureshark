using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.Extend;

namespace Kureshark.Model.DataLink
{
    public sealed class MacAddress : IEquatable<MacAddress>
    {
        private byte[] _macAddress;

        public MacAddress(byte[] macAddress)
        {
            if (macAddress.Length == 6)
            {
                _macAddress = macAddress;
            }
            else
            {
                throw new ArgumentException("参数错误，参数byte的长度应为6，实际为" + macAddress.Length);
            }
        }

        /// <summary>
        /// 将string的地址解析为byte
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public static MacAddress Parse(string macAddress)
        {
            if (String.IsNullOrWhiteSpace(macAddress))
            {
                throw new ArgumentNullException("参数为空，无法解析");
            }
            string[] hexs = macAddress.Split(':');
            if (hexs.Length != 6)
            {
                throw new ArgumentException("解析应得到6组参数，实际得到" + hexs.Length + "组");
            }
            byte[] value = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                value[i] = Convert.ToByte(hexs[i], 16);
            }
            return new MacAddress(value);
        }

        /// <summary>
        /// 获取用byte表示的MAC地址
        /// </summary>
        public byte[] ByteAddress
        {
            get
            {
                return this._macAddress;
            }
        }

        /// <summary>
        /// 判断2个MacAddress对象是否相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MacAddress other)
        {
            bool isEqual = true;

            for (int i = 0; i < 6; i++)
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
            return (obj is MacAddress &&
                Equals((MacAddress)obj));
        }

        #region 重载运算符

        public static bool operator ==(MacAddress mac1, MacAddress mac2)
        {
            return mac1.Equals(mac2);
        }

        public static bool operator !=(MacAddress mac1, MacAddress mac2)
        {
            return !(mac1.Equals(mac2));
        }
        #endregion

        /// <summary>
        /// 重载ToString函数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _macAddress.ToString("X2", ":");
        }

        public override int GetHashCode()
        {
            return _macAddress.GetHashCode();
        }
    }
}

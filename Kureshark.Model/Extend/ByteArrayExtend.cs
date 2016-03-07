using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Extend
{
    //为byte[]提供扩展方法
    public static class ByteArrayExtend
    {
        /// <summary>
        /// 从byte[]中截取
        /// </summary>
        /// <param name="byteArr"></param>
        /// <param name="start">开始截取的位置，从0开始</param>
        /// <param name="length">要截取的长度</param>
        /// <returns></returns>
        public static byte[] Sub(this byte[] byteArr, int start, int length)
        {
            if (byteArr.Length == 0)
            {
                throw new ArgumentNullException("数组为NULL");
            }
            if (length < 1)
            {
                throw new ArgumentException("截取的长度小于1");
            }
            if (start > byteArr.Length - 1)
            {
                throw new ArgumentOutOfRangeException("开始的位置超出数组长度");
            }
            if((byteArr.Length<length)||
                (byteArr.Length - start < length))
            {
                throw new ArgumentException("截取的长度超出数组范围");
            }
            byte[] temp = new byte[length];
            Buffer.BlockCopy(byteArr, start, temp, 0, length);
            return temp;
        }

        /// <summary>
        /// 为byte[]扩展整体ToString方法
        /// </summary>
        /// <param name="byteArr"></param>
        /// <param name="format">用于格式化的字符串</param>
        /// <returns></returns>
        public static string ToString(this byte[] byteArr, string format)
        {
            StringBuilder temp = new StringBuilder();
            foreach (var aByte in byteArr)
            {
                temp.Append(aByte.ToString(format));
            }
            return temp.ToString();
        }

        /// <summary>
        /// 为byte[]扩展整体ToString方法
        /// </summary>
        /// <param name="byteArr"></param>
        /// <param name="format">用于格式化的字符串</param>
        /// <param name="join">用于连接每个byte的字符串</param>
        /// <returns></returns>
        public static string ToString(this byte[] byteArr, string format,string join)
        {
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < byteArr.Length; i++)
            {
                temp.Append(byteArr[i].ToString(format));
                if (i<byteArr.Length-1)
                {
                    temp.Append(join);
                }
            }

            return temp.ToString();
        }

        /// <summary>
        /// 反转byte[]
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>
        public static byte[] Reverse(this byte[] byteArr)
        {
            byte[] temp = byteArr;
            Array.Reverse(temp);
            return temp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model
{
    public class Payload
    {
        #region 私有字段

        protected byte[] _rawData;

        #endregion

        #region 构造函数

        public Payload()
        {

        }

        public Payload(byte[] rawData)
        {
            _rawData = rawData;
        }

        #endregion

        #region 属性

        public int Length
        {
            get
            {
                return _rawData.Length;
            }
        }

        public byte[] RawData
        {
            get
            {
                return _rawData;
            }
        }

        #endregion
    }
}

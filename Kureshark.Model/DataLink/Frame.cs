using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.DataLink
{
    public class Frame
    {
        protected byte[] _rawData;
        public DateTime Timestamp { get; private set; }

        public int Index { get; private set; }

        public Frame()
        {

        }


        public Frame(byte[] rawData, int index, DateTime timestamp)
        {
            _rawData = rawData;
            Index = index;
            Timestamp = timestamp;
        }

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
    }
}

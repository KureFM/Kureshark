using Kureshark.Model.DataLink;
using System;
using System.Collections.Generic;

namespace Kureshark.Model
{
    //解析好的Frame
    public class DecodedFrame
    {
        public DateTime Timestamp { get; private set; }

        public int Index { get; private set; }

        public int Length { get; private set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public string Protocol { get; set; }

        public List<TreeNode> TreeDisplay { get; set; }

        public byte[] RawDate { get;private set; }

        public DecodedFrame(Frame frame)
        {
            Index = frame.Index;
            Timestamp = frame.Timestamp;
            Length = frame.Length;
            RawDate=frame.RawData;
        }
    }
}

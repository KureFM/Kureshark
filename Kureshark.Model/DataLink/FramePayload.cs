using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.DataLink
{
    public class FramePayload : Payload
    {
        public FramePayload(byte[] rawData)
            : base(rawData)
        {

        }

        public FramePayload()
        {

        }
    }
}

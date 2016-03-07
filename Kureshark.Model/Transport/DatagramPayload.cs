using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Transport
{
    public class DatagramPayload : Payload
    {
        public DatagramPayload(byte[] rawData)
            : base(rawData)
        {

        }

        public DatagramPayload()
        {

        }
    }
}

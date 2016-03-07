using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Network
{
    public class PacketPayload : Payload
    {
        public PacketPayload(byte[] rawData)
            : base(rawData)
        {

        }

        public PacketPayload()
        {

        }
    }
}

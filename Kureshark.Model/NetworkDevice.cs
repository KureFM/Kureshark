using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model
{
    public sealed class NetworkDevice
    {
        public string Name { set; get; }

        public string Guid { set; get; }

        public string Description { set; get; }

        public string Ip { set; get; }

        public NetworkDevice()
        {

        }

        public override string ToString()
        {
            return String.Format(Name);
        }
    }
}

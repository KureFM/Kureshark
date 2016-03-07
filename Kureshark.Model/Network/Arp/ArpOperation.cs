using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Network
{
    [Flags]
    public enum ArpOperation : ushort
    {
        Request = 1,
        Reply = 2
    }
}

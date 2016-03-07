using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Network
{
    public enum IpV4Ecn:byte
    {
        /// <summary>
        /// Non ECN-Capable Transport
        /// </summary>
        Non_ECT=0,
        /// <summary>
        /// ECN Capable Transport
        /// </summary>
        ECT0=1,
        /// <summary>
        /// ECN Capable Transport
        /// </summary>
        ECT1=2,
        /// <summary>
        /// Congestion Encountered
        /// </summary>
        CE=3,
    }
}

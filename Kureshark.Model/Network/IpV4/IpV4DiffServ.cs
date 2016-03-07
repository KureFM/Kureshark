using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Network
{
    /// <summary>
    /// +-----+---------------------+-------+
    /// | bit |          6          |   2   |
    /// +-----+---------------------+-------+
    /// |     |         DSCP        |  ECN  |
    /// +-----+---------------------+-------+
    /// </summary>
    public sealed class IpV4DiffServ
    {
        private byte _value;

        public IpV4DiffServ(byte value)
        {
            _value = value;
        }

        public IpV4Dscp DSCP
        {
            get
            {
                byte temp = _value;
                _value >>= 2;
                return (IpV4Dscp)_value;
            }
        }

        public IpV4Ecn ECN
        {
            get
            {
                byte temp = _value;
                //     &  0x00000011
                _value &= 0x03;
                return (IpV4Ecn)temp;
            }
        }

        public byte GetValue()
        {
            return _value;
        }

    }
}

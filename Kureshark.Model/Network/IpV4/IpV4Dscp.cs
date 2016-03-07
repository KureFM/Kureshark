using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.Model.Network
{
    [Flags]
    public enum IpV4Dscp : byte
    {
        /// <summary>
        /// Default
        /// </summary>
        Default=0x00,
        /// <summary>
        /// Expedited Forwarding
        /// </summary>
        EF=0x2e,
        #region Class Selector
        //CSn=n*8
        /// <summary>
        /// Class Selector 1
        /// </summary>
        CS1=8,
        /// <summary>
        /// Class Selector 2
        /// </summary>
        CS2=16,
        /// <summary>
        /// Class Selector 3
        /// </summary>
        CS3=24,
        /// <summary>
        /// Class Selector 4
        /// </summary>
        CS4=32,
        /// <summary>
        /// Class Selector 5
        /// </summary>
        CS5=40,
        /// <summary>
        /// Class Selector 6
        /// </summary>
        CS6=48,
        /// <summary>
        /// Class Selector 7
        /// </summary>
        CS7=56,
        #endregion
        #region Assured Forwarding
        /// <summary>
        /// Class 1, Low Drop
        /// </summary>
        AF11=10,
        /// <summary>
        /// Class 1, Med Drop
        /// </summary>
        AF12=12,
        /// <summary>
        /// Class 1, High Drop
        /// </summary>
        AF13=14,
        /// <summary>
        /// Class 2, Low Drop
        /// </summary>
        AF21 = 18,
        /// <summary>
        /// Class 2, Med Drop
        /// </summary>
        AF22 = 20,
        /// <summary>
        /// Class 2, High Drop
        /// </summary>
        AF23 = 22,
        /// <summary>
        /// Class 3, Low Drop
        /// </summary>
        AF31 = 26,
        /// <summary>
        /// Class 3, Med Drop
        /// </summary>
        AF32 = 28,
        /// <summary>
        /// Class 3, High Drop
        /// </summary>
        AF33 = 30,
        /// <summary>
        /// Class 4, Low Drop
        /// </summary>
        AF41 = 34,
        /// <summary>
        /// Class 4, Med Drop
        /// </summary>
        AF42 = 36,
        /// <summary>
        /// Class 4, High Drop
        /// </summary>
        AF43 = 38,
        #endregion
    }
}

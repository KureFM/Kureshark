using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Kureshark.DAL.Extend
{
    /// <summary>
    /// 为PcapDotNet提供扩展方法
    /// </summary>
    public static class PcapDotNetExtend
    {
        #region DeviceName
        static NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

        /// <summary>
        /// 获取设备名
        /// </summary>
        /// <param name="lpd"></param>
        /// <returns></returns>
        public static string DeviceName(this LivePacketDevice lpd)
        {
            string deviceName = "";

            foreach (var adapter in adapters)
            {
                if (lpd.Name.Split('_')[1] == adapter.Id)
                {
                    deviceName = adapter.Name;
                }
            }

            return deviceName;
        }

        #endregion

        #region Guid

        /// <summary>
        /// 获取Guid
        /// </summary>
        /// <param name="lpd"></param>
        /// <returns></returns>
        public static string Guid(this LivePacketDevice lpd)
        {
            return lpd.Name.Split('_')[1].Substring(1, 32);
        }

        #endregion

        #region ExtendDescription

        /// <summary>
        /// 获取额外的描述
        /// </summary>
        /// <param name="lpd"></param>
        /// <returns></returns>
        public static string ExtendDescription(this LivePacketDevice lpd)
        {
            string extendDescription = "";

            foreach (var adapter in adapters)
            {
                if (lpd.Name.Split('_')[1] == adapter.Id)
                {
                    extendDescription = adapter.Description;
                }
            }

            return extendDescription;
        }
        #endregion

        #region Ip
        public static string Ip(this LivePacketDevice lpd)
        {
            return lpd.Addresses[lpd.Addresses.Count - 1].Address.ToString().Split(' ')[1];
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.Net.NetworkInformation;
using Kureshark.DAL.Extend;
using Kureshark.Model.DataLink;
using System.Threading;

namespace Kureshark.DAL
{
    public sealed class Pcap
    {
        #region 私有字段

        private static IList<LivePacketDevice> _allPacketDevices;
        private static List<NetworkDevice> _allNetworkDevices;
        private static object _lock = new object();

        #endregion

        #region 公共方法

        #region 获取所有网络设备
        /// <summary>
        /// 获取所有网络设备
        /// </summary>
        /// <returns></returns>
        public static List<NetworkDevice> AllDevices()
        {
            _allNetworkDevices = new List<NetworkDevice>();

            //使用Pcap获取设备列表
            if (_allPacketDevices == null)
            {
                lock (_lock)
                {
                    _allPacketDevices = LivePacketDevice.AllLocalMachine;
                }
            }

            foreach (var device in _allPacketDevices)
            {
                _allNetworkDevices.Add(new NetworkDevice().Convert(device));
            }

            return _allNetworkDevices;
        }

        #endregion

        #region 手动更新网络设备列表
        /// <summary>
        /// 手动更新网络设备列表
        /// </summary>
        public static void UpdataDeviceList()
        {
            _allPacketDevices = LivePacketDevice.AllLocalMachine;
            _allNetworkDevices.Clear();
            foreach (var device in _allPacketDevices)
            {
                _allNetworkDevices.Add(new NetworkDevice().Convert(device));
            }
        }

        #endregion

        #region 将NetworkDevice转换为对应的LivePacketDevice
        /// <summary>
        /// 将NetworkDevice转换为对应的LivePacketDevice
        /// </summary>
        /// <param name="device">NetworkDevice</param>
        /// <returns></returns>
        public static LivePacketDevice ToPacketDevice(NetworkDevice device)
        {
            return _allPacketDevices[_allNetworkDevices.IndexOf(device)];
        }

        #endregion

        #region 从pcap文件中加载packet
        /// <summary>
        /// 从pcap文件中加载packet
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="linkType">文件的数据链路类型</param>
        /// <returns></returns>
        public static List<Frame> LoadFrom(string fileName, out LinkType linkType)
        {
            OfflinePacketDevice offlineDevice = new OfflinePacketDevice(fileName);

            using (PacketCommunicator communicator =
                offlineDevice.Open(65536,                                   // 65535保证能捕获到不同数据链路层上的每个数据包的全部内容
                                    PacketDeviceOpenAttributes.Promiscuous, // 混杂模式
                                    1000))                                  // 读取超时时间
            {
                linkType = (LinkType)communicator.DataLink.Value;
                List<Frame> frameList = new List<Frame>();
                int i = 1;
                Packet packet;
                //不断循环读取数据包
                while (true)
                {

                    communicator.ReceivePacket(out packet);

                    //当文件结束时packet为null
                    if (packet == null)
                    {
                        break;
                    }
                    frameList.Add(new Frame(packet.Buffer, i, packet.Timestamp));
                    i += 1;
                }

                return frameList;
            }
        }
        #endregion

        #endregion
    }
}

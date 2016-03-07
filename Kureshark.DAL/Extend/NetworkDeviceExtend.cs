using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model;
using PcapDotNet.Core;
using System.Threading;
using PcapDotNet.Packets;
using System.Collections.ObjectModel;
using Kureshark.Model.DataLink;

namespace Kureshark.DAL.Extend
{
    /// <summary>
    /// 为Device提供扩展方法
    /// </summary>
    public static class NetworkDeviceExtend
    {
        #region 私有字段

        private static Thread _capturingPackets;
        private static ObservableQueue<Frame> _captureQueue;
        private static bool _captureflag = true;

        #endregion
            
        #region Convert
        /// <summary>
        /// 将LivePacketDevice转换为Device
        /// </summary>
        /// <param name="device"></param>
        /// <param name="index">索引</param>
        /// <param name="lpd">LivePacketDevice对象</param>
        /// <returns></returns>
        public static NetworkDevice Convert(this NetworkDevice device, LivePacketDevice lpd)
        {
            device.Name = lpd.DeviceName();
            device.Guid = lpd.Guid();
            device.Description = lpd.ExtendDescription();
            device.Ip = lpd.Ip();
            return device;
        }
        #endregion

        #region Start
        /// <summary>
        /// 使用所选NetworkDevice进行捕获
        /// </summary>
        /// <param name="device"></param>
        /// <param name="filterString">过滤表达式</param>
        public static void Start(this NetworkDevice device, string filterString="")
        {
            if (_capturingPackets != null)
            {
                throw new ApplicationException("捕获线程已在运行");
            }
            _captureQueue = new ObservableQueue<Frame>(10000);
            _captureflag = true;
            _capturingPackets = new Thread(() =>
            {
                using (PacketCommunicator communicator =
                    Pcap.ToPacketDevice(device).Open(65536,              // portion of the packet to capture
                    // 65536 guarantees that the whole packet will be captured on all the link layers
                    PacketDeviceOpenAttributes.Promiscuous,                     // promiscuous mode
                    1000))                                                      // read timeout
                {
                    //包过滤器
                    if (!String.IsNullOrWhiteSpace(filterString))
                    {
                        if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
                        {
                            throw new ApplicationException("包过滤只能工作在Ethernet");
                        }
                        communicator.SetFilter(filterString);
                    }

                    //计数器，用于给帧编号
                    int count = 1;
                    Packet packet;
                    while (_captureflag)
                    {
                        PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                        switch (result)
                        {
                            case PacketCommunicatorReceiveResult.Ok:
                                _captureQueue.Enqueue(new Frame(packet.Buffer,count,packet.Timestamp));
                                count += 1;
                                break;
                            case PacketCommunicatorReceiveResult.Timeout:
                                break;
                            default:
                                break;
                        }
                    }
                }
            });

            _capturingPackets.Start();
        }
        #endregion

        #region StartAndSaveTo
        /// <summary>
        /// 使用所选的NetworkDevice进行捕获，并保存到文件
        /// </summary>
        /// <param name="device"></param>
        /// <param name="fileName">文件名</param>
        /// <param name="filterString">过滤表达式</param>
        public static void StartAndSaveTo(this NetworkDevice device, string fileName, string filterString="")
        {
            if (_capturingPackets != null)
            {
                throw new ApplicationException("捕获线程已在运行");
            }
            _captureQueue = new ObservableQueue<Frame>(10000);
            _captureflag = true;
            //使用Lambda创建线程
            _capturingPackets = new Thread(() =>
            {
                using (PacketCommunicator communicator =
                    Pcap.ToPacketDevice(device).Open(65536,              // portion of the packet to capture
                    // 65536 guarantees that the whole packet will be captured on all the link layers
                    PacketDeviceOpenAttributes.Promiscuous,                     // promiscuous mode
                    1000))                                                      // read timeout
                {
                    //包过滤器
                    if (!String.IsNullOrWhiteSpace(filterString))
                    {
                        if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
                        {
                            throw new ApplicationException("包过滤只能工作在Ethernet");
                        }
                        communicator.SetFilter(filterString);
                    }

                    //保存到文件
                    using (PacketDumpFile dumpFile = communicator.OpenDump(fileName))
                    {
                        //计数器，用于给帧编号
                        int count = 1;
                        Packet packet;
                        while (_captureflag)
                        {
                            PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                            switch (result)
                            {
                                case PacketCommunicatorReceiveResult.Ok:
                                    _captureQueue.Enqueue(new Frame(packet.Buffer, count, packet.Timestamp));
                                    count += 1;
                                    dumpFile.Dump(packet);
                                    break;
                                case PacketCommunicatorReceiveResult.Timeout:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            });

            _capturingPackets.Start();
        }
        #endregion

        #region Stop
        /// <summary>
        /// 终止捕获
        /// </summary>
        /// <param name="device"></param>
        public static void Stop(this NetworkDevice device)
        {
            if (_capturingPackets != null)
            {
                _captureflag = false;
                //等待线程结束
                _capturingPackets.Join();
                _capturingPackets = null;
                _captureQueue = null;
            }
        }
        #endregion

        #region GetLinkType
        /// <summary>
        /// 获取该NetworkDevice的数据链路类型
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static LinkType GetLinkType(this NetworkDevice device)
        {
            using (PacketCommunicator communicator =
                Pcap.ToPacketDevice(device).Open(65536,              // portion of the packet to capture
                // 65536 guarantees that the whole packet will be captured on all the link layers
                PacketDeviceOpenAttributes.Promiscuous,                     // promiscuous mode
                1000))                                                      // read timeout
            {
                return (LinkType)communicator.DataLink.Value;
            }
        }
        #endregion

        #region GetQueue
        public static ObservableQueue<Frame> GetQueue(this NetworkDevice device)
        {
            return _captureQueue;
        }
        #endregion
    }
}
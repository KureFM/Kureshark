using Kureshark.DAL;
using Kureshark.DAL.Extend;
using Kureshark.Model;
using Kureshark.Model.DataLink;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Kureshark.BLL
{
    public sealed class PcapOperate
    {
        public ObservableQueue<DecodedFrame> DecodedQueue { get; private set; }

        object _lock = new object();
        public NetworkDevice Device { get; private set; }

        private LinkType _linkType;

        public ObservableQueue<Frame> _captureQueue;


        public static List<NetworkDevice> GetAllDevices()
        {
            return Pcap.AllDevices();
        }

        public PcapOperate(NetworkDevice device)
        {
            Device = device;
            _linkType = Device.GetLinkType();
            DecodedQueue = new ObservableQueue<DecodedFrame>(10000);

        }

        public PcapOperate(string fileName)
        {
            //_captureQueue = Pcap.LoadFrom(fileName, out _linkType);
            _captureQueue = new ObservableQueue<Frame>(Pcap.LoadFrom(fileName, out _linkType));
            //_captureQueue.HasEnqueue += _captureQueue_HasEnqueue;
            DecodedQueue = new ObservableQueue<DecodedFrame>(1000);
            //取Frame解码放入_decodedQueue
            new Thread(() =>
            {
                while (_captureQueue.Count > 0)
                {
                    DecodeFrame();
                }
            }).Start();
        }

        private void _captureQueue_HasEnqueue(object sender, EventArgs e)
        {
            DecodeFrame();
        }

        private void DecodeFrame()
        {
            //取Frame解码放入_decodedQueue
            while (_captureQueue.Count > 0)
            {
                Frame frame = _captureQueue.Dequeue();
                new Thread(() =>
                {
                    DecodedFrame decodedFrame = Decode.As(frame, _linkType);
                    lock (_lock)
                    {
                        DecodedQueue.Enqueue(decodedFrame);
                    }
                }).Start();
            }
        }

        public void Start(string fileName="",string filterString = "")
        {
            if (Device!=null)
            {
                if (String.IsNullOrEmpty(fileName))
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".pcap";
                }
                Device.StartAndSaveTo(fileName, filterString);
                _captureQueue = Device.GetQueue();
                _captureQueue.HasEnqueue += _captureQueue_HasEnqueue;
            }
        }

        public void Stop()
        {
            Device.Stop();
        }
    }
}

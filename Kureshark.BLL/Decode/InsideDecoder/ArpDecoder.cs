using Kureshark.Model;
using Kureshark.Model.DataLink;
using Kureshark.Model.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kureshark.BLL.InsideDecoder
{
    public class ArpDecoder : IDecoder
    {
        #region 私有字段

        private List<TreeNode> _datas;

        Arp arp;

        #endregion

        #region 属性

        public DecoderType Type
        {
            get
            {
                return DecoderType.EtherType;
            }
        }

        public ushort TypeNum
        {
            get
            {
                return (ushort)EtherType.Arp;
            }
        }

        public string Name
        {
            get
            {
                return "ARP";
            }
        }

        public List<TreeNode> Datas
        {
            get
            {
                return _datas;
            }
        }

        public bool HasPayload
        {
            get
            {
                return false;
            }
        }

        public byte[] Payload
        {
            get { throw new NotImplementedException(); }
        }

        public DecoderType PayloadType
        {
            get { throw new NotImplementedException(); }
        }

        public ushort PayloadTypeNum
        {
            get { throw new NotImplementedException(); }
        }

        public ushort PayloadSrcTypeNum
        {
            get { throw new NotImplementedException(); }
        }

        public ushort PayloadDstTypeNum
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public void LoadFrom(byte[] data)
        {
            if (data.Length == 0 || data == null)
            {
                throw new ArgumentException("加载的数据为空，无法完成加载");
            }

            _datas.Clear();
            arp = new Arp(data);

            TreeNode master = new TreeNode("Address Resolution Protocol",
                String.Format("({0})", arp.Operation));

            //硬件类型
            master.Add(new TreeNode("Hardware type:",
                String.Format("{0} ({1})", arp.HardwareType, arp.HardwareType.ToString("D"))));

            //协议类型
            master.Add(new TreeNode("Protocol type:",
                String.Format("{0} (0x{1})", arp.ProtocolType, arp.ProtocolType.ToString("x"))));

            //硬件长度
            master.Add(new TreeNode("Hardware size:", arp.HardwareLength.ToString()));

            //协议长度
            master.Add(new TreeNode("Hardware size:", arp.ProtocolLength.ToString()));

            //操作字段
            master.Add(new TreeNode("Opcode:",
                String.Format("{0} ({1})", arp.Operation, arp.Operation.ToString("d"))));

            //发送方MAC
            master.Add(new TreeNode("Sender MAC address:",
                String.Format("{0} ({1})",
                arp.SenderMac,
                arp.SenderMac)));

            //发送方IP
            master.Add(new TreeNode("Sender IP address:",
                String.Format("{0} ({1})",
                arp.SenderIp,
                arp.SenderIp)));

            //目标MAC
            master.Add(new TreeNode("Target MAC address:",
                String.Format("{0} ({1})",
                arp.TargetMac,
                arp.TargetMac)));

            //目标IP
            master.Add(new TreeNode("Target IP address:",
                String.Format("{0} ({1})",
                arp.TargetIp,
                arp.TargetIp)));

            _datas.Add(master);
        }
        public ArpDecoder()
        {
            _datas = new List<TreeNode>();
        }


        public IDecoder Clone()
        {
            return new ArpDecoder();
        }
    }
}

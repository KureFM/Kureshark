using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model.Network;
using Kureshark.Model.Transport;
using Kureshark.Model.Extend;
using System.Collections.ObjectModel;
using Kureshark.Model;

namespace Kureshark.BLL.InsideDecoder
{
    public class UdpDecoder : IDecoder
    {
        #region 私有字段

        private List<TreeNode> _datas;

        Udp udp;

        #endregion

        #region 属性
        public DecoderType Type
        {
            get
            {
                return DecoderType.Protocol;
            }
        }

        public ushort TypeNum
        {
            get
            {
                return (ushort)IpV4ProtocolField.UDP;
            }
        }

        public string Name
        {
            get
            {
                return "UDP";
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
                return true;
            }
        }

        public byte[] Payload
        {
            get
            {
                return udp.Payload.RawData;
            }
        }

        public DecoderType PayloadType
        {
            get
            {
                return DecoderType.Port;
            }
        }

        public ushort PayloadTypeNum
        {
            get { throw new NotImplementedException(); }
        }

        public ushort PayloadSrcTypeNum
        {
            get
            {
                return (ushort)udp.SourcePort;
            }
        }

        public ushort PayloadDstTypeNum
        {
            get
            {
                return (ushort)udp.DestinationPort;
            }
        }
        #endregion

        public void LoadFrom(byte[] data)
        {
            if (data.Length == 0 || data == null)
            {
                throw new ArgumentException("加载的数据为空，无法完成加载");
            }

            _datas.Clear();

            udp = new Udp(data);

            TreeNode master = new TreeNode();
            master.Header = "User Datagram Protocol,";
            master.Info = String.Format("Src Prot: {0} ({0}), Dst Prot: {1} ({1})",
                udp.SourcePort, udp.DestinationPort);

            //源端口
            master.Add(new TreeNode("Source Port:", udp.SourcePort.ToString()));

            //目的端口
            master.Add(new TreeNode("Destination Port:", udp.DestinationPort.ToString()));

            //长度
            master.Add(new TreeNode("Length:", udp.Length.ToString()));

            //校验和
            master.Add(new TreeNode("Checksum:",
                String.Format("0x{0}", udp.Checksum.ToString("x2"))));

            _datas.Add(master);
        }

        public UdpDecoder()
        {
            _datas = new List<TreeNode>();
        }


        public IDecoder Clone()
        {
            return new UdpDecoder();
        }
    }
}

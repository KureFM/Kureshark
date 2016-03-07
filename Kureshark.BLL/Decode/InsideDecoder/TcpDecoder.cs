using Kureshark.Model.Transport;
using Kureshark.Model.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Kureshark.Model;
using Kureshark.Model.Extend;

namespace Kureshark.BLL.InsideDecoder
{
    public class TcpDecoder : IDecoder
    {
        #region 私有字段

        private List<TreeNode> _datas;

        Tcp tcp;

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
                return (byte)IpV4ProtocolField.TCP;
            }
        }

        public string Name
        {
            get
            {
                return "TCP";
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
                if (tcp.Payload == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public byte[] Payload
        {
            get
            {
                return tcp.Payload.RawData;
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
                return tcp.SourcePort;
            }
        }

        public ushort PayloadDstTypeNum
        {
            get
            {
                return tcp.DestinationPort;
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
            tcp = new Tcp(data);

            TreeNode master = new TreeNode("Transmission Control Protocol,",
                String.Format("Src Port: {0} ({0}),Dst Port: {1} ({1}), Seq: {2}, Ack: {3}, Len: {4}",
                tcp.SourcePort, tcp.DestinationPort, tcp.SeqNum, tcp.AckNum, tcp.Length));

            //源端口
            master.Add(new TreeNode("Source Port:",
                String.Format("{0} ({0})", tcp.SourcePort)));

            //目的端口
            master.Add(new TreeNode("Destination Port:",
                String.Format("{0} ({0})", tcp.DestinationPort)));

            //Seq
            master.Add(new TreeNode("Sequence number:", tcp.SeqNum.ToString()));

            //Ack
            master.Add(new TreeNode("Acknowledgment number:", tcp.AckNum.ToString()));

            //首部长度
            master.Add(new TreeNode("Header Length:",
                String.Format("{0} bytes", tcp.DataOffest * 4)));
            #region Flags
            //Flags
            TreeNode flags = new TreeNode("Flags:",
                String.Format("0b{0}", Convert.ToString(tcp.Flags.GetValue(),2).PadLeft(12, '0')));

            //保留
            flags.Add(new TreeNode("000. .... .... = Reserved:", BooleanToSet(false)));

            //NS
            flags.Add(new TreeNode(String.Format("...{0} .... .... = Nonce Sum:", Convert.ToByte(tcp.Flags.NS)),
                BooleanToSet(tcp.Flags.NS)));

            //CWR
            flags.Add(new TreeNode(String.Format(".... {0}... .... = Congestion Window Reduced:", Convert.ToByte(tcp.Flags.CWR)),
                BooleanToSet(tcp.Flags.CWR)));

            //ECE
            flags.Add(new TreeNode(String.Format(".... .{0}.. .... = ECN-Echo:", Convert.ToByte(tcp.Flags.ECE)),
                BooleanToSet(tcp.Flags.ECE)));

            //URG
            flags.Add(new TreeNode(String.Format(".... ..{0}. .... = Urgent Pointer:", Convert.ToByte(tcp.Flags.URG)),
                BooleanToSet(tcp.Flags.URG)));

            //ACK
            flags.Add(new TreeNode(String.Format(".... ...{0} .... = Acknowledgment:", Convert.ToByte(tcp.Flags.ACK)),
                BooleanToSet(tcp.Flags.ACK)));

            //PSH
            flags.Add(new TreeNode(String.Format(".... .... {0}... = Push:", Convert.ToByte(tcp.Flags.PSH)),
                BooleanToSet(tcp.Flags.PSH)));

            //RST
            flags.Add(new TreeNode(String.Format(".... .... .{0}.. = Reset:", Convert.ToByte(tcp.Flags.RST)),
                BooleanToSet(tcp.Flags.RST)));

            //SYN
            flags.Add(new TreeNode(String.Format(".... .... ..{0}. = Synchronize:", Convert.ToByte(tcp.Flags.SYN)),
                BooleanToSet(tcp.Flags.SYN)));

            //FIN
            flags.Add(new TreeNode(String.Format(".... .... ...{0} = Finish:", Convert.ToByte(tcp.Flags.FIN)),
                BooleanToSet(tcp.Flags.FIN)));
            master.Add(flags);

            #endregion

            //窗口大小
            master.Add(new TreeNode("Window size:", tcp.WindowSize.ToString()));

            //校验和
            master.Add(new TreeNode("Checksum:",
                String.Format("0x{0}",tcp.Checksum.ToString("x2"))));

            //紧急指针
            master.Add(new TreeNode("Urgent Pointer:", tcp.UrgentPointer.ToString("d")));

            Datas.Add(master);
        }

        public TcpDecoder()
        {
            _datas = new List<TreeNode>();
        }


        public IDecoder Clone()
        {
            return new TcpDecoder();
        }

        /// <summary>
        /// 将Boolean转换为Set或者Not set
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string BooleanToSet(bool value)
        {
            if (value)
            {
                return "Set";
            }
            else
            {
                return "Not set";
            }
        }
    }
}

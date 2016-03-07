using Kureshark.BLL;
using Kureshark.Model.DataLink;
using Kureshark.Model.Network;
using Kureshark.Model.Extend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Kureshark.Model;

namespace Kureshark.BLL.InsideDecoder
{
    /// <summary>
    /// IpV4解码器，内置的解码器
    /// </summary>
    public class IpV4Decoder : IDecoder
    {
        #region 私有字段

        private List<TreeNode> _datas;

        Ipv4 ipV4;

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
                return (ushort)EtherType.IpV4;
            }
        }

        public string Name
        {
            get
            {
                return "IPv4";
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
                if (ipV4.Payload == null)
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
                return ipV4.Payload.RawData;
            }
        }

        public DecoderType PayloadType
        {
            get
            {
                return DecoderType.Protocol;
            }
        }

        public ushort PayloadTypeNum
        {
            get
            {
                return (ushort)ipV4.Protocol;
            }
        }

        public ushort PayloadSrcTypeNum
        {
            get
            {
                return PayloadTypeNum;
            }
        }

        public ushort PayloadDstTypeNum
        {
            get
            {
                return PayloadTypeNum;
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
            ipV4 = new Ipv4(data);

            TreeNode master = new TreeNode();
            master.Header = "Internet Protocol Version 4,";
            master.Info = String.Format("Src: {0} ({0}), Dst: {1} ({1})",
                ipV4.SourceIp,
                ipV4.DestinationIp);

            //版本信息
            TreeNode version = new TreeNode();
            version.Header = "Version:";
            version.Info = ipV4.Version.ToString();
            master.Add(version);

            //头部长度
            master.Add(new TreeNode("Header Length:",
                String.Format("{0} bytes", ipV4.IHL * 4).ToString()));

            //DiffServ
            TreeNode dsf = new TreeNode("Differentiated Services Field:",
                String.Format("0b{0} (DSCP 0b{1}: {2}; ECN 0b{3}: {4})",
                Convert.ToString(ipV4.DiffServ.GetValue(), 2).PadLeft(8, '0'),
                Convert.ToString((byte)ipV4.DiffServ.DSCP, 2).PadLeft(6, '0'),
                GetDscpInfo(ipV4.DiffServ.DSCP),
                Convert.ToString((byte)ipV4.DiffServ.ECN, 2).PadLeft(2, '0'),
                GetEcnInfo(ipV4.DiffServ.ECN)));

            //DSCP
            string dscp = Convert.ToString((byte)ipV4.DiffServ.DSCP, 2).PadLeft(6, '0');
            dsf.Add(new TreeNode(
                String.Format("{0} {1}.. = Differentiated Services Codepoint:",
                dscp.Substring(0, 4),
                dscp.Substring(4, 2)),
                String.Format("{0} (0b{1})",
                GetDscpInfo(ipV4.DiffServ.DSCP), dscp)));

            //ECN
            string ecn = Convert.ToString((byte)ipV4.DiffServ.ECN, 2).PadLeft(2, '0');
            dsf.Add(new TreeNode(
                String.Format(".... ..{0} = EXplicit Congestion Notification:", ecn),
                String.Format("{0} (0b{1})",
                GetEcnInfo(ipV4.DiffServ.ECN), ecn)));

            master.Add(dsf);

            //总长度
            master.Add(new TreeNode("Total Length:", ipV4.Length.ToString()));

            //标识字段
            master.Add(new TreeNode("Identification:",
                String.Format("0x{0} ({1})",
                ipV4.Identification.ToString("x2"),
                BitConverter.ToUInt16(ipV4.Identification.Reverse(), 0).ToString())));

            //标志字段
            TreeNode flags = new TreeNode("Flags:",
                String.Format("0b{0}", Convert.ToString(ipV4.Flags.Value, 2).PadLeft(3, '0')));

            //保留标记
            flags.Add(new TreeNode("0.. = Reserved bit:", "Must be zero"));

            //DF标志
            TreeNode df = new TreeNode(
                String.Format(".{0}. = Don't fragment:",
                Convert.ToByte(ipV4.Flags.DF)),
                BooleanToSet(ipV4.Flags.DF));
            flags.Add(df);

            //MF标志
            TreeNode mf = new TreeNode(
                String.Format("..{0} = More fragments:",
                Convert.ToByte(ipV4.Flags.MF)),
                BooleanToSet(ipV4.Flags.MF));
            flags.Add(mf);

            master.Add(flags);

            //片偏移
            master.Add(new TreeNode("Fragment offest:",
                ipV4.FragmentOffset.ToString("D")));

            //TTL
            master.Add(new TreeNode("Time to live:",
                ipV4.TTL.ToString()));

            //协议号
            master.Add(new TreeNode("Protocol:",
                String.Format("{0} ({1})",
                ipV4.Protocol,
                ipV4.Protocol.ToString("D"))));

            //头部校验和
            master.Add(new TreeNode("Header checksum:",
                String.Format("0x{0}", ipV4.HeaderChecksum.ToString("x2"))));

            //源IP地址
            master.Add(new TreeNode("Source:", ipV4.SourceIp.ToString()));

            //目的IP地址
            master.Add(new TreeNode("Destination:", ipV4.DestinationIp.ToString()));

            Datas.Add(master);
        }

        public IpV4Decoder()
        {
            _datas = new List<TreeNode>();
        }

        #region 私有辅助方法

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

        /// <summary>
        /// 从DSCP枚举中提取更多的信息
        /// </summary>
        /// <param name="dscp"></param>
        /// <returns></returns>
        private string GetDscpInfo(IpV4Dscp dscp)
        {
            switch (dscp)
            {
                case IpV4Dscp.Default:
                    return "Default";
                case IpV4Dscp.EF:
                    return "Expedited Forwarding";
                case IpV4Dscp.CS1:
                case IpV4Dscp.CS2:
                case IpV4Dscp.CS3:
                case IpV4Dscp.CS4:
                case IpV4Dscp.CS5:
                case IpV4Dscp.CS6:
                case IpV4Dscp.CS7:
                    string cs = dscp.ToString();
                    return String.Format("{0} (Class Selector {1})", cs, cs.Remove(0, 2));
                default:
                    string af = dscp.ToString();
                    string afA = af.Substring(0, 1);
                    string afB = af.Substring(1, 1);
                    StringBuilder info = new StringBuilder(af);
                    info.Append(" (");
                    switch (afA)
                    {
                        case "1":
                            info.Append("Class1, ");
                            break;
                        case "2":
                            info.Append("Class2, ");
                            break;
                        case "3":
                            info.Append("Class3, ");
                            break;
                        case "4":
                            info.Append("Class4, ");
                            break;
                        default:
                            break;
                    }
                    switch (afB)
                    {
                        case "1":
                            info.Append("Low Drop)");
                            break;
                        case "2":
                            info.Append("Med Drop)");
                            break;
                        case "3":
                            info.Append("High Drop)");
                            break;
                        default:
                            break;
                    }
                    return info.ToString();
            }
        }

        /// <summary>
        /// 从ECN枚举中提取更多信息
        /// </summary>
        /// <param name="ecn"></param>
        /// <returns></returns>
        private string GetEcnInfo(IpV4Ecn ecn)
        {
            switch (ecn)
            {
                case IpV4Ecn.Non_ECT:
                    return "Non-ECT (Non ECN-Capable Transport)";
                case IpV4Ecn.ECT0:
                    return "ECT(0) (ECN Capable Transport)";
                case IpV4Ecn.ECT1:
                    return "ECT(1) (ECN Capable Transport)";
                case IpV4Ecn.CE:
                    return "CE (Congestion Encountered)";
                default:
                    return "";
            }
        }
        #endregion


        public IDecoder Clone()
        {
            return new IpV4Decoder();
        }
    }
}

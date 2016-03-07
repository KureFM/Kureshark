using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Kureshark.Model.DataLink;
using Kureshark.Model;

namespace Kureshark.BLL.InsideDecoder
{
    /// <summary>
    /// 以太网解码器，内置的解码器
    /// </summary>
    public class EthernetDecoder : IDecoder
    {
        #region 私有字段

        private List<TreeNode> _datas;

        Ethernet ether;

        #endregion

        #region 属性
        public DecoderType Type
        {
            get
            {
                return DecoderType.LinkType;
            }
        }

        public ushort TypeNum
        {
            get
            {
                return (ushort)LinkType.Ethernet_II;
            }
        }

        public string Name
        {
            get
            {
                return "Ethernet";
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
                return ether.Payload.RawData;
            }
        }

        public DecoderType PayloadType
        {
            get
            {
                return DecoderType.EtherType;
            }
        }

        public ushort PayloadTypeNum
        {
            get
            {
                return (ushort)ether.Type;
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

            ether = new Ethernet(data);
            TreeNode master = new TreeNode();
            master.Header = "Ethernet II,";
            master.Info = String.Format("Src: ({0}), Dst: ({1})", ether.Source, ether.Destination);
            //目的MAC地址
            TreeNode dst = new TreeNode();
            dst.Header = "Destination:";
            dst.Info = ether.Destination.ToString();
            master.Nodes.Add(dst);
            //源MAC地址
            TreeNode src = new TreeNode();
            src.Header = "Source:";
            src.Info = ether.Source.ToString();
            master.Nodes.Add(src);
            //以太类型
            TreeNode type = new TreeNode();
            type.Header = "EtherType:";
            type.Info = String.Format("{0} (0x{1})",
                ether.Type.ToString(),
                ether.Type.ToString("X"));
            master.Nodes.Add(type);
            _datas.Add(master);
        }
        public EthernetDecoder()
        {
            _datas = new List<TreeNode>();
        }


        public IDecoder Clone()
        {
            return new EthernetDecoder();
        }
    }
}

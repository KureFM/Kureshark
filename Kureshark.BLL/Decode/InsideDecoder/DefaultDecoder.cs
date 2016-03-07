using Kureshark.Model;
using Kureshark.Model.Extend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kureshark.BLL.InsideDecoder
{
    /// <summary>
    /// 默认解码器，在找不到符合条件的解码器时使用该解码器解码
    /// </summary>
    public class DefaultDecoder:IDecoder
    {
        #region 私有字段

        private List<TreeNode> _datas;

        #endregion

        #region 属性
        public DecoderType Type
        {
            get { throw new NotImplementedException(); }
        }

        public ushort TypeNum
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get
            {
                return "Default";
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
            TreeNode master = new TreeNode("Data", String.Format("({0} bytes)", data.Length));
            master.Add(new TreeNode("Data:",data.ToString("x2")));
            master.Add(new TreeNode(String.Format("[Length: {0}]",data.Length),""));
            _datas.Add(master);
        }

        public DefaultDecoder()
        {
            _datas = new List<TreeNode>();
        }

        IDecoder IDecoder.Clone()
        {
            return new DefaultDecoder();
        }
    }
}

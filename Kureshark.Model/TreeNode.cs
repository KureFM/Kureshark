using System.Collections.Generic;

namespace Kureshark.Model
{
    /// <summary>
    /// DecodedFrame中的树形结构节点
    /// </summary>
    public class TreeNode
    {
        public string Header { set; get; }

        public string Info { set; get; }

        #region 颜色属性，为绑定后的显示提供视觉效果

        /// <summary>
        /// 颜色值可使用KnownColor枚举中或者"#+hex颜色值" https://msdn.microsoft.com/zh-cn/library/system.drawing.knowncolor(v=vs.110).aspx
        /// </summary>
        public string HeaderColor { set; get; }

        public string InfoColor { set; get; }

        public string Background { set; get; }

        #endregion 颜色属性，为绑定后的显示提供视觉效果

        /// <summary>
        /// 子节点集合
        /// </summary>
        public List<TreeNode> Nodes { set; get; }

        public TreeNode()
        {
            //默认的颜色
            HeaderColor = "Black";
            InfoColor = "Black";
            Background = "White";
            Nodes = new List<TreeNode>();
        }

        /// <summary>
        /// 提供快速创建TreeNode的构造函数
        /// </summary>
        /// <param name="header"></param>
        /// <param name="info"></param>
        public TreeNode(string header, string info)
        {
            //默认的颜色
            HeaderColor = "Black";
            InfoColor = "Black";
            Background = "White";
            Nodes = new List<TreeNode>();
            Header = header;
            Info = info;
        }

        /// <summary>
        /// 提供快速为Nodes添加节点的方法
        /// </summary>
        /// <param name="node"></param>
        public void Add(TreeNode node)
        {
            Nodes.Add(node);
        }
    }
}
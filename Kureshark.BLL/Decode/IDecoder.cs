using Kureshark.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kureshark.BLL
{
    /// <summary>
    /// 解码器接口，每个解码器都必须继承此接口
    /// </summary>
    public interface IDecoder
    {
        #region 属性
        #region 描述解码器特征
        /// <summary>
        /// 解码器的工作层
        /// </summary>
        DecoderType Type { get; }

        /// <summary>
        /// 解码器的类型号
        /// </summary>
        ushort TypeNum { get; }

        /// <summary>
        /// 解码器的名称，显示协议栈需要
        /// </summary>
        string Name { get; }
        #endregion

        /// <summary>
        /// 解码后的树状结构信息
        /// </summary>
        List<TreeNode> Datas { get; }

        #region 描述Payload的特征
        /// <summary>
        /// 是否携带Payload
        /// </summary>
        bool HasPayload { get; }

        /// <summary>
        /// Payload数据
        /// </summary>
        byte[] Payload { get; }

        /// <summary>
        /// Payload的类型号
        /// </summary>
        DecoderType PayloadType { get; }

        /// <summary>
        /// 如果不需要实现该接口请务必抛出NotImplementedException
        /// </summary>
        ushort PayloadTypeNum { get;  }
        #region 源类型码和目的类型码，为应用层解析服务
        /// <summary>
        /// 如果不需要实现该接口请务必抛出NotImplementedException
        /// </summary>
        ushort PayloadSrcTypeNum { get; }
        /// <summary>
        /// 如果不需要实现该接口请务必抛出NotImplementedException
        /// </summary>
        ushort PayloadDstTypeNum { get; }
        #endregion
        #endregion

        #endregion

        /// <summary>
        /// 该方法的实现要求在加载的同时解码完毕
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        void LoadFrom(byte[] data);

        /// <summary>
        /// 请务必实现深拷贝
        /// </summary>
        /// <returns></returns>
        IDecoder Clone();
    }
}

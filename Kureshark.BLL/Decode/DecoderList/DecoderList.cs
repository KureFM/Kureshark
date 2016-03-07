using Kureshark.BLL.InsideDecoder;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kureshark.BLL.DecoderList
{
    /// <summary>
    /// 实现一些解码器列表的基本功能
    /// </summary>
    public class DecoderList
    {
        protected Hashtable _decoderList;

        /// <summary>
        /// 根据类型号码取出解码器
        /// </summary>
        /// <param name="TypeNum"></param>
        /// <returns></returns>
        public IDecoder GetDecoder(ushort typeNum)
        {
            IDecoder iDecoder = (IDecoder)_decoderList[typeNum];
            if (iDecoder == null)
            {
                //没有找到解码器就使用默认解码器
                return new DefaultDecoder();
            }
            return iDecoder.Clone();
        }


        protected DecoderList()
        {
            _decoderList = new Hashtable();
        }

        /// <summary>
        /// 添加解码器
        /// </summary>
        /// <param name="decoder">实现IDecoder的解码器</param>
        public void AddDecoder(IDecoder decoder)
        {
            _decoderList.Add(decoder.TypeNum, decoder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model;
using Kureshark.Model.DataLink;
using System.Collections.ObjectModel;
using Kureshark.BLL.DecoderList;
using System.Globalization;

namespace Kureshark.BLL
{
    public class Decode
    {
        public static DecodedFrame As(Frame frame, LinkType linkType)
        {
            List<string> decoderName;
            List<TreeNode> datas = new List<TreeNode>();
            List<TreeNode> temp = As(frame.RawData, DecoderType.LinkType, (ushort)linkType, out decoderName);

            int length = frame.Length;
            int number = frame.Index;

            //帧解码
            TreeNode master = new TreeNode(
                String.Format("Frame {0}:", number),
                String.Format("{0} bytes on wire ({1} bits), {2} bytes captured ({3} bits)",
                length, length * 8, length, length * 8)) { Background = "#E5E5E5" };

            //封装类型
            master.Add(new TreeNode("Encapsulation type:",
                String.Format("{0} ({1})", linkType, linkType.ToString("D"))));

            //到达时间
            master.Add(new TreeNode("Arrival Time:", frame.Timestamp.ToString("MMM d, yyyy HH:mm:ss.fffffff",
                CultureInfo.CreateSpecificCulture("en-US"))));

            //编号
            master.Add(new TreeNode("Frame Number:", number.ToString()));

            //长度
            master.Add(new TreeNode("Frame Length:", String.Format("{0} bytes ({1} bits)", length, length)));
            master.Add(new TreeNode("Capture Length:", String.Format("{0} bytes ({1} bits)", length, length)));

            //协议栈
            master.Add(new TreeNode("Protocols in frame:", String.Join("|", decoderName)));

            datas.Add(master);

            datas.AddRange(temp);

            DecodedFrame dFrame = new DecodedFrame(frame);

            dFrame.TreeDisplay = datas;
            if(decoderName[decoderName.Count - 1]!="Default")
            {
                dFrame.Protocol = decoderName[decoderName.Count - 1];
            }
            else
            {
                dFrame.Protocol = decoderName[decoderName.Count - 2];
            }
            try
            {
                List<string> ls = findInfos(temp, "Source:");
                dFrame.Source = ls[ls.Count-1];
                ls = findInfos(temp, "Destination:");
                dFrame.Destination = ls[ls.Count - 1];
            }
            catch (Exception)
            {
                
            }


            return dFrame;
        }

        /// <summary>
        /// 解码为
        /// </summary>
        /// <param name="data">要解码的byte</param>
        /// <param name="type">解码类型</param>
        /// <param name="typeNum">解码类型号</param>
        /// <param name="srcTypeNum">源解码类型号</param>
        /// <param name="dstTypeNum">目的解码类型号</param>
        /// <returns></returns>
        private static List<TreeNode> As(byte[] data, DecoderType type, ushort typeNum,
            out List<string> decoderName)                                                                   //out出所有使用的解码器的名称
        {
            List<TreeNode> nodes = new List<TreeNode>();

            decoderName = new List<string>();

            IDecoder iDecoder;

            #region 选择解码器

            switch (type)
            {
                case DecoderType.LinkType:
                    iDecoder = DataLinkDecoderList.Instance().GetDecoder(typeNum);
                    break;
                case DecoderType.EtherType:
                    iDecoder = NetworkDecoderList.Instance().GetDecoder(typeNum);
                    break;
                case DecoderType.Protocol:
                    iDecoder = TransportDecoderList.Instance().GetDecoder(typeNum);
                    break;
                case DecoderType.Port:
                    iDecoder = AppDecoderList.Instance().GetDecoder(typeNum);
                    break;
                //throw new NotImplementedException();
                default:
                    throw new ArgumentException("数据类型type错误，该数值不符合DecoderType枚举");
            }

            #endregion

            //解码
            iDecoder.LoadFrom(data);
            nodes.AddRange(iDecoder.Datas);

            decoderName.Add(iDecoder.Name);

            #region 递归解码

            if (iDecoder.HasPayload)
            {
                List<string> sTemp=new List<string>();
                List<TreeNode> temp = new List<TreeNode>();
                try
                {
                    temp = As(iDecoder.Payload, iDecoder.PayloadType, iDecoder.PayloadTypeNum, out sTemp);
                }
                catch (NotImplementedException)
                {
                    try
                    {
                        if (iDecoder.PayloadSrcTypeNum>1024)
                        {
                            throw new NotImplementedException();
                        }
                        temp = As(iDecoder.Payload, iDecoder.PayloadType, iDecoder.PayloadSrcTypeNum, out sTemp);
                    }
                    catch (NotImplementedException)
                    {
                        try
                        {
                            temp = As(iDecoder.Payload, iDecoder.PayloadType, iDecoder.PayloadDstTypeNum, out sTemp);
                        }
                        catch (NotImplementedException)
                        {

                        }
                    }
                }
                decoderName.AddRange(sTemp);
                nodes.AddRange(temp);
            }

            #endregion

            return nodes;
        }

        /// <summary>
        /// 用于递归的私有方法
        /// 深度优先遍历，返回找到的第一个header的Info
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private static string findInfo(IEnumerable<TreeNode> nodes, string header)
        {
            if (String.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentException("header is empty!");
            }
            if (nodes.Count() > 0)
            {
                foreach (var node in nodes)
                {
                    if (node.Header == header)
                    {
                        return node.Info;
                    }
                    //递归
                    return findInfo(node.Nodes, header);
                }
            }
            return "没有找到";
        }

        /// <summary>
        /// 深度优先遍历，返回找到的所有header的Info
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private static List<string> findInfos(IEnumerable<TreeNode> nodes, string header)
        {
            if (String.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentException("header is empty!");
            }
            List<string> temp = new List<string>();
            if (nodes.Count() > 0)
            {
                foreach (var node in nodes)
                {
                    if (node.Header == header)
                    {
                        temp.Add(node.Info);
                    }
                    //递归
                    temp.AddRange(findInfos(node.Nodes, header));
                }
            }
            return temp;
        }
    }
}

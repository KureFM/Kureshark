using Kureshark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Kureshark.Model.Extend;

namespace Kureshark.BLL.InsideDecoder
{
    public class DnsDecoder : IDecoder
    {
        #region 私有字段

        private List<TreeNode> nodes;

        private byte[] rawData;

        #endregion

        #region 属性
        public DecoderType Type
        {
            get { return DecoderType.Port; }
        }

        public ushort TypeNum
        {
            get { return 53; }
        }

        public string Name
        {
            get { return "DNS"; }
        }

        public List<TreeNode> Datas
        {
            get { return nodes; }
        }

        public bool HasPayload
        {
            get { return false; }
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
            /*The header contains the following fields:

                                            1  1  1  1  1  1
              0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
            +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            |                      ID                       |
            +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            |QR|   Opcode  |AA|TC|RD|RA|        |   RCODE   |
            +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            |                    QDCOUNT                    |
            +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            |                    ANCOUNT                    |
            +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            |                    NSCOUNT                    |
            +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            |                    ARCOUNT                    |
            +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            */

            rawData = data;
            TreeNode master = new TreeNode(String.Format("Domain Name System ({0})",
                (GetBit(rawData[2], 8) == 0 ? "reply" : "response")), "");

            //Transaction ID
            master.Add(new TreeNode("Transaction ID:",
                String.Format("0x{0}", rawData.Sub(0, 2).ToString("x2"))));

            //Flags
            TreeNode flags = new TreeNode("Flags:",
                String.Format("0b{0}", Convert.ToString(BitConverter.ToUInt16(rawData.Sub(2, 2).Reverse(), 0), 2).PadLeft(16, '0')));

            //QR
            flags.Add(new TreeNode(
                String.Format("{0}... .... .... .... = Response:", GetBit(rawData[2], 8)),
                String.Format("Message is a {0}", GetBit(rawData[2], 8) == 0 ? "reply" : "response")));

            //Opcode
            string opcodeStatus = "";
            int opcode = ((rawData[2] >> 3) & 15);
            switch (opcode)
            {
                case 0:
                    opcodeStatus = "Standard query (0)";
                    break;
                case 1:
                    opcodeStatus = "Inverse query (1)";
                    break;
                case 2:
                    opcodeStatus = "Query allowing multiple answers (2)";
                    break;
                default:
                    opcodeStatus = String.Format("Undefined ({0})", opcode);
                    break;
            }

            flags.Add(new TreeNode(
                String.Format(".{0}{1}{2} {3}... .... .... = Opcode:", GetBit(rawData[2], 7), GetBit(rawData[2], 6), GetBit(rawData[2], 5), GetBit(rawData[2], 4)),
                opcodeStatus));

            //Authoritative Answer
            flags.Add(new TreeNode(
                String.Format(".... .{0}.. .... .... = Authoritative answer:", GetBit(rawData[2], 3)),
                String.Format("Message is {0}", GetBit(rawData[2], 3) == 0 ? "authoritative answer" : "non-authoritative answer")));

            //TrunCation
            flags.Add(new TreeNode(
                String.Format(".... ..{0}. .... .... = Truncation:", GetBit(rawData[2], 2)),
                String.Format("Message is {0}truncated", GetBit(rawData[2], 2) == 0 ? "not " : "")));

            //Recursion Desired
            flags.Add(new TreeNode(
                String.Format(".... ...{0} .... .... = Recursion desired:", GetBit(rawData[2], 1)),
                String.Format("{0} query recursively", GetBit(rawData[2], 1) == 0 ? "Undo" : "Do")));

            //Recursion Available
            //flags.Add(new TreeNode(
            //    String.Format(".... .... {0}... .... = Recursion available",GetBit(rawData[3],8),
            //    String.Format("")));

            master.Add(flags);

            //问题数
            master.Add(new TreeNode("Questions:", QuestionRRS.ToString()));

            //应答数
            master.Add(new TreeNode("Answer RRS:", AnswerRRS.ToString()));

            //机构数
            master.Add(new TreeNode("Authority RRS:", AuthorityRRS.ToString()));

            //其他资源数
            master.Add(new TreeNode("Additional RRS", AdditionalRRS.ToString()));

            //请求
            TreeNode queries = new TreeNode("Queries", "");
            int next;
            for (int i = 0; i < QuestionRRS; i++)
            {
                queries.Add(DecodeQuery(12, out next));
            }

            master.Add(queries);

            nodes.Add(master);
        }

        public IDecoder Clone()
        {
            return new DnsDecoder();
        }

        public DnsDecoder()
        {
            nodes = new List<TreeNode>();
        }

        /// <summary>
        /// 取出byte中某一位(位数按低到高的顺序从1开始计)的值(bit)
        /// </summary>
        /// <param name="aByte">要取出值得byte</param>
        /// <param name="bitNum">要取出byte值的位数(位数按低到高的顺序从1开始计)</param>
        /// <returns>因为每个bit取值只有0或1，故返回值为bool</returns>
        private byte GetBit(byte aByte, int bitNum)
        {
            if (bitNum > 8)
            {
                throw new ArgumentException("bitNum must be < 8.");
            }
            aByte >>= (bitNum - 1);
            aByte &= 1;
            return aByte;
        }

        #region 各种计数

        /// <summary>
        /// 问题资源记录计数
        /// </summary>
        private ushort QuestionRRS
        {
            get
            {
                return BitConverter.ToUInt16(rawData.Sub(4, 2).Reverse(), 0);
            }
        }

        /// <summary>
        /// 应答资源记录计数
        /// </summary>
        private ushort AnswerRRS
        {
            get
            {
                return BitConverter.ToUInt16(rawData.Sub(6, 2).Reverse(), 0);
            }
        }

        /// <summary>
        /// 颁发机构资源记录计数
        /// </summary>
        private ushort AuthorityRRS
        {
            get
            {
                return BitConverter.ToUInt16(rawData.Sub(8, 2).Reverse(), 0);
            }
        }

        /// <summary>
        /// 其他资源记录计数
        /// </summary>
        private ushort AdditionalRRS
        {
            get
            {
                return BitConverter.ToUInt16(rawData.Sub(10, 2).Reverse(), 0);
            }
        }

        #endregion

        private TreeNode DecodeQuery(int thisOffest, out int nextOffest)
        {
            TreeNode query = new TreeNode();

            List<string> names = new List<string>();

            while (true)
            {
                byte strLen = rawData[thisOffest];
                if (strLen == 0)
                {
                    break;
                }

                thisOffest += 1;

                names.Add(Encoding.ASCII.GetString(rawData.Sub(thisOffest, strLen)));

                thisOffest += strLen;
            }

            string name = String.Join(".", names);

            query.Header = String.Format("{0}:", name);
            query.Info = "type A";

            query.Add(new TreeNode("Name:", name));
            query.Add(new TreeNode("[Name Length:", String.Format("{0}]", name.Length)));
            query.Add(new TreeNode("[Label Count:", String.Format("{0}]", names.Count)));

            thisOffest += 1;

            ushort qType = BitConverter.ToUInt16(rawData.Sub(thisOffest, 2).Reverse(), 0);

            string typeString = "";

            #region 以下swatch代码使用python脚本生成
            switch (qType)
            {
                case 1:
                    typeString = "A (a host address) (1)";
                    break;
                case 2:
                    typeString = "NS (an authoritative name server) (2)";
                    break;
                case 3:
                    typeString = "MD (a mail destination) (3)";
                    break;
                case 4:
                    typeString = "MF (a mail forwarder) (4)";
                    break;
                case 5:
                    typeString = "CNAME (the canonical name for an alias) (5)";
                    break;
                case 6:
                    typeString = "SOA (marks the start of a zone of authority) (6)";
                    break;
                case 7:
                    typeString = "MB (a mailbox domain name) (7)";
                    break;
                case 8:
                    typeString = "MG (a mail group member) (8)";
                    break;
                case 9:
                    typeString = "MR (a mail rename domain name) (9)";
                    break;
                case 10:
                    typeString = "NULL (a null RR) (10)";
                    break;
                case 11:
                    typeString = "WKS (a well known service description) (11)";
                    break;
                case 12:
                    typeString = "PTR (a domain name pointer) (12)";
                    break;
                case 13:
                    typeString = "HINFO (host information) (13)";
                    break;
                case 14:
                    typeString = "MINFO (mailbox or mail list information) (14)";
                    break;
                case 252:
                    typeString = "AXFR (A request for a transfer of an entire zone of authority) (252)";
                    break;
                case 253:
                    typeString = "MAILB (A request for mailbox-related records (MB, MG or MR)) (253)";
                    break;
                case 254:
                    typeString = "MAILA (A request for mail agent RRs (MD and MF)) (254)";
                    break;
                case 255:
                    typeString = "* (A request for all records) (255)";
                    break;
                default:
                    typeString = String.Format("Unknow type {0}", qType.ToString());
                    break;
            }
            #endregion

            query.Add(new TreeNode("Type:", typeString));

            thisOffest += 2;

            ushort qClass = BitConverter.ToUInt16(rawData.Sub(thisOffest, 2).Reverse(), 0);

            string classString = "";

            switch (qClass)
            {
                case 1:
                    classString = "IN (ARPA Internet) (1)";
                    break;
                case 2:
                    classString = "NS (Computer Science Network) (2)";
                    break;
                default:
                    classString = String.Format("Unknow class {0}", qClass.ToString());
                    break;
            }

            query.Add(new TreeNode("Class:", classString));

            nextOffest = thisOffest + 2;

            return query;
        }
    }
}

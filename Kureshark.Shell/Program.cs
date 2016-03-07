using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kureshark.Model;
using Kureshark.DAL;
using Kureshark.DAL.Extend;
using Kureshark.Model.DataLink;
using Kureshark.Model.Network;
using Kureshark.Model.Extend;
using Kureshark.BLL.InsideDecoder;
using System.Collections.ObjectModel;
using Kureshark.BLL;

namespace Kureshark.Shell
{
    class Program
    {
        static NetworkDevice wlan;
        static void Main(string[] args)
        {
            List<NetworkDevice> devices = Pcap.AllDevices();
            foreach (var device in devices)
            {
                Console.WriteLine(device.ToString());
            }

            wlan = devices[5];

            Console.ReadKey();
            wlan.Start();
            Console.WriteLine("开始捕获");
            //while (true)
            //{
            //    Console.ReadKey();
            //    Console.WriteLine("共捕获到{0}个数据包", wlan.GetFrameList().Count);
            //}
            //Console.ReadKey();
            //wlan.Stop();
            //Console.WriteLine("捕获结束");
            //Console.ReadKey();
            //MacAddress mac1 = MacAddress.Parse("12:12:12:12:12:12");
            //MacAddress mac2 = MacAddress.Parse("12:12:12:12:12:12");
            //Console.WriteLine(mac1.ToString());
            //if (mac1.Equals(mac2))
            //{
            //    Console.WriteLine("equals");
            //}
            //byte[] payload={212,2,3,4,5,6,7,8,9,10};
            //Frame f1 = new Frame(payload);
            //Console.ReadKey();

            //Frame f1 = new Frame(new byte[] { 212, 2, 3, 4, 5, 6, 7, 8, 9, 10, 23, 56, 68, 154, 198, 5, 2, 54, 5, 2 });
            //Ethernet ef1 = new Ethernet(new byte[] { 1,2,3,34,4,45,55,5,6 });
            //Ethernet ef2 = new Ethernet(f1);
            //Console.WriteLine(ef2.Source.ToString());

            //byte[] ba = new byte[] { 0x00, 0x4c};

            //Array.Reverse(ba);
            ////Console.WriteLine(BitConverter.ToUInt16(ba,0));
            ////Console.WriteLine(Convert.ToUInt16(hex >> 4));
            ////Console.WriteLine(Convert.ToUInt16(hex & 0x15));

            //string hex = "c83a351bdd309c4e3659c5dc0800450005ba79ff000080112e64c0a8006731dc99e42885222905a6ad29bb5c7051493faedb59c83e97dcf5e5c2fd73ee5018a98f89ac537ac111d7d047ba4b62db4c7121a28a521b849fff8fcd98f8b554ea6d51f43e615c6239cdd5582905fc2f6531fa1e4e8f7c0ffdfdda117d7342c66b93d1af7f7a1f386f73549e32a38da8d1ddaec876435d93aa377f7fe9d214627f343ac25c6efbc2a5b3780588c87c6ebc43a0a9b14695c7d205f879f769a4a8674137a3151d5bcd8c8684c427851972f9e3254641ae7c3bf52d1672e8fae158ef4d76ee1652c2ca78175b36fa2f90edc785b9c3906d0abe1d2d3a51c2179640be30e34277a21c3cfdae1ff60f9dd63236cb79ec5148a82befb155601ae6c8f2943b46bc213827958e810bf36e804951444226b2034046dbf92ddf15da3eaf25f548c2025f1565ba3c192bbcdf5b2e7b077cbfdacd9d5983e7cfdefc08b5e7248fa0646844451d40add732d6fde2f3c967b6c5442e8c6bd9b76424bfe860716f7017a871144bc0d959dc9a5e404aed717e3b90099820002204ff444817c0aa46956a26a232e8852b3cd223d8783745d813dac6f1795b7d5e5ce42de80b0112bf1056be6428f77879e4c34df2f933f1a8177223ad48b4bd4e692594f4cfce58033e8b5ea4b7283b1531022789d2ed3a4272bdb876da4af4f51f76d8e95bb7276b85f516318e0700df6e52f67d1ed27d33075588455163f17792790f8d9803aecf1367dacee1d682fd885bdda10e946add063e36aaa4b1297806a9baf5d79f6aabb1d97a283f0aa34b8f0154480f8eb6a007e969c3425ce88edb7fde1aadcc6a4c8232aeb2d53c88cb4c990e0bc722316c7d0b26ac4a8f120f18842393eb9df2bd9bd0127e7af9146822cfffe8563f0159a5619b2b389c1509de8a10d0cb02ba91f6088ef479276c2069de699fc009391a6e1c28313ac1618ffcbb795902e0273d1b3fd41d59b5942d5011cc52733023762ca6c947c3b49f67b9cf4ac083a9d5636ba6d233e2fe062dd039fcdfe9a650789ef5fec7ef91b74399c711f24d26ddc5b7630241f41e354eb74e3fbaebf090b1cd51aed10ac8cc60c0cb866f868a2121ecdb96305f191fca72f43ff13b4d9f79dc1dda1d0f04b6b061c834537a38657d32d361c236d11a140c5a795341aabc0818f658830cbb819c2d705a857f3f13b1bd52973e9f9e24a4ed94db356aac336fd7b28d8a6e1ba2ea05df40a36d2494662ada3902b8107ab0cb17998f56f93e9741a2446caef718a6381c77cf40349175ca5ad16fbf2cf2a41d5a85b6807db1f227763c3af6f71f78dd682006d8e2b810e5684a94d1eabf5d36e274a45e1ec08de4ac58931169044dd48c887c999ff490ca953113d76e27d9cd8c78d0530e36c43a9e79d57c0602109f44a9c1be44174b57a92817682d00ee21ee26314a3bd6aeefc9d47cfeda56f604231e53f14a7243a5ed1e4deed72378ed36b12828fe7d3054d1880e73bacdd341e58fe34ae76d04803d5860ccd74465018d926fc1de11b16a72d625de9eebb917e4c95912ca729dd334c6b0a161b7de621884d4738fad1dd9dd5aff466285f7f8eec5ce35a6c9cd76f176d28c66e5c1557ed7d5d9d0cef4d2298be34bd4aed6f6324fd77c04b418402fb8e9e9a4ba915092c4f7ee900cf360f79f9988f860ddb3a2cfe559674eb0760f7e0d06c67716d71e2928ad8b49f86be737b365aa6e1da2f6b87438d875a2155808d6968d2f4352d63c25ec5d32325a3b2a52c263e7e03022fa988b8a6b7a4e26f3077ba9e35ad0b3a09c93e29e56ed4ad3377d5ab5164b06a5812b5c261dc50b081a053732ea76c452df82840f0855b9bb42f5c14573f83674371939a13b0ec0d211027229356a62507cdf21283917258456580a0830886931446b72a1e2d24a50059953d22d91c3dab9dbf0610efe2c85ed19b6d6ad6dd9c343e111424c399fd27377b913590c49c9d3733c555742a3e4f58d57b6106d8053418c738829f10043b4e3e3a0a28b0af89bfabe939160702e7115bdaaf427caef24e3448d44d4eeb0698b65e4871559";

            //int length = hex.Length / 2;

            //byte[] byteArr=new byte[length];

            //for (int i = 0; i < length; i++)
            //{
            //    byteArr[i] = Convert.ToByte(hex.Substring(2 * i, 2), 16);
            //}

            ////Console.WriteLine(byteArr.ToString("X2"));

            //byte[] test = new byte[] {0x79,0xff };
            //Console.WriteLine(BitConverter.ToUInt16(test.Reverse(),0));

            Console.ReadKey();

        }

        static void Program_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("共捕获到个数据包");
        }
    }
}

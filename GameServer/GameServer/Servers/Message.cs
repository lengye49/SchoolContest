using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Common;

namespace GameServer
{
    class Message
    {
        private byte[] data;

        public Message()
        {
            data = new byte[3072];
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public void ReadMessage(int newDataCount,Action<RequestCode,ActionCode,string> processMsgCallBack)
        {
            Console.WriteLine("尝试解读信息...");

            if (newDataCount < 8)
            {
                Console.WriteLine("信息不完整！");
                return;
            }

            try
            {
                RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 0);
                Console.WriteLine("RequestCode:" + requestCode);
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                Console.WriteLine("ActionCode:" + actionCode);
                string s = System.Text.Encoding.UTF8.GetString(data, 8, newDataCount - 8);
                Console.WriteLine("Message:" + s);
                processMsgCallBack(requestCode, actionCode, s);
            }
            catch (Exception e)
            {
                Console.WriteLine("接收到的数据有问题：" + e);
            }
            finally {
                data = new byte[3072];
            }
        }

        public static byte[] PackData(RequestCode requestCode,ActionCode actionCode, string data) {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            requestCodeBytes = requestCodeBytes.Concat(actionCodeBytes).ToArray().Concat(dataBytes).ToArray();
            return requestCodeBytes;
        }
    }
}

﻿using System;
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
            if (newDataCount < 8)
            {
                return;
            }

            try
            {
                RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 0);
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                string s = System.Text.Encoding.UTF8.GetString(data, 8, newDataCount - 8);
                processMsgCallBack(requestCode, actionCode, s);
            }
            catch (Exception e)
            {
                Console.WriteLine("Message Error!\n" + e);
            }
            finally {
                data = new byte[3072];
            }
        }

        public static byte[] PackData(RequestCode requestCode,ActionCode actionCode, string data) {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataCount = requestCodeBytes.Length + actionCodeBytes.Length + dataBytes.Length;
            byte[] msg = BitConverter.GetBytes(dataCount);
            msg = msg.Concat(requestCodeBytes).ToArray().Concat(actionCodeBytes).ToArray().Concat(dataBytes).ToArray();
            return msg;
        }
    }
}

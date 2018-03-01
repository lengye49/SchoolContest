using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Common;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string ipStr = "127.0.0.1";
            int port = 7457;

		    Socket clientSocket;
            byte[] msgBytes;
            string msgSend;
            //byte[] msgRec;
            Message msg;

            while (true)
            {
                msg = new Message();
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Connecting...");
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipStr), port));
                Console.WriteLine("Connecting Accepted!");
                msgSend = Console.ReadLine();
                switch (msgSend) {
                    case "1":
                        Console.WriteLine("尝试注册！");
                        msgBytes = Message.PackData(RequestCode.Register, ActionCode.Register, "1");
                        break;
                    case "2":
                        Console.WriteLine("尝试获取个人排名！");
                        Random r = new Random();
                        int id = 0;//r.Next(1, 99);
                        User u = new User(id, "honey", 1, 9, 9800);
                        msgBytes = Message.PackData(RequestCode.Game, ActionCode.GetPersonalResult, u.GetUserStr());
                        break;
                    case "3":
                        Console.WriteLine("尝试获取总排名！");
                        msgBytes = Message.PackData(RequestCode.Game, ActionCode.GetTotalRank, "2");
                        break;
                    default:
                        Console.WriteLine("你输入的信息有误！默认注册");
                        msgBytes = Message.PackData(RequestCode.Register, ActionCode.None, "1");
                        break;
                }
                Console.WriteLine(msgBytes);
                Console.WriteLine("发送消息...");

                clientSocket.Send(msgBytes);
                int count = clientSocket.Receive(msg.Data);
                msg.ReadMessage(count);
                clientSocket.Close();
            }


	}
        static void AcceptCallBack(IAsyncResult ar) {

        }

    }
}

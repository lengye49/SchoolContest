using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerAsync();
            Console.ReadKey();
        }

        static Message msg = new Message();

        static byte[] dataBuffer = new byte[1024];
        static void ServerAsync() {
            string ipStr = "127.0.0.1";
            int port = 7457;

            //绑定服务器的ip和端口
            Console.WriteLine("Binding ServerSocket...");
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipStr);
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
            serverSocket.Bind(ipEndPoint);


            //监听接受客户端链接
            Console.WriteLine("Listening Client...");
            serverSocket.Listen(0);//0表示监听队列上限

            Console.WriteLine("Accepting...");
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }

        static void AcceptCallBack(IAsyncResult ar) {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);
            //向客户端发送一条消息
            Console.WriteLine("Sending Message...");
            string msgSent = "黄河黄河，收到请回答！";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msgSent);
            clientSocket.Send(data);

            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.LeftSize, SocketFlags.None, AnalysisData, clientSocket);

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        static void AnalysisData(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                msg.UpdateStartIndex(count);

                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                //string msgRec = Encoding.UTF8.GetString(dataBuffer, 0, count);
                //Console.WriteLine(msgRec);
                msg.ReadMessage();


                //循环调用
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.LeftSize, SocketFlags.None, AnalysisData, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                    clientSocket.Close();
            }
            finally
            {
                
            }
        }

        void ServerSync() {
            string ipStr = "192.168.1.100";
            int port = 88;

            //绑定服务器的ip和端口
            Console.WriteLine("Binding ServerSocket...");
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipStr);
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
            serverSocket.Bind(ipEndPoint);


            //监听接受客户端链接
            Console.WriteLine("Listening Client...");
            serverSocket.Listen(0);//0表示监听队列上限

            Console.WriteLine("Accepting...");
            Socket clientSocket = serverSocket.Accept();

            //向客户端发送一条消息
            Console.WriteLine("Sending Message...");
            string msg = "黄河黄河，收到请回答！";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);

            //接受客户端的一条消息
            Console.WriteLine("Receiving Message...");
            byte[] dataBuffer = new byte[1024];
            int count = clientSocket.Receive(dataBuffer);
            string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine(msgReceive);

            Console.WriteLine("Closing...");
            Console.ReadKey();
            clientSocket.Close();
            serverSocket.Close();
        }
    }
}

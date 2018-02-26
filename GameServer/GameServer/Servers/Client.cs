using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Net.Sockets;
using Common;

namespace GameServer.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;

        private Message msg; 
        public Client(){}
        public Client(Socket clientSocket, Server server) {
            this.clientSocket = clientSocket;
            this.server = server;
            msg = new Message();
        }

        public void StartClient() {
            clientSocket.BeginReceive(msg.Data, 0, msg.Data.Length, SocketFlags.None, ReceiveCallBack, null);
        }

        private void OnProgressMessage(RequestCode requestCode, ActionCode actionCode, string data) {
            Console.WriteLine("尝试处理信息...");
            server.HandleRequest(requestCode, actionCode, data, this);
        }
        void ReceiveCallBack(IAsyncResult ar) {
            try
            {
                int count = clientSocket.EndReceive(ar);
                Console.WriteLine("count = " + count);
                msg.ReadMessage(count, OnProgressMessage);
                Close();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                Close();
            }
        }

        void Close() {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }

        public void Send(RequestCode requestCode,ActionCode actionCode, string data) {
            byte[] bytes = Message.PackData(requestCode,actionCode, data);
            clientSocket.Send(bytes);
        }
    }
}

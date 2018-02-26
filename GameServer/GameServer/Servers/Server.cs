using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{ 
    class Server
    {
        IPEndPoint ipEndPoint;
        Socket serverSocket;
        List<Client> clientList;
        ControllerManager controllerManager;

        public Server(){}
        public Server(string ipStr, int port) {
            SetIpEndPoint(ipStr, port);
            controllerManager = new ControllerManager(this);
            clientList = new List<Client>();
        }

        public ControllerManager Controller{
            get { return this.controllerManager; }
        }

        public void SetIpEndPoint(string ipStr, int port) {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void StartServer() {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            Console.WriteLine("服务器已开启!!\n");
            serverSocket.Listen(0);
            Console.WriteLine("开始监听消息...\n");
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        void AcceptCallBack(IAsyncResult ar) {
            Console.WriteLine("接受到新消息！");
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.StartClient();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        public void RemoveClient(Client client) {
            lock (client)
            {
                clientList.Remove(client);
            }
        }

        public void SendResponse(Client client,RequestCode requestCode,ActionCode actionCode, string data) {
            Console.WriteLine("正在尝试回复客户端...");
            client.Send(requestCode,actionCode, data);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
    }
}

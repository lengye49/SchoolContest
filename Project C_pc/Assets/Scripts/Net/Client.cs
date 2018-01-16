using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class Client:MonoBehaviour
{
    void Start(){
        
    }

    void ConnectSever(){
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(Configs.Ip), Configs.Port);
        Debug.Log("Connecting...");

        string msg = "Account";
        clientSocket.Send(Message.GetBytes(msg));
        Debug.Log("Requesting Account Id...");

        byte[] dataBuffer = new byte[1024];
        int count = clientSocket.Receive(dataBuffer);
        string msgReceive = Message.GetMsg(dataBuffer, count);
        Debug.Log("Account Received: " + msgReceive);
    }

}



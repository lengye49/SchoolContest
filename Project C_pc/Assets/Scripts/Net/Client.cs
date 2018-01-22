using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;

public class Client
{

    private Socket clientSocket;
    private Message msg;

    int ConnectSever(){
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(Configs.Ip), Configs.Port);
        Debug.Log("Connecting...");
        try{
            clientSocket.Connect(ipEndPoint);
            return 1;
        }catch(Exception e){
            Debug.Log("无法链接服务器，请检查网络！" + e);
            return 0;
        }
    }

    public int GetRemoteService(RequestCode requestCode,string data){
        int netState = ConnectSever();
        if (netState > 0)
        {
            SendRequest(requestCode, data);
            StartListening();
        }
        else
        {
            Debug.Log("无法链接服务器，请检查网络！");  
        }
        return netState;
    }

    void SendRequest(RequestCode requestCode,string data){
        byte[] msg = Message.PackData(requestCode, data);
        clientSocket.Send(msg);
    }

    void StartListening(){
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.LeftSize, SocketFlags.None, ReceiveCallBack, null);
    }

    void ReceiveCallBack(IAsyncResult ar){
        try{
            int count = clientSocket.EndReceive(ar);
            if(count==0){
                CloseClient();
            }
            msg.ReadMessage(count,HandleMessage);
        }catch(Exception e){
            Debug.Log("Can Not Handle Received Message!" + e);
            CloseClient();
        }
    }

    void HandleMessage(RequestCode requestCode,string data){
        
    }

    void CloseClient(){
        if (clientSocket != null)
        {
            clientSocket.Close();
        }
    }



}



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

	~Client(){
		CloseClient ();
	}

    public void GetRemoteService(RequestCode requestCode,ActionCode actionCode,string data){
        int netState = ConnectSever();
        if (netState > 0)
        {
			SendRequest (requestCode, actionCode, data);
            StartListening();
        }
        else
        {
            Debug.Log("无法链接服务器，请检查网络！");  
			return;
        }
    }

    void SendRequest(RequestCode requestCode,ActionCode actionCode, string data){
		byte[] msg = Message.PackData (requestCode, actionCode, data);
        clientSocket.Send(msg);
    }

    void StartListening(){
		ViewManager.isLoading = true;
		clientSocket.BeginReceive (msg.Data, 0, 2048, SocketFlags.None, ReceiveCallBack, null);
    }

    void ReceiveCallBack(IAsyncResult ar){
        try{
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count,HandleMessage);

        }catch(Exception e){
            Debug.Log("Can Not Handle Received Message!" + e);
        }finally{
			CloseClient ();
			ViewManager.isLoading=false;
		}
    }


    void HandleMessage(RequestCode requestCode,ActionCode actionCode, string data){
		switch (requestCode) {
			case RequestCode.Register:
				int id = 0;
				try {
					id = int.Parse (data);
					DataManager.AccountId = id;
				} catch (Exception e) {
					Debug.Log ("无法获取id" + e + "自动重试..");
				}
				break;
//			case RequestCode.Rank:
//				int rank = 0;
//				int rate = 0;
//				try {
//					string[] s = data.Split (',');
//					rank = int.Parse (s [0]);
//					rate = int.Parse (s [1]);
//					DataManager.OnlineRank = rank;
//					DataManager.OnlineRate = rate;
//				} catch (Exception e) {
//					Debug.Log ("无法获得排名" + e);
//				}
//				break;
//			case RequestCode.PlaceArea:
//				DataManager.PlaceAreaRank = data;
//				break;
//			case RequestCode.RankList:
//				DataManager.TotalRank = data;
////				User[] ranks = new User[100];
////				try {
////					string[] s = data.Split (';');
////					for (int i = 0; i < ranks.Length; i++) {
////						if (i < s.Length) {
////							string[] userStr = s [i].Split (',');
////							ranks [i].name = userStr [0];
////							ranks [i].place = int.Parse (userStr [1]);
////							ranks [i].school = int.Parse (userStr [2]);
////							ranks [i].level = int.Parse (userStr [3]);
////							ranks [i].score = int.Parse (userStr [4]);
////						}
////					}
////				} catch (Exception e) {
////					Debug.Log ("获取排名列表失败！\n" + e);
////				}
//				break;
			default:
				Debug.Log ("没有找到RequestCode!");
				break;
		}
	}

    void CloseClient(){
        if (clientSocket != null)
        {
            clientSocket.Close();
        }
    }
        
}



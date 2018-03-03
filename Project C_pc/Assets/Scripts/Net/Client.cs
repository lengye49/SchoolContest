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
        try{
            clientSocket.Connect(ipEndPoint);
            return 1;
        }catch(Exception e){
			Debug.Log (e);
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
			Warning.ShowShortWarning (0, "当前无法链接服务器，请检查网络！", Vector3.zero,false);
			return;
        }
    }

    void SendRequest(RequestCode requestCode,ActionCode actionCode, string data){
		byte[] msg = Message.PackData (requestCode, actionCode, data);
        clientSocket.Send(msg);
    }

    void StartListening(){
		ViewManager.isLoading = true;
		clientSocket.BeginReceive (msg.Data, 0, msg.Data.Length, SocketFlags.None, ReceiveCallBack, null);
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
        if(requestCode == RequestCode.Register){
            int id = 0;
            try {
                id = int.Parse (data);
                DataManager.AccountId = id;
            } catch (Exception e) {
				Debug.Log (e);
				Warning.ShowShortWarning (0, "未能验明正身，将在稍后自动验证。",Vector3.zero,false);
            }
        }else if(requestCode == RequestCode.Game){
            if(actionCode == ActionCode.GetPersonalResult){
                try {
					string[] s= data.Split(',');
					if(DataManager.AccountId==0)
						DataManager.AccountId=int.Parse(s[0]);

					int rank = int.Parse(s[1]);
					string msg="";
					if(rank<=0){
						msg="未进入仙路排行，\n道友请重新来过！";
						Warning.ShowShortWarning(2,msg,Vector3.zero,false);
						//处理没有排名的情况
					}else {
						msg="恭喜道友登上仙路排行榜，\n名列仙榜第"+rank+"位";
						Warning.ShowShortWarning(2,msg,Vector3.zero,false);
					}

                } catch (Exception e) {
                    Debug.Log ("无法获得排名!\n" + e);
                }
            }else if(actionCode == ActionCode.GetTotalRank){
                DataManager.SetTotalRank(data);
            }else{
                Debug.Log("没有找到actionCode:"+actionCode);
            }
        }else{
            Debug.Log("没有找到RequestCode:"+requestCode);
        }
    }

    void CloseClient(){
        if (clientSocket != null)
        {
            clientSocket.Close();
        }
    }
        
}



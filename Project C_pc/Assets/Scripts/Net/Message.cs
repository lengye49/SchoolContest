using System;
using System.Text;
using System.Linq;
using UnityEngine;
public class Message
{
    private byte[] data = new byte[3072];

    public Message() {
        data = new byte[3072];
    }

    public byte[] Data{
        get{ return data;}
		set{ data = value;}
    }

    public void ReadMessage(int newDataCount,Action<RequestCode,ActionCode,string> HandleMessage){

        if (newDataCount < 8) {
			Debug.Log("信息不完整！");
            return;
        }
        RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 0);
        ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
        string s = Encoding.UTF8.GetString(data, 8, newDataCount - 8);
		Debug.Log("Request:" + requestCode + ",Action:" + actionCode + ",Message:" + s);
		HandleMessage (requestCode, actionCode, s);
    }

    public static byte[] PackData(RequestCode requestCode,ActionCode actionCode, string data){
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        requestCodeBytes = requestCodeBytes.Concat(actionCodeBytes).ToArray().Concat(dataBytes).ToArray();           
        return requestCodeBytes;
    }
}



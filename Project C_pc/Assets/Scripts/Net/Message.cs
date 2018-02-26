using System;

using System.Linq;

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
            Console.WriteLine("信息不完整！");
            return;
        }

        RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 0);
        ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
        string s = Encoding.UTF8.GetString(data, 8, newDataCount - 8);
        Console.WriteLine("Request:" + requestCode + ",Action:" + actionCode + ",Message:" + s);
    }

    public static byte[] PackData(RequestCode request,ActionCode action, string data){
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        requestCodeBytes = requestCodeBytes.Concat(actionCodeBytes).ToArray().Concat(dataBytes).ToArray();           
        return requestCodeBytes;
    }
}



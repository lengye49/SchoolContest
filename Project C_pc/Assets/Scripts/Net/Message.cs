using System;

using System.Linq;

public class Message
{

    private byte[] data = new byte[2048];

    public byte[] Data{
        get{ return data;}
		set{ data = value;}
    }

    public void ReadMessage(int newDataCount,Action<RequestCode,ActionCode,string> HandleMessage){

        while (true)
        {
            //数据长度
            int count = BitConverter.ToInt32(data, 0);

            RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
			ActionCode actionCode = (ActionCode)BitConverter.ToInt32 (data, 8);
			string s = System.Text.Encoding.UTF8.GetString (data, 12, count - 12);
			data = new byte[2048];
			HandleMessage (requestCode, actionCode, s);
        }
    }

    public static byte[] PackData(RequestCode request,ActionCode action, string data){
        byte[] reqBytes = BitConverter.GetBytes((int)request);
		byte[] actBytes = BitConverter.GetBytes ((int)action);
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
		int dataLength = reqBytes.Length + actBytes.Length + dataBytes.Length;
        byte[] lengthBytes = BitConverter.GetBytes(dataLength);

		return lengthBytes.Concat (reqBytes).ToArray<byte> ()
			.Concat (actBytes).ToArray<byte> ()
			.Concat (dataBytes).ToArray<byte> ();
    }
}



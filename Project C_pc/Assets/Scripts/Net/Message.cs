using System;

using System.Linq;

public class Message
{

    private byte[] data = new byte[1024];
    private int startIndex = 0;

    public byte[] Data{
        get{ return data;}
    }

    public int StartIndex{
        get{ return startIndex;}
    }

    public int LeftSize{
        get{ return LeftSize;}
    }

    public void ReadMessage(int newDataCount,Action<RequestCode,string> HandleMessage){
        
    }

    public static byte[] PackData(RequestCode request,string data){
        byte[] reqBytes = BitConverter.GetBytes((int)request);
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
        int dataLength = reqBytes.Length + dataBytes.Length;
        byte[] lengthBytes = BitConverter.GetBytes(dataLength);

        return lengthBytes.Concat(reqBytes).ToArray<byte>()
            .Concat(dataBytes).ToArray<byte>();
    }


    public static byte[] GetBytes(string data){
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
        byte[] countBytes = BitConverter.GetBytes(dataBytes.Length);
        byte[] newBytes = countBytes.Concat(dataBytes).ToArray();
        return newBytes;
    }

    public static string GetMsg(byte[] b, int count){
        string msg = System.Text.Encoding.UTF8.GetString(b, 0, count);
        return msg;
    }
}



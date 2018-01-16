using System;

using System.Linq;

public class Message
{
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



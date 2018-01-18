using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TCPClient
{
    private static TcpClient client;
    private const int buffer_size = 1024 * 1024;
    public void Init()
    {
        client = new TcpClient();
        client.ReceiveBufferSize = buffer_size;
        try
        {
            client.Connect(IPAddress.Parse(Network.player.ipAddress), 6001);
            OpenDecode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private static Thread receiveThread = null;
    private void OpenDecode()
    {
        receiveThread = new Thread(ReciveMsg);
        receiveThread.Start();
        receiveThread.IsBackground = true;
    }

    //接收服务器数据
    static void ReciveMsg()
    {
        NetworkStream receiveStream = client.GetStream();
       
        while (client.Connected)
        {
            if (!receiveStream.CanRead)
            {
                continue;
            }
            else
            {
                if (receiveStream.DataAvailable)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int length = receiveStream.Read(buffer, 0, buffer_size);//接收数据报
                    ReadFrameData(buffer, length);
                }
            }
        }
    }

    private const int dataSize = 10240;
    private const int totalSize = 10252;
    static List<PackData> packlist = new List<PackData>();

    //read frame data
    public static void ReadFrameData(byte[] data, int length)
    {
        packlist.Clear();
        int curpos = 0;
        byte[] onepack = new byte[totalSize];

        while (curpos < data.Length && curpos < length && (curpos + totalSize) <= data.Length && (curpos + totalSize) <= length)
        {
            Array.Copy(data, curpos, onepack, 0, totalSize);
            packlist.Add(ReadOnePack(onepack));
            curpos += totalSize;
        }

        JoinPackData(packlist);
    }

    //read one pack
    private static PackData ReadOnePack(byte[] data)
    {
        byte[] pack_data = new byte[dataSize];
        PackData pack = new PackData();
        pack.datasize = System.BitConverter.ToInt32(data, 0);
        pack.cursize = System.BitConverter.ToInt32(data, 4);
        pack.last_pak = System.BitConverter.ToInt32(data, 8);
        Array.Copy(data, 12, pack_data, 0, dataSize);
        pack.data = pack_data;

        return pack;
    }

    static byte[] frameBuffer = null;
    static byte[] oneFrameBuffer = null;
    public static byte[] curFrameBuffer
    {
        get
        {
            return oneFrameBuffer;
        }
    }
    static int cursize = 0;
    //joint pack list
    private static void JoinPackData(List<PackData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //Debuger.Log("get one pack");
            PackData data = list[i];
            //Debug.LogError(string.Format("datasize:{0}cursize:{1}last_pak:{2}datalength:{3}", data.datasize, data.cursize, data.last_pak, data.data.Length));
            if (frameBuffer == null || frameBuffer.Length != data.datasize)
            {
                frameBuffer = new byte[data.datasize];
                //Debug.LogError("buffersize:" + data.datasize);
            }
            //若为最後一片
            if (data.last_pak == 1)
            {
                //成功获取一个数据包
                if ((cursize + data.cursize) == data.datasize)
                {
                    //Debug.LogError("cursize:" + cursize);
                    Array.Copy(data.data, 0, frameBuffer, cursize, data.cursize);

                    //Debuger.Log("get one frame success!!!!");

                    if (oneFrameBuffer == null || oneFrameBuffer.Length != data.datasize)
                    {
                        oneFrameBuffer = new byte[data.datasize];
                    }

                    lock (oneFrameBuffer)
                    {
                        frameBuffer.CopyTo(oneFrameBuffer, 0);
                        cursize = 0;
                        CreateColors(oneFrameBuffer);
                        BigScreen.flag = true;
                        //send to jni
                        //YUVDeCoder.Decode(oneFrameBuffer);
                    }
                }
                else
                {
                    //丢弃该帧
                    cursize = 0;
                }
            }
            else if (cursize < data.datasize && (cursize + data.cursize) <= data.datasize)
            {
                //往下拼接
                data.data.CopyTo(frameBuffer, cursize);
                cursize += data.cursize;
                //Debug.LogError("往下拼接:cursize:" + cursize);
            }
            else
            {
                //丢弃该帧
                cursize = 0;
            }
        }
    }

    public static Color32[] colors = null;
    private static void CreateColors(byte[] rgb)
    {
        int length = rgb.Length / 3;
        if (colors == null)
        {
            colors = new Color32[length];
        }
        for (int i = 0, j = 0; i < rgb.Length; i = i + 3, j++)
        {
            colors[j] = new Color32(rgb[i + 2], rgb[i + 1], rgb[i], 255);
        }
    }

    public static byte[] GetCurFrameBuffer()
    {
        return oneFrameBuffer;
    }

    public static void StopClient()
    {
        //Debug.LogError("客户端已关闭");
        client.Close();
    }
}

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Client
{
    Socket client;
    const int dataSizePerFrame = 1600 * 10;
    static int totalSizePerFrame
    {
        get
        {
            return dataSizePerFrame + 12;
        }
    }
    static byte[] frameBuffer = null;
    static byte[] oneFrameBuffer = null;
    static int cursize = 0;
    static Thread receiveThread = null;
    ObjectPool<PackData> packDataPool;
    public static Color32[] colors = null;

    public void Init()
    {
        int pool_size = 2 * BigScreen.width * BigScreen.height * 3 / dataSizePerFrame;
        packDataPool = new ObjectPool<PackData>(pool_size, dataSizePerFrame);

        client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        client.EnableBroadcast = true;
        client.ReceiveBufferSize = 1024 * 1024;
        try
        {
            client.Bind(new IPEndPoint(IPAddress.Parse(Network.player.ipAddress), 6000));

            receiveThread = new Thread(ReciveMsg);
            receiveThread.Start();
            receiveThread.IsBackground = true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    //接收服务器数据
    private void ReciveMsg()
    {
        int buffer_size = totalSizePerFrame * 5;
        byte[] buffer = new byte[buffer_size];
        byte[] onepack = new byte[totalSizePerFrame];
        List<PackData> packlist = new List<PackData>();
        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
            int length = client.ReceiveFrom(buffer, ref point);//接收数据报

            //ReadFrameData(buffer, length);

            packlist.Clear();
            int curpos = 0;

            while ((curpos + totalSizePerFrame) <= buffer_size && (curpos + totalSizePerFrame) <= length)
            {
                Array.Copy(buffer, curpos, onepack, 0, totalSizePerFrame);
                packlist.Add(ReadOnePack(onepack));
                curpos += totalSizePerFrame;
            }
            JoinPackData(packlist);
        }
    }

    //read frame data
    //private byte[] onepack = new byte[totalSizePerFrame];
    //public void ReadFrameData(byte[] data, int length)
    //{
    //    packlist.Clear();
    //    int curpos = 0;

    //    while (curpos < data.Length && curpos < length && (curpos + totalSizePerFrame) <= data.Length && (curpos + totalSizePerFrame) <= length)
    //    {
    //        Array.Copy(data, curpos, onepack, 0, totalSizePerFrame);
    //        packlist.Add(ReadOnePack(onepack));
    //        curpos += totalSizePerFrame;
    //    }
    //    JoinPackData(packlist);
    //}

    //read one pack
    private PackData ReadOnePack(byte[] data)
    {
        PackData pack = packDataPool.Create();
        pack.datasize = System.BitConverter.ToInt32(data, 0);
        pack.cursize = System.BitConverter.ToInt32(data, 4);
        pack.last_pak = System.BitConverter.ToInt32(data, 8);
        Array.Copy(data, 12, pack.data, 0, dataSizePerFrame);
        return pack;
    }

    //joint pack list
    private void JoinPackData(List<PackData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            PackData data = list[i];
            if (frameBuffer == null || frameBuffer.Length != data.datasize)
            {
                frameBuffer = new byte[data.datasize];
            }
            //若为最後一片
            if (data.last_pak == 1)
            {
                //成功获取一个数据包
                if ((cursize + data.cursize) == data.datasize)
                {
                    Array.Copy(data.data, 0, frameBuffer, cursize, data.cursize);

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
            }
            else
            {
                //丢弃该帧
                cursize = 0;
            }
        }
    }

    private void CreateColors(byte[] rgb)
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

    public byte[] GetCurFrameBuffer()
    {
        return oneFrameBuffer;
    }

    public void StopClient()
    {
        //Debug.LogError("客户端已关闭");
        client.Close();
    }
}

public class PackData : IObjectPoolItem
{
    public int datasize;       //數據大小
    public int cursize;        //當前大小
    public int last_pak;       //是否是最後一片
    public byte[] data;        //帧数据   

    public void init(int data_size)
    {
        data = new byte[data_size];
    }

    public override string ToString()
    {
        //string msg = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(data));
        return string.Format("datasize:{0}cursize:{1}last_pak:{2}", datasize, cursize, last_pak);
    }
}
using MCaptureDemo.TCPServer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

public class TcpServer
{
    private const int max_buff_size = 1600;
    private const string server_ip = "10.2.24.210";
    private const string client_ip = "10.2.24.210";//"10.2.140.13";//
    List<Socket> socConnections = new List<Socket>();
    List<Thread> dictThread = new List<Thread>();

    private TcpClient client = null;
    private TcpListener server = null;

    private Thread threadWatch, threadSend;
    public TcpServer()
    {
        server = new TcpListener(IPAddress.Parse(server_ip), 6001);
        server.Start();
        Console.WriteLine("服务端已经开启");


        threadWatch = new Thread(WatchConnecting);
        threadWatch.IsBackground = true;
        threadWatch.Start();

        threadSend = new Thread(sendMsg);//开启发送消息线程
        threadSend.IsBackground = true;
        threadSend.Start();
    }

    private void WatchConnecting()
    {
        bool error = false;
        while (true)
        {
            try
            {
                client = server.AcceptTcpClient();
                if (client != null)
                {
                    MessageBox.Show("A client has connected!");
                    return;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            finally
            {
                error = true;
            }
            if (error)
            {
                return;
            }
        }
    }

    /// <summary>
    /// 向特定ip的主机的端口发送数据报
    /// </summary>
    private void sendMsg()
    {
        while (client == null)
        {
            Thread.Sleep(500);
        }
        NetworkStream sendStream = client.GetStream();

        PackData packdata = new PackData();
        byte[] buff = new byte[max_buff_size + 12];
        while (true)
        {
            if (BitmapCoder.instance.packDataQueue.Count != 0)
            {
                byte[] bits = BitmapCoder.instance.packDataQueue.Peek();
                int offset = bits.Length % max_buff_size;
                int count = bits.Length / max_buff_size + (offset == 0 ? 0 : 1);
                for (int i = 0; i < count; i++)
                {
                    if (i < count - 1)
                    {
                        packdata.cursize = max_buff_size;
                        packdata.last_pak = 0;
                    }
                    else
                    {
                        packdata.cursize = offset == 0 ? max_buff_size : offset;
                        packdata.last_pak = 1;
                    }
                    packdata.datasize = bits.Length;
                    Array.Copy(bits, i * max_buff_size, packdata.data, 0, packdata.cursize);
                    StructToBytes(packdata, ref buff);
                    sendStream.Write(buff, 0, buff.Length);
                }
                BitmapCoder.instance.packDataQueue.Dequeue();
            }
        }
    }

    public class PackData
    {
        public int datasize;       //數據大小
        public int cursize;        //當前大小
        public int last_pak;       //是否是最後一片
        public byte[] data;        //帧数据 

        public PackData()
        {
            data = new byte[max_buff_size];
        }
    }

    //将Byte转换为结构体类型
    private static void StructToBytes(PackData structObj, ref byte[] data)
    {
        byte[] byte1 = BitConverter.GetBytes(structObj.datasize);
        byte[] byte2 = BitConverter.GetBytes(structObj.cursize);
        byte[] byte3 = BitConverter.GetBytes(structObj.last_pak);
        byte[] byte4 = structObj.data;
        byte1.CopyTo(data, 0);
        byte2.CopyTo(data, 4);
        byte3.CopyTo(data, 8);
        byte4.CopyTo(data, 12);
    }
}

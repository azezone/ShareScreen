using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MCaptureDemo.TCPServer
{
    public class Server
    {
        static Socket server;
        private const int buffSizePerFrame = 1600;
        private const string server_ip = "10.2.24.210";
        private const string client_ip1 = "10.2.139.191";//"10.2.139.221"
        private const string client_ip2 = "10.2.156.1";//"10.2.139.221"
        private const string client_broadcast = "255.255.255.255";

        private string clientIP
        {
            get
            {
                return client_ip1;
            }
        }

        public Server()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse(server_ip), 6001));//绑定端口号和IP
            server.SendBufferSize = 1024 * 1024;
            //server.EnableBroadcast = true;

            Thread t2 = new Thread(sendMsg);//开启发送消息线程
            t2.IsBackground = true;
            t2.Start();
        }

        private DateTime lastTime;
        private TimeSpan delttime;
        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        private void sendMsg()
        {
            EndPoint point1 = new IPEndPoint(IPAddress.Parse(clientIP), 6000);
            //EndPoint point2 = new IPEndPoint(IPAddress.Parse(client_ip2), 6000);
            PackData packdata = new PackData();
            byte[] buff = new byte[buffSizePerFrame + 12];
            while (true)
            {
                if (BitmapCoder.instance.packDataQueue.Count != 0)
                {
                    byte[] bits = BitmapCoder.instance.packDataQueue.Peek();
                    int offset = bits.Length % buffSizePerFrame;
                    int count = bits.Length / buffSizePerFrame + (offset == 0 ? 0 : 1);
                    for (int i = 0; i < count; i++)
                    {
                        if (i < count - 1)
                        {
                            packdata.cursize = buffSizePerFrame;
                            packdata.last_pak = 0;
                        }
                        else
                        {
                            packdata.cursize = offset == 0 ? buffSizePerFrame : offset;
                            packdata.last_pak = 1;
                        }
                        packdata.datasize = bits.Length;
                        Array.Copy(bits, i * buffSizePerFrame, packdata.data, 0, packdata.cursize);
                        StructToBytes(packdata, ref buff);
                        server.SendTo(buff, point1);
                        //server.SendTo(buff, point2);
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
                data = new byte[buffSizePerFrame];
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
}
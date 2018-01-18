using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;

namespace MCaptureDemo.TCPServer
{
    public class BitmapCoder
    {
        public Queue<Bitmap> imageQuene = new Queue<Bitmap>();
        public Queue<Bitmap> resizeImageQueue = new Queue<Bitmap>();
        public Queue<byte[]> packDataQueue = new Queue<byte[]>();
        private Thread codeThread = null;
        private Thread resizeThread = null;
        private static BitmapCoder _instance = null;
        public static BitmapCoder instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(BitmapCoder))
                    {
                        _instance = new BitmapCoder();
                        _instance.Start();
                    }
                }
                return _instance;
            }
        }

        private const int max_count = 24;
        private const int width = 120;
        private const int height = 80;

        private void Start()
        {
            if (codeThread == null)
            {
                codeThread = new Thread(CodeBitMap);
                codeThread.Start();
            }

            if (resizeThread == null)
            {
                resizeThread = new Thread(ResizeBitMap);
                resizeThread.Start();
            }
        }

        public void AddImage(Bitmap image)
        {
            if (imageQuene.Count < max_count)
            {
                imageQuene.Enqueue(image);
            }
            //else
            //{
            //    image.Dispose();
            //}
            else
            {
                lock (imageQuene)
                {
                    if (imageQuene.Count >= max_count)
                    {
                        Bitmap img = imageQuene.Dequeue();
                        img.Dispose();
                        imageQuene.Enqueue(image);
                    }
                    else
                    {
                        imageQuene.Enqueue(image);
                    }
                }
            }
        }

        private void ResizeBitMap()
        {
            while (true)
            {
                if (imageQuene.Count != 0)
                {
                    Bitmap bitmap = imageQuene.Dequeue();
                    if (resizeImageQueue.Count <= max_count)
                    {
                        Bitmap b = new Bitmap(bitmap, width, height);
                        resizeImageQueue.Enqueue(b);
                    }
                    bitmap.Dispose();
                    bitmap = null;
                }
            }
        }

        private void CodeBitMap()
        {
            while (true)
            {
                if (resizeImageQueue.Count != 0)
                {
                    Bitmap bitmap = resizeImageQueue.Dequeue();
                    if (packDataQueue.Count <= max_count)
                    {
                        //Bitmap b = new Bitmap(width, height);
                        //Graphics g = Graphics.FromImage(b);
                        //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        //g.DrawImage(bitmap, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                        //g.Dispose();

                        //MemoryStream ms = new MemoryStream();
                        //b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        //byte[] bytes = ms.GetBuffer();
                        //ms.Close();
                        byte[] bytes = Bitmap2Byte(bitmap);
                        packDataQueue.Enqueue(bytes);
                    }
                    bitmap.Dispose();
                    bitmap = null;
                }
            }
        }

        public static byte[] Bitmap2RBG(Bitmap srcBitmap)
        {
            int width = srcBitmap.Width;
            int height = srcBitmap.Height;
            int length = width * height * 3;
            byte[] rgb = new byte[length];

            Rectangle rect = new Rectangle(0, 0, width, height);
            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//PixelFormat.Format24bppRgb
            System.IntPtr srcPtr = srcBmData.Scan0;

            //复制GRB信息到byte数组
            System.Runtime.InteropServices.Marshal.Copy(srcPtr, rgb, 0, length);
            srcBitmap.UnlockBits(srcBmData);

            return rgb;
        }

        Rectangle rect = new Rectangle(0, 0, width, height);
        public byte[] Bitmap2Byte(Bitmap srcBitmap)
        {
            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);//PixelFormat.Format24bppRgb
            System.IntPtr srcPtr = srcBmData.Scan0;
            //将Bitmap对象的信息存放到byte数组中
            int src_bytes = srcBmData.Stride * height;
            byte[] srcValues = new byte[src_bytes];

            //复制GRB信息到byte数组
            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);
            //解锁位图
            srcBitmap.UnlockBits(srcBmData);
            return srcValues;
        }

        public static Bitmap RGB2Gray(Bitmap srcBitmap)
        {
            int wide = srcBitmap.Width;
            int height = srcBitmap.Height;
            Rectangle rect = new Rectangle(0, 0, wide, height);
            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //创建Bitmap
            Bitmap dstBitmap = new Bitmap(wide, height);//这个函数在后面有定义

            BitmapData dstBmData = dstBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
            System.IntPtr srcPtr = srcBmData.Scan0;
            System.IntPtr dstPtr = dstBmData.Scan0;
            //将Bitmap对象的信息存放到byte数组中
            int src_bytes = srcBmData.Stride * height;
            byte[] srcValues = new byte[src_bytes];
            int dst_bytes = dstBmData.Stride * height;
            byte[] dstValues = new byte[dst_bytes];

            //复制GRB信息到byte数组
            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);
            System.Runtime.InteropServices.Marshal.Copy(dstPtr, dstValues, 0, dst_bytes);

            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < wide; j++)
                {
                    //只处理每行中图像像素数据,舍弃未用空间
                    //注意位图结构中RGB按BGR的顺序存储
                    int k = 3 * j;
                    byte temp = (byte)(srcValues[i * srcBmData.Stride + k + 2] * .299
                         + srcValues[i * srcBmData.Stride + k + 1] * .587
                         + srcValues[i * srcBmData.Stride + k] * .114);
                    dstValues[i * dstBmData.Stride + j] = temp;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);
            //解锁位图
            srcBitmap.UnlockBits(srcBmData);
            dstBitmap.UnlockBits(dstBmData);
            return dstBitmap;
        }
    }
}
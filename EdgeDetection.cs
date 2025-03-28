using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;


namespace DetekcijaIvica
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static bool Konvolucija3x3(Bitmap b, KonvMatrica m)
        {
            if (m.Fact == 0) return false;
            Bitmap bSrc = (Bitmap)b.Clone();
            BitmapData bmpD = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmpD.Stride;
            int stride2 = stride * 2;

            System.IntPtr Scan0 = bmpD.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;
                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopL) +
                            (pSrc[5] * m.TopM) +
                            (pSrc[8] * m.TopR) +
                            (pSrc[2 + stride] * m.MidL) +
                            (pSrc[5 + stride] * m.MidM) +
                            (pSrc[8 + stride] * m.MidR) +
                            (pSrc[2 + stride2] * m.BotL) +
                            (pSrc[5 + stride2] * m.BotM) +
                            (pSrc[8 + stride2] * m.BotR))
                            / m.Fac) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopL) +
                            (pSrc[4] * m.TopM) +
                            (pSrc[7] * m.TopR) +
                            (pSrc[1 + stride] * m.MidL) +
                            (pSrc[4 + stride] * m.MidM) +
                            (pSrc[7 + stride] * m.MidR) +
                            (pSrc[1 + stride2] * m.BotL) +
                            (pSrc[4 + stride2] * m.BotM) +
                            (pSrc[7 + stride2] * m.BotR))
                            / m.Fac) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopL) +
                                       (pSrc[3] * m.TopM) +
                                       (pSrc[6] * m.TopR) +
                                       (pSrc[0 + stride] * m.MidL) +
                                       (pSrc[3 + stride] * m.MidM) +
                                       (pSrc[6 + stride] * m.MidR) +
                                       (pSrc[0 + stride2] * m.BotL) +
                                       (pSrc[3 + stride2] * m.BotM) +
                                       (pSrc[6 + stride2] * m.BotR))
                            / m.Fac) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmpD);
            bSrc.UnlockBits(bmSrc);
            return true;
        }

        public static bool RazlikaPixela(Bitmap b, byte nThreshold)
        {
            
            Bitmap b2 = (Bitmap)b.Clone();            
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                           ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = b2.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                             ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                int nPixel = 0, nPixelMax = 0;

                p += stride;
                p2 += stride;

                for (int y = 1; y < b.Height - 1; ++y)
                {
                    p += 3;
                    p2 += 3;

                    for (int x = 3; x < nWidth - 3; ++x)
                    {
                        nPixelMax = Math.Abs(p2[0] - (p2 + stride - 3)[0]);
                        nPixel = Math.Abs(p2[0] - (p2 + stride)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs(p2[0] - (p2 + stride + 3)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs(p2[0] - (p2 - stride)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs(p2[0] - (p2 + stride)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs(p2[0] - (p2 - stride - 3)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs(p2[0] - (p2 - stride)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs(p2[0] - (p2 - stride + 3)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        if (nPixelMax < nThreshold) nPixelMax = 0;

                        p[0] = (byte)nPixelMax;

                        ++p;
                        ++p2;
                    }

                    p += 3 + nOffset;
                    p2 += 3 + nOffset;
                }
            }

            b.UnlockBits(bmData);
            b2.UnlockBits(bmData2);

            return true;

        }
        public static bool ParoviPixela(Bitmap b, byte nThreshold)
        {
            Bitmap b2 = (Bitmap)b.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                           ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = b2.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                             ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                int nPixel = 0, nPixelMax = 0;

                p += stride;
                p2 += stride;

                for (int y = 1; y < b.Height - 1; ++y)
                {
                    p += 3;
                    p2 += 3;

                    for (int x = 3; x < nWidth - 3; ++x)
                    {
                        nPixelMax = Math.Abs((p2 - stride + 3)[0] - (p2 + stride - 3)[0]);
                        nPixel = Math.Abs((p2 + stride + 3)[0] - (p2 - stride - 3)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs((p2 - stride)[0] - (p2 + stride)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs((p2 + 3)[0] - (p2 - 3)[0]);
                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        if (nPixelMax < nThreshold) nPixelMax = 0;

                        p[0] = (byte)nPixelMax;

                        ++p;
                        ++p2;
                    }

                    p += 3 + nOffset;
                    p2 += 3 + nOffset;
                }
            }

            b.UnlockBits(bmData);
            b2.UnlockBits(bmData2);

            return true;

        }
        public static bool IzostriIvice(Bitmap b, byte nThreshold)
        {
            Bitmap b2 = (Bitmap)b.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                           ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = b2.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                            ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                int nPixel = 0, nPixelMax = 0;

                p += stride;
                p2 += stride;

                for (int y = 1; y < b.Height - 1; ++y)
                {
                    p += 3;
                    p2 += 3;

                    for (int x = 3; x < nWidth - 3; ++x)
                    {
                        nPixelMax = Math.Abs((p2 - stride + 3)[0] - (p2 + stride - 3)[0]);

                        nPixel = Math.Abs((p2 + stride + 3)[0] - (p2 - stride - 3)[0]);

                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs((p2 - stride)[0] - (p2 + stride)[0]);

                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        nPixel = Math.Abs((p2 + 3)[0] - (p2 - 3)[0]);

                        if (nPixel > nPixelMax) nPixelMax = nPixel;

                        if (nPixelMax > nThreshold && nPixelMax > p[0])
                            p[0] = (byte)Math.Max(p[0], nPixelMax);

                        ++p;
                        ++p2;
                    }

                    p += nOffset + 3;
                    p2 += nOffset + 3;
                }
            }

            b.UnlockBits(bmData);
            b2.UnlockBits(bmData2);

            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g1 = pictureBox1.CreateGraphics();
            Graphics g2 = pictureBox2.CreateGraphics();
            Graphics g3 = pictureBox3.CreateGraphics();
            Graphics g4 = pictureBox4.CreateGraphics();
            Graphics g5 = pictureBox5.CreateGraphics();
            Graphics g6 = pictureBox6.CreateGraphics();
            Graphics gs = pictureBox7.CreateGraphics();
            Bitmap bmp = new Bitmap(textBox1.Text);
            Bitmap bmp1, bmp2;
            bool p1;
            byte thresh = 0;
            thresh = Convert.ToByte(textBox2.Text);
            KonvMatrica S = new KonvMatrica(1, 2, 1, 0, 0, 0, -1, -2, -1, 1,thresh);
            KonvMatrica P = new KonvMatrica(1, 1, 1, 0, 0, 0, -1, -1, -1, 1, thresh);
            KonvMatrica K = new KonvMatrica(5, 5, 5, -3, -3, -3, -3, -3, -3, 1, thresh);
            g6.DrawImage(bmp, 0, 0);
            bmp2 = (Bitmap)bmp.Clone();
            bmp1 = (Bitmap)bmp.Clone();
            p1 = RazlikaPixela(bmp1, 55);
            if (checkBox1.Checked)
            {
                p1 = IzostriIvice(bmp2, 0);
                bmp1 = bmp2;
            }
            if (radioButton1.Checked)
            {
                p1 = Konvolucija3x3(bmp1, P);
                if (p1 == true) g5.DrawImage(bmp1, 0, 0);
            }
            if (radioButton2.Checked)
            {
                p1 = Konvolucija3x3(bmp1, K);
                if (p1 == true) g5.DrawImage(bmp1, 0, 0);
            }
            if (radioButton3.Checked)
            {
                p1 = Konvolucija3x3(bmp1, S);
                if (p1 == true) g5.DrawImage(bmp1, 0, 0);
            }
            //if(radioButton9.Checked)... --> Promenljiva matrica!

            if (radioButton5.Checked)
            {
                p1 = RazlikaPixela(bmp1, thresh);
                if (p1 == true) g5.DrawImage(bmp1, 0, 0);
            }
            if (radioButton6.Checked)
            {
                p1 = ParoviPixela(bmp1, thresh);
                if (p1 == true) g5.DrawImage(bmp1, 0, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //for (int i = 0; i < dataGridView1.Width; i++)
            //{
            //    for (int j = 0; j < dataGridView1.Height; j++)
            //    {
            //        dataGridView1[j, i].Value = "0";
            //    }
            //}
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();
            pictureBox4.Refresh();
            pictureBox5.Refresh();
            pictureBox6.Refresh();
            pictureBox7.Refresh();
        }

          
       
    }
}

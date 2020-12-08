using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing.Imaging;

using Tesseract;


namespace packing
{
    public partial class Form1 : Form
    {
        private Capture cap;
        private object picloading;
        private Bitmap screenBitmap;
        private Graphics screenGraphics;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cap = new Capture(0);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //tạo mot bien nextframe de chup anh
            Image<Bgr, byte> nextFrame = cap.QueryFrame();
            Image<Gray, byte> grayframe = nextFrame.Convert<Gray, byte>();
            p1.Image = grayframe.ToBitmap(); //hien anh mau
            p2.Image = grayframe.ToBitmap();

        }

        private void btn_Capture_Click(object sender, EventArgs e)
        {
            screenBitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            screenGraphics = Graphics.FromImage(screenBitmap);
            screenGraphics.CopyFromScreen(new Point(this.Left,
            this.Top), Point.Empty, this.Size);
            screenBitmap.Save("D:\\cam\\3.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private string OCR(Bitmap b)
        {
            string res = "";
            try
            {
                using (var engine = new TesseractEngine(@"tessdata", "vie", EngineMode.Default))
                {
                    using (var page = engine.Process(b, PageSegMode.AutoOnly))
                        res = page.GetText();
                }
            }
            catch { }
            return res;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            string result = "";
            Task.Factory.StartNew(() =>
            {
                picloading.BeginInvoke(new Action(() =>
                {
                    picloading.Visible = true;
                }));
                result = OCR((Bitmap)pictureBox1.Image);
                richTextBox1.BeginInvoke(new Action(() =>
                {
                    richTextBox1.Text = result;
                }));
                picloading.BeginInvoke(new Action(() =>
                {
                    picloading.Visible = false;
                }));
            });
        }
    }
}

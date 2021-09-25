using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using NAudio;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace KyoshinMonitor_EEW_Observer_REV_2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(canvas);
            SolidBrush b = new SolidBrush(Color.FromArgb(40, 60, 60));
            g.FillRectangle(b, 0, 0, 245, 47);
            Pen p = new Pen(Color.FromArgb(47, 79, 79), 3);
            g.DrawRectangle(p, 1, 1, 242, 44);
            Font fnt = new Font("Koruri Light", 20);
            Font fnt2 = new Font("Koruri Light", 15);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.DrawString("接続中", fnt, Brushes.White, 3, 2);
            g.Dispose();
            b.Dispose();
            pictureBox2.Image = canvas;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt1 = DateTime.Now;
            var dt = dt1.AddSeconds(-2);
            var url1 = $"";
            var time1 = dt.ToString("yyyyMMdd");
            var time12 = dt.ToString("yyyyMMddHHmmss");

            switch (Properties.Settings.Default.monit_url2)
            {
                case 0:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/jma_s/{time1}/{time12}.jma_s.gif";
                    label3.Text = "地表震度";
                    break;

                case 1:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/acmap_s/{time1}/{time12}.acmap_s.gif";
                    label3.Text = "地表加速度";
                    break;

                case 2:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/vcmap_s/{time1}/{time12}.vcmap_s.gif";
                    label3.Text = "地表速度";
                    break;

                case 3:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/dcmap_s/{time1}/{time12}.dcmap_s.gif";
                    label3.Text = "地表変位";
                    break;

                case 4:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0125_s/{time1}/{time12}.rsp0125_s.gif";
                    label3.Text = "地表0.125Hz応答";
                    break;

                case 5:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0250_s/{time1}/{time12}.rsp0250_s.gif";
                    label3.Text = "地表0.250Hz応答";
                    break;

                case 6:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0500_s/{time1}/{time12}.rsp0500_s.gif";
                    label3.Text = "地表0.500Hz応答";
                    break;

                case 7:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp1000_s/{time1}/{time12}.rsp1000_s.gif";
                    label3.Text = "地表1Hz応答";
                    break;

                case 8:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp2000_s/{time1}/{time12}.rsp2000_s.gif";
                    label3.Text = "地表2Hz応答";
                    break;

                case 9:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp4000_s/{time1}/{time12}.rsp4000_s.gif";
                    label3.Text = "地表4Hz応答";
                    break;

                case 10:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/jma_b/{time1}/{time12}.jma_b.gif";
                    label3.Text = "地中震度";
                    break;

                case 11:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/acmap_b/{time1}/{time12}.acmap_b.gif";
                    label3.Text = "地中加速";
                    break;

                case 12:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/vcmap_b/{time1}/{time12}.vcmap_b.gif";
                    label3.Text = "地中速度";
                    break;

                case 13:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/dcmap_b/{time1}/{time12}.dcmap_b.gif";
                    label3.Text = "地中変位";
                    break;

                case 14:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0125_b/{time1}/{time12}.rsp0125_b.gif";
                    label3.Text = "地中0.125Hz応答";
                    break;

                case 15:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0250_b/{time1}/{time12}.rsp0250_b.gif";
                    label3.Text = "地中0.250Hz応答";
                    break;

                case 16:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0500_b/{time1}/{time12}.rsp0500_b.gif";
                    label3.Text = "地中0.500Hz応答";
                    break;

                case 17:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp1000_b/{time1}/{time12}.rsp1000_b.gif";
                    label3.Text = "地中1Hz応答";
                    break;

                case 18:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp2000_b/{time1}/{time12}.rsp2000_b.gif";
                    label3.Text = "地中2Hz応答";
                    break;

                case 19:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp4000_b/{time1}/{time12}.rsp4000_b.gif";
                    label3.Text = "地中4Hz応答";
                    break;

                case 20:
                    url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/EstShindoImg/eew/{time1}/{time12}.eew.gif";
                    label3.Text = "EEW予測";
                    break;

                case 21:
                    url1 = $"https://www.lmoni.bosai.go.jp/monitor/data/data/map_img/RealTimeImg/abrspmx_s/{time1}/{time12}.abrspmx_s.gif";
                    label3.Text = "長周期地震動";
                    break;
            }


            WebClient wc1 = new WebClient();
            Stream stream1 = wc1.OpenRead(url1);
            Bitmap bitmap1 = new Bitmap(stream1);
            stream1.Close();
            bitmap1.MakeTransparent();
            pictureBox1.BackgroundImage = bitmap1;

            string url = $"http://www.kmoni.bosai.go.jp/data/map_img/PSWaveImg/eew/{time1}/{time12}.eew.gif";
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url);
            Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(canvas);
            Bitmap img = new Bitmap(stream);
            System.Drawing.Imaging.ColorMap[] cms = new System.Drawing.Imaging.ColorMap[]
            {new System.Drawing.Imaging.ColorMap(), new System.Drawing.Imaging.ColorMap()};
            {
                //P波
                cms[0].OldColor = Color.Blue;
                cms[0].NewColor = Color.SpringGreen;
                //S波
                cms[1].OldColor = Color.Red;
                cms[1].NewColor = Color.OrangeRed;
            };
            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
            ia.SetRemapTable(cms);
            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            g.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            pictureBox1.Image = canvas;
            g.Dispose();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var al_flg = Properties.Settings.Default.eew_flg;

            if(al_flg == "f")
            {
                Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Graphics g = Graphics.FromImage(canvas);
                SolidBrush b = new SolidBrush(Color.FromArgb(255, 219, 0));
                g.FillRectangle(b, 0, 0, 245, 47);
                Pen p = new Pen(Color.FromArgb(218, 165, 2), 3);
                g.DrawRectangle(p, 1, 1, 242, 44);
                Font fnt = new Font("Koruri Light", 20);
                Font fnt2 = new Font("Koruri Light", 15);
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.DrawString("緊急地震速報(予報)", fnt, Brushes.Black, 3, 2);
                g.Dispose();
                b.Dispose();
                pictureBox2.Image = canvas;
            }
            else if (al_flg == "w")
            {
                Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Graphics g = Graphics.FromImage(canvas);
                SolidBrush b = new SolidBrush(Color.FromArgb(142, 0, 0));
                g.FillRectangle(b, 0, 0, 245, 47);
                Pen p = new Pen(Color.FromArgb(212, 0, 0), 3);
                g.DrawRectangle(p, 1, 1, 242, 44);
                Font fnt = new Font("Koruri Light", 20);
                Font fnt2 = new Font("Koruri Light", 15);
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.DrawString("緊急地震速報(警報)", fnt, Brushes.White, 3, 2);
                g.Dispose();
                b.Dispose();
                pictureBox2.Image = canvas;
            }
            else if (al_flg == "n")
            {
                Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Graphics g = Graphics.FromImage(canvas);
                SolidBrush b = new SolidBrush(Color.FromArgb(40, 60, 60));
                g.FillRectangle(b, 0, 0, 245, 47);
                Pen p = new Pen(Color.FromArgb(47, 79, 79), 3);
                g.DrawRectangle(p, 1, 1, 242, 44);
                Font fnt = new Font("Koruri Light", 20);
                Font fnt2 = new Font("Koruri Light", 15);
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.DrawString("受信待機中", fnt, Brushes.White, 3, 2);
                g.Dispose();
                b.Dispose();
                pictureBox2.Image = canvas;
            }
        }
    }
}

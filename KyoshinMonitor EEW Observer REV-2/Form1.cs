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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(canvas);
            SolidBrush b = new SolidBrush(Color.FromArgb(40, 60, 60));
            g.FillRectangle(b, 0, 0, 230, 85);
            Pen p = new Pen(Color.FromArgb(47, 79, 79), 3);
            g.DrawRectangle(p, 1, 1, 227, 82);
            Font fnt = new Font("Koruri Light", 20);
            Font fnt2 = new Font("Koruri Light",15);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.DrawString("接続中",fnt,Brushes.White,3,2);
            g.DrawString("Now Loading...", fnt2, Brushes.White, 4, 30);
            g.Dispose();
            b.Dispose();
            pictureBox2.Image = canvas;

            
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            //EEW部分
            {
                DateTime dt = DateTime.Now; //現在時刻の取得(PC時刻より)
                var tm = dt.AddSeconds(-2); //現在時刻から2秒引く(取得失敗を防ぐため)
                var time = tm.ToString("yyyyMMddHHmmss");//時刻形式の指定(西暦/月/日/時/分/秒)

                Font fnt = new Font("Koruri Light", 20);
                Font fnt2 = new Font("Koruri Light", 15);
                Font fnt4 = new Font("Koruri Regular", 12);
                Font fnt5 = new Font("Koruri Regular", 15);
                Font fnt6 = new Font("Koruri Regular", 8);
                Font fnt7 = new Font("Koruri Light", 20);
                Font fnt8 = new Font("Koruri Light", 25);

                Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Graphics g = Graphics.FromImage(canvas);

                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                try
                {
                    var client = new HttpClient();

                    var url = $"http://www.kmoni.bosai.go.jp/webservice/hypo/eew/{time}.json"; //強震モニタURLの指定

                    var json = await client.GetStringAsync(url); //awaitを用いた非同期JSON取得
                    var eew = JsonConvert.DeserializeObject<EEW>(json);//EEWクラスを用いてJSONを解析(デシリアライズ)
                    var rpt_t = eew.report_time;
                    var reg = eew.region_name;
                    var lati = eew.latitude;
                    var lotu = eew.longitude;
                    var canc_flg = eew.is_cancel;
                    var depth_r = eew.depth;
                    var intn = eew.calcintensity;
                    var end_flg = eew.is_final;
                    var rpt_no = eew.report_num;
                    var ori_t = eew.origin_time;
                    var al_flg = eew.alertflg;
                    var eew_flg = eew.result.message;
                    var mag = eew.magunitude;

                    string depth = depth_r.Replace("km", "");

                    if (al_flg == "予報")
                    {
                        Properties.Settings.Default.eew_flg = "f";
                        if (end_flg == "false")
                        {
                            SolidBrush b = new SolidBrush(Color.FromArgb(255, 219, 0));//文字部分
                            Pen p = new Pen(Color.FromArgb(218, 165, 2), 3);//枠1
                            SolidBrush b2 = new SolidBrush(Color.FromArgb(218, 165, 2));//枠2
                            g.FillRectangle(b, 0, 0, 230, 85);
                            g.FillRectangle(b2, 0, 0, 230, 20);
                            g.DrawRectangle(p, 1, 1, 227, 82);
                            g.DrawString($"緊急地震速報(予報) #{rpt_no}", fnt4, Brushes.Black, 0, 0);
                            g.DrawString(reg, fnt5, Brushes.Black, 0, 20);
                            g.DrawString("震度", fnt6, Brushes.Black, 3, 67);
                            g.DrawString(intn, fnt8, Brushes.Black, 25, 42);
                            g.DrawString("M", fnt6, Brushes.Black, 85, 67);
                            g.DrawString(mag, fnt7, Brushes.Black, 95, 50);
                            g.DrawString("深さ", fnt6, Brushes.Black, 140, 67);
                            g.DrawString(depth, fnt7, Brushes.Black, 160, 50);
                            g.DrawString("km", fnt6, Brushes.Black, 205, 67);
                            b.Dispose();
                            b2.Dispose();
                            p.Dispose();
                        }
                        else
                        {
                            SolidBrush b = new SolidBrush(Color.FromArgb(255, 219, 0));//文字部分
                            Pen p = new Pen(Color.FromArgb(218, 165, 2), 3);//枠1
                            SolidBrush b2 = new SolidBrush(Color.FromArgb(218, 165, 2));//枠2
                            g.FillRectangle(b, 0, 0, 230, 85);
                            g.FillRectangle(b2, 0, 0, 230, 20);
                            g.DrawRectangle(p, 1, 1, 227, 82);
                            g.DrawString($"緊急地震速報(予報) #{rpt_no} 最終", fnt4, Brushes.Black, 0, 0);
                            g.DrawString(reg, fnt5, Brushes.Black, 0, 20);
                            g.DrawString("震度", fnt6, Brushes.Black, 3, 67);
                            g.DrawString(intn, fnt8, Brushes.Black, 25, 42);
                            g.DrawString("M", fnt6, Brushes.Black, 85, 67);
                            g.DrawString(mag, fnt7, Brushes.Black, 95, 50);
                            g.DrawString("深さ", fnt6, Brushes.Black, 140, 67);
                            g.DrawString(depth, fnt7, Brushes.Black, 160, 50);
                            g.DrawString("km", fnt6, Brushes.Black, 205, 67);
                            b.Dispose();
                            b2.Dispose();
                            p.Dispose();
                        }

                    }
                    else if (al_flg == "警報")
                    {
                        Properties.Settings.Default.eew_flg = "w";
                        if (end_flg == "false")
                        {
                            SolidBrush b = new SolidBrush(Color.FromArgb(142, 0, 0));//文字部分
                            Pen p = new Pen(Color.FromArgb(212, 0, 0), 3);//枠1
                            SolidBrush b2 = new SolidBrush(Color.FromArgb(212, 0, 0));//枠2
                            g.FillRectangle(b, 0, 0, 230, 85);
                            g.FillRectangle(b2, 0, 0, 230, 20);
                            g.DrawRectangle(p, 1, 1, 227, 82);
                            g.DrawString($"緊急地震速報(警報) #{rpt_no}", fnt4, Brushes.White, 0, 0);
                            g.DrawString(reg, fnt5, Brushes.White, 0, 20);
                            g.DrawString("震度", fnt6, Brushes.White, 3, 67);
                            g.DrawString(intn, fnt8, Brushes.White, 25, 42);
                            g.DrawString("M", fnt6, Brushes.White, 85, 67);
                            g.DrawString(mag, fnt7, Brushes.White, 95, 50);
                            g.DrawString("深さ", fnt6, Brushes.White, 140, 67);
                            g.DrawString(depth, fnt7, Brushes.White, 160, 50);
                            g.DrawString("km", fnt6, Brushes.White, 205, 67);
                            b.Dispose();
                            b2.Dispose();
                            p.Dispose();
                        }
                        else
                        {
                            SolidBrush b = new SolidBrush(Color.FromArgb(142, 0, 0));//文字部分
                            Pen p = new Pen(Color.FromArgb(212, 0, 0), 3);//枠1
                            SolidBrush b2 = new SolidBrush(Color.FromArgb(212, 0, 0));//枠2
                            g.FillRectangle(b, 0, 0, 230, 85);
                            g.FillRectangle(b2, 0, 0, 230, 20);
                            g.DrawRectangle(p, 1, 1, 227, 82);
                            g.DrawString($"緊急地震速報(警報) #{rpt_no} 最終", fnt4, Brushes.White, 0, 0);
                            g.DrawString(reg, fnt5, Brushes.White, 0, 20);
                            g.DrawString("震度", fnt6, Brushes.White, 3, 67);
                            g.DrawString(intn, fnt8, Brushes.White, 25, 42);
                            g.DrawString("M", fnt6, Brushes.White, 85, 67);
                            g.DrawString(mag, fnt7, Brushes.White, 95, 50);
                            g.DrawString("深さ", fnt6, Brushes.White, 140, 67);
                            g.DrawString(depth, fnt7, Brushes.White, 160, 50);
                            g.DrawString("km", fnt6, Brushes.White, 205, 67);
                            b.Dispose();
                            b2.Dispose();
                            p.Dispose();
                        }
                    }
                    else
                    {
                        Properties.Settings.Default.eew_flg = "n";
                        SolidBrush b = new SolidBrush(Color.FromArgb(40, 60, 60));
                        g.FillRectangle(b, 0, 0, 230, 85);
                        Pen p = new Pen(Color.FromArgb(47, 79, 79), 3);
                        g.DrawRectangle(p, 1, 1, 227, 82);
                        g.DrawString("受信待機中", fnt, Brushes.White, 3, 2);
                        g.DrawString("No Data...", fnt2, Brushes.White, 4, 30);
                        b.Dispose();
                        p.Dispose();
                    }
                    g.Dispose();
                    pictureBox2.Image = canvas;
                    
                }
                catch
                {
                    
                    await Task.Delay(10);
                    timer1.Enabled = false;
                    SolidBrush b = new SolidBrush(Color.FromArgb(40, 60, 60));
                    g.FillRectangle(b, 0, 0, 230, 85);
                    Pen p = new Pen(Color.FromArgb(47, 79, 79), 3);
                    g.DrawRectangle(p, 1, 1, 227, 82);
                    g.DrawString("再接続中", fnt, Brushes.White, 3, 2);
                    g.DrawString("Re conecting", fnt2, Brushes.White, 4, 30);
                    b.Dispose();
                    p.Dispose();
                    await Task.Delay(10);
                    timer1.Enabled = true;
                }
                
                label2.Text = dt.ToString("yyyy/MM/dd HH:mm:ss");
                await Task.Delay(100);
            }

        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            await Task.Delay(0);
            //強震モニタ画像部分
            {
                try
                {
                    
                    DateTime dt1 = DateTime.Now;
                    var dt = dt1.AddSeconds(-2);
                    var url1 = $"";
                    var time1 = dt.ToString("yyyyMMdd");
                    var time12 = dt.ToString("yyyyMMddHHmmss");
                    switch (Properties.Settings.Default.monit_url)
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
                    await Task.Delay(100);
                }
                catch(Exception ea)
                {

                }
            }

        }

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void 再起動ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void サブウインドウToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
        }
    }
}

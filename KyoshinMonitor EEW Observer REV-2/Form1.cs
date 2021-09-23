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
                var client = new HttpClient();

                var url = $"http://www.kmoni.bosai.go.jp/webservice/hypo/eew/{time}.json"; //強震モニタURLの指定

                var json = await client.GetStringAsync(url); //awaitを用いた非同期JSON取得
                var eew = JsonConvert.DeserializeObject<EEW>(json);//EEWクラスを用いてJSONを解析(デシリアライズ)
                var rpt_t = eew.report_time;
                var reg = eew.region_name;
                var lati = eew.latitude;
                var lotu = eew.longitude;
                var canc_flg = eew.is_cancel;
                var depth = eew.depth;
                var intn = eew.calcintensity;
                var end_flg = eew.is_final;
                var rpt_no = eew.report_num;
                var ori_t = eew.origin_time;
                var al_flg = "予報";
                var eew_flg = eew.result.message;
                var mag = eew.magunitude;
                
                Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Graphics g = Graphics.FromImage(canvas);
                Font fnt = new Font("Koruri Light", 20);
                Font fnt2 = new Font("Koruri Light", 15);
                Font fnt3 = new Font("Koruri Light", 10);
                Font fnt4 = new Font("Koruri Regular", 12); 
                Font fnt5 = new Font("Koruri Regular", 15);
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                if (al_flg == "予報")
                {
                    SolidBrush b = new SolidBrush(Color.FromArgb(255, 219, 0));//文字部分
                    Pen p = new Pen(Color.FromArgb(218, 165, 2), 3);//枠1
                    SolidBrush b2 = new SolidBrush(Color.FromArgb(218, 165, 2));//枠2
                    g.FillRectangle(b, 0, 0, 230, 85);
                    g.FillRectangle(b2, 0, 0, 230, 20);
                    g.DrawRectangle(p, 1, 1, 227, 82);
                    g.DrawString("緊急地震速報(予報) #188", fnt4, Brushes.White, 0, 0);
                    g.DrawString("山梨県東部・富士五湖", fnt5, Brushes.Black, 0, 20);
                }
                else if (al_flg == "警報")
                {
                    SolidBrush b = new SolidBrush(Color.FromArgb(142, 0, 0));//文字部分
                    Pen p = new Pen(Color.FromArgb(212, 0, 0), 3);//枠1
                    SolidBrush b2 = new SolidBrush(Color.FromArgb(212, 0, 0));//枠2
                    g.FillRectangle(b, 0, 0, 230, 85);
                    g.FillRectangle(b2, 0, 0, 230, 20);
                    g.DrawRectangle(p, 1, 1, 227, 82);
                }
                else
                {
                    SolidBrush b = new SolidBrush(Color.FromArgb(40, 60, 60));
                    g.FillRectangle(b, 0, 0, 230, 85);
                    Pen p = new Pen(Color.FromArgb(47, 79, 79), 3);
                    g.DrawRectangle(p, 1, 1, 227, 82);
                    g.DrawString("受信待機中", fnt, Brushes.White, 3, 2);
                    g.DrawString("No Data...", fnt2, Brushes.White, 4, 30);
                    b.Dispose();
                }
                g.Dispose();
                pictureBox2.Image = canvas;

                label2.Text = dt.ToString("yyyy/MM/dd HH:mm:ss");
            }
            //強震モニタ画像部分
            {
                try
                {
                    DateTime dt1 = DateTime.Now;
                    var dt = dt1.AddSeconds(-2);
                    var url1 = $"http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/acmap_s/{dt.ToString("yyyyMMdd")}/{dt.ToString("yyyyMMddHHmmss")}.acmap_s.gif";
                    WebClient wc1 = new WebClient();
                    Stream stream1 = wc1.OpenRead(url1);
                    Bitmap bitmap1 = new Bitmap(stream1);
                    stream1.Close();
                    bitmap1.MakeTransparent();
                    pictureBox1.BackgroundImage = bitmap1;

                    pictureBox1.ImageLocation = $"http://www.kmoni.bosai.go.jp/data/map_img/PSWaveImg/eew/{dt.ToString("yyyyMMdd")}/{dt.ToString("yyyyMMddHHmmss")}.eew.gif";
                }
                catch
                {
                    var aa = "at";
                }
            }

        }
    }
}

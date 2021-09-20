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
            SolidBrush b = new SolidBrush(Color.FromArgb(47, 79, 79));
            g.FillRectangle(b, 0, 0, 230, 85);
            Pen p = new Pen(Color.FromArgb(40, 60, 60), 3);
            g.DrawRectangle(p, 1, 1, 227, 82);
            Font fnt = new Font("Koruri Regular", 20);
            g.DrawString("取得中",fnt,Brushes.White,10,10);
            g.SmoothingMode = SmoothingMode.AntiAlias;
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
                var al_flg = eew.alertflg;
                var eew_flg = eew.result.message;
                var mag = eew.magunitude;

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

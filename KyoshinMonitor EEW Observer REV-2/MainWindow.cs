using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Drawing.Text;

namespace KyoshinMonitor_EEW_Observer_REV_2
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.main;

            WriteInformationToDisplay(GeneralInfoColor, ("接続中", "Now Loading..."));
        }

        private readonly Font StatusFont = new Font("Koruri Light", 20);    // 状態表示用フォント
        private readonly Font EnglishStatusFont = new Font("Koruri Light", 15);   // 英文状態表示用フォント
        private readonly Font AlertTypeFont = new Font("Koruri Regular", 12); // 速報情報 報版表示用フォント
        private readonly Font RegionFont = new Font("Koruri Regular", 15); // 地域表示用フォント
        private readonly Font DetailLabelFont = new Font("Koruri Regular", 8);  // 震度、マグニチュード、深さ情報 接頭語、単位表示用フォント
        private readonly Font DetailFont = new Font("Koruri Light", 20);   // マグニチュード、深さ情報 表示用フォント
        private readonly Font IntensityFont = new Font("Koruri Light", 25);   // 震度表示用フォント

        private readonly (Color?, Color?, Color?) GeneralInfoColor = (Color.FromArgb(40, 60, 60), Color.FromArgb(47, 79, 79), null);

        private readonly (Color?, Color?, Color?) ForecastColor = (Color.FromArgb(255, 219, 0), Color.FromArgb(218, 165, 2), Color.FromArgb(218, 165, 2));

        private readonly (Color?, Color?, Color?) WarningColor = (Color.FromArgb(142, 0, 0), Color.FromArgb(212, 0, 0), Color.FromArgb(212, 0, 0));

        /// <summary>
        /// 情報をメインウィンドウに表示
        /// </summary>
        /// <param name="backgroundColors">Display Colors</param>
        /// <param name="status">(Japanese Status, English Status)</param>
        /// <param name="primarydata"></param>
        /// <param name="region"></param>
        /// <param name="intensity"></param>
        /// <param name="magnitude"></param>
        /// <param name="depthKm"></param>
        private void WriteInformationToDisplay((Color?, Color?, Color?) backgroundColors, (string, string)? status = null, string primarydata = null, string region = null, string intensity = null, float? magnitude = null, int? depthKm = null)
        {
            Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);

            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                if (backgroundColors.Item1.HasValue)
                    using (SolidBrush b = new SolidBrush(backgroundColors.Item1.Value))
                        g.FillRectangle(b, 0, 0, 230, 85); //文字部分

                if (backgroundColors.Item2.HasValue)
                    using (Pen p = new Pen(backgroundColors.Item2.Value, 3))
                        g.DrawRectangle(p, 1, 1, 227, 82); //枠1

                if (backgroundColors.Item3.HasValue)
                    using (SolidBrush b2 = new SolidBrush(backgroundColors.Item3.Value))
                        g.FillRectangle(b2, 0, 0, 230, 20); //枠2

                if (status != null)
                {
                    g.DrawString(status.Value.Item1 ?? string.Empty, StatusFont, Brushes.White, 3, 2);
                    g.DrawString(status.Value.Item2 ?? string.Empty, EnglishStatusFont, Brushes.White, 4, 30);
                }

                if (primarydata != null)
                    g.DrawString(primarydata, AlertTypeFont, Brushes.Black, 0, 0);
                if (region != null)
                    g.DrawString(region, RegionFont, Brushes.Black, 0, 20);
                if (intensity != null)
                {
                    g.DrawString("震度", DetailLabelFont, Brushes.Black, 3, 67);
                    g.DrawString(intensity, IntensityFont, Brushes.Black, 25, 42);
                }
                if (magnitude != null)
                {
                    g.DrawString("M", DetailLabelFont, Brushes.Black, 85, 67);
                    g.DrawString(magnitude.ToString(), DetailFont, Brushes.Black, 95, 50);
                }
                if (depthKm != null)
                {
                    g.DrawString("深さ", DetailLabelFont, Brushes.Black, 140, 67);
                    g.DrawString(depthKm.ToString(), DetailFont, Brushes.Black, 160, 50);
                    g.DrawString("km", DetailLabelFont, Brushes.Black, 205, 67);
                }
            }

            pictureBox2.Image = canvas;
        }

        private readonly HttpClient EewHttpClient = new HttpClient();

        private async void timer1_Tick(object sender, EventArgs e)
        {
            //EEW部分
            DateTime dt = DateTime.Now; //現在時刻の取得(PC時刻より)
            var tm = dt.AddSeconds(-2); //現在時刻から2秒引く(取得失敗を防ぐため)
            var time = tm.ToString("yyyyMMddHHmmss"); //時刻形式の指定(西暦/月/日/時/分/秒)

            try
            {
                var url = $"http://www.kmoni.bosai.go.jp/webservice/hypo/eew/{time}.json"; //強震モニタURLの指定

                var json = await EewHttpClient.GetStringAsync(url); //awaitを用いた非同期JSON取得
                var eew = JsonConvert.DeserializeObject<EEW>(json);//EEWクラスを用いてJSONを解析(デシリアライズ)
                string reg = eew.region_name;           // 地域
                string intn = eew.calcintensity;        // 震度
                bool end_flg = eew.is_final == "true";  // 最終報
                string al_flg = eew.alertflg;           // アラートタイプ (予報、警報)

                float mag;
                if (!float.TryParse(eew.magunitude, out mag /* マグニチュード */)) mag = float.NaN;

                if (!int.TryParse(eew.depth.Replace("km", ""), out int depth /* 深度 */)) goto OnError;

                if (!int.TryParse(eew.report_num, out int rpt_no /* 報版 */)) goto OnError;

                switch (al_flg)
                {
                    case "警報":
                        Program.LastEewResult = EewResult.Warning;
                        WriteInformationToDisplay(WarningColor, null, $"緊急地震速報(警報) #{rpt_no}{(end_flg ? " 最終" : "")}", reg, intn, mag, depth);
                        break;

                    case "予報":
                        Program.LastEewResult = EewResult.Forecast;
                        WriteInformationToDisplay(ForecastColor, null, $"緊急地震速報(予報) #{rpt_no}{(end_flg ? " 最終" : "")}", reg, intn, mag, depth);
                        break;

                    default:
                        Program.LastEewResult = EewResult.None;
                        WriteInformationToDisplay(GeneralInfoColor, ("受信待機中", "No Data..."));
                        break;
                }
            }
            catch
            {
                goto OnError;
            }

            label2.Text = dt.ToString("yyyy/MM/dd HH:mm:ss");
            await Task.Delay(100); // 尋問
            return;

        OnError:
            timer1.Enabled = false;
            WriteInformationToDisplay(GeneralInfoColor, ("再接続中", "Re connectiong"));
            await Task.Delay(10);
            timer1.Enabled = true;
        }

        private readonly HttpClient ImageHttpClient = new HttpClient();

        private async void timer2_Tick(object sender, EventArgs e)
        {
            await Task.Delay(0);
            //強震モニタ画像部分
            try
            {
                DateTime dt1 = DateTime.Now;
                var dt = dt1.AddSeconds(-2);
                var time1 = dt.ToString("yyyyMMdd");
                var time2 = dt.ToString("yyyyMMddHHmmss");

                {
                    var monimgurlanddesc = MonitorImageSelector.GetUrlAndDescriptionFromCode(Properties.Settings.Default.monit_url, time1, time2);

                    string url = monimgurlanddesc.Item1;
                    label3.Text = monimgurlanddesc.Item2;

                    using (Stream stream = await ImageHttpClient.GetStreamAsync(url))
                    { 
                        Bitmap bitmap = new Bitmap(stream);
                        bitmap.MakeTransparent();
                        pictureBox1.BackgroundImage = bitmap;
                    }
                }

                { 
                    string url = $"http://www.kmoni.bosai.go.jp/data/map_img/PSWaveImg/eew/{time1}/{time2}.eew.gif";

                    Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                    System.Drawing.Imaging.ColorMap[] cms = new System.Drawing.Imaging.ColorMap[]
                            {new System.Drawing.Imaging.ColorMap(), new System.Drawing.Imaging.ColorMap()};
                    //P波
                    cms[0].OldColor = Color.Blue;
                    cms[0].NewColor = Color.SpringGreen;
                    //S波
                    cms[1].OldColor = Color.Red;
                    cms[1].NewColor = Color.OrangeRed;

                    System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
                    ia.SetRemapTable(cms);

                    using (Stream stream = await ImageHttpClient.GetStreamAsync(url))
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        Bitmap img = new Bitmap(stream);

                        Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
                        g.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                    }

                    pictureBox1.Image = canvas;
                }
                
                await Task.Delay(100);
            }
            catch { }
        }

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings f = new Settings();
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
            SubWindow f = new SubWindow();
            f.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.main = this.Location;
            Properties.Settings.Default.Save();
        }
    }
}

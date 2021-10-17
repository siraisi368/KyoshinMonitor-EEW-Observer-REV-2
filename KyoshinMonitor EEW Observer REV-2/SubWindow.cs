using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using System.Drawing.Text;

namespace KyoshinMonitor_EEW_Observer_REV_2
{
    public partial class SubWindow : Form
    {
        public SubWindow()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            WriteInformationToDisplay(GeneralInfoColor, "接続中");
        }

        private readonly HttpClient ImageHttpClient = new HttpClient();

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await Task.Delay(0);
            try
            {
                DateTime dt1 = DateTime.Now;
                var dt = dt1.AddSeconds(-2);
                var time1 = dt.ToString("yyyyMMdd");
                var time2 = dt.ToString("yyyyMMddHHmmss");

                {
                    var monurlanddesc = MonitorImageSelector.GetUrlAndDescriptionFromCode(Properties.Settings.Default.monit_url2, time1, time2);
                    string url = monurlanddesc.Item1;
                    label3.Text = monurlanddesc.Item2;

                    using (Stream stream1 = await ImageHttpClient.GetStreamAsync(url))
                    {
                        Bitmap bitmap1 = new Bitmap(stream1);
                        bitmap1.MakeTransparent();
                        pictureBox1.BackgroundImage = bitmap1;
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
                        pictureBox1.Image = canvas;
                    }
                }
                await Task.Delay(100);
            }
            catch { }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            switch (Program.LastEewResult)
            {
                case EewResult.Forecast:
                    WriteInformationToDisplay(ForecastColor, "緊急地震速報(予報)");
                    break;

                case EewResult.Warning:
                    WriteInformationToDisplay(WarningColor, "緊急地震速報(警報)");
                    break;

                case EewResult.None:
                    WriteInformationToDisplay(GeneralInfoColor, "受信待機中");
                    break;
            }
        }

        private readonly Font StatusFont = new Font("Koruri Light", 20);

        private readonly (Color?, Color?) GeneralInfoColor = (Color.FromArgb(40, 60, 60), Color.FromArgb(47, 79, 79));
        private readonly (Color?, Color?) ForecastColor = (Color.FromArgb(255, 219, 0), Color.FromArgb(218, 165, 2));
        private readonly (Color?, Color?) WarningColor = (Color.FromArgb(142, 0, 0), Color.FromArgb(212, 0, 0));

        private void WriteInformationToDisplay((Color?, Color?) backgroundColors, string infoText)
        {
            Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);

            using (Graphics g = Graphics.FromImage(canvas))
            {
                if (backgroundColors.Item1.HasValue)
                    using (SolidBrush b = new SolidBrush(backgroundColors.Item1.Value))
                        g.FillRectangle(b, 0, 0, 245, 47);

                if (backgroundColors.Item2.HasValue)
                    using (Pen p = new Pen(backgroundColors.Item2.Value, width: 3))
                        g.DrawRectangle(p, 1, 1, 242, 44);

                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                g.DrawString(infoText, StatusFont, Brushes.White, 3, 2);
            }

            pictureBox2.Image = canvas;
        }
    }
}

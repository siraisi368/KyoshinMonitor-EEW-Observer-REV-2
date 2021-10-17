using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;

namespace KyoshinMonitor_EEW_Observer_REV_2
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.monit_url = comboBox1.SelectedIndex;
            Properties.Settings.Default.monit_url2 = comboBox2.SelectedIndex;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = Properties.Settings.Default.monit_url;
            comboBox2.SelectedIndex = Properties.Settings.Default.monit_url2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("https://siraisiofficial.net");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("https://siraisiofficial.net/soft.htm");
        }

        private async void button3_Click(object sender, EventArgs e)
        {

            var client = new HttpClient();

            var url = $"https://siraisiofficial.net/kmeor2.json"; //強震モニタURLの指定

            var json = await client.GetStringAsync(url); //awaitを用いた非同期JSON取得
            var master = JsonConvert.DeserializeObject<master>(json);//EEWクラスを用いてJSONを解析(デシリアライズ)
            var version = master.version;
            var edition = master.edition;
            var a = "";
            if(version == "1.0.0")
            {
                if(edition == "beta")
                {
                    label7.Text = $"最新のバージョン:v1.0.0 {edition}";
                    a = "t";
                }
            }
            if(a == "t")
            {
                
            }
            else
            {
                label7.Text = $"最新のバージョン:v{version} {edition}";
                label8.Text = "最新verダウンロード";
            }
        }

        class master
        {
            public string version { get; set; }
            public string edition { get; set; }
        }

        private async void label8_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();

            var url = $"https://siraisiofficial.net/kmeor2.json"; //強震モニタURLの指定

            var json = await client.GetStringAsync(url); //awaitを用いた非同期JSON取得
            var master = JsonConvert.DeserializeObject<master>(json);//EEWクラスを用いてJSONを解析(デシリアライズ)
            var version = master.version;
            var edition = master.edition;

            Process.Start($"https://siraisiofficial.net/kmeo-rev2{version}/kmeo{version}-{edition}.zip");
        }
    }
}

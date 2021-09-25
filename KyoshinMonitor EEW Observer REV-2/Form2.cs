using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KyoshinMonitor_EEW_Observer_REV_2
{
    public partial class Form2 : Form
    {
        public Form2()
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WHash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool FilesExists()
        {
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("No file " + textBox1.Text);
                return false;
            }
            if (!File.Exists(textBox2.Text))
            {
                MessageBox.Show("No file " + textBox2.Text);
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!FilesExists()) return;
            MessageBox.Show(Util.HammingDistance(WHashAlg.Do(textBox1.Text), WHashAlg.Do(textBox2.Text)).ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!FilesExists()) return;
            MessageBox.Show(Util.HammingDistance(DHashAlg.Do(textBox1.Text), DHashAlg.Do(textBox2.Text)).ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!FilesExists()) return;
            MessageBox.Show(Util.HammingDistance(AHashAlg.Do(textBox1.Text), AHashAlg.Do(textBox2.Text)).ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnalizHelperSystem
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            label1.Text = " Utility:\"" + this.ProductName + "\"\n Kation, Suhov 545a  v" + this.ProductVersion + "\n               2o11";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1.PerformClick();
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                button1.PerformClick();
            }
        }
    }
}

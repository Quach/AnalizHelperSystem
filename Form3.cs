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
    public partial class Form3 : Form
    {
        public bool exit = false;       //should we close an application?
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exit = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            exit = false;
            this.Close();
        }

        private void Form3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1.PerformClick();
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                button2.PerformClick();
            }
        }
    }
}

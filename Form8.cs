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
    public partial class Form8 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        private string c_factor_name = "";
        private string c_factor_def = "";
        private string c_temp_factor_name = "";

        public Form8()
        {
            InitializeComponent();
            comboBox1.DisplayMember = "Name_fact";
            comboBox1.ValueMember = "Name_fact";
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            c_temp_factor_name = comboBox1.Text;
            backgroundWorker2.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            c_factor_name = textBox1.Text;
            c_factor_def = richTextBox1.Text;
            c_temp_factor_name = comboBox1.Text;
            backgroundWorker3.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form8_KeyPress(object sender, KeyPressEventArgs e)
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet dataSet1 = dbw1.ReadFactors();
            comboBox1.DataSource = dataSet1.Tables[0];
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Visible = false;
            groupBox1.Enabled = true;
            textBox1.Focus();
            if (e.Error != null)
            {
                MessageBox.Show(this, e.Error.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
                comboBox1_SelectedIndexChanged(comboBox1, null);
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet dataSet1 = dbw1.ReadNameDefFactor(c_temp_factor_name);
            c_factor_name = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            c_factor_def = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Visible = false;
            groupBox1.Enabled = true;
            comboBox1.Focus();
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox1.Text = c_factor_name;
                richTextBox1.Text = c_factor_def;
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            dbw1.UpdateFactor(c_factor_name, c_factor_def, c_temp_factor_name);
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Visible = false;
            groupBox1.Enabled = true;
            if (e.Error == null)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show(this, e.Error.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

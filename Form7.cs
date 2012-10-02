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
    public partial class Form7 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        private string c_metric_name = "";
        private string c_metric_def = "";
        private string c_metric_formula = "";
        private string c_metric_unit = "";
        private string c_criteria_name = "";
        public Form7()
        {
            InitializeComponent();
            comboBox1.DisplayMember = "Name_crit";
            comboBox1.ValueMember = "Name_crit";
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            c_metric_def = richTextBox1.Text;
            c_metric_name = textBox1.Text;
            c_criteria_name = comboBox1.SelectedValue.ToString();
            c_metric_formula = richTextBox3.Text;
            c_metric_unit = richTextBox2.Text;
            backgroundWorker2.RunWorkerAsync();
        }

        private void Form7_KeyPress(object sender, KeyPressEventArgs e)
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

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            dbw1.InsertMetric(c_metric_name, c_metric_def, c_metric_formula, c_metric_unit, c_criteria_name);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet dataSet1 = dbw1.ReadCriterias();
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
        }
    }
}

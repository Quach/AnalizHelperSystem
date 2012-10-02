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
    public partial class Form6 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        private string c_criteria_name = "";
        private string c_criteria_def = "";
        private String[] c_factor_names;
        public Form6()
        {
            InitializeComponent();
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void Form6_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            c_criteria_name = textBox1.Text;
            c_criteria_def = richTextBox1.Text;
            c_factor_names = new String[checkedListBox1.CheckedItems.Count];
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                c_factor_names[i] = checkedListBox1.CheckedItems[i].ToString();
            }
            backgroundWorker2.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //dbw1.ReadDataBaseToDataSet("QUIM", "select * from factor, criteria, metric, report, profile");
            DataSet dataSet1 = dbw1.ReadFactors();
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString(), false);
            }
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

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            dbw1.InsertCriteria(c_criteria_name, c_criteria_def, c_factor_names);
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
    }
}

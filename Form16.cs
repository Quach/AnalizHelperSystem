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
    public partial class Form16 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        private string c_criteria_name = "";
        private string c_criteria_def = "";
        private String[] c_factor_names;
        private string c_temp_criteria_name = "";
        private DataSet dataSet1;
        private DataSet dataSet2;

        public Form16()
        {
            InitializeComponent();
            comboBox1.DisplayMember = "Name_crit";
            comboBox1.ValueMember = "Name_crit";
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            c_temp_criteria_name = comboBox1.Text;
            backgroundWorker2.RunWorkerAsync();
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
            c_temp_criteria_name = comboBox1.Text;
            backgroundWorker3.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
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
            else
            {
                this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
                comboBox1_SelectedIndexChanged(comboBox1, null);
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            dataSet1 = dbw1.ReadNameDefCriteria(c_temp_criteria_name);
            dataSet2 = dbw1.ReadFactors(c_temp_criteria_name);
            c_criteria_name = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            c_criteria_def = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
            dataSet1 = dbw1.ReadFactors();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Visible = false;
            groupBox1.Enabled = true;
            comboBox1.Focus();
            if (e.Error != null)
            {
                MessageBox.Show(this, e.Error.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox1.Text = c_criteria_name;
                richTextBox1.Text = c_criteria_def;
                checkedListBox1.Items.Clear();
                for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
                {
                    checkedListBox1.Items.Add(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString(), false);
                }
                for (int i = 0; i < dataSet2.Tables[0].Rows.Count; i++)
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(dataSet2.Tables[0].Rows[i].ItemArray[0].ToString()), true);
                }
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            dbw1.UpdateCriterias(c_criteria_name, c_criteria_def, c_factor_names, c_temp_criteria_name);
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

        private void Form16_KeyPress(object sender, KeyPressEventArgs e)
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
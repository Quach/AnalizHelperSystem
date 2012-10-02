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
    public partial class Form18 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        DataSet dataSet1;

        public Form18()
        {
            InitializeComponent();
            dataSet1 = dbw1.ReadProfiles();
            comboBox1.DataSource = dataSet1.Tables[0];
            comboBox1.DisplayMember = "name_prof";
            comboBox1.ValueMember = "name_prof";
            dataSet1 = dbw1.ReadMetrics();
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString(), false);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            pictureBox1.Enabled = true;
            pictureBox1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox1.Enabled = true;
                comboBox1_SelectedIndexChanged(comboBox1, null);
            }
            else
            {
                foreach (int checkedItemIndex in checkedListBox1.CheckedIndices)
                {
                    checkedListBox1.SetItemChecked(checkedItemIndex, false);
                }
                comboBox1.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSet1 = dbw1.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select m.name_met from metric m, profile p, profile_metric pm where p.id_prof = pm.id_prof and m.id_met = pm.id_met and p.name_prof like '" + comboBox1.GetItemText(comboBox1.Items[comboBox1.SelectedIndex]) + "'");
            foreach (int checkedItemIndex in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemChecked(checkedItemIndex, false);
            }
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString()), true);
            }
        }

        private void Form18_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1.PerformClick();
                this.Close();
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                button2.PerformClick();
                this.Close();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            dbw1.ChangeDataInDataBase("QUIM", "insert into report (progName, PercOfEnd, CreationD_rep) values('" + textBox1.Text + "', 0.0, 2342645)");
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                dbw1.ChangeDataInDataBase(SqlConnectionParametrs.DataBaseName, "insert into MetrInRep (id_rep, id_met) select (select id_rep from report where ProgName like '" + textBox1.Text + "'), (select id_met from metric where name_met like '" + checkedListBox1.CheckedItems[i].ToString() + "')");
            }
            //dbw1.ReadDataBaseToDataSet(temp.name_db, "select * from factor, criteria, metric, profile, report");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            groupBox1.Enabled = true;
            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;

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

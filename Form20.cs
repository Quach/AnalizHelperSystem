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
    public partial class Form20 : Form
    {
        DBwork dbw1 = new DBwork("QUIM", "");
        DataSet dataSet1;
        int lastSelect = -1;
        String[] Metrics;
        public Form20()
        {
            InitializeComponent();
            dataSet1 = dbw1.ReadReports();
            comboBox2.DataSource = dataSet1.Tables[0];
            comboBox2.DisplayMember = "progName";
            comboBox2.ValueMember = "progName";
            dataSet1 = dbw1.ReadProfiles();
            comboBox1.DataSource = dataSet1.Tables[0];
            comboBox1.DisplayMember = "name_prof";
            comboBox1.ValueMember = "name_prof";
            checkedListBox1.Items.Clear();
            dataSet1 = dbw1.ReadMetrics();
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString(), false);
            }
            comboBox2_SelectedIndexChanged(comboBox2, null);
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
                dataSet1 = dbw1.ReadDataBaseToDataSet(temp.name_db, "select m.name_met from metric m, metrInRep mr, report r where r.id_rep = mr.id_rep and m.id_met = mr.id_met and r.progName like '" + comboBox2.GetItemText(comboBox2.Items[lastSelect]) + "'");
                for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString()), true);
                }
                comboBox1.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (int checkedItemIndex in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemChecked(checkedItemIndex, false);
            }
            dataSet1 = dbw1.ReadDataBaseToDataSet(temp.name_db, "select m.name_met from metric m, metrInRep mr, report r where r.id_rep = mr.id_rep and m.id_met = mr.id_met and r.progName like '" + comboBox2.GetItemText(comboBox2.Items[lastSelect]) + "'");
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString()), true);
            }
            dataSet1 = dbw1.ReadDataBaseToDataSet(temp.name_db, "select m.name_met from metric m, profile p, profile_metric pm where p.id_prof = pm.id_prof and m.id_met = pm.id_met and p.name_prof like '" + comboBox1.GetItemText(comboBox1.Items[comboBox1.SelectedIndex]) + "'");
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString()), true);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastSelect = comboBox2.SelectedIndex;
            dataSet1 = dbw1.ReadDataBaseToDataSet(temp.name_db, "select m.name_met from metric m, metrInRep mr, report r where r.id_rep = mr.id_rep and m.id_met = mr.id_met and r.progName like '" + comboBox2.GetItemText(comboBox2.Items[lastSelect]) + "'");
            foreach (int checkedItemIndex in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemChecked(checkedItemIndex, false);
            }
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString()), true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Metrics = new String[checkedListBox1.CheckedItems.Count];
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                Metrics[i] = checkedListBox1.CheckedItems[i].ToString();
            }
            dbw1.ChangeDataInDataBase(temp.name_db, "update report set progName = '" + comboBox2.Text + "' where progName like '" + comboBox2.GetItemText(comboBox2.Items[lastSelect]) + "'");
            dbw1.UpdateMetrics(comboBox2.Text, Metrics);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

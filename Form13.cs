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
    public partial class Form13 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        DataSet dataSet1;
        DataSet dataSet2;
        int lastSelect = -1;
        public Form13()
        {
            InitializeComponent();
            dataSet1 = dbw1.ReadProfiles();
            comboBox1.DataSource = dataSet1.Tables[0];
            comboBox1.DisplayMember = "name_prof";
            comboBox1.ValueMember = "name_prof";
            dataSet1 = dbw1.ReadCriterias();
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                TreeNode node = treeView1.Nodes.Add(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString());
                dataSet2 = dbw1.ReadMetrics(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString());
                for (int j = 0; j < dataSet2.Tables[0].Rows.Count; j++)
                {
                    TreeNode node1 = node.Nodes.Add(dataSet2.Tables[0].Rows[j].ItemArray[0].ToString());
                }
            }
            comboBox1_SelectedIndexChanged(comboBox1, null);
        }

        private void treeView1_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            // Ставим галочку на всех подузлах.
            SelectAllSubnodes(e.Node);
        }
        // Метод для установки галочки для всех подузлов.
        void SelectAllSubnodes(TreeNode treeNode)
        {
            // Ставим или убираем отметку со всех подузлов.
            foreach (TreeNode treeSubNode in treeNode.Nodes)
            {
                treeSubNode.Checked = treeNode.Checked;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastSelect = comboBox1.SelectedIndex;
            dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "Select m.name_met from Criteria c, Profile_Metric pm, [Profile] p, Metric m, Factor f, Factor_criteria fc where p.id_prof = pm.id_prof and m.id_met = pm.id_met and m.id_crit = c.id_crit and f.id_fact = fc.id_fact and c.id_crit = fc.id_crit and p.name_prof like '" + comboBox1.GetItemText(comboBox1.Items[lastSelect]) + "' group by name_met");
            string[] metricsChecked = new string[dataSet1.Tables[0].Rows.Count];
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                metricsChecked[i] = dataSet1.Tables[0].Rows[i].ItemArray[0].ToString();
            }
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes[i].Checked = false;
            }
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode node = treeView1.Nodes[i];
                if (node.Nodes.Count != 0)
                    for (int j = 0; j < node.Nodes.Count; j++)
                    {
                        if (metricsChecked.Contains(node.Nodes[j].Text))
                        {
                            node.Nodes[j].Checked = true;
                            treeView1.Nodes[i].Checked = true;
                        }
                        else
                            node.Nodes[j].Checked = false;
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbw1.ChangeDataInDataBase("QUIM", "delete from profile_metric where id_prof in (select id_prof from profile where name_prof like '" + comboBox1.GetItemText(comboBox1.Items[lastSelect]) + "')");
            dbw1.ChangeDataInDataBase("QUIM", "update profile set name_prof = '" + comboBox1.Text + "' where name_prof like '" + comboBox1.GetItemText(comboBox1.Items[lastSelect]) + "'");
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode node = treeView1.Nodes[i];
                if (node.Nodes.Count != 0)
                    for (int j = 0; j < node.Nodes.Count; j++)
                    {
                        if (node.Nodes[j].Checked)
                            dbw1.ChangeDataInDataBase("QUIM", "insert into profile_metric (id_prof, id_met) select id_prof, id_met from metric, [profile] where name_prof like '" + comboBox1.Text + "' and name_met like '" + node.Nodes[j].Text + "'");
                    }
            }
            this.Close();
        }
    }
}

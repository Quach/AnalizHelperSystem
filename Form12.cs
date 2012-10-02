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
    public partial class Form12 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        private string c_profile_name = "";
        private string[] c_metrics;
        private DataSet dataSet1;
        private DataSet[] dataSet2;
        private int c_number_of_metrics = 0;

        public Form12()
        {
            InitializeComponent();
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
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

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            pictureBox1.Visible = true;
            c_profile_name = textBox1.Text;
            c_metrics = new String[c_number_of_metrics];
            c_number_of_metrics = 0;
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode node = treeView1.Nodes[i];
                if (node.Nodes.Count != 0)
                    for (int j = 0; j < node.Nodes.Count; j++)
                    {
                        if (node.Nodes[j].Checked)
                        {
                            c_metrics[c_number_of_metrics] = node.Nodes[j].Text;
                            c_number_of_metrics++;
                        }
                    }
            }
            backgroundWorker2.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            dataSet1 = dbw1.ReadCriterias();
            dataSet2 = new DataSet[dataSet1.Tables[0].Rows.Count];
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {
                dataSet2[i] = dbw1.ReadMetrics(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString());
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
            else
            {
                for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
                {
                    TreeNode node = treeView1.Nodes.Add(dataSet1.Tables[0].Rows[i].ItemArray[0].ToString());
                    for (int j = 0; j < dataSet2[i].Tables[0].Rows.Count; j++)
                    {
                        TreeNode node1 = node.Nodes.Add(dataSet2[i].Tables[0].Rows[j].ItemArray[0].ToString());
                        c_number_of_metrics++;
                    }
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            dbw1.InsertProfile(c_profile_name);
            dbw1.UpdateMetricsProfile(c_profile_name, c_metrics, c_number_of_metrics);
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

        private void Form12_KeyPress(object sender, KeyPressEventArgs e)
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

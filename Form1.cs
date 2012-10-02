using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AnalizHelperSystem
{
    public partial class Form1 : Form
    {
        DBwork dbw1 = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);
        private DataSet dataSetTemp;
        private delegate void DelegateListView();
        DelegateListView g_del_listView1;
        DelegateListView g_del_listView2;
        DataSet dataSet21;
        DataSet dataSet11;
        SqlDataAdapter adapter;
        SqlCommandBuilder cmdBuilder;

        public Form1()
        {
            InitializeComponent();
            g_del_listView1 = new DelegateListView(ReadProfiles);
            g_del_listView2 = new DelegateListView(ReadReports);

            modifyReportToolStripMenuItem_Click(modifyReportToolStripMenuItem, null);

            //Reading Factors form db firstly
            dataGridView1_ReadFactors();

            //Reading Profiles from db
            listView1_ReadProfiles();

            //Reading reports from db
            listView2_ReadReports();
        }

        void form1_block()
        {
            menuStrip1.Enabled = false;
            tabControl1.Enabled = false;
            toolStripProgressBar1.Visible = true;
        }

        void form1_unBlock()
        {
            toolStripProgressBar1.Visible = false;
            menuStrip1.Enabled = true;
            tabControl1.Enabled = true;
        }

        void listView2_ReadReports()
        {
            //form1_block();
            listView2.Clear();
            dataGridView8.DataSource = null;
            richTextBox8.Clear();
            richTextBox7.Clear();
            richTextBox13.Clear();
            ReadReports();
            //backgroundWorker1.RunWorkerAsync(g_del_listView2);
        }

        void listView1_ReadProfiles()
        {
            //form1_block();
            listView1.Clear();
            dataGridView4.DataSource = null;
            dataGridView6.DataSource = null;
            dataGridView7.DataSource = null;
            richTextBox9.Clear();
            richTextBox11.Clear();
            richTextBox12.Clear();
            richTextBox10.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            ReadProfiles();
            //backgroundWorker1.RunWorkerAsync(g_del_listView1);
        }

        void dataGridView1_ReadFactors()
        {
            DataSet dataSet1 = dbw1.ReadFactors();
            dataGridView1.DataSource = dataSet1.Tables[0];
            dataGridView1.Columns[0].Width = dataGridView1.Width - 3;
            
        }
        void dataGridView2_ReadCriterias()
        {
            DataSet dataSet1 = dbw1.ReadCriterias(dataGridView1[0, (dataGridView1.SelectedCells[0].RowIndex)].Value.ToString());
            dataGridView2.DataSource = dataSet1.Tables[0];
            dataGridView2.Columns[0].Width = dataGridView2.Width - 3;
        }
        void dataGridView3_ReadMetrics()
        {
            if (dataGridView2.Rows.Count != 0)
            {
                DataSet dataSet1 = dbw1.ReadMetrics(dataGridView2[0, ((dataGridView2.SelectedCells[0].RowIndex))].Value.ToString());
                dataGridView3.DataSource = dataSet1.Tables[0];
                dataGridView3.Columns[0].Width = dataGridView3.Width - 3;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Perform a window which ask exit
            toolStripStatusLabel1.Text = "Exiting...";
            Form3 f3 = new Form3();
            f3.ShowDialog();
            if (!f3.exit)
            {
                e.Cancel = true;
                toolStripStatusLabel1.Text = "Working...";
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tabControl1.Height = this.Height - 98;
            tabControl1.Width = this.Width - 40;
        }

        private void tabControl1_SizeChanged(object sender, EventArgs e)
        {
            tabControl1.Invalidate();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BindingSource bindingSource1 = new BindingSource();
            DataSet dataSet1 = dbw1.ReadCriterias(dataGridView1[0, (e.RowIndex)].Value.ToString());
            bindingSource1.DataSource = dataSet1.Tables[0];
            dataGridView2.DataSource = bindingSource1;
            dataGridView2.Columns[0].Width = dataGridView2.Width - 3;
            dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select def_fact, name_fact from factor where name_fact LIKE '" + dataGridView1[0, (e.RowIndex)].Value.ToString() + "'");
            richTextBox2.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            richTextBox1.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
            richTextBox3.Clear();
            richTextBox4.Clear();
            dataGridView3.DataSource = null;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BindingSource bindingSource1 = new BindingSource();
            DataSet dataSet1 = dbw1.ReadMetrics(dataGridView2[0, (e.RowIndex)].Value.ToString());
            bindingSource1.DataSource = dataSet1.Tables[0];
            dataGridView3.DataSource = bindingSource1;
            dataGridView3.Columns[0].Width = dataGridView3.Width - 3;
            dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select def_crit, name_crit from criteria where name_crit LIKE '" + dataGridView2[0, (e.RowIndex)].Value.ToString() + "'");
            richTextBox2.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            richTextBox1.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
            richTextBox3.Clear();
            richTextBox4.Clear();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataSet dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select name_met, Interpretation_met, Formula_met, def_met from metric where name_met LIKE '" + dataGridView3[0, (e.RowIndex)].Value.ToString() + "'");
            richTextBox1.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            richTextBox4.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
            richTextBox3.Text = dataSet1.Tables[0].Rows[0].ItemArray[2].ToString();
            richTextBox2.Text = dataSet1.Tables[0].Rows[0].ItemArray[3].ToString();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //info
            toolStripStatusLabel1.Text = "Informing...";
            Form4 f4 = new Form4();
            f4.ShowDialog();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void Form1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            оПрограммеToolStripMenuItem_Click(sender, hlpevent);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                menuStrip1.Items[1].Enabled = true;
                menuStrip1.Items[2].Enabled = false;
                menuStrip1.Items[3].Enabled = false;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                menuStrip1.Items[1].Enabled = false;
                menuStrip1.Items[2].Enabled = true;
                menuStrip1.Items[3].Enabled = false;
            }
            if (tabControl1.SelectedIndex == 2)
            {
                menuStrip1.Items[1].Enabled = false;
                menuStrip1.Items[2].Enabled = false;
                menuStrip1.Items[3].Enabled = true;
            }
        }

        private void addFactorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Adding factor...";
            Form5 f5 = new Form5();
            f5.ShowDialog();
            dataGridView1_ReadFactors();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void addCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Adding criteria...";
            Form6 f6 = new Form6();
            f6.ShowDialog(this);
            dataGridView2_ReadCriterias();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void addMetricToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Adding metric...";
            Form7 f7 = new Form7();
            f7.ShowDialog(this);
            dataGridView3_ReadMetrics();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void tabPage1_Resize(object sender, EventArgs e)
        {
            int lenth = (tabPage1.Width - 28) / 3;
            dataGridView1.Width = dataGridView2.Width = dataGridView3.Width = lenth;
            dataGridView2.Location = new Point(lenth + 15, 19);
            dataGridView3.Location = new Point(2 * lenth + 21, 19);
            label2.Location = new Point(lenth + 12, 3);
            label3.Location = new Point(2 * lenth + 18, 3);
            lenth = (tabPage1.Width - 23) / 2;
            groupBox1.Width = groupBox2.Width = groupBox3.Width = groupBox4.Width = lenth;
            groupBox3.Location = new Point(9, tabPage1.Height - 17 - groupBox3.Height);
            groupBox4.Location = new Point(groupBox3.Width + 14, tabPage1.Height - 17 - groupBox4.Height);
            groupBox1.Location = new Point(9, tabPage1.Height - 23 - groupBox3.Height - groupBox1.Height);
            groupBox2.Location = new Point(groupBox1.Width + 14, tabPage1.Height - 23 - groupBox2.Height - groupBox2.Height);
            lenth = tabPage1.Height - groupBox1.Height - groupBox3.Height - 48;
            dataGridView1.Height = lenth;
            dataGridView2.Height = lenth;
            dataGridView3.Height = lenth;
        }

        private void tabPage2_Resize(object sender, EventArgs e)
        {
            listView1.Height = tabPage2.Height - 45;
            label5.Location = new Point(11, tabPage2.Height - 23);
            int lenth = (tabPage2.Width - 244) / 3;
            richTextBox5.Width = lenth * 2;
            richTextBox6.Location = new Point(232 + lenth * 2, 19);
            richTextBox6.Width = lenth;
            label7.Location = new Point(232 + lenth * 2, 3);
            dataGridView7.Width = lenth;
            dataGridView6.Location = new Point(239 + lenth, 61);
            dataGridView6.Width = lenth;
            label13.Location = new Point(dataGridView6.Location.X - 3, 45);
            dataGridView4.Location = new Point(245 + lenth * 2, 61);
            dataGridView4.Width = lenth - 15;
            label8.Location = new Point(dataGridView4.Location.X - 3, 45);
            lenth = (tabPage2.Width - 244) / 2;
            groupBox5.Width = groupBox7.Width = lenth - 10;
            groupBox6.Width = groupBox8.Width = lenth;
            lenth = (tabPage2.Height - 71) / 3;
            dataGridView4.Height = dataGridView6.Height = dataGridView7.Height = lenth * 2;
            groupBox7.Height = groupBox8.Height = lenth / 2;
            groupBox6.Height = groupBox5.Height = lenth / 2 - 12;
            groupBox8.Location = new Point(233, dataGridView7.Height + 67);
            groupBox7.Location = new Point(239 + groupBox8.Width, dataGridView7.Height + 67);
            groupBox6.Location = new Point(233, groupBox8.Location.Y + groupBox8.Height + 6);
            groupBox5.Location = new Point(239 + groupBox6.Width, groupBox7.Location.Y + groupBox7.Height + 6);
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            richTextBox1.Width = groupBox1.Width - 12;
        }

        private void groupBox2_Resize(object sender, EventArgs e)
        {
            richTextBox2.Width = groupBox2.Width - 12;
        }

        private void groupBox3_Resize(object sender, EventArgs e)
        {
            richTextBox3.Width = groupBox3.Width - 12;
        }

        private void groupBox4_Resize(object sender, EventArgs e)
        {
            richTextBox4.Width = groupBox4.Width - 12;
        }

        private void dataGridView_Resize(object sender, EventArgs e)
        {
            DataGridView temp = sender as DataGridView;
            if (temp.Columns.Count != 0)
                temp.Columns[0].Width = temp.Width - 3;
        }

        private void modifyFactorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Modifing Factor...";
            Form8 f8 = new Form8();
            f8.ShowDialog(this);
            dataGridView1_ReadFactors();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void modifyCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Modifing Criteria...";
            Form16 f16 = new Form16();
            f16.ShowDialog(this);
            dataGridView2_ReadCriterias();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void modifyMetricToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Modifing Metric...";
            Form17 f17 = new Form17();
            f17.ShowDialog(this);
            dataGridView3_ReadMetrics();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void deleteFactorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Deleting Factor...";
            Form9 f9 = new Form9();
            f9.ShowDialog(this);
            dataGridView1_ReadFactors();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void deleteCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Deleting Criteria...";
            Form10 f10 = new Form10();
            f10.ShowDialog(this);
            dataGridView2_ReadCriterias();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void deleteMetricToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Deleting Metric...";
            Form11 f11 = new Form11();
            f11.ShowDialog(this);
            dataGridView3_ReadMetrics();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void groupBox8_Resize(object sender, EventArgs e)
        {
            richTextBox12.Width = groupBox8.Width - 12;
            richTextBox12.Height = groupBox8.Height - 25;
        }

        private void groupBox7_Resize(object sender, EventArgs e)
        {
            richTextBox11.Width = groupBox7.Width - 12;
            richTextBox11.Height = groupBox7.Height - 25;
        }

        private void groupBox5_Resize(object sender, EventArgs e)
        {
            richTextBox9.Width = groupBox5.Width - 12;
            richTextBox9.Height = groupBox5.Height - 25;
        }

        private void groupBox6_Resize(object sender, EventArgs e)
        {
            richTextBox10.Width = groupBox6.Width - 12;
            richTextBox10.Height = groupBox6.Height - 25;
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BindingSource bindingSource1 = new BindingSource();
            DataSet dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "Select c.name_crit from Criteria c, Profile_Metric pm, [Profile] p, Factor f, Factor_criteria fc, Metric m where p.id_prof = pm.id_prof and m.id_met = pm.id_met and f.id_fact = fc.id_fact and c.id_crit = fc.id_crit and m.id_crit = c.id_crit and p.name_prof like '" + listView1.Items[listView1.SelectedIndices[0]].Text + "' and f.name_fact like '" + dataGridView7[0, (e.RowIndex)].Value.ToString() + "' group by name_crit");
            bindingSource1.DataSource = dataSet1.Tables[0];
            dataGridView6.DataSource = bindingSource1;
            dataGridView6.Columns[0].Width = dataGridView6.Width - 3;
            dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select def_fact, name_fact from factor where name_fact LIKE '" + dataGridView7[0, (e.RowIndex)].Value.ToString() + "'");
            richTextBox10.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            richTextBox12.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
            richTextBox11.Clear();
            richTextBox9.Clear();
            dataGridView4.DataSource = null;
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BindingSource bindingSource1 = new BindingSource();
            DataSet dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "Select m.name_met from Criteria c, Profile_Metric pm, [Profile] p, Factor f, Factor_criteria fc, Metric m where p.id_prof = pm.id_prof and m.id_met = pm.id_met and f.id_fact = fc.id_fact and c.id_crit = fc.id_crit and m.id_crit = c.id_crit and p.name_prof like '" + listView1.Items[listView1.SelectedIndices[0]].Text + "' and c.name_crit like '" + dataGridView6[0, (e.RowIndex)].Value.ToString() + "' group by name_met");
            bindingSource1.DataSource = dataSet1.Tables[0];
            dataGridView4.DataSource = bindingSource1;
            dataGridView4.Columns[0].Width = dataGridView4.Width - 3;
            dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select def_crit, name_crit from criteria where name_crit LIKE '" + dataGridView6[0, (e.RowIndex)].Value.ToString() + "'");
            richTextBox10.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            richTextBox12.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
            richTextBox9.Clear();
            richTextBox11.Clear();
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataSet dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select name_met, Interpretation_met, Formula_met, def_met from metric where name_met LIKE '" + dataGridView4[0, (e.RowIndex)].Value.ToString() + "'");
            richTextBox12.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
            richTextBox9.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
            richTextBox11.Text = dataSet1.Tables[0].Rows[0].ItemArray[2].ToString();
            richTextBox10.Text = dataSet1.Tables[0].Rows[0].ItemArray[3].ToString();
        }

        private void createProfileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Creating Profile...";
            Form12 f12 = new Form12();
            f12.ShowDialog(this);
            listView1_ReadProfiles();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void modifyProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Modifing Profile...";
            Form13 f13 = new Form13();
            f13.ShowDialog(this);
            listView1_ReadProfiles();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Deleting Profile...";
            Form14 f14 = new Form14();
            f14.ShowDialog(this);
            listView1_ReadProfiles();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void dataGridView8_Resize(object sender, EventArgs e)
        {
            if (dataGridView8.Columns.Count != 0)
            {
                int lenth = dataGridView8.Width / 5;
                dataGridView8.Columns[0].Width = lenth * 3;
                dataGridView8.Columns[1].Width = lenth;
                dataGridView8.Columns[2].Width = lenth;
            }
        }

        private void modifyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modifyReportToolStripMenuItem.Checked)
            {
                dataGridView8.ReadOnly = false;
                if (dataGridView8.Columns.Count != 0)
                {
                    dataGridView8.Columns[0].ReadOnly = true;
                    dataGridView8.Columns[3].ReadOnly = true;
                }
            }
            else
            {
                dataGridView8.ReadOnly = true;
            }
        }

        private void tabPage3_Resize(object sender, EventArgs e)
        {
            listView2.Height = tabPage3.Height - 45;
            label9.Location = new Point(11, tabPage3.Height - 23);
            int lenth = (tabPage3.Width - 259) / 3;
            richTextBox7.Width = richTextBox8.Width = lenth;
            richTextBox13.Width = lenth + 1;
            richTextBox7.Location = new Point(6 + richTextBox8.Width + richTextBox8.Location.X, 19);
            label11.Location = new Point(richTextBox7.Location.X - 3, 3);
            richTextBox13.Location = new Point(6 + richTextBox7.Width + richTextBox7.Location.X, 19);
            label15.Location = new Point(richTextBox13.Location.X - 3, 3);
            dataGridView8.Height = tabPage3.Height - 59;
            dataGridView8.Width = tabPage3.Width - 247;
        }

        private void createReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Creating Report...";
            Form18 f18 = new Form18();
            f18.ShowDialog(this);
            listView2_ReadReports();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void deleteReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Deleting Report...";
            Form19 f19 = new Form19();
            f19.ShowDialog(this);
            listView2_ReadReports();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void modifyReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Modifing Report...";
            Form20 f20 = new Form20();
            f20.ShowDialog(this);
            listView2_ReadReports();
            toolStripStatusLabel1.Text = "Working...";
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Options...";
            Form21 f21 = new Form21();
            f21.ShowDialog(this);
            toolStripStatusLabel1.Text = "Working...";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                dataGridView4.DataSource = null;
                dataGridView6.DataSource = null;
                dataGridView7.DataSource = null;
                richTextBox11.Clear();
                richTextBox9.Clear();
                richTextBox12.Clear();
                richTextBox10.Clear();
                richTextBox5.Clear();
                richTextBox6.Clear();
            }
            else
            {
                UseWaitCursor = true;
                BindingSource bindingSource1 = new BindingSource();
                DataSet dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select name_prof, CreationD_prof from Profile where name_prof LIKE '" + listView1.Items[listView1.SelectedIndices[0]].Text + "'");
                richTextBox5.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
                richTextBox6.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();

                //dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "Select f.name_fact, c.Name_crit, m.Name_met from Criteria c, Profile_Criteria pc, [Profile] p, Metric m, Factor f, Factor_criteria fc where p.id_prof = pc.id_prof and c.id_crit = pc.id_crit and m.id_crit = c.id_crit and f.id_fact = fc.id_fact and c.id_crit = fc.id_crit and p.name_prof like '" + listView1.Items[listView1.SelectedIndices[0]].Text + "'");
                dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "Select f.name_fact from Criteria c, Profile_Metric pm, [Profile] p, Metric m, Factor f, Factor_criteria fc where p.id_prof = pm.id_prof and m.id_met = pm.id_met and m.id_crit = c.id_crit and f.id_fact = fc.id_fact and c.id_crit = fc.id_crit and p.name_prof like '" + listView1.Items[listView1.SelectedIndices[0]].Text + "' group by name_fact");
                bindingSource1.DataSource = dataSet1.Tables[0];
                dataGridView7.DataSource = bindingSource1;
                dataGridView_Resize(dataGridView7, null);
                dataGridView4.DataSource = null;
                dataGridView6.DataSource = null;
                UseWaitCursor = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DelegateListView tempDel = e.Argument as DelegateListView;
            tempDel();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form1_unBlock();
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadReports()
        {
            dataSetTemp = dbw1.ReadReports();
            for (int i = 0; i < dataSetTemp.Tables[0].Rows.Count; i++)
            {
                listView2.Items.Add(dataSetTemp.Tables[0].Rows[i].ItemArray[0].ToString());
            }
            dataSetTemp = null;
        }

        private void ReadProfiles()
        {
            dataSetTemp = dbw1.ReadProfiles();
            for (int i = 0; i < dataSetTemp.Tables[0].Rows.Count; i++)
            {
                listView1.Items.Add(dataSetTemp.Tables[0].Rows[i].ItemArray[0].ToString());
            }
            dataSetTemp = null;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count == 0)
            {
                dataGridView8.DataSource = null;
                richTextBox8.Clear();
                richTextBox7.Clear();
                richTextBox13.Clear();
            }
            else
            {
                try
                {
                    dataSet21 = dataSet11.GetChanges();
                    if (dataSet21 != null)
                        adapter.Update(dataSet21);
                }
                catch (Exception)
                {
                    dataSet21 = null;
                }
                UseWaitCursor = true;
                BindingSource bindingSource1 = new BindingSource();
                DataSet dataSet1 = dbw1.ReadDataBaseToDataSet("QUIM", "select progName, CreationD_rep, percofend from Report where progName LIKE '" + listView2.Items[listView2.SelectedIndices[0]].Text + "'");
                richTextBox8.Text = dataSet1.Tables[0].Rows[0].ItemArray[0].ToString();
                richTextBox7.Text = dataSet1.Tables[0].Rows[0].ItemArray[1].ToString();
                richTextBox13.Text = dataSet1.Tables[0].Rows[0].ItemArray[2].ToString();
                dataSet21 = new DataSet();
                adapter = new SqlDataAdapter();
                dataSet11 = dbw1.ReadDataBaseToDataSet("QUIM", "select m.name_met, mr.Curvalue, mr.Type, mr.id_metrInrep from  metric m, MetrInRep mr where m.id_met = mr.id_met and mr.id_rep in (select r.id_rep from report r where r.progName like '" + listView2.Items[listView2.SelectedIndices[0]].Text + "')");
                dataGridView8.DataSource = dataSet11.Tables[0];
                dataGridView8.Columns[0].HeaderText = "NAME METRIC:";
                dataGridView8.Columns[0].ReadOnly = true;
                dataGridView8.Columns[1].HeaderText = "VALUE:";
                dataGridView8.Columns[2].HeaderText = "TYPE:";
                dataGridView8.Columns[3].Visible = false;
                dataGridView8.Columns[3].ReadOnly = true;
                adapter = dbw1.fillDataAdapter(SqlConnectionParametrs.DataBaseName, "select mr.Curvalue, mr.Type, mr.id_metrInrep from MetrInRep mr where mr.id_rep in (select r.id_rep from report r where r.progName like '" + listView2.Items[listView2.SelectedIndices[0]].Text + "')");
                cmdBuilder = new SqlCommandBuilder(adapter);
                dataGridView8_Resize(dataGridView8, null);
                UseWaitCursor = false;
            }
        }
    }
}
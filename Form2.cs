using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AnalizHelperSystem
{
    public partial class Form2 : Form
    {
        static BackgroundWorker bgw = new BackgroundWorker();
        DBwork dbworker; // = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //dbworker.Attatch_DataBase(SqlConnectionParametrs.DataBaseName, @"F:\TEMP\123\QUIM.mdf", @"F:\TEMP\123\QUIM_log.ldf"); //TEMP ATTATCHING!!!!!!!!!!!!!!!!!!!!!

            DBwork dbw1 = new DBwork("QUIM", "");
            DBwork dbw2 = new DBwork("QUIM", "SQLEXPRESS");
            try
            {
                dbw1.ReadDataBaseToDataSet("master", "select * from spt_monitor");
                SqlConnectionParametrs.DataBaseServiceName = "";
            }
            catch (Exception)
            {
                try
                {
                    dbw2.ReadDataBaseToDataSet("master", "select * from spt_monitor");
                    SqlConnectionParametrs.DataBaseServiceName = "SQLEXPRESS";
                }
                catch (Exception)
                {
                    throw new Exception("Data base server 2005 not found.");
                }
            }

            dbworker = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);


            //attatching db
            dbworker.Attatch_DataBase(SqlConnectionParametrs.DataBaseName, Environment.CurrentDirectory + "\\QUIM.mdf", Environment.CurrentDirectory + "\\QUIM_log.ldf");
            //checking struck of db
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from factor");
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from criteria");
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from metric");
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from profile");
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from report");
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from profile_metric");
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from metrInRep");
            dbworker.ReadDataBaseToDataSet(SqlConnectionParametrs.DataBaseName, "select * from Factor_Criteria");
            //collect garb. after temp selecting
            GC.Collect();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.Text = "Error...";
                label1.Visible = false;
                label2.Visible = false;
                pictureBox2.Visible = false;
                button1.Visible = true;
                richTextBox1.Visible = true;
                richTextBox1.Text = e.Error.Message;
            }
            else
            {
                this.Visible = false;
                //run main program Window
                Form1 f1 = new Form1();
                f1.ShowDialog();
                this.Close();
            }
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbworker != null)
                dbworker.Detatch_DataBase(SqlConnectionParametrs.DataBaseName);
            //collect garb. after temp selecting
            GC.Collect();
            Application.Exit();
        }
    }
}
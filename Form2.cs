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
        DBwork dbworker = new DBwork(SqlConnectionParametrs.DataBaseName, SqlConnectionParametrs.DataBaseServiceName);

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

            //attatching db
            dbworker.Attatch_DataBase(SqlConnectionParametrs.DataBaseName, Environment.CurrentDirectory + "\\QUIM.mdf", Environment.CurrentDirectory + "\\QUIM_log.ldf");
            //checking struck of db
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from factor");
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from criteria");
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from metric");
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from profile");
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from report");
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from profile_metric");
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from metrInRep");
            dbworker.ReadDataBaseToDataSet(temp.name_db, "select * from Factor_Criteria");
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
            dbworker.Detatch_DataBase(SqlConnectionParametrs.DataBaseName);
            //collect garb. after temp selecting
            GC.Collect();
            Application.Exit();
        }
    }
}
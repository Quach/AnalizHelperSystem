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
    public struct temp
    {
        public static string name_db = "QUIM";
    }
    public partial class Form19 : Form
    {
        DBwork dbw1 = new DBwork(temp.name_db, "");
        DataSet dataSet1;
        public Form19()
        {
            InitializeComponent();
            dataSet1 = dbw1.ReadReports();
            comboBox1.DataSource = dataSet1.Tables[0];
            comboBox1.DisplayMember = "ProgName";
            comboBox1.ValueMember = "ProgName";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbw1.DeleteReport(comboBox1.GetItemText(comboBox1.Items[comboBox1.SelectedIndex]));
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

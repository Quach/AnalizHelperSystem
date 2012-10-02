using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AnalizHelperSystem
{
    struct SqlConnectionParametrs
    {
        static public string DataBaseName = "QUIM";
        static public string DataBaseServiceName = "";
    }

    public class DBwork
    {
        public string g_dbName;
        public string g_serverName;
        public DBwork()
        {
            g_dbName = "";
            g_serverName = "";
        }
        public DBwork(string _dataBaseName)
        {
            g_dbName = _dataBaseName;
        }
        public DBwork(string _dataBaseName, string _serverName)
        {
            g_dbName = _dataBaseName;
            g_serverName = _serverName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql_name"></param>
        /// <param name="file_mdf"></param>
        /// <param name="file_ldf"></param>
        public void Attatch_DataBase(string sql_name, string file_mdf, string file_ldf)
        {
            string connection_string = @"Data Source=;Initial Catalog=master;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connection_string))
                {
                    string sql_txt = @"if db_id('" + sql_name + "') is null  EXEC sp_attach_db @dbname = '" + sql_name + "', @filename1 = '"
                        + file_mdf + "', @filename2 = '" + file_ldf + "'";
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql_txt;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql_name"></param>
        /// <param name="file_mdf"></param>
        /// <param name="file_ldf"></param>
        public void Detatch_DataBase(string sql_name)
        {
            string connection_string = @"Data Source=;Initial Catalog=master;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connection_string))
                {
                    string sql_txt = @"if db_id('" + sql_name + "') is not null begin ALTER DATABASE QUIM SET SINGLE_USER WITH ROLLBACK IMMEDIATE EXEC sp_detach_db @dbname = '" + sql_name + "', @skipchecks  = 'true', @keepfulltextindexfile = 'true' end";
                    //string sql_txt = @"if db_id('" + sql_name + "') is not null begin EXEC sp_detach_db @dbname = '" + sql_name + "', @skipchecks  = 'true', @keepfulltextindexfile = 'true' end";
                    //string sql_text = "ALTER DATABASE QUIM SET SINGLE_USER";
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql_txt;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Read data from data base to Data Set
        /// </summary>
        /// <param name="_dataBaseName">Name of data base</param>
        /// <param name="_selectString">Select string of T-Sql sintax</param>
        /// <returns></returns>
        public DataSet ReadDataBaseToDataSet(string _dataBaseName, string _selectString)
        {
            SqlConnection _sqlConnection = new SqlConnection();          //Connector 2 db
            SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter();       //
            DataSet _dataSet = new DataSet();                            //4 outputung in2 gridview
            SqlConnectionStringBuilder connectionStringBuilder1 = new SqlConnectionStringBuilder();
            connectionStringBuilder1.DataSource = Environment.MachineName + g_serverName;
            //connectionStringBuilder1.DataSource = Environment.MachineName; // +"\\MSSQLSERVER";
            connectionStringBuilder1.IntegratedSecurity = true;
            connectionStringBuilder1.InitialCatalog = _dataBaseName;
            _sqlConnection.ConnectionString = connectionStringBuilder1.ConnectionString;
            try
            {
                _sqlConnection.Open();
                _sqlDataAdapter = new SqlDataAdapter(_selectString, _sqlConnection);
                _sqlDataAdapter.Fill(_dataSet);
                _sqlConnection.Close();
            }
            catch (Exception err)
            {
                throw (err);
            }
            return _dataSet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dataBaseName"></param>
        /// <param name="_DMLString"></param>
        /// <returns></returns>
        public int ChangeDataInDataBase(string _dataBaseName, string _DMLString)
        {
            int _rowsAffected = 0;
            SqlConnection _sqlConnection = new SqlConnection();          //Connector 2 db
            SqlConnectionStringBuilder connectionStringBuilder1 = new SqlConnectionStringBuilder();
            connectionStringBuilder1.DataSource = Environment.MachineName + g_serverName;
            connectionStringBuilder1.IntegratedSecurity = true;
            connectionStringBuilder1.InitialCatalog = _dataBaseName;
            _sqlConnection.ConnectionString = connectionStringBuilder1.ConnectionString;
            SqlCommand _sqlCommand = new SqlCommand(_DMLString, _sqlConnection);
            try
            {
                _sqlConnection.Open();
                _rowsAffected = _sqlCommand.ExecuteNonQuery();
                _sqlConnection.Close();
            }
            catch (Exception err)
            {
                throw (err);
            }
            return _rowsAffected;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dataBaseName"></param>
        /// <param name="selectStr"></param>
        /// <returns></returns>
        public SqlDataAdapter fillDataAdapter(string _dataBaseName, string selectStr)
        {
            DataSet _dataSet = new DataSet();
            SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter();
            SqlConnection _sqlConnection = new SqlConnection();          //Connector 2 db
            SqlConnectionStringBuilder connectionStringBuilder1 = new SqlConnectionStringBuilder();
            connectionStringBuilder1.DataSource = Environment.MachineName;
            connectionStringBuilder1.IntegratedSecurity = true;
            connectionStringBuilder1.InitialCatalog = _dataBaseName;
            _sqlConnection.ConnectionString = connectionStringBuilder1.ConnectionString;
            try
            {
                _sqlConnection.Open();
                _sqlDataAdapter = new SqlDataAdapter(selectStr, _sqlConnection);
                _sqlDataAdapter.Fill(_dataSet);
                _sqlConnection.Close();
            }
            catch (Exception ee)
            {
                throw (ee);
            }
            return _sqlDataAdapter;
        }
        public int InsertFactor(string _name_fact, string _def_fact)
        {
            int _return = 0;
            _return += ChangeDataInDataBase(g_dbName, "INSERT INTO dbo.Factor (Name_fact,Def_fact) VALUES (\'" + _name_fact + "\', \'" + _def_fact + "\')");
            return _return;
        }
        public int InsertCriteria(string _name_crit, string _def_crit, String[] _name_fact)
        {
            int _return = 0;
            _return += ChangeDataInDataBase(g_dbName, "INSERT INTO dbo.Criteria (Name_crit,Def_crit) VALUES (\'" + _name_crit + "\', \'" + _def_crit + "\')");
            for (int i = 0; i < _name_fact.Count(); i++)
            {
                _return += ChangeDataInDataBase(g_dbName, "insert into factor_criteria (id_fact, id_crit) select (select id_fact from factor where name_fact like \'" + _name_fact[i] + "\'),(select id_crit from criteria where name_crit like \'" + _name_crit + "\')");
            }
            return _return;
        }
        public int InsertMetric(string _name_met, string _def_met, string _formula_met, string _unit_met, string _name_crit)
        {
            int _return = 0;
            _return += ChangeDataInDataBase(g_dbName, "INSERT INTO dbo.Metric (Name_met, Def_met, Interpretation_met, Formula_met, Id_crit) select (\'" + _name_met + "\'), (\'" + _def_met + "\'), (\'" + _unit_met + "\'), (\'" + _formula_met + "\'), (select id_crit from criteria where name_crit like \'" + _name_crit + "\')");
            return _return;
        }
        public DataSet ReadFactors()
        {
            return ReadDataBaseToDataSet("QUIM", "Select name_fact from Factor");
        }
        public DataSet ReadCriterias(string _nameFactor)
        {
            return ReadDataBaseToDataSet("QUIM", "select c.Name_Crit from Factor f, Criteria c, Factor_Criteria fc where f.id_fact = fc.id_fact and c.id_crit = fc.id_crit and f.name_fact like \'" + _nameFactor + "\'");
        }
        public DataSet ReadMetrics(string _nameCriteria)
        {
            return ReadDataBaseToDataSet("QUIM", "select m.Name_met from Criteria c, Metric m where m.id_crit = c.id_crit and c.name_crit LIKE \'" + _nameCriteria + "\'");
        }
        public DataSet ReadCriterias()
        {
            return ReadDataBaseToDataSet("QUIM", "select c.Name_Crit from Criteria c");
        }
        public DataSet ReadMetrics()
        {
            return ReadDataBaseToDataSet("QUIM", "select m.Name_met from Metric m");
        }
        public DataSet ReadProfiles()
        {
            return ReadDataBaseToDataSet("QUIM", "Select name_prof from Profile");
        }
        public void DeleteFactor(string _nameFactor)
        {
            ChangeDataInDataBase(g_dbName, "delete from Factor where name_fact like '" + _nameFactor + "'");
        }
        public void DeleteCriteria(string _nameCriteria)
        {
            ChangeDataInDataBase(g_dbName, "delete from Criteria where name_crit like '" + _nameCriteria + "'");
        }
        public void DeleteMetric(string _nameMetric)
        {
            ChangeDataInDataBase(g_dbName, "delete from Metric where name_met like '" + _nameMetric + "'");
        }
        public int InsertProfile(string _name_profile)
        {
            int _return = 0;
            _return += ChangeDataInDataBase(g_dbName, "INSERT INTO dbo.Profile (Name_prof,creationD_prof) VALUES (\'" + _name_profile + "\', 2342645)");
            return _return;
        }
        public void DeleteProfile(string _name_profile)
        {
            ChangeDataInDataBase(g_dbName, "delete from Profile where name_prof like '" + _name_profile + "'");
        }
        public DataSet ReadReports()
        {
            return ReadDataBaseToDataSet("QUIM", "select r.progName from Report r");
        }
        public void DeleteReport(string _name_report)
        {
            ChangeDataInDataBase(g_dbName, "delete from Report where progName like '" + _name_report + "'");
        }
        public int UpdateFactor(string _name_factor, string _def_factor, string _last_name_factor)
        {
            return ChangeDataInDataBase("QUIM", "update Factor set name_fact = '" + _name_factor + "', def_fact = '" + _def_factor + "' where name_fact like '" + _last_name_factor + "'");
        }
        public void UpdateMetrics(string _name_prog, String[] _name_metric)
        {
            //???????????????????
            DataSet dataSet;
            int[] id_metrInRep = new int[_name_metric.Count()];
            for (int i = 0; i < _name_metric.Count(); i++)
            {
                dataSet = ReadDataBaseToDataSet(g_dbName, "select mr.id_metrInRep from metrInRep mr, report r, metric m where r.id_rep = mr.id_rep and m.id_met = mr.id_met and r.progName like '" + _name_prog + "' and m.name_met like '" + _name_metric[i] + "'");
                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    ChangeDataInDataBase(g_dbName, "insert into metrInRep (id_rep, id_met) select (select id_rep from report where progName like '" + _name_prog + "'),(select id_met from metric where name_met like '" + _name_metric[i] + "')");
                    dataSet = ReadDataBaseToDataSet(g_dbName, "select mr.id_metrInRep from metrInRep mr, report r, metric m where r.id_rep = mr.id_rep and m.id_met = mr.id_met and r.progName like '" + _name_prog + "' and m.name_met like '" + _name_metric[i] + "'");
                }
                id_metrInRep[i] = Int32.Parse(dataSet.Tables[0].Rows[0].ItemArray[0].ToString());
            }
            dataSet = ReadDataBaseToDataSet(g_dbName, "select mr.id_metrInRep from metrInRep mr, report r where r.id_rep = mr.id_rep and r.progName like '" + _name_prog + "'");
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                if (!id_metrInRep.Contains(Int32.Parse(dataSet.Tables[0].Rows[i].ItemArray[0].ToString())))
                {
                    ChangeDataInDataBase(g_dbName, "delete from MetrInRep where id_metrInRep = " + dataSet.Tables[0].Rows[i].ItemArray[0].ToString());
                }
            }
        }
        public void UpdateCriterias(string _name_criteria, string _def_criteria, String[] _name_factor, string _last_name_criteria)
        {
            ChangeDataInDataBase(g_dbName, "delete from factor_criteria where id_crit in (select id_crit from criteria where name_crit like \'" + _last_name_criteria + "\')");
            for (int i = 0; i < _name_factor.Count(); i++)
            {
                ChangeDataInDataBase(g_dbName, "insert into factor_criteria (id_fact, id_crit) select (select id_fact from factor where name_fact like \'" + _name_factor[i] + "\'),(select id_crit from criteria where name_crit like \'" + _last_name_criteria + "\')");
            }
            ChangeDataInDataBase("QUIM", "update Criteria set name_crit = '" + _name_criteria + "', def_crit = '" + _def_criteria + "' where name_crit like '" + _last_name_criteria + "'");

        }
        public DataSet ReadNameDefFactor(string _name_factor)
        {
            return ReadDataBaseToDataSet("QUIM", "select name_fact, def_fact from Factor where name_fact like '" + _name_factor + "'");
        }
        public DataSet ReadNameDefCriteria(string _name_criteria)
        {
            return ReadDataBaseToDataSet("QUIM", "select name_crit, def_crit from Criteria where name_crit like '" + _name_criteria + "'");
        }
        public DataSet ReadFactors(string _name_crit)
        {
            return ReadDataBaseToDataSet("QUIM", "Select f.name_fact from Criteria c, Factor f, Factor_criteria fc where c.id_crit = fc.id_crit and f.id_fact = fc.id_fact and c.name_crit like '" + _name_crit + "'");
        }
        public DataSet ReadNameDefFormUnitMetric(string _name_metric)
        {
            return ReadDataBaseToDataSet("QUIM", "select name_met, def_met, formula_met, interpretation_met from Metric where name_met like '" + _name_metric + "'");
        }
        public int UpdateMetric(string _name_metric, string _def_metric, string _formula_metric, string _unit_metric, string _last_name_metric)
        {
            return ChangeDataInDataBase("QUIM", "update Metric set name_met = '" + _name_metric + "', def_met = '" + _def_metric + "', formula_met = '" + _formula_metric + "', interpretation_met = '" + _unit_metric + "' where name_met like '" + _last_name_metric + "'");
        }
        public int UpdateMetricsProfile(string _name_profile, String[] _name_metric, int _number_of_insert)
        {
            int _return = 0;
            for (int i = 0; i < _number_of_insert; i++)
            {
                _return += ChangeDataInDataBase(g_dbName, "insert into profile_metric (id_prof, id_met) select id_prof, id_met from metric, [profile] where name_prof like '" + _name_profile + "' and name_met like '" + _name_metric[i] + "'");
            }
            return _return;
        }


        ////////ADDICTED!
        private double GetUniverseNumber(int CurrentValue, int MinValue, int MaxValue, double rate)
        {
            double value = 0.0;
            if (MinValue >= MaxValue)
                throw new Exception("Min value must be lower that the max value.");
            if (!(rate >= 0.0 && rate <= 1.0))
                throw new Exception("Rate must be in [0.0, 1.0].");
            if (!(CurrentValue >= MinValue && CurrentValue <= MaxValue))
                throw new Exception("Current value must be grather then min value and less then max value.");
            value = ((CurrentValue - MinValue) / (MaxValue - MinValue)) * rate;
            return value;
        }

        private double GetUniverseNumber(int CurrentValue, int MinValue, int MaxValue)
        {
            return GetUniverseNumber(CurrentValue, MinValue, MaxValue, 1.0);
        }
    }
}

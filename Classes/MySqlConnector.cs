using Astro.Library.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon
{
    public class MySqlConnectorX
    {
        private MySqlConnection mysqlConn;
        private MySqlCommand mysqlCmd;
        private String sqlStatus;

        public String SqlStatus
        {
            get { return sqlStatus; }
            set
            {
                sqlStatus = value;
                if (MessageHandler != null) { MessageHandler(this, new SqlArgs(sqlStatus)); }
            }
        }

        public event EventHandler<SqlArgs> MessageHandler;

        private String ConnectionString { get; set; }

        public MySqlConnectorX()
        {
            try
            {
                String server = "10.0.1.252";
                String database = "astrodon_co_za";
                String uid = "root";
                String password = "66r94e77";
                String connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                ConnectionString = connectionString;
                KillSleepingConnections();
                mysqlConn = new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                Controller.HandleError(ex);
            }
        }

        private int KillSleepingConnections()
        {
            string strSQL = "show processlist";
            System.Collections.ArrayList m_ProcessesToKill = new ArrayList();
            String status;
            DataSet ds = GetData(strSQL, null, out status);

            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        // Find all processes sleeping with a timeout value higher than our threshold.
                        int iPID = Convert.ToInt32(dr["Id"].ToString());
                        string strState = dr["Command"].ToString();
                        int iTime = Convert.ToInt32(dr["Time"].ToString());

                        if (strState == "Sleep" && iPID > 0)
                        {
                            // This connection is sitting around doing nothing. Kill it.
                            m_ProcessesToKill.Add(iPID);
                        }
                    }
                }

                foreach (int aPID in m_ProcessesToKill)
                {
                    strSQL = "kill " + aPID;
                    SetData(strSQL, null, out status);
                }
            }
            catch (Exception excep)
            {
                Controller.HandleError(excep);
            }
            finally
            {
            }
            return m_ProcessesToKill.Count;
        }

        public bool ToggleConnection(bool open)
        {
            bool success = false;
            try
            {
                if (open)
                {
                    if (mysqlConn.State != System.Data.ConnectionState.Open) { mysqlConn.Open(); }
                    success = mysqlConn.State == System.Data.ConnectionState.Open;
                }
                else
                {
                    if (mysqlConn.State != System.Data.ConnectionState.Closed) { mysqlConn.Close(); }
                    success = mysqlConn.State == System.Data.ConnectionState.Closed;
                }
            }
            catch { }
            return success;
        }

        private bool InsertStatement(String title, String description, String file, String unitno, String[] emails)
        {
            description = "Customer Statements";
            String status = String.Empty;
            String cQuery = "SELECT * FROM tx_astro_docs WHERE file = @file";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@file", file);
            DataSet cDS = GetData(cQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = status; }
            if (cDS != null && cDS.Tables.Count > 0 && cDS.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                String cruser_id = "0";
                if (emails.Length > 0)
                {
                }
                sqlParms.Add("@cruser_id", cruser_id);
                sqlParms.Add("@title", title);
                sqlParms.Add("@description", description);
                sqlParms.Add("@unitno", unitno);
                String query = "INSERT INTO tx_astro_docs(pid, tstamp, crdate, cruser_id, title, description, file, unitno)";
                query += " VALUES(1, UNIX_TIMESTAMP(now()),UNIX_TIMESTAMP(now()),@cruser_id,@title,@description,@file,@unitno);";
                return SetData(query, sqlParms, out status);
            }
        }
    
        private String GetPID(String title)
        {
            String query = "SELECT uid FROM pages WHERE title = '" + title + "'";
            String status = "";
            DataSet pidDS = GetData(query, null, out status);
            if (pidDS != null && pidDS.Tables.Count > 0 && pidDS.Tables[0].Rows.Count > 0)
            {
                return pidDS.Tables[0].Rows[0]["uid"].ToString();
            }
            else
            {
                return "";
            }
        }

        private DataSet GetData(String query, Dictionary<String, Object> sqlParms, out String status)
        {
            DataSet ds = new DataSet();
            if (Environment.MachineName == "PASTELPARTNER")
            {
                status = "";
                return ds;
            }
            using (MySqlConnection connect = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connect))
                {
                    try
                    {
                        if (connect.State != ConnectionState.Open) { connect.Open(); }
                        if (sqlParms != null)
                        {
                            foreach (KeyValuePair<String, Object> sqlParm in sqlParms)
                            {
                                cmd.Parameters.AddWithValue(sqlParm.Key, sqlParm.Value);
                            }
                        }
                        MySql.Data.MySqlClient.MySqlDataAdapter da = new MySqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        status = "OK";
                        connect.Close();
                    }
                    catch (Exception ex)
                    {
                        ds = null;
                        status = ex.Message;
                        Controller.HandleError(ex, "MySQL Connector Error");
                        //MessageBox.Show(status);
                    }
                    finally
                    {
                        if (connect.State != ConnectionState.Closed) { connect.Close(); }
                    }
                }
            }
            return ds;
        }

        private DataSet GetFiles(String unitno, String buildingName)
        {
            String query = "SELECT d.tstamp, d.title, d.file FROM tx_astro_docs d inner join tx_astro_account_user_mapping m on d.unitno = m.account_no and d.cruser_id = m.cruser_id";
            query += " where d.unitno = @unitno and m.complex_name = @buildingName ORDER BY d.tstamp DESC";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@unitno", unitno);
            sqlParms.Add("@buildingName", buildingName);
            String status = "";
            DataSet fileDS = GetData(query, sqlParms, out status);
            //MessageBox.Show(status);
            return fileDS;
        }

        private DataSet GetFilesRental(String unitno)
        {
            String rentalUnitNo = unitno + "R";
            String query = "SELECT d.tstamp, d.title, d.file FROM tx_astro_docs d where (d.unitno = @rentalUnitNo) OR (d.unitno = @unitno AND cruser_id = 0) ORDER BY d.tstamp DESC";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@unitno", unitno);
            sqlParms.Add("@rentalUnitNo", rentalUnitNo);
            String status = "";
            DataSet fileDS = GetData(query, sqlParms, out status);
            if (status != "OK")
            {
                MessageBox.Show(status);
            }
            return fileDS;
        }

        private DataSet GetCustomerDocs(String buildingName, out String status)
        {
            if (buildingName.ToUpper().StartsWith("VILLAGE GREEN")) { buildingName = "VILLAGE GREEN B/C"; }
            String query = "SELECT d.uid, d.tstamp, d.title, d.file, d.unitno FROM tx_astro_docs d inner join tx_astro_account_user_mapping m on d.unitno = m.account_no";
            query += " inner join tx_astro_complex c on m.complex_id = c.uid where c.name = '" + buildingName + "' order by d.tstamp";
            return GetData(query, null, out status);
        }

        private bool SetData(String query, Dictionary<String, Object> sqlParms, out String status)
        {
            bool success = false;
            using (MySqlConnection connect = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connect))
                {
                    try
                    {
                        if (connect.State != ConnectionState.Open) { connect.Open(); }
                        if (sqlParms != null)
                        {
                            foreach (KeyValuePair<String, Object> sqlParm in sqlParms)
                            {
                                cmd.Parameters.AddWithValue(sqlParm.Key, sqlParm.Value);
                            }
                        }
                        cmd.ExecuteNonQuery();
                        success = true;
                        status = "OK";
                        connect.Close();
                    }
                    catch (Exception ex)
                    {
                        status = ex.Message;
                       // Controller.HandleError(ex);
                    }
                    finally
                    {
                        if (connect.State != ConnectionState.Closed) { connect.Close(); }
                    }
                }
            }
            return success;
        }

        private String[] HasLogin(String email)
        {
            String query = "SELECT uid, usergroup FROM fe_users WHERE username = '" + email + "' AND usergroup <> '1,2,6'";
            String[] returners = new string[2];
            String status;
            DataSet ds = GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                returners[0] = ds.Tables[0].Rows[0]["uid"].ToString();
                returners[1] = ds.Tables[0].Rows[0]["usergroup"].ToString();
                return returners;
            }
            else
            {
                return null;
            }
        }

        private bool UpdateGroup(String uid, String group)
        {
            String query = "UPDATE fe_users SET usergroup = '" + group + "' WHERE uid = " + uid;
            String status;
            return SetData(query, null, out status);
        }
    }
}
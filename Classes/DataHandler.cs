using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Astrodon
{
    public class SqlDataHandler
    {
        private static String connStringDefault = "Data Source=SERVER-SQL;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=@str0d0n"; //Astrodon
        private static String connStringL = "Data Source=STEPHEN-PC\\MTDNDSQL;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=m3t@p@$$"; //Local

      
        private static String connStringD = "Data Source=DEVELOPERPC\\SQLEXPRESS;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=$DEVELOPER$"; //Astrodon
        private static String connStringLocal = "Data Source=.;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=1q2w#E$R"; //LamaDev
        public string  connString = null;

        public SqlConnection sqlConnection;
        private SqlCommand cmd;

        public SqlDataHandler()
        {
            connString = GetConnectionString();

            sqlConnection = new SqlConnection(connString);
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandType = CommandType.Text;
        }

        public static string ASTRODON_Path
        {
            get
            {
                string sql = "select business from tblSettings";
                SqlDataHandler dh = new SqlDataHandler();
                string status;
                DataSet ds = dh.GetData(sql, null, out status);

                return ds.Tables[0].Rows[0]["business"] as string;
            }
        }

        public static string CENTRE_Path
        {
            get
            {
                string sql = "select centrec from tblSettings";
                SqlDataHandler dh = new SqlDataHandler();
                string status;
                DataSet ds = dh.GetData(sql, null, out status);

                return ds.Tables[0].Rows[0]["centrec"] as string;
            }
        }

        public static string GetConnectionString()
        {
            if (Environment.MachineName == "STEPHEN-PC")
            {
                return connStringL;
            }
            else if (Environment.MachineName == "DEVELOPERPC")
            {
                return connStringD;
            }
            else if (Environment.MachineName == "PASTELPARTNER")
                return connStringLocal;
            return connStringDefault;
        }

        public static string GetClientPortalConnectionString()
        {
            string result = GetConnectionString();
            result = result.Replace("Initial Catalog=Astrodon;", "Initial Catalog=ClientPortal;");
            return result;
        }

        #region Entity Framework Hooks
        public static Astrodon.Data.DataContext GetDataContext()
        {
            var dbContext = new Astrodon.Data.DataContext(GetConnectionString());
            return dbContext;
        }

        public static void MigrateEFDataBase()
        {
            Astrodon.Data.DataContext.Setup(GetConnectionString());
        }
        #endregion


        public DataSet GetData(String sqlQuery, Dictionary<String, Object> sqlParms, out String status)
        {
            if(string.IsNullOrWhiteSpace(sqlQuery))
            {
                status = null;
                return null;
            }
            DataSet ds = new DataSet();
            try
            {
                cmd.CommandText = sqlQuery;
                if (sqlParms != null)
                {
                    cmd.Parameters.Clear();
                    foreach (KeyValuePair<String, Object> sqlParm in sqlParms)
                    {
                        cmd.Parameters.AddWithValue(sqlParm.Key, sqlParm.Value);
                    }
                }
                if (sqlConnection.State != ConnectionState.Open) { sqlConnection.Open(); }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                status = "OK";
            }
            catch (Exception ex)
            {
                status = ex.Message;
                ds = null;
            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Closed) { sqlConnection.Close(); }
            }
            return ds;
        }

        public int SetData(String sqlQuery, Dictionary<String, Object> sqlParms, out String status)
        {
            int rs = -1;
            try
            {
                cmd.CommandText = sqlQuery;
                if (sqlParms != null)
                {
                    cmd.Parameters.Clear();
                    foreach (KeyValuePair<String, Object> sqlParm in sqlParms)
                    {
                        cmd.Parameters.AddWithValue(sqlParm.Key, sqlParm.Value);
                    }
                }
                if (sqlConnection.State != ConnectionState.Open) { sqlConnection.Open(); }
                rs = cmd.ExecuteNonQuery();
                status = "";
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Closed) { sqlConnection.Close(); }
            }
            return rs;
        }

        public int SetData(String sqlQuery, Dictionary<String, Object> sqlParms, bool scalar, out String status)
        {
            int rs = -1;
            try
            {
                cmd.CommandText = sqlQuery;
                if (sqlParms != null)
                {
                    cmd.Parameters.Clear();
                    foreach (KeyValuePair<String, Object> sqlParm in sqlParms)
                    {
                        cmd.Parameters.AddWithValue(sqlParm.Key, sqlParm.Value);
                    }
                }
                if (sqlConnection.State != ConnectionState.Open) { sqlConnection.Open(); }
                Object obj = cmd.ExecuteScalar();
                rs = int.Parse(obj.ToString());
                status = "";
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Closed) { sqlConnection.Close(); }
            }
            return rs;
        }

        public int SaveOutboundMessage(int id, String building, String customer, String number, String reference, String message, bool billable, bool bulkbillable,
    DateTime sent, String sender, String astStatus, String batchID, String status, DateTime nextPolled, int pollCount, double cbal, String smsType, out String msg)
        {
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@id", id);
            sqlParms.Add("@building", building);
            sqlParms.Add("@customer", customer);
            sqlParms.Add("@number", number);
            sqlParms.Add("@reference", reference);
            sqlParms.Add("@message", message);
            sqlParms.Add("@billable", billable);
            sqlParms.Add("@bulkbillable", bulkbillable);
            sqlParms.Add("@sent", sent);
            sqlParms.Add("@sender", sender);
            sqlParms.Add("@astStatus", astStatus);
            sqlParms.Add("@batchID", batchID);
            sqlParms.Add("@status", "-1");
            sqlParms.Add("@cbal", cbal);
            sqlParms.Add("@smstype", smsType);
            sqlParms.Add("@nextPolled", (id == 0 ? DateTime.Now.AddMinutes(5) : nextPolled));
            sqlParms.Add("@pollCount", (id == 0 ? 0 : pollCount));

            if (id == 0)
            {
                String insertQuery = "INSERT INTO tblSMS(building, customer, number, reference, message, billable, bulkbillable, sent, sender, astStatus, batchID, status, nextPolled, pollCount, currentBalance, smsType) ";
                insertQuery += " VALUES(@building, @customer, @number, @reference, @message, @billable, @bulkbillable, @sent, @sender, @astStatus, @batchID, @status, @nextPolled, @pollCount, @cbal, @smstype);";
                insertQuery += " SELECT @@IDENTITY;";
                DataSet ds = GetData(insertQuery, sqlParms, out msg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                String updateQuery = "UPDATE tblSMS SET building = @building, customer = @customer, number = @number, reference = @reference, message = @message, billable = @billable, ";
                updateQuery += " bulkbillable = @bulkbillable, sent = @sent, sender = @sender, astStatus = @astStatus, batchID = @batchID, status = @status, nextPolled = @nextPolled, ";
                updateQuery += " pollCount = @pollCount, currentBalance = @cbal, smsType = @smstype WHERE id = @id";
                return SetData(updateQuery, sqlParms, out msg);
            }
        }

        public int SaveQueuedMessage(String building, String customer, double currentBalance, String smsType, String number, String reference, String message, bool billable, bool bulkbillable, DateTime sent,
    String sender, String astStatus, String batchID, String status)
        {
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();

            sqlParms.Add("@building", building);
            sqlParms.Add("@customer", customer);
            sqlParms.Add("@number", number);
            sqlParms.Add("@reference", reference);
            sqlParms.Add("@message", message);
            sqlParms.Add("@billable", billable);
            sqlParms.Add("@bulkbillable", bulkbillable);
            sqlParms.Add("@sent", sent);
            sqlParms.Add("@sender", sender);
            sqlParms.Add("@astStatus", astStatus);
            sqlParms.Add("@batchID", batchID);
            sqlParms.Add("@status", status);
            sqlParms.Add("@cbal", currentBalance);
            sqlParms.Add("@smstype", smsType);
            sqlParms.Add("@nextPolled", sent);
            sqlParms.Add("@pollCount", 0);

            String insertQuery = "INSERT INTO tblSMS(building, customer, number, reference, message, billable, bulkbillable, sent, sender, astStatus, batchID, status, nextPolled, pollCount, currentBalance, smsType) ";
            insertQuery += " VALUES(@building, @customer, @number, @reference, @message, @billable, @bulkbillable, @sent, @sender, @astStatus, @batchID, @status, @nextPolled, @pollCount, @cbal, @smstype);";
            insertQuery += " SELECT @@IDENTITY;";
            DataSet ds = GetData(insertQuery, sqlParms, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }


   
    }
}
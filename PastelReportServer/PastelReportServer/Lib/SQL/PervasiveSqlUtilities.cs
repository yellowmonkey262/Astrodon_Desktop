﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Lib.Pervasive
{
    public class PervasiveSqlUtilities
    {
        private static string odbcName = "Pervasive";

        public static string ReadResourceScript(string path)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string result = string.Empty;

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        public static DataTable FetchPervasiveData(string sql, OdbcParameter param1)
        {
            try { 
            DataTable table = new DataTable();
            string strAccessConn = @"Dsn=" + odbcName + ";";
            using (var conn = new OdbcConnection(strAccessConn))
            {
                conn.ConnectionTimeout = 600;
                using (var cmd = new OdbcCommand(sql, conn))
                {
                    if (param1 != null)
                        cmd.Parameters.Add(param1);

                    OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(cmd);
                    cmd.CommandTimeout = 0; //no wait time
                    conn.Open();
                    myDataAdapter.Fill(table);
                    conn.Close();
                }
            }
            return table;
            }
            catch (Exception ex2) { throw new Exception("DB Exception running qry on " + odbcName); }
        }


        public static DataTable FetchPervasiveData(string sql, List<OdbcParameter> parameters)
        {
            try
            {
                DataTable table = new DataTable();
                string strAccessConn = @"Dsn=" + odbcName + ";";
                using (var conn = new OdbcConnection(strAccessConn))
                {
                    conn.ConnectionTimeout = 600;
                    using (var cmd = new OdbcCommand(sql, conn))
                    { 
                        if (parameters != null && parameters.Count > 0)
                            cmd.Parameters.AddRange(parameters.ToArray());

                        OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(cmd);
                        cmd.CommandTimeout = 0; //no wait time
                        conn.Open();
                        myDataAdapter.Fill(table);
                        conn.Close();
                    }
                }
                return table;
            }
            catch (Exception ex2) { throw new Exception("DB Exception running qry on " + odbcName); }
        }


        public static DataTable FetchPervasiveData(string sql)
        {
            return FetchPervasiveData(sql, null as List<OdbcParameter>);
        }

        public static void ExecuteSQLCommand(string sql)
        {
            string strAccessConn = @"Dsn=" + odbcName + ";";
            using (var conn = new OdbcConnection(strAccessConn))
            {
                conn.ConnectionTimeout = 600;
                using (var noneCmd = new OdbcCommand(sql, conn))
                {
                    conn.Open();
                    noneCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static string SetDataSource(string sqlQuery, string dataPath)
        {
            //if (System.AppDomain.CurrentDomain.FriendlyName == "SelfHosted.exe" || System.Diagnostics.Debugger.IsAttached)
            //    return sqlQuery = sqlQuery.Replace("[DataSet].", "");

            return sqlQuery.Replace("[DataSet].", "PAS11" + dataPath + ".");
        }
    }
}

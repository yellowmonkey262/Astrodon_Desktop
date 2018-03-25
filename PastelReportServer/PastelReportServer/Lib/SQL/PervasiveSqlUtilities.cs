using System;
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
        private static string odbcName= "Pervasive";

        public static string ReadResourceScript(string path)
        {
            var assembly =  System.Reflection.Assembly.GetExecutingAssembly();
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

        public static DataTable FetchPervasiveData(string sql,OdbcParameter param1)
        {
            DataTable table = new DataTable();
            string strAccessConn = @"Dsn="+odbcName+";";
            using (var conn = new OdbcConnection(strAccessConn))
            {
                conn.ConnectionTimeout = 600;
                using (var cmd = new OdbcCommand(sql, conn))
                {
                    if(param1 != null)
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

        public static DataTable FetchPervasiveData(string sql)
        {
            return FetchPervasiveData(sql, null);
        }

        public static string SetDataSource(string sqlQuery, string dataPath)
        {
            if (System.AppDomain.CurrentDomain.FriendlyName == "SelfHosted.exe" || System.Diagnostics.Debugger.IsAttached)
                return sqlQuery = sqlQuery.Replace("[DataSet].", "");

            return sqlQuery.Replace("[DataSet].", "PAS11" + dataPath + ".");
        }
    }
}

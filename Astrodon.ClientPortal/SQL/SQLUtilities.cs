using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Astrodon.ClientPortal.SQL
{
    public class SQLUtilities
    {
        public static string ReadResourceScript(Assembly assembly, string path)
        {
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

        public static DataSet FetchData(string connectionString, string sqlCommand, List<SqlParameter> parameters)
        {
            DataSet result = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    result = FetchData(conn, sqlCommand, parameters);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public static DataSet FetchData(SqlConnection connection, string sqlCommand, List<SqlParameter> parameters)
        {
            DataSet result = new DataSet();

            using (SqlCommand cmd = new SqlCommand(sqlCommand, connection))
            {
                cmd.CommandTimeout = 600;
                foreach (var parm in parameters)
                    cmd.Parameters.Add(parm);
                using (SqlDataAdapter adaptor = new SqlDataAdapter(cmd))
                {
                    adaptor.Fill(result);
                }
            }
            return result;
        }

        public static void ExecuteSqlCommand(SqlConnection connection, string sql, List<SqlParameter> parameters = null)
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = sql;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandTimeout = 600; //Ten minutes
                if (parameters != null)
                    sqlCommand.Parameters.AddRange(parameters.ToArray());

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void ExecuteSqlCommand(string connectionString, string sql, List<SqlParameter> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    ExecuteSqlCommand(connection, sql, parameters);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }
}

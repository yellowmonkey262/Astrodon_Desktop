using Desktop.Lib.Pervasive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Astrodon.TransactionSearch
{
    public class TransactionSearch
    {
        public static List<TransactionDataItem> SearchPastel(string buildingPath,
            DateTime fromDate,DateTime toDate,
            string reference,
            string description,
            decimal? minimumAmount,
            decimal? maximumAmount)
        {
            List<TransactionDataItem> result = new List<TransactionDataItem>();

            string sqlQuery = PervasiveSqlUtilities.ReadResourceScript("Astrodon.TransactionSearch.TransactionSearch.sql");
            sqlQuery = PervasiveSqlUtilities.SetDataSource(sqlQuery, buildingPath);

            if (!string.IsNullOrWhiteSpace(reference))
                sqlQuery = sqlQuery = sqlQuery + " and Refrence like '%" + reference + "%' ";

            if (!string.IsNullOrWhiteSpace(description))
                sqlQuery = sqlQuery = sqlQuery + " and description like '%" + description + "%' ";


            if (minimumAmount != null)
                sqlQuery = sqlQuery = sqlQuery + " and Abs(Amount) >= " + minimumAmount.Value.ToString("#0.00", CultureInfo.InstalledUICulture);

            if (maximumAmount != null)
                sqlQuery = sqlQuery = sqlQuery + " and Abs(Amount) <= " + maximumAmount.Value.ToString("#0.00", CultureInfo.InstalledUICulture);

            var p1 = new OdbcParameter("FromDate", OdbcType.Date);
            var p2 = new OdbcParameter("ToDate", OdbcType.Date);
            p1.Value = fromDate.Date;
            p2.Value = toDate.Date;
            List<OdbcParameter> parameters = new List<OdbcParameter>()
            {
               p1,
               p2
            };

            var dt = PervasiveSqlUtilities.FetchPervasiveData(sqlQuery, parameters);

            foreach(DataRow row in dt.Rows)
            {
                result.Add(new TransactionDataItem(row, buildingPath));
            }
            return result.OrderBy(a => a.TransactionDate).ToList();
        }
    }
}

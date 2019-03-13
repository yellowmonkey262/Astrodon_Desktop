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
                sqlQuery = sqlQuery = sqlQuery + " and Refrence = '%" + reference + "%' ";

            if (!string.IsNullOrWhiteSpace(description))
                sqlQuery = sqlQuery = sqlQuery + " and description = '%" + description + "%' ";


            if (minimumAmount != null)
                sqlQuery = sqlQuery = sqlQuery + " and Abs(Amount) >= " + minimumAmount.Value.ToString("#0.00", CultureInfo.InstalledUICulture);

            if (maximumAmount != null)
                sqlQuery = sqlQuery = sqlQuery + " and Abs(Amount) <= " + maximumAmount.Value.ToString("#0.00", CultureInfo.InstalledUICulture);

            List<OdbcParameter> parameters = new List<OdbcParameter>()
            {
                new OdbcParameter("FromDate",fromDate),
                new OdbcParameter("ToDate",toDate),
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

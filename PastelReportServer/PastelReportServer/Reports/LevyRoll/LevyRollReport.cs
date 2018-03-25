using Desktop.Lib.Pervasive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Astrodon.Reports.LevyRoll
{
    public class LevyRollReport
    {
        public byte[] RunReport(DateTime processMonth, string buildingName, string dataPath, bool includeSundries)
        {
            DateTime dDate = new DateTime(processMonth.Year, processMonth.Month, 1);
            int period;
            List<LevyRollDataItem> data = LoadReportData(processMonth, dataPath,null,  out period);

            List<SundryDataItem> sundries = new List<SundryDataItem>();
            if (includeSundries)
            {
                string sundriesQry = PervasiveSqlUtilities.ReadResourceScript("Astrodon.Reports.Scripts.Sundries.sql");
                sundriesQry = SetDataSource(sundriesQry, dataPath);
                var reportDB = PervasiveSqlUtilities.FetchPervasiveData(sundriesQry, new OdbcParameter("@PPeriod", period));
                foreach (DataRow row in reportDB.Rows)
                {
                    sundries.Add(new SundryDataItem(row));
                }
            }

            return RunReportToPdf(data, sundries, dDate, buildingName);
        }

        public List<LevyRollDataItem> LoadReportData(DateTime processMonth, string dataPath,List<string> customers, out int period)
        {
            string customerCodeFilter = "";
            if(customers != null && customers.Count > 0)
            {
                for(int x=0; x< customers.Count; x++)
                {
                    if (x == 0)
                        customerCodeFilter = "  m.CustomerCode in ('" + customers[x] + "'";
                    else
                        customerCodeFilter = customerCodeFilter + ",'" + customers[x] + "'";
                }
                customerCodeFilter = customerCodeFilter + ")";
            }

            List<LevyRollDataItem> data;
            var dDate = new DateTime(processMonth.Year, processMonth.Month, 1);
            string sqlPeriodConfig = PervasiveSqlUtilities.ReadResourceScript("Astrodon.Reports.Scripts.PeriodParameters.sql");
            sqlPeriodConfig = SetDataSource(sqlPeriodConfig, dataPath);
            var periodData = PervasiveSqlUtilities.FetchPervasiveData(sqlPeriodConfig, null);
            PeriodDataItem periodItem = null;
            foreach (DataRow row in periodData.Rows)
            {
                periodItem = new PeriodDataItem(row);
                break;
            }
            period = 0;
            try
            {
                period = periodItem.PeriodNumberLookup(dDate);
            }
            catch (Exception err)
            {
                throw err;
            }


            //run the main report query
            string sqlQuery = PervasiveSqlUtilities.ReadResourceScript("Astrodon.Reports.Scripts.LevyRollAllCustomers.sql");
            sqlQuery = SetDataSource(sqlQuery, dataPath);
            if (!string.IsNullOrWhiteSpace(customerCodeFilter))
                sqlQuery = sqlQuery.Replace(" %CUSTOMERCODEFILTER%", " WHERE " + customerCodeFilter);
            else
                sqlQuery = sqlQuery.Replace(" %CUSTOMERCODEFILTER%", "");


            var allMasterAccounts = PervasiveSqlUtilities.FetchPervasiveData(sqlQuery, null);

            sqlQuery = PervasiveSqlUtilities.ReadResourceScript("Astrodon.Reports.Scripts.LevyRoll.sql");
            sqlQuery = SetDataSource(sqlQuery, dataPath);

            if (!string.IsNullOrWhiteSpace(customerCodeFilter))
                sqlQuery = sqlQuery.Replace(" %CUSTOMERCODEFILTER%", " AND " + customerCodeFilter);
            else
                sqlQuery = sqlQuery.Replace(" %CUSTOMERCODEFILTER%", "");


            var reportDB = PervasiveSqlUtilities.FetchPervasiveData(sqlQuery, new OdbcParameter("@PPeriod", period));
            data = new List<LevyRollDataItem>();
            foreach (DataRow row in reportDB.Rows)
            {
                data.Add(new LevyRollDataItem(row, period));
            }

            foreach (DataRow row in allMasterAccounts.Rows)
            {
                var rowItem = new LevyRollDataItem(row, period);
                var currentCustomer = data.Where(a => a.CustomerCode == rowItem.CustomerCode).FirstOrDefault();
                if (currentCustomer == null)
                {
                    data.Add(rowItem);
                }
            }

            return data;
        }

        private byte[] RunReportToPdf(List<LevyRollDataItem> data, List<SundryDataItem> sundries, DateTime dDate, string building)
        {
            string rdlcPath = "Astrodon.Reports.LevyRoll.LevyRollReport.rdlc";
            byte[] report = null;

            Dictionary<string, IEnumerable> reportData = new Dictionary<string, IEnumerable>();
            Dictionary<string, string> reportParams = new Dictionary<string, string>();

            string period = dDate.ToString("MMM yyyy");

            reportParams.Add("BuildingName", building);
            reportParams.Add("Period", period);

            reportData.Add("LevyRollMain", data);
            reportData.Add("dsLevySundries", sundries);
            if (sundries.Count > 0)
                reportParams.Add("IncludeSundries", "true");
            else
                reportParams.Add("IncludeSundries", "false");

            using (RdlcHelper rdlcHelper = new RdlcHelper(rdlcPath,
                                                        reportData,
                                                        reportParams))
            {

                rdlcHelper.Report.EnableExternalImages = true;
                report = rdlcHelper.GetReportAsFile();
            }
            return report;
        }

        private string SetDataSource(string sqlQuery, string dataPath)
        {
            return PervasiveSqlUtilities.SetDataSource(sqlQuery, dataPath);
        }
    }
}
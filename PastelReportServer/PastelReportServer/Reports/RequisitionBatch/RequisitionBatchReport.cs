using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data;
using System.Data.Entity;
using System.Collections;
using System.Globalization;

namespace Astrodon.Reports.RequisitionBatch
{
    public class RequisitionBatchReport
    {
        private DataContext _context;

        public RequisitionBatchReport(DataContext context)
        {
            _context = context;
        }

        public byte[] RunReport(int requisitionBatchId, string sqlConnectionString)
        {
            try
            {
                var batch = _context.RequisitionBatchSet.Include(a => a.Building).Single(a => a.id == requisitionBatchId);
                var building = batch.Building;

                var reportDataSet = (from r in _context.tblRequisitions.Include(a => a.Supplier)
                                     where r.RequisitionBatchId == batch.id
                                     select new RequisitionBatchReportDataItem()
                                     {
                                         Created = r.trnDate,
                                         Bank = r.BankName,
                                         BranchCode = r.BranchCode,
                                         AccountNumber = r.AccountNumber,
                                         SupplierName = r.Supplier != null ? r.Supplier.CompanyName : r.contractor,
                                         LedgerAccount = r.ledger,
                                         Amount = r.amount,
                                         SupplierReference = r.payreference,
                                         InvoiceNumber = r.InvoiceNumber,
                                         UseNedbankCSV = r.UseNedbankCSV == null ? false : r.UseNedbankCSV.Value
                                     }).ToList().OrderBy(a => a.CSVDescription).ThenBy(a => a.Created).ToList();
                int x = 1;
                foreach (var r in reportDataSet)
                {
                    r.Number = x;
                    if (!String.IsNullOrWhiteSpace(building.AccNumber))
                    {
                        if (building.AccNumber.Length > 4)
                            r.TrustAccount = "(" + building.AccNumber.Substring(0, 4) + ")";
                        else
                            r.TrustAccount = "(" + building.AccNumber + ")";
                    }
                    else
                        r.TrustAccount = "";
                    r.Abbr = building.Code;
                    x++;
                }

                Dictionary<string, IEnumerable> reportData = new Dictionary<string, IEnumerable>();
                Dictionary<string, string> reportParams = new Dictionary<string, string>();

                reportParams.Add("BatchNumber", batch.Building.Code.ToString() + "-" + batch.BatchNumber.ToString().PadLeft(6, '0'));
                reportParams.Add("Created", batch.Created.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
                reportParams.Add("BuildingName", batch.Building.Building);
                reportParams.Add("Address1", batch.Building.addy1);
                reportParams.Add("Address2", batch.Building.addy2);
                reportParams.Add("Address3", batch.Building.addy3);
                reportParams.Add("Address4", batch.Building.addy4);
                reportParams.Add("Address5", batch.Building.addy5);

                reportParams.Add("Trust", batch.Building.bank);
                reportParams.Add("Bank", batch.Building.bankName);
                reportParams.Add("AccountName", batch.Building.accName);
                reportParams.Add("AccountNumber", batch.Building.bankAccNumber);
                reportParams.Add("BranchCode", batch.Building.branch);

                reportData.Add("dsReportData", reportDataSet);

                string rdlcPath = "Astrodon.Reports.RequisitionBatch.RequisitionReport.rdlc";
                byte[] report = null;

                using (RdlcHelper rdlcHelper = new RdlcHelper(rdlcPath, reportData, reportParams))
                {
                    rdlcHelper.Report.EnableExternalImages = true;
                    report = rdlcHelper.GetReportAsFile();
                }
                return report;
            }
            catch (Exception e)
            {
                using (var dc = new DataContext(sqlConnectionString))
                {
                    dc.SystemLogSet.Add(new Data.Log.SystemLog()
                    {
                        EventTime = DateTime.Now,
                        Message = "RequisitionBatchReport: " + e.Message + e.InnerException != null ? " Inner: " + e.InnerException.Message : string.Empty,
                        StackTrace = e.StackTrace + e.InnerException != null ? " Inner: " + e.InnerException.StackTrace : string.Empty

                    });
                    dc.SaveChanges();
                }

                throw e;
            }

        }
    }
}

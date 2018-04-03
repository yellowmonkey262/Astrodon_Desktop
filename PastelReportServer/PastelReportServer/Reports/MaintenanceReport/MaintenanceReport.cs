using Astrodon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.DataContracts;
using System.Data.Entity;
using Desktop.Lib.Pervasive;
using Astrodon.Reports.LevyRoll;
using System.Data;
using System.Diagnostics;
using System.Collections;
using Astrodon.Data.MaintenanceData;
using Astrodon.DataProcessor;

namespace Astrodon.Reports.MaintenanceReport
{
    public class MaintenanceReport
    {
        private DataContext _dataContext;
        public MaintenanceReport(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public byte[] RunReport(MaintenanceReportType reportType, DateTime fromDate,DateTime toDate,int buildingId, string buildingName, string dataPath)
        {
            DateTime startDate = fromDate;
            DateTime endDate = toDate;

            var pastelTransactions = new MaintenanceProcessor(_dataContext, buildingId).FetchAndLinkMaintenanceTransactions(startDate,endDate);

            var buildingConfig = _dataContext.BuildingMaintenanceConfigurationSet.Where(a => a.BuildingId == buildingId).ToList();

            var q = from r in _dataContext.tblRequisitions
                    join maint in _dataContext.MaintenanceSet on r.id equals maint.RequisitionId into maintSet
                    from m in maintSet.DefaultIfEmpty()
                    join d in _dataContext.MaintenanceDetailItemSet on m.id equals d.MaintenanceId into maintenanceDetail
                    from detail in maintenanceDetail.DefaultIfEmpty()
                    where r.trnDate >= startDate
                       && r.trnDate <= endDate
                       && r.building == buildingId
                    select new MaintenanceReportDataItem()
                    {
                        Ledger = r.ledger,
                        MaintenanceClassificationType = m != null ? m.BuildingMaintenanceConfiguration.MaintenanceClassificationType : MaintenanceClassificationType.MaintenancePlan,
                        Unit = detail == null ? string.Empty : detail.IsForBodyCorporate ? "Body Corporate" : detail.CustomerAccount,
                        MaintenanceType = m == null ? string.Empty : m.BuildingMaintenanceConfiguration.Name,
                        PastelAccountNumber = m == null ? string.Empty : m.BuildingMaintenanceConfiguration.PastelAccountNumber,
                        PastelAccountName = m == null ? string.Empty : m.BuildingMaintenanceConfiguration.PastelAccountName,
                        MaintenanceDate = r.trnDate,
                        Description = m == null ? r.reference : m.Description,
                        Supplier = r.Supplier == null ? string.Empty : r.Supplier.CompanyName,
                        CompanyReg = r.Supplier == null ? string.Empty : r.Supplier.CompanyRegistration,
                        VatNumber = r.Supplier == null ? string.Empty : r.Supplier.VATNumber,
                        ContactPerson = r.Supplier == null ? string.Empty : r.Supplier.ContactPerson,
                        EmailAddress = r.Supplier == null ? string.Empty : r.Supplier.EmailAddress,
                        ContactNumber = r.Supplier == null ? string.Empty : r.Supplier.ContactNumber,
                        Bank = r.BankName,
                        Branch = r.BranchName,
                        BranchCode = r.BranchCode,
                        AccountNumber = r.AccountNumber,
                        InvoiceNumber = m == null ? string.Empty : m.InvoiceNumber,
                        WarrantyDuration = m == null ? null : m.WarrantyDuration,
                        WarrantyType = m == null ? null : m.WarrantyDurationType,
                        WarrantyNotes = m == null ? string.Empty : m.WarrantyNotes,
                        WarrantyExpires = m == null ? null : m.WarrentyExpires,
                        SerialNumber = m == null ? string.Empty : m.WarrantySerialNumber,
                        Amount = detail == null ? r.amount : detail.Amount,
                        Paid = r.paid ? "Y" : "N",
                        LinkedPastelTransaction = r.PastelLedgerAutoNumber
                    };

            var reportData = q.ToList();

            reportData = (from r in reportData
                          join bc in buildingConfig on r.LedgerAccountNumber equals bc.PastelAccountNumber
                          select new MaintenanceReportDataItem
                          {
                              Ledger = r.Ledger,
                              MaintenanceClassificationType = bc.MaintenanceClassificationType,
                              Unit = r.Unit,
                              MaintenanceType =bc.Name,
                              PastelAccountNumber = bc.PastelAccountNumber,
                              PastelAccountName = bc.PastelAccountName,
                              MaintenanceDate = r.MaintenanceDate,
                              Description = r.Description,
                              Supplier = r.Supplier,
                              CompanyReg = r.CompanyReg,
                              VatNumber = r.VatNumber,
                              ContactPerson = r.ContactPerson,
                              EmailAddress = r.EmailAddress,
                              ContactNumber = r.ContactNumber,
                              Bank = r.Bank,
                              Branch = r.Branch,
                              BranchCode = r.BranchCode,
                              AccountNumber = r.AccountNumber,
                              InvoiceNumber = r.InvoiceNumber,
                              WarrantyDuration = r.WarrantyDuration,
                              WarrantyType = r.WarrantyType,
                              WarrantyNotes = r.WarrantyNotes,
                              WarrantyExpires = r.WarrantyExpires,
                              SerialNumber = r.SerialNumber,
                              Amount = r.Amount,
                              Paid = r.Paid,
                              LinkedPastelTransaction = r.LinkedPastelTransaction
                          }).ToList();
            
            //union the first query with the second one

            // now remove all pastel transctions from the pastel transaction list not already in the report
            var toRemove = from p in pastelTransactions
                           join r in reportData on p.AutoNumber equals r.LinkedPastelTransaction
                           select p;

            pastelTransactions = pastelTransactions.Except(toRemove.ToList()).ToList();
            //now add these to the report data

        
            var configPastel = from p in pastelTransactions
                               join c in buildingConfig on p.Account equals c.PastelAccountNumber
                               select new { pastel = p, BuildingMaintenanceConfiguration = c };

            reportData.AddRange(configPastel.Select(r => new MaintenanceReportDataItem()
            {
                MaintenanceClassificationType = r.BuildingMaintenanceConfiguration.MaintenanceClassificationType,
                Unit = "Unknown Unit",
                MaintenanceType = r.BuildingMaintenanceConfiguration.Name,
                PastelAccountNumber = r.BuildingMaintenanceConfiguration.PastelAccountNumber,
                PastelAccountName = r.BuildingMaintenanceConfiguration.PastelAccountName,
                MaintenanceDate = r.pastel.TransactionDate,
                Description = r.pastel.Description,
                Supplier = string.Empty,
                CompanyReg = string.Empty,
                VatNumber = string.Empty,
                ContactPerson = string.Empty,
                EmailAddress = string.Empty,
                ContactNumber = string.Empty,
                Bank = string.Empty,
                Branch = string.Empty,
                BranchCode = string.Empty,
                AccountNumber = string.Empty,
                InvoiceNumber = r.pastel.Reference,
                WarrantyDuration = null,
                WarrantyType = null,
                WarrantyNotes = string.Empty,
                WarrantyExpires = null,
                SerialNumber = string.Empty,
                Amount = Math.Abs( r.pastel.Amount),
                Paid = string.Empty,
                LinkedPastelTransaction = r.pastel.AutoNumber
            }));


            if (reportData.Count <= 0)
                return null;

            reportData = reportData.OrderBy(a => a.MaintenanceClassificationType).ThenBy(a => a.Unit).ThenBy(a => a.MaintenanceType).ThenBy(a => a.MaintenanceDate).ThenBy(a => a.Supplier).ToList();

            var accountNumbers = buildingConfig.Select(a => a.PastelAccountNumber).ToArray();// reportData.Select(a => a.PastelAccountNumber).Distinct().ToArray();
            var accountList = LoadAccountValues(startDate,endDate, dataPath, accountNumbers);

            //merge account data with account list
            string currentLedgerAccount = "";

            //to load the budgets - foreach month foreach account find the first record and then apply the budget amount.
            foreach (var account in accountList.Where(a => a.Budget != 0))
            {

                var transaction = reportData.Where(a => a.PastelAccountNumber == account.AccNumber && a.PeriodMonth == account.PeriodMonth).FirstOrDefault();
                if (transaction == null)
                {
                    var config = buildingConfig.Where(a => a.PastelAccountNumber == account.AccNumber).First();
                    transaction = new MaintenanceReportDataItem()
                    {
                        MaintenanceClassificationType = config.MaintenanceClassificationType,
                        Unit = "Unknown Unit",
                        MaintenanceType = config.Name,
                        PastelAccountNumber = config.PastelAccountNumber,
                        PastelAccountName = config.PastelAccountName,
                        MaintenanceDate = account.PeriodMonth,
                        Description = string.Empty,
                        Supplier = string.Empty,
                        CompanyReg = string.Empty,
                        VatNumber = string.Empty,
                        ContactPerson = string.Empty,
                        EmailAddress = string.Empty,
                        ContactNumber = string.Empty,
                        Bank = string.Empty,
                        Branch = string.Empty,
                        BranchCode = string.Empty,
                        AccountNumber = string.Empty,
                        InvoiceNumber = string.Empty,
                        WarrantyDuration = null,
                        WarrantyType = null,
                        WarrantyNotes = string.Empty,
                        WarrantyExpires = null,
                        SerialNumber = string.Empty,
                        Amount = 0,
                        Paid = string.Empty,
                        LinkedPastelTransaction = null
                    };
                    reportData.Add(transaction);
                }

                transaction.Budget = account.Budget;
                transaction.BudgetAvailable = account.BudgetAvailable;
            }

            decimal balance = 0;
            foreach (var dataItem in reportData.Where(a => a.LinkedPastelTransaction != null))
            {
                if (dataItem.PastelAccountNumber != currentLedgerAccount)
                {
                    balance = dataItem.Amount;
                    currentLedgerAccount = dataItem.PastelAccountNumber;
                }
                else
                    balance = balance + dataItem.Amount;
                dataItem.Balance = balance;
            }

            return RunReportToPdf(reportData, startDate,endDate, buildingName, reportType != MaintenanceReportType.SummaryReport);
        }

        private List<PervasiveAccount> LoadAccountValues(DateTime startDate,DateTime endDate, string dataPath, string[] accountNumbers)
        {
            string accQry = "";


            if (accountNumbers.Length == 1)
                accQry = " = '" + accountNumbers[0] + "'";
            else
            {
                accQry = " in (";
                foreach(var s in accountNumbers)
                {
                    if(accQry.EndsWith("("))
                      accQry = accQry + "'" + s + "'";
                    else
                        accQry = accQry + ",'" + s + "'";
                }

                accQry = accQry + ")";
            }

            string qry = "select * from [DataSet].LedgerMaster where AccNumber " + accQry;

            qry = SetDataSource(qry, dataPath);
            var accountData = PervasiveSqlUtilities.FetchPervasiveData( qry, null);
            List<PervasiveAccount> accountList = new List<PervasiveAccount>();
            foreach (DataRow row in accountData.Rows)
            {
                var dt = startDate;
                while (dt <= endDate)
                {
                    accountList.Add(new PervasiveAccount(row, GetBuildingPeriod(dt, dataPath),dt));
                    dt = dt.AddMonths(1);
                }
            }

            return accountList;
        }

        PeriodDataItem _periodItem = null;
        private int GetBuildingPeriod(DateTime startDate, string dataPath)
        {
            DateTime dDate = new DateTime(startDate.Year, startDate.Month, 1);
            if (_periodItem == null)
            {
                string sqlPeriodConfig = PervasiveSqlUtilities.ReadResourceScript("Astrodon.Reports.Scripts.PeriodParameters.sql");
                sqlPeriodConfig = SetDataSource(sqlPeriodConfig, dataPath);
                var periodData = PervasiveSqlUtilities.FetchPervasiveData(sqlPeriodConfig, null);
                foreach (DataRow row in periodData.Rows)
                {
                    _periodItem = new PeriodDataItem(row);
                    break;
                }
            }
            int period = 0;
            try
            {
                period = _periodItem.PeriodNumberLookup(dDate);
            }
            catch (Exception err)
            {
                throw err;
            }
            return period;
        }

        private string SetDataSource(string sqlQuery, string dataPath)
        {
            return PervasiveSqlUtilities.SetDataSource(sqlQuery, dataPath);
        }


        private byte[] RunReportToPdf(List<MaintenanceReportDataItem> data, DateTime startDate, DateTime endDate, string buildingName, bool detailedReport)
        {
            string rdlcPath = "Astrodon.Reports.MaintenanceReport.MaintenanceReport.rdlc";
            byte[] report = null;

            Dictionary<string, IEnumerable> reportData = new Dictionary<string, IEnumerable>();
            Dictionary<string, string> reportParams = new Dictionary<string, string>();

            string period = startDate.ToString("MMM yyyy") + " - " + endDate.ToString("MMM yyyy");

            reportParams.Add("Period", period);
            reportParams.Add("BuildingName", buildingName);
            reportParams.Add("DetailedReport", detailedReport ? "true" : "false");

            reportData.Add("MaintenanceReport", data);

            using (RdlcHelper rdlcHelper = new RdlcHelper(rdlcPath, reportData, reportParams))
            {
                rdlcHelper.Report.EnableExternalImages = true;
                report = rdlcHelper.GetReportAsFile();
            }
            return report;
        }
    }

    class UnlinkedRequisitions
    {
        public tblRequisition Requisition { get; set; }
        public BuildingMaintenanceConfiguration BuildingMaintenanceConfiguration { get; set; }
    }
}

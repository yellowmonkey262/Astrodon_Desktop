using Astrodon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.DataContracts.Maintenance;
using Desktop.Lib.Pervasive;
using System.Data;
using Astrodon.Data.MaintenanceData;
using System.Data.Entity;
using Astrodon.DataContracts;
using System.Globalization;

namespace Astrodon.DataProcessor
{
    public class MaintenanceProcessor
    {
        private DataContext _context;
        private int _buildingId;

        private tblBuilding _building;
        private List<BuildingMaintenanceConfiguration> _buildingConfig;

        public MaintenanceProcessor(DataContext context, int buildingId)
        {
            _context = context;
            _buildingId = buildingId;
            _building = context.tblBuildings.Where(a => a.id == buildingId).Single();
            _buildingConfig = context.BuildingMaintenanceConfigurationSet.Where(a => a.BuildingId == _buildingId).ToList();
        }

        public ICollection<PastelMaintenanceTransaction> MissingMaintenanceRecordsGet()
        {
            var result = new List<PastelMaintenanceTransaction>();
            if (_buildingConfig.Count <= 0) //nothing configured
                return result;

            //load pastel transaction list
            var pastelTransactions = FetchPastelMaintTransactions();

            if (pastelTransactions == null)
                pastelTransactions = new List<PastelMaintenanceTransaction>();

            if (pastelTransactions.Count <= 0)
                return result;

           
             //load requisitions
            var minDate = pastelTransactions.Min(a => a.TransactionDate).Date.AddDays(-7);
            var maxDate = pastelTransactions.Max(a => a.TransactionDate).Date.AddDays(7).AddSeconds(-1);

            var dbList = (from r in _context.tblRequisitions
                          where r.trnDate >= minDate && r.trnDate <= maxDate
                          && r.building == _buildingId
                          select r).ToList();

            //remove all non maintenance transactions
            List<tblRequisition> reqList = new List<tblRequisition>();
            foreach (var config in _buildingConfig)
            {
                reqList.AddRange(dbList.Where(a => a.LedgerAccountNumber == config.PastelAccountNumber));
            }

            //remove already matched transactions
            foreach (var req in reqList.Where(a => a.PastelLedgerAutoNumber != null))
            {
                var matched = pastelTransactions.Where(a => a.AutoNumber == req.PastelLedgerAutoNumber && a.DataPath == req.PastelDataPath).SingleOrDefault();
                if (matched != null)
                    pastelTransactions.Remove(matched);
            }

            //remove matched requisitions
            pastelTransactions = CalculateMatches(pastelTransactions, reqList);
           


            return pastelTransactions.OrderBy(a => a.TransactionDate).ToList();
        }

        private List<PastelMaintenanceTransaction> FetchPastelMaintTransactions()
        {
            List<PastelMaintenanceTransaction> result = new List<PastelMaintenanceTransaction>();

            string accountList = string.Empty;

            foreach (var config in _buildingConfig)
            {
                if (!string.IsNullOrWhiteSpace(accountList))
                    accountList = accountList + " or  t.AccNumber = '" + config.PastelAccountNumber + "'";
                else
                    accountList = " t.AccNumber = '" + config.PastelAccountNumber + "'";
            }

            string dataPath = _building.DataPath;

            string sqlMaintenanceRecords = PervasiveSqlUtilities.ReadResourceScript("Astrodon.DataProcessor.Scripts.MaintenanceRecordList.sql");
            sqlMaintenanceRecords = PervasiveSqlUtilities.SetDataSource(sqlMaintenanceRecords, dataPath);
            sqlMaintenanceRecords = sqlMaintenanceRecords.Replace("[AccountList]", accountList);

            foreach (DataRow row in PervasiveSqlUtilities.FetchPervasiveData(sqlMaintenanceRecords).Rows)
                result.Add(new PastelMaintenanceTransaction(row, dataPath));

            return result;
        }

        public List<PastelMaintenanceTransaction> FetchAndLinkMaintenanceTransactions(DateTime fromDate, DateTime toDate)
        {
            List<PastelMaintenanceTransaction> pastelTransactions = new List<PastelMaintenanceTransaction>();

            string accountList = string.Empty;

            foreach (var config in _buildingConfig)
            {
                if (!string.IsNullOrWhiteSpace(accountList))
                    accountList = accountList + " or  t.AccNumber = '" + config.PastelAccountNumber + "'";
                else
                    accountList = " t.AccNumber = '" + config.PastelAccountNumber + "'";
            }

            string dataPath = _building.DataPath;

            string sqlMaintenanceRecords = PervasiveSqlUtilities.ReadResourceScript("Astrodon.DataProcessor.Scripts.MaintenanceRecordListBetweenDates.sql");
            sqlMaintenanceRecords = PervasiveSqlUtilities.SetDataSource(sqlMaintenanceRecords, dataPath);
            sqlMaintenanceRecords = sqlMaintenanceRecords.Replace("[AccountList]", accountList);
            sqlMaintenanceRecords = sqlMaintenanceRecords.Replace("[FromDate]", "'" + fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'");
            sqlMaintenanceRecords = sqlMaintenanceRecords.Replace("[ToDate]", "'" + toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'");

            foreach (DataRow row in PervasiveSqlUtilities.FetchPervasiveData(sqlMaintenanceRecords).Rows)
                pastelTransactions.Add(new PastelMaintenanceTransaction(row, dataPath));

            var minDate = fromDate.Date.AddDays(-7);
            var maxDate = toDate.Date.AddDays(8);

            if (pastelTransactions.Count > 0)
            {
                minDate = pastelTransactions.Min(a => a.TransactionDate).Date.AddDays(-7);
                maxDate = pastelTransactions.Max(a => a.TransactionDate).Date.AddDays(8).AddSeconds(-1);

                var reqList = (from r in _context.tblRequisitions
                               where r.trnDate >= minDate && r.trnDate <= maxDate
                               && r.building == _buildingId
                               select r).ToList();


                //match requisition transactions to pastel transactions and update changes
                return CalculateMatches(pastelTransactions, reqList);
            }
            return pastelTransactions;
        }

        private List<PastelMaintenanceTransaction> CalculateMatches(List<PastelMaintenanceTransaction> pastelTransactions, List<tblRequisition> reqList)
        {
            reqList = reqList.Except(reqList.Where(a => a.PastelLedgerAutoNumber != null)).ToList();


            //clear null references
            foreach (var r in reqList.Where(a => a.reference == null))
            {
                r.reference = string.Empty;
            }

            foreach (var req in reqList.Where(a => a.PastelLedgerAutoNumber == null))
            {
                DateTime minDate = req.trnDate.AddDays(-7);
                DateTime maxDate = req.trnDate.AddDays(7);

                var matched = pastelTransactions.Where(a =>  //match the requisition to the account, payments are matched to the LinkAccount
                                                         Math.Abs(a.Amount) == Math.Abs(req.amount)
                                                         && a.TransactionDate >= minDate
                                                         && a.TransactionDate <= maxDate)
                                                         .OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate))).ToList();
                //clear null references
                foreach (var r in matched.Where(a => a.Reference == null))
                {
                    r.Reference = string.Empty;
                }

                var potential = matched.Where(a => a.Account == req.LedgerAccountNumber
                                                && a.Reference.ToLower() == req.reference.ToLower()
                                                && a.Reference != string.Empty)
                                     .OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //amount and account and reference


                if (potential == null)
                {
                    potential = matched.Where(a => a.Account == req.LedgerAccountNumber
                                                && a.Reference.ToLower().Contains(req.reference.ToLower()) || req.reference.ToLower().Contains(a.Reference.ToLower())
                                                 && a.Reference != string.Empty)
                                     .OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //just amount and account and reference
                }

                if (potential == null)
                {
                    potential = matched.Where(a => a.Account == req.LedgerAccountNumber)
                                     .OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //just amount and account
                }

                if (potential == null && req.trnDate < DateTime.Today.AddDays(-2)) //more than 48 hours
                {
                    potential = matched.OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //just amount 
                }

                if (potential != null)
                {
                    req.PastelLedgerAutoNumber = potential.AutoNumber;
                    req.PastelDataPath = potential.DataPath;
                    pastelTransactions.Remove(potential);
                }
            }
            _context.SaveChanges();

            return pastelTransactions;
        }
    }
}

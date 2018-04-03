using Astrodon.Data;
using Astrodon.DataContracts;
using Desktop.Lib.Pervasive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.DataProcessor
{
    public class RequisitionProcessor
    {
        private DataContext _context;
        private int _buildingId;
        private int _DayTolerance = 60;

        private tblBuilding _building;

        public RequisitionProcessor(DataContext context, int buildingId)
        {
            _context = context;
            _buildingId = buildingId;
            _building = context.tblBuildings.Where(a => a.id == buildingId).Single();
        }

        private List<PaymentTransaction> FetchPaymentTransactions()
        {
            List<PaymentTransaction> result = new List<PaymentTransaction>();
            string dataPath = _building.DataPath;

            string sqlPaymentRecords = string.Empty;


            sqlPaymentRecords = PervasiveSqlUtilities.ReadResourceScript("Astrodon.DataProcessor.Scripts.PaymentTransactionList.sql");

            sqlPaymentRecords = PervasiveSqlUtilities.SetDataSource(sqlPaymentRecords, dataPath);

            foreach (DataRow row in PervasiveSqlUtilities.FetchPervasiveData(sqlPaymentRecords).Rows)
                result.Add(new PaymentTransaction(row, dataPath));

            return result;
        }

        public int LinkPayments()
        {


            //step 1 find all payments in pastel
            var pastelTransactions = FetchPaymentTransactions();

            if (pastelTransactions == null)
                pastelTransactions = new List<PaymentTransaction>();

            if (pastelTransactions.Count <= 0)
                return 0;


            //load requisitions
            var minDate = pastelTransactions.Min(a => a.TransactionDate).Date.AddDays(_DayTolerance*-1);
            var maxDate = pastelTransactions.Max(a => a.TransactionDate).Date.AddDays(_DayTolerance).AddSeconds(-1);
            minDate = minDate.AddDays(-7);
            maxDate = maxDate.AddDays(7);

            var reqList = (from r in _context.tblRequisitions
                           where r.trnDate >= minDate 
                           && r.trnDate <= maxDate
                           && r.building == _buildingId
                           && r.processed == true
                           select r).ToList();

            //filter all items already matched

            //remove already matched transactions
            foreach (var req in reqList.Where(a => a.PaymentLedgerAutoNumber != null))
            {
                var matched = pastelTransactions.Where(a => a.AutoNumber == req.PaymentLedgerAutoNumber && a.DataPath == req.PaymentDataPath).SingleOrDefault();
                if (matched != null)
                    pastelTransactions.Remove(matched);
            }

            return CalculateMatches(pastelTransactions, reqList);

        }

        private int CalculateMatches(List<PaymentTransaction> pastelTransactions, List<tblRequisition> reqList)
        {
            int result = 0;
            reqList = reqList.Except(reqList.Where(a => a.PaymentLedgerAutoNumber != null)).ToList(); //remove already linked items

            //clear null references
            foreach(var r in reqList.Where(a => a.reference == null))
            {
                r.reference = string.Empty;
            }

            foreach (var req in reqList.Where(a => a.PaymentLedgerAutoNumber == null))
            {
                DateTime minDate = req.trnDate.AddDays(_DayTolerance*-1);
                DateTime maxDate = req.trnDate.AddDays(_DayTolerance);

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

                var potential = matched.Where(a => a.LedgerAccount == req.LedgerAccountNumber
                                                && a.Reference.ToLower() == req.reference.ToLower()
                                                && a.Reference != string.Empty)
                                     .OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //amount and account and reference

                
                if(potential == null)
                {
                    potential = matched.Where(a => a.LedgerAccount == req.LedgerAccountNumber
                                                && a.Reference.ToLower().Contains(req.reference.ToLower()) || req.reference.ToLower().Contains(a.Reference.ToLower())
                                                 && a.Reference != string.Empty)
                                     .OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //just amount and account and reference
                }

                if (potential == null)
                {
                    potential = matched.Where(a => a.LedgerAccount == req.LedgerAccountNumber)
                                     .OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //just amount and account
                }

                if (potential == null && req.trnDate < DateTime.Today.AddDays(-3)) //more than 48 hours
                {
                    potential = matched.OrderByDescending(a => Math.Abs(DateTime.Compare(a.TransactionDate, req.trnDate)))
                                     .FirstOrDefault(); //just amount 
                }

                if (potential != null)
                {
                    req.PaymentLedgerAutoNumber = potential.AutoNumber;
                    req.PaymentDataPath = potential.DataPath;
                    req.paid = true;
                    result++;
                    pastelTransactions.Remove(potential);
                }
            }
            _context.SaveChanges();

            

            return result;
        }
    }
}

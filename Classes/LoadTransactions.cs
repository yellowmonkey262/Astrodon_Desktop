using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Astro.Library;
using Astrodon.ReportService;

namespace Astrodon.Classes
{
    public class LoadTrans
    {

        public void ShowDebug(string message)
        {
            
        }

        public List<Transaction> LoadTransactions(Building building, Customer customer, DateTime transDate, out double totalDue, out String trnMsg)
        {
            trnMsg = "";
            PeriodItem startPeriod;
            PeriodItem endPeriod;
            try
            {

                using (var reportService = ReportServiceClient.CreateInstance())
                {

                    var result = reportService.CustomerStatementParameterLookup(SqlDataHandler.GetConnectionString(), building.ID, customer.accNumber, transDate, 3);
                    startPeriod = result.OrderBy(a => a.Start).First();
                    endPeriod = result.OrderBy(a => a.Start).Last();
                }

                totalDue = 0;

                List<Transaction> trans = new List<Transaction>();
                List<Transaction> optrans = new List<Transaction>();
                totalDue = 0;

                DateTime trnDate = startPeriod.Start.Value;

                DateTime dt1 = new DateTime(transDate.Year, transDate.Month, 1);
                var minDate = dt1.AddMonths(-3);

                var opBal = startPeriod.OpeningBalance;
                Transaction optran = new Transaction
                {
                    AccAmt = startPeriod.OpeningBalance,
                    Description = "Balance Brought Forward",
                    Reference = "",
                    TrnAmt = startPeriod.OpeningBalance,
                    TrnDate = trnDate,
                    IsOpeningBalance = true
                };
                var transactions = Controller.pastel.GetTransactions(building.DataPath, startPeriod.PeriodNumber, endPeriod.PeriodNumber, customer.accNumber).OrderBy(c => c.Date).ToList();
                double subtractAmount = 0;
                foreach (Trns trn in transactions)
                {
                    Transaction tran = new Transaction
                    {
                        Description = trn.Description,
                        Reference = trn.Reference,
                        TrnAmt = double.Parse(trn.Amount),
                        TrnDate = DateTime.Parse(trn.Date)
                    };
                    subtractAmount += double.Parse(trn.Amount);
                    trans.Add(tran);
                }
                optran.TrnAmt = opBal;
                optran.AccAmt = optran.TrnAmt;
                optrans.Add(optran);
                foreach (Transaction tran in trans)
                {
                    opBal += tran.TrnAmt;
                    tran.AccAmt = opBal;
                    optrans.Add(tran);
                }
                totalDue = opBal;
                return optrans;
            }
            catch (Exception e)
            {
                ShowDebug(e.Message);
                totalDue = 0;
                throw e;
            }
        }
    }
}

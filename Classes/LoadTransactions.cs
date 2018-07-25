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

            int bPeriod;
            int tPeriod = Methods.getPeriod(transDate, building.Period, out bPeriod);
            ShowDebug("bPeriod:" + bPeriod.ToString());

            //  int startperiod = 0;
            //  int endperiod = 0;

            //DateTime trnSEndDate = transDate;
            //DateTime trnEndDate = (trnSEndDate.Day != 1 ? new DateTime(trnSEndDate.Year, trnSEndDate.Month, 1) : trnSEndDate);

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

                /*

           

                if (bPeriod - 2 == 0)
                {
                  
                    if (thisYear == dataYear)
                    {
                        ShowDebug("Stage 1" + Environment.NewLine + "bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());

                        isThisYear = true;
                        startperiod = 12;
                        endperiod = 102;
                    }
                    else
                    {
                        ShowDebug("Stage 2" + Environment.NewLine +"bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());

                        startperiod = 12;
                        endperiod = 102;
                    }
                }
                else if (bPeriod - 2 < 0)
                {
                    ShowDebug("Stage 3" + Environment.NewLine +"bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());

                    if (thisYear == dataYear)
                    {
                        isThisYear = true;
                        if ((tPeriod == 12 || tPeriod == 1) && bPeriod == 1)
                        {
                            ShowDebug("Stage 4" + Environment.NewLine + "bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());


                            startperiod = 111;
                            endperiod = 113;
                        }
                        else
                        {
                            if (bPeriod == 1 && building.Period != 10)
                            //!building.DataPath.StartsWith("MABAL"))
                            {
                                ShowDebug("Stage 5" + Environment.NewLine + "bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());
                                if (Controller.user.id == 1) { MessageBox.Show("std"); }
                                startperiod = 111;
                                endperiod = 113;
                            }
                            else
                            {
                                ShowDebug("Stage 6" + Environment.NewLine + "bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());
                                if (Controller.user.id == 1) { MessageBox.Show("dec"); }
                                startperiod = (bPeriod - 2 == -1 ? 11 : 111);
                                endperiod = (bPeriod - 2 == -1 ? 101 : 113);
                            }
                        }
                    }
                    else
                    {
                        if (dataYear < thisYear) { isThisYear = true; }
                        if (bPeriod == 1 && building.Period == 10)
                        {
                            ShowDebug("Stage 7" + Environment.NewLine + "bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());
                            startperiod = 111;
                            endperiod = 113;
                        }
                        else
                        {
                            ShowDebug("Stage 8" + Environment.NewLine + "bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());

                            startperiod = 11;
                            endperiod = 101;
                        }
                    }
                }
                else if (bPeriod - 2 > 0)
                {
                    ShowDebug("Stage 9" + Environment.NewLine + "bPeriod: " + bPeriod.ToString() + " thisYear " + thisYear.ToString() + " dataYear " + dataYear.ToString());

                    isThisYear = true;
                    startperiod = 100 + bPeriod - 2;
                    endperiod = 100 + bPeriod;
                }

                if (startperiod > 200)
                    startperiod = startperiod - 100;
                if (endperiod > 200)
                    endperiod = endperiod - 100;

    */


                totalDue = 0;

                //       ShowDebug(startperiod.ToString() + " " + endperiod.ToString());

                //   DateTime trnSEndDate = transDate;
                //   DateTime trnEndDate = (trnSEndDate.Day != 1 ? new DateTime(trnSEndDate.Year, trnSEndDate.Month, 1) : trnSEndDate);
                List<Transaction> trans = new List<Transaction>();
                List<Transaction> optrans = new List<Transaction>();
                totalDue = 0;

                /*
                if (customer != null)
                {
                    List<Trns> transactions = new List<Trns>();
                    double os = 0;
                    double opBal = 0;

                    if (customer != null)
                    {
                        int opBalPeriod = bPeriod - 3;
                        trnMsg += (opBalPeriod.ToString() + " - " + isThisYear.ToString());
                        if (opBalPeriod < 0 && !isThisYear)
                        {
                            opBalPeriod *= -1;
                            if (!newYear && building.Period != 10)
                            //!building.DataPath.StartsWith("MABAL"))
                            {
                                for (int li = 0; li < customer.lastBal.Length; li++) { opBal += customer.lastBal[li]; }
                                for (int i = 0; i < 12 - opBalPeriod; i++) { opBal += customer.balance[i]; }
                            }
                            else
                            {
                                for (int li = 0; li < (customer.lastBal.Length - (opBalPeriod + 1)); li++) { opBal += customer.lastBal[li]; }
                            }
                        }
                        else if (opBalPeriod == -1 && isThisYear)
                        {
                            opBalPeriod *= -1;
                            for (int li = 0; li < (customer.lastBal.Length - (opBalPeriod + 1)); li++) { opBal += customer.lastBal[li]; }
                        }
                        else if (opBalPeriod < -1 && isThisYear)
                        {
                            opBalPeriod *= -1;
                            if (bPeriod - 2 == -1)
                            {
                                if ((tPeriod == 12 || tPeriod == 1) && bPeriod == 1)
                                {
                                    for (int li = 0; li < customer.lastBal.Length; li++) { opBal += customer.lastBal[li]; }
                                    for (int i = 0; i < 12 - opBalPeriod; i++) { opBal += customer.balance[i]; }
                                }
                                else if (bPeriod == 1 && building.Period != 10)
                                //!building.DataPath.StartsWith("MABAL"))
                                {
                                    for (int li = 0; li < customer.lastBal.Length; li++) { opBal += customer.lastBal[li]; }
                                    for (int i = 0; i < 12 - opBalPeriod; i++) { opBal += customer.balance[i]; }
                                }
                                else if (bPeriod == 1 && building.Period == 10)
                                {
                                    if (thisYear > dataYear)
                                    {
                                        //for (int li = 0; li < customer.lastBal.Length; li++) { opBal += customer.lastBal[li]; }
                                        for (int i = 0; i < 12 - opBalPeriod; i++) { opBal += customer.balance[i]; }
                                    }
                                    else
                                    {
                                        for (int li = 0; li < (customer.lastBal.Length - (opBalPeriod + 1)); li++) { opBal += customer.lastBal[li]; }
                                    }
                                }
                                else
                                {
                                    for (int li = 0; li < (customer.lastBal.Length - (opBalPeriod + 1)); li++) { opBal += customer.lastBal[li]; }
                                }
                            }
                            else
                            {
                                for (int li = 0; li < customer.lastBal.Length; li++) { opBal += customer.lastBal[li]; }
                                for (int i = 0; i < 12 - opBalPeriod; i++) { opBal += customer.balance[i]; }
                            }
                        }
                        else
                        {
                            for (int li = 0; li < customer.lastBal.Length; li++) { opBal += customer.lastBal[li]; }
                            for (int i = 0; i < bPeriod - 3; i++) { opBal += customer.balance[i]; }
                        }
                        for (int li = 0; li < customer.lastBal.Length; li++) { os += customer.lastBal[li]; }
                        for (int i = 0; i <= (bPeriod - 1 == 0 ? 1 : bPeriod - 1); i++) { os += customer.balance[i]; }
                    }
                    else
                    {
                        os = 0;
                        opBal = 0;
                    }*/

                DateTime trnDate = startPeriod.Start.Value;

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

using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Astro.Library;

namespace Astrodon.Classes
{
    public class LoadTrans
    {
        public List<Transaction> LoadTransactions(Building building, Customer customer, DateTime transDate, out double totalDue, out String trnMsg)
        {
            trnMsg = "";
            try
            {
                int bPeriod;
                int tPeriod = Methods.getPeriod(transDate, building.Period, out bPeriod);
                int startperiod = 0;
                int thisYear = DateTime.Now.Year - 2000;
                int endperiod = 0;
                bool isThisYear = false;
                int dataYear = int.Parse(building.DataPath.Substring(building.DataPath.Length - 2, 2));
                bool newYear = thisYear < dataYear;
                if (bPeriod - 2 == 0)
                {
                    if (thisYear == dataYear)
                    {
                        isThisYear = true;
                        startperiod = 12;
                        endperiod = 102;
                    }
                    else
                    {
                        startperiod = 12;
                        endperiod = 102;
                    }
                }
                else if (bPeriod - 2 < 0)
                {
                    if (thisYear == dataYear)
                    {
                        isThisYear = true;
                        if ((tPeriod == 12 || tPeriod == 1) && bPeriod == 1)
                        {
                            startperiod = 111;
                            endperiod = 113;
                        }
                        else
                        {
                            if (bPeriod == 1 && building.Period != 10)
                            //!building.DataPath.StartsWith("MABAL"))
                            {
                                if (Controller.user.id == 1) { MessageBox.Show("std"); }
                                startperiod = 111;
                                endperiod = 113;
                            }
                            else
                            {
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
                            startperiod = 111;
                            endperiod = 113;
                        }
                        else
                        {
                            startperiod = 11;
                            endperiod = 101;
                        }
                    }
                }
                else if (bPeriod - 2 > 0)
                {
                    isThisYear = true;
                    startperiod = 100 + bPeriod - 2;
                    endperiod = 100 + bPeriod;
                }
                totalDue = 0;
                //MessageBox.Show(startperiod.ToString() + " " + endperiod.ToString());
                DateTime trnSEndDate = transDate;
                DateTime trnEndDate = (trnSEndDate.Day != 1 ? new DateTime(trnSEndDate.Year, trnSEndDate.Month, 1) : trnSEndDate);
                List<Transaction> trans = new List<Transaction>();
                List<Transaction> optrans = new List<Transaction>();
                totalDue = 0;

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
                    }

                    DateTime trnDate = trnEndDate.AddMonths(-2);

                    Transaction optran = new Transaction
                    {
                        AccAmt = os,
                        Description = "Balance Brought Forward",
                        Reference = "",
                        TrnAmt = os,
                        TrnDate = trnDate
                    };
                    transactions = Controller.pastel.GetTransactions(building.DataPath, startperiod, endperiod, customer.accNumber).OrderBy(c => c.Date).ToList();

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
                }
                return optrans;
            }
            catch
            {
                totalDue = 0;
                return null;
            }
        }
    }
}

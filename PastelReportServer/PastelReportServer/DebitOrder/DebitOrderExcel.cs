using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data;
using Astrodon.Data.DebitOrder;
using Astrodon.Reports.LevyRoll;
using OfficeOpenXml;
using System.Globalization;
using System.IO;
using Desktop.Lib.Pervasive;
using System.Data;

namespace Astrodon.DebitOrder
{
    public class DebitOrderExcel
    {
        private DataContext _DataContext;

        public DebitOrderExcel(DataContext dataContext)
        {
            this._DataContext = dataContext;
        }

        public List<DebitOrderItem> RunDebitOrderForBuilding(int buildingId, DateTime processMonth, bool showFeeBreakdown)
        {
            int period;
            processMonth = new DateTime(processMonth.Year, processMonth.Month, 1);
            var query = _DataContext.CustomerDebitOrderSet
                                    .Where(a => a.BuildingId == buildingId)
                                    .Select(b => new DebitOrderItem()
                                    {
                                        BuildingId = b.BuildingId,
                                        CustomerCode = b.CustomerCode,
                                        BranchCode = b.BranceCode,
                                        AccountTypeId = b.AccountType,
                                        AccountNumber = b.AccountNumber,
                                        DebitOrderCollectionDay = b.DebitOrderCollectionDay,
                                        DebitOrderFeeDisabled = b.IsDebitOrderFeeDisabled, //disabled on unit level
                                        DebitOrderCancelDate = b.DebitOrderCancelDate,
                                        DebitOrderCancelled = b.DebitOrderCancelled,
                                        MaxDebitOrderAmount = b.MaxDebitAmount
                                    });

            var debitOrderItems = query.ToList().Where(a => a.DebitOrderActive).ToList();


         //   DateTime collectionDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
            DateTime collectionDay = new DateTime(processMonth.Year, processMonth.Month, 1);
            var building = _DataContext.tblBuildings.Single(a => a.id == buildingId);
            var buildingSettings = _DataContext.tblBuildingSettings.SingleOrDefault(a => a.buildingID == buildingId);
            var levyRollData = new List<LevyRollDataItem>();

            if (debitOrderItems.Count > 0)
            {
                var customers = debitOrderItems.Select(a => a.CustomerCode).Distinct().ToList();
                levyRollData = new LevyRollReport().LoadReportData(processMonth, building.DataPath, customers, out period);
            }

            decimal debitOrderFee = 0;
            if (buildingSettings != null)
            {
                debitOrderFee = buildingSettings.DebitOrderFee;
                if (building.IsDebitOrderFeeDisabled)
                    debitOrderFee = 0; //disabled on building level
            }
            foreach (var item in debitOrderItems)
            {
                item.DebitOrderFee = debitOrderFee;
                item.IsDebitOrderFeeDisabledOnBuilding = building.IsDebitOrderFeeDisabled;
                var levyRollItem = levyRollData.SingleOrDefault(a => a.CustomerCode.Trim().ToLower() == item.CustomerCode.Trim().ToLower());
                if (levyRollItem != null)
                {
                    item.LevyRollDue = levyRollItem.Due;
                    item.Payments = levyRollItem.Payments;
                    item.AmountDue = item.LevyRollDue + item.Payments;
                    item.CustomerName = levyRollItem.CustomerDesc;
                }
                if (item.DebitOrderCollectionDay == DebitOrderDayType.One)
                    item.CollectionDay = collectionDay;
                else
                    item.CollectionDay = new DateTime(collectionDay.Year, collectionDay.Month, 15);
            } 

            return debitOrderItems.Where(a => a.AmountDue > 0).ToList();
        }



        public List<PeriodItem> CustomerStatementParameterLookup(int buildingId, string customerCode, DateTime processMonth, int numberOfMonths)
        {

            var building = _DataContext.tblBuildings.Single(a => a.id == buildingId);

            var dDate = new DateTime(processMonth.Year, processMonth.Month, 1);
            string sqlPeriodConfig = PervasiveSqlUtilities.ReadResourceScript("Astrodon.Reports.Scripts.PeriodParameters.sql");
            sqlPeriodConfig = SetDataSource(sqlPeriodConfig, building.DataPath);
            var periodData = PervasiveSqlUtilities.FetchPervasiveData(sqlPeriodConfig, null);

            string sqlCustomerBalances = PervasiveSqlUtilities.ReadResourceScript("Astrodon.Reports.Scripts.CustomerBalance.sql");
            sqlCustomerBalances = SetDataSource(sqlCustomerBalances, building.DataPath);

            sqlCustomerBalances = sqlCustomerBalances.Replace("@CUSTOMERCODE", customerCode.Trim());
            var customerBalanceData = PervasiveSqlUtilities.FetchPervasiveData(sqlCustomerBalances, null);

            CustomerBalance custBalance = new CustomerBalance(customerBalanceData.Rows[0]);


            PeriodDataItem periodItem = null;
            foreach (DataRow row in periodData.Rows)
            {
                periodItem = new PeriodDataItem(row);
                break;
            }

            List<PeriodItem> result = new List<PeriodItem>();
            var lastPeriod = periodItem.PeriodLookup(dDate);
            var firstPeriod = periodItem.PeriodLookup(lastPeriod.Start.Value.AddMonths((numberOfMonths * -1)+1));

            lastPeriod.OpeningBalance = Convert.ToDouble(custBalance.CalcOpening(lastPeriod.PeriodNumber));
            lastPeriod.ClosingBalance = Convert.ToDouble(custBalance.CalcOpening(lastPeriod.PeriodNumber));

            firstPeriod.OpeningBalance = Convert.ToDouble(custBalance.CalcOpening(firstPeriod.PeriodNumber));
            firstPeriod.ClosingBalance = Convert.ToDouble(custBalance.CalcOpening(firstPeriod.PeriodNumber));

            result.Add(firstPeriod);
            result.Add(lastPeriod);

            return result;

        }

        private string SetDataSource(string sqlQuery, string dataPath)
        {
            return PervasiveSqlUtilities.SetDataSource(sqlQuery, dataPath);
        }
    }
}

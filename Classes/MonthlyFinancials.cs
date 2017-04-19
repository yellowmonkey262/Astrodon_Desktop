using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Classes
{
    public class MonthlyFinancial
    {
        public MonthlyFinancial(int buildingID, int period, int year)
        {
            DateTime startDate = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime endDate = new DateTime(year, 12, 31, 23, 59, 59);
            SqlDataHandler dh = new SqlDataHandler();
            String query = "SELECT id, completeDate, buildingID, finPeriod, levies, leviesReason, sewage, sewageNotes, electricity, electricityNotes, water, waterNotes, specialLevies, ";
            query += " specialLevyNotes, otherIncomeDescription, otherIncome, otherIncomeNotes, memberInterest, memberInterestNotes, bankInterest, bankInterestNotes, accountingFees, ";
            query += " accountingFeesNotes, bankCharges, bankChargesNotes, sewageExpense, sewageExpenseNotes, deliveries, deliveriesNotes, electricityExpense, ";
            query += " electricityExpenseNotes, gardens, gardensNotes, insurance, insuranceNotes, interestPaid, interestPaidNotes, managementFees, managementFeesNotes, ";
            query += " meterReading, meterReadingNotes, printing, printingNotes, post, postNotes, repairs, repairsNotes, refuse, refuseNotes, salaries, salariesNotes, security, ";
            query += " securityNotes, telephone, telephoneNotes, waterExpense, waterExpenseNotes, municipal, municipalReason, trust, trustNotes, own, ownNotes, investment, ";
            query += " investmentNotes, sundy, sundryNotes, assets, assetsNotes, debtors, debtorsNotes, municipalAccounts, municipalAccountsNotes, owners, ownersNotes, suppliers, ";
            query += " suppliersNotes, liabilities, liabilitiesNotes, electricityRecon, waterRecon";
            query += " FROM tblMonthFin WHERE buildingID = " + buildingID.ToString() + " AND finPeriod = " + period.ToString() + " AND completeDate >= '" + startDate.ToString() + "' AND completeDate <= '" + endDate.ToString() + "'";
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                id = int.Parse(dr["id"].ToString());
                bool result;
                completeDate = DateTime.Parse(dr["completeDate"].ToString());
                buildingID = int.Parse(dr["buildingID"].ToString());
                finPeriod = int.Parse(dr["finPeriod"].ToString());
                levies = (bool.TryParse(dr["levies"].ToString(), out result) ? result : false);
                leviesReason = dr["leviesReason"].ToString();
                sewage = (bool.TryParse(dr["sewage"].ToString(), out result) ? result : false);
                sewageNotes = dr["sewageNotes"].ToString();
                electricity = (bool.TryParse(dr["electricity"].ToString(), out result) ? result : false);
                electricityNotes = dr["electricityNotes"].ToString();
                water = (bool.TryParse(dr["water"].ToString(), out result) ? result : false);
                waterNotes = dr["waterNotes"].ToString();
                specialLevies = (bool.TryParse(dr["specialLevies"].ToString(), out result) ? result : false);
                specialLevyNotes = dr["specialLevyNotes"].ToString();
                otherIncomeDescription = dr["otherIncomeDescription"].ToString();
                otherIncome = (bool.TryParse(dr["otherIncome"].ToString(), out result) ? result : false);
                otherIncomeNotes = dr["otherIncomeNotes"].ToString();
                memberInterest = (bool.TryParse(dr["memberInterest"].ToString(), out result) ? result : false);
                memberInterestNotes = dr["memberInterestNotes"].ToString();
                bankInterest = (bool.TryParse(dr["bankInterest"].ToString(), out result) ? result : false);
                bankInterestNotes = dr["bankInterestNotes"].ToString();
                accountingFees = (bool.TryParse(dr["accountingFees"].ToString(), out result) ? result : false);
                accountingFeesNotes = dr["accountingFeesNotes"].ToString();
                bankCharges = (bool.TryParse(dr["bankCharges"].ToString(), out result) ? result : false);
                bankChargesNotes = dr["bankChargesNotes"].ToString();
                sewageExpense = (bool.TryParse(dr["sewageExpense"].ToString(), out result) ? result : false);
                sewageExpenseNotes = dr["sewageExpenseNotes"].ToString();
                deliveries = (bool.TryParse(dr["deliveries"].ToString(), out result) ? result : false);
                deliveriesNotes = dr["deliveriesNotes"].ToString();
                electricityExpense = (bool.TryParse(dr["electricityExpense"].ToString(), out result) ? result : false);
                electricityExpenseNotes = dr["electricityExpenseNotes"].ToString();
                gardens = (bool.TryParse(dr["gardens"].ToString(), out result) ? result : false);
                gardensNotes = dr["gardensNotes"].ToString();
                insurance = (bool.TryParse(dr["insurance"].ToString(), out result) ? result : false);
                insuranceNotes = dr["insuranceNotes"].ToString();
                interestPaid = (bool.TryParse(dr["interestPaid"].ToString(), out result) ? result : false);
                interestPaidNotes = dr["interestPaidNotes"].ToString();
                managementFees = (bool.TryParse(dr["managementFees"].ToString(), out result) ? result : false);
                managementFeesNotes = dr["managementFeesNotes"].ToString();
                meterReading = (bool.TryParse(dr["meterReading"].ToString(), out result) ? result : false);
                meterReadingNotes = dr["meterReadingNotes"].ToString();
                printing = (bool.TryParse(dr["printing"].ToString(), out result) ? result : false);
                printingNotes = dr["printingNotes"].ToString();
                post = (bool.TryParse(dr["post"].ToString(), out result) ? result : false);
                postNotes = dr["postNotes"].ToString();
                repairs = (bool.TryParse(dr["repairs"].ToString(), out result) ? result : false);
                repairsNotes = dr["repairsNotes"].ToString();
                refuse = (bool.TryParse(dr["refuse"].ToString(), out result) ? result : false);
                refuseNotes = dr["refuseNotes"].ToString();
                salaries = (bool.TryParse(dr["salaries"].ToString(), out result) ? result : false);
                salariesNotes = dr["salariesNotes"].ToString();
                security = (bool.TryParse(dr["security"].ToString(), out result) ? result : false);
                securityNotes = dr["securityNotes"].ToString();
                telephone = (bool.TryParse(dr["telephone"].ToString(), out result) ? result : false);
                telephoneNotes = dr["telephoneNotes"].ToString();
                waterExpense = (bool.TryParse(dr["waterExpense"].ToString(), out result) ? result : false);
                waterExpenseNotes = dr["waterExpenseNotes"].ToString();
                municipal = (bool.TryParse(dr["municipal"].ToString(), out result) ? result : false);
                municipalReason = dr["municipalReason"].ToString();
                trust = (bool.TryParse(dr["trust"].ToString(), out result) ? result : false);
                trustNotes = dr["trustNotes"].ToString();
                own = (bool.TryParse(dr["own"].ToString(), out result) ? result : false);
                ownNotes = dr["ownNotes"].ToString();
                investment = (bool.TryParse(dr["investment"].ToString(), out result) ? result : false);
                investmentNotes = dr["investmentNotes"].ToString();
                sundy = (bool.TryParse(dr["sundy"].ToString(), out result) ? result : false);
                sundryNotes = dr["sundryNotes"].ToString();
                assets = (bool.TryParse(dr["assets"].ToString(), out result) ? result : false);
                assetsNotes = dr["assetsNotes"].ToString();
                debtors = (bool.TryParse(dr["debtors"].ToString(), out result) ? result : false);
                debtorsNotes = dr["debtorsNotes"].ToString();
                municipalAccounts = (bool.TryParse(dr["municipalAccounts"].ToString(), out result) ? result : false);
                municipalAccountsNotes = dr["municipalAccountsNotes"].ToString();
                owners = (bool.TryParse(dr["owners"].ToString(), out result) ? result : false);
                ownersNotes = dr["ownersNotes"].ToString();
                suppliers = (bool.TryParse(dr["suppliers"].ToString(), out result) ? result : false);
                suppliersNotes = dr["suppliersNotes"].ToString();
                liabilities = (bool.TryParse(dr["liabilities"].ToString(), out result) ? result : false);
                liabilitiesNotes = dr["liabilitiesNotes"].ToString();
                electricityRecon = int.Parse(dr["electricityRecon"].ToString());
                waterRecon = int.Parse(dr["waterRecon"].ToString());
            }
            else
            {
                id = 0;
            }
        }

        public int id { get; set; }

        public DateTime completeDate { get; set; }

        public int buildingID { get; set; }

        public int finPeriod { get; set; }

        public bool levies { get; set; }

        public String leviesReason { get; set; }

        public bool sewage { get; set; }

        public String sewageNotes { get; set; }

        public bool electricity { get; set; }

        public String electricityNotes { get; set; }

        public bool water { get; set; }

        public String waterNotes { get; set; }

        public bool specialLevies { get; set; }

        public String specialLevyNotes { get; set; }

        public String otherIncomeDescription { get; set; }

        public bool otherIncome { get; set; }

        public String otherIncomeNotes { get; set; }

        public bool memberInterest { get; set; }

        public String memberInterestNotes { get; set; }

        public bool bankInterest { get; set; }

        public String bankInterestNotes { get; set; }

        public bool accountingFees { get; set; }

        public String accountingFeesNotes { get; set; }

        public bool bankCharges { get; set; }

        public String bankChargesNotes { get; set; }

        public bool sewageExpense { get; set; }

        public String sewageExpenseNotes { get; set; }

        public bool deliveries { get; set; }

        public String deliveriesNotes { get; set; }

        public bool electricityExpense { get; set; }

        public String electricityExpenseNotes { get; set; }

        public bool gardens { get; set; }

        public String gardensNotes { get; set; }

        public bool insurance { get; set; }

        public String insuranceNotes { get; set; }

        public bool interestPaid { get; set; }

        public String interestPaidNotes { get; set; }

        public bool managementFees { get; set; }

        public String managementFeesNotes { get; set; }

        public bool meterReading { get; set; }

        public String meterReadingNotes { get; set; }

        public bool printing { get; set; }

        public String printingNotes { get; set; }

        public bool post { get; set; }

        public String postNotes { get; set; }

        public bool repairs { get; set; }

        public String repairsNotes { get; set; }

        public bool refuse { get; set; }

        public String refuseNotes { get; set; }

        public bool salaries { get; set; }

        public String salariesNotes { get; set; }

        public bool security { get; set; }

        public String securityNotes { get; set; }

        public bool telephone { get; set; }

        public String telephoneNotes { get; set; }

        public bool waterExpense { get; set; }

        public String waterExpenseNotes { get; set; }

        public bool municipal { get; set; }

        public String municipalReason { get; set; }

        public bool trust { get; set; }

        public String trustNotes { get; set; }

        public bool own { get; set; }

        public String ownNotes { get; set; }

        public bool investment { get; set; }

        public String investmentNotes { get; set; }

        public bool sundy { get; set; }

        public String sundryNotes { get; set; }

        public bool assets { get; set; }

        public String assetsNotes { get; set; }

        public bool debtors { get; set; }

        public String debtorsNotes { get; set; }

        public bool municipalAccounts { get; set; }

        public String municipalAccountsNotes { get; set; }

        public bool owners { get; set; }

        public String ownersNotes { get; set; }

        public bool suppliers { get; set; }

        public String suppliersNotes { get; set; }

        public bool liabilities { get; set; }

        public String liabilitiesNotes { get; set; }

        public int electricityRecon { get; set; }

        public int waterRecon { get; set; }
    }
}
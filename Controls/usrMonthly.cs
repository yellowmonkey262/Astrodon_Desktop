using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Astrodon.Data;

namespace Astrodon.Controls
{
    public partial class usrMonthly : UserControl
    {
        private List<Building> buildings;
        private SqlDataHandler dh;

        public usrMonthly()
        {
            InitializeComponent();
            dh = new SqlDataHandler();
        }

        private void LoadYears()
        {
            int maxYear = 2016;
            while (maxYear < DateTime.Now.Year)
            {
                maxYear += 1;
                cmbYear.Items.Add(maxYear);
            }
        }

        private void usrMonthly_Load(object sender, EventArgs e)
        {
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbFinPeriod.SelectedIndexChanged -= cmbFinPeriod_SelectedIndexChanged;
            cmbYear.SelectedIndexChanged -= cmbYear_SelectedIndexChanged;
            LoadBuildings();
            LoadYears();
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
            cmbFinPeriod.SelectedIndexChanged += cmbFinPeriod_SelectedIndexChanged;
            cmbYear.SelectedIndexChanged += cmbYear_SelectedIndexChanged;
        }

        private void LoadBuildings()
        {
            Buildings bManager = new Buildings(false);
            buildings = bManager.buildings.OrderBy(c => c.Name).ToList();
            cmbBuilding.DataSource = buildings;
            cmbBuilding.ValueMember = "Abbr";
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.SelectedIndex = -1;
        }

        private void chkBankIncN_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.Checked)
            {
                switch (chk.Name)
                {
                    case "chkLeviesIncY":
                        chkLeviesIncN.Checked = false;
                        break;

                    case "chkLeviesIncN":
                        chkLeviesIncY.Checked = false;
                        break;

                    case "chkWaterIncY":
                        chkWaterIncN.Checked = false;
                        break;

                    case "chkWaterIncN":
                        chkWaterIncY.Checked = false;
                        break;

                    case "chkBankIncN":
                        chkBankIncY.Checked = false;
                        break;

                    case "chkBankIncY":
                        chkBankIncN.Checked = false;
                        break;

                    case "chkDomIncN":
                        chkDomIncY.Checked = false;
                        break;

                    case "chkDomIncY":
                        chkDomIncN.Checked = false;
                        break;

                    case "chkElecIncN":
                        chkElecIncY.Checked = false;
                        break;

                    case "chkElecIncY":
                        chkElecIncN.Checked = false;
                        break;

                    case "chkIntIncN":
                        chkIntIncY.Checked = false;
                        break;

                    case "chkIntIncY":
                        chkIntIncN.Checked = false;
                        break;

                    case "chkOtherIncN":
                        chkOtherIncY.Checked = false;
                        break;

                    case "chkOtherIncY":
                        chkOtherIncN.Checked = false;
                        break;

                    case "chkSpecialIncN":
                        chkSpecialIncY.Checked = false;
                        break;

                    case "chkSpecialIncY":
                        chkSpecialIncN.Checked = false;
                        break;

                    case "chkMunAssetN":
                        chkMunAssetY.Checked = false;
                        break;

                    case "chkMunAssetY":
                        chkMunAssetN.Checked = false;
                        break;

                    case "chkInvestAssN":
                        chkInvestAssY.Checked = false;
                        break;

                    case "chkInvestAssY":
                        chkInvestAssN.Checked = false;
                        break;

                    case "chkOtherAssN":
                        chkOtherAssY.Checked = false;
                        break;

                    case "chkOtherAssY":
                        chkOtherAssN.Checked = false;
                        break;

                    case "chkOwnAccAssN":
                        chkOwnAccAssY.Checked = false;
                        break;

                    case "chkOwnAccAssY":
                        chkOwnAccAssN.Checked = false;
                        break;

                    case "chkSunAssN":
                        chkSunAssY.Checked = false;
                        break;

                    case "chkSunAssY":
                        chkSunAssN.Checked = false;
                        break;

                    case "chkTrustAssN":
                        chkTrustAssY.Checked = false;
                        break;

                    case "chkTrustAssY":
                        chkTrustAssN.Checked = false;
                        break;

                    case "chkDebtLiaN":
                        chkDebtLiaY.Checked = false;
                        break;

                    case "chkDebtLiaY":
                        chkDebtLiaN.Checked = false;
                        break;

                    case "chkMunLiaN":
                        chkMunLiaY.Checked = false;
                        break;

                    case "chkMunLiaY":
                        chkMunLiaN.Checked = false;

                        break;

                    case "chkOtherLiaN":
                        chkOtherLiaY.Checked = false;
                        break;

                    case "chkOtherLiaY":
                        chkOtherLiaN.Checked = false;
                        break;

                    case "chkOwnLiaN":
                        chkOwnLiaY.Checked = false;
                        break;

                    case "chkOwnLiaY":
                        chkOwnLiaN.Checked = false;
                        break;

                    case "chkSunLiaN":
                        chkSunLiaY.Checked = false;
                        break;

                    case "chkSunLiaY":
                        chkSunLiaN.Checked = false;
                        break;

                    case "chkAccExpN":
                        chkAccExpY.Checked = false;
                        break;

                    case "chkAccExpY":
                        chkAccExpN.Checked = false;
                        break;

                    case "chkBankExpN":
                        chkBankExpY.Checked = false;
                        break;

                    case "chkBankExpY":
                        chkBankExpN.Checked = false;
                        break;

                    case "chkDelExpN":
                        chkDelExpY.Checked = false;
                        break;

                    case "chkDelExpY":
                        chkDelExpN.Checked = false;
                        break;

                    case "chkDomExpN":
                        chkDomExpY.Checked = false;
                        break;

                    case "chkDomExpY":
                        chkDomExpN.Checked = false;
                        break;

                    case "chkElecExpN":
                        chkElecExpY.Checked = false;
                        break;

                    case "chkElecExpY":
                        chkElecExpN.Checked = false;
                        break;

                    case "chkGardensExpN":
                        chkGardensExpY.Checked = false;
                        break;

                    case "chkGardensExpY":
                        chkGardensExpN.Checked = false;
                        break;

                    case "chkInsExpN":
                        chkInsExpY.Checked = false;
                        break;

                    case "chkInsExpY":
                        chkInsExpN.Checked = false;
                        break;

                    case "chkIntExpN":
                        chkIntExpY.Checked = false;
                        break;

                    case "chkIntExpY":
                        chkIntExpN.Checked = false;
                        break;

                    case "chkManExpN":
                        chkManExpY.Checked = false;
                        break;

                    case "chkManExpY":
                        chkManExpN.Checked = false;
                        break;

                    case "chkMeterExpN":
                        chkMeterExpY.Checked = false;
                        break;

                    case "chkMeterExpY":
                        chkMeterExpN.Checked = false;
                        break;

                    case "chkPostExpN":
                        chkPostExpY.Checked = false;
                        break;

                    case "chkPostExpY":
                        chkPostExpN.Checked = false;
                        break;

                    case "chkPrintExpN":
                        chkPrintExpY.Checked = false;
                        break;

                    case "chkPrintExpY":
                        chkPrintExpN.Checked = false;
                        break;

                    case "chkRefuseExpN":
                        chkRefuseExpY.Checked = false;
                        break;

                    case "chkRefuseExpY":
                        chkRefuseExpN.Checked = false;
                        break;

                    case "chkRepairsExpN":
                        chkRepairsExpY.Checked = false;
                        break;

                    case "chkRepairsExpY":
                        chkRepairsExpN.Checked = false;
                        break;

                    case "chkSalExpN":
                        chkSalExpY.Checked = false;
                        break;

                    case "chkSalExpY":
                        chkSalExpN.Checked = false;
                        break;

                    case "chkSecExpN":
                        chkSecExpY.Checked = false;
                        break;

                    case "chkSecExpY":
                        chkSecExpN.Checked = false;
                        break;

                    case "chkTelExpN":
                        chkTelExpY.Checked = false;
                        break;

                    case "chkTelExpY":
                        chkTelExpN.Checked = false;
                        break;

                    case "chkWaterExpN":
                        chkWaterExpY.Checked = false;
                        break;

                    case "chkWaterExpY":
                        chkWaterExpN.Checked = false;
                        break;
                    case "chkCSOSLeviesExpY":
                        chkCSOSLeviesExpN.Checked = false;
                        break;
                    case "chkCSOSLeviesExpN":
                        chkCSOSLeviesExpY.Checked = false;
                        break;
                }
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCheckList();
            LoadChecklist();
        }

        private void cmbFinPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCheckList();
            LoadChecklist();
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCheckList();
            LoadChecklist();
        }

        private void LoadChecklist()
        {
            ClearCheckList();
            if (cmbBuilding.SelectedIndex >= 0 && cmbFinPeriod.SelectedIndex >= 0 && cmbYear.SelectedIndex >= 0)
            {
                String building = cmbBuilding.SelectedValue.ToString();
                int period = int.Parse(cmbFinPeriod.SelectedItem.ToString());
                int year = int.Parse(cmbYear.SelectedItem.ToString());
                DateTime findate = new DateTime(year, period, 1);
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var itm = context.tblMonthFins.SingleOrDefault(a => a.buildingID == building && a.findate == findate);
                    if (itm != null)
                    {
                        tbAdditionalInstructions.Text = itm.AdditionalComments;
                        PopulateCheckList(itm);
                    }
                    var prefDate = findate.AddMonths(-1);
                    var prefItem = context.tblMonthFins.SingleOrDefault(a => a.buildingID == building && a.findate == prefDate);
                    if (prefItem != null)
                    {
                        PopulatePrevCheckList(itm != null, prefItem);
                    }

                }
            }
        }

        private void PopulatePrevCheckList(bool hasValues, tblMonthFin itm)
        {
            PopulatePrevCheckList(hasValues,
                        itm.accountingFees,
                        itm.bankCharges,
                        itm.bankInterest,
                        itm.debtors,
                        itm.deliveries,
                        itm.sewageExpense,
                        itm.sewage,
                        itm.electricityExpense,
                        itm.electricity,
                        itm.electricityRecon,
                        itm.levies,
                        itm.gardens,
                        itm.insurance,
                        itm.interestPaid,
                        itm.memberInterest,
                        itm.investment,
                        itm.managementFees,
                        itm.meterReading,
                        itm.municipal,
                        itm.municipalAccounts,
                        itm.assets,
                        itm.otherIncome,
                        itm.liabilities,
                        itm.own,
                        itm.owners,
                        itm.post,
                        itm.printing,
                        itm.refuse,
                        itm.repairs,
                        itm.salaries,
                        itm.security,
                        itm.specialLevies,
                        itm.sundy,
                        itm.suppliers,
                        itm.telephone,
                        itm.trust,
                        itm.waterExpense,
                        itm.water,
                        itm.waterRecon,
                        itm.csosLeviesExpense,
                        itm.accountingFeesNotes,
                        itm.sundryNotes,
                        itm.assetsNotes,
                        itm.bankChargesNotes,
                        itm.bankInterestNotes,
                        itm.debtorsNotes,
                        itm.deliveriesNotes,
                        itm.sewageNotes,
                        itm.sewageExpenseNotes,
                        itm.electricityExpenseNotes,
                        itm.electricityNotes,
                        itm.gardensNotes,
                        itm.insuranceNotes,
                        itm.interestPaidNotes,
                        itm.memberInterestNotes,
                        itm.investmentNotes,
                        itm.leviesReason,
                        itm.liabilitiesNotes,
                        itm.managementFeesNotes,
                        itm.meterReadingNotes,
                        itm.municipalAccountsNotes,
                        itm.municipalReason,
                        itm.otherIncomeNotes,
                        itm.ownNotes,
                        itm.ownersNotes,
                        itm.postNotes,
                        itm.printingNotes,
                        itm.refuseNotes,
                        itm.repairsNotes,
                        itm.salariesNotes,
                        itm.securityNotes,
                        itm.specialLevyNotes,
                        itm.suppliersNotes,
                        itm.telephoneNotes,
                        itm.trustNotes,
                        itm.waterExpenseNotes,
                        itm.waterNotes,
                        itm.csosLeviesNotes);
        }

        private void PopulateCheckList(tblMonthFin itm)
        {
            PopulateCheckList(
                        itm.accountingFees,
                        itm.bankCharges,
                        itm.bankInterest,
                        itm.debtors,
                        itm.deliveries,
                        itm.sewageExpense,
                        itm.sewage,
                        itm.electricityExpense,
                        itm.electricity,
                        itm.electricityRecon,
                        itm.levies,
                        itm.gardens,
                        itm.insurance,
                        itm.interestPaid,
                        itm.memberInterest,
                        itm.investment,
                        itm.managementFees,
                        itm.meterReading,
                        itm.municipal,
                        itm.municipalAccounts,
                        itm.assets,
                        itm.otherIncome,
                        itm.liabilities,
                        itm.own,
                        itm.owners,
                        itm.post,
                        itm.printing,
                        itm.refuse,
                        itm.repairs,
                        itm.salaries,
                        itm.security,
                        itm.specialLevies,
                        itm.sundy,
                        itm.suppliers,
                        itm.telephone,
                        itm.trust,
                        itm.waterExpense,
                        itm.water,
                        itm.waterRecon,
                        itm.csosLeviesExpense,
                        itm.accountingFeesNotes,
                        itm.sundryNotes,
                        itm.assetsNotes,
                        itm.bankChargesNotes,
                        itm.bankInterestNotes,
                        itm.debtorsNotes,
                        itm.deliveriesNotes,
                        itm.sewageNotes,
                        itm.sewageExpenseNotes,
                        itm.electricityExpenseNotes,
                        itm.electricityNotes,
                        itm.gardensNotes,
                        itm.insuranceNotes,
                        itm.interestPaidNotes,
                        itm.memberInterestNotes,
                        itm.investmentNotes,
                        itm.leviesReason,
                        itm.liabilitiesNotes,
                        itm.managementFeesNotes,
                        itm.meterReadingNotes,
                        itm.municipalAccountsNotes,
                        itm.municipalReason,
                        itm.otherIncomeNotes,
                        itm.ownNotes,
                        itm.ownersNotes,
                        itm.postNotes,
                        itm.printingNotes,
                        itm.refuseNotes,
                        itm.repairsNotes,
                        itm.salariesNotes,
                        itm.securityNotes,
                        itm.specialLevyNotes,
                        itm.suppliersNotes,
                        itm.telephoneNotes,
                        itm.trustNotes,
                        itm.waterExpenseNotes,
                        itm.waterNotes,
                        itm.csosLeviesNotes);

        }

        private void ClearCheckList()
        {
            tbAdditionalInstructions.Text = "";

            chkAccExpN.Checked = chkAccExpY.Checked = chkBankExpN.Checked = chkBankExpY.Checked = chkBankIncN.Checked = chkBankIncY.Checked = chkDebtLiaN.Checked = false;
            chkDebtLiaY.Checked = chkDelExpN.Checked = chkDelExpY.Checked = chkDomExpN.Checked = chkDomExpY.Checked = chkDomIncN.Checked = chkDomIncY.Checked = chkWaterUnder.Checked = false;
            chkElecExpN.Checked = chkElecExpY.Checked = chkElecIncN.Checked = chkElecIncY.Checked = chkElecInline.Checked = chkElecOver.Checked = chkElecUnder.Checked = false;
            chkGardensExpN.Checked = chkGardensExpY.Checked = chkInsExpN.Checked = chkInsExpY.Checked = chkIntExpN.Checked = chkIntExpY.Checked = chkIntIncN.Checked = chkIntIncY.Checked = false;
            chkInvestAssN.Checked = chkInvestAssY.Checked = chkLeviesIncN.Checked = chkLeviesIncY.Checked = chkManExpN.Checked = chkManExpY.Checked = chkMeterExpN.Checked = false;
            chkMeterExpY.Checked = chkMunAssetN.Checked = chkMunAssetY.Checked = chkMunLiaN.Checked = chkMunLiaY.Checked = chkOtherAssN.Checked = chkOtherAssY.Checked = false;
            chkOtherIncN.Checked = chkOtherIncY.Checked = chkOtherLiaN.Checked = chkOtherLiaY.Checked = chkOwnAccAssN.Checked = chkOwnAccAssY.Checked = chkOwnLiaN.Checked = false;
            chkOwnLiaY.Checked = chkPostExpN.Checked = chkPostExpY.Checked = chkPrintExpN.Checked = chkPrintExpY.Checked = chkRefuseExpN.Checked = chkRefuseExpY.Checked = false;
            chkRepairsExpN.Checked = chkRepairsExpY.Checked = chkSalExpN.Checked = chkSalExpY.Checked = chkSecExpN.Checked = chkSecExpY.Checked = chkSpecialIncN.Checked = false;
            chkSpecialIncY.Checked = chkSunAssN.Checked = chkSunAssY.Checked = chkSunLiaN.Checked = chkSunLiaY.Checked = chkTelExpN.Checked = chkTelExpY.Checked = chkWaterOver.Checked = false;
            chkTrustAssN.Checked = chkTrustAssY.Checked = chkWaterExpN.Checked = chkWaterExpY.Checked = chkWaterIncN.Checked = chkWaterIncY.Checked = chkWaterInline.Checked = false;

            chkCSOSLeviesExpN.Checked = chkCSOSLeviesExpY.Checked = chkEDCSOSLevies.Checked = chkEPCSOSLevies.Checked = false;


            txtAccountFees.Text = txtAccruals.Text = txtAssets.Text = txtBankCharges.Text = txtBankIncome.Text = txtDebtors.Text = txtDeliveries.Text = String.Empty;
            txtDomesticIncome.Text = txtDomExpense.Text = txtElectricity.Text = txtElectricityIncome.Text = txtGardens.Text = txtInsurance.Text = txtInterest.Text = String.Empty;
            txtInterestIncome.Text = txtInvestment.Text = txtLevies.Text = txtLiabilities.Text = txtManagementFees.Text = txtMeter.Text = txtMunAcc.Text = String.Empty;
            txtMunDep.Text = txtOtherIncome.Text = txtOwnAccount.Text = txtOwnerDep.Text = txtPost.Text = txtPrinting.Text = txtRefuse.Text = String.Empty;
            txtRepairs.Text = txtSalaries.Text = txtSecurity.Text = txtSpecialIncome.Text = txtSuppliers.Text = txtTelephone.Text = txtTrust.Text = String.Empty;
            txtWater.Text = txtWaterIncome.Text = String.Empty;
        }

        private void PopulateCheckList(int? accExp, int? bankExp, int? bankInc, int? debtLia, int? delExp, int? domExp, int? domInc, int? elecExp, int? elecInc, int? elecRecover,
            int? levies, int? gardensExp, int? insExp, int? intExp, int? intInc, int? invAss, int? manExp, int? meterExp, int? munAss, int? munLia, int? otherAss, int? otherInc,
            int? otherLia, int? ownAss, int? ownLia, int? postExp, int? printExp, int? refuseExp, int? repairExp, int? salExp, int? secExp, int? specialInc, int? sunAss,
            int? sunLia, int? telExp, int? trustAss, int? waterExp, int? waterInc, int? waterRecover, 
            int? csosLeviesExp,
            String accountFees, String accruals, String assets, String bankCharges, String bankIncome,
            String debtors, String deliveries, String domIncome, String domExpense, String electricity, String elecIncome, String gardens, String insurance, String interest,
            String intIncome, String investment, String leviesReason, String liabilities, String manFees, String meter, String muniAcc, String muniDep, String otherIncome,
            String ownAccount, String ownerDeposits, String post, String printing, String refuse, String repairs, String salaries, String security, String specialIncome,
            String suppliers, String telephone, String trust, String water, String waterIncome,
            string csosLeviesComment)
        {
            #region Checkboxes

            chkAccExpN.Checked = accExp == 2;
            chkAccExpY.Checked = accExp == 1;
            chkBankExpN.Checked = bankExp == 2;
            chkBankExpY.Checked = bankExp == 1;
            chkBankIncN.Checked = bankInc == 2;
            chkBankIncY.Checked = bankInc == 1;
            chkDebtLiaN.Checked = debtLia == 2;
            chkDebtLiaY.Checked = debtLia == 1;
            chkDelExpN.Checked = delExp == 2;
            chkDelExpY.Checked = delExp == 1;
            chkDomExpN.Checked = domExp == 2;
            chkDomExpY.Checked = domExp == 1;
            chkDomIncN.Checked = domInc == 2;
            chkDomIncY.Checked = domInc == 1;
            chkElecExpN.Checked = elecExp == 2;
            chkElecExpY.Checked = elecExp == 1;
            chkElecIncN.Checked = elecInc == 2;
            chkElecIncY.Checked = elecInc == 1;
            chkElecInline.Checked = elecRecover == 1;
            chkElecOver.Checked = elecRecover == 2;
            chkElecUnder.Checked = elecRecover == 3;
            chkGardensExpN.Checked = gardensExp == 2;
            chkGardensExpY.Checked = gardensExp == 1;
            chkInsExpN.Checked = insExp == 2;
            chkInsExpY.Checked = insExp == 1;
            chkIntExpN.Checked = intExp == 2;
            chkIntExpY.Checked = intExp == 1;
            chkIntIncN.Checked = intInc == 2;
            chkIntIncY.Checked = intInc == 1;
            chkInvestAssN.Checked = invAss == 2;
            chkInvestAssY.Checked = invAss == 1;
            chkLeviesIncN.Checked = levies == 2;
            chkLeviesIncY.Checked = levies == 1;
            chkManExpN.Checked = manExp == 2;
            chkManExpY.Checked = manExp == 1;
            chkMeterExpN.Checked = meterExp == 2;
            chkMeterExpY.Checked = meterExp == 1;
            chkMunAssetN.Checked = munAss == 2;
            chkMunAssetY.Checked = munAss == 1;
            chkMunLiaN.Checked = munLia == 2;
            chkMunLiaY.Checked = munLia == 1;
            chkOtherAssN.Checked = otherAss == 2;
            chkOtherAssY.Checked = otherAss == 1;
            chkOtherIncN.Checked = otherInc == 2;
            chkOtherIncY.Checked = otherInc == 1;
            chkOtherLiaN.Checked = otherLia == 2;
            chkOtherLiaY.Checked = otherLia == 1;
            chkOwnAccAssN.Checked = ownAss == 2;
            chkOwnAccAssY.Checked = ownAss == 1;
            chkOwnLiaN.Checked = ownLia == 2;
            chkOwnLiaY.Checked = ownLia == 1;
            chkPostExpN.Checked = postExp == 2;
            chkPostExpY.Checked = postExp == 1;
            chkPrintExpN.Checked = printExp == 2;
            chkPrintExpY.Checked = printExp == 1;
            chkRefuseExpN.Checked = refuseExp == 2;
            chkRefuseExpY.Checked = refuseExp == 1;
            chkRepairsExpN.Checked = repairExp == 2;
            chkRepairsExpY.Checked = repairExp == 1;
            chkSalExpN.Checked = salExp == 2;
            chkSalExpY.Checked = salExp == 1;
            chkSecExpN.Checked = secExp == 2;
            chkSecExpY.Checked = secExp == 1;
            chkSpecialIncN.Checked = specialInc == 2;
            chkSpecialIncY.Checked = specialInc == 1;
            chkSunAssN.Checked = sunAss == 2;
            chkSunAssY.Checked = sunAss == 1;
            chkSunLiaN.Checked = sunLia == 2;
            chkSunLiaY.Checked = sunLia == 1;
            chkTelExpN.Checked = telExp == 2;
            chkTelExpY.Checked = telExp == 1;
            chkTrustAssN.Checked = trustAss == 2;
            chkTrustAssY.Checked = trustAss == 1;

            chkWaterExpN.Checked = waterExp == 2;
            chkWaterExpY.Checked = waterExp == 1;
            chkCSOSLeviesExpN.Checked = csosLeviesExp == 2;
            chkCSOSLeviesExpY.Checked = csosLeviesExp == 1;

            chkWaterIncN.Checked = waterInc == 2;
            chkWaterIncY.Checked = waterInc == 1;
            chkWaterInline.Checked = waterRecover == 1;
            chkWaterOver.Checked = waterRecover == 2;
            chkWaterUnder.Checked = waterRecover == 3;

            #endregion Checkboxes

            txtAccountFees.Text = accountFees;
            txtAccruals.Text = accruals;
            txtAssets.Text = assets;
            txtBankCharges.Text = bankCharges;
            txtBankIncome.Text = bankIncome;
            txtDebtors.Text = debtors;
            txtDeliveries.Text = deliveries;
            txtDomesticIncome.Text = domIncome;
            txtDomExpense.Text = domExpense;
            txtElectricity.Text = electricity;
            txtElectricityIncome.Text = elecIncome;
            txtGardens.Text = gardens;
            txtInsurance.Text = insurance;
            txtInterest.Text = interest;
            txtInterestIncome.Text = intIncome;
            txtInvestment.Text = investment;
            txtLevies.Text = leviesReason;
            txtLiabilities.Text = liabilities;
            txtManagementFees.Text = manFees;
            txtMeter.Text = meter;
            txtMunAcc.Text = muniAcc;
            txtMunDep.Text = muniDep;
            txtOtherIncome.Text = otherIncome;
            txtOwnAccount.Text = ownAccount;
            txtOwnerDep.Text = ownerDeposits;
            txtPost.Text = post;
            txtPrinting.Text = printing;
            txtRefuse.Text = refuse;
            txtRepairs.Text = repairs;
            txtSalaries.Text = salaries;
            txtSecurity.Text = security;
            txtSpecialIncome.Text = specialIncome;
            txtSuppliers.Text = suppliers;
            txtTelephone.Text = telephone;
            txtTrust.Text = trust;
            txtWater.Text = water;
            txtWaterIncome.Text = waterIncome;
            txtCSOSLevies.Text = csosLeviesComment;
        }

        private void PopulatePrevCheckList(bool hasValues, int?accExp, int?bankExp, int?bankInc, int?debtLia, int?delExp, int?domExp, int?domInc, int?elecExp, int?elecInc, int?elecRecover,
            int?levies, int?gardensExp, int?insExp, int?intExp, int?intInc, int?invAss, int?manExp, int?meterExp, int?munAss, int?munLia, int?otherAss, int?otherInc,
            int?otherLia, int?ownAss, int?ownLia, int?postExp, int?printExp, int?refuseExp, int?repairExp, int?salExp, int?secExp, int?specialInc, int?sunAss,
            int?sunLia, int?telExp, int?trustAss, int?waterExp, int?waterInc, int?waterRecover,
            int? csosLeviesExp,
            String accountFees, String accruals, String assets, String bankCharges, String bankIncome,
            String debtors, String deliveries, String domIncome, String domExpense, String electricity, String elecIncome, String gardens, String insurance, String interest,
            String intIncome, String investment, String leviesReason, String liabilities, String manFees, String meter, String muniAcc, String muniDep, String otherIncome,
            String ownAccount, String ownerDeposits, String post, String printing, String refuse, String repairs, String salaries, String security, String specialIncome,
            String suppliers, String telephone, String trust, String water, String waterIncome,
            string csosLeviesComment)
        {
            #region Checkboxes

            txtPrevLevies.Text = (levies == 1 ? "Y" : "N");
            txtPrevDomInc.Text = (domInc == 1 ? "Y" : "N");
            txtPrevElecInc.Text = (elecInc == 1 ? "Y" : "N");
            txtPrevWaterInc.Text = (waterInc == 1 ? "Y" : "N");
            txtPrevSpecialInc.Text = (specialInc == 1 ? "Y" : "N");
            txtPrevOtherInc.Text = (otherInc == 1 ? "Y" : "N");
            txtPrevMemberInc.Text = (intInc == 1 ? "Y" : "N");
            txtPrevInterestInc.Text = (bankInc == 1 ? "Y" : "N");
            txtPrevAcc.Text = (accExp == 1 ? "Y" : "N");
            txtPrevBank.Text = (bankExp == 1 ? "Y" : "N");
            txtPrevDebtors.Text = (debtLia == 1 ? "Y" : "N");
            txtPrevDel.Text = (delExp == 1 ? "Y" : "N");
            txtPrevDom.Text = (domExp == 1 ? "Y" : "N");
            txtPrevEle.Text = (elecExp == 1 ? "Y" : "N");
            txtPrevGardens.Text = (gardensExp == 1 ? "Y" : "N");
            txtPrevInsurance.Text = (insExp == 1 ? "Y" : "N");
            txtPrevInt.Text = (intExp == 1 ? "Y" : "N");
            txtPrevInv.Text = (invAss == 1 ? "Y" : "N");
            txtPrevMan.Text = (manExp == 1 ? "Y" : "N");
            txtPrevMeter.Text = (meterExp == 1 ? "Y" : "N");
            txtPrevMunDep.Text = (munAss == 1 ? "Y" : "N");
            txtPrevMun.Text = (munLia == 1 ? "Y" : "N");
            txtPrevOtherAss.Text = (otherAss == 1 ? "Y" : "N");
            txtPrevLia.Text = (otherLia == 1 ? "Y" : "N");
            txtPrevOwn.Text = (ownAss == 1 ? "Y" : "N");
            txtPrevOwnDep.Text = (ownLia == 1 ? "Y" : "N");
            txtPrevPost.Text = (postExp == 1 ? "Y" : "N");
            txtPrevPrint.Text = (printExp == 1 ? "Y" : "N");
            txtPrevRefuse.Text = (refuseExp == 1 ? "Y" : "N");
            txtPrevRepairs.Text = (repairExp == 1 ? "Y" : "N");
            txtPrevSalaries.Text = (salExp == 1 ? "Y" : "N");
            txtPrevSec.Text = (secExp == 1 ? "Y" : "N");
            txtPrevSunAss.Text = (sunAss == 1 ? "Y" : "N");
            txtPrevSuppliers.Text = (sunLia == 1 ? "Y" : "N");
            txtPrevTelephone.Text = (telExp == 1 ? "Y" : "N");
            txtPrevTrust.Text = (trustAss == 1 ? "Y" : "N");
            txtPrevWater.Text = (waterExp == 1 ? "Y" : "N");
            txtPrevCSOSLevies.Text = (csosLeviesExp == 1 ? "Y" : "N");

            #region Old Values

            if (!hasValues)
            {
                chkLeviesIncN.Checked = txtPrevLevies.Text == "N";
                chkDomIncN.Checked = txtPrevDomInc.Text == "N";
                chkElecIncN.Checked = txtPrevElecInc.Text == "N";
                chkWaterIncN.Checked = txtPrevWaterInc.Text == "N";
                chkSpecialIncN.Checked = txtPrevSpecialInc.Text == "N";
                chkOtherIncN.Checked = txtPrevOtherInc.Text == "N";
                chkIntIncN.Checked = txtPrevMemberInc.Text == "N";
                chkBankIncN.Checked = txtPrevInterestInc.Text == "N";
                chkAccExpN.Checked = txtPrevAcc.Text == "N";
                chkBankExpN.Checked = txtPrevBank.Text == "N";
                chkDebtLiaN.Checked = txtPrevDebtors.Text == "N";
                chkDelExpN.Checked = txtPrevDel.Text == "N";
                chkDomExpN.Checked = txtPrevDom.Text == "N";
                chkElecExpN.Checked = txtPrevEle.Text == "N";
                chkGardensExpN.Checked = txtPrevGardens.Text == "N";
                chkInsExpN.Checked = txtPrevInsurance.Text == "N";
                chkIntExpN.Checked = txtPrevInt.Text == "N";
                chkInvestAssN.Checked = txtPrevInv.Text == "N";
                chkManExpN.Checked = txtPrevMan.Text == "N";
                chkMeterExpN.Checked = txtPrevMeter.Text == "N";
                chkMunAssetN.Checked = txtPrevMunDep.Text == "N";
                chkMunLiaN.Checked = txtPrevMun.Text == "N";
                chkOtherAssN.Checked = txtPrevOtherAss.Text == "N";
                chkOtherLiaN.Checked = txtPrevLia.Text == "N";
                chkOwnAccAssN.Checked = txtPrevOwn.Text == "N";
                chkOwnLiaN.Checked = txtPrevOwnDep.Text == "N";
                chkPostExpN.Checked = txtPrevPost.Text == "N";
                chkPrintExpN.Checked = txtPrevPrint.Text == "N";
                chkRefuseExpN.Checked = txtPrevRefuse.Text == "N";
                chkRepairsExpN.Checked = txtPrevRepairs.Text == "N";
                chkSalExpN.Checked = txtPrevSalaries.Text == "N";
                chkSecExpN.Checked = txtPrevSec.Text == "N";
                chkSunAssN.Checked = txtPrevSunAss.Text == "N";
                chkSunLiaN.Checked = txtPrevSuppliers.Text == "N";
                chkTelExpN.Checked = txtPrevTelephone.Text == "N";
                chkTrustAssN.Checked = txtPrevTrust.Text == "N";
                chkWaterExpN.Checked = txtPrevWater.Text == "N";

                txtLevies.Text = leviesReason;
                txtDomesticIncome.Text = domIncome;
                txtElectricityIncome.Text = elecIncome;
                txtInterestIncome.Text = intIncome;
                txtBankIncome.Text = bankIncome;
                txtSpecialIncome.Text = specialIncome;
                txtWaterIncome.Text = waterIncome;
                txtAccountFees.Text = accountFees;
                txtAccruals.Text = accruals;
                txtAssets.Text = assets;
                txtBankCharges.Text = bankCharges;
                txtDebtors.Text = debtors;
                txtDeliveries.Text = deliveries;
                txtDomExpense.Text = domExpense;
                txtElectricity.Text = electricity;
                txtGardens.Text = gardens;
                txtInsurance.Text = insurance;
                txtInterest.Text = interest;
                txtInvestment.Text = investment;
                txtLiabilities.Text = liabilities;
                txtManagementFees.Text = manFees;
                txtMeter.Text = meter;
                txtMunAcc.Text = muniAcc;
                txtMunDep.Text = muniDep;
                txtOwnAccount.Text = ownAccount;
                txtOwnerDep.Text = ownerDeposits;
                txtPost.Text = post;
                txtPrinting.Text = printing;
                txtRefuse.Text = refuse;
                txtRepairs.Text = repairs;
                txtSalaries.Text = salaries;
                txtSecurity.Text = security;

                txtSuppliers.Text = suppliers;
                txtTelephone.Text = telephone;
                txtTrust.Text = trust;
                txtWater.Text = water;
            }

            #endregion Old Values

            txtPrevLeviesNotes.Text = leviesReason;
            txtPrevDomNotes.Text = domIncome;
            txtPrevElecNotes.Text = elecIncome;
            txtPrevInterestNotes.Text = bankIncome;
            txtPrevMemberNotes.Text = intIncome;
            txtPrevOtherNotes.Text = otherIncome;
            txtPrevSpecialNotes.Text = specialIncome;
            txtPrevWaterNotes.Text = waterIncome;
            txtPrevAccNotes.Text = accountFees;
            txtPrevSunAssNotes.Text = accruals;
            txtPrevAssNotes.Text = assets;
            txtPrevBankNotes.Text = bankCharges;
            txtPrevDebtorNotes.Text = debtors;
            txtPrevDelNotes.Text = deliveries;
            txtPrevDomExpNotes.Text = domExpense;
            txtPrevEleNotes.Text = electricity;
            txtPrevGardensNotes.Text = gardens;
            txtPrevInsNotes.Text = insurance;
            txtPrevIntNotes.Text = interest;
            txtPrevInvNotes.Text = investment;
            txtPrevLiaNotes.Text = liabilities;
            txtPrevManNotes.Text = manFees;
            txtPrevMeterNotes.Text = meter;
            txtPrevMun.Text = muniAcc;
            txtPrevMunDep.Text = muniDep;
            txtPrevOwnNotes.Text = ownAccount;
            txtPrevOwnDepNotes.Text = ownerDeposits;
            txtPrevPost.Text = post;
            txtPrevPrintNotes.Text = printing;
            txtPrevRefuseNotes.Text = refuse;
            txtPrevRepairsNotes.Text = repairs;
            txtPrevSalNotes.Text = salaries;
            txtPrevSecNotes.Text = security;
            txtPrevSupplierNotes.Text = suppliers;
            txtPrevTelNotes.Text = telephone;
            txtPrevTrustNotes.Text = trust;
            txtPrevWaterExpNotes.Text = water;
            txtPrevCSOSLeviesExpNotes.Text = csosLeviesComment;

            #endregion Checkboxes
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadChecklist();
        }

        private Random _RandomGenerator = new Random();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedItem == null || cmbFinPeriod.SelectedItem == null || cmbYear.SelectedItem == null)
            {
                if (cmbBuilding.SelectedItem == null)
                {
                    MessageBox.Show("Please select a building", "Monthly Financials", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (cmbFinPeriod.SelectedItem == null)
                {
                    MessageBox.Show("Please select a month", "Monthly Financials", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (cmbYear.SelectedItem == null)
                {
                    MessageBox.Show("Please select a year", "Monthly Financials", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    String building = cmbBuilding.SelectedValue.ToString();
                    int myPeriod = int.Parse(cmbFinPeriod.SelectedItem.ToString());
                    int myYear = int.Parse(cmbYear.SelectedItem.ToString());
                    DateTime myFinDate = new DateTime(myYear, myPeriod, 1);
                    var itm = context.tblMonthFins.SingleOrDefault(a => a.buildingID == building && a.findate == myFinDate);
                    
                    if(itm == null)
                    {
                        itm = new tblMonthFin()
                        {
                            buildingID = building,
                            userID = Controller.user.id,
                            finPeriod = myFinDate.Month,
                            year = myFinDate.Year,
                            findate = myFinDate
                        };
                        context.tblMonthFins.Add(itm);

                        var buildingEntity = context.tblBuildings.FirstOrDefault(a => a.Code == building);
                        if(buildingEntity != null)
                        {
                            if(buildingEntity.IsFixed == false)
                            {
                                //create a new random number between 1 and 20 inclusive
                                var rNumber = _RandomGenerator.Next(1, 21);
                                if (rNumber < 1)
                                    rNumber = 1;
                                if (rNumber > 20)
                                    rNumber = 20;
                                buildingEntity.FinancialDayOfMonth = ValidateWeekendOrPublicHoliday(rNumber, context);
                            }
                        }
                    }
                    itm.userID = Controller.user.id;
                    itm.completeDate = DateTime.Now;

                    itm.levies = this.chkLeviesIncY.Checked ? 1 : this.chkLeviesIncN.Checked ? 2 : 0;
                    itm.leviesReason = txtLevies.Text;
                    itm.sewage = this.chkDomIncY.Checked ? 1 : this.chkDomIncN.Checked ? 2 : 0;
                    itm.sewageNotes = txtDomesticIncome.Text;
                    itm.electricity = this.chkElecIncY.Checked ? 1 : this.chkElecIncN.Checked ? 2 : 0;
                    itm.electricityNotes = txtElectricityIncome.Text;
                    itm.water = this.chkWaterIncY.Checked ? 1 : this.chkWaterIncN.Checked ? 2 : 0;
                    itm.waterNotes = txtWaterIncome.Text;
                    itm.specialLevies = this.chkSpecialIncY.Checked ? 1 : this.chkSpecialIncN.Checked ? 2 : 0;
                    itm.specialLevyNotes = txtSpecialIncome.Text;
                    itm.otherIncome = this.chkOtherIncY.Checked ? 1 : this.chkOtherIncN.Checked ? 2 : 0;
                    itm.otherIncomeNotes = txtOtherIncome.Text;
                    itm.memberInterest = this.chkIntIncY.Checked ? 1 : this.chkIntIncN.Checked ? 2 : 0;
                    itm.memberInterestNotes = txtInterestIncome.Text;
                    itm.bankInterest = this.chkBankIncY.Checked ? 1 : this.chkBankIncN.Checked ? 2 : 0;
                    itm.bankInterestNotes = txtBankIncome.Text;
                    itm.accountingFees = this.chkAccExpY.Checked ? 1 : this.chkAccExpN.Checked ? 2 : 0;
                    itm.accountingFeesNotes = txtAccountFees.Text;
                    itm.bankCharges = this.chkBankExpY.Checked ? 1 : this.chkBankExpN.Checked ? 2 : 0;
                    itm.bankChargesNotes = txtBankCharges.Text;
                    itm.sewageExpense = this.chkDomExpY.Checked ? 1 : this.chkDomExpN.Checked ? 2 : 0;
                    itm.sewageExpenseNotes = txtDomExpense.Text;
                    itm.deliveries = this.chkDelExpY.Checked ? 1 : this.chkDelExpN.Checked ? 2 : 0;
                    itm.deliveriesNotes = txtDeliveries.Text;
                    itm.electricityExpense = this.chkElecExpY.Checked ? 1 : this.chkElecExpN.Checked ? 2 : 0;
                    itm.electricityExpenseNotes = txtElectricity.Text;
                    itm.gardens = this.chkGardensExpY.Checked ? 1 : this.chkGardensExpN.Checked ? 2 : 0;
                    itm.gardensNotes = txtGardens.Text;
                    itm.insurance = this.chkInsExpY.Checked ? 1 : this.chkInsExpN.Checked ? 2 : 0;
                    itm.insuranceNotes = txtInsurance.Text;
                    itm.interestPaid = this.chkIntExpY.Checked ? 1 : this.chkIntExpN.Checked ? 2 : 0;
                    itm.interestPaidNotes = txtInterest.Text;
                    itm.managementFees = this.chkManExpY.Checked ? 1 : this.chkManExpN.Checked ? 2 : 0;
                    itm.managementFeesNotes = txtManagementFees.Text;
                    itm.meterReading = this.chkMeterExpY.Checked ? 1 : this.chkMeterExpN.Checked ? 2 : 0;
                    itm.meterReadingNotes = txtMeter.Text;
                    itm.printing = this.chkPrintExpY.Checked ? 1 : this.chkPrintExpN.Checked ? 2 : 0;
                    itm.printingNotes = txtPrinting.Text;
                    itm.post = this.chkPostExpY.Checked ? 1 : this.chkPostExpN.Checked ? 2 : 0;
                    itm.postNotes = txtPost.Text;
                    itm.repairs = this.chkRepairsExpY.Checked ? 1 : this.chkRepairsExpN.Checked ? 2 : 0;
                    itm.repairsNotes = txtRepairs.Text;
                    itm.refuse = this.chkRefuseExpY.Checked ? 1 : this.chkRefuseExpN.Checked ? 2 : 0;
                    itm.refuseNotes = txtRefuse.Text;
                    itm.salaries = this.chkSalExpY.Checked ? 1 : this.chkSalExpN.Checked ? 2 : 0;
                    itm.salariesNotes = txtSalaries.Text;
                    itm.security = this.chkSecExpY.Checked ? 1 : this.chkSecExpN.Checked ? 2 : 0;
                    itm.securityNotes = txtSecurity.Text;
                    itm.telephone = this.chkTelExpY.Checked ? 1 : this.chkTelExpN.Checked ? 2 : 0;
                    itm.telephoneNotes = txtTelephone.Text;
                    itm.waterExpense = this.chkWaterExpY.Checked ? 1 : this.chkWaterExpN.Checked ? 2 : 0;
                    itm.waterExpenseNotes = txtWater.Text;
                    itm.municipal = this.chkMunAssetY.Checked ? 1 : this.chkMunAssetN.Checked ? 2 : 0;
                    itm.municipalReason = txtMunDep.Text;
                    itm.trust = this.chkTrustAssY.Checked ? 1 : this.chkTrustAssN.Checked ? 2 : 0;
                    itm.trustNotes = txtTrust.Text;
                    itm.own = this.chkOwnAccAssY.Checked ? 1 : this.chkOwnAccAssN.Checked ? 2 : 0;
                    itm.ownNotes = txtOwnAccount.Text;
                    itm.investment = this.chkInvestAssY.Checked ? 1 : this.chkInvestAssN.Checked ? 2 : 0;
                    itm.investmentNotes = txtInvestment.Text;
                    itm.sundy = this.chkSunAssY.Checked ? 1 : this.chkSunAssN.Checked ? 2 : 0;
                    itm.sundryNotes = txtAccruals.Text;
                    itm.assets = this.chkOtherAssY.Checked ? 1 : this.chkOtherAssN.Checked ? 2 : 0;
                    itm.assetsNotes = txtAssets.Text;
                    itm.debtors = this.chkDebtLiaY.Checked ? 1 : this.chkDebtLiaN.Checked ? 2 : 0;
                    itm.debtorsNotes = txtDebtors.Text;
                    itm.municipalAccounts = this.chkMunLiaY.Checked ? 1 : this.chkMunLiaN.Checked ? 2 : 0;
                    itm.municipalAccountsNotes = txtMunAcc.Text;
                    itm.owners = this.chkOwnLiaY.Checked ? 1 : this.chkOwnLiaN.Checked ? 2 : 0;
                    itm.ownersNotes = txtOwnerDep.Text;
                    itm.suppliers = this.chkSunLiaY.Checked ? 1 : this.chkSunLiaN.Checked ? 2 : 0;
                    itm.suppliersNotes = txtSuppliers.Text;
                    itm.liabilities = this.chkOtherLiaY.Checked ? 1 : this.chkOtherLiaN.Checked ? 2 : 0;
                    itm.liabilitiesNotes = txtLiabilities.Text;
                    itm.waterRecon = this.chkWaterInline.Checked ? 1 : this.chkWaterOver.Checked ? 2 : 3;
                    itm.electricityRecon = this.chkElecInline.Checked ? 1 : this.chkElecOver.Checked ? 2 : 3;
                    context.SaveChanges();
                    var pdfReport = CreateExcel(true);
                    itm.CheckListPDF = pdfReport;
                    context.SaveChanges();

                    ProcessEmail(pdfReport);

                    MessageBox.Show("Record saved!");
                }
            }
        }

        private void ProcessEmail(byte[] attachment)
        {
            Building building = buildings[cmbBuilding.SelectedIndex];
            string pmEmail = building.PM;
            string debtorEmail = building.Debtor;

            string toEmail = pmEmail;

            if (cbEmailTo.SelectedIndex == 1)
                toEmail = debtorEmail;
            else if (cbEmailTo.SelectedIndex == 2)
                toEmail = debtorEmail + ";" + pmEmail;

            var attachmentList = new List<Tuple<string, byte[]>>();
            attachmentList.Add(new Tuple<string, byte[]>("FinancialReport.pdf", attachment));
            string subject = building.Name + " " + cmbFinPeriod.SelectedItem.ToString() + "/" + cmbYear.SelectedItem.ToString();

            string cced = "";
            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from u in context.tblUsers
                        where u.ProcessCheckLists == true && u.Active == true
                        select u.email;

                foreach (var email in q.ToList())
                {
                    if (!String.IsNullOrWhiteSpace(email) && !toEmail.Contains(email))
                    {
                        if (!String.IsNullOrWhiteSpace(cced))
                            cced = cced + ";" + email;
                        else
                            cced = email;
                    }
                }
            }

            OutlookEmail.SendEmail(toEmail, cced, "tertia@astrodon.co.za", "Financial Report - " + subject, txtComments.Text, attachmentList);
        }

        private int ValidateWeekendOrPublicHoliday(int day, DataContext context)
        {
            var dtStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
            var dtEnd = new DateTime(dtStart.Year, dtStart.Month, 20);
            var selectedDate = new DateTime(dtStart.Year, dtStart.Month, day); //next month

            var datesToExclude = context.PublicHolidaySet.Where(a => a.Date >= dtStart && a.Date <= dtEnd).Select(a => a.Date).ToList();
            while(dtStart < dtEnd)
            {
                if(dtStart.DayOfWeek == DayOfWeek.Saturday || dtStart.DayOfWeek == DayOfWeek.Sunday)
                {
                    datesToExclude.Add(dtStart);
                }

                dtStart = dtStart.AddDays(1);
            }

            //ok find out if my selected date is in this list then adjust either one back or one forward.
            if(datesToExclude.Contains(selectedDate))
            {
                DateTime startSearch = selectedDate;
                while(selectedDate.Day > 1 && datesToExclude.Contains(selectedDate))
                    selectedDate = selectedDate.AddDays(-1);

                if (datesToExclude.Contains(selectedDate))
                {
                    selectedDate = startSearch; //search forward

                    while (selectedDate.Day < 20 && datesToExclude.Contains(selectedDate))
                        selectedDate = selectedDate.AddDays(1);
                }

                if (datesToExclude.Contains(selectedDate)) //could not find one return the random day
                    selectedDate = startSearch;
            }
            return selectedDate.Day;
        }

        private void chkElecUnder_CheckedChanged(object sender, EventArgs e)
        {
            //chkElecInline, chkElecOver, chkElecUnder, chkWaterInline, chkWaterOver, chkWaterUnder
            if ((sender as CheckBox).Checked)
            {
                switch ((sender as CheckBox).Name)
                {
                    case "chkElecInline":
                        chkElecOver.Checked = false;
                        chkElecUnder.Checked = false;
                        break;

                    case "chkElecOver":
                        chkElecInline.Checked = false;
                        chkElecUnder.Checked = false;
                        break;

                    case "chkElecUnder":
                        chkElecInline.Checked = false;
                        chkElecOver.Checked = false;
                        break;

                    case "chkWaterInline":
                        chkWaterOver.Checked = false;
                        chkWaterUnder.Checked = false;
                        break;

                    case "chkWaterOver":
                        chkWaterInline.Checked = false;
                        chkWaterUnder.Checked = false;
                        break;

                    case "chkWaterUnder":
                        chkWaterInline.Checked = false;
                        chkWaterOver.Checked = false;
                        break;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }

        private void btnEmailPM_Click(object sender, EventArgs e)
        {
            //byte[] attachment = CreateExcel(true);

            //String message = "Queries regarding financials for " + cmbBuilding.Text + " for period " + cmbFinPeriod.SelectedItem.ToString() + "/" + cmbYear.SelectedItem.ToString() + Environment.NewLine + Environment.NewLine;
            //if (chkEPLevies.Checked || chkEPDomInc.Checked || chkEPElecInc.Checked || chkEPWaterInc.Checked || chkEPSpecialInc.Checked || chkEPOtherInc.Checked || chkEPIntInc.Checked || chkEPBankInc.Checked)
            //{
            //    message += "Income Statement - Income" + Environment.NewLine;
            //    if (chkEPLevies.Checked) { message += "Levies: provision - " + (chkLeviesIncY.Checked ? "Y" : (chkLeviesIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtLevies.Text + Environment.NewLine; }
            //    if (chkEPDomInc.Checked) { message += "Domestic Effluent Recovery: provision - " + (this.chkDomExpY.Checked ? "Y" : (chkDomExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtDomesticIncome.Text + Environment.NewLine; }
            //    if (chkEPElecInc.Checked) { message += "Electricity Recovery: provision - " + (chkElecIncY.Checked ? "Y" : (chkElecIncN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtElectricityIncome.Text + Environment.NewLine; }
            //    if (chkEPWaterInc.Checked) { message += "Water Recovery: provision - " + (chkWaterIncY.Checked ? "Y" : (chkWaterIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtWaterIncome.Text + Environment.NewLine; }
            //    if (chkEPSpecialInc.Checked) { message += "Special Levies: provision - " + (this.chkSpecialIncY.Checked ? "Y" : (chkSpecialIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtSpecialIncome.Text + Environment.NewLine; }
            //    if (chkEPOtherInc.Checked) { message += "Any other income accounts to be noted: provision - " + (chkOtherIncY.Checked ? "Y" : (chkOtherIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtOtherIncome.Text + Environment.NewLine; }
            //    if (chkEPIntInc.Checked) { message += "Interest from members: provision - " + (chkIntIncY.Checked ? "Y" : (chkIntIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtInterestIncome.Text + Environment.NewLine; }
            //    if (chkEPBankInc.Checked) { message += "Interest from bank investments: provision - " + (chkBankIncY.Checked ? "Y" : (chkBankIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtBankIncome.Text + Environment.NewLine; }
            //}
            //if (chkEPWater.Checked || chkEPTel.Checked || chkEPSec.Checked || chkEPSal.Checked || chkEPRefuse.Checked || chkEPRepairs.Checked || chkEPPost.Checked || chkEPPrint.Checked ||
            //    chkEPMeter.Checked || chkEPMan.Checked || chkEPIntExp.Checked || chkEPIns.Checked || chkEPGardens.Checked || chkEPElec.Checked || chkEPDel.Checked || chkEPDomExp.Checked ||
            //    chkEPBank.Checked || chkEPAcc.Checked || chkEPCSOSLevies.Checked)
            //{
            //    message += Environment.NewLine + Environment.NewLine + "Income Statement - Expenses";
            //    if (chkEPWater.Checked) { message += "Water expense: provision - " + (chkWaterExpY.Checked ? "Y" : (chkWaterExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtWater.Text + Environment.NewLine; }
            //    if (chkEPCSOSLevies.Checked) { message += "CSOS levies: provision - " + (chkCSOSLeviesExpY.Checked ? "Y" : (chkCSOSLeviesExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtCSOSLevies.Text + Environment.NewLine; }
            //    if (chkEPTel.Checked) { message += "Telephone expense: provision - " + (chkTelExpY.Checked ? "Y" : (chkTelExpY.Checked ? "N" : "N/A")) + "; Notes - " + txtTelephone.Text + Environment.NewLine; }
            //    if (chkEPSec.Checked) { message += "Security expense: provision - " + (chkSecExpY.Checked ? "Y" : (chkSecExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtSecurity.Text + Environment.NewLine; }
            //    if (chkEPSal.Checked) { message += "Salaries & Wages expense: provision - " + (chkSalExpY.Checked ? "Y" : (chkSalExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtSalaries.Text + Environment.NewLine; }
            //    if (chkEPRefuse.Checked) { message += "Refuse removal expense: provision - " + (this.chkRefuseExpY.Checked ? "Y" : (chkRefuseExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtRefuse.Text + Environment.NewLine; }
            //    if (chkEPRepairs.Checked) { message += "Repairs & Maintenance expense: provision - " + (chkRepairsExpY.Checked ? "Y" : (chkRepairsExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtRepairs.Text + Environment.NewLine; }
            //    if (chkEPPost.Checked) { message += "Post & petties expense: provision - " + (chkPostExpY.Checked ? "Y" : (chkPostExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtPost.Text + Environment.NewLine; }
            //    if (chkEPPrint.Checked) { message += "Printing & stationery expense: provision - " + (chkPrintExpY.Checked ? "Y" : (chkPrintExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtPrinting.Text + Environment.NewLine; }
            //    if (chkEPMeter.Checked) { message += "Meter reading expense: provision - " + (chkMeterExpY.Checked ? "Y" : (chkMeterExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtMeter.Text + Environment.NewLine; }
            //    if (chkEPMan.Checked) { message += "Management fees expense: provision - " + (chkManExpY.Checked ? "Y" : (chkManExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtManagementFees.Text + Environment.NewLine; }
            //    if (chkEPIntExp.Checked) { message += "Interest paid expense: provision - " + (chkIntExpY.Checked ? "Y" : (chkIntExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtInterest.Text + Environment.NewLine; }
            //    if (chkEPIns.Checked) { message += "Insurance expense: provision - " + (chkInsExpY.Checked ? "Y" : (chkInsExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtInsurance.Text + Environment.NewLine; }
            //    if (chkEPGardens.Checked) { message += "Gardens expense: provision - " + (chkGardensExpY.Checked ? "Y" : (chkGardensExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtGardens.Text + Environment.NewLine; }
            //    if (chkEPElec.Checked) { message += "Electricity expense: provision - " + (chkElecExpY.Checked ? "Y" : (chkElecExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtElectricity.Text + Environment.NewLine; }
            //    if (chkEPDel.Checked) { message += "Deliveries expense: provision - " + (chkDelExpY.Checked ? "Y" : (chkDelExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtDeliveries.Text + Environment.NewLine; }
            //    if (chkEPDomExp.Checked) { message += "Domestic effluent expense: provision - " + (chkDomExpY.Checked ? "Y" : (chkDomExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtDomExpense.Text + Environment.NewLine; }
            //    if (chkEPBank.Checked) { message += "Bank charges: provision - " + (chkBankExpY.Checked ? "Y" : (chkBankExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtBankCharges.Text + Environment.NewLine; }
            //    if (chkEPAcc.Checked) { message += "Accounting fees: provision - " + (chkAccExpY.Checked ? "Y" : (chkAccExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtAccountFees.Text + Environment.NewLine; }
            //}
            //if (chkEPMunDep.Checked || chkEPTrust.Checked || chkEPOwn.Checked || chkEPInv.Checked || chkEPSunAss.Checked || chkEPAss.Checked)
            //{
            //    message += Environment.NewLine + Environment.NewLine + "Balance Sheet - Assets" + Environment.NewLine;
            //    if (chkEPMunDep.Checked) { message += "Municipal deposits: provision - " + (this.chkMunAssetY.Checked ? "Y" : (chkMunAssetN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtMunDep.Text + Environment.NewLine; }
            //    if (chkEPTrust.Checked) { message += "Trust Account: provision - " + (this.chkTrustAssY.Checked ? "Y" : (chkTrustAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtTrust.Text + Environment.NewLine; }
            //    if (chkEPOwn.Checked) { message += "Own Account: provision - " + (this.chkOwnAccAssY.Checked ? "Y" : (chkOwnAccAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtOwnAccount.Text + Environment.NewLine; }
            //    if (chkEPInv.Checked) { message += "Investment Account: provision - " + (this.chkInvestAssY.Checked ? "Y" : (chkInvestAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtInvestment.Text + Environment.NewLine; }
            //    if (chkEPSunAss.Checked) { message += "Sundry Assets: provision - " + (this.chkSunAssY.Checked ? "Y" : (chkSunAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtAccruals.Text + Environment.NewLine; }
            //    if (chkEPAss.Checked) { message += "Other Assets: provision - " + (this.chkOtherAssY.Checked ? "Y" : (chkOtherAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtAssets.Text + Environment.NewLine; }
            //}
            //if (chkEPDebtors.Checked || chkEPMunAcc.Checked || chkEPOwnDep.Checked || chkEPSunLia.Checked || chkEPLia.Checked)
            //{
            //    message += Environment.NewLine + Environment.NewLine + "Balance Sheet - Liabilities" + Environment.NewLine;
            //    if (chkEPDebtors.Checked) { message += "Debtors: provision - " + (this.chkDebtLiaY.Checked ? "Y" : (chkDebtLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtDebtors.Text + Environment.NewLine; }
            //    if (chkEPMunAcc.Checked) { message += "Municipal Accounts: provision - " + (this.chkMunLiaY.Checked ? "Y" : (chkMunLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtMunAcc.Text + Environment.NewLine; }
            //    if (chkEPOwnDep.Checked) { message += "Owners Deposits: provision - " + (this.chkOwnLiaY.Checked ? "Y" : (chkOwnLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtOwnerDep.Text + Environment.NewLine; }
            //    if (chkEPSunLia.Checked) { message += "Sundry suppliers: provision - " + (this.chkSunLiaY.Checked ? "Y" : (chkSunLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtSuppliers.Text + Environment.NewLine; }
            //    if (chkEPLia.Checked) { message += "Other liabilities: provision - " + (this.chkOtherLiaY.Checked ? "Y" : (chkOtherLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtLiabilities.Text + Environment.NewLine; }
            //}
            //message += Environment.NewLine + Environment.NewLine + "Recoveries" + Environment.NewLine;
            //message += "Water:" + (chkWaterInline.Checked ? "Inline" : (chkWaterOver.Checked ? "Over recovery" : "Under recovery")) + Environment.NewLine;
            //message += "Electricity:" + Environment.NewLine;
            //message += Environment.NewLine + Environment.NewLine + "Comments" + Environment.NewLine;
            //message += txtComments.Text;
            //Building building = buildings[cmbBuilding.SelectedIndex];
            //String pmEmail = building.PM;

            //var attachmentList = new List<Tuple<string, byte[]>>();
            //attachmentList.Add(new Tuple<string, byte[]>("FinancialReport.pdf", attachment));
            //string subject = building.Name + " " + cmbFinPeriod.SelectedItem.ToString() + "/" + cmbYear.SelectedItem.ToString();

            //string cced = "";
            //using (var context = SqlDataHandler.GetDataContext())
            //{
            //    var q = from u in context.tblUsers
            //            where u.ProcessCheckLists && u.Active
            //            select u.email;

            //    foreach(var email in q.ToList())
            //    {
            //        if(!String.IsNullOrWhiteSpace(email) && email != pmEmail)
            //        {
            //            if (!String.IsNullOrWhiteSpace(cced))
            //                cced = cced + ";" + email;
            //            else
            //                cced = email;
            //        }
            //    }
            //}

            //OutlookEmail.SendEmail(pmEmail, cced,"", "Financial Report Query - " + subject, message, attachmentList);

        }

        private void btnEmailDebtor_Click(object sender, EventArgs e)
        {
            //byte[] attachment = CreateExcel(true);

            //String message = "Queries regarding financials for " + cmbBuilding.Text + " for period " + cmbFinPeriod.SelectedItem.ToString() + "/" + cmbYear.SelectedItem.ToString() + Environment.NewLine + Environment.NewLine;
            //if (chkEDLevies.Checked || chkEDDomInc.Checked || chkEDElecInc.Checked || chkEDWaterInc.Checked || chkEDSpecialInc.Checked || chkEDOtherInc.Checked || chkEDIntInc.Checked 
            //    || chkEDBankInc.Checked)
            //{
            //    message += "Income Statement - Income" + Environment.NewLine;
            //    if (chkEDLevies.Checked) { message += "Levies: provision - " + (chkLeviesIncY.Checked ? "Y" : (chkLeviesIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtLevies.Text + Environment.NewLine; }
            //    if (chkEDDomInc.Checked) { message += "Domestic Effluent Recovery: provision - " + (this.chkDomExpY.Checked ? "Y" : (chkDomExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtDomesticIncome.Text + Environment.NewLine; }
            //    if (chkEDElecInc.Checked) { message += "Electricity Recovery: provision - " + (chkElecIncY.Checked ? "Y" : (chkElecIncN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtElectricityIncome.Text + Environment.NewLine; }
            //    if (chkEDWaterInc.Checked) { message += "Water Recovery: provision - " + (chkWaterIncY.Checked ? "Y" : (chkWaterIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtWaterIncome.Text + Environment.NewLine; }
            //    if (chkEDSpecialInc.Checked) { message += "Special Levies: provision - " + (this.chkSpecialIncY.Checked ? "Y" : (chkSpecialIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtSpecialIncome.Text + Environment.NewLine; }
            //    if (chkEDOtherInc.Checked) { message += "Any other income accounts to be noted: provision - " + (chkOtherIncY.Checked ? "Y" : (chkOtherIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtOtherIncome.Text + Environment.NewLine; }
            //    if (chkEDIntInc.Checked) { message += "Interest from members: provision - " + (chkIntIncY.Checked ? "Y" : (chkIntIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtInterestIncome.Text + Environment.NewLine; }
            //    if (chkEDBankInc.Checked) { message += "Interest from bank investments: provision - " + (chkBankIncY.Checked ? "Y" : (chkBankIncN.Checked ? "N" : "N/A")) + "; Notes - " + txtBankIncome.Text + Environment.NewLine; }
            //}
            //if (chkEDWater.Checked || chkEDTel.Checked || chkEDSec.Checked || chkEDSal.Checked || chkEDRefuse.Checked || chkEDRepairs.Checked || chkEDPost.Checked || chkEDPrint.Checked ||
            //    chkEDMeter.Checked || chkEDMan.Checked || chkEDIntExp.Checked || chkEDIns.Checked || chkEDGardens.Checked || chkEDElec.Checked || chkEDDel.Checked || chkEDDomExp.Checked ||
            //    chkEDBank.Checked || chkEDAcc.Checked || chkEDCSOSLevies.Checked 
            //    )
            //{
            //    message += Environment.NewLine + Environment.NewLine + "Income Statement - Expenses";
            //    if (chkEDWater.Checked) { message += "Water expense: provision - " + (chkWaterExpY.Checked ? "Y" : (chkWaterExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtWater.Text + Environment.NewLine; }
            //    if (chkEDCSOSLevies.Checked) { message += "CSOS Levies expense: provision - " + (chkCSOSLeviesExpY.Checked ? "Y" : (chkCSOSLeviesExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtCSOSLevies.Text + Environment.NewLine; }
            //    if (chkEDTel.Checked) { message += "Telephone expense: provision - " + (chkTelExpY.Checked ? "Y" : (chkTelExpY.Checked ? "N" : "N/A")) + "; Notes - " + txtTelephone.Text + Environment.NewLine; }
            //    if (chkEDSec.Checked) { message += "Security expense: provision - " + (chkSecExpY.Checked ? "Y" : (chkSecExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtSecurity.Text + Environment.NewLine; }
            //    if (chkEDSal.Checked) { message += "Salaries & Wages expense: provision - " + (chkSalExpY.Checked ? "Y" : (chkSalExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtSalaries.Text + Environment.NewLine; }
            //    if (chkEDRefuse.Checked) { message += "Refuse removal expense: provision - " + (this.chkRefuseExpY.Checked ? "Y" : (chkRefuseExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtRefuse.Text + Environment.NewLine; }
            //    if (chkEDRepairs.Checked) { message += "Repairs & Maintenance expense: provision - " + (chkRepairsExpY.Checked ? "Y" : (chkRepairsExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtRepairs.Text + Environment.NewLine; }
            //    if (chkEDPost.Checked) { message += "Post & petties expense: provision - " + (chkPostExpY.Checked ? "Y" : (chkPostExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtPost.Text + Environment.NewLine; }
            //    if (chkEDPrint.Checked) { message += "Printing & stationery expense: provision - " + (chkPrintExpY.Checked ? "Y" : (chkPrintExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtPrinting.Text + Environment.NewLine; }
            //    if (chkEDMeter.Checked) { message += "Meter reading expense: provision - " + (chkMeterExpY.Checked ? "Y" : (chkMeterExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtMeter.Text + Environment.NewLine; }
            //    if (chkEDMan.Checked) { message += "Management fees expense: provision - " + (chkManExpY.Checked ? "Y" : (chkManExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtManagementFees.Text + Environment.NewLine; }
            //    if (chkEDIntExp.Checked) { message += "Interest paid expense: provision - " + (chkIntExpY.Checked ? "Y" : (chkIntExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtInterest.Text + Environment.NewLine; }
            //    if (chkEDIns.Checked) { message += "Insurance expense: provision - " + (chkInsExpY.Checked ? "Y" : (chkInsExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtInsurance.Text + Environment.NewLine; }
            //    if (chkEDGardens.Checked) { message += "Gardens expense: provision - " + (chkGardensExpY.Checked ? "Y" : (chkGardensExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtGardens.Text + Environment.NewLine; }
            //    if (chkEDElec.Checked) { message += "Electricity expense: provision - " + (chkElecExpY.Checked ? "Y" : (chkElecExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtElectricity.Text + Environment.NewLine; }
            //    if (chkEDDel.Checked) { message += "Deliveries expense: provision - " + (chkDelExpY.Checked ? "Y" : (chkDelExpN.Checked ? "N" : "N/A")) + "; Notes - " + txtDeliveries.Text + Environment.NewLine; }
            //    if (chkEDDomExp.Checked) { message += "Domestic effluent expense: provision - " + (chkDomExpY.Checked ? "Y" : (chkDomExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtDomExpense.Text + Environment.NewLine; }
            //    if (chkEDBank.Checked) { message += "Bank charges: provision - " + (chkBankExpY.Checked ? "Y" : (chkBankExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtBankCharges.Text + Environment.NewLine; }
            //    if (chkEDAcc.Checked) { message += "Accounting fees: provision - " + (chkAccExpY.Checked ? "Y" : (chkAccExpN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtAccountFees.Text + Environment.NewLine; }
            //}
            //if (chkEDMunDep.Checked || chkEDTrust.Checked || chkEDOwn.Checked || chkEDInv.Checked || chkEDSunAss.Checked || chkEDAss.Checked)
            //{
            //    message += Environment.NewLine + Environment.NewLine + "Balance Sheet - Assets" + Environment.NewLine;
            //    if (chkEDMunDep.Checked) { message += "Municipal deposits: provision - " + (this.chkMunAssetY.Checked ? "Y" : (chkMunAssetN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtMunDep.Text + Environment.NewLine; }
            //    if (chkEDTrust.Checked) { message += "Trust Account: provision - " + (this.chkTrustAssY.Checked ? "Y" : (chkTrustAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtTrust.Text + Environment.NewLine; }
            //    if (chkEDOwn.Checked) { message += "Own Account: provision - " + (this.chkOwnAccAssY.Checked ? "Y" : (chkOwnAccAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtOwnAccount.Text + Environment.NewLine; }
            //    if (chkEDInv.Checked) { message += "Investment Account: provision - " + (this.chkInvestAssY.Checked ? "Y" : (chkInvestAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtInvestment.Text + Environment.NewLine; }
            //    if (chkEDSunAss.Checked) { message += "Sundry Assets: provision - " + (this.chkSunAssY.Checked ? "Y" : (chkSunAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtAccruals.Text + Environment.NewLine; }
            //    if (chkEDAss.Checked) { message += "Other Assets: provision - " + (this.chkOtherAssY.Checked ? "Y" : (chkOtherAssN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtAssets.Text + Environment.NewLine; }
            //}
            //if (chkEDDebtors.Checked || chkEDMunAcc.Checked || chkEDOwnDep.Checked || chkEDSunLia.Checked || chkEDLia.Checked)
            //{
            //    message += Environment.NewLine + Environment.NewLine + "Balance Sheet - Liabilities" + Environment.NewLine;
            //    if (chkEDDebtors.Checked) { message += "Debtors: provision - " + (this.chkDebtLiaY.Checked ? "Y" : (chkDebtLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtDebtors.Text + Environment.NewLine; }
            //    if (chkEDMunAcc.Checked) { message += "Municipal Accounts: provision - " + (this.chkMunLiaY.Checked ? "Y" : (chkMunLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtMunAcc.Text + Environment.NewLine; }
            //    if (chkEDOwnDep.Checked) { message += "Owners Deposits: provision - " + (this.chkOwnLiaY.Checked ? "Y" : (chkOwnLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtOwnerDep.Text + Environment.NewLine; }
            //    if (chkEDSunLia.Checked) { message += "Sundry suppliers: provision - " + (this.chkSunLiaY.Checked ? "Y" : (chkSunLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtSuppliers.Text + Environment.NewLine; }
            //    if (chkEDLia.Checked) { message += "Other liabilities: provision - " + (this.chkOtherLiaY.Checked ? "Y" : (chkOtherLiaN.Checked ? "N" : "N/A")) + "; Notes - " + this.txtLiabilities.Text + Environment.NewLine; }
            //}
            //message += Environment.NewLine + Environment.NewLine + "Recoveries" + Environment.NewLine;
            //message += "Water:" + (chkWaterInline.Checked ? "Inline" : (chkWaterOver.Checked ? "Over recovery" : "Under recovery")) + Environment.NewLine;
            //message += "Electricity:" + Environment.NewLine;
            //message += Environment.NewLine + Environment.NewLine + "Comments" + Environment.NewLine;
            //message += txtComments.Text;

            //Building building = buildings[cmbBuilding.SelectedIndex];
            //String pmEmail = building.Debtor;
            //String status;
            //if (Mailer.SendMail(Controller.user.email, new string[] { pmEmail }, "Financial Report Query", message, false, true, false, out status, new String[0]))
            //{
            //    MessageBox.Show("Message sent");
            //}
            //else
            //{
            //    MessageBox.Show("Message not sent: " + status);
            //}
        }

        private byte[] CreateExcel(bool createPDF = false)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Excel.Application xlApp = new Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("EXCEL could not be started. Check that your office installation and project references are correct.");
                    return null;
                }

                xlApp.Visible = true;

                Excel.Workbook wb = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

               

                if (ws == null)
                {
                    MessageBox.Show("Worksheet could not be created. Check that your office installation and project references are correct.");
                    return null;
                }
                ws.Name = "Monthly Financial Checklist";
                ws.Cells[1, "A"].Value2 = "Astrodon (Pty) Ltd";
                ws.Cells[1, "A"].Font.Bold = true;
                ws.get_Range("A1", "E1").Merge(Type.Missing);
                ws.Cells[2, "A"].Value2 = "Monthly financials checklist / verification forms - Balance sheet / Income statement";
                ws.get_Range("A2", "E2").Merge(Type.Missing);
                ws.Cells[3, "A"].Value2 = "Building Name: " + cmbBuilding.Text;
                ws.Cells[3, "D"].Value2 = "Financial Period: " + cmbYear.SelectedItem.ToString() + "/" + cmbFinPeriod.SelectedItem.ToString();
                ws.get_Range("A3", "C3").Merge(Type.Missing);
                ws.get_Range("D3", "E3").Merge(Type.Missing);
                ws.Cells[4, "A"].Value2 = "Verification of the following items:";
                ws.get_Range("A4", "E4").Merge(Type.Missing);

                #region Income

                ws.Cells[5, "B"].Value2 = "Income Statement - Income";
                ws.Cells[5, "C"].Value2 = "Yes";
                ws.Cells[5, "D"].Value2 = "No";
                ws.Cells[5, "E"].Value2 = "Reason";
                ws.Cells[5, "A"].EntireRow.Font.Bold = true;

                ws.Cells[6, "A"].Value2 = "1";
                ws.Cells[6, "B"].Value2 = "Levies Received";
                ws.Cells[6, "C"].Value2 = chkLeviesIncY.Checked ? "X" : "";
                ws.Cells[6, "D"].Value2 = chkLeviesIncN.Checked ? "X" : "";
                ws.Cells[6, "E"].Value2 = txtLevies.Text;
                ws.Cells[7, "A"].Value2 = "2";
                ws.Cells[7, "B"].Value2 = "Domestic Effluent Recoveries";
                ws.Cells[7, "C"].Value2 = this.chkDomIncY.Checked ? "X" : "";
                ws.Cells[7, "D"].Value2 = chkDomIncN.Checked ? "X" : "";
                ws.Cells[7, "E"].Value2 = this.txtDomesticIncome.Text;
                ws.Cells[8, "A"].Value2 = "3";
                ws.Cells[8, "B"].Value2 = "Electricity Recoveries";
                ws.Cells[8, "C"].Value2 = this.chkElecIncY.Checked ? "X" : "";
                ws.Cells[8, "D"].Value2 = chkElecIncN.Checked ? "X" : "";
                ws.Cells[8, "E"].Value2 = this.txtElectricityIncome.Text;
                ws.Cells[9, "A"].Value2 = "4";
                ws.Cells[9, "B"].Value2 = "Water Recoveries";
                ws.Cells[9, "C"].Value2 = chkWaterIncY.Checked ? "X" : "";
                ws.Cells[9, "D"].Value2 = chkWaterIncN.Checked ? "X" : "";
                ws.Cells[9, "E"].Value2 = this.txtWaterIncome.Text;
                ws.Cells[10, "A"].Value2 = "5";
                ws.Cells[10, "B"].Value2 = "Special Levies";
                ws.Cells[10, "C"].Value2 = chkSpecialIncY.Checked ? "X" : "";
                ws.Cells[10, "D"].Value2 = chkSpecialIncN.Checked ? "X" : "";
                ws.Cells[10, "E"].Value2 = this.txtSpecialIncome.Text;
                ws.Cells[11, "A"].Value2 = "6";
                ws.Cells[11, "B"].Value2 = "Any other income accounts to be noted";
                ws.Cells[11, "C"].Value2 = chkOtherIncY.Checked ? "X" : "";
                ws.Cells[11, "D"].Value2 = chkOtherIncN.Checked ? "X" : "";
                ws.Cells[11, "E"].Value2 = this.txtOtherIncome.Text;
                ws.Cells[12, "A"].Value2 = "7";
                ws.Cells[12, "B"].Value2 = "Interest from Members";
                ws.Cells[12, "C"].Value2 = this.chkIntIncY.Checked ? "X" : "";
                ws.Cells[12, "D"].Value2 = chkIntIncN.Checked ? "X" : "";
                ws.Cells[12, "E"].Value2 = this.txtInterestIncome.Text;
                ws.Cells[13, "A"].Value2 = "8";
                ws.Cells[13, "B"].Value2 = "Interest from Bank Investments";
                ws.Cells[13, "C"].Value2 = this.chkBankIncY.Checked ? "X" : "";
                ws.Cells[13, "D"].Value2 = chkBankIncN.Checked ? "X" : "";
                ws.Cells[13, "E"].Value2 = this.txtBankIncome.Text;
                Excel.Range rinc = ws.get_Range("A6", "E13");
                rinc.Borders.Color = System.Drawing.Color.Black.ToArgb();

                #endregion Income

                #region Expenses

                ws.Cells[14, "B"].Value2 = "Income Statement - Expenses";
                ws.Cells[14, "C"].Value2 = "Yes";
                ws.Cells[14, "D"].Value2 = "No";
                ws.Cells[14, "E"].Value2 = "Reason";
                ws.Cells[14, "A"].EntireRow.Font.Bold = true;

                ws.Cells[15, "A"].Value2 = "1";
                ws.Cells[15, "B"].Value2 = "Accounting Fees";
                ws.Cells[15, "C"].Value2 = this.chkAccExpY.Checked ? "X" : "";
                ws.Cells[15, "D"].Value2 = chkAccExpY.Checked ? "X" : "";
                ws.Cells[15, "E"].Value2 = this.txtAccountFees.Text;
                ws.Cells[16, "A"].Value2 = "2";
                ws.Cells[16, "B"].Value2 = "Bank Charges";
                ws.Cells[16, "C"].Value2 = this.chkBankExpY.Checked ? "X" : "";
                ws.Cells[16, "D"].Value2 = chkBankExpN.Checked ? "X" : "";
                ws.Cells[16, "E"].Value2 = this.txtBankCharges.Text;
                ws.Cells[17, "A"].Value2 = "3";
                ws.Cells[17, "B"].Value2 = "Domestic Effluent";
                ws.Cells[17, "C"].Value2 = this.chkDomExpY.Checked ? "X" : "";
                ws.Cells[17, "D"].Value2 = chkDomExpN.Checked ? "X" : "";
                ws.Cells[17, "E"].Value2 = this.txtDomExpense.Text;
                ws.Cells[18, "A"].Value2 = "4";
                ws.Cells[18, "B"].Value2 = "Deliveries";
                ws.Cells[18, "C"].Value2 = chkDelExpY.Checked ? "X" : "";
                ws.Cells[18, "D"].Value2 = chkDelExpN.Checked ? "X" : "";
                ws.Cells[18, "E"].Value2 = this.txtDeliveries.Text;
                ws.Cells[19, "A"].Value2 = "5";
                ws.Cells[19, "B"].Value2 = "Electricity";
                ws.Cells[19, "C"].Value2 = this.chkElecExpY.Checked ? "X" : "";
                ws.Cells[19, "D"].Value2 = chkElecExpN.Checked ? "X" : "";
                ws.Cells[19, "E"].Value2 = this.txtElectricity.Text;
                ws.Cells[20, "A"].Value2 = "6";
                ws.Cells[20, "B"].Value2 = "Gardens";
                ws.Cells[20, "C"].Value2 = this.chkGardensExpY.Checked ? "X" : "";
                ws.Cells[20, "D"].Value2 = chkGardensExpN.Checked ? "X" : "";
                ws.Cells[20, "E"].Value2 = this.txtGardens.Text;
                ws.Cells[21, "A"].Value2 = "7";
                ws.Cells[21, "B"].Value2 = "Insurance";
                ws.Cells[21, "C"].Value2 = this.chkInsExpY.Checked ? "X" : "";
                ws.Cells[21, "D"].Value2 = chkInsExpN.Checked ? "X" : "";
                ws.Cells[21, "E"].Value2 = this.txtInsurance.Text;
                ws.Cells[22, "A"].Value2 = "8";
                ws.Cells[22, "B"].Value2 = "Interest paid (Why & reason)";
                ws.Cells[22, "C"].Value2 = this.chkIntExpY.Checked ? "X" : "";
                ws.Cells[22, "D"].Value2 = chkIntExpN.Checked ? "X" : "";
                ws.Cells[22, "E"].Value2 = this.txtInterest.Text;
                ws.Cells[23, "A"].Value2 = "9";
                ws.Cells[23, "B"].Value2 = "Management Fees";
                ws.Cells[23, "C"].Value2 = chkManExpY.Checked ? "X" : "";
                ws.Cells[23, "D"].Value2 = chkManExpN.Checked ? "X" : "";
                ws.Cells[23, "E"].Value2 = this.txtManagementFees.Text;
                ws.Cells[24, "A"].Value2 = "10";
                ws.Cells[24, "B"].Value2 = "Meter reading fees";
                ws.Cells[24, "C"].Value2 = this.chkMeterExpY.Checked ? "X" : "";
                ws.Cells[24, "D"].Value2 = chkMeterExpN.Checked ? "X" : "";
                ws.Cells[24, "E"].Value2 = this.txtMeter.Text;
                ws.Cells[25, "A"].Value2 = "11";
                ws.Cells[25, "B"].Value2 = "Printing & stationery";
                ws.Cells[25, "C"].Value2 = this.chkPrintExpY.Checked ? "X" : "";
                ws.Cells[25, "D"].Value2 = chkPrintExpN.Checked ? "X" : "";
                ws.Cells[25, "E"].Value2 = this.txtPrinting.Text;
                ws.Cells[26, "A"].Value2 = "12";
                ws.Cells[26, "B"].Value2 = "Post & Petties";
                ws.Cells[26, "C"].Value2 = chkPostExpY.Checked ? "X" : "";
                ws.Cells[26, "D"].Value2 = chkPostExpN.Checked ? "X" : "";
                ws.Cells[26, "E"].Value2 = this.txtPost.Text;
                ws.Cells[27, "A"].Value2 = "13";
                ws.Cells[27, "B"].Value2 = "Repairs & Maintenance";
                ws.Cells[27, "C"].Value2 = this.chkRepairsExpY.Checked ? "X" : "";
                ws.Cells[27, "D"].Value2 = chkRepairsExpN.Checked ? "X" : "";
                ws.Cells[27, "E"].Value2 = this.txtRepairs.Text;
                ws.Cells[28, "A"].Value2 = "14";
                ws.Cells[28, "B"].Value2 = "Refuse Removal";
                ws.Cells[28, "C"].Value2 = chkRefuseExpY.Checked ? "X" : "";
                ws.Cells[28, "D"].Value2 = chkRefuseExpN.Checked ? "X" : "";
                ws.Cells[28, "E"].Value2 = this.txtRefuse.Text;
                ws.Cells[29, "A"].Value2 = "15";
                ws.Cells[29, "B"].Value2 = "Salaries & Wages";
                ws.Cells[29, "C"].Value2 = this.chkSalExpY.Checked ? "X" : "";
                ws.Cells[29, "D"].Value2 = chkSalExpN.Checked ? "X" : "";
                ws.Cells[29, "E"].Value2 = this.txtSalaries.Text;
                ws.Cells[30, "A"].Value2 = "16";
                ws.Cells[30, "B"].Value2 = "Security";
                ws.Cells[30, "C"].Value2 = this.chkSecExpY.Checked ? "X" : "";
                ws.Cells[30, "D"].Value2 = chkSecExpN.Checked ? "X" : "";
                ws.Cells[30, "E"].Value2 = this.txtSecurity.Text;
                ws.Cells[31, "A"].Value2 = "17";
                ws.Cells[31, "B"].Value2 = "Telephone";
                ws.Cells[31, "C"].Value2 = this.chkTelExpY.Checked ? "X" : "";
                ws.Cells[31, "D"].Value2 = chkTelExpN.Checked ? "X" : "";
                ws.Cells[31, "E"].Value2 = this.txtTelephone.Text;
                ws.Cells[32, "A"].Value2 = "18";
                ws.Cells[32, "B"].Value2 = "Water";
                ws.Cells[32, "C"].Value2 = this.chkWaterExpY.Checked ? "X" : "";
                ws.Cells[32, "D"].Value2 = chkWaterExpN.Checked ? "X" : "";
                ws.Cells[32, "E"].Value2 = this.txtWater.Text;

                ws.Cells[33, "A"].Value2 = "19";
                ws.Cells[33, "B"].Value2 = "CSOS Levies";
                ws.Cells[33, "C"].Value2 = this.chkCSOSLeviesExpY.Checked ? "X" : "";
                ws.Cells[33, "D"].Value2 = chkCSOSLeviesExpN.Checked ? "X" : "";
                ws.Cells[33, "E"].Value2 = this.txtCSOSLevies.Text;

                Excel.Range rexp = ws.get_Range("A15", "E33");
                rexp.Borders.Color = System.Drawing.Color.Black.ToArgb();

                #endregion Expenses

                #region Assets
                int row = 34;
                ws.Cells[row, "B"].Value2 = "Balance Sheet - Assets";
                ws.Cells[row, "C"].Value2 = "Yes";
                ws.Cells[row, "D"].Value2 = "No";
                ws.Cells[row, "E"].Value2 = "Reason";
                ws.Cells[row, "A"].EntireRow.Font.Bold = true;
                row++;
                ws.Cells[row, "A"].Value2 = "1";
                ws.Cells[row, "B"].Value2 = "Municipal deposit verified against council account";
                ws.Cells[row, "C"].Value2 = this.chkMunAssetY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkMunAssetN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtMunDep.Text;
                row++;
                ws.Cells[row, "A"].Value2 = "2";
                ws.Cells[row, "B"].Value2 = "Trust account balance verified";
                ws.Cells[row, "C"].Value2 = this.chkTrustAssY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkTrustAssN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtTrust.Text;
                row++;
                ws.Cells[row, "A"].Value2 = "3";
                ws.Cells[row, "B"].Value2 = "Own account balance verified against statement";
                ws.Cells[row, "C"].Value2 = this.chkOwnAccAssY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkOwnAccAssN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtOwnAccount.Text;
                row++;

                ws.Cells[row, "A"].Value2 = "4";
                ws.Cells[row, "B"].Value2 = "Investment account balance verified against statement";
                ws.Cells[row, "C"].Value2 = this.chkInvestAssY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkInvestAssN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtInvestment.Text;
                row++;
                ws.Cells[row, "A"].Value2 = "5";
                ws.Cells[row, "B"].Value2 = "Sundry accruals verified";
                ws.Cells[row, "C"].Value2 = this.chkSunAssY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkSunAssN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtAccruals.Text;
                row++;
                ws.Cells[row, "A"].Value2 = "6";
                ws.Cells[row, "B"].Value2 = "Any other assets to be noted";
                ws.Cells[row, "C"].Value2 = this.chkOtherAssY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkOtherAssN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtAssets.Text;
                Excel.Range rass = ws.get_Range("A34", "E"+row.ToString());
                rass.Borders.Color = System.Drawing.Color.Black.ToArgb();

                row++;

                #endregion Assets

                #region Liabilities

                ws.Cells[row, "B"].Value2 = "Balance Sheet - Liabilities";
                ws.Cells[row, "C"].Value2 = "Yes";
                ws.Cells[row, "D"].Value2 = "No";
                ws.Cells[row, "E"].Value2 = "Reason";
                ws.Cells[row, "A"].EntireRow.Font.Bold = true;
                row++;
                ws.Cells[row, "A"].Value2 = "1";
                ws.Cells[row, "B"].Value2 = "Verification of debtors";
                ws.Cells[row, "C"].Value2 = this.chkDebtLiaY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkDebtLiaN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtDebtors.Text;
                row++;

                ws.Cells[row, "A"].Value2 = "2";
                ws.Cells[row, "B"].Value2 = "Verification of municipal accounts - all payments reflect";
                ws.Cells[row, "C"].Value2 = this.chkMunLiaY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkMunLiaN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtMunAcc.Text;
                row++;

                ws.Cells[row, "A"].Value2 = "3";
                ws.Cells[row, "B"].Value2 = "Verification of owners deposits - note any changes";
                ws.Cells[row, "C"].Value2 = this.chkOwnLiaY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkOwnLiaN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtOwnerDep.Text;
                row++;

                ws.Cells[row, "A"].Value2 = "4";
                ws.Cells[row, "B"].Value2 = "Sundry suppliers verified";
                ws.Cells[row, "C"].Value2 = this.chkSunLiaY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkSunLiaN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtSuppliers.Text;
                row++;

                ws.Cells[row, "A"].Value2 = "5";
                ws.Cells[row, "B"].Value2 = "Any other liabilities to be noted";
                ws.Cells[row, "C"].Value2 = this.chkOtherLiaY.Checked ? "X" : "";
                ws.Cells[row, "D"].Value2 = chkOtherLiaN.Checked ? "X" : "";
                ws.Cells[row, "E"].Value2 = this.txtLiabilities.Text;
                Excel.Range rlia = ws.get_Range("A41", "E" + row.ToString());
                rlia.Borders.Color = System.Drawing.Color.Black.ToArgb();
                row++;

                #endregion Liabilities

                #region Recoveries

                ws.Cells[row, "B"].Value2 = "Recoveries v Expense";
                ws.Cells[row, "A"].EntireRow.Font.Bold = true;
                row++;
                ws.Cells[row, "A"].Value2 = "1";
                ws.Cells[row, "B"].Value2 = "Electricity";
                if (chkElecUnder.Checked)
                {
                    ws.Cells[row, "C"].Value2 = "Under Recovery";
                }
                else if (chkElecOver.Checked)
                {
                    ws.Cells[row, "C"].Value2 = "Over Recovery";
                }
                else
                {
                    ws.Cells[row, "C"].Value2 = "In Line";
                }
                ws.get_Range("C"+row.ToString(), "E"+ row.ToString()).Merge(Type.Missing);
                row++;

                ws.Cells[row, "A"].Value2 = "2";
                ws.Cells[row, "B"].Value2 = "Water";
                if (chkWaterUnder.Checked)
                {
                    ws.Cells[row, "C"].Value2 = "Under Recovery";
                }
                else if (chkWaterOver.Checked)
                {
                    ws.Cells[row, "C"].Value2 = "Over Recovery";
                }
                else
                {
                    ws.Cells[row, "C"].Value2 = "In Line";
                }
                ws.get_Range("C"+ row.ToString(), "E"+row.ToString()).Merge(Type.Missing);
                Excel.Range rrec = ws.get_Range("A"+row.ToString(), "E"+row.ToString());
                rrec.Borders.Color = System.Drawing.Color.Black.ToArgb();
                row++;
                #endregion Recoveries

                if (!string.IsNullOrWhiteSpace(txtComments.Text))
                {
                    #region Comments
                    ws.Cells[row, "B"].Value2 = "COMMENTS";
                    ws.Cells[row, "A"].EntireRow.Font.Bold = true;
                    row++;

                    ws.get_Range("B"+row.ToString(), "E" + (row+2).ToString()).Merge(Type.Missing);
                    ws.get_Range("B" + row.ToString(), "E" + (row+2).ToString()).WrapText = true;

                    ws.Cells[50, "B"].Value2 = txtComments.Text;
                    #endregion
                }
                ws.Rows.AutoFit();
                ws.Columns.AutoFit();
                row = row + 2;

                ws.PageSetup.Zoom = false;
                ws.PageSetup.FitToPagesWide = 1;
                ws.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
                ws.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;


                if (createPDF)
                {
                    if (!Directory.Exists(@"C:\Temp"))
                        Directory.CreateDirectory(@"C:\Temp");

                    string tempFile = @"C:\Temp\" + Guid.NewGuid().ToString("N") + ".pdf";

                    if (File.Exists(tempFile))
                        File.Delete(tempFile);

                    //Download and install this for the below to work
                    //https://www.microsoft.com/en-za/download/details.aspx?id=7
                    wb.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, tempFile);

                    object missing = Type.Missing;

                    wb.Close(false, missing, missing);
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlApp);
                    GC.Collect();
                    byte[] result = File.ReadAllBytes(tempFile);

                     File.Delete(tempFile);

                    this.Cursor = Cursors.Default;

                    Application.DoEvents();

                    return result;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
                return null;
            }
            this.Cursor = Cursors.Default;
            return null;
        }
    }
}
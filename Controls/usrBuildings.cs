using Astro.Library.Entities;
using Astrodon.Data.InsuranceData;
using Astrodon.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using iTextSharp.text.pdf;

namespace Astrodon
{
    public partial class usrBuildings : UserControl
    {
        private Buildings BuildingManager;
        private Building selectedBuilding = null;
        private List<Astrodon.Data.BankData.Bank> _Banks;

        private List<IInsurancePqRecord> InsurancePqGrid { get; set; }

        public usrBuildings()
        {
            InitializeComponent();

            dtpEventToTime.Format = DateTimePickerFormat.Time;
            dtpEventToTime.ShowUpDown = true;

            dtpEventTime.Format = DateTimePickerFormat.Time;
            dtpEventTime.ShowUpDown = true;

        }

        private void usrBuildings_Load(object sender, EventArgs e)
        {
            LoadCombo();
            clearBuilding();
            LoadBanks();
        }

        private void LoadBanks()
        {
            //_Banks
            this.Cursor = Cursors.WaitCursor;

            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    _Banks = context.BankSet.Where(a => a.IsActive).ToList();
                    cmbBondHolder.DataSource = _Banks;
                    cmbBondHolder.ValueMember = "Id";
                    cmbBondHolder.DisplayMember = "Name";
                    cmbBondHolder.SelectedIndex = -1;
                }
                
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LoadCombo()
        {
            BuildingManager = new Buildings(true,false);
            cmbBuilding.DataSource = BuildingManager.buildings;
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.ValueMember = "ID";
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedBuilding = BuildingManager.buildings[cmbBuilding.SelectedIndex];
                if (selectedBuilding != null) { LoadBuilding(); }
            }
            catch { }
        }

        private void LoadBuilding()
        {
            cmbBondHolder.Visible = false;
            cmbBondHolder.Text = "";
            txtID.Text = selectedBuilding.ID.ToString();
            txtName.Text = selectedBuilding.Name;
            txtAbbr.Text = selectedBuilding.Abbr;
            txtTrust.Text = selectedBuilding.Trust;
            txtPath.Text = selectedBuilding.DataPath;
            txtPeriod.Text = selectedBuilding.Period.ToString();
            txtCash.Text = selectedBuilding.Cash_Book;
            txtOwnBank.Text = selectedBuilding.OwnBank;
            txtCashbook3.Text = selectedBuilding.Cashbook3;
            txtPayment.Text = selectedBuilding.Payments.ToString();
            txtReceipt.Text = selectedBuilding.Receipts.ToString();
            txtJournal.Text = selectedBuilding.Journal.ToString();
            txtCentrec1.Text = selectedBuilding.Centrec_Account;
            txtCentrec2.Text = selectedBuilding.Centrec_Building;
            txtBus.Text = selectedBuilding.Business_Account;
            try { cmbBank.SelectedItem = selectedBuilding.Bank; } catch { cmbBank.SelectedItem = "PLEASE SELECT"; }
            txtPM.Text = selectedBuilding.PM;
            txtBankName.Text = selectedBuilding.Bank_Name;
            txtAccNumber.Text = selectedBuilding.Bank_Acc_Number;
            txtAccName.Text = selectedBuilding.Acc_Name;
            txtBranch.Text = selectedBuilding.Branch_Code;
            chkWeb.Checked = selectedBuilding.Web_Building;
            txtLetter.Text = selectedBuilding.letterName;
            txtRF.Text = selectedBuilding.reminderFee.ToString();
            txtDebitOrderFee.Text = selectedBuilding.DebitOrderFee.ToString();
            txtRFS.Text = selectedBuilding.reminderSplit.ToString();
            txtFF.Text = selectedBuilding.finalFee.ToString();
            txtFFS.Text = selectedBuilding.finalSplit.ToString();
            txtDCF.Text = selectedBuilding.disconnectionNoticefee.ToString();
            txtDCFS.Text = selectedBuilding.disconnectionNoticeSplit.ToString();
            txtSF.Text = selectedBuilding.summonsFee.ToString();
            txtSFS.Text = selectedBuilding.summonsSplit.ToString();
            txtDF.Text = selectedBuilding.disconnectionFee.ToString();
            txtDFS.Text = selectedBuilding.disconnectionSplit.ToString();
            txtHF.Text = selectedBuilding.handoverFee.ToString();
            txtHFS.Text = selectedBuilding.handoverSplit.ToString();
            txtAddress1.Text = selectedBuilding.addy1;
            txtAddress2.Text = selectedBuilding.addy2;
            txtAddress3.Text = selectedBuilding.addy3;
            txtAddress4.Text = selectedBuilding.addy4;
            txtAddress5.Text = selectedBuilding.addy5;
            txtLimitM.Text = selectedBuilding.limitM.ToString("#,##0.00");
            txtLimitW.Text = selectedBuilding.limitW.ToString("#,##0.00");
            txtLimitD.Text = selectedBuilding.limitD.ToString("#,##0.00");
            LoadBuildingInsurance();
            LoadCustomers();
            btnSave.Enabled = true;
        }

        private void LoadCustomers()
        {
            List<Customer> customers = Controller.pastel.AddCustomers(selectedBuilding.Abbr, selectedBuilding.DataPath);

            //merge trustees db and override setting in pastel

            using (var context = SqlDataHandler.GetDataContext())
            {
                var dbCustomers = context.CustomerSet
                                        .Where(a => a.BuildingId == selectedBuilding.ID)
                                        .ToList();

                foreach (var acc in customers)
                {
                    var cust = dbCustomers.SingleOrDefault(a => a.BuildingId == selectedBuilding.ID && a.AccountNumber == acc.accNumber);
                    if (cust == null)
                    {
                        cust = new Data.CustomerData.Customer()
                        {
                            BuildingId = selectedBuilding.ID,
                            AccountNumber = acc.accNumber,
                            Created = DateTime.Now,
                            IsTrustee = acc.IsTrustee
                        };
                        context.CustomerSet.Add(cust);
                        dbCustomers.Add(cust);
                    }
                    else if(cust.IsTrustee)
                    {
                        acc.IsTrustee = true;
                    }
                    if (cust.Description != acc.description)
                        cust.Description = acc.description;
                }
                context.SaveChanges();

                //foreach (Customer c in customers)
                //{
                //    int iCat = Convert.ToInt32(c.category);
                //    c.IsTrustee = iCat == 7;
                //}

                dgTrustees.AutoGenerateColumns = false;
                dgTrustees.DataSource = customers;
                dgTrustees.Refresh();
            }
        }

        private void LoadBuildingInsurance()
        {
            try
            {
                cbDisableDebitOrderFee.Enabled = Controller.UserIsSheldon();
                cbBuildingFinancialsEnabled.Enabled = Controller.UserIsSheldon();
                cbDisabled.Enabled = Controller.UserIsSheldon();
                Data.tblBuilding buildingEntity = null;
                using (var context = SqlDataHandler.GetDataContext())
                {
                    buildingEntity = context.tblBuildings.Include(a => a.InsuranceBroker)
                            .FirstOrDefault(a => a.id == selectedBuilding.ID);
                }
                if (buildingEntity != null)
                {
                    cbDisableDebitOrderFee.Checked = buildingEntity.IsDebitOrderFeeDisabled;
                    cbBuildingFinancialsEnabled.Checked = buildingEntity.BuildingFinancialsEnabled;
                    pnlFinancials.Visible = cbBuildingFinancialsEnabled.Checked;
                    cbDisabled.Checked = buildingEntity.BuildingDisabled;
                    txtCommonPropertyDim.Text = buildingEntity.CommonPropertyDimensions.ToString();
                    txtUnitPropertyDim.Text = buildingEntity.UnitPropertyDimensions.ToString();
                    txtReplacementValue.Text = buildingEntity.UnitReplacementCost.ToString("#,##0.00");
                    cbReplacementIncludesCommonProperty.Checked = buildingEntity.InsuranceReplacementValueIncludesCommonProperty;
                    cbBondHolderInterest.Checked = buildingEntity.BondHolderInterestNotedOnPolicy;
                    cmbBondHolder.Visible = cbBondHolderInterest.Checked;
                    cmbBondHolder.SelectedText = buildingEntity.InsuranceBondHolder;

                    _SelectedBroker = buildingEntity.InsuranceBroker;
                    if (_SelectedBroker != null)
                        lbBrokerName.Text = _SelectedBroker.CompanyName;
                    else
                        lbBrokerName.Text = "-- None Selected --";

                    txtCommonPropertyValue.Text = buildingEntity.CommonPropertyReplacementCost.ToString("#,##0.00");
                    txtInsurancePolicyNumber.Text = buildingEntity.PolicyNumber;

                    cbFixedFinalcials.Checked = buildingEntity.IsFixed;
                    tbFinancialDayOfMonth.Value = buildingEntity.FinancialDayOfMonth;

                    if (buildingEntity.FinancialStartDate == null)
                        buildingEntity.FinancialStartDate = DateTime.Today;
                    dtpFinancialStartDate.Value = buildingEntity.FinancialStartDate.Value;

                    if (buildingEntity.FinancialEndDate != null)
                    {
                        dtpFinancialEndDate.Value = buildingEntity.FinancialEndDate.Value;
                        dtpFinancialEndDate.Visible = true;
                        cbFinancialEndDateSet.Checked = true;
                    }
                    else
                    {
                        cbFinancialEndDateSet.Checked = false;
                        dtpFinancialEndDate.Visible = false;
                    }

                    /*Calendar*/
                    cbFixedMonhlyFinMeeting.Checked = buildingEntity.FixedMonthyFinancialMeetingEnabled;
                    pnlFinancialMeeting.Visible = cbFixedMonhlyFinMeeting.Checked;
                    if (buildingEntity.FinancialMeetingDayOfMonth != null)
                        numMeetingDayOfMonth.Value = buildingEntity.FinancialMeetingDayOfMonth.Value;
                    else
                        numMeetingDayOfMonth.Value = 1;

                    if(buildingEntity.FinancialMeetingStartTime != null)
                      dtpEventTime.Value = buildingEntity.FinancialMeetingStartTime.Value;
                    if(buildingEntity.FinancialMeetingEndTime != null)
                      dtpEventToTime.Value = buildingEntity.FinancialMeetingEndTime.Value;

                    tbSubject.Text = buildingEntity.FinancialMeetingSubject;
                    cbEvent.Text = buildingEntity.FinancialMeetingEvent;
                    tbVenue.Text = buildingEntity.FinancialMeetingVenue;
                    tbBCC.Text = buildingEntity.FinancialMeetingBCC;
                    tbBodyContent.Text = buildingEntity.FinancialMeetingBodyText;
                    cbNotifyTrustees.Checked = buildingEntity.FinancialMeetingSendInviteToAllTrustees;
                    LoadInsuranceUnitPq(buildingEntity);
                }
            }
            catch (Exception e)
            {
                Controller.HandleError(e);
            }
        }

        private void LoadInsuranceUnitPq(Data.tblBuilding buildingEntity)
        {
            var unitRecords = Controller.pastel.AddCustomers(buildingEntity.Code, buildingEntity.DataPath);
            InsurancePqGrid = new List<IInsurancePqRecord>();
            var items = unitRecords.Select(a => new InsurancePqRecord(InsurancePqGrid)
            {
                UnitNo = a.accNumber,
                SquareMeters = decimal.Round(buildingEntity.UnitPropertyDimensions / unitRecords.Count, 2),
                Notes = "",
                TotalUnitPropertyDimensions = buildingEntity.UnitPropertyDimensions,
                BuildingReplacementValue = buildingEntity.UnitReplacementCost
            }).OrderBy(a => a.UnitNo).ToList();

            InsurancePqGrid.AddRange(items);
            InsurancePqGrid.Add(new PQTotal(InsurancePqGrid));


            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildingUnits = context.BuildingUnitSet
                        .Where(a => a.BuildingId == selectedBuilding.ID).ToList();
                foreach (var unit in buildingUnits)
                {
                    var record = InsurancePqGrid.FirstOrDefault(a => a.UnitNo == unit.UnitNo) as InsurancePqRecord;
                    if (record != null)
                    {
                        record.Id = unit.id;
                        record.SquareMeters = unit.SquareMeters;
                        record.Notes = unit.Notes;
                        record.AdditionalInsurance = unit.AdditionalInsurance;
                        record.BuildingReplacementValue = buildingEntity.UnitReplacementCost;
                        record.TotalUnitPropertyDimensions = buildingEntity.UnitPropertyDimensions;
                        record.Calculate();
                    }
                }
            }

            //Add Total

            PopulateInsurancePq();
        }

        private void PopulateInsurancePq()
        {
            dgInsurancePq.ClearSelection();
            dgInsurancePq.MultiSelect = false;
            dgInsurancePq.AutoGenerateColumns = false;

            dgInsurancePq.Columns.Clear();

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UnitNo",
                HeaderText = "UnitNo",
                ReadOnly = true,
            });


            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SquareMeters",
                HeaderText = "m²",
                ReadOnly = false,
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PQCalculated",
                HeaderText = "PQ",
                ReadOnly = true
            });
            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UnitReplacementCost",
                HeaderText = "Replacement Value",
                ReadOnly = true
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AdditionalInsurance",
                HeaderText = "Additional",
                ReadOnly = false
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TotalReplacementValue",
                HeaderText = "Total Replacement Value",
                ReadOnly = true

            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Notes",
                HeaderText = "Notes",
                ReadOnly = false,
                MinimumWidth = 300
            });

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            BindingSource bs = new BindingSource();
            dgInsurancePq.DataSource = bs;
            bs.DataSource = InsurancePqGrid;
            dgInsurancePq.AutoResizeColumns();
        }

        private bool ValidatePQ()
        {
            var itms = InsurancePqGrid.Where(a => a is InsurancePqRecord).Sum(a => a.PQCalculated);
            var totalSQM = InsurancePqGrid.Where(a => a is InsurancePqRecord).Sum(a => a.SquareMeters);

            try
            {

                if (itms > 0 && totalSQM != Convert.ToDecimal(txtUnitPropertyDim.Text))
                {
                    Controller.HandleError("Please verify that the total sqm " + totalSQM.ToString() + " must equal " + txtUnitPropertyDim.Text);

                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                Controller.HandleError("Invalid Unit Property Dimension " + txtUnitPropertyDim.Text);
                return false;
            }
        }

        private void UpdateBuildingSettings()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildingEntity = context.tblBuildings
                      .FirstOrDefault(a => a.id == selectedBuilding.ID);
                buildingEntity.IsDebitOrderFeeDisabled = cbDisableDebitOrderFee.Checked;
                buildingEntity.BuildingFinancialsEnabled = cbBuildingFinancialsEnabled.Checked;
                buildingEntity.FinancialDayOfMonth = Convert.ToInt32(tbFinancialDayOfMonth.Value);
                buildingEntity.IsFixed = cbFixedFinalcials.Checked;

                buildingEntity.FinancialStartDate = dtpFinancialStartDate.Value.Date;
                if (cbFinancialEndDateSet.Checked)
                    buildingEntity.FinancialEndDate = dtpFinancialEndDate.Value.Date;
                else
                    buildingEntity.FinancialEndDate = null;

                if (Controller.UserIsSheldon())
                {
                    buildingEntity.BuildingDisabled = cbDisabled.Checked;
                }

                /*Calendar*/
                buildingEntity.FixedMonthyFinancialMeetingEnabled = cbFixedMonhlyFinMeeting.Checked;
                if (buildingEntity.FixedMonthyFinancialMeetingEnabled)
                {
                    if (String.IsNullOrWhiteSpace(tbSubject.Text) ||
                        String.IsNullOrWhiteSpace(tbSubject.Text) ||
                        String.IsNullOrWhiteSpace(tbSubject.Text) ||
                        dtpEventTime.Value > dtpEventToTime.Value)
                    {
                        Controller.HandleError("Financial meeting subject, event and venue required or To Time < From Time");
                        return;
                    }
                    else
                    {
                        buildingEntity.FinancialMeetingDayOfMonth = Convert.ToInt32(numMeetingDayOfMonth.Value);
                        buildingEntity.FinancialMeetingSubject = tbSubject.Text;
                        buildingEntity.FinancialMeetingEvent = cbEvent.Text;
                        buildingEntity.FinancialMeetingVenue = tbVenue.Text;
                        buildingEntity.FinancialMeetingBCC = tbBCC.Text;
                        buildingEntity.FinancialMeetingBodyText = tbBodyContent.Text;
                        buildingEntity.FinancialMeetingSendInviteToAllTrustees = cbNotifyTrustees.Checked;
                        buildingEntity.FinancialMeetingStartTime = dtpEventTime.Value;
                        buildingEntity.FinancialMeetingEndTime = dtpEventToTime.Value;

                    }
                }
                context.SaveChanges();
            }
        }


        private void SaveBuildingInsurance()
        {
            if (!ValidatePQ())
            {
                return;
            }

            if (_SelectedBroker == null)
            {
                Controller.HandleError("Please select an insurance broker");
                return;
            }

            if(String.IsNullOrWhiteSpace(txtInsurancePolicyNumber.Text))
            {
                Controller.HandleError("Please enter an Insurance Policy Number.");
                return;
            }

            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildingEntity = context.tblBuildings
                        .FirstOrDefault(a => a.id == selectedBuilding.ID);
                buildingEntity.CommonPropertyDimensions = string.IsNullOrWhiteSpace(txtCommonPropertyDim.Text) ? 0 :
                    Convert.ToDecimal(txtCommonPropertyDim.Text);
                buildingEntity.UnitPropertyDimensions = string.IsNullOrWhiteSpace(txtUnitPropertyDim.Text) ? 0 :
                    Convert.ToDecimal(txtUnitPropertyDim.Text);
                buildingEntity.UnitReplacementCost = string.IsNullOrWhiteSpace(txtReplacementValue.Text) ? 0 :
                    Convert.ToDecimal(txtReplacementValue.Text);
                buildingEntity.CommonPropertyReplacementCost = string.IsNullOrWhiteSpace(txtCommonPropertyValue.Text) ? 0 :
                    Convert.ToDecimal(txtCommonPropertyValue.Text);
                buildingEntity.PolicyNumber = txtInsurancePolicyNumber.Text;
                buildingEntity.InsuranceReplacementValueIncludesCommonProperty = cbReplacementIncludesCommonProperty.Checked;

                buildingEntity.BondHolderInterestNotedOnPolicy = cbBondHolderInterest.Checked;
                cmbBondHolder.Visible = cbBondHolderInterest.Checked;
                buildingEntity.InsuranceBondHolder = cmbBondHolder.SelectedText;

                if (_SelectedBroker == null)
                    buildingEntity.InsuranceBrokerId = null;
                else
                    buildingEntity.InsuranceBrokerId = _SelectedBroker.id;


                foreach (var item in InsurancePqGrid.Where(a => a is InsurancePqRecord).Select(a => a as InsurancePqRecord).ToList())
                {
                    if (item.Id == null)
                    {
                        context.BuildingUnitSet.Add(new Data.MaintenanceData.BuildingUnit()
                        {
                            BuildingId = buildingEntity.id,
                            AdditionalInsurance = item.AdditionalInsurance,
                            Notes = item.Notes,
                            PQRating = item.PQCalculated,
                            SquareMeters = item.SquareMeters,
                            UnitNo = item.UnitNo,
                        });
                    }
                    else
                    {
                        var update = context.BuildingUnitSet.First(a => a.id == item.Id);
                        update.BuildingId = buildingEntity.id;
                        update.AdditionalInsurance = item.AdditionalInsurance;
                        update.Notes = item.Notes;
                        update.PQRating = item.PQCalculated;
                        update.SquareMeters = item.SquareMeters;
                        update.UnitNo = item.UnitNo;
                    }
                }
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Controller.HandleError(e);
                }
            }
        }

        private void RecursiveClearForm(Control c)
        {
            foreach (var x in c.Controls)
            {
                if (x is TextBox)
                    ((TextBox)x).Text = "";
                else if (x is CheckBox)
                    ((CheckBox)x).Checked = false;
                else if (((Control)x).Controls != null)
                    foreach (var y in ((Control)x).Controls)
                        RecursiveClearForm(y as Control);
            }
        }

        private void clearBuilding()
        {
            foreach (var item in this.Controls)
                RecursiveClearForm(item as Control);
            //txtID.Text = txtName.Text = txtAbbr.Text = txtTrust.Text = txtPath.Text = txtPeriod.Text = txtCash.Text = txtOwnBank.Text = txtCashbook3.Text = txtPayment.Text = txtReceipt.Text = "";
            //txtJournal.Text = txtCentrec1.Text = txtCentrec2.Text = txtBus.Text = "";
            cmbBank.SelectedItem = "PLEASE SELECT";
            dgTrustees.DataSource = null;
            //txtPM.Text = txtBankName.Text = txtAccNumber.Text = txtAccName.Text = txtBranch.Text = "";
            //chkWeb.Checked = btnSave.Enabled = false;
            //txtRF.Text = txtRFS.Text = txtFF.Text = txtFFS.Text = txtDCF.Text = txtDCFS.Text = txtSF.Text = txtSFS.Text = txtDF.Text = txtDFS.Text = txtHF.Text = txtHFS.Text = txtAddress1.Text = "";
            //txtAddress2.Text = txtAddress3.Text = txtAddress4.Text = txtAddress5.Text = "";
        }

        private void SaveBuilding()
        {
            selectedBuilding.ID = int.Parse(txtID.Text);
            selectedBuilding.Name = txtName.Text;
            selectedBuilding.Abbr = txtAbbr.Text;
            selectedBuilding.Trust = txtTrust.Text;
            selectedBuilding.DataPath = txtPath.Text;
            selectedBuilding.Period = int.Parse(txtPeriod.Text);
            selectedBuilding.Cash_Book = txtCash.Text;
            selectedBuilding.OwnBank = txtOwnBank.Text;
            selectedBuilding.Cashbook3 = txtCashbook3.Text;
            selectedBuilding.Payments = int.Parse(txtPayment.Text);
            selectedBuilding.Receipts = int.Parse(txtReceipt.Text);
            selectedBuilding.Journal = int.Parse(txtJournal.Text);
            selectedBuilding.Centrec_Account = txtCentrec1.Text;
            selectedBuilding.Centrec_Building = txtCentrec2.Text;
            selectedBuilding.Business_Account = txtBus.Text;
            selectedBuilding.Bank = cmbBank.SelectedItem.ToString();
            selectedBuilding.PM = txtPM.Text;
            selectedBuilding.Bank_Name = txtBankName.Text;
            selectedBuilding.Bank_Acc_Number = txtAccNumber.Text;
            selectedBuilding.Acc_Name = txtAccName.Text;
            selectedBuilding.Branch_Code = txtBranch.Text;
            selectedBuilding.Web_Building = chkWeb.Checked;
            selectedBuilding.letterName = txtLetter.Text;
            selectedBuilding.DebitOrderFee = decimal.Parse(txtDebitOrderFee.Text);
            selectedBuilding.reminderFee = double.Parse(txtRF.Text);
            selectedBuilding.reminderSplit = double.Parse(txtRFS.Text);
            selectedBuilding.finalFee = double.Parse(txtFF.Text);
            selectedBuilding.finalSplit = double.Parse(txtFFS.Text);
            selectedBuilding.disconnectionNoticefee = double.Parse(txtDCF.Text);
            selectedBuilding.disconnectionNoticeSplit = double.Parse(txtDCFS.Text);
            selectedBuilding.summonsFee = double.Parse(txtSF.Text);
            selectedBuilding.summonsSplit = double.Parse(txtSFS.Text);
            selectedBuilding.disconnectionFee = double.Parse(txtDF.Text);
            selectedBuilding.disconnectionSplit = double.Parse(txtDFS.Text);
            selectedBuilding.handoverFee = double.Parse(txtHF.Text);
            selectedBuilding.handoverSplit = double.Parse(txtHFS.Text);
            selectedBuilding.addy1 = txtAddress1.Text;
            selectedBuilding.addy2 = txtAddress2.Text;
            selectedBuilding.addy3 = txtAddress3.Text;
            selectedBuilding.addy4 = txtAddress4.Text;
            selectedBuilding.addy5 = txtAddress5.Text;

            double value = 0;
            selectedBuilding.limitM = double.TryParse(txtLimitM.Text, out value) ? value : 0;
            value = 0;
            selectedBuilding.limitW = double.TryParse(txtLimitW.Text, out value) ? value : 0;
            value = 0;
            selectedBuilding.limitD = double.TryParse(txtLimitD.Text, out value) ? value : 0;

            try
            {
                BuildingManager.buildings[cmbBuilding.SelectedIndex] = selectedBuilding;
                if (selectedBuilding.Name != "Add new building" && !String.IsNullOrEmpty(selectedBuilding.Name))
                {
                    String status = String.Empty;
                    if (BuildingManager.Update(cmbBuilding.SelectedIndex, false, out status))
                    {
                        SaveWebBuilding(false);
                        UpdateBuildingSettings();
                        SaveBuildingInsurance();
                        SaveTrustees();
                        MessageBox.Show("Building updated!", "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedBuilding = null;
                        clearBuilding();
                        LoadCombo();
                    }
                    else
                    {
                        MessageBox.Show("Building update failed: SV1 " + status, "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter building name", "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Building update failed: SV2" + ex.Message, "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveTrustees()
        {
            this.Cursor = Cursors.WaitCursor;
            using (var dbContext = SqlDataHandler.GetDataContext())
            {
                List<Customer> vCustomers = Controller.pastel.AddCustomers(selectedBuilding.Abbr, selectedBuilding.DataPath);
                MySqlConnector myConn = new MySqlConnector();
                myConn.ToggleConnection(true);
                String usergroup = "";
                List<Customer> customers = dgTrustees.DataSource as List<Customer>;
                var dbCustomers = dbContext.CustomerSet.Where(a => a.BuildingId == selectedBuilding.ID).ToList();

                foreach (Customer customer in customers)
                {
                    bool trustee = false;

                    foreach (String email in customer.Email)
                    {
                        if (email != "sheldon@astrodon.co.za")
                        {
                            String[] login = myConn.HasLogin(email);
                            if (login != null)
                            {
                                if (customer.IsTrustee)
                                {
                                    usergroup = "1,2,4";
                                    trustee = true;
                                }
                                else
                                {
                                    usergroup = "1,2";
                                }
                                myConn.UpdateGroup(login[0], usergroup);
                            }

                        }
                    }
                    Customer vCustomer = vCustomers.SingleOrDefault(c => c.accNumber == customer.accNumber);

                    var dbCust = dbCustomers.Where(a => a.AccountNumber == customer.accNumber).SingleOrDefault();
                    if(dbCust == null)
                    {
                        dbCust = new Astrodon.Data.CustomerData.Customer()
                        {
                            AccountNumber = customer.accNumber,
                            BuildingId = selectedBuilding.ID,
                            Created = DateTime.Now
                        };
                        dbContext.CustomerSet.Add(dbCust);
                    }
                    dbCust.Description = customer.description;
                    dbCust.IsTrustee = customer.IsTrustee;

                    UpdateCustomer(vCustomer, trustee);
                }
                myConn.ToggleConnection(false);
                dbContext.SaveChanges();
            }
            this.Cursor = Cursors.Arrow;
        }

        private void UpdateCustomer(Customer vCustomer, bool isTrustee)
        {
            bool changeMe = false;
            if (Convert.ToInt32(vCustomer.category) == 7 && !isTrustee)
            {
                vCustomer.category = "0";
                changeMe = true;
            }
            else if (Convert.ToInt32(vCustomer.category) != 7 && isTrustee)
            {
                vCustomer.category = "07";
                changeMe = true;
            }
            if (changeMe)
            {
                String result = Controller.pastel.UpdateCustomer(vCustomer.GetCustomer(), selectedBuilding.DataPath);
            }
        }

        private void SaveWebBuilding(bool remove)
        {
            MySqlConnector mysql = new MySqlConnector();
            Classes.Sftp ftpClient = new Classes.Sftp(String.Empty, false);
            String websafeName = selectedBuilding.Name.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
            selectedBuilding.webFolder = websafeName;
            try
            {
                String status = String.Empty;
                if (!remove)
                {
                    if (BuildingManager.Update(selectedBuilding, out status))
                    {
                        String newID = "";
                        mysql.InsertBuilding(selectedBuilding, selectedBuilding.Name, selectedBuilding.Abbr, out newID, out status);
                        selectedBuilding.pid = newID;
                        BuildingManager.Update(selectedBuilding, out status);
                        if (ftpClient.CreateDirectory(websafeName, false))
                        {
                            ftpClient.WorkingDirectory += "/" + websafeName;
                            ftpClient.ChangeDirectory(false);
                            ftpClient.CreateDirectory("Annual Financial Statements", false);
                            ftpClient.CreateDirectory("Conduct Rules", false);
                            ftpClient.CreateDirectory("Insurance Information", false);
                            ftpClient.CreateDirectory("Meeting Minutes", false);
                            ftpClient.CreateDirectory("Meeting Notices", false);
                            ftpClient.CreateDirectory("Sectional Title Plans", false);
                        }
                        ftpClient.CreateDirectory(websafeName, true);
                    }
                }
                else
                {
                    mysql.DeleteBuilding(selectedBuilding, selectedBuilding.Name, selectedBuilding.Abbr, out status);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectedBuilding = null;
            clearBuilding();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveBuilding();
        }

        private void btnStandard_Click(object sender, EventArgs e)
        {
            String settingsQuery = "SELECT reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee FROM tblSettings";
            SqlDataHandler dh = new SqlDataHandler();
            String status = String.Empty;
            DataSet dsStandard = dh.GetData(settingsQuery, null, out status);
            if (dsStandard != null && dsStandard.Tables.Count > 0 && dsStandard.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsStandard.Tables[0].Rows[0];
                txtRF.Text = dr["reminder_fee"].ToString();
                txtFF.Text = dr["final_fee"].ToString();
                txtDCF.Text = dr["discon_notice_fee"].ToString();
                txtSF.Text = dr["summons_fee"].ToString();
                txtDF.Text = dr["discon_fee"].ToString();
                txtHF.Text = dr["handover_fee"].ToString();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedBuilding != null)
            {
                if (MessageBox.Show("Confirm remove building " + selectedBuilding.Name + "?", "Remove building", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (Prompt.ShowDialog("Please enter password", "Buildings") == "Sheldon123")
                    {
                        String status = "";
                        BuildingManager.Update(cmbBuilding.SelectedIndex, true, out status);
                        SaveWebBuilding(true);
                        clearBuilding();
                        LoadCombo();
                    }
                }
            }
        }

        private void btnUploadInsuranceContract_Click(object sender, EventArgs e)
        {
            if (fdOpen.ShowDialog() == DialogResult.OK)
            {
                btnUploadInsuranceContract.Enabled = false;
                try
                {
                    if(!IsValidPdf(fdOpen.FileName))
                    {
                        btnUploadInsuranceContract.Enabled = true;
                        Controller.HandleError("Not a valid PDF");
                        return;
                    }

                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var fileEntity = context.BuildingDocumentSet
                                .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceContract);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.InsuranceContract;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fdOpen.FileName);
                        fileEntity.FileName = Path.GetFileName(fdOpen.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded Insurance Form");
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to upload Insurance Form");
                }
                finally
                {
                    btnUploadInsuranceContract.Enabled = true;
                }
            }
        }

        private void btnUploadClaimForm_Click(object sender, EventArgs e)
        {
            if (fUploadClaimForm.ShowDialog() == DialogResult.OK)
            {
                btnUploadClaimForm.Enabled = false;
                try
                {
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var fileEntity = context.BuildingDocumentSet
                             .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceClaimForm);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.InsuranceClaimForm;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fUploadClaimForm.FileName);
                        fileEntity.FileName = Path.GetFileName(fUploadClaimForm.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded Claim Form");
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to upload Claim Form");
                }
                finally
                {
                    btnUploadClaimForm.Enabled = true;
                }
            }
        }

        private void btnViewInsuranceContract_Click(object sender, EventArgs e)
        {
            btnViewInsuranceContract.Enabled = false;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var fileEntity = context.BuildingDocumentSet
                     .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceContract);
                    if (fileEntity != null)
                    {
                        fdSave.FileName = fileEntity.FileName;
                        if (fdSave.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(fdSave.FileName, fileEntity.FileData);
                            MessageBox.Show("Successfully Downloaded Insurance Form");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No document exits");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to download Insurance Form");
            }
            finally
            {
                btnViewInsuranceContract.Enabled = true;
            }
        }

        private void btnViewClaimForm_Click(object sender, EventArgs e)
        {
            btnViewClaimForm.Enabled = false;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var fileEntity = context.BuildingDocumentSet
                     .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceClaimForm);
                    if (fileEntity != null)
                    {
                        fdSaveClaimForm.FileName = fileEntity.FileName;
                        if (fdSaveClaimForm.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(fdSaveClaimForm.FileName, fileEntity.FileData);
                            MessageBox.Show("Successfully Downloaded Form");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No document exits");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to download Claim Form");
            }
            finally
            {
                btnViewClaimForm.Enabled = true;
            }
        }

        private void txtUnitPropertyDim_TextChanged(object sender, EventArgs e)
        {
            var d = txtUnitPropertyDim.Text;
            if (!string.IsNullOrWhiteSpace(d))
            {
                try
                {
                    var val = Convert.ToDecimal(d);
                    foreach (var x in InsurancePqGrid)
                    {
                        if (x is InsurancePqRecord)
                        {
                            x.TotalUnitPropertyDimensions = val;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void tbInsurance_Click(object sender, EventArgs e)
        {
        }

        private void dgTrustees_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //customers = dgTrustees.DataSource as List<Customer>;
            //foreach (Customer c in customers)
            //{
            //    int iCat = Convert.ToInt32(c.category);
            //    if (Controller.user.id == 1) { MessageBox.Show(iCat.ToString()); }
            //    c.IsTrustee = iCat == 7;
            //}
        }

        private void txtReplacementValue_TextChanged(object sender, EventArgs e)
        {
            var d = txtReplacementValue.Text;
            if (!string.IsNullOrWhiteSpace(d))
            {
                try
                {
                    var val = Convert.ToDecimal(d);
                    foreach (var x in InsurancePqGrid)
                    {
                        if (x is InsurancePqRecord)
                        {
                            x.BuildingReplacementValue = val;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void dgInsurancePq_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgInsurancePq.Rows)
            {
                IInsurancePqRecord reqItem = row.DataBoundItem as IInsurancePqRecord;
                reqItem.DataRow = row;
                if(reqItem is PQTotal)
                {
                    row.ReadOnly = true;
                }



            }
        }

        private void btnBuildingPlans_Click(object sender, EventArgs e)
        {
            if (fUploadClaimForm.ShowDialog() == DialogResult.OK)
            {
                btnBuildingPlans.Enabled = false;
                try
                {
                    if (!IsValidPdf(fdOpen.FileName))
                    {
                        btnBuildingPlans.Enabled = true;
                        Controller.HandleError("Not a valid PDF");
                        return;
                    }
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var fileEntity = context.BuildingDocumentSet
                             .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.BuildingPlans);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.BuildingPlans;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fUploadClaimForm.FileName);
                        fileEntity.FileName = Path.GetFileName(fUploadClaimForm.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded Building Plans");
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to upload Building Plans");
                }
                finally
                {
                    btnBuildingPlans.Enabled = true;
                }
            }
        }

        private void btnDownloadBuildingPlans_Click_1(object sender, EventArgs e)
        {
            btnDownloadBuildingPlans.Enabled = false;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var fileEntity = context.BuildingDocumentSet
                     .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.BuildingPlans);
                    if (fileEntity != null)
                    {
                        fdSaveClaimForm.FileName = fileEntity.FileName;
                        if (fdSaveClaimForm.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(fdSaveClaimForm.FileName, fileEntity.FileData);
                            MessageBox.Show("Successfully Downloaded");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No document exits");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to download");
            }
            finally
            {
                btnDownloadBuildingPlans.Enabled = true;
            }
        }

        private void btnUploadPQ_Click(object sender, EventArgs e)
        {
            if (fUploadClaimForm.ShowDialog() == DialogResult.OK)
            {
                btnUploadPQ.Enabled = false;
                try
                {
                    if (!IsValidPdf(fdOpen.FileName))
                    {
                        btnUploadPQ.Enabled = true;
                        Controller.HandleError("Not a valid PDF");
                        return;
                    }
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var fileEntity = context.BuildingDocumentSet
                             .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.PQ);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.PQ;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fUploadClaimForm.FileName);
                        fileEntity.FileName = Path.GetFileName(fUploadClaimForm.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded PQ");
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to upload PQ");
                }
                finally
                {
                    btnUploadPQ.Enabled = true;
                }
            }
        }

        private void btnDownloadPQ_Click(object sender, EventArgs e)
        {
            btnDownloadPQ.Enabled = false;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var fileEntity = context.BuildingDocumentSet
                     .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == Data.InsuranceData.DocumentType.PQ);
                    if (fileEntity != null)
                    {
                        fdSaveClaimForm.FileName = fileEntity.FileName;
                        if (fdSaveClaimForm.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(fdSaveClaimForm.FileName, fileEntity.FileData);
                            MessageBox.Show("Successfully Downloaded");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No document exits");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to download");
            }
            finally
            {
                btnDownloadPQ.Enabled = true;
            }
        }

        private void cbBondHolderInterest_CheckedChanged(object sender, EventArgs e)
        {
            cmbBondHolder.Visible = cbBondHolderInterest.Checked;
        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        private InsuranceBroker _SelectedBroker;
        private void btnBrokerLookup_Click(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedIndex < 0)
            {
                Controller.HandleError("Please select a building first.", "Validation Error");
                return;
            }

            using (var context = SqlDataHandler.GetDataContext())
            {
                var frmBrokerLookup = new frmInsuranceBrokerLookup(context);

                var dialogResult = frmBrokerLookup.ShowDialog();
                var broker = frmBrokerLookup.SelectedInsuranceBroker;

                if (dialogResult == DialogResult.OK && broker != null)
                {

                    _SelectedBroker = broker;
                    lbBrokerName.Text = _SelectedBroker.CompanyName;
                }
                else
                {
                    ClearBroker();
                }
            }
        }

        private void ClearBroker()
        {
            _SelectedBroker = null;
            lbBrokerName.Text = "";
        }

        #region PDF Handler


        private string _TempPDFFile = string.Empty;
        private void DisplayPDF(byte[] pdfData)
        {
            if (pdfData == null)
            {
                this.axAcroPDF1.Visible = false;
                return;
            }
            if (!String.IsNullOrWhiteSpace(_TempPDFFile))
                File.Delete(_TempPDFFile);
            _TempPDFFile = Path.GetTempPath();
            if (!_TempPDFFile.EndsWith(@"\"))
                _TempPDFFile = _TempPDFFile + @"\";

            _TempPDFFile = _TempPDFFile + System.Guid.NewGuid().ToString("N") + ".pdf";
            File.WriteAllBytes(_TempPDFFile, pdfData);


            try
            {
                this.axAcroPDF1.Visible = true;
                this.axAcroPDF1.LoadFile(_TempPDFFile);
                this.axAcroPDF1.src = _TempPDFFile;
                this.axAcroPDF1.setShowToolbar(false);
                this.axAcroPDF1.setView("FitH");
                this.axAcroPDF1.setLayoutMode("SinglePage");
                this.axAcroPDF1.setShowToolbar(false);

                this.axAcroPDF1.Show();
                tabControl1.SelectedTab = tbPDFViewer;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            File.Delete(_TempPDFFile);
        }

        private bool IsValidPdf(string filepath)
        {
            bool Ret = true;

            PdfReader reader = null;

            try
            {
                using (reader = new PdfReader(filepath))
                {
                    reader.Close();
                }
            }
            catch
            {
                Ret = false;
            }

            return Ret;
        }


        #endregion

        private void ViewBuildingDocument(Data.InsuranceData.DocumentType documentType)
        {
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var fileEntity = context.BuildingDocumentSet
                     .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == documentType);
                    if (fileEntity != null)
                    {
                        DisplayPDF(fileEntity.FileData);
                    }
                    else
                    {
                        MessageBox.Show("No document exits");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to download");
            }
        }

        private void btnViewBuildingPlans_Click(object sender, EventArgs e)
        {
            btnViewBuildingPlans.Enabled = false;
            try
            {
                ViewBuildingDocument(DocumentType.BuildingPlans);
            }
            finally
            {
                btnViewBuildingPlans.Enabled = true;
            }
        }

        private void btnViewPq_Click(object sender, EventArgs e)
        {
            btnViewPq.Enabled = false;
            try
            {
                ViewBuildingDocument(DocumentType.PQ);
            }
            finally
            {
                btnViewPq.Enabled = true;
            }
        }

        private void btnViewContract_Click(object sender, EventArgs e)
        {
            btnViewContract.Enabled = false;
            try
            {
                ViewBuildingDocument(DocumentType.InsuranceContract);
            }
            finally
            {
                btnViewContract.Enabled = true;
            }
        }

        private void label57_Click(object sender, EventArgs e)
        {

        }

        private void label62_Click(object sender, EventArgs e)
        {

        }

        private void cbFinancialEndDateSet_CheckedChanged(object sender, EventArgs e)
        {
            dtpFinancialEndDate.Visible = cbFinancialEndDateSet.Checked;
        }

        private void cbBuildingFinancialsEnabled_CheckedChanged(object sender, EventArgs e)
        {
            pnlFinancials.Visible = cbBuildingFinancialsEnabled.Checked;
        }

        private void cbFixedMonhlyFinMeeting_CheckedChanged(object sender, EventArgs e)
        {
            pnlFinancialMeeting.Visible = cbFixedMonhlyFinMeeting.Checked;
        }
    }

    internal interface IInsurancePqRecord
    {

        string UnitNo { get; set; }

        decimal SquareMeters { get; set; }

        decimal AdditionalInsurance { get; set; }

        string Notes { get; set; }

        decimal TotalUnitPropertyDimensions { get; set; }

        decimal PQCalculated { get; }

        decimal BuildingReplacementValue { get; set; }
        decimal UnitReplacementCost { get;  }
        decimal TotalReplacementValue { get;  }
        DataGridViewRow DataRow { get; set; }
    }

    internal class InsurancePqRecord: IInsurancePqRecord
    {
        protected List<IInsurancePqRecord> _Items;
        public InsurancePqRecord(List<IInsurancePqRecord> items)
        {
            _Items = items;
        }

        public int? Id { get; set; }
        public string UnitNo { get; set; }

        private decimal _SquareMeters;
        public decimal SquareMeters { get { return _SquareMeters; } set { _SquareMeters = value; Calculate(); } }

        private decimal _AdditionalInsurance;
        public decimal AdditionalInsurance { get { return _AdditionalInsurance; } set { _AdditionalInsurance = value; Calculate(); } }

        public string Notes { get; set; }

        private decimal _TotalUnitPropertyDimensions;
        public decimal TotalUnitPropertyDimensions { get { return _TotalUnitPropertyDimensions; } set { _TotalUnitPropertyDimensions = value; Calculate(); } }

        public virtual decimal PQCalculated
        {
            get
            {
                if (TotalUnitPropertyDimensions <= 0)
                    return 0;
                return Math.Round((SquareMeters / TotalUnitPropertyDimensions)*100, 4);
            }
        }

        private decimal _BuildingReplacementValue;
        public decimal BuildingReplacementValue { get { return _BuildingReplacementValue; } set { _BuildingReplacementValue = value; Calculate(); } }
        public virtual decimal UnitReplacementCost { get { return Math.Round(BuildingReplacementValue * PQCalculated, 2); } }
        public virtual decimal TotalReplacementValue { get { return AdditionalInsurance + UnitReplacementCost; } }

        public DataGridViewRow DataRow { get; set; }

        public virtual void Calculate()
        {
            if (DataRow != null)
            {
                foreach (var c in DataRow.Cells)
                {
                    var cell = c as DataGridViewCell;
                    DataRow.DataGridView.InvalidateCell(cell);
                }
            }

            var total = _Items.Where(a => a is PQTotal).FirstOrDefault();
            if (total != null)
            {
                (total as PQTotal).Calculate();
            }
        }
    }

    internal class PQTotal : IInsurancePqRecord
    {
        protected List<IInsurancePqRecord> _Items;
        public PQTotal(List<IInsurancePqRecord> items)
        {
            _Items = items;
            UnitNo = "Total";
        }

        public  void Calculate()
        {
            UnitReplacementCost = _Items.Where(a => a != this).Sum(a => a.UnitReplacementCost);
            PQCalculated = _Items.Where(a => a != this).Sum(a => a.PQCalculated);
            TotalReplacementValue = _Items.Where(a => a != this).Sum(a => a.TotalReplacementValue);
            SquareMeters = _Items.Where(a => a != this).Sum(a => a.SquareMeters);
            AdditionalInsurance = _Items.Where(a => a != this).Sum(a => a.AdditionalInsurance);
            TotalUnitPropertyDimensions = _Items.Where(a => a != this).Sum(a => a.TotalUnitPropertyDimensions);
            BuildingReplacementValue = _Items.Where(a => a != this).Sum(a => a.BuildingReplacementValue);

            if (DataRow != null)
            {
                foreach (var c in DataRow.Cells)
                {
                    var cell = c as DataGridViewCell;
                    DataRow.DataGridView.InvalidateCell(cell);
                }
            }
        }

        public  decimal UnitReplacementCost { get; set; }

        public  decimal PQCalculated { get; set; }

        public  decimal TotalReplacementValue { get; set; }

        public decimal SquareMeters { get; set; }

        public decimal AdditionalInsurance { get; set; }

        public decimal TotalUnitPropertyDimensions { get; set; }

        public decimal BuildingReplacementValue { get; set; }

        public string Notes { get; set; }

        public string UnitNo { get; set; }

        public DataGridViewRow DataRow { get; set; }

    }
}
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
using Astrodon.Data.BankData;
using Astrodon.ClientPortal;

namespace Astrodon
{
    public partial class usrBuildings : UserControl
    {
        private Buildings BuildingManager;
        private Building selectedBuilding = null;
        private List<BondOriginator> _BondOriginators;

        private List<IInsurancePqRecord> InsurancePqGrid { get; set; }

        public usrBuildings()
        {
            InitializeComponent();

            dtpEventToTime.Format = DateTimePickerFormat.Time;
            dtpEventToTime.ShowUpDown = true;

            dtpEventTime.Format = DateTimePickerFormat.Time;
            dtpEventTime.ShowUpDown = true;
            btnRemove.Visible = Controller.UserIsSheldon();
            LoadBondOriginators();
        }

        private void LoadBondOriginators()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                _BondOriginators = context.BondOriginatorSet.OrderBy(a => a.CompanyName).ToList();
            }
        }

        private void usrBuildings_Load(object sender, EventArgs e)
        {
            LoadCombo();
            clearBuilding();
            
        }

        private void LoadCombo()
        {
            BuildingManager = new Buildings(true,true);
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
            List<Customer> customers = Controller.pastel.AddCustomers(selectedBuilding.Abbr, selectedBuilding.DataPath, true);

            //merge trustees db and override setting in pastel

            using (var context = SqlDataHandler.GetDataContext())
            {
                var dbCustomers = context.CustomerSet
                                        .Where(a => a.BuildingId == selectedBuilding.ID)
                                        .ToList();
                bool saveChanges = false;
                foreach (var acc in customers)
                {
                    var cust = dbCustomers.SingleOrDefault(a => a.BuildingId == selectedBuilding.ID
                                                             && a.AccountNumber == acc.accNumber);
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
                        saveChanges = true;
                    }
                    acc.IsTrustee = cust.IsTrustee;
                    if (cust.Description != acc.description)
                    {
                        cust.Description = acc.description;
                    }
                    cust.LoadEmails(acc.Email);
                    saveChanges = true;

                }
                if (saveChanges)
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
                List<BuildingDocument> buildingDocumentEntities = null;
                using (var context = SqlDataHandler.GetDataContext())
                {
                    buildingEntity = context.tblBuildings.Include(a => a.InsuranceBroker)
                            .FirstOrDefault(a => a.id == selectedBuilding.ID);
                    buildingDocumentEntities = context.BuildingDocumentSet
                         .Where(a => a.BuildingId == selectedBuilding.ID).ToList();
                }
                if (buildingEntity != null)
                {
                    if (!String.IsNullOrWhiteSpace(buildingEntity.BuildingRegistrationNumber))
                        txtBuildingRegNum.Text = buildingEntity.BuildingRegistrationNumber;
                    else
                        txtBuildingRegNum.Text = string.Empty;

                    if (!String.IsNullOrWhiteSpace(buildingEntity.CSOSRegistrationNumber))
                        txtCSOSRegNum.Text = buildingEntity.CSOSRegistrationNumber;
                    else
                        txtCSOSRegNum.Text = string.Empty;

                    cbDisableDebitOrderFee.Checked = buildingEntity.IsDebitOrderFeeDisabled;
                    cbBuildingFinancialsEnabled.Checked = buildingEntity.BuildingFinancialsEnabled;
                    pnlFinancials.Visible = cbBuildingFinancialsEnabled.Checked;
                    cbDisabled.Checked = buildingEntity.BuildingDisabled;
                    txtCommonPropertyDim.Text = buildingEntity.CommonPropertyDimensions.ToString();
                    txtUnitPropertyDim.Text = buildingEntity.UnitPropertyDimensions.ToString();
                    txtReplacementValue.Text = buildingEntity.UnitReplacementCost.ToString("#,##0.00");
                    cbReplacementIncludesCommonProperty.Checked = buildingEntity.InsuranceReplacementValueIncludesCommonProperty;

                    _SelectedBroker = buildingEntity.InsuranceBroker;
                    if (_SelectedBroker != null)
                        lbBrokerName.Text = _SelectedBroker.CompanyName;
                    else
                        lbBrokerName.Text = "-- None Selected --";

                    txtCommonPropertyValue.Text = buildingEntity.CommonPropertyReplacementCost.ToString("#,##0.00");
                    txtAdditionalInsuredValue.Text = buildingEntity.AdditionalInsuredValueCost.ToString("#,##0.00");
                    txtAdditionalPremium.Text = buildingEntity.AdditionalPremiumValue.ToString("#,##0.00");
                    txtInsurancePolicyNumber.Text = buildingEntity.PolicyNumber;
                    tbExcessStructures.Text = buildingEntity.ExcessStructures;

                    dtpPolicyRenewalDate.Format = DateTimePickerFormat.Custom;
                    dtpPolicyRenewalDate.CustomFormat = "yyyy/MM/dd";
                    dtpPolicyRenewalDate.MinDate = DateTime.Today.AddYears(-5);

                    dtpPolicyExpiryDate.Format = DateTimePickerFormat.Custom;
                    dtpPolicyExpiryDate.CustomFormat = "yyyy/MM/dd";
                    dtpPolicyExpiryDate.MinDate = DateTime.Today.AddYears(-5);

                    if (buildingEntity.InsurancePolicyRenewalDate != null)
                        dtpPolicyRenewalDate.Value = buildingEntity.InsurancePolicyRenewalDate.Value;
                    else
                        dtpPolicyRenewalDate.Value = DateTime.Today;

                    if (buildingEntity.InsurancePolicyExpiryDate != null)
                        dtpPolicyExpiryDate.Value = buildingEntity.InsurancePolicyExpiryDate.Value;
                    else
                        dtpPolicyExpiryDate.Value = DateTime.Today.AddMonths(12);


                    txtMonthlyPremium.Text = buildingEntity.MonthlyInsurancePremium.ToString("#,##0.00");

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

                    var totalPqItems = InsurancePqGrid.Where(a => a is PQTotal).FirstOrDefault();
                    if (totalPqItems != null)
                    {
                        var totalReplacementValue = totalPqItems.TotalReplacementValue;
                        txtTotalReplacementValue.Text = totalReplacementValue.ToString("#,##0.00");
                    }
                }
                btnUploadInsuranceContract.Enabled = true;
                btnUploadClaimForm.Enabled = true;
                btnBuildingPlans.Enabled = true;
                btnUploadPQ.Enabled = true;

                if (buildingDocumentEntities != null && buildingDocumentEntities.Count > 0)
                {
                    foreach (var buildingDocument in buildingDocumentEntities)
                    {
                        switch (buildingDocument.DocumentType)
                        {
                            case DocumentType.InsuranceContract:
                                btnViewContract.Enabled = true;
                                btnViewInsuranceContract.Enabled = true;
                                break;
                            case DocumentType.InsuranceClaimForm:
                                btnViewClaimForm.Enabled = true;
                                break;
                            case DocumentType.BuildingPlans:
                                btnViewBuildingPlans.Enabled = true;
                                btnDownloadBuildingPlans.Enabled = true;
                                break;
                            case DocumentType.PQ:
                                btnViewPq.Enabled = true;
                                btnDownloadPQ.Enabled = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Controller.HandleError(e);
            }
        }

        private void LoadInsuranceUnitPq(Data.tblBuilding buildingEntity)
        {
            var unitRecords = Controller.pastel.AddCustomers(buildingEntity.Code, buildingEntity.DataPath, true);
            InsurancePqGrid = new List<IInsurancePqRecord>();
            var items = unitRecords.Select(a => new InsurancePqRecord(InsurancePqGrid,_BondOriginators)
            {
                UnitNo = a.accNumber,
                SquareMeters = decimal.Round(buildingEntity.UnitPropertyDimensions / unitRecords.Count, 2),
                Notes = "",
                TotalUnitPropertyDimensions = buildingEntity.UnitPropertyDimensions,
                BuildingReplacementValue = buildingEntity.UnitReplacementCost,
                BuildingPremium = buildingEntity.MonthlyInsurancePremium
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
                        record.AdditionalPremium = unit.AdditionalPremium;
                        record.BondOriginatorInterestNoted = unit.BondOriginatorInterestNoted;
                        record.BondOriginatorId = unit.BondOriginatorId;

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

            DataGridViewCellStyle currencyCellFormat = new DataGridViewCellStyle()
            {
                Format = "c",
                Alignment = DataGridViewContentAlignment.MiddleRight
            };


            DataGridViewCellStyle numberCell = new DataGridViewCellStyle()
            {
                Format = "0.0000",
                Alignment = DataGridViewContentAlignment.MiddleRight
            };

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
                DefaultCellStyle = numberCell
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PQCalculatedPercentage",
                HeaderText = "PQ",
                ReadOnly = true,
                DefaultCellStyle = numberCell
            });
            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UnitReplacementCost",
                HeaderText = "Replacement / PQ",
                ReadOnly = true,
                DefaultCellStyle = currencyCellFormat,
                MinimumWidth = 100
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AdditionalInsurance",
                HeaderText = "Additional Insurance",
                ReadOnly = false,
                DefaultCellStyle = currencyCellFormat,
                MinimumWidth = 100
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UnitPremium",
                HeaderText = "Unit Premium",
                ReadOnly = true,
                DefaultCellStyle = currencyCellFormat,
                MinimumWidth = 100
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AdditionalPremium",
                HeaderText = "Additional Premium",
                ReadOnly = false,
                DefaultCellStyle = currencyCellFormat,
                MinimumWidth = 100
            });
            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TotalPremium",
                HeaderText = "Total Premium",
                ReadOnly = false,
                DefaultCellStyle = currencyCellFormat,
                MinimumWidth = 100
            });

            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TotalReplacementValue",
                HeaderText = "Total Replacement",
                ReadOnly = true,
                DefaultCellStyle = currencyCellFormat,
                MinimumWidth = 100

            });

            dgInsurancePq.Columns.Add(new DataGridViewComboBoxColumn()
            {
                Name = "BondOriginator",
                DataPropertyName = "BondOriginatorId",
                HeaderText = "Bond Originator",
                ReadOnly = false                
            });

            dgInsurancePq.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "BondOriginatorInterestNoted",
                HeaderText = "Interest Noted",
                ReadOnly = false,
                MinimumWidth = 300
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

                buildingEntity.BuildingRegistrationNumber = txtBuildingRegNum.Text;
                buildingEntity.CSOSRegistrationNumber = txtCSOSRegNum.Text;

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
                throw new Exception("PQ Validation Failed");
            }

            if (_SelectedBroker == null)
            {
                Controller.ShowWarning("Please select an insurance broker");
            }

            if(String.IsNullOrWhiteSpace(txtInsurancePolicyNumber.Text))
            {
                Controller.ShowWarning("Please enter an Insurance Policy Number.");
            }

            if (dtpPolicyExpiryDate.Value <= DateTime.Today)
            {
                Controller.ShowWarning("Policy is set to expire " + dtpPolicyExpiryDate.Value.ToString("yyyy/MM/dd"));
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

                buildingEntity.MonthlyInsurancePremium = string.IsNullOrWhiteSpace(txtMonthlyPremium.Text) ? 0 :
                    Convert.ToDecimal(txtMonthlyPremium.Text);


                buildingEntity.PolicyNumber = txtInsurancePolicyNumber.Text;
                buildingEntity.InsuranceReplacementValueIncludesCommonProperty = cbReplacementIncludesCommonProperty.Checked;
                buildingEntity.InsurancePolicyRenewalDate = dtpPolicyRenewalDate.Value;
                buildingEntity.InsurancePolicyExpiryDate = dtpPolicyExpiryDate.Value;
                buildingEntity.ExcessStructures = tbExcessStructures.Text;
                if (_SelectedBroker == null)
                    buildingEntity.InsuranceBrokerId = null;
                else
                    buildingEntity.InsuranceBrokerId = _SelectedBroker.id;

                decimal totalAdditionalInsuredValue = 0;
                decimal totalAdditionalPremiumValue = 0;
                foreach (var item in InsurancePqGrid.Where(a => a is InsurancePqRecord).Select(a => a as InsurancePqRecord).ToList())
                {
                    totalAdditionalInsuredValue += item.AdditionalInsurance;
                    totalAdditionalPremiumValue += item.AdditionalPremium;
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
                            UnitPremium = item.UnitPremium,
                            AdditionalPremium = item.AdditionalPremium,
                            BondOriginatorId = item.BondOriginatorInterestNoted ? item.BondOriginatorId : null,
                            BondOriginatorInterestNoted = item.BondOriginatorInterestNoted
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
                        update.UnitPremium = item.UnitPremium;
                        update.AdditionalPremium = item.AdditionalPremium;
                        update.BondOriginatorInterestNoted = item.BondOriginatorInterestNoted;
                        if (item.BondOriginatorInterestNoted)
                            update.BondOriginatorId = item.BondOriginatorId;
                        else
                            update.BondOriginatorId = null;
                    }
                }
                buildingEntity.AdditionalInsuredValueCost = totalAdditionalInsuredValue;
                buildingEntity.AdditionalPremiumValue = totalAdditionalPremiumValue;
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
            DisplayPDFNew(null);
            DisplayPDFOld(null);
            tabControl1.SelectedIndex = 0;
            cmbBuilding.SelectedIndex = 0;
            btnViewContract.Enabled = false;
            btnViewInsuranceContract.Enabled = false;
            btnViewClaimForm.Enabled = false;
            btnViewBuildingPlans.Enabled = false;
            btnDownloadBuildingPlans.Enabled = false;
            btnViewPq.Enabled = false;
            btnDownloadPQ.Enabled = false;
            btnUploadInsuranceContract.Enabled = false;
            btnUploadClaimForm.Enabled = false;
            btnBuildingPlans.Enabled = false;
            btnUploadPQ.Enabled = false;
            dgInsurancePq.DataSource = null;
        }

        private void SaveBuilding()
        {
            bool error = false;
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
                        try
                        {
                            SaveWebBuilding(false);
                        }
                        catch (Exception ex1)
                        {
                            error = true;
                            Controller.HandleError("Save Web Building failed: " + ex1.Message + "\r" + ex1.StackTrace, "Buildings");
                        }

                        try
                        {
                            UpdateBuildingSettings();
                        }
                        catch (Exception ex2)
                        {
                            error = true;
                            Controller.HandleError("Update Building Settings failed: " + ex2.Message + "\r" + ex2.StackTrace, "Buildings");
                        }
                        try
                        {
                            SaveBuildingInsurance();
                        }
                        catch (Exception ex3)
                        {
                            error = true;
                            Controller.HandleError("Save Insurance Feailed: " + ex3.Message + "\r" + ex3.StackTrace, "Buildings");
                        }
                        try
                        {
                            SaveTrustees();
                        }
                        catch (Exception ex4)
                        {
                            error = true;
                            Controller.HandleError("Save Trustees Failed: " + ex4.Message + "\r" + ex4.StackTrace, "Buildings");
                        }
                        if (!error)
                        {
                            MessageBox.Show("Building updated!", "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            selectedBuilding = null;
                            clearBuilding();
                            LoadCombo();
                        }
                    }
                    else
                    {
                        Controller.HandleError("Building update failed: SV1 " + status, "Buildings");
                    }
                }
                else
                {
                    Controller.HandleError("Please enter building name", "Buildings");
                }
            }
            catch (Exception ex)
            {
                Controller.HandleError("Building update failed: SV2" + ex.Message + "\r" + ex.StackTrace, "Buildings");
            }
        }

        private void SaveTrustees()
        {

            this.Cursor = Cursors.WaitCursor;
            using (var dbContext = SqlDataHandler.GetDataContext())
            {
                List<Customer> vCustomers = Controller.pastel.AddCustomers(selectedBuilding.Abbr, selectedBuilding.DataPath, true);
                List<Customer> customers = dgTrustees.DataSource as List<Customer>;
                var dbCustomers = dbContext.CustomerSet.Where(a => a.BuildingId == selectedBuilding.ID).ToList();

                foreach (Customer customer in customers)
                {
                    Customer vCustomer = vCustomers.SingleOrDefault(c => c.accNumber == customer.accNumber);

                    var dbCust = dbCustomers.Where(a => a.AccountNumber == customer.accNumber).SingleOrDefault();
                    if (dbCust == null)
                    {
                        dbCust = new Astrodon.Data.CustomerData.Customer()
                        {
                            AccountNumber = customer.accNumber,
                            BuildingId = selectedBuilding.ID,
                            Created = DateTime.Now,
                            IsTrustee = customer.IsTrustee
                        };
                        dbContext.CustomerSet.Add(dbCust);
                    }
                    dbCust.Description = customer.description;
                    dbCust.IsTrustee = customer.IsTrustee;
                    vCustomer.IsTrustee = customer.IsTrustee;
                    dbCust.LoadEmails(vCustomer.Email);
                    dbContext.SaveChanges();

                    UpdateCustomer(vCustomer, dbCust.IsTrustee);
                    if (dbCust.IsTrustee)
                    {
                        if (customer.Email == null || customer.Email.Length == 0)
                        {
                            Controller.HandleError("Trustee " + customer.description + " does not have an email address configured.\r" +
                                "The trustee will not be able to login to the website without an email\r" +
                                "Please configure an email address and try again.");
                        }
                    }
                }
            }

            new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString()).SyncBuildingAndClients(selectedBuilding.ID);

            this.Cursor = Cursors.Arrow;

        }

        private void UpdateCustomer(Customer vCustomer, bool isTrustee)
        {
            /*
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
            */
        }

        private void SaveWebBuilding(bool remove)
        {
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
                        new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString()).SyncBuildingAndClients(selectedBuilding.ID);

                        selectedBuilding.pid = newID;
                        BuildingManager.Update(selectedBuilding, out status);
                        //if (ftpClient.CreateDirectory(websafeName, false))
                        //{
                        //    ftpClient.WorkingDirectory += "/" + websafeName;
                        //    ftpClient.ChangeDirectory(false);
                        //    ftpClient.CreateDirectory("Annual Financial Statements", false);
                        //    ftpClient.CreateDirectory("Conduct Rules", false);
                        //    ftpClient.CreateDirectory("Insurance Information", false);
                        //    ftpClient.CreateDirectory("Meeting Minutes", false);
                        //    ftpClient.CreateDirectory("Meeting Notices", false);
                        //    ftpClient.CreateDirectory("Sectional Title Plans", false);
                        //}
                        //ftpClient.CreateDirectory(websafeName, true);
                    }
                }
                else
                {
                    new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString()).DeleteBuilding(selectedBuilding.ID);
                }
            }
            catch (Exception ex)
            {
                Controller.HandleError(ex);
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
            if (Controller.UserIsSheldon())
            {
                if (selectedBuilding != null)
                {
                    if (MessageBox.Show("Confirm remove building " + selectedBuilding.Name + "?", "Remove building", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        String status = "";
                        BuildingManager.Update(cmbBuilding.SelectedIndex, true, out status);
                        SaveWebBuilding(true);
                        Controller.ShowMessage("Building " + selectedBuilding.Name + " Removed");
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
                        var uploadDate = DateTime.Today;
                        var fileEntity = context.BuildingDocumentSet
                                .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID 
                                                  && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceContract
                                                  && a.DateUploaded == uploadDate);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.InsuranceContract;
                            fileEntity.DateUploaded = uploadDate;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fdOpen.FileName);
                        fileEntity.FileName = Path.GetFileName(fdOpen.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded Insurance Form");
                    }
                    btnViewContract.Enabled = true;
                    btnViewInsuranceContract.Enabled = true;
                }
                catch(Exception ex)
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
            if (fdOpen.ShowDialog() == DialogResult.OK)
            {
                btnUploadClaimForm.Enabled = false;
                try
                {
                    if (!IsValidPdf(fdOpen.FileName))
                    {
                        btnUploadClaimForm.Enabled = true;
                        Controller.HandleError("Not a valid PDF");
                        return;
                    }

                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var uploadDate = DateTime.Today;
                        var fileEntity = context.BuildingDocumentSet
                             .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID 
                                               && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceClaimForm
                                               && a.DateUploaded == uploadDate);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.InsuranceClaimForm;
                            fileEntity.DateUploaded = uploadDate;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fdOpen.FileName);
                        fileEntity.FileName = Path.GetFileName(fdOpen.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded Claim Form");
                    }
                    btnViewClaimForm.Enabled = true;
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
                     .Where(a => a.BuildingId == selectedBuilding.ID
                                       && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceContract)
                     .OrderByDescending(a => a.DateUploaded).FirstOrDefault();
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
                             .Where(a => a.BuildingId == selectedBuilding.ID
                                       && a.DocumentType == Data.InsuranceData.DocumentType.InsuranceClaimForm)
                     .OrderByDescending(a => a.DateUploaded).FirstOrDefault();

                
                    if (fileEntity != null)
                    {
                        fdSave.FileName = fileEntity.FileName;
                        if (fdSave.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(fdSave.FileName, fileEntity.FileData);
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

        private void txtMonthlyPremium_TextChanged(object sender, EventArgs e)
        {
            var d = txtMonthlyPremium.Text;
            if (!string.IsNullOrWhiteSpace(d))
            {
                try
                {
                    var val = Convert.ToDecimal(d);
                    foreach (var x in InsurancePqGrid)
                    {
                        if (x is InsurancePqRecord)
                        {
                            x.BuildingPremium = val;
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
                if (reqItem is PQTotal)
                {
                    row.ReadOnly = true;
                }
                else
                {
                    var comboBox = row.Cells["BondOriginator"] as DataGridViewComboBoxCell;
                    comboBox.ReadOnly = false;
                    comboBox.DataSource = reqItem.BondOriginatorList;
                    comboBox.DisplayMember = "CompanyName";
                    comboBox.ValueMember = "id";

                  //  comboBox.Value = reqItem.BondOriginatorId;
                }

            }
        }

        private void btnBuildingPlans_Click(object sender, EventArgs e)
        {
            if (fdOpen.ShowDialog() == DialogResult.OK)
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
                        var uploadDate = DateTime.Today;

                        var fileEntity = context.BuildingDocumentSet
                             .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID 
                                               && a.DocumentType == Data.InsuranceData.DocumentType.BuildingPlans
                                               && a.DateUploaded == uploadDate);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.BuildingPlans;
                            fileEntity.DateUploaded = uploadDate;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fdOpen.FileName);
                        fileEntity.FileName = Path.GetFileName(fdOpen.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded Building Plans");
                    }
                    btnViewBuildingPlans.Enabled = true;
                    btnDownloadBuildingPlans.Enabled = true;
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
                         .Where(a => a.BuildingId == selectedBuilding.ID
                                       && a.DocumentType == Data.InsuranceData.DocumentType.BuildingPlans)
                     .OrderByDescending(a => a.DateUploaded).FirstOrDefault();

                    if (fileEntity != null)
                    {
                        fdSave.FileName = fileEntity.FileName;
                        if (fdSave.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(fdSave.FileName, fileEntity.FileData);
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
            if (fdOpen.ShowDialog() == DialogResult.OK)
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
                        var uploadDate = DateTime.Today;
                        var fileEntity = context.BuildingDocumentSet
                             .FirstOrDefault(a => a.BuildingId == selectedBuilding.ID 
                                               && a.DocumentType == Data.InsuranceData.DocumentType.PQ
                                               && a.DateUploaded == uploadDate);
                        if (fileEntity == null)
                        {
                            fileEntity = new Data.InsuranceData.BuildingDocument();
                            fileEntity.BuildingId = selectedBuilding.ID;
                            fileEntity.DocumentType = Data.InsuranceData.DocumentType.PQ;
                            fileEntity.DateUploaded = uploadDate;
                            context.BuildingDocumentSet.Add(fileEntity);
                        }
                        fileEntity.FileData = File.ReadAllBytes(fdOpen.FileName);
                        fileEntity.FileName = Path.GetFileName(fdOpen.FileName);
                        context.SaveChanges();
                        MessageBox.Show("Successfully Uploaded PQ");
                    }
                    btnViewPq.Enabled = true;
                    btnDownloadPQ.Enabled = true;
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
                      .Where(a => a.BuildingId == selectedBuilding.ID
                                    && a.DocumentType == Data.InsuranceData.DocumentType.PQ)
                  .OrderByDescending(a => a.DateUploaded).FirstOrDefault();

                    if (fileEntity != null)
                    {
                        fdSave.FileName = fileEntity.FileName;
                        if (fdSave.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(fdSave.FileName, fileEntity.FileData);
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


        private string _TempPDFNewFile = string.Empty;

        private void DisplayPDFNew(byte[] pdfData)
        {
            if (pdfData == null)
            {
                this.axAcroPDFNew.Visible = false;
                return;
            }
            if (!String.IsNullOrWhiteSpace(_TempPDFNewFile))
                File.Delete(_TempPDFNewFile);
            _TempPDFNewFile = Path.GetTempPath();
            if (!_TempPDFNewFile.EndsWith(@"\"))
                _TempPDFNewFile = _TempPDFNewFile + @"\";

            _TempPDFNewFile = _TempPDFNewFile + System.Guid.NewGuid().ToString("N") + ".pdf";
            File.WriteAllBytes(_TempPDFNewFile, pdfData);


            try
            {
                this.axAcroPDFNew.Visible = true;
                this.axAcroPDFNew.LoadFile(_TempPDFNewFile);
                this.axAcroPDFNew.src = _TempPDFNewFile;
                this.axAcroPDFNew.setShowToolbar(false);
                this.axAcroPDFNew.setView("FitH");
                this.axAcroPDFNew.setLayoutMode("SinglePage");
                this.axAcroPDFNew.setShowToolbar(false);

                this.axAcroPDFNew.Show();
                tabControl1.SelectedTab = tbPDFViewer;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            File.Delete(_TempPDFNewFile);
        }

        private string _TempPDFOldFile = string.Empty;

        private void DisplayPDFOld(byte[] pdfData)
        {
            if (pdfData == null)
            {
                this.axAcroPDFOld.Visible = false;
                return;
            }
            if (!String.IsNullOrWhiteSpace(_TempPDFOldFile))
                File.Delete(_TempPDFOldFile);
            _TempPDFOldFile = Path.GetTempPath();
            if (!_TempPDFOldFile.EndsWith(@"\"))
                _TempPDFOldFile = _TempPDFOldFile + @"\";

            _TempPDFOldFile = _TempPDFOldFile + System.Guid.NewGuid().ToString("N") + ".pdf";
            File.WriteAllBytes(_TempPDFOldFile, pdfData);


            try
            {
                this.axAcroPDFOld.Visible = true;
                this.axAcroPDFOld.LoadFile(_TempPDFOldFile);
                this.axAcroPDFOld.src = _TempPDFOldFile;
                this.axAcroPDFOld.setShowToolbar(false);
                this.axAcroPDFOld.setView("FitH");
                this.axAcroPDFOld.setLayoutMode("SinglePage");
                this.axAcroPDFOld.setShowToolbar(false);

                this.axAcroPDFOld.Show();
                tabControl1.SelectedTab = tbPDFViewer;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            File.Delete(_TempPDFOldFile);
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
            catch(Exception ex)
            {
                Controller.HandleError(filepath + " reader error " + ex.Message);
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
                     .Where(a => a.BuildingId == selectedBuilding.ID && a.DocumentType == documentType)
                     .OrderByDescending(a => a.DateUploaded)
                     .Take(2).ToList();

                    if (fileEntity != null && fileEntity.Count >= 0)
                    {
                        DisplayPDFNew(fileEntity[0].FileData);
                    }
                    else
                    {
                        DisplayPDFNew(null);
                        MessageBox.Show("No document exits");
                        return;
                    }

                    if (fileEntity != null && fileEntity.Count >= 2)
                    {
                        DisplayPDFOld(fileEntity[1].FileData);
                    }
                    else
                    {
                        DisplayPDFOld(null);
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

        private void dgInsurancePq_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (InsurancePqGrid != null && InsurancePqGrid.Count > 1)
            {
                if (InsurancePqGrid.Any(a => a is InsurancePqRecord))
                {
                    txtAdditionalInsuredValue.Text = InsurancePqGrid.Where(a => a is InsurancePqRecord).Sum(a => a.AdditionalInsurance).ToString("#,##0.00");
                    txtAdditionalPremium.Text = InsurancePqGrid.Where(a => a is InsurancePqRecord).Sum(a => a.AdditionalPremium).ToString("#,##0.00");
                }

                if (InsurancePqGrid.Any(a => a is PQTotal))
                    txtTotalReplacementValue.Text = InsurancePqGrid.First(a => a is PQTotal).TotalReplacementValue.ToString("#,##0.00");
            }
        }
        private string cbReplacementIncludesCommonPropertyValue { get; set; }
        private void cbReplacementIncludesCommonProperty_CheckedChanged(object sender, EventArgs e)
        {
            if (cbReplacementIncludesCommonProperty.Checked)
            {
                cbReplacementIncludesCommonPropertyValue = txtCommonPropertyValue.Text;
                txtCommonPropertyValue.Text = 0.ToString("#,##0.00");
            }
            else
            {
                txtCommonPropertyValue.Text = cbReplacementIncludesCommonPropertyValue;
            }
            txtCommonPropertyValue.Enabled = !cbReplacementIncludesCommonProperty.Checked;
        }

        private void dtpPolicyRenewalDate_ValueChanged(object sender, EventArgs e)
        {
            dtpPolicyExpiryDate.Value = dtpPolicyRenewalDate.Value.AddMonths(12);
        }
    }

    internal interface IInsurancePqRecord
    {

        string UnitNo { get; set; }

        decimal SquareMeters { get; set; }

        decimal AdditionalInsurance { get; set; }

        decimal AdditionalPremium { get; set; }

        string Notes { get; set; }

        decimal TotalUnitPropertyDimensions { get; set; }

        decimal PQCalculated { get; }

        decimal PQCalculatedPercentage { get; }

        decimal BuildingReplacementValue { get; set; }
        decimal UnitReplacementCost { get; }
        decimal TotalReplacementValue { get; }
        decimal UnitPremium { get; }
        decimal TotalPremium { get; }

        decimal BuildingPremium { get; set; }

        DataGridViewRow DataRow { get; set; }

        bool BondOriginatorInterestNoted { get; set; }
        int? BondOriginatorId { get; set; }

        BondOriginator SelectedBondOriginator { get; set; }

        List<BondOriginator> BondOriginatorList { get; set; }

    }

    internal class InsurancePqRecord: IInsurancePqRecord
    {
        protected List<IInsurancePqRecord> _Items;
        public InsurancePqRecord(List<IInsurancePqRecord> items, List<BondOriginator> bondOriginators)
        {
            _Items = items;
            BondOriginatorList = bondOriginators.ToList();
        }

        public int? Id { get; set; }
        public string UnitNo { get; set; }

        private decimal _SquareMeters;
        public decimal SquareMeters { get { return _SquareMeters; } set { _SquareMeters = value; Calculate(); } }

        private decimal _AdditionalInsurance;
        public decimal AdditionalInsurance { get { return _AdditionalInsurance; } set { _AdditionalInsurance = value; Calculate(); } }

        private decimal _AdditionalPremium;
        public decimal AdditionalPremium { get { return _AdditionalPremium; } set { _AdditionalPremium = value; Calculate(); } }

        public string Notes { get; set; }

        private decimal _TotalUnitPropertyDimensions;
        public decimal TotalUnitPropertyDimensions { get { return _TotalUnitPropertyDimensions; } set { _TotalUnitPropertyDimensions = value; Calculate(); } }

        public virtual decimal PQCalculated
        {
            get
            {
                if (TotalUnitPropertyDimensions <= 0)
                    return 0;
                return Math.Round((SquareMeters / TotalUnitPropertyDimensions), 8);
            }
        }

        public virtual decimal PQCalculatedPercentage
        {
            get
            {
                if (PQCalculated <= 0)
                    return 0;
                return Math.Round(PQCalculated * 100.0000M, 4);
            }
        }

        private decimal _BuildingReplacementValue;

        public decimal BuildingReplacementValue { get { return _BuildingReplacementValue; } set { _BuildingReplacementValue = value; Calculate(); } }

        private decimal _BuildingPremiumValue;
        public decimal BuildingPremium { get { return _BuildingPremiumValue; } set { _BuildingPremiumValue = value; Calculate(); } }

        public virtual decimal UnitReplacementCost { get { return Math.Round(BuildingReplacementValue * PQCalculated, 2); } }
        public virtual decimal TotalReplacementValue { get { return AdditionalInsurance + UnitReplacementCost; } }

        private decimal _UnitPremium = 0;
        public decimal UnitPremium { get { return _UnitPremium; }  }
        
        public DataGridViewRow DataRow { get; set; }

        public bool BondOriginatorInterestNoted { get;  set; }
        public int? BondOriginatorId { get;  set; }

        public BondOriginator SelectedBondOriginator
        {
            get
            {
                if (BondOriginatorId == null)
                    return null;
                return BondOriginatorList.Single(a => a.id == BondOriginatorId.Value);
            }
            set
            {
                if (value == null)
                    BondOriginatorId = null;
                else
                    BondOriginatorId = value.id;
            }
        }

        public List<BondOriginator> BondOriginatorList { get; set; }

        public decimal TotalPremium
        {
            get
            {
                return UnitPremium + AdditionalPremium;
            }
        }

        private bool _InCalculate = false;

        public decimal CalculatePremium()
        {
            var insureTemp = _BuildingPremiumValue * PQCalculated; 

            if (insureTemp < 0)
                return 0;
            _UnitPremium = Math.Round(insureTemp, 2);

            if (DataRow != null)
            {
                foreach (var c in DataRow.Cells)
                {
                    var cell = c as DataGridViewCell;
                    if (cell != null)
                    {
                        if(cell.OwningColumn.DataPropertyName == "UnitPremium")
                           DataRow.DataGridView.InvalidateCell(cell);
                    }
                }
            }

            return _UnitPremium;
        }

        public virtual void Calculate()
        {
            if (_InCalculate)
                return;
            try
            {
                _InCalculate = true;

                CalculatePremium();
                foreach(var itm in _Items)
                {
                    if (itm is InsurancePqRecord)
                        (itm as InsurancePqRecord).CalculatePremium();
                }

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
            finally
            {
                _InCalculate = false;
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
            AdditionalPremium = _Items.Where(a => a != this).Sum(a => a.AdditionalPremium);
            TotalUnitPropertyDimensions = _Items.Where(a => a != this).Sum(a => a.TotalUnitPropertyDimensions);
            BuildingReplacementValue = _Items.Where(a => a != this).Sum(a => a.BuildingReplacementValue);
            UnitPremium = _Items.Where(a => a != this).Sum(a => a.UnitPremium);
            PQCalculatedPercentage = _Items.Where(a => a != this).Sum(a => a.PQCalculatedPercentage);
            BondOriginatorList = null;
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

        public decimal AdditionalPremium { get; set; }

        public decimal TotalUnitPropertyDimensions { get; set; }

        public decimal BuildingReplacementValue { get; set; }

        public decimal BuildingPremium { get; set; }

        public string Notes { get; set; }

        public string UnitNo { get; set; }

        public decimal UnitPremium { get; set; }

        public DataGridViewRow DataRow { get; set; }

        public bool BondOriginatorInterestNoted { get; set; }

        public int? BondOriginatorId { get; set; }

        public List<BondOriginator> BondOriginatorList { get; set; }

        public BondOriginator SelectedBondOriginator { get; set; }

        public decimal TotalPremium
        {
            get
            {
                return UnitPremium + AdditionalPremium;
            }
        }

        public decimal PQCalculatedPercentage { get; set; }
    }
}
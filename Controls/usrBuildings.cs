using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class usrBuildings : UserControl
    {
        private Buildings BuildingManager;
        private Building selectedBuilding = null;
        private List<InsurancePqRecord> InsurancePqGrid { get; set; }

        public usrBuildings()
        {
            InitializeComponent();
        }

        private void usrBuildings_Load(object sender, EventArgs e)
        {
            LoadCombo();
            clearBuilding();
        }

        private void LoadCombo()
        {
            BuildingManager = new Buildings(true);
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
            btnSave.Enabled = true;
        }

        private void LoadBuildingInsurance()
        {
            Data.tblBuilding buildingEntity = null;
            using (var context = SqlDataHandler.GetDataContext())
            {
                buildingEntity = context.tblBuildings
                        .FirstOrDefault(a => a.id == selectedBuilding.ID);
            }
            if (buildingEntity != null)
            {
                txtCommonPropertyDim.Text = buildingEntity.CommonPropertyDimensions.ToString();
                txtUnitPropertyDim.Text = buildingEntity.UnitPropertyDimensions.ToString();
                txtReplacementValue.Text = buildingEntity.UnitReplacementCost.ToString("#,##0.00");
                txtCommonPropertyValue.Text = buildingEntity.CommonPropertyReplacementCost.ToString("#,##0.00");
                txtBrokerCompany.Text = buildingEntity.InsuranceCompanyName;
                txtBrokerAccountNumber.Text = buildingEntity.InsuranceAccountNumber;
                txtBrokerName.Text = buildingEntity.BrokerName;
                txtBrokerTel.Text = buildingEntity.BrokerTelNumber;
                txtBrokerEmail.Text = buildingEntity.BrokerEmail;
                LoadInsuranceUnitPq(buildingEntity);
            }
        }

        private void LoadInsuranceUnitPq(Data.tblBuilding buildingEntity)
        {
            var unitRecords = Controller.pastel.AddCustomers(buildingEntity.Code, buildingEntity.DataPath);
            InsurancePqGrid = unitRecords.Select(a => new InsurancePqRecord()
            {
                UnitNo = a.accNumber,
                SquareMeters = decimal.Round(buildingEntity.UnitPropertyDimensions / unitRecords.Count, 2),
                Notes = "*New Unit*",
                TotalUnitPropertyDimensions = buildingEntity.UnitPropertyDimensions
            }).ToList();
            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildingUnits = context.BuildingUnitSet
                        .Where(a => a.BuildingId == selectedBuilding.ID).ToList();
                foreach (var unit in buildingUnits)
                {
                    var record = InsurancePqGrid.FirstOrDefault(a => a.UnitNo == unit.UnitNo);
                    if (record != null)
                    {
                        record.Id = unit.id;
                        record.SquareMeters = unit.SquareMeters;
                        record.Notes = unit.Notes;
                        record.PQRating = unit.PQRating;
                        record.AdditionalInsurance = unit.AdditionalInsurance;
                    }
                }
            }
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
                HeaderText = "SquareMeters",
                ReadOnly = false,
            });
            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AdditionalInsurance",
                HeaderText = "AdditionalInsurance",
                ReadOnly = false
            });
            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Notes",
                HeaderText = "Notes",
                ReadOnly = false,
            });
            dgInsurancePq.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PQCalculated",
                HeaderText = "PQCalculated",
                ReadOnly = true
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

        private void SaveBuildingInsurance()
        {
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
                buildingEntity.InsuranceCompanyName = txtBrokerCompany.Text;
                buildingEntity.InsuranceAccountNumber = txtBrokerAccountNumber.Text;
                buildingEntity.BrokerName = txtBrokerName.Text;
                buildingEntity.BrokerTelNumber = txtBrokerTel.Text;
                buildingEntity.BrokerEmail = txtBrokerEmail.Text;

                foreach (var item in InsurancePqGrid)
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

                context.SaveChanges();
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
                    try
                    {
                        SaveBuildingInsurance();
                    }
                    catch (Exception)
                    {
                    }
                    if (BuildingManager.Update(cmbBuilding.SelectedIndex, false, out status))
                    {
                        SaveWebBuilding(false);
                        SaveBuildingInsurance();
                        MessageBox.Show("Building updated!", "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedBuilding = null;
                        clearBuilding();
                        LoadCombo();
                    }
                    else
                    {
                        MessageBox.Show("Building update failed: " + status, "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter building name", "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show("Building update failed", "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }

    class InsurancePqRecord
    {
        public int? Id { get; set; }
        public string UnitNo { get; set; }
        public decimal SquareMeters { get; set; }
        public decimal AdditionalInsurance { get; set; }
        public string Notes { get; set; }
        public decimal? PQRating { get; set; }

        public decimal TotalUnitPropertyDimensions { get; set; }
        public decimal PQCalculated
        {
            get
            {
                return PQRating == null ? SquareMeters / TotalUnitPropertyDimensions : PQRating.Value;
            }
        }
    }
}
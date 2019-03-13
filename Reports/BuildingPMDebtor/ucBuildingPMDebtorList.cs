using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using ClosedXML.Excel;
using Astrodon.Classes;
using ExcelExportExample.ExcelHelper;
using System.IO;
using System.Diagnostics;

namespace Astrodon.Reports.BuildingPMDebtor
{
    public partial class ucBuildingPMDebtorList : UserControl
    {
        private int? selectedPortfolioManagerId;
        private int? selectedDebtorId;

        private List<BuildingPMDebtorResult> _BuildingPMDebtorResultList { get; set; }

        private List<KeyValuePair<int?, string>> _PortfolioManagers { get; set; }

        private List<KeyValuePair<int?, string>> _Debtors { get; set; }

        public ucBuildingPMDebtorList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                _BuildingPMDebtorResultList = (from b in context.tblBuildings
                                               join pmTemp in context.tblUsers on b.pm equals pmTemp.email into pmJoin
                                               from pm in pmJoin.DefaultIfEmpty()
                                               join ubTemp in context.tblUserBuildings on b.id equals ubTemp.buildingid into ubJoin
                                               from ub in ubJoin.DefaultIfEmpty()
                                               join debtorTemp in context.tblUsers on ub.userid equals debtorTemp.id into debtorJoin
                                               from debtor in debtorJoin.DefaultIfEmpty()
                                               join cTemp in context.CustomerSet on b.id equals cTemp.BuildingId into cJoin
                                               from c in cJoin.DefaultIfEmpty()

                                               where b.BuildingDisabled == false

                                               group c by new
                                               {
                                                   BuildingId = b.id,
                                                   BuildingName = b.Building,
                                                   ABR = b.Code,
                                                   BuildingRegistrationNumber = b.BuildingRegistrationNumber,
                                                   CSOSRegistrationNumber = b.CSOSRegistrationNumber,

                                                   Units = c.BuildingId,
                                                   YearEndPeriod = b.Period,
                                                   Code = b.AccNumber,
                                                   DataPath = b.DataPath,

                                                   PortfolioManagerId = (pm == null) ? 0 : pm.id,
                                                   PortfolioManager = (pm == null) ? "" : pm.name,
                                                   PortfolioManagerEmail = (pm == null) ? "" : pm.email,

                                                   DebtorId = debtor.id,
                                                   Debtor = debtor.name,
                                                   DebtorEmail = debtor.email,

                                                   Bank = b.bankName,
                                                   AccountNumber = b.bankAccNumber,
                                                   BranchCode = b.branch,

                                                   AddressLine1 = b.addy1,
                                                   AddressLine2 = b.addy2,
                                                   AddressLine3 = b.addy3,
                                                   AddressLine4 = b.addy4,
                                                   AddressLine5 = b.addy5,
                                                   ODBCConnectionOK = b.ODBCConnectionOK == true ? "Yes" : "No"
                                               } into grp

                                               orderby grp.Key.BuildingName

                                               select new BuildingPMDebtorResult()
                                               {
                                                   BuildingId = grp.Key.BuildingId,
                                                   BuildingName = grp.Key.BuildingName,
                                                   ABR = grp.Key.ABR,
                                                   BuildingRegistrationNumber = grp.Key.BuildingRegistrationNumber,
                                                   CSOSRegistrationNumber = grp.Key.CSOSRegistrationNumber,

                                                   Units = grp.Count(t => t != null),
                                                   YearEndPeriod = grp.Key.YearEndPeriod,
                                                   Code = grp.Key.Code,
                                                   DataPath = grp.Key.DataPath,
                                                   ODBCConnectionOK = grp.Key.ODBCConnectionOK,

                                                   PortfolioManagerId = grp.Key.PortfolioManagerId,
                                                   PortfolioManager = grp.Key.PortfolioManager,
                                                   PortfolioManagerEmail = grp.Key.PortfolioManagerEmail,

                                                   DebtorId = grp.Key.DebtorId,
                                                   Debtor = grp.Key.Debtor,
                                                   DebtorEmail = grp.Key.DebtorEmail,

                                                   Bank = grp.Key.Bank,
                                                   AccountNumber = grp.Key.AccountNumber,
                                                   BranchCode = grp.Key.BranchCode,

                                                   AddressLine1 = grp.Key.AddressLine1,
                                                   AddressLine2 = grp.Key.AddressLine2,
                                                   AddressLine3 = grp.Key.AddressLine3,
                                                   AddressLine4 = grp.Key.AddressLine4,
                                                   AddressLine5 = grp.Key.AddressLine5
                                               }).ToList();
            }

            _PortfolioManagers = _BuildingPMDebtorResultList.Where(a => !String.IsNullOrWhiteSpace(a.PortfolioManager))
                                                            .Select(a => new KeyValuePair<int?, string>(a.PortfolioManagerId, a.PortfolioManager))
                                                            .Distinct()
                                                            .OrderBy(a => a.Value)
                                                            .ToList();

            _PortfolioManagers.Insert(0, new KeyValuePair<int?, string>(null, "All"));

            _Debtors = _BuildingPMDebtorResultList.Select(a => new KeyValuePair<int?, string>(a.DebtorId, a.Debtor))
                                                  .Distinct()
                                                  .OrderBy(a => a.Value)
                                                  .ToList();

            _Debtors.Insert(0, new KeyValuePair<int?, string>(null, "All"));

            cbPMDropDown.DataSource = _PortfolioManagers;
            cbPMDropDown.DisplayMember = "Value";
            cbPMDropDown.ValueMember = "Key";

            cbDebtorDropDown.DataSource = _Debtors;
            cbDebtorDropDown.DisplayMember = "Value";
            cbDebtorDropDown.ValueMember = "Key";

            BindDataGrid();
        }

        private void BindDataGrid()
        {

            buildingPMDebtorGridView.ClearSelection();
            buildingPMDebtorGridView.MultiSelect = false;
            buildingPMDebtorGridView.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();

            bs.DataSource = FilterData();

            buildingPMDebtorGridView.Columns.Clear();

            buildingPMDebtorGridView.DataSource = bs;

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingId",
                HeaderText = "Building ID",
                ReadOnly = true,
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingName",
                HeaderText = "Building Name",
                ReadOnly = true,
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ABR",
                HeaderText = "ABR",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingRegistrationNumber",
                HeaderText = "Building Reg Number",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CSOSRegistrationNumber",
                HeaderText = "CSOS Reg Number",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Units",
                HeaderText = "Units",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "YearEnd",
                HeaderText = "Year End",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Code",
                HeaderText = "Code",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DataPath",
                HeaderText = "Pastel Folder",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ODBCConnectionOK",
                HeaderText = "ODBC Ok",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PortfolioManager",
                HeaderText = "PM",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PortfolioManagerEmail",
                HeaderText = "PM Email",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Debtor",
                HeaderText = "Debtor",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DebtorEmail",
                HeaderText = "Debtor Email",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Bank",
                HeaderText = "Bank",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountNumber",
                HeaderText = "Account Number",
                ReadOnly = true
            });
            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BranchCode",
                HeaderText = "Branch Code",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AddressLine1",
                HeaderText = "Address 1",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AddressLine2",
                HeaderText = "Address 2",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AddressLine3",
                HeaderText = "Address 3",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AddressLine4",
                HeaderText = "Address 4",
                ReadOnly = true
            });

            buildingPMDebtorGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AddressLine5",
                HeaderText = "Address 5",
                ReadOnly = true
            });

        }

        private void cbPMDropDown_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedPortfolioManagerId = cbPMDropDown.SelectedValue as int?;
            BindDataGrid();
        }

        private void cbDebtorDropDown_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedDebtorId = cbDebtorDropDown.SelectedValue as int?;
            BindDataGrid();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files | *.xlsx";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        if (sfd.FileName != "" && sfd.FileName.EndsWith(".xlsx"))
                        {
                            var excelProvider = new ExcelProvider();

                            var fileBytes = excelProvider.ExportQuery("BuildingPMDebtor", FilterData(), new ExcelStyleSheet());

                            File.WriteAllBytes(sfd.FileName, fileBytes);

                            Process.Start(sfd.FileName);
                            // MessageBox.Show("Saved Succesfully", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private IQueryable<BuildingPMDebtorResult> FilterData()
        {
            var filteredData = _BuildingPMDebtorResultList.AsEnumerable();
            if (selectedPortfolioManagerId != null)
                filteredData = filteredData.Where(a => a.PortfolioManagerId == selectedPortfolioManagerId);
            if (selectedDebtorId != null)
                filteredData = filteredData.Where(a => a.DebtorId == selectedDebtorId);

            return filteredData.AsQueryable();
        }
    }
}


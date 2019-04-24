using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelExportExample.ExcelHelper;
using System.IO;
using System.Diagnostics;
using Astro.Library.Entities;

namespace Astrodon.Reports.TrusteeReport
{
    public partial class ucTrusteeReport : UserControl
    {
        List<Building> _Buildings { get; set; }
        List<TrusteeReportModel> _TrusteeReportResults { get; set; }

        public ucTrusteeReport()
        {
            InitializeComponent();
            LoadBuildings();
        }

        private void LoadData(int buildingId)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                _TrusteeReportResults = (from b in context.tblBuildings
                                         join cTemp in context.CustomerSet on b.id equals cTemp.BuildingId into cJoin
                                         from c in cJoin.DefaultIfEmpty()

                                         where c.IsTrustee == true && (b.id == buildingId || buildingId == 0)

                                         select new TrusteeReportModel()
                                         {
                                             Code = b.Code,
                                             BuildingName = b.Building,
                                             Portfolio = (c.Portfolio == null) ? "" : c.Portfolio,
                                             AccountNumber = c.AccountNumber,
                                             CustomerFullName = (c.CustomerFullName == null) ? "" : c.CustomerFullName,
                                             CellNumber = c.CellNumber,
                                             EmailAddress = c.EmailAddress1
                                         }).ToList();
            }

            BindDataGrid();
        }

        private void BindDataGrid()
        {

            dgvTrusteeReport.ClearSelection();
            dgvTrusteeReport.MultiSelect = false;
            dgvTrusteeReport.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();

            bs.DataSource = _TrusteeReportResults;

            dgvTrusteeReport.Columns.Clear();

            dgvTrusteeReport.DataSource = bs;

            dgvTrusteeReport.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Code",
                HeaderText = "Code",
                ReadOnly = true,
            });

            dgvTrusteeReport.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingName",
                HeaderText = "Building Name",
                ReadOnly = true,
            });

            dgvTrusteeReport.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Portfolio",
                HeaderText = "Portfolio",
                ReadOnly = true
            });

            dgvTrusteeReport.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountNumber",
                HeaderText = "Account Number",
                ReadOnly = true
            });

            dgvTrusteeReport.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CustomerFullName",
                HeaderText = "Customer Full Name",
                ReadOnly = true
            });

            dgvTrusteeReport.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CellNumber",
                HeaderText = "Cell Number",
                ReadOnly = true
            });

            dgvTrusteeReport.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "EmailAddress",
                HeaderText = "Email Address",
                ReadOnly = true
            });
        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var userid = Controller.user.id;
                Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
                _Buildings = bManager.buildings.ToList();
                _Buildings.Insert(0, new Building() { Name = "All Buildings", ID = 0 });
                cmbBuilding.DataSource = _Buildings;
                cmbBuilding.ValueMember = "ID";
                cmbBuilding.DisplayMember = "Name";
                if (_Buildings.Count > 0)
                    cmbBuilding.SelectedIndex = 0;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            Building selectedBuildingId = cmbBuilding.SelectedItem as Building;
            LoadData(selectedBuildingId.ID);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files | *.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (sfd.FileName != "" && sfd.FileName.EndsWith(".xlsx"))
                        {
                            var excelProvider = new ExcelProvider();

                            var fileBytes = excelProvider.ExportQuery("Trustee Report List", _TrusteeReportResults.AsQueryable(), new ExcelStyleSheet());

                            File.WriteAllBytes(sfd.FileName, fileBytes);

                            Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

}

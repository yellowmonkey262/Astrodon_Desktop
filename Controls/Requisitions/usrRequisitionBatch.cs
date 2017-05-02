using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astro.Library.Entities;
using Astrodon.Data.RequisitionData;
using Astrodon.ReportService;
using System.IO;
using System.Diagnostics;

namespace Astrodon.Controls.Requisitions
{
    public partial class usrRequisitionBatch : UserControl
    {
        private List<Building> _Buildings;
        private List<BatchItem> _Data;

        public usrRequisitionBatch()
        {
            InitializeComponent();
            _Data = new List<BatchItem>();
            LoadBuildings();
            LoadGrid();
        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var userid = Controller.user.id;
                Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
                _Buildings = bManager.buildings;
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var building = cmbBuilding.SelectedItem as Building;
                var batch = CreateRequisitionBatch(building.ID);
                LoadGrid();
                DownloadReport(batch.id);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnDownload.Enabled = true;
            }
        }

        private RequisitionBatch CreateRequisitionBatch(int buildingId)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                int batchNumber = 0;
                var requisitions = context.tblRequisitions.Where(a => a.building == buildingId && a.processed == false).ToList();
                if (requisitions.Count <= 0)
                {
                    Controller.ShowMessage("There are no outstanding requisitions to process.");
                    return null;
                }
                var previousBatch = context.RequisitionBatchSet.Where(a => a.BuildingId == buildingId).OrderByDescending(a => a.BatchNumber).FirstOrDefault();
                if (previousBatch == null)
                    batchNumber = 1;
                else
                    batchNumber = previousBatch.BatchNumber + 1;

                //find all requisitions

                var batch = new RequisitionBatch()
                {
                    BuildingId = buildingId,
                    BatchNumber = batchNumber,
                    UserId = Controller.user.id,
                    Created = DateTime.Now,
                    Entries = requisitions.Count()
                };

                foreach (var requisition in requisitions)
                {
                    requisition.RequisitionBatch = batch;
                    requisition.processed = true;
                }

                context.RequisitionBatchSet.Add(batch);

                context.SaveChanges();

                return batch;


            }
        }

        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();

            dgItems.DataSource = bs;

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Download",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Created",
                HeaderText = "Created",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
                MinimumWidth = 80
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CreatedBy",
                HeaderText = "Created By",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
                MinimumWidth = 80
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Building",
                HeaderText = "Building",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BatchNumber",
                HeaderText = "BatchNumber",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Entries",
                HeaderText = "Entries",
                ReadOnly = true
            });


            dgItems.AutoResizeColumns();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var item = senderGrid.Rows[e.RowIndex].DataBoundItem as BatchItem;
                DownloadReport(item.Id);

            }
        }

        private void LoadGrid()
        {
            var building = cmbBuilding.SelectedItem as Building;
            using (var context = SqlDataHandler.GetDataContext())
            {
                _Data = context.RequisitionBatchSet
                        .Where(a => a.BuildingId == building.ID)
                        .Select(b => new BatchItem()
                        {
                            Id = b.id,
                            Building = b.Building.Building,
                            Created = b.Created,
                            BatchNumber = b.BatchNumber,
                            Entries = b.Entries,
                            CreatedBy = b.UserCreated.name
                        }).OrderByDescending(a => a.Created).Take(200).ToList();
            }
            BindDataGrid();
        }

        private void DownloadReport(int requisitionBatchId)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                using (var reportService = new ReportServiceClient())
                {
                    var reportData = reportService.RequisitionBatchReport(requisitionBatchId);
                    File.WriteAllBytes(dlgSave.FileName, reportData);
                    Process.Start(dlgSave.FileName);
                }
            }
        }
    }

    class BatchItem
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public DateTime Created { get; set; }
        public int BatchNumber { get; set; }
        public int Entries { get; set; }
        public string CreatedBy { get;  set; }
    }
}

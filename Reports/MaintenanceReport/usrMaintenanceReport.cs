using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data.Base;
using System.Globalization;
using Astrodon.ReportService;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Astro.Library.Entities;

namespace Astrodon.Reports.MaintenanceReport
{
    public partial class usrMaintenanceReport : UserControl
    {
        private List<Building> _Buildings;
        private List<IdValue> _FromYears;
        private List<IdValue> _FromMonths;

        private List<IdValue> _ToYears;
        private List<IdValue> _ToMonths;


        public usrMaintenanceReport()
        {
            InitializeComponent();
            lbFinancialYear.Text = "";
            LoadBuildings();
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

        private void LoadYears(DateTime dtStart, DateTime dtEnd)
        {
            _FromYears = new List<IdValue>();
            _FromYears.Add(new IdValue() { Id = DateTime.Now.Year - 1, Value = (DateTime.Now.Year - 1).ToString() });
            _FromYears.Add(new IdValue() { Id = DateTime.Now.Year, Value = (DateTime.Now.Year).ToString() });
            _FromYears.Add(new IdValue() { Id = DateTime.Now.Year + 1, Value = (DateTime.Now.Year + 1).ToString() });

            _FromMonths = new List<IdValue>();
            int startMonth = dtStart.Month;
            for (int x = 1; x <= 12; x++)
            {
                _FromMonths.Add(new IdValue()
                {
                    Id = x,
                    Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x)
                });
              
            }

            cmbFromYear.DataSource = _FromYears;
            cmbFromYear.ValueMember = "Id";
            cmbFromYear.DisplayMember = "Value";
            cmbFromYear.SelectedValue = dtStart.Year;

            cmbFromMonth.DataSource = _FromMonths;
            cmbFromMonth.ValueMember = "Id";
            cmbFromMonth.DisplayMember = "Value";
            cmbFromMonth.SelectedValue = dtStart.Month;

            _ToYears = _FromYears.ToList();
            _ToMonths = _FromMonths.ToList();

            _ToMonths = new List<IdValue>();
            for (int x = 1; x <= 12; x++)
            {
                _ToMonths.Add(new IdValue()
                {
                    Id = x,
                    Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x)
                });
            
            }

            cmbToYear.DataSource = _ToYears;
            cmbToYear.ValueMember = "Id";
            cmbToYear.DisplayMember = "Value";
            cmbToYear.SelectedValue = dtEnd.Year;

            cmbToMonth.DataSource = _ToMonths;
            cmbToMonth.ValueMember = "Id";
            cmbToMonth.DisplayMember = "Value";
            cmbToMonth.SelectedValue = dtEnd.Month;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                button1.Enabled = false;

                try
                {
                    using (var reportService = ReportServiceClient.CreateInstance())
                    {
                        DateTime startDate = new DateTime((cmbFromYear.SelectedItem as IdValue).Id, (cmbFromMonth.SelectedItem as IdValue).Id, 1);
                        DateTime endDate = new DateTime((cmbToYear.SelectedItem as IdValue).Id, (cmbToMonth.SelectedItem as IdValue).Id, 1).AddMonths(1).AddSeconds(-1);

                        MaintenanceReportType repType;
                        if (rbDetailed.Checked)
                            repType = MaintenanceReportType.DetailedReport;
                        else if (rbDetailWithDocs.Checked)
                            repType = MaintenanceReportType.DetailedReportWithSupportingDocuments;
                        else
                            repType = MaintenanceReportType.SummaryReport;

                        var building = cmbBuilding.SelectedItem as Building;

                        var reportData = reportService.MaintenanceReport(SqlDataHandler.GetConnectionString(), repType, startDate, endDate, building.ID, building.Name, (cmbBuilding.SelectedItem as Building).DataPath);
                        if (reportData == null)
                        {
                            Controller.HandleError("No data found for " + startDate.ToString("MMM yyyy") + " - " + endDate.ToString("MMM yyyy"), "Maintenance Report");
                            return;
                        }

                        if (repType == MaintenanceReportType.DetailedReportWithSupportingDocuments)
                        {
                            byte[] combinedReport = null;


                            using (var dataContext = SqlDataHandler.GetDataContext())
                            {
                                var documentIds = (from m in dataContext.MaintenanceSet
                                                   from d in m.MaintenanceDocuments
                                                   where m.BuildingMaintenanceConfiguration.BuildingId == building.ID
                                                      && m.Requisition.trnDate >= startDate && m.Requisition.trnDate <= endDate
                                                   orderby m.DateLogged
                                                   select d.id).ToList();

                                var reqDocIds = (from m in dataContext.MaintenanceSet
                                                 from d in m.Requisition.Documents
                                                 where m.BuildingMaintenanceConfiguration.BuildingId == building.ID
                                                    && m.Requisition.trnDate >= startDate && m.Requisition.trnDate <= endDate
                                                 orderby m.DateLogged
                                                 select d.id).ToList();

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    using (Document doc = new Document())
                                    {
                                        using (PdfCopy copy = new PdfCopy(doc, ms))
                                        {
                                            doc.Open();

                                            AddPdfDocument(copy, reportData);

                                            foreach (var documentId in reqDocIds)
                                            {
                                                var document = dataContext.RequisitionDocumentSet.Where(a => a.id == documentId).Select(a => a.FileData).Single();
                                                AddPdfDocument(copy, document);
                                            }

                                            foreach (var documentId in documentIds)
                                            {
                                                var document = dataContext.MaintenanceDocumentSet.Where(a => a.id == documentId).Select(a => a.FileData).Single();
                                                AddPdfDocument(copy, document);
                                            }

                                        }
                                    }

                                    combinedReport = ms.ToArray();
                                }
                            }

                            File.WriteAllBytes(dlgSave.FileName, combinedReport);
                        }
                        else
                            File.WriteAllBytes(dlgSave.FileName, reportData);

                        Process.Start(dlgSave.FileName);
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    button1.Enabled = true;
                }
            }
        }

        private void AddPdfDocument(PdfCopy copy, byte[] document)
        {
            PdfReader reader = new PdfReader(document);
            int n = reader.NumberOfPages;
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
            Application.DoEvents();
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            var building = (cmbBuilding.SelectedItem as Building);
            if(building != null)
            {
                int month = 2; //feb
                for(int x=0; x< building.Period; x++)
                {
                    month++;
                    if (month > 12)
                        month = 1;
                }

                var dtEnd = new DateTime(DateTime.Now.Year+1, month, 1);
                var dtStart = dtEnd.AddMonths(-11);
                dtEnd = dtEnd.AddMonths(1).AddDays(-1);

                if(dtStart > DateTime.Today)
                {
                    dtStart = dtStart.AddYears(-1);
                    dtEnd = dtEnd.AddYears(-1); 
                }

                lbFinancialYear.Text = dtStart.ToString("dd MMM") + " - " + dtEnd.ToString("dd MMM");

                LoadYears(dtStart, dtEnd);

            }
        }
    }
}

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
using Astro.Library.Entities;

namespace Astrodon.Reports.SupplierReport
{
    public partial class usrSupplierReport : UserControl
    {
        private List<IdValue> _Years;
        private List<IdValue> _Months;

        private List<IdValue> _ToYears;
        private List<IdValue> _ToMonths;

        private List<Building> _Buildings;

        public usrSupplierReport()
        {
            InitializeComponent();
            LoadYears();
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var userid = Controller.user.id;
                Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
                _Buildings = bManager.buildings.ToList();
                _Buildings.Insert(0, new Building() { Name = " -- All Buildings --", ID = 0 });
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

        private void LoadYears()
        {
            _Years = new List<IdValue>();
            _Years.Add(new IdValue() { Id = DateTime.Now.Year - 1, Value = (DateTime.Now.Year - 1).ToString() });
            _Years.Add(new IdValue() { Id = DateTime.Now.Year, Value = (DateTime.Now.Year).ToString() });
            _Years.Add(new IdValue() { Id = DateTime.Now.Year + 1, Value = (DateTime.Now.Year + 1).ToString() });

            _Months = new List<IdValue>();
            for (int x = 1; x <= 12; x++)
            {
                _Months.Add(new IdValue()
                {
                    Id = x,
                    Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x)
                });
            }

            _ToYears = _Years.ToList();
            _ToMonths = _Months.ToList();

            cmbFromYear.DataSource = _Years;
            cmbFromYear.ValueMember = "Id";
            cmbFromYear.DisplayMember = "Value";
            cmbFromYear.SelectedValue = DateTime.Now.AddMonths(-1).Year;

            cmbFromMonth.DataSource = _Months;
            cmbFromMonth.ValueMember = "Id";
            cmbFromMonth.DisplayMember = "Value";
            cmbFromMonth.SelectedValue = DateTime.Now.AddMonths(-1).Month;

            cmbToYear.DataSource = _ToYears;
            cmbToYear.ValueMember = "Id";
            cmbToYear.DisplayMember = "Value";
            cmbToYear.SelectedValue = DateTime.Now.AddMonths(-1).Year;

            cmbToMonth.DataSource = _ToMonths;
            cmbToMonth.ValueMember = "Id";
            cmbToMonth.DisplayMember = "Value";
            cmbToMonth.SelectedValue = DateTime.Now.AddMonths(-1).Month;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                button1.Enabled = false;

                try
                {
                    using (var reportService = new ReportServiceClient())
                    {
                        DateTime dtFrom = new DateTime((cmbFromYear.SelectedItem as IdValue).Id, (cmbFromMonth.SelectedItem as IdValue).Id, 1);
                        DateTime dtTo = new DateTime((cmbToYear.SelectedItem as IdValue).Id, (cmbToMonth.SelectedItem as IdValue).Id, 1);
                        if(dtFrom > dtTo)
                        {
                            Controller.HandleError("Invalid date range", "Supplier Report");
                            return;
                        }
                        var building = cmbBuilding.SelectedItem as Building;

                        var reportData = reportService.SupplierReport(SqlDataHandler.GetConnectionString(), dtFrom,dtTo, building.ID);
                        if(reportData == null)
                        {
                            Controller.HandleError("No data found", "Supplier Report");
                            return;
                        }
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
    }
}

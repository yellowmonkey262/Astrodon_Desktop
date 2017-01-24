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

namespace Astrodon.Reports.SupplierReport
{
    public partial class usrSupplierReport : UserControl
    {
        private List<IdValue> _Years;
        private List<IdValue> _Months;

        public usrSupplierReport()
        {
            InitializeComponent();
            LoadYears();
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

            cmbYear.DataSource = _Years;
            cmbYear.ValueMember = "Id";
            cmbYear.DisplayMember = "Value";
            cmbYear.SelectedValue = DateTime.Now.AddMonths(-1).Year;

            cmbMonth.DataSource = _Months;
            cmbMonth.ValueMember = "Id";
            cmbMonth.DisplayMember = "Value";
            cmbMonth.SelectedValue = DateTime.Now.AddMonths(-1).Month;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (dlgSave.ShowDialog() == DialogResult.OK)
            {

                button1.Enabled = false;
                try
                {
                    using (var reportService = new ReportServiceClient())
                    {
                        DateTime dDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
                        var reportData = reportService.SupplierReport(SqlDataHandler.GetConnectionString(), dDate);
                        if(reportData == null)
                        {
                            Controller.HandleError("No data found for " + dDate.ToString("MMM yyyy"), "Supplier Report");
                            return;
                        }
                        File.WriteAllBytes(dlgSave.FileName, reportData);
                        Process.Start(dlgSave.FileName);
                    }
                }
                finally
                {
                    button1.Enabled = true;
                }
            }
        }
    }
}

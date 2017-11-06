using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using Astrodon.Data.Base;
using System.Globalization;

namespace Astrodon.Reports.Calendar
{
    public partial class ucPrintCalendar : UserControl
    {
        private List<IdValue> _Years;
        private List<IdValue> _Months;
        private DateTime _ReportDate = DateTime.Today;

        public ucPrintCalendar()
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

        private void pdocCalendar_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var calendarReport = new CalendarReport();
            Dictionary<int, string> data = new Dictionary<int, string>();

            calendarReport.DrawCalendar(e.Graphics, e.MarginBounds, _ReportDate, data);
        }

        private void pdocCalendar_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.Landscape = true;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            _ReportDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
            printDialog1.Document = pdocCalendar;
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                pdocCalendar.Print();
            }
        }
    }
}

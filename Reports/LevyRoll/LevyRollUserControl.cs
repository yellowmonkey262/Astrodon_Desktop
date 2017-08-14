using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Desktop.Lib.Pervasive;
using System.Data.Odbc;
using System.Diagnostics;
using System.Collections;
using System.IO;
using Astrodon.ReportService;
using Astro.Library.Entities;
using Astrodon.Data.Base;

namespace Astrodon.Reports
{
    public partial class LevyRollUserControl : UserControl
    {
        private List<Building> _Buildings;
        private List<IdValue> _Years;
        private List<IdValue> _Months;

        private SqlDataHandler dh = new SqlDataHandler();

        public LevyRollUserControl()
        {
            InitializeComponent();
            LoadBuildings();
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

        private void LoadBuildings()
        {
            var userid = Controller.user.id;
            Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));

            _Buildings = bManager.buildings;
            cmbBuilding.DataSource = _Buildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = false;
                try
                {
                    using (var reportService = ReportServiceClient.CreateInstance())
                    {
                        try
                        {
                            DateTime dDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
                            byte[] reportData = null;
                            if (cbIncludeSundries.Checked)
                                reportData = reportService.LevyRollReport(dDate, (cmbBuilding.SelectedItem as Building).Name, (cmbBuilding.SelectedItem as Building).DataPath);
                            else
                                reportData = reportService.LevyRollExcludeSundries(dDate, (cmbBuilding.SelectedItem as Building).Name, (cmbBuilding.SelectedItem as Building).DataPath);
                            File.WriteAllBytes(dlgSave.FileName, reportData);
                            Process.Start(dlgSave.FileName);
                        }catch(Exception ex)
                        {
                            Controller.HandleError(ex);

                            Controller.ShowMessage(ex.GetType().ToString());
                        }
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
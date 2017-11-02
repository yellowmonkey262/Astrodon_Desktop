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
using Astrodon.Data.DebitOrder;

namespace Astrodon.Reports.DebitOrder
{
    public partial class DebitOrderUserControl : UserControl
    {

        private List<Building> _Buildings;
        private List<IdValue> _Years;
        private List<IdValue> _Months;
        private SqlDataHandler dh = new SqlDataHandler();

        public DebitOrderUserControl()
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
            _Buildings.Insert(0, new Building() { ID = 0, Name = "All Buildings" });
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
                    lbProgress.Text = "Starting";
                    var buildingList = _Buildings.Where(a => a.ID > 0).ToList();
                    if((cmbBuilding.SelectedItem as Building).ID != 0)
                        buildingList = _Buildings.Where(a => a.ID == (cmbBuilding.SelectedItem as Building).ID).ToList();

                    lbStatus.Items.Clear();
                    int errorCount = 0;

                    DateTime dDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
                    List<DebitOrderItem> compiledList = new List<DebitOrderItem>();
                    int buildingNum = 1;
                    using (var reportService = ReportServiceClient.CreateInstance())
                    {
                        foreach(var building in buildingList)
                        {
                            lbProgress.Text = "Processing " + buildingNum.ToString() +"/"+ buildingList.Count().ToString() +" => "+building.Name;
                            buildingNum++;
                            Application.DoEvents();
                            try
                            {
                                var items = reportService.RunDebitOrderForBuilding(SqlDataHandler.GetConnectionString(), building.ID, dDate, cbShowBreakdown.Checked);
                                if(items != null && items.Length > 0)
                                   compiledList.AddRange(items.ToList());
                                lbStatus.Items.Insert(0,building.Name + " => "+ items.Length.ToString() + " records");
                            }
                            catch(Exception ex)
                            {
                                lbStatus.Items.Insert(0, "ERROR => " +building.Name +" " + ex.Message);
                                errorCount++;
                            }
                            Application.DoEvents();
                        }
                        lbProgress.Text = "Compiling Excel File " + compiledList.Count().ToString() + " records total";
                        Application.DoEvents();
                        if (errorCount > 0)
                        {
                            Controller.HandleError("Warning " + errorCount.ToString() + " buildings that could not be processed.", "Warning");
                        }

                        byte[] reportData = null;
                        Controller.ShowMessage("CNT " + compiledList.Count() + " " + compiledList[0].AmountDue.ToString() + " - > " + compiledList[1].AmountDue.ToString());

                        reportData = reportService.SAPORDebitOrder(SqlDataHandler.GetConnectionString(), compiledList.ToArray(), cbShowBreakdown.Checked);
                        File.WriteAllBytes(dlgSave.FileName, reportData);
                        Process.Start(dlgSave.FileName);
                        lbProgress.Text = "Completed " + compiledList.Count().ToString() + " records total";
                        Application.DoEvents();

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

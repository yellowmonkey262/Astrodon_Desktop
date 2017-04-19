using Astro.Library;
using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class usrSummaryReport : UserControl
    {
        private int trustPeriod;
        private Dictionary<String, Building2> repBuildings;
        private List<SummRep> summaries = new List<SummRep>();

        public usrSummaryReport()
        {
            InitializeComponent();
        }

        private void usrSummaryReport_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            repBuildings = Utilities.GetReportBuildings();
            List<Building2> rpb = repBuildings.Values.ToList();
            trustPeriod = Methods.getPeriod(DateTime.Now);
            CalcTotals();
            this.Cursor = Cursors.Arrow;
        }

        private void CalcTotals()
        {
            foreach (KeyValuePair<String, Building2> mBuilding in repBuildings)
            {
                try
                {
                    Building2 building = mBuilding.Value;
                    int buildPeriod = (trustPeriod - building.Period < 1 ? trustPeriod - building.Period + 12 : trustPeriod - building.Period);
                    double buildBal = 0;
                    double centrecBal = 0;
                    for (int li = 0; li < building.buildCentrec.lastBal.Length; li++) { buildBal += building.buildCentrec.lastBal[li]; }
                    for (int i = 0; i < buildPeriod; i++) { buildBal += building.buildCentrec.thisBal[i]; }
                    for (int li = 0; li < building.centrecBuild.lastBal.Length; li++) { centrecBal += building.centrecBuild.lastBal[li]; }
                    for (int i = 0; i < trustPeriod; i++) { centrecBal += building.centrecBuild.balance[i]; }
                    SummRep summary = new SummRep
                    {
                        Bank = building.Bank,
                        Code = building.Code,
                        TrustCode = building.Acc,
                        BuildingName = building.BuildingName,
                        BuildBal = double.Parse(buildBal.ToString("##0.00")),
                        CentrecBal = double.Parse(centrecBal.ToString("##0.00"))
                    };
                    if (buildBal != centrecBal)
                    {
                        buildBal = buildBal * (buildBal < 0 ? -1 : 1);
                        centrecBal = centrecBal * (centrecBal < 0 ? -1 : 1);
                        summary.Difference = buildBal - centrecBal;
                        summary.Difference = double.Parse((summary.Difference * (summary.Difference < 0 ? -1 : 1)).ToString("##0.00"));
                    }
                    else
                    {
                        summary.Difference = 0;
                    }
                    summaries.Add(summary);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (summaries.Count > 0) { dgSummary.DataSource = summaries; }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ReportWriter reporter = new ReportWriter();
            String msg = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files | *.xls";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (sfd.FileName != "" && sfd.FileName.EndsWith(".xls"))
                    {
                        reporter.CreateSummaryReport(summaries, sfd.FileName, out msg);
                        if (msg != "Excel Report Saved")
                        {
                            MessageBox.Show(msg, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrJobReport : UserControl
    {
        private SqlDataHandler dh = new SqlDataHandler();
        private BindingSource bs = new BindingSource();
        private String status;
        private BindingSource cmbVals = new BindingSource();
        private bool validSelection = false;
        private int selectionType = 0;
        private String criteria = "";

        public usrJobReport()
        {
            InitializeComponent();
        }

        private void usrJobReport_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bs;
            cmbCriteria.DataSource = cmbVals;
            dtStart.Value = DateTime.Now.AddMonths(-1);
            dtTo.Value = DateTime.Now;
        }

        private void cmbSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            bs.Clear();
            cmbVals.Clear();
            cmbCriteria.SelectedIndexChanged -= cmbCriteria_SelectedIndexChanged;
            SelectionValues svTemp = new SelectionValues()
            {
                text = "Please select",
                value = "0"
            };
            cmbVals.Add(svTemp);
            try
            {
                switch (cmbSelector.SelectedItem.ToString())
                {
                    case "Status":
                        LoadStatus();
                        selectionType = 1;
                        break;

                    case "PM":
                        LoadUsers("2");
                        selectionType = 2;
                        break;

                    case "PA":
                        LoadUsers("4");
                        selectionType = 3;
                        break;
                }
                cmbCriteria.DataSource = cmbVals;
                cmbCriteria.DisplayMember = "text";
                cmbCriteria.ValueMember = "value";
                cmbCriteria.Refresh();
            }
            catch { }
            cmbCriteria.SelectedIndexChanged += cmbCriteria_SelectedIndexChanged;
        }

        private void cmbCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                criteria = cmbCriteria.SelectedValue.ToString();
                if (criteria != "0")
                {
                    validSelection = true;
                    LoadJobs();
                }
            }
            catch
            {
                validSelection = false;
            }
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {
            if (validSelection) { LoadJobs(); }
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            if (validSelection) { LoadJobs(); }
        }

        private void LoadJobs()
        {
            int totJobs = 0;
            int totAss = 0;
            int totComp = 0;

            bs.Clear();
            if (validSelection && selectionType != 0)
            {
                String query = "SELECT j.id, u1.name AS creator, u2.name AS processor, j.buildingCode, j.status, j.createDate, j.assignedDate, ";
                query += " CASE WHEN DATEDIFF(hour, j.createDate, j.assignedDate) IS NULL THEN - 1 ELSE DATEDIFF(hour, j.createDate, j.assignedDate) END AS assDiff, ";
                query += " j.completeDate, CASE WHEN DATEDIFF(hour, j.assignedDate, j.completeDate) IS NULL THEN - 1 ELSE DATEDIFF(hour, j.assignedDate, j.completeDate) ";
                query += " END AS compDiff FROM tblPMJob AS j LEFT OUTER JOIN tblUsers AS u2 ON j.processedBy = u2.id LEFT OUTER JOIN ";
                query += " tblUsers AS u1 ON j.creator = u1.id WHERE (j.createDate >= @start) AND (j.createDate <= @end) AND ";
                switch (selectionType)
                {
                    case 1:
                        query += " (j.status = @crit)";
                        break;

                    case 2:
                        query += " (j.creator = @crit)";
                        break;

                    case 3:
                        query += " (j.processedBy = @crit)";
                        break;
                }
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                DateTime start = new DateTime(dtStart.Value.Year, dtStart.Value.Month, dtStart.Value.Day, 0, 0, 0);
                DateTime finish = new DateTime(dtTo.Value.Year, dtTo.Value.Month, dtTo.Value.Day, 23, 59, 59);
                sqlParms.Add("@start", start);
                sqlParms.Add("@end", finish);
                sqlParms.Add("@crit", criteria);
                DataSet dsJobs = dh.GetData(query, sqlParms, out status);
                if (dsJobs != null && dsJobs.Tables.Count > 0 && dsJobs.Tables[0].Rows.Count > 0)
                {
                    totJobs = dsJobs.Tables[0].Rows.Count;
                    foreach (DataRow drJob in dsJobs.Tables[0].Rows)
                    {
                        JobData jd = new JobData();
                        jd.id = drJob["id"].ToString();
                        jd.creator = drJob["creator"].ToString();
                        jd.processor = drJob["processor"].ToString();
                        jd.buildingCode = drJob["buildingCode"].ToString();
                        jd.status = drJob["status"].ToString();
                        jd.createDate = drJob["createDate"].ToString();
                        jd.assignedDate = drJob["assignedDate"].ToString();
                        jd.assDiff = (jd.assignedDate == "" ? "" : CalcDiff(DateTime.Parse(jd.createDate), DateTime.Parse(jd.assignedDate)).ToString());
                        if (jd.assDiff != "") { totAss += int.Parse(jd.assDiff); }
                        jd.completeDate = drJob["completeDate"].ToString();
                        jd.compDiff = (jd.assignedDate == "" || jd.completeDate == "" ? "" : CalcDiff(DateTime.Parse(jd.assignedDate), DateTime.Parse(jd.completeDate)).ToString());
                        if (jd.compDiff != "") { totComp += int.Parse(jd.compDiff); }
                        bs.Add(jd);
                    }
                }
            }
            lblTotal.Text = totJobs.ToString();
            lblTotAss.Text = totAss.ToString();
            lblTotComp.Text = totComp.ToString();
            lblAvgAss.Text = ((double)totAss / (double)totJobs).ToString("#,##0.00");
            lblAvgComp.Text = ((double)totComp / (double)totJobs).ToString("#,##0.00");
            dataGridView1.Refresh();
        }

        private int CalcDiff(DateTime startDate, DateTime endDate)
        {
            int diff = 0;
            int minHour = 8;
            int maxHour = 17;
            int iniHours = (endDate - startDate).Hours;
            int iniDays = (endDate - startDate).Days;
            if (iniHours > 0 || iniDays > 0)
            {
                while (startDate < endDate)
                {
                    diff += 1;
                    startDate = startDate.AddHours(1);
                    while (startDate.Hour < minHour || startDate.Hour >= maxHour || startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        startDate = startDate.AddHours(1);
                    }
                }
            }
            else
            {
                diff = 0;
            }
            return diff;
        }

        private void LoadStatus()
        {
            String query = "SELECT DISTINCT status FROM tblPMJob ORDER BY status";
            DataSet dsStatus = dh.GetData(query, null, out status);
            if (dsStatus != null && dsStatus.Tables.Count > 0 && dsStatus.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsStatus.Tables[0].Rows)
                {
                    SelectionValues sv = new SelectionValues
                    {
                        text = dr["status"].ToString(),
                        value = dr["status"].ToString()
                    };
                    try { cmbVals.Add(sv); } catch { }
                }
            }
        }

        private void LoadUsers(String type)
        {
            String query = "SELECT DISTINCT id, name FROM tblUsers WHERE usertype = " + type + " and Active = 1";
            DataSet dsStatus = dh.GetData(query, null, out status);
            if (dsStatus != null && dsStatus.Tables.Count > 0 && dsStatus.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsStatus.Tables[0].Rows)
                {
                    SelectionValues sv = new SelectionValues
                    {
                        text = dr["name"].ToString(),
                        value = dr["id"].ToString()
                    };
                    try { cmbVals.Add(sv); } catch { }
                }
            }
        }

        private class SelectionValues
        {
            public String text { get; set; }
            public String value { get; set; }
        }

        private class JobData
        {
            public String id { get; set; }
            public String creator { get; set; }
            public String processor { get; set; }
            public String buildingCode { get; set; }
            public String status { get; set; }
            public String createDate { get; set; }
            public String assignedDate { get; set; }
            public String assDiff { get; set; }
            public String completeDate { get; set; }
            public String compDiff { get; set; }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = sender as DataGridView;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                String jid = dataGridView1.Rows[e.RowIndex].Cells["cID"].Value.ToString();
                using (Forms.frmJobBreakdown jbFrm = new Forms.frmJobBreakdown(jid))
                {
                    jbFrm.ShowDialog();
                }
            }
        }
    }
}
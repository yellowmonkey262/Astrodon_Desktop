using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon.Forms
{
    public partial class frmJobBreakdown : Form
    {
        private String jobID;
        private BindingSource bs = new BindingSource();

        public frmJobBreakdown(String jid)
        {
            jobID = jid;
            InitializeComponent();
        }

        private void frmJobBreakdown_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bs;
            LoadBreakdown();
        }

        private void LoadBreakdown()
        {
            bs.Clear();
            String query = " SELECT u.name, ps.status, ps.actionDate FROM tblPMJobStatus AS ps INNER JOIN tblUsers AS u ON ps.actioned = u.id ";
            query += " WHERE (ps.jobID = @jid) ORDER BY ps.actionDate";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@jid", jobID);
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            DataSet ds = dh.GetData(query, sqlParms, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DateTime iniDate = DateTime.Now;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    jobBreakdown jb = new jobBreakdown
                    {
                        user = dr["name"].ToString(),
                        status = dr["status"].ToString(),
                        date = dr["actionDate"].ToString()
                    };
                    if (i == 0)
                    {
                        jb.delay = "";
                        iniDate = DateTime.Parse(jb.date);
                    }
                    else
                    {
                        jb.delay = CalcDiff(iniDate, DateTime.Parse(jb.date)).ToString();
                        iniDate = DateTime.Parse(jb.date);
                    }
                    bs.Add(jb);
                }
            }
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

        private class jobBreakdown
        {
            public String user { get; set; }
            public String status { get; set; }
            public String date { get; set; }
            public String delay { get; set; }
        }
    }
}
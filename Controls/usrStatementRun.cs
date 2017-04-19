using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrStatementRun : UserControl
    {
        private List<Building> buildings;
        private SqlDataHandler dh = new SqlDataHandler();

        public usrStatementRun()
        {
            InitializeComponent();
            buildings = new Buildings(true, "All buildings").buildings;
        }

        private void usrStatementRun_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtFrom.Value = today.AddDays(-7);
            dtTo.Value = today.AddDays(1);
            rdBoth.Checked = true;
        }

        private void LoadBuildings()
        {
            cmbBuilding.DataSource = buildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedItem != null)
            {
                this.Cursor = Cursors.WaitCursor;
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                String abbr = cmbBuilding.SelectedIndex > 0 ? buildings[cmbBuilding.SelectedIndex].Abbr : String.Empty;
                String query = "SELECT id as ID, unit AS Unit, debtorEmail AS [Sent From], email1 AS [Sent To], queueDate AS Queued, sentDate1 AS Sent, errorMessage AS Status FROM tblStatementRun";
                String orderBy = "";
                if (rdSent.Checked)
                {
                    query += " WHERE (sentDate1 >= @dtFrom AND sentDate1 <= @dtTo) ";
                    orderBy = "sentDate1";
                }
                else if (rdUnsent.Checked)
                {
                    query += " WHERE (queueDate >= @dtFrom AND queueDate <= @dtTo) AND (sentDate1 is null)";
                    orderBy = "queueDate";
                }
                else
                {
                    query += " WHERE ((sentDate1 >= @dtFrom AND sentDate1 <= @dtTo) OR (queueDate >= @dtFrom AND queueDate <= @dtTo)) ";
                    orderBy = "queueDate, sentDate1";
                }
                sqlParms.Add("@dtFrom", dtFrom.Value);
                sqlParms.Add("@dtTo", dtTo.Value);

                if (!String.IsNullOrEmpty(abbr))
                {
                    query += " AND LEFT(unit, " + abbr.Length.ToString() + ") = '" + abbr + "' ";
                    query += " AND ISNUMERIC(SUBSTRING(unit, " + (abbr.Length + 1).ToString() + ", 1)) = 1";
                    sqlParms.Add("@abbr", abbr);
                }

                query += " ORDER BY " + orderBy;
                // MessageBox.Show(query);
                String status;
                DataSet ds = dh.GetData(query, sqlParms, out status);
                dgStatements.DataSource = null;
                if (ds != null && ds.Tables.Count > 0)
                {
                    dgStatements.DataSource = ds.Tables[0];
                    lblStatements.Text = "Statement count: " + ds.Tables[0].Rows.Count.ToString();
                }
                else
                {
                    lblStatements.Text = "Statement count: 0";
                }
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dgStatements_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            String id = e.Row.Cells[0].Value.ToString();
            String query = "DELETE FROM tblStatementRun WHERE id = " + id;
            String status;
            dh.SetData(query, null, out status);
        }
    }
}
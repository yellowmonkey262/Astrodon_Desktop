using System;
using System.Data;
using System.Windows.Forms;

namespace Astrodon.Controls {

    public partial class usrEmail : UserControl {
        private SqlDataHandler dh = new SqlDataHandler();
        private BindingSource bs = new BindingSource();

        public usrEmail() {
            InitializeComponent();
        }

        private DataSet GetUnits(bool statements) {
            String query = "SELECT DISTINCT unit" + (!statements ? "no" : "") + " as item FROM " + (!statements ? "tblLetterRun" : "tblStatementRun") + " ORDER BY unit" + (!statements ? "no" : "");
            String status = String.Empty;
            DataSet ds = dh.GetData(query, null, out status);
            return ds;
        }

        private DataSet GetStatus(bool statements, bool del) {
            String query = "SELECT DISTINCT " + (del ? "status" : "errorMessage") + " as item FROM " + (!statements ? "tblLetterRun" : "tblStatementRun") + " ORDER BY " + (del ? "status" : "errorMessage");
            String status = String.Empty;
            DataSet ds = dh.GetData(query, null, out status);
            return ds;
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e) {
            bs.Clear();
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e) {
            cmbCrit.Items.Clear();
            bs.Clear();
            bool statements = (cmbSearch.SelectedItem.ToString() == "Statements");
            DataSet ds;
            if (cmbSearchBy.SelectedItem.ToString() == "Unit") {
                ds = GetUnits(statements);
            } else {
                if (cmbSearchBy.SelectedItem.ToString() == "Sent Status") {
                    ds = GetStatus(statements, false);
                } else {
                    ds = GetStatus(statements, true);
                }
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    cmbCrit.Items.Add(dr["item"].ToString());
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            bool statements = (cmbSearch.SelectedItem.ToString() == "Statements");
            String crit = cmbCrit.SelectedItem.ToString();
            DataSet ds;
            if (cmbSearchBy.SelectedItem.ToString() == "Unit") {
                if (statements) {
                    ds = GetStatementsUnit(crit);
                } else {
                    ds = GetLettersUnit(crit);
                }
            } else {
                if (cmbSearchBy.SelectedItem.ToString() == "Sent Status") {
                    if (statements) {
                        ds = GetStatementsStatus(crit, false);
                    } else {
                        ds = GetLettersStatus(crit, false);
                    }
                } else {
                    if (statements) {
                        ds = GetStatementsStatus(crit, true);
                    } else {
                        ds = GetLettersStatus(crit, true);
                    }
                }
            }
            bs.Clear();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    EmailResponse er = new EmailResponse();
                    er.Unit = dr["unitNo"].ToString();
                    er.Date = DateTime.Parse(dr["Date Sent"].ToString()).ToString("yyyy/MM/dd");
                    er.Delivery_Status = dr["Delivery Status"].ToString();
                    er.Sent_Status = dr["Sent Status"].ToString();
                    er.Subject = dr["Subject"].ToString();
                    er.To = dr["Sent To"].ToString();
                    bs.Add(er);
                }
            }
        }

        private DataSet GetLettersUnit(String unit) {
            String query = "SELECT unitNo, sentDate AS [Date Sent], toEmail AS [Sent To], subject AS Subject, errorMessage AS [Sent Status], status AS [Delivery Status]";
            query += " FROM tblLetterRun WHERE unitno = '" + unit + "' ORDER BY [Date Sent]";
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            return ds;
        }

        private DataSet GetStatementsUnit(String unit) {
            String query = "SELECT unit as unitNo, email1 AS [Sent To], subject AS Subject, sentDate1 AS [Date Sent], errorMessage AS [Sent Status], status AS [Delivery Status]";
            query += " FROM tblStatementRun WHERE unit = '" + unit + "' ORDER BY [Date Sent]";
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            return ds;
        }

        private DataSet GetLettersStatus(String crit, bool del) {
            String query = "SELECT unitNo, sentDate AS [Date Sent], toEmail AS [Sent To], subject AS Subject, errorMessage AS [Sent Status], status AS [Delivery Status]";
            query += " FROM tblLetterRun WHERE " + (del ? "status" : "errorMessage") + " = '" + crit + "' ORDER BY [Date Sent]";
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            return ds;
        }

        private DataSet GetStatementsStatus(String crit, bool del) {
            String query = "SELECT unit as unitNo, email1 AS [Sent To], subject AS Subject, sentDate1 AS [Date Sent], errorMessage AS [Sent Status], status AS [Delivery Status]";
            query += " FROM tblStatementRun WHERE " + (del ? "status" : "errorMessage") + " = '" + crit + "' ORDER BY [Date Sent]";
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            return ds;
        }

        private void usrEmail_Load(object sender, EventArgs e) {
            dataGridView1.DataSource = bs;
        }

        private class EmailResponse {
            public String Unit { get; set; }

            public String Date { get; set; }

            public String To { get; set; }

            public String Subject { get; set; }

            public String Sent_Status { get; set; }

            public String Delivery_Status { get; set; }
        }

        private void btnPrint_Click(object sender, EventArgs e) {
            LoadPrintGrid();
        }

        private void LoadPrintGrid() {
            //2-9
            BindingSource bsPrint = new BindingSource();
            DataGridView dgPrint = new DataGridView();
            foreach (DataGridViewRow dr in dataGridView1.Rows) {
                EmailResponse er = dr.DataBoundItem as EmailResponse;
                bsPrint.Add(er);
            }
            dgPrint.DataSource = bsPrint;
            this.Controls.Add(dgPrint);
            dgPrint.Size = dataGridView1.Size;
            dgPrint.Visible = true;
            PrintDGV.Print_DataGridView(dgPrint);
            dgPrint.Visible = false;
            this.Controls.Remove(dgPrint);
        }
    }
}
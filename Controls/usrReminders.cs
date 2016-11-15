using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon.Controls {

    public partial class usrReminders : UserControl {
        private SqlDataHandler dh = new SqlDataHandler();
        private BindingSource bsRem = new BindingSource();

        public usrReminders() {
            InitializeComponent();
        }

        private void usrReminders_Load(object sender, EventArgs e) {
            dataGridView1.DataSource = bsRem;
            LoadReminders();
        }

        private void LoadReminders() {
            bsRem.Clear();
            String remQuery = "SELECT r.id, b.Building, b.DataPath, r.customer, r.remDate, r.remNote, r.action FROM tblReminders AS r INNER JOIN tblBuildings AS b ON r.building = b.id";
            remQuery += " WHERE userID = " + Controller.user.id.ToString() + " ORDER BY r.action, r.remDate";
            String status;
            DataSet dsRem = dh.GetData(remQuery, null, out status);
            if (dsRem != null && dsRem.Tables.Count > 0 && dsRem.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in dsRem.Tables[0].Rows) {
                    MyReminders mr = new MyReminders();
                    mr.RemID = int.Parse(dr["id"].ToString());
                    mr.Building = dr["Building"].ToString();
                    mr.Customer = dr["customer"].ToString();
                    mr.ReminderDate = DateTime.Parse(dr["remDate"].ToString());
                    mr.Reminder = dr["remNote"].ToString();
                    mr.Contacts = String.Empty;
                    mr.Phone = String.Empty;
                    mr.Fax = String.Empty;
                    mr.Email = String.Empty;
                    mr.Action = bool.Parse(dr["action"].ToString());
                    if (!mr.Action) {
                        Customer c = Controller.pastel.GetOneCustomer(dr["DataPath"].ToString(), mr.Customer);
                        List<AdditionalAddress> delAddresses = Controller.pastel.GetDeliveryInfo(dr["DataPath"].ToString(), mr.Customer);
                        foreach (AdditionalAddress aa in delAddresses) {
                            if (!mr.Contacts.Contains(aa.Contact)) { mr.Contacts += aa.Contact + ";"; }
                            if (!mr.Phone.Contains(aa.Telephone)) { mr.Phone += aa.Telephone + ";"; }
                            if (!mr.Phone.Contains(aa.Cell)) { mr.Phone += aa.Cell + ";"; }
                            if (!mr.Fax.Contains(aa.Fax)) { mr.Fax += aa.Fax + ";"; }
                            if (!mr.Email.Contains(aa.Email)) { mr.Email += aa.Email + ";"; }
                        }
                    }
                    bsRem.Add(mr);
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            try {
                if (e.ColumnIndex == 9) {
                    int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    bool action = (bool)dataGridView1.Rows[e.RowIndex].Cells[9].Value;
                    String updateQuery = "UPDATE tblReminders SET action = '" + action.ToString() + "', actionDate = getdate() WHERE id = " + id.ToString();
                    String status;
                    dh.SetData(updateQuery, null, out status);
                    LoadReminders();
                    Controller.mainF.LoadReminders();
                    foreach (DataGridViewRow dvr in dataGridView1.Rows) {
                        DateTime remDate = (DateTime)dvr.Cells[3].Value;
                        bool actioned = (bool)dvr.Cells[9].Value;
                        if (remDate <= DateTime.Now && !actioned) {
                            dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                            dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        } else if (remDate <= DateTime.Now.AddDays(1) && !actioned) {
                            dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                            dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        } else if (actioned) {
                            dvr.Cells[9].ReadOnly = true;
                            dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            } catch { }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            foreach (DataGridViewRow dvr in dataGridView1.Rows) {
                DateTime remDate = (DateTime)dvr.Cells[3].Value;
                bool actioned = (bool)dvr.Cells[9].Value;
                if (remDate <= DateTime.Now && !actioned) {
                    dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                } else if (remDate <= DateTime.Now.AddDays(1) && !actioned) {
                    dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                } else if (actioned) {
                    dvr.Cells[9].ReadOnly = true;
                    dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                    dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
            }
        }
    }

    public class MyReminders {
        public int RemID { get; set; }

        public String Building { get; set; }

        public String Customer { get; set; }

        public DateTime ReminderDate { get; set; }

        public String Reminder { get; set; }

        public String Contacts { get; set; }

        public String Phone { get; set; }

        public String Fax { get; set; }

        public String Email { get; set; }

        public bool Action { get; set; }
    }
}
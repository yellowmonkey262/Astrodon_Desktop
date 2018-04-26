using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrReminders : UserControl
    {
        private SqlDataHandler dh = new SqlDataHandler();
        private BindingSource bsRem = new BindingSource();

        public usrReminders()
        {
            InitializeComponent();
        }

        private void usrReminders_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bsRem;
            LoadReminders();
        }

        private void LoadReminders()
        {
            bsRem.Clear();
            string remQuery = "SELECT r.id, b.Building, b.DataPath, r.customer, r.remDate, r.remNote, r.action " +
                              " FROM tblReminders  r "+
                              " LEFT JOIN tblBuildings b on CASE WHEN ISNUMERIC(r.building) = 1 THEN CAST(r.building AS INT) ELSE NULL END = b.Id " +
                              " WHERE userID = "+ Controller.user.id.ToString() + " " +
                              " ORDER BY r.action, r.remDate";

            String status;
            DataSet dsRem = dh.GetData(remQuery, null, out status);
            if (dsRem != null && dsRem.Tables.Count > 0 && dsRem.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRem.Tables[0].Rows)
                {
                    MyReminders mr = new MyReminders
                    {
                        RemID = int.Parse(dr["id"].ToString()),
                        Building = dr["Building"].ToString(),
                        Customer = dr["customer"].ToString(),
                        ReminderDate = DateTime.Parse(dr["remDate"].ToString()),
                        Reminder = dr["remNote"].ToString(),
                        Contacts = String.Empty,
                        Phone = String.Empty,
                        Fax = String.Empty,
                        Email = String.Empty,
                        Action = bool.Parse(dr["action"].ToString())
                    };
                    if (!mr.Action)
                    {
                        Customer c = Controller.pastel.GetOneCustomer(dr["DataPath"].ToString(), mr.Customer);
                        if (c != null)
                        {
                            List<AdditionalAddress> delAddresses = Controller.pastel.GetDeliveryInfo(dr["DataPath"].ToString(), mr.Customer);
                            var builder = new System.Text.StringBuilder();
                            builder.Append(mr.Contacts);
                            var builder1 = new System.Text.StringBuilder();
                            builder1.Append(mr.Phone);
                            var builder2 = new System.Text.StringBuilder();
                            builder2.Append(mr.Phone);
                            var builder3 = new System.Text.StringBuilder();
                            builder3.Append(mr.Fax);
                            var builder4 = new System.Text.StringBuilder();
                            builder4.Append(mr.Email);
                            foreach (AdditionalAddress aa in delAddresses)
                            {
                                if (!mr.Contacts.Contains(aa.Contact)) { builder.Append(aa.Contact + ";"); }
                                if (!mr.Phone.Contains(aa.Telephone)) { builder1.Append(aa.Telephone + ";"); }
                                if (!mr.Phone.Contains(aa.Cell)) { builder2.Append(aa.Cell + ";"); }
                                if (!mr.Fax.Contains(aa.Fax)) { builder3.Append(aa.Fax + ";"); }
                                if (!mr.Email.Contains(aa.Email)) { builder4.Append(aa.Email + ";"); }
                            }
                            mr.Email = builder4.ToString();
                            mr.Fax = builder3.ToString();
                            mr.Phone = builder2.ToString();
                            mr.Phone = builder1.ToString();
                            mr.Contacts = builder.ToString();
                        }
                    }
                    bsRem.Add(mr);
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 9)
                {
                    int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    bool action = (bool)dataGridView1.Rows[e.RowIndex].Cells[9].Value;
                    String updateQuery = "UPDATE tblReminders SET action = '" + action.ToString() + "', actionDate = getdate() WHERE id = " + id.ToString();
                    String status;
                    dh.SetData(updateQuery, null, out status);
                    LoadReminders();
                    Controller.mainF.LoadReminders();
                    foreach (DataGridViewRow dvr in dataGridView1.Rows)
                    {
                        DateTime remDate = (DateTime)dvr.Cells[3].Value;
                        bool actioned = (bool)dvr.Cells[9].Value;
                        if (remDate <= DateTime.Now && !actioned)
                        {
                            dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                            dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        }
                        else if (remDate <= DateTime.Now.AddDays(1) && !actioned)
                        {
                            dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                            dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        }
                        else if (actioned)
                        {
                            dvr.Cells[9].ReadOnly = true;
                            dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
            catch { }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dvr in dataGridView1.Rows)
            {
                DateTime remDate = (DateTime)dvr.Cells[3].Value;
                bool actioned = (bool)dvr.Cells[9].Value;
                if (remDate <= DateTime.Now && !actioned)
                {
                    dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
                else if (remDate <= DateTime.Now.AddDays(1) && !actioned)
                {
                    dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
                else if (actioned)
                {
                    dvr.Cells[9].ReadOnly = true;
                    dvr.DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                    dvr.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
            }
        }
    }
}
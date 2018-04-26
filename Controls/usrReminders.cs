using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Data.Entity;

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

            using (var context = SqlDataHandler.GetDataContext())
            {
                var start = DateTime.Today.AddDays(-30);

                var q = from r in context.tblReminders
                        where r.UserId == Controller.user.id
                        && (r.action == false || r.actionDate == null || r.actionDate > start)
                        select new MyReminders()
                        {
                            RemID = r.id,
                            Building = r.Building.Building,
                            Customer = r.customer,
                            ReminderDate = r.remDate,
                            Reminder = r.remNote,
                            Contacts = r.Contacts,
                            Phone = r.Phone,
                            Fax = r.Fax,
                            Email = r.Email,
                            Action = r.action
                        };

                bsRem.DataSource = q.OrderBy(a => a.Action).ThenBy(a => a.ReminderDate).ToList();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateRowColours();
        }

        private void UpdateRowColours()
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                var selectedItem = senderGrid.Rows[e.RowIndex].DataBoundItem as MyReminders;
                if (selectedItem != null)
                {
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var obj = context.tblReminders.Where(a => a.id == selectedItem.RemID).FirstOrDefault();

                        obj.action = selectedItem.Action;
                        if (selectedItem.Action)
                            obj.actionDate = DateTime.Now;
                        else
                            obj.actionDate = null;
                        context.SaveChanges();
                    }

                    UpdateRowColours();
                }
            }
        }
    }
}
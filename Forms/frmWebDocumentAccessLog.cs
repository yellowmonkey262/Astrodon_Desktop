using Astrodon.ClientPortal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Astrodon.Forms
{
    public partial class frmWebDocumentAccessLog : Form
    {
        private AstrodonClientPortal _ClientPortal = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString());

        public frmWebDocumentAccessLog()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void ShowUnitDocumentHistory(Guid documentId, string documentTitle)
        {
            var frm = new frmWebDocumentAccessLog();
            frm.lbDocumentTitle.Text = documentTitle;
            bool result = frm.LoadDocumentHistory(documentId);
            if(result)
              frm.ShowDialog();
        }

        private bool LoadDocumentHistory(Guid documentId)
        {
            var fileData = _ClientPortal.GetUnitFileAccessHistory(documentId);

            dgMaintenance.ClearSelection();
            dgMaintenance.MultiSelect = false;
            dgMaintenance.AutoGenerateColumns = false;

            dgMaintenance.Columns.Clear();
            dgMaintenance.DataSource = null;

            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            if (fileData.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = fileData;
                dgMaintenance.DataSource = bs;


                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "AccessDateStr",
                    HeaderText = "Date",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "AccessType",
                    HeaderText = "Access Type",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "EmailAddress",
                    HeaderText = "Web User",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "AccessToken",
                    HeaderText = "Link Info",
                    ReadOnly = true,
                    DefaultCellStyle = currencyColumnStyle
                });



                dgMaintenance.AutoResizeColumns();
                return true;
            }else
            {
                Controller.ShowMessage("No access history could be retrieved for this file.");
                return false;
            }
        }
    }
}
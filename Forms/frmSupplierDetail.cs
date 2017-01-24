using Astrodon.Controls.Supplier;
using Astrodon.Data;
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
    public partial class frmSupplierDetail : Form
    {
        private DataContext _DataContext;
        private usrSupplierDetail _SupplierDetailControl;

        public frmSupplierDetail(DataContext context, int supplierId)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            _DataContext = context;

            _SupplierDetailControl = new usrSupplierDetail(_DataContext, supplierId);
            _SupplierDetailControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(_SupplierDetailControl);

            _SupplierDetailControl.SaveSuccessEvent += SupplierDetail_SaveSuccessEvent;
        }

        private void SupplierDetail_SaveSuccessEvent(object sender, SaveSuccessEventArgs e)
        {
            if (e.SaveSuccess)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;

            Close();
        }

        private void frmSupplierDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            _SupplierDetailControl.SaveSuccessEvent -= SupplierDetail_SaveSuccessEvent;
        }
    }
}

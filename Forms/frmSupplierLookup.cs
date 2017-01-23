using Astrodon.Controls.Supplier;
using Astrodon.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Astrodon.Forms
{
    public partial class frmSupplierLookup : Form
    {
        private usrSupplierLookup _LookupControl;

        private Data.SupplierData.Supplier _SelectedItem = null;

        public frmSupplierLookup(DataContext context)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            _LookupControl = new usrSupplierLookup(context);
            _LookupControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(_LookupControl);

            _LookupControl.SupplierSelectedEvent += LookupControl_SupplierSelectedEvent;
        }

        private void LookupControl_SupplierSelectedEvent(object sender, SupplierEventArgs e)
        {
            _SelectedItem = e.SelectedItem;
            if (e.SupplierSelected)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _LookupControl.SupplierSelectedEvent -= LookupControl_SupplierSelectedEvent;
            base.OnClosed(e);
        }
      

        public static bool SelectSupplier(out Data.SupplierData.Supplier supplier)
        {
            supplier = null;
            using (var context = SqlDataHandler.GetDataContext())
            {
                var frm = new frmSupplierLookup(context);
                var dialogResult = frm.ShowDialog();
                supplier = frm._SelectedItem;

                if (Debugger.IsAttached)
                {
                    supplier = context.SupplierSet.FirstOrDefault();
                    return supplier != null;
                }

                return (dialogResult == DialogResult.OK && supplier != null);
            }
        }
    }
}

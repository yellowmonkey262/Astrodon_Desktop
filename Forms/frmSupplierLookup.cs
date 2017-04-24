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

        public Data.SupplierData.Supplier SelectedSupplier { get ; private set; }

        public frmSupplierLookup(DataContext context, int buildingId)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            _LookupControl = new usrSupplierLookup(context, buildingId, true);
            _LookupControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(_LookupControl);

            _LookupControl.SupplierSelectedEvent += LookupControl_SupplierSelectedEvent;
        }

        private void LookupControl_SupplierSelectedEvent(object sender, SupplierSelectEventArgs e)
        {
            SelectedSupplier = e.SelectedItem;
            if (e.SupplierSelected)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmSupplierLookup_FormClosed(object sender, FormClosedEventArgs e)
        {
            _LookupControl.SupplierSelectedEvent -= LookupControl_SupplierSelectedEvent;
        }
    }
}

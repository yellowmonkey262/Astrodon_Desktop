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
    public partial class frmSupplierLookup : Form
    {
        private DataContext _DataContext;

        public frmSupplierLookup()
        {
            InitializeComponent();
            _DataContext = new DataContext();

            usrSupplierLookup lookupControl = new usrSupplierLookup(_DataContext);
            lookupControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(lookupControl);
        }

        protected override void OnClosed(EventArgs e)
        {
            _DataContext.Dispose();
            base.OnClosed(e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

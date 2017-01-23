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
        //private DataContext _DataContext;

        public frmSupplierDetail()
        {
            InitializeComponent();
            //_DataContext = new DataContext();

            //usrSupplierDetail lookupControl = new usrSupplierDetail(_DataContext);
            //lookupControl.Dock = DockStyle.Fill;
            //pnlContents.Controls.Add(lookupControl);
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    _DataContext.Dispose();
        //    base.OnClosed(e);
        //}
    }
}

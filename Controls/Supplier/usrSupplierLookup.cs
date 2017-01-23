using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;

namespace Astrodon.Controls.Supplier
{

    public partial class usrSupplierLookup : UserControl
    {
        public usrSupplierLookup(DataContext context)
        {
            InitializeComponent();
        }

        #region Supplier Lookup Events
        public event SupplierSelectedEventHandler SupplierSelectedEvent;

        private void SupplierSelected(Astrodon.Data.SupplierData.Supplier selectedItem)
        {
            if (SupplierSelectedEvent != null)
                SupplierSelectedEvent(this, new SupplierEventArgs(selectedItem));
        }

        private void SupplierCancelled()
        {
            if (SupplierSelectedEvent != null)
                SupplierSelectedEvent(this, new SupplierEventArgs());
        }
        #endregion
    }
}

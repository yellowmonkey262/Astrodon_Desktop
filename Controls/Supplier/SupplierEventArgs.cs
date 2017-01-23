using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Controls.Supplier
{
    public delegate void SupplierSelectedEventHandler(object sender, SupplierEventArgs e);

    public class SupplierEventArgs : EventArgs
    {
        public SupplierEventArgs(Astrodon.Data.SupplierData.Supplier supplier)
        {
            SelectedItem = supplier;
            SupplierSelected = true;
        }

        public SupplierEventArgs()
        {
            SupplierSelected = false;
        }

        public Astrodon.Data.SupplierData.Supplier SelectedItem { get; set; }

        public bool SupplierSelected { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Controls.Supplier
{
    public delegate void SupplierSelectedEventHandler(object sender, SupplierSelectEventArgs e);

    public class SupplierSelectEventArgs : EventArgs
    {
        public SupplierSelectEventArgs(Astrodon.Data.SupplierData.Supplier supplier)
        {
            SelectedItem = supplier;
            SupplierSelected = true;
        }

        public SupplierSelectEventArgs()
        {
            SupplierSelected = false;
        }

        public Astrodon.Data.SupplierData.Supplier SelectedItem { get; set; }

        public bool SupplierSelected { get; private set; }
    }
}

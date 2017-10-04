using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Controls.Insurance
{
    public delegate void InsuranceBrokerSelectedEventHandler(object sender, InsuranceBrokerSelectEventArgs e);

    public class InsuranceBrokerSelectEventArgs : EventArgs
    {
        public InsuranceBrokerSelectEventArgs(Astrodon.Data.InsuranceData.InsuranceBroker InsuranceBroker)
        {
            SelectedItem = InsuranceBroker;
            InsuranceBrokerSelected = true;
        }

        public InsuranceBrokerSelectEventArgs()
        {
            InsuranceBrokerSelected = false;
        }

        public Astrodon.Data.InsuranceData.InsuranceBroker SelectedItem { get; set; }

        public bool InsuranceBrokerSelected { get; private set; }
    }
}

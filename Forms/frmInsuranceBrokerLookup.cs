using Astrodon.Controls.Insurance;
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
    public partial class frmInsuranceBrokerLookup : Form
    {
        private usrInsuranceBrokerLookup _LookupControl;

        public Data.InsuranceData.InsuranceBroker SelectedInsuranceBroker { get ; private set; }

        public frmInsuranceBrokerLookup(DataContext context)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            _LookupControl = new usrInsuranceBrokerLookup(context, true);
            _LookupControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(_LookupControl);

            _LookupControl.InsuranceBrokerSelectedEvent += LookupControl_InsuranceBrokerSelectedEvent;
        }

        private void LookupControl_InsuranceBrokerSelectedEvent(object sender, InsuranceBrokerSelectEventArgs e)
        {
            SelectedInsuranceBroker = e.SelectedItem;
            if (e.InsuranceBrokerSelected)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmInsuranceBrokerLookup_FormClosed(object sender, FormClosedEventArgs e)
        {
            _LookupControl.InsuranceBrokerSelectedEvent -= LookupControl_InsuranceBrokerSelectedEvent;
        }
    }
}

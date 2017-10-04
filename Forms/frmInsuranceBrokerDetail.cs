using Astrodon.Controls.Events;
using Astrodon.Controls.Insurance;
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
    public partial class frmInsuranceBrokerDetail : Form
    {
        private DataContext _DataContext;
        private usrInsuranceBrokerDetail _InsuranceBrokerDetailControl;

        public frmInsuranceBrokerDetail(DataContext context, int InsuranceBrokerId)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            _DataContext = context;

            _InsuranceBrokerDetailControl = new usrInsuranceBrokerDetail(_DataContext, InsuranceBrokerId,true);
            _InsuranceBrokerDetailControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(_InsuranceBrokerDetailControl);

            _InsuranceBrokerDetailControl.SaveResultEvent += _InsuranceBrokerDetailControl_SaveResultEvent; ;
        }

        private void _InsuranceBrokerDetailControl_SaveResultEvent(object sender, SaveResultEventArgs e)
        {
            if (e.Success)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;

            Close();
        }

        private void frmInsuranceBrokerDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            _InsuranceBrokerDetailControl.SaveResultEvent -= _InsuranceBrokerDetailControl_SaveResultEvent;
        }
    }
}

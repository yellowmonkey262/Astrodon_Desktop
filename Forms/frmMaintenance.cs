using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Entity;
using Astrodon.Controls.Maintenance;
using Astrodon.Data;
using Astrodon.Data.MaintenanceData;
using Astrodon.Data.SupplierData;

namespace Astrodon.Forms
{
    public partial class frmMaintenance : Form
    {
        private usrCaptureMaintenance _LookupControl;
     
        public frmMaintenance()
        {
            InitializeComponent();
        }

        public frmMaintenance(DataContext context, tblRequisition item, BuildingMaintenanceConfiguration config)
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;
            _LookupControl = new usrCaptureMaintenance(context, item, config);
            _LookupControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(_LookupControl);
        }

        public static bool CaptureMaintenanceRecord(DataContext context, tblRequisition item, BuildingMaintenanceConfiguration config)
        {
            var frm = new frmMaintenance(context, item, config);
            var dialogResult = frm.ShowDialog();

            return dialogResult == DialogResult.OK;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.MaintenanceData;
using Astrodon.Data.SupplierData;

namespace Astrodon.Controls.Maintenance
{
    public partial class usrCaptureMaintenance : UserControl
    {
        private BuildingMaintenanceConfiguration _config;
        private tblRequisition _requisition;

        public usrCaptureMaintenance(DataContext context, tblRequisition requisition, BuildingMaintenanceConfiguration config)
        {
            InitializeComponent();
            _requisition = requisition;
            _config = config;
        }

       
    }
}

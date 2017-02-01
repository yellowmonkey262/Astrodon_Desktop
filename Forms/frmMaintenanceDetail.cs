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
    public partial class frmMaintenanceDetail : Form
    {
        private usrMaintenanceDetail _MaintenanceDetailControl;
     
        public frmMaintenanceDetail(DataContext context, tblRequisition item, BuildingMaintenanceConfiguration config)
        {
            DialogResult = DialogResult.Cancel;
            
            try
            {
                InitializeComponent();

                _MaintenanceDetailControl = new usrMaintenanceDetail(context, item, config);
                _MaintenanceDetailControl.Dock = DockStyle.Fill;
                pnlContents.Controls.Add(_MaintenanceDetailControl);

                _MaintenanceDetailControl.SaveResultEvent += _MaintenanceDetailControl_SaveResultEvent;
            }
            catch (MaintenanceException e)
            {
                Controller.HandleError(e.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public frmMaintenanceDetail(DataContext context, int maintenanceId)
        {
            DialogResult = DialogResult.Cancel;
            
            try
            {
                InitializeComponent();

                _MaintenanceDetailControl = new usrMaintenanceDetail(context, maintenanceId);
                _MaintenanceDetailControl.Dock = DockStyle.Fill;
                pnlContents.Controls.Add(_MaintenanceDetailControl);

                _MaintenanceDetailControl.SaveResultEvent += _MaintenanceDetailControl_SaveResultEvent;
            }
            catch (MaintenanceException e)
            {
                Controller.HandleError(e.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void _MaintenanceDetailControl_SaveResultEvent(object sender, Controls.Events.SaveResultEventArgs e)
        {
            if (e.Success)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}

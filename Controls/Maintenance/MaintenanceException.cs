using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Controls.Maintenance
{
    public class MaintenanceException : Exception
    {
        public MaintenanceException(string message) : base(message)
        {

        }
    }
}

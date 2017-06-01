using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Controls.Events
{
    public delegate void SaveResultEventHandler(object sender, SaveResultEventArgs e);

    public class SaveResultEventArgs : EventArgs
    {
        public SaveResultEventArgs(bool success = false)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Controls.Supplier
{
    public delegate void SaveSuccessEventHandler(object sender, SaveSuccessEventArgs e);

    public class SaveSuccessEventArgs : EventArgs
    {
        public SaveSuccessEventArgs(bool saveSuccess = false)
        {
            SaveSuccess = saveSuccess;
        }

        public bool SaveSuccess { get; private set; }
    }
}

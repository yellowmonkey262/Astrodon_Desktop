using System;
using System.Windows.Forms;
using System.Linq;

namespace Astrodon {

    internal static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Controller.RunProgram();
            SqlDataHandler.MigrateEFDataBase();
            Application.Run();
        }
    }
}
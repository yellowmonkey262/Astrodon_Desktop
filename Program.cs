using System;
using System.Windows.Forms;
using System.Linq;
using Astrodon.Letter;

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
            //   SqlDataHandler.MigrateEFDataBase();

            iTextSharp.text.pdf.PdfReader.unethicalreading = true;


            Application.Run();
        }
    }
}
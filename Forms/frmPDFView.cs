using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Astrodon.Forms
{
    public partial class frmPDFView : Form
    {
        public frmPDFView()
        {
            InitializeComponent();
        }

        public static void PreviewPDF(byte[] fileData)
        {
            var frm = new frmPDFView();
            frm.DisplayPDF(fileData);
            frm.ShowDialog();
        }

        private string _TempPDFNewFile = string.Empty;
        public void DisplayPDF(byte[] pdfData)
        {
            if (pdfData == null)
            {
                this.axAcroPDFNew.Visible = false;
                return;
            }
            if (!String.IsNullOrWhiteSpace(_TempPDFNewFile))
                File.Delete(_TempPDFNewFile);
            _TempPDFNewFile = Path.GetTempPath();
            if (!_TempPDFNewFile.EndsWith(@"\"))
                _TempPDFNewFile = _TempPDFNewFile + @"\";

            _TempPDFNewFile = _TempPDFNewFile + System.Guid.NewGuid().ToString("N") + ".pdf";
            File.WriteAllBytes(_TempPDFNewFile, pdfData);
            try
            {
                this.axAcroPDFNew.Visible = true;
                this.axAcroPDFNew.LoadFile(_TempPDFNewFile);
                this.axAcroPDFNew.src = _TempPDFNewFile;
                this.axAcroPDFNew.setShowToolbar(false);
                this.axAcroPDFNew.setView("FitH");
                this.axAcroPDFNew.setLayoutMode("SinglePage");
                this.axAcroPDFNew.setShowToolbar(false);

                this.axAcroPDFNew.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            File.Delete(_TempPDFNewFile);
        }

      
    }
}

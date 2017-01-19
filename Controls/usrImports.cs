using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Astrodon {

    public partial class usrImports : UserControl {
        private SqlDataHandler dh = new SqlDataHandler();
        private int lines = 0;
        private int processedLines = 0;
        private int pastelPeriod = 0;

        public usrImports() {
            InitializeComponent();
        }

        private void usrImports_Load(object sender, EventArgs e) {
        }

        private void btnSelect_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                if (!String.IsNullOrEmpty(ofd.FileName) && File.Exists(ofd.FileName)) {
                    String fileName = ofd.FileName;
                    if (fileName.Contains(".xls")) {
                        txtFileName.Text = fileName;
                    } else {
                        MessageBox.Show("Please select an Excel file!", "Imports");
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e) {
            if (int.TryParse(txtPeriod.Text, out pastelPeriod) && pastelPeriod >= 1 && pastelPeriod <= 12 && !String.IsNullOrEmpty(txtFileName.Text)) {
                ReportWriter rw = new ReportWriter();
                String errors = "";
                List<Dictionary<String, String>> contents = rw.ExtractData(txtFileName.Text, out errors);
                lines = contents.Count;
                if (errors != "") { txtProgress.Text += errors + Environment.NewLine; }
                txtProgress.Text = "Starting processing" + Environment.NewLine;
                Application.DoEvents();
                txtProgress.Text += lines.ToString() + " in Excel file" + Environment.NewLine;
                MessageBox.Show("Extract Completed");
                Application.DoEvents();
                ProcessContents(contents);
                txtProgress.Text += processedLines.ToString() + " processed" + Environment.NewLine;
                Application.DoEvents();
                txtProgress.Text += "Completed" + Environment.NewLine;
                Application.DoEvents();
            } else if (!int.TryParse(txtPeriod.Text, out pastelPeriod) || pastelPeriod < 1 || pastelPeriod > 12) {
                MessageBox.Show("Please enter a valid period");
            } else {
                MessageBox.Show("Please select an Excel file!", "Imports");
            }
        }

        private void ProcessContents(List<Dictionary<String, String>> contents) {
            List<Building> buildings = new Buildings(false).buildings;
            processedLines = 0;
            int enterProcess = 0;

            foreach (Dictionary<String, String> content in contents) {
                enterProcess++;
                String errorKey = "";
                try {
                    errorKey = "building";
                    String building = content["BUILDING"].Trim();
                    errorKey = "unit";
                    String unit = content["UNIT"].Trim();
                    DateTime trnDate = DateTime.Now;
                    errorKey = "date 1";
                    try { trnDate = DateTime.Parse(content["POST DATE"]); } catch { MessageBox.Show("Error in post date: " + content["POST DATE"]); }
                    //MessageBox.Show(trnDate.ToString());
                    errorKey = "type";
                    String description = content["TYPE"].Trim() + " ";
                    errorKey = "reading";
                    description += content["READING"].Trim() + " ";
                    String readDate = "";
                    errorKey = "date 2";
                    try { readDate = DateTime.Parse(content["READ DATE"]).ToString("dd/MM/yyyy"); } catch { }
                    description += readDate;
                    errorKey = "fee";
                    String amt = content["FEE"].Trim();
                    errorKey = "contra";
                    String contra = content["CONTRA"].Trim();
                    String dataPath = String.Empty;
                    int trustPeriod = 0;
                    int buildPeriod = 0;
                    int journalType = 0;
                    foreach (Building b in buildings) {
                        if (b.Abbr == building) {
                            dataPath = b.DataPath;
                            trustPeriod = Utilities.getPeriod(trnDate, b.Period, out buildPeriod);
                            journalType = b.Journal;
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(dataPath)) { continue; }
                    String returner = String.Empty;
                    String strIn;
                    if (building == "RENT") {
                        double amount = double.Parse(amt);
                        amount = amount * -1;
                        String supamt = amount.ToString();
                        PostLine(trnDate, dataPath, journalType, pastelPeriod, unit, "0000000", unit, description, amt, true, out strIn, out returner);
                        PostLine(trnDate, dataPath, journalType, pastelPeriod, contra, "0000000", unit, description, supamt, false, out strIn, out returner);
                    } else {
                        PostLine(trnDate, dataPath, journalType, pastelPeriod, unit, contra.Replace("/", ""), unit, description, amt, true, out strIn, out returner);
                    }
                    if (returner != "0") {
                        MessageBox.Show(enterProcess.ToString() + ": " + returner + " - " + strIn);
                    } else {
                        processedLines += 1;
                    }
                } catch (Exception ex) {
                    MessageBox.Show("key before = " + errorKey + " - " + ex.Message);
                }
            }
        }

        private void PostLine(DateTime trnDate, String buildPath, int journalType, int buildPeriod, String debit, String credit, String reference, String description, String amt, bool customer, out String StrIn, out String returner) {
            if (customer) {
                Controller.pastel.PostBuildBatchDirect(trnDate, buildPath, journalType, buildPeriod, debit, credit, reference, description, amt, out StrIn, out returner);
            } else {
                StrIn = "";
                Controller.pastel.PostBuildBatchC(trnDate, buildPath, journalType, buildPeriod, debit, credit, reference, description, amt, out returner);
            }
        }
    }
}
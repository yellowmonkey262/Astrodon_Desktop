using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon {

    public partial class usrImportBank : UserControl {
        private Dictionary<String, Building> buildings;
        private int trustPeriod;
        private SqlDataHandler dh;

        public usrImportBank() {
            InitializeComponent();
            List<Building> buildingList = new Buildings(false).buildings;
            buildings = new Dictionary<string, Building>();
            foreach (Building b in buildingList) { buildings.Add(b.Abbr, b); }
            trustPeriod = Utilities.getPeriod(DateTime.Now);
            dh = new SqlDataHandler();
        }

        private void usrImportBank_Load(object sender, EventArgs e) {
            txtReconPeriod.Text = trustPeriod.ToString();
        }

        private void btnUpload_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            String uploadDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
            if (!Directory.Exists(uploadDirectory)) { Directory.CreateDirectory(uploadDirectory); }
            if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName)) {
                if (File.Exists(ofd.FileName)) {
                    try {
                        String fileName = Path.Combine(uploadDirectory, Path.GetFileName(ofd.FileName));
                        File.Copy(ofd.FileName, fileName, true);
                        if (File.Exists(fileName) && !lstFiles.Items.Contains(fileName)) { lstFiles.Items.Add(fileName); }
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message, "Imports", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAllocate_Click(object sender, EventArgs e) {
            LoadFiles();
            Match();
            MatchRental();
        }

        private void LoadFiles() {
            foreach (Object obj in lstFiles.Items) {
                String fileName = obj.ToString();
                importFiles(fileName);
            }
        }

        private void importFiles(String fileName) {
            try {
                StreamReader objReader = new StreamReader(fileName);
                ArrayList contents = new ArrayList();
                int whichLine = 0;
                String[] lineBreaker = new String[] { "," };
                String accNumber = "";
                String accDescription = "";
                String statementNumber = "";
                while (!objReader.EndOfStream) {
                    String strLine = objReader.ReadLine();
                    if (strLine.ToLower().Contains("prepared by")) {
                        objReader.Close();
                        loadExcelFile(fileName);
                        break;
                    }
                    String[] lineContents = strLine.Split(lineBreaker, StringSplitOptions.RemoveEmptyEntries);
                    switch (whichLine) {
                        case 0:
                            break;

                        case 1:
                            accNumber = lineContents[1];
                            break;

                        case 2:
                            accDescription = lineContents[1];
                            break;

                        case 3:
                            statementNumber = lineContents[1];
                            break;

                        default:
                            String trnDate = Utilities.cleanDate(lineContents[0]);
                            String description = Utilities.cleanDescription(lineContents[1]);
                            String rawAmount = lineContents[2];
                            String rawBalance = lineContents[3];
                            importMe(accNumber, accDescription, statementNumber, trnDate, description, rawAmount, rawBalance);
                            break;
                    }
                    whichLine++;
                }
                String str = " DELETE FROM tblLedgerTransactions WHERE Description = 'BROUGHT FORWARD' OR Description = 'CARRIED FORWARD' OR Description = 'PROVISIONAL STATEMENT' ";
                String status;
                dh.SetData(str, null, out status);
                objReader.Close();
                File.Delete(fileName);
                lstFiles.Items.Remove(fileName);
                txtProgress.Text += "Import " + fileName + " Successful" + Environment.NewLine;
            } catch (Exception ex) {
                txtProgress.Text += fileName + ":" + ex.Message + Environment.NewLine;
            }
        }

        private void loadExcelFile(String fileName) {
            StreamReader objReader = new StreamReader(fileName);
            ArrayList contents = new ArrayList();
            int whichLine = 1;
            String[] lineBreaker = new String[] { "," };
            while (!objReader.EndOfStream) {
                String strLine = objReader.ReadLine();
                String[] lineContents = strLine.Split(lineBreaker, StringSplitOptions.None);
                try {
                    if (whichLine >= 7) {
                        String tranDate = Utilities.cleanDate(lineContents[0]);
                        String[] dateSplitter = new String[] { "/" };
                        String[] dateBits = tranDate.Split(dateSplitter, StringSplitOptions.None);
                        DateTime trnDate = new DateTime(int.Parse(dateBits[2]), int.Parse(dateBits[1]), int.Parse(dateBits[0]));
                        String reference = lineContents[2];
                        String description = lineContents[4];
                        double debit = 0;
                        double credit = 0;
                        double cumulative = 0;
                        double.TryParse(lineContents[5], out debit);
                        double.TryParse(lineContents[6], out credit);
                        if (lineContents.Length > 7) { double.TryParse(lineContents[7], out cumulative); }
                        importRental(trnDate, reference, description, debit, credit, cumulative);
                    }
                } catch {
                    continue;
                }
                whichLine++;
            }
            objReader.Close();
            File.Delete(fileName);
            lstFiles.Items.Remove(fileName);
            txtProgress.Text += "Import " + fileName + " Successful" + Environment.NewLine;
        }

        private void importMe(String accNumber, String accDescription, String statementNr, String trnDate, String description, String amount, String balance) {
            String status;
            String str = " BEGIN TRAN ";
            str += " IF NOT EXISTS (SELECT * FROM tblLedgerTransactions WHERE AccNumber = '" + accNumber + "' AND AccDescription = '" + accDescription + "' AND ";
            str += " StatementNr = '" + statementNr + "' AND Date = '" + trnDate + "' AND Description = '" + description + "' AND Amount = '" + amount;
            str += "' AND Balance = '" + balance + "') ";
            str += " BEGIN ";
            str += " INSERT INTO tblLedgerTransactions (AccNumber, AccDescription, StatementNr, Date, Description, Amount, Balance, Allocate) VALUES ";
            str += " ('" + accNumber + "', '" + accDescription + "', '" + statementNr + "', '" + trnDate + "', '" + description + "', '" + amount;
            str += "', '" + balance + "', 0) ";
            str += " END  ";
            str += " COMMIT TRAN";
            dh.SetData(str, null, out status);
        }

        private void importRental(DateTime trnDate, String reference, String description, double drValue, double crValue, double cumValue) {
            String status;
            String str = " BEGIN TRAN ";
            str += " IF NOT EXISTS (SELECT * FROM tblRentals WHERE trnDate = '" + trnDate + "' AND reference = '" + reference + "' AND ";
            str += " description = '" + description + "' AND drValue = " + drValue + " AND crValue = " + crValue + " AND cumValue = " + cumValue + ") ";
            str += " BEGIN ";
            str += " INSERT INTO tblRentals (trnDate, reference, description, drValue, crValue, cumValue) VALUES ";
            str += " ('" + trnDate + "', '" + reference + "', '" + description + "', '" + drValue + "', '" + crValue + "', '" + cumValue + "') ";
            str += " END  ";
            str += " COMMIT TRAN";
            dh.SetData(str, null, out status);
        }

        private void Match() {
            String status;
            try {
                DataSet trustDS = dh.GetData(" SELECT * FROM tblLedgerTransactions WHERE Allocate = '0'", null, out status);
                String Numbers = "0123456789";
                String Reference = "";
                String Cash1 = "";
                String b = "";
                String code = "";
                foreach (DataRow trustRow in trustDS.Tables[0].Rows) {
                    int startIndex = 0;
                    int CodeLength = 0;
                    String trnCode = "";
                    String Descript = trustRow["Description"].ToString().Trim();
                    String Description = Descript.Replace(" ", "");
                    String AccNumber = "";
                    int DescrpLength = Description.Length;
                    bool isPayment = false;
                    double amount = double.Parse(trustRow["Amount"].ToString());
                    if (DescrpLength > 5) { Cash1 = Description.Substring(0, 6); } else { Cash1 = ""; }
                    bool changeRef = false;
                    if (Cash1 != "BRCASH" && Cash1 != "CASHTR" && !Description.Contains("INTEREST")) {
                        code = SecondPass(Descript);
                        if (code == "") {
                            String testMatch = GetMatch(Descript);
                            if (!String.IsNullOrEmpty(testMatch)) {
                                Reference = testMatch;
                                code = SecondPass(testMatch);
                                changeRef = true;
                            }
                        }
                        AccNumber = buildings[code].Trust;
                        trnCode = "";
                    } else {
                        trnCode = "trn";
                    }
                    bool matched = false;
                    startIndex = -1;
                    if (code == "" && trnCode == "") {
                        startIndex = -1;
                    } else if (trnCode != "trn") {
                        CodeLength = code.Length;
                        if (amount < 0) {
                            startIndex = Description.IndexOf(AccNumber.Substring(0, 4));
                            isPayment = true;
                            int accStart = Description.IndexOf(AccNumber.Substring(0, 4)) + 5;
                            Reference = Description.Substring(accStart, Description.Length - accStart).Replace("/", "");
                        } else {
                            startIndex = Description.IndexOf(code);
                            if (startIndex == -1 && code != "") {
                                startIndex = Description.IndexOf(buildings[code].Name);
                            }
                        }
                    }
                    if (code != "") {
                        String test = Reference;
                        if (startIndex != -1 || Cash1 == "BRCASH" || Cash1 == "CASHTR" && test != "") {
                            if (Cash1 == "BRCASH" || Cash1 == "CASHTR") {
                                Reference = Reference;
                            } else {
                                if (!changeRef) {
                                    if (!isPayment) {
                                        if (!Description.Contains(code)) {
                                            Reference = code;
                                        } else {
                                            String outRef = code;
                                            int codeIndex = Description.IndexOf(code);
                                            for (int ei = codeIndex + code.Length; ei < Description.Length; ei++) {
                                                String nextChar = Description.Substring(ei, 1);
                                                if (Numbers.Contains(nextChar)) {
                                                    outRef += nextChar;
                                                }
                                            }
                                            Reference = outRef;
                                        }
                                    }
                                }
                            }
                            if (Reference != "") {
                                double amt, balance;
                                if (!double.TryParse(trustRow["Amount"].ToString().Trim(), out amt)) { amt = 0; }
                                if (!double.TryParse(trustRow["Balance"].ToString().Trim(), out balance)) { balance = 0; }
                                String str = " BEGIN TRAN ";
                                str += " IF NOT EXISTS (SELECT * FROM tblDevision WHERE Date = '" + trustRow["Date"].ToString().Trim() + "' AND ";
                                str += " Description = '" + Descript + "' AND ";
                                str += " Amount = '" + amt.ToString() + "' AND ";
                                str += " Balance = '" + balance.ToString() + "' AND ";
                                str += " FromAccNumber = '" + trustRow["AccNumber"].ToString().Trim() + "' AND ";
                                str += " AccDescription = '" + trustRow["AccDescription"].ToString().Trim() + "' AND ";
                                str += " StatementNr = '" + trustRow["StatementNr"].ToString().Trim() + "') ";
                                str += "INSERT INTO tblDevision (Date, Description, Amount, Balance, FromAccNumber, AccDescription, StatementNr, ";
                                str += " Allocate, Reference, Building, AccNumber, Period, lid) VALUES ";
                                str += " ('" + trustRow["Date"].ToString().Trim() + "', ";
                                str += "'" + Descript + "', ";
                                str += "'" + amt.ToString() + "', ";
                                str += "'" + balance.ToString() + "', ";
                                str += "'" + trustRow["AccNumber"].ToString().Trim() + "', ";
                                str += "'" + trustRow["AccDescription"].ToString().Trim() + "', ";
                                str += "'" + trustRow["StatementNr"].ToString().Trim() + "', ";
                                str += "'1', '" + Reference + "', ";
                                //RENT
                                if (Description.Contains("INTEREST")) {
                                    str += "'126', '9320000', ";
                                } else if (Description.Contains("D/") && Description.EndsWith("R")) {
                                    code = "RENT";
                                    str += "'" + buildings[code].ID + "', ";
                                    str += "'" + buildings[code].Trust + "', ";
                                } else {
                                    str += "'" + buildings[code].ID + "', ";
                                    str += "'" + buildings[code].Trust + "', ";
                                }
                                str += trustPeriod.ToString() + ", ";
                                str += "'" + trustRow["id"].ToString().Trim() + "')";
                                str += " ELSE ";
                                str += " UPDATE tblDevision SET posted = 'False' WHERE Date = '" + trustRow["Date"].ToString().Trim() + "' AND ";
                                str += " Description = '" + trustRow["Description"].ToString().Trim() + "' AND ";
                                str += " Amount = '" + amt.ToString() + "' AND ";
                                str += " Balance = '" + balance.ToString() + "' AND ";
                                str += " FromAccNumber = '" + trustRow["AccNumber"].ToString().Trim() + "' AND ";
                                str += " AccDescription = '" + trustRow["AccDescription"].ToString().Trim() + "' AND ";
                                str += " StatementNr = '" + trustRow["StatementNr"].ToString().Trim() + "'";

                                str += " UPDATE tblLedgerTransactions SET Allocate = '1' WHERE Date = '" + trustRow["Date"].ToString().Trim() + "' AND ";
                                str += " Description = '" + trustRow["Description"].ToString().Trim() + "' AND ";
                                str += " Amount = '" + amt.ToString() + "' AND ";
                                str += " Balance = '" + balance.ToString() + "' AND ";
                                str += " AccNumber = '" + trustRow["AccNumber"].ToString().Trim() + "' AND ";
                                str += " AccDescription = '" + trustRow["AccDescription"].ToString().Trim() + "' AND ";
                                str += " StatementNr = '" + trustRow["StatementNr"].ToString().Trim() + "'";
                                str += " COMMIT TRAN";
                                dh.SetData(str, null, out status);
                            }
                        } else {
                            code = "";
                        }
                    }
                }
            } catch (Exception ex) {
                txtProgress.Text += "Allocation error: " + ex.Message + Environment.NewLine;
            }

            String loadExport1 = "UPDATE d SET d.building = b.id FROM tblDevision d JOIN tblBuildings b ON d.building = b.code or d.Building = b.building;";
            dh.SetData(loadExport1, null, out status);

            String loadExport = "INSERT INTO tblExport(lid, trnDate, amount, building, code, description, reference, accnumber, contra, datapath, period, una)";
            loadExport += " SELECT tblDevision.lid, tblDevision.Date, tblDevision.Amount, tblBuildings.Building, tblBuildings.Code, ";
            loadExport += " tblDevision.Description, tblDevision.Reference, tblBuildings.AccNumber, tblBuildings.Contra, tblBuildings.DataPath, tblDevision.period, 0";
            loadExport += " FROM tblDevision INNER JOIN tblBuildings ON tblDevision.Building = tblBuildings.id WHERE (tblDevision.posted = 'False') ";
            loadExport += " AND (tblDevision.lid NOT IN (SELECT lid FROM tblExport AS tblExport_1));";
            txtProgress.Text = dh.SetData(loadExport, null, out status).ToString() + " entries imported" + Environment.NewLine;
        }

        private String SecondPass(String description) {
            String fullDesc = description.Replace(" ", "");
            int x = 0;
            String testChar = fullDesc.Substring(x, 1);
            String codeString = "";
            int unitNumber = 0;
            while (!int.TryParse(testChar, out unitNumber)) {
                codeString += testChar;
                if (fullDesc.Length - 1 > x + 1) {
                    x++;
                    testChar = fullDesc.Substring(x, 1);
                } else {
                    break;
                }
            }
            String twoChar = "";
            String threeChar = "";
            String bCode = "";
            if (x >= 2) { twoChar = codeString.Substring(x - 2, 2); }
            if (x >= 3) { threeChar = codeString.Substring(x - 3, 3); }
            if (threeChar != "") { bCode = GetExactCode(threeChar); }
            if (bCode == "" && twoChar != "") {
                bCode = GetExactCode(twoChar);
            }
            if ((bCode == "") && (fullDesc.Contains("(") && description.Contains(")") && description.Contains("/"))) {
                String testAccount = fullDesc.Substring(fullDesc.IndexOf("(") + 1, 4);
                if (testAccount == "9374") { string breakHere = ""; }
                bCode = GetAccNumber(testAccount);
            }
            if (bCode == "") {
                String[] stringSplit = new String[] { " " };

                String[] descBits = description.Split(stringSplit, StringSplitOptions.RemoveEmptyEntries);

                foreach (String descBit in descBits) {
                    bCode = GetBuilding(descBit);
                    if (bCode != "") { break; }
                }
            }
            return bCode;
        }

        private String GetAccNumber(String Code) {
            String status;
            String str = "SELECT Code FROM tblBuildings WHERE AccNumber LIKE '%" + Code + "%'";
            DataSet idDS = dh.GetData(str, null, out status);
            if (idDS != null) {
                try { return idDS.Tables[0].Rows[0]["Code"].ToString().Trim(); } catch (Exception ex) { return ""; }
            } else {
                return "";
            }
        }

        private String GetBuilding(String Code) {
            String status;
            String str = "SELECT Code FROM tblBuildings WHERE Building LIKE '" + Code + "%'";
            DataSet idDS = dh.GetData(str, null, out status);
            if (idDS != null) {
                try { return idDS.Tables[0].Rows[0]["Code"].ToString().Trim(); } catch (Exception ex) { return ""; }
            } else {
                return "";
            }
        }

        private String GetExactCode(String code) {
            if (buildings.Keys.Contains(code)) { return code; } else { return ""; }
        }

        private String GetMatch(String statementRef) {
            String status;
            String astroRef = String.Empty;
            String query = "SELECT astroRef FROM tblMatch WHERE statementRef = '" + statementRef + "'";
            DataSet mDS = dh.GetData(query, null, out status);
            if (mDS != null && mDS.Tables.Count > 0 && mDS.Tables[0].Rows.Count > 0) {
                astroRef = mDS.Tables[0].Rows[0]["astroRef"].ToString();
            }
            return astroRef;
        }

        private void MatchRental() {
            String status;
            String str = " INSERT INTO tblRentalRecon (rentalId, trnDate, value, account, contra)";
            str += " SELECT tblRentals.id, tblRentals.trnDate, CASE WHEN drValue = 0 THEN crvalue ELSE drvalue * - 1 END AS value, ";
            str += " tblRentalAccounts.crAccount, tblRentalAccounts.crContra FROM tblRentals INNER JOIN tblRentalAccounts ON ";
            str += " tblRentals.description = tblRentalAccounts.description WHERE (tblRentalAccounts.crAccount IS NOT NULL) AND tblRentals.id not in";
            str += " (SELECT distinct rentalId FROM tblRentalRecon) AND tblRentalAccounts.crAccount <> 'NULL'";
            txtProgress.Text = dh.SetData(str, null, out status).ToString() + " rental lines imported" + Environment.NewLine;
        }
    }
}
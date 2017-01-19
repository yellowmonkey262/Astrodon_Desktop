using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class usrStatements : UserControl
    {
        private SqlDataHandler dh;
        private String status = String.Empty;
        private DataSet dsBuildings;
        private BindingSource bs;
        private int userid;
        private Statements statements;

        public usrStatements()
        {
            InitializeComponent();
            dh = new SqlDataHandler();
            bs = new BindingSource();
            userid = Controller.user.id;
            if (userid == 0) { userid = 1; }
            dgBuildings.DataSource = bs;
        }

        private void usrStatements_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            DateTime callDate = DateTime.Now.AddMonths(1);
            DateTime stmtDate = new DateTime(callDate.Year, callDate.Month, 1);
            stmtDatePicker.Value = stmtDate;
        }

        private void LoadBuildings()
        {
            String point = "0";
            String build = String.Empty;
            try
            {
                String query = String.Empty;
                try
                {
                    query = String.Format("SELECT DISTINCT b.Building, b.DataPath, b.Period, '' as [Last Processed], b.pm, b.bankName, b.accName, b.bankAccNumber, b.branch, b.bank FROM tblBuildings AS b INNER JOIN tblUserBuildings AS u ON b.id = u.buildingid WHERE u.userid = {0} ORDER BY b.Building", userid.ToString());
                }
                catch
                {
                    MessageBox.Show("query generator" + userid.ToString());
                }
                dsBuildings = dh.GetData(query, null, out status);
                if (dsBuildings != null && dsBuildings.Tables.Count > 0 && dsBuildings.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsBuildings.Tables[0].Rows)
                    {
                        String building = dr["Building"].ToString();
                        String lpQuery = String.Format("SELECT top(1) lastProcessed FROM tblStatements WHERE building = '{0}' ORDER BY lastProcessed DESC", building);
                        DataSet dsLP = dh.GetData(lpQuery, null, out status);
                        DateTime lastProcessed;
                        if (dsLP != null && dsLP.Tables.Count > 0 && dsLP.Tables[0].Rows.Count > 0)
                        {
                            lastProcessed = DateTime.Parse(dsLP.Tables[0].Rows[0]["lastProcessed"].ToString());
                        }
                        else
                        {
                            lastProcessed = DateTime.Now.AddYears(-1);
                        }
                        point = "1";
                        build = dr["Building"].ToString();
                        point = "2";
                        String dp = dr["DataPath"].ToString();
                        point = "3";
                        int p = int.Parse(dr["Period"].ToString());
                        point = "4";
                        StatementBuilding stmtBuilding = new StatementBuilding(build, dp, p, lastProcessed);
                        point = "5";
                        if (!bs.Contains(stmtBuilding)) { bs.Add(stmtBuilding); }
                        point = "6";
                    }
                }
            }
            catch
            {
                MessageBox.Show(point + " - " + build);
            }
        }

        private void dgBuildings_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgBuildings.Rows.Count == dsBuildings.Tables[0].Rows.Count)
                {
                    foreach (DataGridViewRow dvr in dgBuildings.Rows)
                    {
                        DateTime lastProcessed = DateTime.Parse(dvr.Cells[7].Value.ToString());
                        TimeSpan elapsed = DateTime.Now - lastProcessed;
                        for (int i = 0; i < dgBuildings.ColumnCount; i++)
                        {
                            if (elapsed.TotalDays > 5) { dvr.Cells[i].Style.BackColor = System.Drawing.Color.Red; } else { dvr.Cells[i].Style.BackColor = System.Drawing.Color.White; }
                        }
                    }
                }
            }
            catch { }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvr in dgBuildings.Rows)
            {
                dvr.Cells[0].Value = chkSelectAll.Checked;
            }
        }

        private String getDebtorEmail(String buildingName)
        {
            List<Building> testBuildings = new Buildings(false).buildings;
            String dEmail = "";
            foreach (Building b in testBuildings)
            {
                if (b.Name == buildingName)
                {
                    dEmail = b.Debtor;
                }
            }
            //MessageBox.Show("debtor = " + Controller.user.email + " and b email = " + dEmail);
            return (dEmail != "" ? dEmail : Controller.user.email);
        }

        private void SetBuildingStatement(String buildingName)
        {
            List<Building> testBuildings = new Buildings(false).buildings;
            int buildingID = 0;
            foreach (Building b in testBuildings)
            {
                if (b.Name == buildingName)
                {
                    buildingID = b.ID;
                    String query = "IF NOT EXISTS(SELECT id FROM tblDebtors WHERE buildingID = " + b.ID.ToString() + " AND completeDate = '" + DateTime.Now.ToString("yyyy/MM/dd") + "')";
                    query += " INSERT INTO tblDebtors(buildingID, completeDate, stmtprintemail) VALUES(" + b.ID.ToString() + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'True')";
                    dh.SetData(query, null, out status);
                    break;
                }
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            statements = new Statements();
            statements.statements = new List<Statement>();
            Dictionary<String, bool> hasStatements = new Dictionary<string, bool>();
            foreach (DataGridViewRow dvr in dgBuildings.Rows)
            {
                if ((bool)dvr.Cells[0].Value)
                {
                    String buildingName = dvr.Cells[1].Value.ToString();
                    SetBuildingStatement(buildingName);
                    String datapath = dvr.Cells[5].Value.ToString();
                    int period = (int)dvr.Cells[6].Value;
                    if (String.IsNullOrEmpty(buildingName)) { MessageBox.Show("name"); }
                    if (String.IsNullOrEmpty(datapath)) { MessageBox.Show("datapath"); }
                    if (String.IsNullOrEmpty(period.ToString())) { MessageBox.Show("period"); }
                    if (dvr.Cells[2].Value == null) { MessageBox.Show("ishoa"); }
                    List<Statement> bStatements = SetBuildings(buildingName, datapath, period, (bool)dvr.Cells[2].Value);
                    int idx = dvr.Index;
                    DataRow dr = dsBuildings.Tables[0].Rows[idx];
                    //pm, b.bankName, b.accName, b.bankAccNumber, b.branch
                    String pm = dr["pm"].ToString();
                    String bankName = dr["bankName"].ToString();
                    String accName = dr["accName"].ToString();
                    String bankAccNumber = dr["bankAccNumber"].ToString();
                    String branch = dr["branch"].ToString();
                    bool isStd = bankName.ToLower().Contains("standard");// dr["bank"].ToString() == "STANDARD";
                    foreach (Statement s in bStatements)
                    {
                        s.pm = pm;
                        s.bankName = bankName;
                        s.accName = accName;
                        s.accNumber = bankAccNumber;
                        s.branch = branch;
                        s.isStd = isStd;
                        statements.statements.Add(s);
                    }
                }
            }
            bool printerSet = false;
            PDF generator = new PDF(true);
            MySqlConnector mySqlConn = new MySqlConnector();
            Classes.Sftp ftpClient = new Classes.Sftp(String.Empty, true);
            foreach (Statement stmt in statements.statements)
            {
                String fileName = String.Empty;
                generator.CreateStatement(stmt, stmt.BuildingName != "ASTRODON RENTALS" ? true : false, out fileName, stmt.isStd);

                #region Upload Letter

                String actFileTitle = Path.GetFileNameWithoutExtension(fileName);
                String actFile = Path.GetFileName(fileName);
                //if (Controller.user.id != 1) {
                //    try {
                //        mySqlConn.InsertStatement(actFileTitle, "Customer Statements", actFile, stmt.AccNo, stmt.email1);
                //        ftpClient.Upload(fileName, actFile);
                //    } catch { }
                //}

                #endregion Upload Letter

                if (stmt.EmailMe)
                {
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        if (!hasStatements.ContainsKey(stmt.BuildingName)) { hasStatements.Add(stmt.BuildingName, true); }
                        if (Controller.user.id != 1)
                        {
                            SetupEmail(stmt, fileName);
                        }
                        if (stmt.PrintMe && Controller.user.id != 1)
                        {
                            if (!printerSet)
                            {
                                frmPrintDialog printDialog = new frmPrintDialog();
                                if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    SetDefaultPrinter(printDialog.selectedPrinter);
                                    Properties.Settings.Default.defaultPrinter = printDialog.selectedPrinter;
                                    Properties.Settings.Default.Save();
                                    printerSet = true;
                                }
                            }
                            SendToPrinter(fileName);
                        }
                    }
                }
                else if (stmt.PrintMe && Controller.user.id != 1)
                {
                    if (!printerSet)
                    {
                        frmPrintDialog printDialog = new frmPrintDialog();
                        if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            SetDefaultPrinter(printDialog.selectedPrinter);
                            Properties.Settings.Default.defaultPrinter = printDialog.selectedPrinter;
                            Properties.Settings.Default.Save();
                            printerSet = true;
                        }
                    }
                    SendToPrinter(fileName);
                    try
                    {
                        mySqlConn.InsertStatement(actFileTitle, "Customer Statements", actFile, stmt.AccNo, stmt.email1);
                        ftpClient.Upload(fileName, actFile, false);
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        mySqlConn.InsertStatement(actFileTitle, "Customer Statements", actFile, stmt.AccNo, stmt.email1);
                        ftpClient.Upload(fileName, actFile, false);
                    }
                    catch { }
                }
            }
            foreach (DataGridViewRow dvr in dgBuildings.Rows)
            {
                dvr.Cells[0].Value = false;
            }
            foreach (KeyValuePair<String, bool> hasStatement in hasStatements)
            {
                String query = "INSERT INTO tblStatements(building, lastProcessed) VALUES(@building, @lastProcessed)";
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@building", hasStatement.Key);
                sqlParms.Add("@lastProcessed", DateTime.Now);
                dh.SetData(query, sqlParms, out status);
            }
            this.Cursor = Cursors.Arrow;
            MessageBox.Show("Process Complete");
        }

        public List<Statement> SetBuildings(String buildingName, String buildingPath, int buildingPeriod, bool isHOA)
        {
            Building build = new Building();
            build.Name = buildingName;
            build.DataPath = buildingPath;
            build.Period = buildingPeriod;
            List<Customer> customers = Controller.pastel.AddCustomers(buildingName, buildingPath);
            List<Statement> myStatements = new List<Statement>();
            lblCCount.Text = build.Name + " 0/" + customers.Count.ToString();
            lblCCount.Refresh();
            int ccount = 0;
            foreach (Customer customer in customers)
            {
                try
                {
                    bool stophere = false;
                    Statement myStatement = new Statement();
                    myStatement.AccNo = customer.accNumber;
                    List<String> address = new List<string>();
                    address.Add(customer.description);
                    foreach (String addyLine in customer.address) { if (!String.IsNullOrEmpty(addyLine)) { address.Add(addyLine); } }
                    myStatement.Address = address.ToArray();
                    if (stophere) { MessageBox.Show("213"); }
                    myStatement.BankDetails = (!String.IsNullOrEmpty(Controller.pastel.GetBankDetails(buildingPath)) ? Controller.pastel.GetBankDetails(buildingPath) : "");
                    myStatement.BuildingName = buildingName;
                    myStatement.LevyMessage1 = (isHOA ? HOAMessage1 : BCMessage1);
                    myStatement.LevyMessage2 = (!String.IsNullOrEmpty(Message2) ? Message2 : "");
                    myStatement.Message = (!String.IsNullOrEmpty(txtMessage.Text) ? txtMessage.Text : "");
                    myStatement.StmtDate = stmtDatePicker.Value;
                    double totalDue = 0;
                    List<Transaction> transactions = (new Classes.LoadTrans()).LoadTransactions(build, customer, stmtDatePicker.Value, out totalDue);
                    if (transactions != null) { myStatement.Transactions = transactions; }
                    if (stophere) { MessageBox.Show("222"); }
                    myStatement.totalDue = totalDue;
                    myStatement.DebtorEmail = getDebtorEmail(buildingName);

                    myStatement.PrintMe = (customer.statPrintorEmail == 2 || customer.statPrintorEmail == 4 ? false : true);
                    myStatement.EmailMe = (customer.statPrintorEmail == 4 ? false : true);
                    if (customer.Email != null && customer.Email.Length > 0)
                    {
                        List<String> newEmails = new List<string>();
                        foreach (String emailAddress in customer.Email)
                        {
                            if (!emailAddress.Contains("@imp.ad-one.co.za")) { newEmails.Add(emailAddress); }
                        }
                        myStatement.email1 = newEmails.ToArray();
                    }
                    if (stophere) { MessageBox.Show("235"); }

                    myStatements.Add(myStatement);
                }
                catch { }
                ccount++;
                lblCCount.Text = build.Name + " " + ccount.ToString() + "/" + customers.Count.ToString();
                lblCCount.Refresh();
                Application.DoEvents();
            }
            return myStatements;
        }

        private void SetupEmail(Statement stmt, String fileName)
        {
            try
            {
                String query = "INSERT INTO tblStatementRun(email1, queueDate, fileName, debtorEmail, unit, attachment, subject) VALUES(@email1, @queueDate, @fileName, @debtorEmail, @unit, @attachment, @subject)";
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                String emailAddy = "";
                foreach (String addy in stmt.email1) { emailAddy += addy + ";"; }
                sqlParms.Add("@email1", emailAddy);
                sqlParms.Add("@queueDate", DateTime.Now);
                sqlParms.Add("@fileName", Path.GetFileName(fileName));
                sqlParms.Add("@debtorEmail", stmt.DebtorEmail);
                sqlParms.Add("@unit", stmt.AccNo + (stmt.BuildingName == "ASTRODON RENTALS" ? "R" : ""));
                sqlParms.Add("@subject", Path.GetFileNameWithoutExtension(fileName) + " " + DateTime.Now.ToString());
                String attachment = "none";
                if (!String.IsNullOrEmpty(txtAttachment.Text))
                {
                    attachment = txtAttachment.Text;
                    if (!attachment.StartsWith("K:"))
                    {
                        File.Copy(attachment, Path.Combine("K:\\Debtors System\\statement", Path.GetFileName(attachment)), true);
                        attachment = Path.GetFileName(attachment);
                    }
                }

                sqlParms.Add("@attachment", attachment);
                if (emailAddy != "")
                {
                    dh.SetData(query, sqlParms, out status);
                    if (!String.IsNullOrEmpty(status)) { MessageBox.Show(status); }
                }
            }
            catch { }
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        private void SendToPrinter(String fileName)
        { //C:\statement.pdf
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = fileName;
            //info.CreateNoWindow = true;
            //info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            //p.WaitForInputIdle();

            //if (false == p.CloseMainWindow()) {
            //p.Kill();
            //}
            System.Threading.Thread.Sleep(5000);
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(ofd.FileName) && File.Exists(ofd.FileName))
                {
                    txtAttachment.Text = ofd.FileName;
                }
            }
            else
            {
                txtAttachment.Text = "";
            }
        }

        public int getPeriod(DateTime trnDate, int buildPeriod)
        {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth);
            buildPeriod = (myMonth - buildPeriod < 1 ? myMonth - buildPeriod + 12 : myMonth - buildPeriod);
            return buildPeriod;
        }

        private String HOAMessage1
        {
            get
            {
                String hoaMessage = "Levies are due and payable on the 1st of every month in advance.  Failure to compy will result in penalties being charged and electricity supply to the unit being suspended and or restricted.";
                return hoaMessage;
            }
        }

        private String BCMessage1
        {
            get
            {
                String hoaMessage = "Levies are due and payable on the 1st of every month in advance.  Failure to compy will result in penalties being charged and electricity supply to the unit being suspended and or restricted.";
                return hoaMessage;
            }
        }

        private String Message2
        {
            get
            {
                String hoaMessage = "***PLEASE ENSURE THAT ALL PAYMENTS REFLECTS IN OUR NOMINATED ACCOUNT ON OR BEFORE DUE DATE TO AVOID ANY PENALTIES, REFER TO TERMS AND CONDITIONS.***";
                return hoaMessage;
            }
        }
    }

    public class StatementBuilding
    {
        private bool process = false;
        private String building = String.Empty;
        private String dataPath = String.Empty;
        private int period = 0;
        public bool hoa = false;
        public bool bc = false;
        private String lastProcessed = String.Empty;

        public bool Process
        {
            get { return process; }
            set { process = value; }
        }

        public String Building
        {
            get { return building; }
            set { building = value; }
        }

        public bool HOA
        {
            get { return hoa; }
        }

        public bool BC
        {
            get { return bc; }
        }

        public String LastProcessed
        {
            get { return lastProcessed; }
            set { lastProcessed = value; }
        }

        public String DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }

        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        public StatementBuilding(String build, String dp, int p, DateTime lastProcessed)
        {
            Process = false;
            Building = build;
            DataPath = dp;
            Period = p;
            LastProcessed = lastProcessed.ToString("yyyy/MM/dd");
            if (building.ToLower().Contains("hoa"))
            {
                hoa = true;
                bc = false;
            }
            else
            {
                hoa = false;
                bc = true;
            }
        }
    }
}
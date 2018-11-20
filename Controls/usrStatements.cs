using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using Astrodon.Forms;
using System.Drawing.Printing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading;
using Astrodon.ClientPortal;
using Astrodon.Data;
using Astrodon.Letter;

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
        private string _AstradonRentalsBuilding = "ASTRODON RENTALS";
        private AstrodonClientPortal _ClientPortal = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString());


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

            String query = "SELECT DISTINCT b.id, b.Building, b.DataPath, b.Period, '' as [Last Processed], b.pm, b.bankName, b.accName, b.bankAccNumber, b.branch, b.bank FROM tblBuildings AS b ";
            query += " INNER JOIN tblUserBuildings AS u ON b.id = u.buildingid WHERE u.userid = {0} ORDER BY b.Building";
            try
            {
                query = String.Format(query, userid.ToString());
            }
            catch
            {
                MessageBox.Show("query generator" + userid.ToString());
            }
            dsBuildings = dh.GetData(query, null, out status);
            if (dsBuildings != null && dsBuildings.Tables.Count > 0 && dsBuildings.Tables[0].Rows.Count > 0)
            {
                List<StatementBuilding> dataList = new List<StatementBuilding>();

                foreach (DataRow dr in dsBuildings.Tables[0].Rows)
                {
                    try
                    {
                        int buildingId = (int)dr["id"];
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
                        build = dr["Building"].ToString();
                        String dp = dr["DataPath"].ToString();
                        int p = int.Parse(dr["Period"].ToString());
                        StatementBuilding stmtBuilding = new StatementBuilding(buildingId, build, dp, p, lastProcessed, Controller.UserIsSheldon());

                        var existing = dataList.Where(a => a.DataPath == stmtBuilding.DataPath).FirstOrDefault();
                        if (existing == null)
                            dataList.Add(stmtBuilding);

                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                    }
                }
                bs.DataSource = dataList;
            }

        }

        private void dgBuildings_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            foreach (DataGridViewRow dvr in dgBuildings.Rows)
            {
                StatementBuilding statementBuilding = dvr.DataBoundItem as StatementBuilding;
                if (statementBuilding != null)
                {
                    for (int i = 0; i < dgBuildings.ColumnCount; i++)
                    {
                        if (statementBuilding.StatementAlreadyProcessed())
                        {
                            dvr.Cells[i].Style.BackColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            dvr.Cells[i].Style.BackColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
            dgBuildings.Refresh();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvr in dgBuildings.Rows)
            {
                if (chkSelectAll.Checked)
                {
                    StatementBuilding statementBuilding = dvr.DataBoundItem as StatementBuilding;
                    if (statementBuilding != null && statementBuilding.Allowed)
                    {
                        dvr.Cells[0].Value = true;
                    }
                }
                else
                {
                    dvr.Cells[0].Value = chkSelectAll.Checked;
                }
            }
        }

        //private String getDebtorEmail(int buildingId)
        //{
        //    List<Building> testBuildings = new Buildings(false).buildings;
        //    var b = testBuildings.Where(a => a.Bu)
        //    String dEmail = "";
        //    foreach (Building b in testBuildings)
        //    {
        //        if (b.Name == buildingName) { dEmail = b.Debtor; }
        //    }
        //    return (dEmail != "" ? dEmail : Controller.user.email);
        //}

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

        private frmProgress _ProgressForm = null;

        private void btnProcess_Click(object sender, EventArgs e)
        {
            _ProgressForm = frmProgress.ShowForm();

            List<string> statementFileList = new List<string>();

            this.Cursor = Cursors.WaitCursor;
            statements = new Statements { statements = new List<Statement>() };
            Dictionary<String, bool> hasStatements = new Dictionary<string, bool>();
            foreach (DataGridViewRow dvr in dgBuildings.Rows)
            {
                if ((bool)dvr.Cells[0].Value)
                {
                    String buildingName = dvr.Cells[1].Value.ToString();
                    AddProgressString("Loading Building " + buildingName);

                    SetBuildingStatement(buildingName);
                    String datapath = dvr.Cells[5].Value.ToString();
                    int period = (int)dvr.Cells[6].Value;
                    if (dvr.Cells[2].Value == null) { MessageBox.Show("ishoa"); }

                    StatementBuilding strm = dvr.DataBoundItem as StatementBuilding;


                    List<Statement> bStatements = SetBuildings(strm.GetBuildingId(), buildingName, datapath, period, (bool)dvr.Cells[2].Value);

                    int idx = dvr.Index;
                    DataRow dr = dsBuildings.Tables[0].Rows[idx];
                    String pm = dr["pm"].ToString();
                    String bankName = dr["bankName"].ToString();
                    String accName = dr["accName"].ToString();
                    String bankAccNumber = dr["bankAccNumber"].ToString();
                    String branch = dr["branch"].ToString();
                    bool isStd = bankName.ToLower().Contains("standard");
                    foreach (Statement s in bStatements)
                    {
                        s.pm = pm;
                        s.bankName = bankName;
                        s.accName = accName;
                        s.BankAccountNumber = bankAccNumber;
                        s.branch = branch;
                        s.isStd = isStd;
                        statements.statements.Add(s);
                    }
                }
            }
            PDF generator = new PDF(true);
            foreach (Statement stmt in statements.statements)
            {
                String fileName = String.Empty;
                bool canProcess = false;
                if (stmt.IsInTransfer && stmt.InTransferLetter != null)
                {
                    string folderPath = generator.StatementFolderPath;
                    fileName = Path.Combine(folderPath, String.Format("{0} - InTransfer - {1}{2}.pdf", stmt.AccNo.Replace(@"/", "-").Replace(@"\", "-"), DateTime.Now.ToString("dd-MMMM-yyyy"), ""));
                    if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    File.WriteAllBytes(fileName, stmt.InTransferLetter);
                    canProcess = true;
                }
                else
                {
                    canProcess = generator.CreateStatement(stmt, stmt.BuildingName != "ASTRODON RENTALS" ? true : false, out fileName, stmt.isStd);
                }

                if (canProcess)
                {
                    #region Upload Letter

                    String actFileTitle = Path.GetFileNameWithoutExtension(fileName);
                    String actFile = Path.GetFileName(fileName);

                    #endregion Upload Letter

                    string statementURL = string.Empty;

                    #region Upload Me
                    try
                    {
                        AddProgressString(stmt.BuildingName + ": " + stmt.accName + " - Upload statement to website");

                        string emailAddress = "";
                        if (stmt.email1 != null && stmt.email1.Length > 0)
                            emailAddress = stmt.email1[0];

                        statementURL = _ClientPortal.InsertStatement(stmt.BuildingId, stmt.AccNo, stmt.StmtDate, fileName, File.ReadAllBytes(fileName), emailAddress);
                    }
                    catch (Exception ex)
                    {
                        statementURL = "";
                        AddProgressString(stmt.BuildingName + ": " + stmt.accName + " - Error Upload statement to website " + ex.Message);

                    }
                    #endregion

                    #region Email Me
                    if (stmt.EmailMe)
                    {

                        if (!String.IsNullOrEmpty(fileName))
                        {
                            if (!hasStatements.ContainsKey(stmt.BuildingName))
                            {
                                hasStatements.Add(stmt.BuildingName, true);
                            }
                            if (Controller.user.id != 1 && !String.IsNullOrWhiteSpace(statementURL))
                            {
                                SetupEmail(stmt, fileName, statementURL);
                            }
                        }
                    }
                    #endregion

                    #region Print Me
                    if (stmt.PrintMe && Controller.user.id != 1)
                    {

                        if (!String.IsNullOrWhiteSpace(fileName))
                        {
                            AddProgressString(stmt.BuildingName + ": " + stmt.AccNo + " - Add Statement to List - " + Path.GetFileName(fileName));
                            statementFileList.Add(fileName);
                            // SendToPrinter(fileName, stmt.BuildingName , stmt.AccNo);
                        }
                        else
                        {
                            AddProgressString(stmt.BuildingName + ": " + stmt.AccNo + " - Error Printing Statement - file name blank");
                        }

                    }
                    #endregion

                    Application.DoEvents();

                }
                else
                {
                    AddProgressString(stmt.BuildingName + ": " + stmt.AccNo + " - ERROR Processing Statement");
                    Application.DoEvents();
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

            CombinePDFsAndPrint(statementFileList);

            this.Cursor = Cursors.Arrow;

            _ProgressForm.Focus();
            _ProgressForm.ProcessComplete();
            _ProgressForm = null;
        }

        private void CombinePDFsAndPrint(List<string> statementFileList)
        {
            string outputFileName = "StatementRun_"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (statementFileList.Count() <= 0)
                return;

            outputFileName = Path.Combine(desktopFolder, outputFileName);
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);

            using (FileStream ms = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (Document doc = new Document())
                {
                    using (PdfCopy copy = new PdfCopy(doc, ms))
                    {
                        doc.Open();

                        foreach (var file in statementFileList)
                        {
                            if (File.Exists(file))
                            {
                                AddProgressString("Adding statement " + file + " to " + outputFileName);
                                using (PdfReader reader = new PdfReader(file))
                                {
                                    int n = reader.NumberOfPages;
                                    for (int page = 0; page < n;)
                                    {
                                        copy.AddPage(copy.GetImportedPage(reader, ++page));
                                    }
                                    Application.DoEvents();
                                }
                            }
                        }

                        ms.Flush();


                    }
                }

                AddProgressString("Combined File Completed");
                Application.DoEvents();
            }

            if(_ProgressForm != null)
               _ProgressForm.Hide();
            frmPrintDialog printDialog = new frmPrintDialog();
            if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SetDefaultPrinter(printDialog.selectedPrinter);
                Properties.Settings.Default.defaultPrinter = printDialog.selectedPrinter;
                Properties.Settings.Default.Save();
                _PrinterName = printDialog.selectedPrinter;
            }
            else
            {
                Controller.ShowMessage("Printing Cancelled, please open " + Path.GetFileName(outputFileName) + " on your desktop and print manually");
                if (_ProgressForm != null)
                    _ProgressForm.Show();
                return;
            }
            if (_ProgressForm != null)
                _ProgressForm.Show();

            AddProgressString("Sending File to Printer");

            PrintOrViewFile(outputFileName);
           
        }

        private void PrintOrViewFile(string outputFileName)
        {
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo
                    {
                        Verb = "print",
                        FileName = outputFileName,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = _PrinterName
                    };
                    p.Start();
                    Thread.Sleep(5000);
                }
            }
            catch (Exception e)
            {
                Controller.HandleError("Unable to print file - the file will now open for manual printing.");
                Process.Start(outputFileName);

            }
        }

        private void AddProgressString(string message)
        {
            if (_ProgressForm != null)
                _ProgressForm.AddMessage(message);
        }
        private string _PrinterName = string.Empty;

        List<Building> _BuildingList = null;
        public List<Statement> SetBuildings(int buildingId, String buildingName, String buildingPath, int buildingPeriod, bool isHOA)
        {
            if (_BuildingList == null)
                _BuildingList = new Buildings(false, false).buildings;

            Building build = _BuildingList.Where(a => a.ID == buildingId).FirstOrDefault();
            if (build == null)
                build = new Building();

            build.Name = buildingName;
            build.DataPath = buildingPath;
            build.Period = buildingPeriod;
            build.ID = buildingId;

            User portfolioManager = null;

            using (var ctx = SqlDataHandler.GetDataContext())
            {
                var pmUser = ctx.tblUsers.Where(a => a.email == build.PM && a.Active).FirstOrDefault();
                if (pmUser != null)
                    portfolioManager = new Users().GetUser(pmUser.id);



                List<Customer> customers = Controller.pastel.AddCustomers(buildingName, buildingPath, true);
                List<Statement> myStatements = new List<Statement>();
                lblCCount.Text = build.Name + " 0/" + customers.Count.ToString();
                lblCCount.Refresh();
                int ccount = 0;
                foreach (Customer customer in customers)
                {
                    if (buildingName.Trim().ToUpper() != _AstradonRentalsBuilding.ToUpper() && (
                           customer.IntCategory == 10 //Units Disconnected
                        || customer.IntCategory == 11 //Units Disconnected
                        ))
                    {
                        string reason = "Customer Account skipped: " + customer.accNumber + " category: " + customer.category;

                        if (customer.IntCategory == 10)
                            reason = "Customer Account is in Unallocated Deposits category and will be skipped: " + customer.accNumber;

                        else if (customer.IntCategory == 11)
                            reason = "Customer Account is in Transferred Units/PMA category and will be skipped: " + customer.accNumber;
                        AddProgressString(reason);

                        ctx.WriteStatementRunLog(customer.accNumber, Controller.user.name, reason);


                    }
                    else if (buildingName.Trim().ToUpper() == _AstradonRentalsBuilding.ToUpper() && (customer.IntCategory == 13 || customer.IntCategory == 16 || customer.IntCategory == 17 || customer.IntCategory == 18))
                    {

                        AddProgressString("Astrodon Rentals category skipped : " + customer.accNumber + " " + customer.category);

                        ctx.WriteStatementRunLog(customer.accNumber, Controller.user.name, "Astrodon Rentals category skipped : " + customer.accNumber + " " + customer.category);
                    }
                    else
                    {
                        try
                        {
                            AddProgressString("Loading Statement " + customer.accNumber);

                            var canemail = customer.Email.Count(d => !String.IsNullOrEmpty(d)) > 0;

                            Statement myStatement = new Statement { AccNo = customer.accNumber, BuildingId = buildingId };
                            List<String> address = new List<string>();
                            address.Add(customer.description);
                            foreach (String addyLine in customer.address) { if (!String.IsNullOrEmpty(addyLine)) { address.Add(addyLine); } }
                            myStatement.Address = address.ToArray();
                            myStatement.BankDetails = (!String.IsNullOrEmpty(Controller.pastel.GetBankDetails(buildingPath)) ? Controller.pastel.GetBankDetails(buildingPath) : "");
                            myStatement.BuildingName = buildingName;
                            myStatement.LevyMessage1 = (isHOA ? HOAMessage1 : BCMessage1);
                            myStatement.LevyMessage2 = (!String.IsNullOrEmpty(Message2) ? Message2 : "");
                            myStatement.Message = (!String.IsNullOrEmpty(txtMessage.Text) ? txtMessage.Text : "");
                            myStatement.StmtDate = stmtDatePicker.Value;
                            double totalDue = 0;
                            String trnMsg;

                            myStatement.DebtorEmail = build.Debtor;
                            if (String.IsNullOrWhiteSpace(myStatement.DebtorEmail))
                                Controller.HandleError("Debtor not configured on this building. Please check building configuration.");

                            myStatement.PrintMe = (customer.statPrintorEmail == 2 || customer.statPrintorEmail == 4 || !canemail ? false : true);
                            myStatement.EmailMe = (customer.statPrintorEmail == 4 && canemail ? false : true);
                            if (customer.Email != null && customer.Email.Length > 0)
                            {
                                List<String> newEmails = new List<string>();
                                foreach (String emailAddress in customer.Email)
                                {
                                    newEmails.Add(emailAddress);
                                }
                                myStatement.email1 = newEmails.ToArray();
                            }
                            else
                                myStatement.PrintMe = true;

                            if (myStatement.PrintMe)
                                AddProgressString(customer.accNumber + " Print : " + customer.statPrintorEmail.ToString() + " = " + myStatement.PrintMe.ToString());

                            //check for in transfer and create a transfer letter instead of a statement.
                            if (myStatement.IsRental == false)
                                myStatement.IsInTransfer = customer.IntCategory == 2;
                            if (myStatement.IsInTransfer)
                            {
                                if (portfolioManager != null)
                                {
                                    var fileData = GenerateCustomerTransferLetter(build, customer, portfolioManager);
                                    myStatement.InTransferLetter = fileData;
                                    myStatement.Transactions = new List<Transaction>();
                                    myStatement.totalDue = 0;
                                    myStatements.Add(myStatement);
                                }
                                else
                                {
                                    ctx.WriteStatementRunLog("ALL", Controller.user.name, "Building PM not found for: " + buildingName);
                                    AddProgressString("Building PM not found for: " + buildingName);
                                }

                            }
                            else
                            {

                                List<Transaction> transactions = (new Classes.LoadTrans()).LoadTransactions(build, customer, stmtDatePicker.Value, out totalDue, out trnMsg);
                                if (!string.IsNullOrWhiteSpace(trnMsg))
                                    AddProgressString("Statement for " + customer.accNumber + " has messages " + trnMsg);

                                if (transactions != null && transactions.Where(a => a.IsOpeningBalance == false).Count() > 0)
                                {
                                    myStatement.Transactions = transactions;
                                    myStatement.totalDue = totalDue;
                                    myStatements.Add(myStatement);
                                }
                                else
                                {
                                    ctx.WriteStatementRunLog(customer.accNumber, Controller.user.name, "zero transactions - statement skipped");
                                    AddProgressString("Statement for " + customer.accNumber + " has zero transactions - statement skipped");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AddProgressString("Error processing " + customer.accNumber + " " + ex.Message);
                            ctx.WriteStatementRunLog(customer.accNumber, Controller.user.name, ex.Message);
                        }
                        ccount++;
                        lblCCount.Text = build.Name + " " + ccount.ToString() + "/" + customers.Count.ToString();
                        lblCCount.Refresh();
                        Application.DoEvents();
                    }
                }
                return myStatements;
            }
        }

        private byte[] GenerateCustomerTransferLetter(Building building, Customer customer, User portfolioManager)
        {
            return LetterProvider.CreateIntransferLetter(customer, building, portfolioManager);
        }

        private void SetupEmail(Statement stmt,string fileName, String url)
        {
            String emailAddy = "";
            var builder = new System.Text.StringBuilder();
            builder.Append(emailAddy);
            if (stmt != null && stmt.email1 != null && stmt.email1.Length > 0)
            {
                foreach (String addy in stmt.email1)
                {
                    if(!string.IsNullOrWhiteSpace(addy) && addy.Contains("@"))
                      builder.Append(addy + ";");
                }
                emailAddy = builder.ToString();
            }
            else
            {
                AddProgressString("No email address available for " + stmt.accName);
                return;
            }

            if(string.IsNullOrWhiteSpace(emailAddy))
            {
                AddProgressString("No email address available for " + stmt.accName);
                return;
            }

            using (var context = SqlDataHandler.GetDataContext())
            {
                var statementItem = new tblStatementRun()
                {
                    email1 = emailAddy,
                    queueDate = DateTime.Now,
                    fileName = Path.GetFileName(fileName),
                    debtorEmail = stmt.DebtorEmail,
                    unit = stmt.AccNo + (stmt.BuildingName == "ASTRODON RENTALS" ? "R" : ""),
                    attachment = string.Empty,
                    subject = Path.GetFileNameWithoutExtension(fileName) + " " + DateTime.Now.ToString(),
                    URL = url,
                    sentDate1 = DateTime.Now                    
                };

                bool isRental = statementItem.fileName.ToUpper().EndsWith("_R.PDF");

                string[] toMail = statementItem.email1.Split(";".ToCharArray());

                try
                {
                    if (Email.EmailProvider.SendStatement(statementItem.debtorEmail, toMail, stmt.AccNo, fileName,stmt.StmtDate,url,isRental))
                    {
                        statementItem.errorMessage = "Processed & Sent";
                    }
                    else
                    {
                        statementItem.errorMessage = "Error";
                    }
                }
                catch (Exception exp)
                {
                    statementItem.errorMessage = "Exception:" + exp.Message;
                }

                context.tblStatementRuns.Add(statementItem);
                context.SaveChanges();
                    
            }
         
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        private void btnFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(ofd.FileName) && File.Exists(ofd.FileName)) { txtAttachment.Text = ofd.FileName; }
                }
                else
                {
                    txtAttachment.Text = "";
                }
            }
        }

        private String HOAMessage1
        {
            get
            {
                String hoaMessage = "Levies are due and payable on the 1st of every month in advance.  Failure to compy will result in penalties being charged and electricity supply to the unit being ";
                hoaMessage += "suspended and or restricted.";
                return hoaMessage;
            }
        }

        private String BCMessage1
        {
            get
            {
                String hoaMessage = "Levies are due and payable on the 1st of every month in advance.  Failure to compy will result in penalties being charged and electricity supply to the unit being ";
                hoaMessage += "suspended and or restricted.";
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

        private void dgBuildings_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           if(e.RowIndex >= 0)
            {
                if (e.ColumnIndex > 0)
                    e.Cancel = true;
                else
                {
                    var itm = dgBuildings.Rows[e.RowIndex].DataBoundItem as StatementBuilding;
                    if (itm.Allowed == false)
                    {
                        Controller.ShowWarning("This building statements has already been processed." + Environment.NewLine +
                            "Only Sheldon or Tertia is allowed to process this building");
                        e.Cancel = true;
                    }
                }
            }
        }
    }
}

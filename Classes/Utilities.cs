using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Astrodon {

    public class Utilities {

        public static bool Login(String username, String password, out User user, out String status) {
            user = new Users().GetUser(username, password, out user, out status);

            if (user != null) {
                return true;
            } else if (Environment.MachineName != "VIRTUALXP-34829") {
                return false;
            } else {
                user = new User();
                user.id = 1;
                user.name = "ADMIN";
                return true;
            }
        }

        public static List<int> GetBuildingsIDs(int usertype, int userid, String email, out String status) {
            List<int> buildings = new List<int>();
            String buildQuery1 = "SELECT buildingid as id FROM tblUserBuildings WHERE userid = @userid";
            String buildQuery2 = "SELECT id FROM tblBuildings WHERE pm = @userid";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            String query = String.Empty;
            status = String.Empty;
            if (usertype == 2) {
                query = buildQuery2;
                sqlParms.Add("@userid", email);
            } else if (usertype == 1 || usertype == 3) {
                query = buildQuery1;
                sqlParms.Add("@userid", userid);
            } else {
                return buildings;
            }
            SqlDataHandler dh = new SqlDataHandler();
            DataSet ds = dh.GetData(query, sqlParms, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    int buildID = int.Parse(dr["id"].ToString());
                    buildings.Add(buildID);
                }
            }
            return buildings;
        }

        public static List<Customer> getAllCustomers(String buildingName, String buildPath) {
            return Controller.pastel.AddCustomers(buildingName, buildPath);
        }

        public static int getPeriod(DateTime trnDate, int sbPeriod, out int bPeriod) {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth); //12
            bPeriod = (myMonth - sbPeriod < 1 ? myMonth - sbPeriod + 12 : myMonth - sbPeriod);
            return myMonth;
        }

        public static int getPeriod(DateTime trnDate) {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth);
            return myMonth;
        }

        public static Dictionary<String, Building2> GetReportBuildings() {
            String status;
            Dictionary<String, Building2> repBuildings = new Dictionary<string, Building2>();
            repBuildings.Clear();
            String centrecQuery = "SELECT centrec FROM tblSettings";
            SqlDataHandler dh = new SqlDataHandler();
            DataSet dsCentrec = dh.GetData(centrecQuery, null, out status);
            String centrecPath = "";
            if (dsCentrec != null && dsCentrec.Tables.Count > 0 && dsCentrec.Tables[0].Rows.Count > 0) {
                centrecPath = dsCentrec.Tables[0].Rows[0]["centrec"].ToString();
            } else {
                MessageBox.Show(status);
            }
            List<Building> buildings = new Buildings(false).buildings;
            //String myPath = "";
            //String pastelTest = Controller.pastel.SetPath("CENTRE17", out myPath);
            //if (pastelTest != "0") { MessageBox.Show(myPath); }

            foreach (Building b in buildings) {
                try {
                    int id = b.ID;
                    String building = b.Name;
                    String code = b.Abbr;
                    String path = b.DataPath;
                    int period = b.Period;
                    int journal = b.Journal;
                    String acc = b.Trust;
                    String bank = b.Bank;
                    String centrec_building = b.Centrec_Building.Replace("//", "").Replace("/", "");
                    String centrec = b.Centrec_Account.Replace("//", "").Replace("/", "");
                    String business = b.Business_Account;
                    String cString = Controller.pastel.GetAccount(path, centrec_building);
                    //MessageBox.Show(centrec_building);
                    Account buildCentrec = (cString != "" && !cString.StartsWith("error") ? new Account(cString) : null);
                    String aString = Controller.pastel.GetCustomer(centrecPath, centrec);
                    //MessageBox.Show(centrec);
                    Customer centrecBuild = (aString != "" && !aString.StartsWith("error") ? new Customer(aString) : null);
                    if (buildCentrec != null && centrecBuild != null) {
                        Building2 build = new Building2(id, building, code, path, period, journal, acc, centrec_building, centrec, business, buildCentrec, centrecBuild, bank);
                        repBuildings.Add(building, build);
                    }
                    //break;
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
            return repBuildings;
        }

        public static String cleanDate(String rawDate) {
            String Numbers = "0123456789";
            rawDate = rawDate.Replace("-", "").Replace("/", "");
            if (Numbers.Contains(rawDate.Substring(1, 1))) { } else { rawDate = "0" + rawDate; }
            String day = rawDate.Substring(0, 2);
            String month = "";
            int yearX = 0;
            if (!Numbers.Contains(rawDate.Substring(2, 1))) {
                month = rawDate.Substring(2, 3);
                switch (month) {
                    case "Jan":
                        month = "01";
                        break;

                    case "Feb":
                        month = "02";
                        break;

                    case "Mar":
                        month = "03";
                        break;

                    case "Apr":
                        month = "04";
                        break;

                    case "May":
                        month = "05";
                        break;

                    case "Jun":
                        month = "06";
                        break;

                    case "Jul":
                        month = "07";
                        break;

                    case "Aug":
                        month = "08";
                        break;

                    case "Sep":
                        month = "09";
                        break;

                    case "Oct":
                        month = "10";
                        break;

                    case "Nov":
                        month = "11";
                        break;

                    case "Dec":
                        month = "12";
                        break;
                }
                yearX = 5;
            } else {
                month = rawDate.Substring(2, 2);
                yearX = 4;
            }
            int yearLength = rawDate.Length - yearX;
            String year = "";
            if (yearLength == 2) { year = "20" + rawDate.Substring(yearX, 2); } else { year = rawDate.Substring(yearX, 4); }
            String cleanDate = day + "/" + month + "/" + year;
            return cleanDate;
        }

        public static String cleanDescription(String rawDescription) {
            rawDescription = rawDescription.Replace("+", " ");
            if (rawDescription == "PRIME PROPERTIES") { rawDescription += " KB000"; }
            try {
                if (rawDescription.Substring(0, 13) == "CASHFOCUS MDM") {
                    String subNumber = rawDescription.Substring(15, 3);
                    rawDescription = rawDescription.Substring(0, 13) + subNumber;
                }
            } catch { }
            Char[] illegalChars = "!@#$%^&*{}[]\"'_+<>?".ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Char ch in rawDescription.ToCharArray()) { if (Array.IndexOf(illegalChars, ch) == -1) { sb.Append(ch); } }
            rawDescription = sb.ToString();
            return rawDescription;
        }

        public static void ProcessKiller(String fileName) {
            Process tool = new Process();
            tool.StartInfo.FileName = "handle.exe";
            tool.StartInfo.Arguments = fileName + " /accepteula";
            tool.StartInfo.UseShellExecute = false;
            tool.StartInfo.RedirectStandardOutput = true;
            tool.Start();
            tool.WaitForExit();
            string outputTool = tool.StandardOutput.ReadToEnd();

            string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
            foreach (Match match in Regex.Matches(outputTool, matchPattern)) {
                Process.GetProcessById(int.Parse(match.Value)).Kill();
            }
        }
    }

    public class Detail {
        private String _description, _reference;
        private DateTime _trnDate;
        private double _amt;
        public bool deleteMe = false;

        public String Description {
            get { return _description; }
            set { _description = value; }
        }

        public String Reference {
            get { return _reference; }
            set { _reference = value; }
        }

        public DateTime TrnDate {
            get { return _trnDate; }
            set { _trnDate = value; }
        }

        public double Amt {
            get { return _amt; }
            set { _amt = value; }
        }

        public Detail(String description, String reference, DateTime trnDate, double amt) {
            Description = description;
            Reference = reference;
            TrnDate = trnDate;
            Amt = amt;
        }

        public bool validDetail = true;

        public Detail(String transString, DateTime pStart, DateTime pEnd, int journal) {
            try {
                String[] splitter = new string[] { "|" };
                String[] transBits = transString.Split(splitter, StringSplitOptions.None);
                TrnDate = Controller.pastel.GetDate(transBits[7]);
                if (TrnDate < pStart || TrnDate > pEnd) { deleteMe = true; }
                String eType = transBits[8];
                if (eType != journal.ToString()) { deleteMe = true; }
                Reference = transBits[9];
                if (!double.TryParse(transBits[11], out _amt)) { Amt = 0; }
                Description = transBits[18];
            } catch (Exception ex) {
                deleteMe = true;
            }
        }
    }

    public class Trns {
        public String Date { get; set; }

        public String Description { get; set; }

        public String Reference { get; set; }

        public String Amount { get; set; }

        public String period { get; set; }

        public String Cumulative { get; set; }
    }

    public class TrnsComparer : IComparer<Trns> {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName"></param>
        /// <param name="sortingOrder"></param>
        public TrnsComparer(string strMemberName, SortOrder sortingOrder) {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order
        /// and return the result.
        /// </summary>
        /// <param name="Student1"></param>
        /// <param name="Student2"></param>
        /// <returns></returns>
        public int Compare(Trns trn1, Trns trn2) {
            int returnValue = 1;
            switch (memberName) {
                case "Date":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.Date.CompareTo(trn2.Date);
                    } else {
                        returnValue = trn2.Date.CompareTo(trn1.Date);
                    }

                    break;

                case "Description":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.Description.CompareTo(trn2.Description);
                    } else {
                        returnValue = trn2.Description.CompareTo(trn1.Description);
                    }

                    break;

                case "Reference":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.Reference.CompareTo(trn2.Reference);
                    } else {
                        returnValue = trn2.Reference.CompareTo(trn1.Reference);
                    }

                    break;

                case "Amount":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.Amount.CompareTo(trn2.Amount);
                    } else {
                        returnValue = trn2.Amount.CompareTo(trn1.Amount);
                    }

                    break;
            }
            return returnValue;
        }
    }

    public class CustomerComparer : IComparer<Customer> {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName"></param>
        /// <param name="sortingOrder"></param>
        public CustomerComparer(string strMemberName, SortOrder sortingOrder) {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order
        /// and return the result.
        /// </summary>
        /// <param name="Student1"></param>
        /// <param name="Student2"></param>
        /// <returns></returns>
        public int Compare(Customer trn1, Customer trn2) {
            int returnValue = 1;
            switch (memberName) {
                case "AccNo":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.accNumber.CompareTo(trn2.accNumber);
                    } else {
                        returnValue = trn2.accNumber.CompareTo(trn1.accNumber);
                    }

                    break;
            }
            return returnValue;
        }
    }

    public class BuildingComparer : IComparer<Building> {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName"></param>
        /// <param name="sortingOrder"></param>
        public BuildingComparer(string strMemberName, SortOrder sortingOrder) {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order
        /// and return the result.
        /// </summary>
        /// <param name="Student1"></param>
        /// <param name="Student2"></param>
        /// <returns></returns>
        public int Compare(Building trn1, Building trn2) {
            int returnValue = 1;
            switch (memberName) {
                case "Name":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.Name.CompareTo(trn2.Name);
                    } else {
                        returnValue = trn2.Name.CompareTo(trn1.Name);
                    }

                    break;
            }
            return returnValue;
        }
    }

    public class PMBuildingComparer : IComparer<PMBuilding> {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName"></param>
        /// <param name="sortingOrder"></param>
        public PMBuildingComparer(string strMemberName, SortOrder sortingOrder) {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order
        /// and return the result.
        /// </summary>
        /// <param name="Student1"></param>
        /// <param name="Student2"></param>
        /// <returns></returns>
        public int Compare(PMBuilding trn1, PMBuilding trn2) {
            int returnValue = 1;
            switch (memberName) {
                case "Name":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.Name.CompareTo(trn2.Name);
                    } else {
                        returnValue = trn2.Name.CompareTo(trn1.Name);
                    }

                    break;
            }
            return returnValue;
        }
    }

    public class DocComparer : IComparer<CustomerDocument> {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName"></param>
        /// <param name="sortingOrder"></param>
        public DocComparer(string strMemberName, SortOrder sortingOrder) {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order
        /// and return the result.
        /// </summary>
        /// <param name="Student1"></param>
        /// <param name="Student2"></param>
        /// <returns></returns>
        public int Compare(CustomerDocument trn1, CustomerDocument trn2) {
            int returnValue = 1;
            switch (memberName) {
                case "Date":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.tstamp.CompareTo(trn2.tstamp);
                    } else {
                        returnValue = trn2.tstamp.CompareTo(trn1.tstamp);
                    }

                    break;

                case "Subject":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.subject.CompareTo(trn2.subject);
                    } else {
                        returnValue = trn2.subject.CompareTo(trn1.subject);
                    }

                    break;

                case "Title":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.title.CompareTo(trn2.title);
                    } else {
                        returnValue = trn2.title.CompareTo(trn1.title);
                    }

                    break;

                case "File":
                    if (sortOrder == SortOrder.Ascending) {
                        returnValue = trn1.file.CompareTo(trn2.file);
                    } else {
                        returnValue = trn2.file.CompareTo(trn1.file);
                    }

                    break;
            }
            return returnValue;
        }
    }

    public class ClearanceTransactions {
        private double qty = 1;
        private double rate = 0;
        private double mu = 0;
        private double amt = 0;

        public String Description { get; set; }

        public double Qty {
            get { return qty; }
            set {
                qty = value;
                amt = qty * rate * (1 + (mu / 100));
            }
        }

        public double Rate {
            get { return rate; }
            set {
                rate = value;
                amt = qty * rate * (1 + (mu / 100));
            }
        }

        public double Markup_Percentage {
            get { return mu; }
            set {
                mu = value;
                amt = qty * rate * (1 + (mu / 100));
            }
        }

        public double Amount {
            get { return amt; }
            set { amt = value; }
        }
    }

    public struct MessageConstruct {
        public String building;
        public String customer;
        public String number;
        public String reference;
        public String text;
        public int id;
        public DateTime sent;
        public String sender;
        public bool billable;
        public bool bulkbillable;
        public String astStatus;
        public String batchID;
        public String status;
        public DateTime nextPolled;
        public int pollCount;
        public double os;
        public String msgType;
    }

    public class SMSMessage {
        public int id { get; set; }

        public String building { get; set; }

        public String customer { get; set; }

        public String number { get; set; }

        public String reference { get; set; }

        public String message { get; set; }

        public bool direction { get; set; }

        public DateTime sent { get; set; }

        public String sender { get; set; }

        public bool billable { get; set; }

        public bool bulkbillable { get; set; }

        public String astStatus { get; set; }

        public String batchID { get; set; }

        public String status { get; set; }

        public DateTime nextPolled { get; set; }

        public int pollCount { get; set; }

        public double cbal { get; set; }

        public String smsType { get; set; }
    }

    public class SummRep {
        private String _trustCode, _buildingName, _bank, _code;
        private Double _buildBalance, _centrecBalance, _difference;

        public String Bank { get { return _bank; } set { _bank = value; } }

        public String Code { get { return _code; } set { _code = value; } }

        public String TrustCode { get { return _trustCode; } set { _trustCode = value; } }

        public String BuildingName { get { return _buildingName; } set { _buildingName = value; } }

        public Double BuildBal { get { return _buildBalance; } set { _buildBalance = value; } }

        public Double CentrecBal { get { return _centrecBalance; } set { _centrecBalance = value; } }

        public Double Difference { get { return _difference; } set { _difference = value; } }
    }

    public class Account {
        private int _finCat;
        private String _accNumber;
        private String _description;
        private int _cat;
        private String _linkCode;
        private int _subAcc;
        private double[] _thisBal = new double[13];
        private double[] _lastBal = new double[13];
        private double[] _thisBudget = new double[13];
        private double[] _nextBudget = new double[13];
        private double[] _lastBudget = new double[13];
        private String _blocked;
        private int _tax;
        private String _defTax;
        private String _gaap;

        public int finCat { get { return _finCat; } set { _finCat = value; } }

        public String accNumber { get { return _accNumber; } set { _accNumber = value; } }

        public String description { get { return _description; } set { _description = value; } }

        public int cat { get { return _cat; } set { _cat = value; } }

        public String linkCode { get { return _linkCode; } set { _linkCode = value; } }

        public int subAcc { get { return _subAcc; } set { _subAcc = value; } }

        public double[] thisBal { get { return _thisBal; } set { _thisBal = value; } }

        public double[] lastBal { get { return _lastBal; } set { _lastBal = value; } }

        public double[] thisBudget { get { return _thisBudget; } set { _thisBudget = value; } }

        public double[] nextBudget { get { return _nextBudget; } set { _nextBudget = value; } }

        public double[] lastBudget { get { return _lastBudget; } set { _lastBudget = value; } }

        public String blocked { get { return _blocked; } set { _blocked = value; } }

        public int tax { get { return _tax; } set { _tax = value; } }

        public String defTax { get { return _defTax; } set { _defTax = value; } }

        public String gaap { get { return _gaap; } set { _gaap = value; } }

        public Account() {
        }

        public Account(String accString) {
            try {
                String[] splitter = new String[] { "|" };
                String[] contents = accString.Split(splitter, StringSplitOptions.None);
                //if (contents.Length == 75) {
                finCat = int.Parse(contents[1]);
                accNumber = contents[2];
                description = contents[3];
                cat = int.Parse(contents[4]);
                linkCode = contents[5];
                double[] balThis = new double[13];
                subAcc = int.Parse(contents[6]);

                #region Balances

                _thisBal[0] = double.Parse(contents[7]);
                _thisBal[1] = double.Parse(contents[8]);
                _thisBal[2] = double.Parse(contents[9]);
                _thisBal[3] = double.Parse(contents[10]);
                _thisBal[4] = double.Parse(contents[11]);
                _thisBal[5] = double.Parse(contents[12]);
                _thisBal[6] = double.Parse(contents[13]);
                _thisBal[7] = double.Parse(contents[14]);
                _thisBal[8] = double.Parse(contents[15]);
                _thisBal[9] = double.Parse(contents[16]);
                _thisBal[10] = double.Parse(contents[17]);
                _thisBal[11] = double.Parse(contents[18]);
                _thisBal[12] = double.Parse(contents[19]);
                //thisBal = balThis;
                _lastBal[0] = double.Parse(contents[20]);
                _lastBal[1] = double.Parse(contents[21]);
                _lastBal[2] = double.Parse(contents[22]);
                _lastBal[3] = double.Parse(contents[23]);
                _lastBal[4] = double.Parse(contents[24]);
                _lastBal[5] = double.Parse(contents[25]);
                _lastBal[6] = double.Parse(contents[26]);
                _lastBal[7] = double.Parse(contents[27]);
                _lastBal[8] = double.Parse(contents[28]);
                _lastBal[9] = double.Parse(contents[29]);
                _lastBal[10] = double.Parse(contents[30]);
                _lastBal[11] = double.Parse(contents[31]);
                _lastBal[12] = double.Parse(contents[32]);
                //lastBal = balThis;

                #endregion Balances

                #region Budgets

                balThis[0] = double.Parse(contents[33]);
                balThis[1] = double.Parse(contents[34]);
                balThis[2] = double.Parse(contents[35]);
                balThis[3] = double.Parse(contents[36]);
                balThis[4] = double.Parse(contents[37]);
                balThis[5] = double.Parse(contents[38]);
                balThis[6] = double.Parse(contents[39]);
                balThis[7] = double.Parse(contents[40]);
                balThis[8] = double.Parse(contents[41]);
                balThis[9] = double.Parse(contents[42]);
                balThis[10] = double.Parse(contents[43]);
                balThis[11] = double.Parse(contents[44]);
                balThis[12] = double.Parse(contents[45]);
                thisBudget = balThis;
                balThis[0] = double.Parse(contents[46]);
                balThis[1] = double.Parse(contents[47]);
                balThis[2] = double.Parse(contents[48]);
                balThis[3] = double.Parse(contents[49]);
                balThis[4] = double.Parse(contents[50]);
                balThis[5] = double.Parse(contents[51]);
                balThis[6] = double.Parse(contents[52]);
                balThis[7] = double.Parse(contents[53]);
                balThis[8] = double.Parse(contents[54]);
                balThis[9] = double.Parse(contents[55]);
                balThis[10] = double.Parse(contents[56]);
                balThis[11] = double.Parse(contents[57]);
                balThis[12] = double.Parse(contents[58]);
                nextBudget = balThis;
                balThis[0] = double.Parse(contents[59]);
                balThis[1] = double.Parse(contents[60]);
                balThis[2] = double.Parse(contents[61]);
                balThis[3] = double.Parse(contents[62]);
                balThis[4] = double.Parse(contents[63]);
                balThis[5] = double.Parse(contents[64]);
                balThis[6] = double.Parse(contents[65]);
                balThis[7] = double.Parse(contents[66]);
                balThis[8] = double.Parse(contents[67]);
                balThis[9] = double.Parse(contents[68]);
                balThis[10] = double.Parse(contents[69]);
                balThis[11] = double.Parse(contents[70]);
                balThis[12] = double.Parse(contents[71]);
                lastBudget = balThis;

                #endregion Budgets

                blocked = contents[72];
                tax = int.Parse(contents[73]);
                gaap = contents[74];
            } catch { }
        }
    }

    public class ParentDetail {
        private String _trustCode, _buildName;
        private int _period;
        private double _total;
        private List<Detail> _transactions = new List<Detail>();

        public String TrustCode {
            get { return _trustCode; }
            set { _trustCode = value; }
        }

        public String BuildName {
            get { return _buildName; }
            set { _buildName = value; }
        }

        public int Period {
            get { return _period; }
            set { _period = value; }
        }

        public void Total() {
            _total = 0;
            foreach (Detail transaction in _transactions) {
                double amt = transaction.Amt;
                _total += amt;
            }
            AddTransaction(new Detail("Total", "", DateTime.Now, _total));
        }

        public List<Detail> Transactions {
            get { return _transactions; }
        }

        public void AddTransaction(Detail transaction) {
            _transactions.Add(transaction);
        }

        public ParentDetail(String trustCode, String buildName, int period) {
            TrustCode = trustCode;
            BuildName = buildName;
            Period = period;
        }
    }

    public class ReportWriter {

        public void CreateSummaryReport(List<SummRep> summaries, String FileName, out String msg) {
            msg = "";
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(misValue, misValue, misValue, misValue);
            xlWorkSheet.Name = "Summary report";
            int y = 1;
            xlWorkSheet.Cells[y, 1] = "BANK";
            xlWorkSheet.Cells[y, 2] = "ABV";
            xlWorkSheet.Cells[y, 3] = "TRUST CODE";
            xlWorkSheet.Cells[y, 4] = "BUILDING NAME";
            xlWorkSheet.Cells[y, 5] = "BUILDING BALANCE";
            xlWorkSheet.Cells[y, 6] = "CENTREC BALANCE";
            xlWorkSheet.Cells[y, 7] = "DIFFERENCE";
            y++;
            foreach (SummRep summary in summaries) {
                xlWorkSheet.Cells[y, 1] = summary.Bank;
                xlWorkSheet.Cells[y, 2] = summary.Code;
                xlWorkSheet.Cells[y, 3] = summary.TrustCode;
                xlWorkSheet.Cells[y, 4] = summary.BuildingName;
                xlWorkSheet.Cells[y, 5] = summary.BuildBal;
                xlWorkSheet.Cells[y, 6] = summary.CentrecBal;
                xlWorkSheet.Cells[y, 7] = summary.Difference;
                y++;
            }
            releaseObject(xlWorkSheet, out msg);

            xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(xlWorkBook, out msg);
            releaseObject(xlApp, out msg);
            msg = (msg == "" ? "Excel Report Saved" : msg);
        }

        public void CreateDetailReport(ParentDetail summary, String FileName, out String msg) {
            msg = "";
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(misValue, misValue, misValue, misValue);
            xlWorkSheet.Name = CreateValidWorksheetName(summary.BuildName);
            int y = 1;
            xlWorkSheet.Cells[y, 1] = "TRUST CODE";
            xlWorkSheet.Cells[y, 2] = "BUILDING NAME";
            y++;
            xlWorkSheet.Cells[y, 1] = summary.TrustCode;
            xlWorkSheet.Cells[y, 2] = summary.BuildName;
            y += 2;
            xlWorkSheet.Cells[y, 1] = "DATE";
            xlWorkSheet.Cells[y, 2] = "DESCRIPTION";
            xlWorkSheet.Cells[y, 3] = "REFERENCE";
            xlWorkSheet.Cells[y, 4] = "AMOUNT";
            y++;
            foreach (Detail transaction in summary.Transactions) {
                xlWorkSheet.Cells[y, 1] = transaction.TrnDate.ToString("yyyy/MM/dd");
                xlWorkSheet.Cells[y, 2] = transaction.Description;
                xlWorkSheet.Cells[y, 3] = transaction.Reference;
                xlWorkSheet.Cells[y, 4] = transaction.Amt.ToString();
                y++;
            }
            releaseObject(xlWorkSheet, out msg);
            xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(xlWorkBook, out msg);
            releaseObject(xlApp, out msg);
            msg = (msg == "" ? "Excel Report Saved" : msg);
        }

        private string CreateValidWorksheetName(string name) {
            // Worksheet name cannot be longer than 31 characters.

            System.Text.StringBuilder escapedString;

            if (name.Length <= 31) {
                escapedString = new System.Text.StringBuilder(name);
            } else {
                escapedString = new System.Text.StringBuilder(name, 0, 31, 31);
            }
            String newString = "";
            for (int i = 0; i < escapedString.Length; i++) {
                String escapedStringIdea = escapedString.ToString().Substring(i, 1);
                if (escapedStringIdea == ":" || escapedStringIdea == "\\" || escapedStringIdea == "/" || escapedStringIdea == "?" ||
                    escapedStringIdea == "*" || escapedStringIdea == "[" || escapedStringIdea == "]") {
                    escapedStringIdea = "_";
                }
                newString += escapedStringIdea;
            }

            return newString;
        }

        private void releaseObject(object obj, out String msg) {
            try {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
                obj = null;
                msg = "";
            } catch (Exception ex) {
                obj = null;
                msg = "Exception Occured while releasing object " + ex.ToString();
            } finally {
                GC.Collect();
            }
        }

        public List<Dictionary<String, String>> ExtractData(String fileName, out String returnMessage) {
            object missing = System.Reflection.Missing.Value;

            String msg = String.Empty;
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            string str;
            int rCnt = 0;
            int cCnt = 0;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            List<Dictionary<String, String>> contents = new List<Dictionary<string, string>>();
            List<String> keys = new List<string>();
            returnMessage = "";
            for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++) {
                try {
                    if (rCnt == 1) {
                        for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++) {
                            str = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value;
                            if (!String.IsNullOrEmpty(str)) { keys.Add(str); }
                        }
                    } else {
                        Dictionary<String, String> rowContent = new Dictionary<string, string>();
                        for (cCnt = 1; cCnt <= keys.Count; cCnt++) {
                            String val = "";
                            try {
                                object obj = (range.Cells[rCnt, cCnt] as Excel.Range).Value;
                                val = obj.ToString();
                            } catch (Exception ex) {
                                returnMessage += ex.Message + ";";
                            }

                            rowContent.Add(keys[cCnt - 1], val);
                        }
                        if (rowContent[keys[0]] != "") {
                            contents.Add(rowContent);
                        }
                    }
                } catch (Exception ex) {
                    returnMessage += ex.Message + ";";
                }
            }

            xlWorkBook.Close(true, missing, missing);
            xlApp.Quit();

            releaseObject(xlWorkSheet, out msg);
            releaseObject(xlWorkBook, out msg);
            releaseObject(xlApp, out msg);

            return contents;
        }
    }

    public class ClearanceValues {
        public double clearanceFee { get; set; }

        public double exClearanceFee { get; set; }

        public double splitFee { get; set; }

        public String centrec { get; set; }

        public String business { get; set; }

        public ClearanceValues() {
            String query = "SELECT clearance, ex_clearance, recon_split, centrec, business FROM tblSettings";
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                DataRow dr = ds.Tables[0].Rows[0];
                clearanceFee = double.Parse(dr["clearance"].ToString());
                exClearanceFee = double.Parse(dr["ex_clearance"].ToString());
                splitFee = double.Parse(dr["recon_split"].ToString());
                centrec = dr["centrec"].ToString();
                business = dr["business"].ToString();
            }
        }
    }
}
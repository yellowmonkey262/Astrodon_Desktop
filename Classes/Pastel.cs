using PasSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Astrodon {

    public class Pastel {
        private PastelPartnerSDK SDK;
        public String pastelDirectory;
        private short keyNumber = 0;

        public Pastel() {
            String lc, auth;
            SDK = new PastelPartnerSDK();
            if (Environment.MachineName != "STEPHEN-PC") {
                lc = "DK11110068";
                auth = "4228113";
                pastelDirectory = (Directory.Exists("K:\\") ? "K:\\" : "C:\\Pastel11\\");
            } else {
                lc = "DK12111473";
                auth = "3627008";
                pastelDirectory = "C:\\Pastel12\\";
            }
            SDK.SetLicense(lc, auth);
        }

        public List<Customer> AddCustomers(String buildKey, String buildPath) {
            List<Customer> customers = new List<Customer>();
            String path = "";
            if (Directory.Exists(Path.Combine(pastelDirectory, buildPath))) {
                path = Path.Combine(pastelDirectory, buildPath);
            } else {
                path = Path.Combine("C:\\Pastel12", buildPath);
                pastelDirectory = "C:\\Pastel12";
            }
            String returner = SDK.SetDataPath(path);
            if (returner.Contains("99")) {
            } else {
                int records = SDK.NumberOfRecords("ACCMASD");
                if (records > 0) {
                    List<String> customerStrings = GetCustomers(buildPath);
                    foreach (String customer in customerStrings) {
                        Customer newCustomer = new Customer(customer);
                        newCustomer.SetDeliveryInfo(DeliveryInfo(newCustomer.accNumber));
                        customers.Add(newCustomer);
                    }
                } else {
                    MessageBox.Show("Pastel Add Customers: " + returner + " - " + records.ToString());
                }
            }
            customers.Sort(new CustomerComparer("AccNo", SortOrder.Ascending));
            return customers;
        }

        #region Event Handler

        public delegate void MessageHandler(object sender, MessageArgs e);

        public event MessageHandler Message;

        private void RaiseEvent(String message) {
            if (Message != null) { Message(this, new MessageArgs(message)); }
        }

        public delegate void CustomerFoundEventHandler(object sender, CustomerArgs e);

        public event CustomerFoundEventHandler CustomerFound;

        private void RaiseCustomerFoundEvent(Customer c, String b) {
            if (CustomerFound != null) { CustomerFound(this, new CustomerArgs(c, b)); }
        }

        #endregion Event Handler

        public volatile bool runSearch = false;

        public void SearchCustomers(List<String> searchCriteria, Dictionary<String, String> buildings) {
            String status = String.Empty;
            runSearch = true;
            bool searchRunning = true;
            try {
                foreach (KeyValuePair<String, String> building in buildings) {
                    String returner = SDK.SetDataPath(pastelDirectory + "\\" + building.Value);
                    if (returner != "0") {
                        status = "Returner = " + returner + " Customers: " + pastelDirectory + "\\" + building.Value;
                        RaiseEvent(status);
                    } else {
                        status = "";
                        int records = SDK.NumberOfRecords("ACCMASD");
                        if (records > 0) {
                            List<String> customerStrings = GetCustomers(building.Value);
                            Parallel.ForEach(customerStrings, customer => {
                                Customer newCustomer = new Customer(customer);
                                newCustomer.SetDeliveryInfo(DeliveryInfo(newCustomer.accNumber));
                                bool foundCustomer = false;
                                foreach (String searchCrit in searchCriteria) {
                                    if (newCustomer.description.ToLower().Contains(searchCrit.ToLower())) {
                                        foundCustomer = true;
                                        break;
                                    } else {
                                        foreach (String cEmail in newCustomer.Email) {
                                            if (cEmail.ToLower().Contains(searchCrit.ToLower())) {
                                                foundCustomer = true;
                                                break;
                                            }
                                        }
                                        if (foundCustomer) { break; }
                                    }
                                }
                                if (foundCustomer) { RaiseCustomerFoundEvent(newCustomer, building.Key); }
                                if (!runSearch) {
                                    searchRunning = false;
                                    RaiseEvent("Search aborted");
                                    return;
                                }
                            });
                        }
                    }
                    if (!runSearch && searchRunning) {
                        RaiseEvent("Search aborted");
                        break;
                    }
                }
                if (runSearch && searchRunning) { RaiseEvent("Search complete"); }
            } catch (Exception ex) {
                RaiseEvent("Error: " + ex.Message);
            }
        }

        public List<String> GetCustomers(String buildPath) {
            List<String> customers = new List<string>();
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCMASD";
                    String keyValue = SDK.MKI(0);
                    returner = SDK.GetNearest(fileName, keyNumber, keyValue);
                    if (Regex.Matches(returner, "|").Count > 5) { customers.Add(returner); }
                    while (!returner.StartsWith("9|")) {
                        returner = SDK.GetNext(fileName, keyNumber);
                        if ((Regex.Matches(returner, "|").Count > 5) && (!returner.StartsWith("9|"))) { customers.Add(returner); }
                    }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            }
            return customers;
        }

        public Customer GetOneCustomer(String buildPath, String accNumber) {
            return new Customer(GetCustomer(buildPath, accNumber));
        }

        public String GetCustomer(String buildPath, String accNumber) {
            String account = "";
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCMASD";
                    String keyValue = accNumber;
                    returner = SDK.GetRecord(fileName, keyNumber, keyValue);
                    if (Regex.Matches(returner, "|").Count > 5 && returner.StartsWith("0")) { account = returner; }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            }
            return account;
        }

        public String[] DeliveryInfo(String customerAcc) {
            //ACCDELIV
            String returner = "";
            String[] delBits = new string[1];
            try {
                String fileName = "ACCDELIV";
                short keyNumber = 2;
                returner = SDK.GetNearest(fileName, keyNumber, customerAcc);
                delBits = SplitDeliveryInfo(returner);
                //if (delBits[13].Contains("@imp.ad-one.co.za")) { delBits[13] = ""; }
                String rCustAcc = delBits[1];
                while (rCustAcc == customerAcc) {
                    returner = SDK.GetNext(fileName, keyNumber);
                    String[] nextBits = SplitDeliveryInfo(returner);
                    rCustAcc = nextBits[1];
                    try {
                        if (rCustAcc == customerAcc && delBits[13] != nextBits[13] && nextBits[13] != "") { delBits[13] += (delBits[13] != "" ? ";" : "") + nextBits[13]; }
                    } catch { }
                }
                //MessageBox.Show(delBits[13]);
            } catch (Exception ex) {
                returner = "error:" + ex.Message;
            }
            return delBits;
        }

        public String[] DeliveryInfo(String buildPath, String customerAcc) {
            //ACCDELIV
            String returner = "";
            String[] delBits = new string[1];
            try {
                returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
                if (returner == "0") {
                    String fileName = "ACCDELIV";
                    short keyNumber = 2;
                    returner = SDK.GetNearest(fileName, keyNumber, customerAcc);
                    delBits = SplitDeliveryInfo(returner);
                    //if (delBits[13].Contains("@imp.ad-one.co.za")) { delBits[13] = ""; }
                    String rCustAcc = delBits[1];
                    while (rCustAcc == customerAcc) {
                        returner = SDK.GetNext(fileName, keyNumber);
                        String[] nextBits = SplitDeliveryInfo(returner);
                        rCustAcc = nextBits[1];
                        try {
                            if (rCustAcc == customerAcc && delBits[13] != nextBits[13] && nextBits[13] != "") { delBits[13] += (delBits[13] != "" ? ";" : "") + nextBits[13]; }
                        } catch { }
                    }
                }
            } catch (Exception ex) {
                returner = "error:" + ex.Message;
            }
            return delBits;
        }

        public List<AdditionalAddress> GetDeliveryInfo(String buildPath, String customerAcc) {
            //ACCDELIV
            List<AdditionalAddress> aas = new List<AdditionalAddress>();
            String returner = "";
            String[] delBits = new string[1];
            try {
                returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
                if (returner == "0") {
                    String fileName = "ACCDELIV";
                    short keyNumber = 2;
                    returner = SDK.GetNearest(fileName, keyNumber, customerAcc);
                    delBits = SplitDeliveryInfo(returner);

                    String rCustAcc = delBits[1];
                    AdditionalAddress aa = new AdditionalAddress();
                    if (rCustAcc == customerAcc) {
                        aa.Contact = delBits[4];
                        aa.Telephone = delBits[5];
                        aa.Cell = delBits[6];
                        aa.Fax = delBits[7];
                        aa.Address = new string[5];
                        for (int i = 8; i < 13; i++) { aa.Address[i - 8] = delBits[i]; }
                        aa.Email = delBits[13];
                        aas.Add(aa);
                        while (rCustAcc == customerAcc) {
                            returner = SDK.GetNext(fileName, keyNumber);
                            String[] nextBits = SplitDeliveryInfo(returner);
                            rCustAcc = nextBits[1];
                            if (rCustAcc == customerAcc) {
                                aa = new AdditionalAddress();
                                aa.Contact = nextBits[4];
                                aa.Telephone = nextBits[5];
                                aa.Cell = nextBits[6];
                                aa.Fax = nextBits[7];
                                aa.Address = new string[5];
                                for (int i = 8; i < 13; i++) { aa.Address[i - 8] = nextBits[i]; }
                                aa.Email = nextBits[13];
                                aas.Add(aa);
                            }
                        }
                    } else {
                        MessageBox.Show(returner);
                    }
                } else {
                    MessageBox.Show(returner);
                }
            } catch (Exception ex) {
                returner = "error:" + ex.Message;
                MessageBox.Show(returner);
            }
            return aas;
        }

        private String[] SplitDeliveryInfo(String delString) {
            String[] stringSplitter = new String[] { "|" };
            String[] delBits = delString.Split(stringSplitter, StringSplitOptions.None);
            // try { Email = delBits[13]; } catch { Email = ""; }
            return delBits;
        }

        public List<Detail> GetTransactions(String buildPath, short period, String acc) {
            List<Detail> customers = new List<Detail>();
            List<String> rs = new List<string>();
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                //11 = GDC + ACC + Per + Refrence
                //period = (short)GetPeriod(path);
                //MessageBox.Show(((int)period).ToString());
                try {
                    String fileName = "ACCTRN";
                    String keyValue = SDK.RightPad("D", 1) + SDK.RightPad(acc, 7) + SDK.MKI(period);
                    String response = SDK.GetNearest(fileName, 11, keyValue);
                    String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                    //MessageBox.Show(response);
                    try {
                        DateTime jDate = SDK.BtrieveToVBDate(responseBits[7]);
                        //MessageBox.Show(jDate.ToString());
                    } catch { }
                    rs.Add(response);

                    if (!response.StartsWith("5") && !response.StartsWith("9|")) {
                        int i = 0;
                        while (!returner.StartsWith("9|")) {
                            response = SDK.GetNext(fileName, 11);
                            responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                            //MessageBox.Show(response);

                            if (i < 5) {
                                try {
                                    DateTime jDate = SDK.BtrieveToVBDate(responseBits[7]);
                                    MessageBox.Show(jDate.ToString());
                                } catch { }

                                //MessageBox.Show(response);
                            } else { break; }
                            i++;
                            rs.Add(returner);
                        }
                    }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            } else {
                //MessageBox.Show(returner);
            }
            //String dir = Directory.GetCurrentDirectory();
            //String fName = string.Format("{0}\\{1}.txt", dir, "transactions");
            //string[] lines = rs.ToArray();
            //System.IO.File.WriteAllLines(fName, lines);

            return customers;
        }

        public List<Trns> GetTransactions(String buildPath, int startperiod, int endperiod, String acc)
        {
            List<Trns> rs = new List<Trns>();
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            //period = 6
            if (returner == "0")
            {
                short period = (short)startperiod;
                try
                {
                    String fileName = "ACCTRN";
                    String keyValue = SDK.RightPad("D", 1) + SDK.RightPad(acc, 7) + SDK.MKI(period);
                    String response = SDK.GetNearest(fileName, 11, keyValue);

                    String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                    //MessageBox.Show(startperiod.ToString() + " - " + endperiod.ToString() + " - " + responseBits[6]);
                    if (response.StartsWith("0|") && int.Parse(responseBits[6]) >= startperiod && int.Parse(responseBits[6]) <= endperiod)
                    {
                        try
                        {
                            String trnAcc = responseBits[2];
                            if (trnAcc.Contains(acc))
                            {
                                Trns trn = new Trns();
                                trn.Amount = responseBits[11];
                                trn.Date = SDK.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                trn.Description = responseBits[18];
                                trn.Reference = responseBits[9];
                                trn.period = responseBits[6];
                                rs.Add(trn);
                            }
                        }
                        catch { }
                        while (response.StartsWith("0|"))
                        {
                            response = SDK.GetNext(fileName, 11);
                            responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                            //MessageBox.Show(response);
                            if (response.StartsWith("0|") && int.Parse(responseBits[6]) >= startperiod && int.Parse(responseBits[6]) <= endperiod)
                            {
                                try
                                {
                                    String trnAcc = responseBits[2];
                                    if (trnAcc.Contains(acc))
                                    {
                                        Trns trn = new Trns();
                                        trn.Amount = responseBits[11];
                                        trn.Date = SDK.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                        trn.Description = responseBits[18];
                                        trn.Reference = responseBits[9];
                                        trn.period = responseBits[6];
                                        rs.Add(trn);
                                    }
                                }
                                catch { }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    returner = "error:" + ex.Message;
                }
            }
            else
            {
                MessageBox.Show(returner);
            }
            return rs;
        }

        public List<Trns> GetTransactions(String buildPath, String gdc, int startperiod, int endperiod, String acc) {
            List<Trns> rs = new List<Trns>();
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            //period = 6
            if (returner == "0") {
                short period = (short)startperiod;
                try {
                    String fileName = "ACCTRN";
                    String keyValue = SDK.RightPad(gdc, 1) + SDK.RightPad(acc, 7) + SDK.MKI(period);
                    String response = SDK.GetNearest(fileName, 11, keyValue);

                    String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                    //MessageBox.Show(startperiod.ToString() + " - " + endperiod.ToString() + " - " + responseBits[6]);
                    if (!response.StartsWith("5") && !response.StartsWith("9|") && int.Parse(responseBits[6]) >= startperiod && int.Parse(responseBits[6]) <= endperiod) {
                        try {
                            String trnAcc = responseBits[2];
                            if (trnAcc == acc) {
                                Trns trn = new Trns();
                                trn.Amount = responseBits[11];
                                trn.Date = SDK.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                trn.Description = responseBits[18];
                                trn.Reference = responseBits[9];
                                rs.Add(trn);
                            }
                        } catch { }
                        while (!response.StartsWith("9|")) {
                            response = SDK.GetNext(fileName, 11);
                            responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                            if (!response.StartsWith("5") && !response.StartsWith("9|") && int.Parse(responseBits[6]) >= startperiod && int.Parse(responseBits[6]) <= endperiod) {
                                try {
                                    String trnAcc = responseBits[2];
                                    if (trnAcc == acc) {
                                        Trns trn = new Trns();
                                        trn.Amount = double.Parse(responseBits[11]).ToString("#,##0.00");
                                        trn.Date = SDK.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                        trn.Description = responseBits[18];
                                        trn.Reference = responseBits[9];
                                        rs.Add(trn);
                                    }
                                } catch { }
                            } else {
                                break;
                            }
                        }
                    }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            } else {
                //MessageBox.Show(returner);
            }
            return rs;
        }

        public int GetPeriod(String buildPath) {
            int period = 0;
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCUSER";
                    String keyValue = "B";
                    String response = SDK.GetNearest(fileName, 0, keyValue);
                    String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                    if (responseBits.Length >= 83) {
                        for (int i = 73; i <= 83; i++) {
                            period = (int.Parse(responseBits[i]) > period ? int.Parse(responseBits[i]) : period);
                        }
                    } else {
                        // MessageBox.Show(response);
                    }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            } else {
                // MessageBox.Show(returner);
            }
            return period;
        }

        public String GetBankDetails(String buildPath) {
            String bankDetails = String.Empty;
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCPRMDC";
                    String keyValue = "0";
                    String response = SDK.GetNearest(fileName, 0, keyValue);
                    //MessageBox.Show(response);
                    String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                    bankDetails = responseBits[17];
                } catch (Exception ex) {
                    bankDetails = ex.Message;
                }
            }
            return bankDetails;
        }

        public Dictionary<String, String> GetCategories(String buildPath) {
            Dictionary<String, String> categories = new Dictionary<string, string>();
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCDCAT";
                    String keyValue = "001";
                    String response = SDK.GetNearest(fileName, 0, keyValue);
                    int cats = SDK.NumberOfRecords(fileName);
                    if (response.StartsWith("0|")) {
                        String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                        String categoryID = responseBits[1];
                        String categoryName = responseBits[2];
                        if (!categories.ContainsKey(categoryID)) { categories.Add(categoryID, categoryName); }
                    }
                    while (response.StartsWith("0|")) {
                        response = SDK.GetNext(fileName, 0);
                        if (response.StartsWith("0|")) {
                            String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                            String categoryID = responseBits[1];
                            String categoryName = responseBits[2];
                            if (!categories.ContainsKey(categoryID)) { categories.Add(categoryID, categoryName); }
                        }
                    }
                } catch (Exception ex) {
                }
            }
            return categories;
        }

        public String GetAccount(String buildPath) {
            String account = "";
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCMAS";
                    String keyValue = "9250000";
                    returner = SDK.GetRecord(fileName, keyNumber, keyValue);
                    if (Regex.Matches(returner, "|").Count > 5) { account = returner; }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            }
            return account;
        }

        public Dictionary<String, String> GetAccountList(String buildPath) {
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            Dictionary<String, String> accounts = new Dictionary<string, string>();
            if (returner == "0") {
                try {
                    String fileName = "ACCMAS";
                    String keyValue = "0";
                    returner = SDK.GetNearest(fileName, keyNumber, keyValue);
                    if (returner.StartsWith("0|")) {
                        String[] accBits = returner.Split(new String[] { "|" }, StringSplitOptions.None);
                        String accNumber = accBits[2];
                        String accDescription = accBits[3];
                        if (!accounts.ContainsKey(accNumber)) { accounts.Add(accNumber, accDescription); }
                    }
                    while (returner.StartsWith("0|")) {
                        returner = SDK.GetNext(fileName, keyNumber);
                        if (returner.StartsWith("0|")) {
                            String[] accBits = returner.Split(new String[] { "|" }, StringSplitOptions.None);
                            String accNumber = accBits[2];
                            String accDescription = accBits[3];
                            if (!accounts.ContainsKey(accNumber)) { accounts.Add(accNumber, accDescription); }
                        }
                    }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            }
            return accounts;
        }

        public String GetAccount(String buildPath, String acc) {
            String account = "";
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCMAS";
                    String keyValue = (acc != "9250000" ? acc : "9250000");
                    returner = SDK.GetRecord(fileName, keyNumber, keyValue);
                    if (Regex.Matches(returner, "|").Count > 5) { account = returner; }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            }
            return account;
        }

        public List<String> GetAccounts(String buildPath) {
            List<String> accounts = new List<string>();
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCMAS";
                    String keyValue = SDK.MKI(0);
                    returner = SDK.GetNearest(fileName, keyNumber, keyValue);
                    if (Regex.Matches(returner, "|").Count > 5) { accounts.Add(returner); }
                    while (!returner.StartsWith("9|")) {
                        returner = SDK.GetNext(fileName, keyNumber);
                        if ((Regex.Matches(returner, "|").Count > 5) && (!returner.StartsWith("9|"))) { accounts.Add(returner); }
                    }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            }
            return accounts;
        }

        public String SetPath(String buildPath, out String myPath) {
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            myPath = pastelDirectory + "\\" + buildPath;
            if (returner != "0") {
                myPath = "\\\\SERVER2\\Pastel11\\" + buildPath;
                returner = SDK.SetDataPath(myPath);
            }
            return returner;
        }

        public Dictionary<int, String> GetCustomerCategories(String buildPath) {
            Dictionary<int, String> categories = new Dictionary<int, string>();
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (returner == "0") {
                try {
                    String fileName = "ACCDCAT";
                    String keyValue = SDK.MKI(-1);
                    returner = SDK.GetNearest(fileName, keyNumber, keyValue);
                    if ((Regex.Matches(returner, "|").Count > 1) && (!returner.StartsWith("9|"))) {
                        String[] rBits = returner.Split(new String[] { "|" }, StringSplitOptions.None);
                        if (!categories.ContainsKey(int.Parse(rBits[1]))) { categories.Add(int.Parse(rBits[1]), rBits[2]); }
                    }
                    while (!returner.StartsWith("9|")) {
                        returner = SDK.GetNext(fileName, keyNumber);
                        if ((Regex.Matches(returner, "|").Count > 1) && (!returner.StartsWith("9|"))) {
                            String[] rBits = returner.Split(new String[] { "|" }, StringSplitOptions.None);
                            if (!categories.ContainsKey(int.Parse(rBits[1]))) { categories.Add(int.Parse(rBits[1]), rBits[2]); }
                        }
                    }
                } catch (Exception ex) {
                    returner = "error:" + ex.Message;
                }
            }
            return categories;
        }

        public DateTime GetDate(String inDate) {
            return SDK.BtrieveToVBDate(inDate);
        }

        public List<String> getNotes(String customerCode, String buildPath) {
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            List<String> notes = new List<string>();
            if (returner.Contains("99")) {
            } else {
                short[] nTypes = new short[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                try {
                    String fileName = "ACCNOTE";
                    short keyNumber = 2;
                    foreach (short nType in nTypes) {
                        String keyValue = SDK.MKI(nType) + "|" + customerCode;
                        String record = SDK.GetNearest(fileName, keyNumber, keyValue);
                        if (record != "") {
                            notes.Add(record);
                            while (record != "") {
                                record = SDK.GetNext(fileName, keyNumber);
                                if (record != "") { notes.Add(record); }
                            }
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.StackTrace);
                }
            }
            return notes;
        }

        public DateTime convertDate(String inDate) {
            DateTime outDate = DateTime.Now;
            DateTime.TryParse(inDate, out outDate);
            return outDate;
        }

        private String PostBatch(String buildPath, String StrIn, int entryType) {
            String StrReturn = "0";
            String strCodeIn;
            String returner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            if (StrReturn == "0") { StrReturn = SDK.SetGLPath("C:\\Pastel11"); }
            if (StrReturn == "0") {
                strCodeIn = StrIn;
                short eType = (short)entryType;
                // MessageBox.Show(buildPath + " - " + entryType.ToString() + " - " + StrIn);
                StrReturn = SDK.ImportGLBatch(StrIn, eType);
            }
            if (StrReturn.Length == 0) {
                return "9";
            }
            return StrReturn;
        }

        public int getPeriod(DateTime trnDate) {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth);
            return myMonth;
        }

        public String PostBatch(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
            String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString) {
            double pAmt = double.Parse(amt);
            String returnValue = "";
            String StrIn;
            if (pAmt < 0) {
                String returner = "";
                int trustPeriod = getPeriod(trnDate);
                buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
                pastelString = "";
                //Building
                if (rAcc != "") {
                    String dbAmt = (pAmt * -1).ToString("##0.00");
                    StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|G|" + buildContra.Replace("/", "") + "|" + buildAcc + "|" + description + "|" + dbAmt + "|0|0|A|||0|0|" + rAcc + "|1|1";
                    pastelString = "; Building = " + StrIn;
                    String pastelResult = PostBatch(buildPath, StrIn, buildType);
                    returner += pastelResult;
                    returnValue += "; Building = " + returner;
                }
            } else {
                String returner = "";
                int trustPeriod = getPeriod(trnDate);
                buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
                //Centrec
                StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + bc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + trustAcc.Replace("/", "") + "|1|1";
                pastelString = "Centrec = " + StrIn;
                //MessageBox.Show(pastelString);
                returner = PostBatch(trustPath, StrIn, trustType);
                //MessageBox.Show(trustPath + " - " + returner);
                returnValue += "Centrec = " + returner;
                //Building
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + buildAcc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
                //MessageBox.Show(buildType.ToString() + ": " + StrIn);
                pastelString += "; Building = " + StrIn;
                returner = PostBatch(buildPath, StrIn, buildType);
                //MessageBox.Show(returner);
                returnValue += "; Building = " + returner;
            }
            return returnValue;
        }

        public String PostReverseBatch(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
            String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString) {
            double pAmt = double.Parse(amt);
            String returnValue = "";
            String StrIn;

            String returner = "";
            int trustPeriod = getPeriod(trnDate);
            buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
            //Centrec
            StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + bc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + trustAcc.Replace("/", "") + "|1|1";
            pastelString = "Centrec = " + StrIn;
            returner = PostBatch(trustPath, StrIn, trustType);
            returnValue += "Centrec = " + returner;
            //Building
            StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + buildAcc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
            pastelString += "; Building = " + StrIn;
            returner = PostBatch(buildPath, StrIn, buildType);
            returnValue += "; Building = " + returner;

            return returnValue;
        }

        public String PostCredit(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
     String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString) {
            double pAmt = double.Parse(amt);
            double cAmt = pAmt * -1;
            String returnValue = "";
            String StrIn;
            String returner = "";
            int trustPeriod = getPeriod(trnDate);
            buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
            //Centrec
            StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + bc + "|" + buildAcc + "|" + description + "|" + pAmt.ToString("#0.00") + "|0|0|A|||0|0|" + trustAcc.Replace("/", "") + "|1|1";
            pastelString = "Centrec = " + StrIn;
            returner = PostBatch(trustPath, StrIn, trustType);
            returnValue += "Centrec = " + returner;
            //Building
            if (rAcc != "") {
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + rAcc + "|" + buildAcc + "|" + description + "|" + pAmt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
                pastelString += "; Building = " + StrIn;
                returner += PostBatch(buildPath, StrIn, buildType);
                returnValue += "; Building = " + returner;
            }
            return returnValue;
        }

        public String PostCredit2(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
     String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString) {
            double pAmt = double.Parse(amt);
            double cAmt = pAmt * -1;
            String returnValue = "";
            String StrIn;
            String returner = "";
            int trustPeriod = getPeriod(trnDate);
            buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
            //Centrec
            StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + bc + "|" + buildAcc + "|" + description + "|" + pAmt.ToString("#0.00") + "|0|0|A|||0|0|0000000|1|1";
            String StrIn2 = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|G|" + trustAcc.Replace("/", "") + "|" + buildAcc + "|" + description + "|" + cAmt.ToString("#0.00") + "|0|0|A|||0|0|0000000|1|1";
            pastelString = "Centrec = " + StrIn;
            returner = PostBatch(trustPath, StrIn, trustType);
            if (returner != "0") { MessageBox.Show("Centrec error: acc " + bc); }
            returnValue += "Centrec = " + returner;
            returner = PostBatch(trustPath, StrIn2, trustType);
            if (returner != "0") { MessageBox.Show("Centrec error: acc " + trustAcc); }
            pastelString += " -- Centrec = " + StrIn2;
            returnValue += " -- Centrec = " + returner;
            //Building
            if (rAcc != "") {
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + rAcc + "|" + buildAcc + "|" + description + "|" + pAmt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
                //MessageBox.Show(StrIn);
                pastelString += "; Building = " + StrIn;
                returner = PostBatch(buildPath, StrIn, buildType);
                if (returner != "0") { MessageBox.Show("Building error: acc " + rAcc); }
                returnValue += "; Building = " + returner;
            }
            return returnValue;
        }

        public void PostBusBatch(DateTime trnDate, int trustType, String acc, String buildAcc, String reference, String description, String amt) {
            String StrIn = "";
            String returner = "";
            int trustPeriod = getPeriod(trnDate);
            StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + acc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|1120000|1|1";
            //MessageBox.Show(StrIn + " - " + trustType.ToString());
            returner = PostBatch("ASTROD17", StrIn, trustType);
            //MessageBox.Show(returner);
        }

        public void PostBusGBatch(DateTime trnDate, int trustType, String acc, String buildAcc, String reference, String description, String amt) {
            String StrIn = "";
            String returner = "";
            int trustPeriod = getPeriod(trnDate);
            StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|G|" + acc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|1120000|1|1";
            //MessageBox.Show(StrIn + " - " + trustType.ToString());
            returner = PostBatch("ASTROD17", StrIn, trustType);
            //MessageBox.Show(returner);
        }

        public void PostBuildBatch(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt, out String returner) {
            String StrIn = "";

            int trustPeriod = getPeriod(trnDate);
            buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
            StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
            //MessageBox.Show(StrIn + " - " + buildType.ToString());
            returner = PostBatch(buildPath, StrIn, buildType);
            //MessageBox.Show(returner);
        }

        public void PostBuildBatchDirect(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt, out String StrIn, out String returner) {
            StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
            //MessageBox.Show(StrIn + " - " + buildType.ToString());
            returner = PostBatch(buildPath, StrIn, buildType);
            //MessageBox.Show(returner);
        }

        public void PostBuildBatchC(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt, out String returner) {
            String StrIn = "";
            StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|C|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
            //MessageBox.Show(StrIn + " - " + buildType.ToString());
            returner = PostBatch(buildPath, StrIn, buildType);
            //MessageBox.Show(returner);
        }

        public void PostDirect(String entry, String buildPath, int buildType) {
            String returner = PostBatch(buildPath, entry, buildType);
            //MessageBox.Show(returner);
        }

        public void PostBuildCredit(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt) {
            String StrIn = "";
            String returner = "";
            StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
            //MessageBox.Show(StrIn + " - " + trustType.ToString());
            returner = PostBatch(buildPath, StrIn, buildType);
            //MessageBox.Show(returner);
        }

        public String UpdateCustomer(String customer, String buildPath) {
            String pathReturner = SDK.SetDataPath(pastelDirectory + "\\" + buildPath);
            String returner = String.Empty;
            if (pathReturner.Contains("99")) {
            } else {
                returner = SDK.ImportCustomer(customer);
                if (returner != "0") {
                    MessageBox.Show("Error updating customer:" + returner, "Pastel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return returner;
        }
    }

    public class MessageArgs : EventArgs {
        public String message { get; set; }

        public MessageArgs(String Message) {
            message = Message;
        }
    }

    public class CustomerArgs : EventArgs {
        public Customer customer { get; set; }

        public String building { get; set; }

        public CustomerArgs(Customer c, String b) {
            customer = c; building = b;
        }
    }
}
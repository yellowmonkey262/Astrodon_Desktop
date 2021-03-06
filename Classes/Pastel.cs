﻿using Astro.Library.Entities;
using Astrodon.Classes;
using PasSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using Astrodon.ReportService;
using System.Data;

namespace Astrodon
{
    public class Pastel
    {
        private short keyNumber = 0;
        public static string PastelRoot { get; private set; }
        public volatile bool runSearch = false;

        #region Event Handler

        public delegate void MessageHandler(object sender, MessageArgs e);

        public event MessageHandler Message;

        private void RaiseEvent(String message)
        {
            if (Message != null) { Message(this, new MessageArgs(message)); }
        }

        public delegate void CustomerFoundEventHandler(object sender, CustomerArgs e);

        public event CustomerFoundEventHandler CustomerFound;

        private void RaiseCustomerFoundEvent(Customer c, String b)
        {
            if (CustomerFound != null) { CustomerFound(this, new CustomerArgs(c, b)); }
        }

        #endregion Event Handler

        private PastelPartnerSDK CreateSDK()
        {
            String lc, auth;
            var sdk = new PastelPartnerSDK();
            lc = "DK11110068";
            auth = "4228113";
            if (Environment.MachineName == "SERVER2")
                PastelRoot = @"K:\Pastel11\";
            else
                PastelRoot = @"K:\";


            string searchFolders = "";

            if (!Directory.Exists(PastelRoot))
            {
                searchFolders += PastelRoot + " does not exist\n";
                PastelRoot = "\\\\SERVER2\\Pastel11\\";
                if (!Directory.Exists(PastelRoot))
                {
                    searchFolders += PastelRoot + " does not exist\n";
                    PastelRoot = "C:\\Pastel11\\";
                    if (!Directory.Exists(PastelRoot))
                    {
                        searchFolders += PastelRoot + " does not exist\n";
                        PastelRoot = "C:\\Pastel12\\";
                        lc = "DK12111473";
                        auth = "3627008";

                        if (!Directory.Exists(PastelRoot))
                            throw new Exception("Pastel folder not found - Searched in: " + searchFolders);
                    }

                }
            }

            sdk.SetLicense(lc, auth);
            return sdk;
        }

        public Pastel()
        {
           
        }

        #region Customer

        public List<Customer> AddCustomers(String buildKey, String buildPath, bool showErrors = false)
        {
            var sdk = CreateSDK();
            try
            {
                List<Customer> customers = new List<Customer>();
                String path = "";
                path = Path.Combine(PastelRoot, buildPath);
                if (!Directory.Exists(path))
                {
                    path = Path.Combine(PastelRoot, buildPath);
                }
                else if (Directory.Exists(Path.Combine("C:\\Pastel12", buildPath)))
                {
                    path = Path.Combine("C:\\Pastel12", buildPath);
                    PastelRoot = "C:\\Pastel12";
                }
                
                String returner = sdk.SetDataPath(path);


                if (returner.Contains("99"))
                {
                    if (showErrors)
                        Controller.HandleError("Patel returned " + returner + " reading path: " + path);
                }
                else
                {
                    int records = sdk.NumberOfRecords("ACCMASD");
                    if (records > 0)
                    {
                        List<String> customerStrings = GetCustomers(buildPath);
                        foreach (String customer in customerStrings)
                        {
                            Customer newCustomer = new Customer(customer);
                            String lastCrDate = customer.Split(new String[] { "|" }, StringSplitOptions.None)[66];
                            String createDate = customer.Split(new String[] { "|" }, StringSplitOptions.None)[127];
                            newCustomer.lastCrDate = convertDate(lastCrDate).ToString("yyyy/MM/dd");
                            newCustomer.createDate = convertDate(createDate).ToString("yyyy/MM/dd");
                            newCustomer.SetDeliveryInfo(DeliveryInfo(newCustomer.accNumber));
                            customers.Add(newCustomer);
                        }
                    }
                    else
                    {
                        if (showErrors)
                            MessageBox.Show("Pastel returned zero customer records");
                    }
                }
                customers = customers.OrderBy(c => c.accNumber).ToList();
                return customers;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public void SearchCustomers(List<String> searchCriteria, Dictionary<String, String> buildings, bool showErrors = true)
        {
            var sdk = CreateSDK();
            try
            {
                String status = String.Empty;
                runSearch = true;
                bool searchRunning = true;
                try
                {
                    foreach (KeyValuePair<String, String> building in buildings)
                    {
                        String returner = sdk.SetDataPath(PastelRoot + "\\" + building.Value);
                        if (returner != "0")
                        {
                            status = "Returner = " + returner + " Customers: " + PastelRoot + "\\" + building.Value;
                            if (showErrors)
                                RaiseEvent(status);
                        }
                        else
                        {
                            status = "";
                            int records = sdk.NumberOfRecords("ACCMASD");
                            if (records > 0)
                            {
                                List<String> customerStrings = GetCustomers(building.Value);
                                Parallel.ForEach(customerStrings, customer =>
                                {
                                    Customer newCustomer = new Customer(customer);
                                    String lastCrDate = customer.Split(new String[] { "|" }, StringSplitOptions.None)[66];
                                    String createDate = customer.Split(new String[] { "|" }, StringSplitOptions.None)[127];
                                    newCustomer.lastCrDate = convertDate(lastCrDate).ToString("yyyy/MM/dd");
                                    newCustomer.createDate = convertDate(createDate).ToString("yyyy/MM/dd");

                                    newCustomer.SetDeliveryInfo(DeliveryInfo(newCustomer.accNumber));
                                    bool foundCustomer = false;
                                    foreach (String searchCrit in searchCriteria)
                                    {
                                        if (newCustomer.description.ToLower().Contains(searchCrit.ToLower()))
                                        {
                                            foundCustomer = true;
                                            break;
                                        }
                                        else
                                        {
                                            foreach (String cEmail in newCustomer.Email)
                                            {
                                                if (cEmail.ToLower().Contains(searchCrit.ToLower()))
                                                {
                                                    foundCustomer = true;
                                                    break;
                                                }
                                            }
                                            if (foundCustomer) { break; }
                                        }
                                    }
                                    if (foundCustomer) { RaiseCustomerFoundEvent(newCustomer, building.Key); }
                                    if (!runSearch)
                                    {
                                        searchRunning = false;
                                        RaiseEvent("Search aborted");
                                        return;
                                    }
                                });
                            }
                        }
                        if (!runSearch && searchRunning)
                        {
                            RaiseEvent("Search aborted");
                            break;
                        }
                    }
                    if (runSearch && searchRunning) { RaiseEvent("Search complete"); }
                }
                catch (Exception ex)
                {
                    if (showErrors)
                        RaiseEvent("Error: " + ex.Message);
                }
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public List<String> GetCustomers(String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                List<String> customers = new List<string>();
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCMASD";
                        String keyValue = sdk.MKI(0);
                        returner = sdk.GetNearest(fileName, keyNumber, keyValue);
                        if (Regex.Matches(returner, "|").Count > 5) { customers.Add(returner); }
                        while (!returner.StartsWith("9|"))
                        {
                            returner = sdk.GetNext(fileName, keyNumber);
                            if ((Regex.Matches(returner, "|").Count > 5) && (!returner.StartsWith("9|"))) { customers.Add(returner); }
                        }
                    }
                    catch (Exception ex)
                    {
                        returner = "error:" + ex.Message;
                    }
                }
                return customers;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public Customer GetOneCustomer(String buildPath, String accNumber)
        {
            var sdk = CreateSDK();
            try
            {
                if (!string.IsNullOrWhiteSpace(buildPath) && !string.IsNullOrWhiteSpace(accNumber))
                {
                    String customerString = GetCustomer(buildPath, accNumber);
                    Customer newCustomer = new Customer(customerString);
                    String lastCrDate = customerString.Split(new String[] { "|" }, StringSplitOptions.None)[66];
                    String createDate = customerString.Split(new String[] { "|" }, StringSplitOptions.None)[127];
                    newCustomer.lastCrDate = convertDate(lastCrDate).ToString("yyyy/MM/dd");
                    newCustomer.createDate = convertDate(createDate).ToString("yyyy/MM/dd");
                    return newCustomer;
                }
                return null;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String GetCustomer(String buildPath, String accNumber)
        {
            var sdk = CreateSDK();
            try
            {
                String account = "";
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCMASD";
                        String keyValue = accNumber;
                        returner = sdk.GetRecord(fileName, keyNumber, keyValue);
                        if (Regex.Matches(returner, "|").Count > 5 && returner.StartsWith("0")) { account = returner; }
                    }
                    catch (Exception ex)
                    {
                        returner = "error:" + ex.Message;
                    }
                }
                return account;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String[] DeliveryInfo(String customerAcc)
        {
            var sdk = CreateSDK();
            try
            {
                //ACCDELIV
                String returner = "";
                String[] delBits = new string[1];
                try
                {
                    String fileName = "ACCDELIV";
                    short keyNumber = 2;
                    returner = sdk.GetNearest(fileName, keyNumber, customerAcc);
                    delBits = SplitDeliveryInfo(returner);
                    //if (delBits[13].Contains("@imp.ad-one.co.za")) { delBits[13] = ""; }
                    String rCustAcc = delBits[1];
                    while (rCustAcc == customerAcc)
                    {
                        returner = sdk.GetNext(fileName, keyNumber);
                        String[] nextBits = SplitDeliveryInfo(returner);
                        rCustAcc = nextBits[1];
                        try
                        {
                            if (rCustAcc == customerAcc && delBits[13] != nextBits[13] && nextBits[13] != "") { delBits[13] += (delBits[13] != "" ? ";" : "") + nextBits[13]; }
                        }
                        catch (Exception ex) { Controller.HandleError(ex); }

                    }
                    //MessageBox.Show(delBits[13]);
                }
                catch (Exception ex)
                {
                    Controller.HandleError(ex);
                    returner = "error:" + ex.Message;
                }
                return delBits;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String[] DeliveryInfo(String buildPath, String customerAcc)
        {
            var sdk = CreateSDK();
            try
            {
                //ACCDELIV
                String returner = "";
                String[] delBits = new string[1];
                try
                {
                    returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                    if (returner == "0")
                    {
                        String fileName = "ACCDELIV";
                        short keyNumber = 2;
                        returner = sdk.GetNearest(fileName, keyNumber, customerAcc);
                        delBits = SplitDeliveryInfo(returner);
                        //if (delBits[13].Contains("@imp.ad-one.co.za")) { delBits[13] = ""; }
                        String rCustAcc = delBits[1];
                        while (rCustAcc == customerAcc)
                        {
                            returner = sdk.GetNext(fileName, keyNumber);
                            String[] nextBits = SplitDeliveryInfo(returner);
                            rCustAcc = nextBits[1];
                            try
                            {
                                if (rCustAcc == customerAcc && delBits[13] != nextBits[13] && nextBits[13] != "") { delBits[13] += (delBits[13] != "" ? ";" : "") + nextBits[13]; }
                            }
                            catch (Exception ex) { Controller.HandleError(ex); }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Controller.HandleError(ex);
                    returner = "error:" + ex.Message;
                }
                return delBits;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public List<AdditionalAddress> GetDeliveryInfo(String buildPath, String customerAcc)
        {
            var sdk = CreateSDK();
            try
            {
                //ACCDELIV
                List<AdditionalAddress> aas = new List<AdditionalAddress>();
                String returner = "";
                String[] delBits = new string[1];
                try
                {
                    returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                    if (returner == "0")
                    {
                        String fileName = "ACCDELIV";
                        short keyNumber = 2;
                        returner = sdk.GetNearest(fileName, keyNumber, customerAcc);
                        delBits = SplitDeliveryInfo(returner);

                        String rCustAcc = delBits[1];
                        AdditionalAddress aa = new AdditionalAddress();
                        if (rCustAcc == customerAcc)
                        {
                            aa.Contact = delBits[4];
                            aa.Telephone = delBits[5];
                            aa.Cell = delBits[6];
                            aa.Fax = delBits[7];
                            aa.Address = new string[5];
                            for (int i = 8; i < 13; i++) { aa.Address[i - 8] = delBits[i]; }
                            aa.Email = delBits[13];
                            aas.Add(aa);
                            while (rCustAcc == customerAcc)
                            {
                                returner = sdk.GetNext(fileName, keyNumber);
                                String[] nextBits = SplitDeliveryInfo(returner);
                                rCustAcc = nextBits[1];
                                if (rCustAcc == customerAcc)
                                {
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
                        }
                        else
                        {
                            MessageBox.Show(returner);
                        }
                    }
                    else
                    {
                        MessageBox.Show(returner);
                    }
                }
                catch (Exception ex)
                {
                    Controller.HandleError(ex);
                    returner = "error:" + ex.Message;
                    // MessageBox.Show(returner);
                }
                return aas;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        private String[] SplitDeliveryInfo(String delString)
        {
            String[] stringSplitter = new String[] { "|" };
            String[] delBits = delString.Split(stringSplitter, StringSplitOptions.None);
            // try { Email = delBits[13]; } catch { Email = ""; }
            return delBits;
        }

        public List<CustomerCategory> GetCustomerCategories(String buildPath)
        {
            using (var reportService = ReportServiceClient.CreateInstance())
            {
                return reportService.GetCustomerCategories(buildPath).ToList();
            }
        }

        public List<String> getNotes(String customerCode, String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                List<String> notes = new List<string>();
                if (returner.Contains("99"))
                {
                }
                else
                {
                    short[] nTypes = new short[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                    try
                    {
                        String fileName = "ACCNOTE";
                        short keyNumber = 2;
                        foreach (short nType in nTypes)
                        {
                            String keyValue = sdk.MKI(nType) + "|" + customerCode;
                            String record = sdk.GetNearest(fileName, keyNumber, keyValue);
                            if (record != "")
                            {
                                notes.Add(record);
                                while (record != "")
                                {
                                    record = sdk.GetNext(fileName, keyNumber);
                                    if (record != "") { notes.Add(record); }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);

                    }
                }
                return notes;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String UpdateCustomer(String customer, String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                String pathReturner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                String returner = String.Empty;
                if (pathReturner.Contains("99"))
                {
                    MessageBox.Show("Pastel SNAFU", "Pastel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    returner = sdk.ImportCustomer(customer);
                    if (returner != "0")
                    {
                        MessageBox.Show("Error updating customer:" + returner, "Pastel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return returner;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        #endregion Customer

        #region Transactions

        public List<Detail> GetTransactions(String buildPath, short period, String acc)
        {
            var sdk = CreateSDK();
            try
            {
                List<Detail> customers = new List<Detail>();
                List<String> rs = new List<string>();


                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);

                if (returner == "0")
                {
                    //11 = GDC + ACC + Per + Refrence
                    //period = (short)GetPeriod(path);
                    //MessageBox.Show(((int)period).ToString());
                    try
                    {
                        String fileName = "ACCTRN";
                        String keyValue = sdk.RightPad("D", 1) + sdk.RightPad(acc, 7) + sdk.MKI(period);
                        String response = sdk.GetNearest(fileName, 11, keyValue);
                        String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                        //MessageBox.Show(response);
                        try
                        {
                            DateTime jDate = sdk.BtrieveToVBDate(responseBits[7]);
                            //MessageBox.Show(jDate.ToString());
                        }
                        catch (Exception ex) { Controller.HandleError(ex); }
                        rs.Add(response);

                        if (!response.StartsWith("5") && !response.StartsWith("9|"))
                        {
                            int i = 0;
                            while (!returner.StartsWith("9|"))
                            {
                                response = sdk.GetNext(fileName, 11);
                                responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                                //MessageBox.Show(response);

                                if (i < 5)
                                {
                                    try
                                    {
                                        DateTime jDate = sdk.BtrieveToVBDate(responseBits[7]);
                                        MessageBox.Show(jDate.ToString());
                                    }
                                    catch (Exception ex) { Controller.HandleError(ex); }

                                    //MessageBox.Show(response);
                                }
                                else { break; }
                                i++;
                                rs.Add(returner);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                        returner = "error:" + ex.Message;
                    }
                }
                else
                {
                    //MessageBox.Show(returner);
                }
                //String dir = Directory.GetCurrentDirectory();
                //String fName = string.Format("{0}\\{1}.txt", dir, "transactions");
                //string[] lines = rs.ToArray();
                //System.IO.File.WriteAllLines(fName, lines);

                return customers;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public List<Trns> GetTransactions(String buildPath, int startperiod, int endperiod, String acc)
        {
            var sdk = CreateSDK();
            try
            {
                List<Trns> rs = new List<Trns>();
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);

                //period = 6
                if (returner == "0")
                {
                    short period = (short)startperiod;
                    try
                    {
                        String fileName = "ACCTRN";
                        String keyValue = sdk.RightPad("D", 1) + sdk.RightPad(acc, 7) + sdk.MKI(period);
                        String response = sdk.GetNearest(fileName, 11, keyValue);

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
                                    trn.Date = sdk.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                    trn.Description = responseBits[18];
                                    trn.Reference = responseBits[9];
                                    trn.period = responseBits[6];
                                    rs.Add(trn);
                                }
                            }
                            catch (Exception ex) { Controller.HandleError(ex); }
                            while (response.StartsWith("0|"))
                            {
                                response = sdk.GetNext(fileName, 11);
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
                                            trn.Date = sdk.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                            trn.Description = responseBits[18];
                                            trn.Reference = responseBits[9];
                                            trn.period = responseBits[6];
                                            rs.Add(trn);
                                        }
                                    }
                                    catch (Exception ex) { Controller.HandleError(ex); }
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
                        Controller.HandleError(ex);
                        returner = "error:" + ex.Message;
                    }
                }
                else
                {
                    MessageBox.Show(returner);
                }
                return rs;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public List<Trns> GetTransactions(String buildPath, String gdc, int startperiod, int endperiod, String acc)
        {
            var sdk = CreateSDK();
            try
            {
                List<Trns> rs = new List<Trns>();
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                //period = 6
                if (returner == "0")
                {
                    short period = (short)startperiod;
                    try
                    {
                        String fileName = "ACCTRN";
                        String keyValue = sdk.RightPad(gdc, 1) + sdk.RightPad(acc, 7) + sdk.MKI(period);
                        String response = sdk.GetNearest(fileName, 11, keyValue);

                        String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                        //MessageBox.Show(startperiod.ToString() + " - " + endperiod.ToString() + " - " + responseBits[6]);
                        if (!response.StartsWith("5") && !response.StartsWith("9|") && responseBits != null && responseBits.Length >= 18 && int.Parse(responseBits[6]) >= startperiod && int.Parse(responseBits[6]) <= endperiod)
                        {
                            try
                            {
                                String trnAcc = responseBits[2];
                                if (trnAcc == acc)
                                {
                                    Trns trn = new Trns();
                                    trn.Amount = responseBits[11];
                                    trn.Date = sdk.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                    trn.Description = responseBits[18];
                                    trn.Reference = responseBits[9];
                                    rs.Add(trn);
                                }
                            }
                            catch (Exception ex) { Controller.HandleError(ex); }
                            while (!response.StartsWith("9|"))
                            {
                                response = sdk.GetNext(fileName, 11);
                                responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                                if (!response.StartsWith("5") && !response.StartsWith("9|") && responseBits.Length >= 18 && int.Parse(responseBits[6]) >= startperiod && int.Parse(responseBits[6]) <= endperiod)
                                {
                                    try
                                    {
                                        String trnAcc = responseBits[2];
                                        if (trnAcc == acc)
                                        {
                                            Trns trn = new Trns();
                                            trn.Amount = double.Parse(responseBits[11]).ToString("#,##0.00");
                                            trn.Date = sdk.BtrieveToVBDate(responseBits[7]).ToString("yyyy/MM/dd");
                                            trn.Description = responseBits[18];
                                            trn.Reference = responseBits[9];
                                            rs.Add(trn);
                                        }
                                    }
                                    catch (Exception ex) { Controller.HandleError("Response: " + response + " -> " + Environment.NewLine + ex.Message); }
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
                        Controller.HandleError(ex);
                        returner = "error:" + ex.Message;
                    }
                }
                else
                {
                    //MessageBox.Show(returner);
                }
                return rs;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        #endregion Transactions

        #region Accounts

        public String GetAccount(String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                String account = "";
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCMAS";
                        String keyValue = "9250000";
                        returner = sdk.GetRecord(fileName, keyNumber, keyValue);
                        if (Regex.Matches(returner, "|").Count > 5) { account = returner; }
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                        returner = "error:" + ex.Message;
                    }
                }
                return account;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public Dictionary<String, String> GetAccountList(String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                Dictionary<String, String> accounts = new Dictionary<string, string>();
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCMAS";
                        String keyValue = "0";
                        returner = sdk.GetNearest(fileName, keyNumber, keyValue);
                        if (returner.StartsWith("0|"))
                        {
                            String[] accBits = returner.Split(new String[] { "|" }, StringSplitOptions.None);
                            String accNumber = accBits[2];
                            String accDescription = accBits[3];
                            if (!accounts.ContainsKey(accNumber)) { accounts.Add(accNumber, accDescription); }
                        }
                        while (returner.StartsWith("0|"))
                        {
                            returner = sdk.GetNext(fileName, keyNumber);
                            if (returner.StartsWith("0|"))
                            {
                                String[] accBits = returner.Split(new String[] { "|" }, StringSplitOptions.None);
                                String accNumber = accBits[2];
                                String accDescription = accBits[3];
                                if (!accounts.ContainsKey(accNumber)) { accounts.Add(accNumber, accDescription); }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                        returner = "error:" + ex.Message;
                    }
                }
                return accounts;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String GetAccount(String buildPath, String acc)
        {
            var sdk = CreateSDK();
            try
            {
                String account = "";
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCMAS";
                        String keyValue = (acc != "9250000" ? acc : "9250000");
                        returner = sdk.GetRecord(fileName, keyNumber, keyValue);
                        if (Regex.Matches(returner, "|").Count > 5) { account = returner; }
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                        returner = "error:" + ex.Message;
                    }
                }
                return account;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public List<String> GetAccounts(String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                List<String> accounts = new List<string>();
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCMAS";
                        String keyValue = sdk.MKI(0);
                        returner = sdk.GetNearest(fileName, keyNumber, keyValue);
                        if (Regex.Matches(returner, "|").Count > 5) { accounts.Add(returner); }
                        while (!returner.StartsWith("9|"))
                        {
                            returner = sdk.GetNext(fileName, keyNumber);
                            if ((Regex.Matches(returner, "|").Count > 5) && (!returner.StartsWith("9|"))) { accounts.Add(returner); }
                        }
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                        returner = "error:" + ex.Message;
                    }
                }
                return accounts;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        #endregion Accounts

        #region Utilities

        private String SetPath(PastelPartnerSDK sdk, String buildPath, out String myPath)
        {
            String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
            myPath = PastelRoot + "\\" + buildPath;
            if (returner != "0")
            {
                myPath = "\\\\SERVER2\\Pastel11\\" + buildPath;
                returner = sdk.SetDataPath(myPath);
            }
            return returner;
        }

        private int GetPeriod(PastelPartnerSDK sdk, String buildPath)
        {

            int period = 0;
            String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
            if (returner == "0")
            {
                try
                {
                    String fileName = "ACCUSER";
                    String keyValue = "B";
                    String response = sdk.GetNearest(fileName, 0, keyValue);
                    String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                    if (responseBits.Length >= 83)
                    {
                        for (int i = 73; i <= 83; i++)
                        {
                            period = (int.Parse(responseBits[i]) > period ? int.Parse(responseBits[i]) : period);
                        }
                    }
                    else
                    {
                        // MessageBox.Show(response);
                    }
                }
                catch (Exception ex)
                {
                    Controller.HandleError(ex);
                    returner = "error:" + ex.Message;
                }
            }
            else
            {
                // MessageBox.Show(returner);
            }
            return period;
        }

        public String GetBankDetails(String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                String bankDetails = String.Empty;
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCPRMDC";
                        String keyValue = "0";
                        String response = sdk.GetNearest(fileName, 0, keyValue);
                        //MessageBox.Show(response);
                        String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                        bankDetails = responseBits[17];
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                        bankDetails = ex.Message;
                    }
                }
                return bankDetails;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public Dictionary<String, String> GetCategories(String buildPath)
        {
            var sdk = CreateSDK();
            try
            {
                Dictionary<String, String> categories = new Dictionary<string, string>();
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                if (returner == "0")
                {
                    try
                    {
                        String fileName = "ACCDCAT";
                        String keyValue = "001";
                        String response = sdk.GetNearest(fileName, 0, keyValue);
                        int cats = sdk.NumberOfRecords(fileName);
                        if (response.StartsWith("0|"))
                        {
                            String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                            String categoryID = responseBits[1];
                            String categoryName = responseBits[2];
                            if (!categories.ContainsKey(categoryID)) { categories.Add(categoryID, categoryName); }
                        }
                        while (response.StartsWith("0|"))
                        {
                            response = sdk.GetNext(fileName, 0);
                            if (response.StartsWith("0|"))
                            {
                                String[] responseBits = response.Split(new String[] { "|" }, StringSplitOptions.None);
                                String categoryID = responseBits[1];
                                String categoryName = responseBits[2];
                                if (!categories.ContainsKey(categoryID)) { categories.Add(categoryID, categoryName); }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                    }
                }
                return categories;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        private DateTime GetDate(PastelPartnerSDK sdk, String inDate)
        {
            return sdk.BtrieveToVBDate(inDate);
        }

        public DateTime convertDate(String inDate)
        {
            DateTime outDate = DateTime.Now;
            DateTime.TryParse(inDate, out outDate);
            return outDate;
        }

        #endregion Utilities

        #region Post

        private String PostBatch(PastelPartnerSDK sdk, String buildPath, String StrIn, int entryType)
        {
            try
            {
                String StrReturn = "0";
                String strCodeIn;
                String returner = sdk.SetDataPath(PastelRoot + "\\" + buildPath);
                StrReturn = sdk.SetGLPath(PastelRoot);
                if (StrReturn == "0")
                {
                    strCodeIn = StrIn;
                    short eType = (short)entryType;
                    StrReturn = sdk.ImportGLBatch(StrIn, eType);
                }
                if (StrReturn.Length == 0)
                {
                    return "9";
                }
                return StrReturn;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to post batch " + sdk == null ? "SDK is null " : "SDK has value " 
                    + " buildPath " + buildPath + Environment.NewLine
                    + " strIn " + StrIn + Environment.NewLine
                    + " entry Type " + entryType + Environment.NewLine
                    + " data path " + PastelRoot + "\\" + buildPath + Environment.NewLine
                    + " error mesg: " + ex.Message + Environment.NewLine);
            }
        }

        public int getPeriod(DateTime trnDate)
        {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth);
            return myMonth;
        }

        public String PostBatch(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
            String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString)
        {
            var sdk = CreateSDK();
            try
            {
                double pAmt = double.Parse(amt);
                String returnValue = "";
                String StrIn;
                if (pAmt < 0)
                {
                    String returner = "";
                    int trustPeriod = getPeriod(trnDate);
                    buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
                    pastelString = "";
                    //Building
                    if (rAcc != "")
                    {
                        String dbAmt = (pAmt * -1).ToString("##0.00");
                        StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|G|" + buildContra.Replace("/", "") + "|" + buildAcc + "|" + description + "|" + dbAmt + "|0|0|A|||0|0|" + rAcc + "|1|1";
                        pastelString = "; Building = " + StrIn;
                        String pastelResult = PostBatch(sdk,buildPath, StrIn, buildType);
                        returner += pastelResult;
                        returnValue += "; Building = " + returner;
                    }
                }
                else
                {
                    String returner = "";
                    int trustPeriod = getPeriod(trnDate);
                    buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
                    //Centrec
                    StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + bc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + trustAcc.Replace("/", "") + "|1|1";
                    pastelString = "Centrec = " + StrIn;
                    returner = PostBatch(sdk,trustPath, StrIn, trustType);
                    returnValue += "Centrec = " + returner;
                    //Building
                    StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + buildAcc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
                    pastelString += "; Building = " + StrIn;
                    returner = PostBatch(sdk,buildPath, StrIn, buildType);
                    returnValue += "; Building = " + returner;
                }
                return returnValue;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String PostReverseBatch(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
            String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString)
        {
            var sdk = CreateSDK();
            try
            {
                double pAmt = double.Parse(amt);
                String returnValue = "";
                String StrIn;

                String returner = "";
                int trustPeriod = getPeriod(trnDate);
                buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
                //Centrec
                StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + bc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + trustAcc.Replace("/", "") + "|1|1";
                pastelString = "Centrec = " + StrIn;
                returner = PostBatch(sdk,trustPath, StrIn, trustType);
                returnValue += "Centrec = " + returner;
                //Building
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + buildAcc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
                pastelString += "; Building = " + StrIn;
                returner = PostBatch(sdk,buildPath, StrIn, buildType);
                returnValue += "; Building = " + returner;

                return returnValue;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String PostCredit(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
     String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString)
        {
            var sdk = CreateSDK();
            try
            {
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
                returner = PostBatch(sdk,trustPath, StrIn, trustType);
                returnValue += "Centrec = " + returner;
                //Building
                if (rAcc != "")
                {
                    StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + rAcc + "|" + buildAcc + "|" + description + "|" + pAmt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
                    pastelString += "; Building = " + StrIn;
                    returner += PostBatch(sdk,buildPath, StrIn, buildType);
                    returnValue += "; Building = " + returner;
                }
                return returnValue;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public String PostCredit2(DateTime trnDate, int buildPeriod, String trustPath, String buildPath, int trustType, int buildType, String bc, String buildAcc, String trustContra, String buildContra,
     String reference, String description, String amt, String trustAcc, String rAcc, out String pastelString)
        {
            var sdk = CreateSDK();
            try
            {
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
                returner = PostBatch(sdk,trustPath, StrIn, trustType);
                if (returner != "0") { MessageBox.Show("Centrec error: acc " + bc); }
                returnValue += "Centrec = " + returner;
                returner = PostBatch(sdk,trustPath, StrIn2, trustType);
                if (returner != "0") { MessageBox.Show("Centrec error: acc " + trustAcc); }
                pastelString += " -- Centrec = " + StrIn2;
                returnValue += " -- Centrec = " + returner;
                //Building
                if (rAcc != "")
                {
                    StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + rAcc + "|" + buildAcc + "|" + description + "|" + pAmt + "|0|0|A|||0|0|" + buildContra.Replace("/", "") + "|1|1";
                    //MessageBox.Show(StrIn);
                    pastelString += "; Building = " + StrIn;
                    returner = PostBatch(sdk,buildPath, StrIn, buildType);
                    if (returner != "0") { MessageBox.Show("Building error: acc " + rAcc); }
                    returnValue += "; Building = " + returner;
                }
                return returnValue;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public void PostBusBatch(DateTime trnDate, int trustType, String acc, String buildAcc, String reference, String description, String amt)
        {
            var sdk = CreateSDK();
            try
            {
                String StrIn = "";
                String returner = "";
                int trustPeriod = getPeriod(trnDate);
                StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + acc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|1120000|1|1";
                //MessageBox.Show(StrIn + " - " + trustType.ToString());
                returner = PostBatch(sdk,SqlDataHandler.ASTRODON_Path, StrIn, trustType);
                //MessageBox.Show(returner);
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public string PostBusGBatch(DateTime trnDate, int trustType, String acc, String buildAcc, String reference, String description, String amt)
        {
            var sdk = CreateSDK();
            try
            {
                String StrIn = "";
                String returner = "";
                int trustPeriod = getPeriod(trnDate);
                StrIn = trustPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|G|" + acc + "|" + buildAcc + "|" + description + "|" + amt + "|0|0|A|||0|0|1120000|1|1";
                //MessageBox.Show(StrIn + " - " + trustType.ToString());
                returner = PostBatch(sdk, SqlDataHandler.ASTRODON_Path, StrIn, trustType);
                //MessageBox.Show(returner);
                return returner;
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public void PostBuildBatch(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt, out String returner)
        {
            var sdk = CreateSDK();
            try
            {
                String StrIn = "";

                int trustPeriod = getPeriod(trnDate);
                buildPeriod = (trustPeriod - buildPeriod < 1 ? trustPeriod - buildPeriod + 12 : trustPeriod - buildPeriod);
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
                //MessageBox.Show(StrIn + " - " + buildType.ToString());
                returner = PostBatch(sdk,buildPath, StrIn, buildType);
                //MessageBox.Show(returner);
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public void PostBuildBatchDirect(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt, out String StrIn, out String returner)
        {
            var sdk = CreateSDK();
            try
            {
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
                //MessageBox.Show(StrIn + " - " + buildType.ToString());
                returner = PostBatch(sdk,buildPath, StrIn, buildType);
                //MessageBox.Show(returner);
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        public void PostBuildBatchC(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt, out String returner)
        {
            var sdk = CreateSDK();
            try
            {
                String StrIn = "";
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|C|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
                //MessageBox.Show(StrIn + " - " + buildType.ToString());
                returner = PostBatch(sdk,buildPath, StrIn, buildType);
                //MessageBox.Show(returner);
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        //public void PostDirect(String entry, String buildPath, int buildType)
        //{
        //    String returner = PostBatch(buildPath, entry, buildType);
        //    //MessageBox.Show(returner);
        //}

        public void PostBuildCredit(DateTime trnDate, String buildPath, int buildType, int buildPeriod, String dr, String cr, String reference, String description, String amt)
        {
            var sdk = CreateSDK();
            try
            {
                String StrIn = "";
                String returner = "";
                StrIn = buildPeriod.ToString() + "|" + trnDate.ToString("dd/MM/yyyy") + "|D|" + dr + "|" + reference + "|" + description + "|" + amt + "|0|0|A|||0|0|" + cr + "|1|1";
                //MessageBox.Show(StrIn + " - " + trustType.ToString());
                returner = PostBatch(sdk,buildPath, StrIn, buildType);
                //MessageBox.Show(returner);
            }
            finally
            {
                sdk.StopBtrieve();
            }
        }

        #endregion Post
    }
}

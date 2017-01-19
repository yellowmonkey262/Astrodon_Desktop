using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Astrodon {

    public partial class usrLetters : UserControl {

        #region Variables

        private List<Building> buildings;
        private Building building;
        private String category;
        private List<CustomerList> cls;
        private List<CustomerList> customerList;
        private WordProcessor wp;
        private List<MessageConstruct> messages;
        private Dictionary<String, Customer> customerDic;
        private SqlDataHandler dh = new SqlDataHandler();
        private double minbal, reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee;
        private String centrec, business;
        private User debtor = null;

        #endregion Variables

        private class BuildingValues {
            private String buildQuery;
            public double reminderFee, reminderSplit, finalFee, finalSplit, disconnectionNoticefee, disconnectionNoticeSplit, summonsFee, summonsSplit;
            public double disconnectionFee, disconnectionSplit, handoverFee, handoverSplit;

            public BuildingValues(int buildingID, double reminder_fee, double final_fee, double summons_fee, double discon_notice_fee, double discon_fee, double handover_fee) {
                buildQuery = "SELECT reminderFee, reminderSplit, finalFee, finalSplit, disconnectionNoticefee, disconnectionNoticeSplit, summonsFee, summonsSplit, disconnectionFee, ";
                buildQuery += " disconnectionSplit, handoverFee, handoverSplit FROM tblBuildingSettings WHERE buildingID = @id";
                SqlDataHandler dh = new SqlDataHandler();
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@id", buildingID);
                String status;
                DataSet ds = dh.GetData(buildQuery, sqlParms, out status);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                    DataRow dr = ds.Tables[0].Rows[0];
                    reminderFee = double.Parse(dr["reminderFee"].ToString());
                    reminderSplit = double.Parse(dr["reminderSplit"].ToString());
                    finalFee = double.Parse(dr["finalFee"].ToString());
                    finalSplit = double.Parse(dr["finalSplit"].ToString());
                    disconnectionNoticefee = double.Parse(dr["disconnectionNoticefee"].ToString());
                    disconnectionNoticeSplit = double.Parse(dr["disconnectionNoticeSplit"].ToString());
                    summonsFee = double.Parse(dr["summonsFee"].ToString());
                    summonsSplit = double.Parse(dr["summonsSplit"].ToString());
                    disconnectionFee = double.Parse(dr["disconnectionFee"].ToString());
                    disconnectionSplit = double.Parse(dr["disconnectionSplit"].ToString());
                    handoverFee = double.Parse(dr["handoverFee"].ToString());
                    handoverSplit = double.Parse(dr["handoverSplit"].ToString());
                } else {
                    reminderFee = reminder_fee;
                    reminderSplit = 0;
                    finalFee = final_fee;
                    finalSplit = 0;
                    disconnectionNoticefee = discon_notice_fee;
                    disconnectionNoticeSplit = 0;
                    summonsFee = summons_fee;
                    summonsSplit = 0;
                    disconnectionFee = discon_fee;
                    disconnectionSplit = 0;
                    handoverFee = handover_fee;
                    handoverSplit = 0;
                }
            }
        }

        #region Constructor

        public usrLetters() {
            InitializeComponent();
            customerList = new List<CustomerList>();
            List<Building> allBuildings = new Buildings(false).buildings;
            buildings = new List<Building>();
            foreach (int bid in Controller.user.buildings) {
                foreach (Building b in allBuildings) {
                    if (bid == b.ID && !buildings.Contains(b)) {
                        buildings.Add(b);
                        break;
                    }
                }
            }
            buildings.Sort(new BuildingComparer("Name", SortOrder.Ascending));
            LoadDefaultValues();
        }

        private void usrLetters_Load(object sender, EventArgs e) {
            cmbBuildings.SelectedIndexChanged -= cmbBuildings_SelectedIndexChanged;
            cmbBuildings.DataSource = buildings;
            cmbBuildings.DisplayMember = "Name";
            cmbBuildings.ValueMember = "ID";
            cmbBuildings.SelectedIndex = -1;
            cmbBuildings.SelectedIndexChanged += cmbBuildings_SelectedIndexChanged;
        }

        private void usrLetters_Leave(object sender, EventArgs e) {
            if (wp != null) { wp.killprocess("winword"); }
        }

        #endregion Constructor

        private void LoadDefaultValues() {
            String settingsQuery = "SELECT minbal, reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee, centrec, business FROM tblSettings";
            String status;
            DataSet dsDefault = dh.GetData(settingsQuery, null, out status);
            if (dsDefault != null && dsDefault.Tables.Count > 0 && dsDefault.Tables[0].Rows.Count > 0) {
                DataRow dr = dsDefault.Tables[0].Rows[0];
                minbal = double.Parse(dr["minbal"].ToString());
                reminder_fee = double.Parse(dr["reminder_fee"].ToString());
                final_fee = double.Parse(dr["final_fee"].ToString());
                summons_fee = double.Parse(dr["summons_fee"].ToString());
                discon_notice_fee = double.Parse(dr["discon_notice_fee"].ToString());
                discon_fee = double.Parse(dr["discon_fee"].ToString());
                handover_fee = double.Parse(dr["handover_fee"].ToString());
                centrec = dr["centrec"].ToString();
                business = dr["business"].ToString();
            }
        }

        private void cmbBuildings_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                building = buildings[cmbBuildings.SelectedIndex];
                category = String.Empty;
                customerGrid.DataSource = null;
                getDebtorEmail(building.Name);
            } catch {
                building = null;
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                String[] splitter = new string[] { "-" };
                category = cmbCategory.SelectedItem.ToString().Split(splitter, StringSplitOptions.None)[0];
                GetCustomers();
            } catch { }
        }

        private void GetCustomers() {
            if (building != null) {
                this.Cursor = Cursors.WaitCursor;
                cls = new List<CustomerList>();
                customerList.Clear();
                int buildPeriod;
                int trustPeriod = Utilities.getPeriod(DateTime.Now, building.Period, out buildPeriod);
                List<Customer> customers = new List<Customer>();
                customers = getCatCustomers(category, buildPeriod);
                if (customers.Count > 0) { cls.Add(new CustomerList("", "", 0)); }
                customerDic = new Dictionary<string, Customer>();
                foreach (Customer customer in customers) {
                    CustomerList cl = new CustomerList(customer.accNumber, customer.description, customer.ageing[0]);
                    cls.Add(cl);
                    customerDic.Add(customer.accNumber, customer);
                }
                customerGrid.DataSource = cls;
                disableCells(false);
                this.Cursor = Cursors.Arrow;
            } else {
                building = null;
            }
        }

        private void disableCells(bool disable) {
            try {
                customerGrid.Columns[11].Visible = false;
                customerGrid.Columns[12].Visible = false;
                //0,1,10
                customerGrid.Columns[2].Visible = !disable;
                customerGrid.Columns[3].Visible = !disable;
                customerGrid.Columns[4].Visible = !disable;
                customerGrid.Columns[5].Visible = !disable;
                customerGrid.Columns[6].Visible = !disable;
                customerGrid.Columns[7].Visible = !disable;
                customerGrid.Columns[8].Visible = disable;
                customerGrid.Columns[9].Visible = disable;

                customerGrid.Columns[5].DisplayIndex = 5;
                customerGrid.Columns[4].DisplayIndex = 6;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Customer> getCatCustomers(String category, int BuildingPeriod) {
            List<Customer> catCustomers = Controller.pastel.AddCustomers(building.Name, building.DataPath);
            List<Customer> _Customers = new List<Customer>();
            foreach (Customer _customer in catCustomers) {
                if (_customer.category == category || building.Abbr == "RENT") {
                    double totBal = 0;
                    for (int li = 0; li < _customer.lastBal.Length; li++) { totBal += _customer.lastBal[li]; }
                    for (int i = 0; i < BuildingPeriod; i++) { totBal += _customer.balance[i]; }
                    _customer.setAgeing(Math.Round(totBal, 2), 0);
                    if (_customer.ageing[0] >= minbal) { _Customers.Add(_customer); }
                }
            }
            return _Customers;
        }

        private void customerGrid_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            customerGrid.CurrentCell = customerGrid.Rows[customerGrid.CurrentRow.Index].Cells[0];
        }

        private void customerGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == 0) {
                DataGridViewRow gvr = customerGrid.Rows[e.RowIndex];
                int colNo = 0;
                if ((bool)gvr.Cells[2].Value == true) {
                    colNo = 2;
                } else if ((bool)gvr.Cells[3].Value == true) {
                    colNo = 3;
                } else if ((bool)gvr.Cells[4].Value == true) {
                    colNo = 4;
                } else if ((bool)gvr.Cells[5].Value == true) {
                    colNo = 5;
                } else if ((bool)gvr.Cells[6].Value == true) {
                    colNo = 6;
                } else if ((bool)gvr.Cells[7].Value == true) {
                    colNo = 7;
                } else if ((bool)gvr.Cells[8].Value == true) {
                    colNo = 8;
                } else if ((bool)gvr.Cells[9].Value == true) {
                    colNo = 9;
                } else if ((bool)gvr.Cells[13].Value == true) {
                    colNo = 13;
                } else if ((bool)gvr.Cells[15].Value == true) {
                    colNo = 15;
                }
                for (int i = 0; i < customerGrid.Rows.Count; i++) {
                    if (colNo != 13 || colNo != 15) {
                        for (int j = 2; j < 10; j++) { customerGrid.Rows[i].Cells[j].Value = false; }
                        if (colNo == 13) { customerGrid.Rows[i].Cells[15].Value = false; }
                        if (colNo == 15) { customerGrid.Rows[i].Cells[13].Value = false; }
                    }

                    if (colNo > 0) { customerGrid.Rows[i].Cells[colNo].Value = true; }
                }
            }
        }

        private void customerGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            //String msg = "";
            //for (int i = 0; i < customerGrid.Columns.Count; i++) {
            //    msg += i.ToString() + " - " + customerGrid.Columns[i].HeaderText + "; ";
            //}
            //MessageBox.Show(msg);
        }

        private void btnPrint_Click(object sender, EventArgs e) {
            LoadPrintGrid();
        }

        private void LoadPrintGrid() {
            //2-9
            BindingSource bsPrint = new BindingSource();
            DataGridView dgPrint = new DataGridView();
            foreach (DataGridViewRow dr in customerGrid.Rows) {
                for (int i = 2; i <= 9; i++) {
                    if ((bool)dr.Cells[i].Value == true) {
                        CustomerList cl = dr.DataBoundItem as CustomerList;
                        bsPrint.Add(cl);
                    }
                }
            }
            dgPrint.DataSource = bsPrint;
            this.Controls.Add(dgPrint);
            dgPrint.Size = customerGrid.Size;
            dgPrint.Visible = true;
            PrintDGV.Print_DataGridView(dgPrint);
            dgPrint.Visible = false;
            this.Controls.Remove(dgPrint);
        }

        private void btnProcess_Click(object sender, EventArgs e) {
            messages = new List<MessageConstruct>();
            List<String> reminders = new List<String>();
            List<String> finals = new List<String>();
            List<String> disconsL = new List<String>();
            List<String> discons = new List<String>();
            List<String> summons = new List<String>();
            List<String> handovers = new List<String>();
            List<String> clearances = new List<string>();
            List<String> exClearances = new List<string>();
            List<String> rentals = new List<string>();
            Dictionary<String, String> journals = new Dictionary<string, string>();

            for (int i = 1; i < customerGrid.Rows.Count; i++) {
                DataGridViewRow gvr = customerGrid.Rows[i];
                String accNumber = gvr.Cells[0].Value.ToString();
                int colNo = 0;
                if ((bool)gvr.Cells[2].Value == true) {
                    colNo = 1;
                    reminders.Add(accNumber);
                } else if ((bool)gvr.Cells[3].Value == true) {
                    colNo = 2;
                    finals.Add(accNumber);
                } else if ((bool)gvr.Cells[5].Value == true) {
                    colNo = 5;
                    summons.Add(accNumber);
                } else if ((bool)gvr.Cells[4].Value == true) {
                    colNo = 4;
                    disconsL.Add(accNumber);
                } else if ((bool)gvr.Cells[6].Value == true) {
                    colNo = 6;
                    discons.Add(accNumber);
                } else if ((bool)gvr.Cells[7].Value == true) {
                    colNo = 7;
                    handovers.Add(accNumber);
                } else if ((bool)gvr.Cells[8].Value == true) {
                    clearances.Add(accNumber);
                } else if ((bool)gvr.Cells[9].Value == true) {
                    exClearances.Add(accNumber);
                } else if ((bool)gvr.Cells[15].Value == true) {
                    rentals.Add(accNumber);
                } else {
                    continue;
                }
                for (int gi = 2; gi < 10; gi++) {
                    gvr.Cells[gi].Value = false;
                }
            }
            if (reminders.Count > 0) { GenerateLetter(1, reminders); }
            if (finals.Count > 0) { GenerateLetter(2, finals); }
            if (disconsL.Count > 0) { GenerateLetter(3, disconsL); }
            if (discons.Count > 0) { DisconnectCustomers(discons); }
            if (summons.Count > 0) { GenerateLetter(5, summons); }
            if (handovers.Count > 0) { HandOverCustomers(handovers); }
            if (rentals.Count > 0) { GenerateLetter(6, rentals); }
            String status;
            if (Controller.user.id != 1) {
                SendSMS(messages, out status);
                runJournals();
            }
            UpdateLetterReport();
            MessageBox.Show("Process Complete");
        }

        private void UpdateLetterReport() {
            String query = "INSERT INTO tblDebtors(buildingID, completeDate, lettersprintemail) VALUES(" + building.ID.ToString() + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'True')";
            String status;
            dh.SetData(query, null, out status);
        }

        private void GenerateLetter(int docIdx, List<String> customerAccs) {
            wp = new WordProcessor();
            double amt = 0;
            String docType = "";

            #region Get Doc Type

            switch (docIdx) {
                case 1:
                    docType = "Reminder Letter";
                    break;

                case 2:
                    docType = "Final Demand Letter";
                    break;

                case 3:
                    docType = "Disconnect Notice";
                    break;

                case 5:
                    docType = "Summons Pending Letter";
                    break;

                case 6:
                    docType = "Late Payment Letter";
                    break;
            }

            #endregion Get Doc Type

            DateTime disconDate = DateTime.Now.AddDays(3);
            if (docType == "Disconnect Notice") { disconDate = dateTimePicker1.Value; }
            String fileName = "";
            DateTime letterDate = DateTime.Now;
            List<Customer> checkedCustomers = new List<Customer>();
            foreach (String acc in customerAccs) { checkedCustomers.Add(customerDic[acc]); }
            BuildingValues values = new BuildingValues(building.ID, reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee);
            double regPost = (building.Abbr == "LR" ? 23.65 : 0);
            String trustAcc = "";
            String pastelString = "";
            String pastelReturn = "";

            PDF pdf = new PDF();
            String uFax, uName, uPhone, uEmail;
            if (debtor == null) {
                uFax = Controller.user.fax;
                uName = Controller.user.name;
                uPhone = Controller.user.phone;
                uEmail = Controller.user.email;
            } else {
                uFax = debtor.fax;
                uName = debtor.name;
                uPhone = debtor.phone;
                uEmail = debtor.email;
            }
            MySqlConnector mysqlConn = new MySqlConnector();
            Classes.Sftp ftpClient = new Classes.Sftp(String.Empty, true);
            ftpClient.ConnectClient(false);

            foreach (Customer c in checkedCustomers) {
                bool created = false;
                bool docStatement = (c.statPrintorEmail == 2 ? false : true);
                switch (docType) {
                    case "Reminder Letter":

                        c.setAgeing(c.ageing[0] + values.reminderFee, 0);
                        if (building.Abbr == "SVT") {
                            created = pdf.CreateSVTReminderLetter(c.accNumber, DateTime.Now.ToString("yyyy/MM/dd"), c.description, c.address, c.ageing[0].ToString("#,##0.00"), values.reminderFee.ToString("#, ##0.00"), uFax, out fileName);
                        } else if (building.Abbr == "WBG") {
                            created = pdf.CreateWBGReminderLetter(c.accNumber, DateTime.Now.ToString("yyyy/MM/dd"), c.description, c.address, c.ageing[0].ToString("#,##0.00"), values.reminderFee.ToString("#, ##0.00"), uFax, out fileName);
                        } else {
                            created = pdf.CreateReminderLetter(c.accNumber, DateTime.Now.ToString("yyyy/MM/dd"), c.description, c.address, c.ageing[0].ToString("#,##0.00"), values.reminderFee.ToString("#, ##0.00"), uFax, uName, uPhone, building.isHOA, out fileName);
                        }
                        amt = values.reminderFee;
                        trustAcc = "1100/000";
                        foreach (CustomerList cl in cls) {
                            if ((cl.AccNumber == c.accNumber) && (!String.IsNullOrEmpty(c.CellPhone)) && cl.SMS) {
                                AddSMS(c, 1);
                                break;
                            }
                        }
                        break;

                    case "Final Demand Letter":
                        created = true;
                        c.setAgeing(c.ageing[0] + values.finalFee, 0);
                        fileName = wp.FinalGen(c, letterDate, values.finalFee, uName, uPhone, uFax, docStatement, building.Abbr, building.isHOA);
                        amt = values.finalFee;
                        trustAcc = "1100/000";
                        foreach (CustomerList cl in cls) {
                            if ((cl.AccNumber == c.accNumber) && (!String.IsNullOrEmpty(c.CellPhone)) && cl.SMS) {
                                AddSMS(c, 2);
                                break;
                            }
                        }
                        break;

                    case "Late Payment Letter":
                        created = true;
                        c.setAgeing(c.ageing[0] + values.finalFee, 0);
                        fileName = wp.LPPGen(c, letterDate, 297, uName, uPhone, uFax, docStatement, building.Abbr);
                        amt = 297;
                        trustAcc = "1100/000";
                        foreach (CustomerList cl in cls) {
                            if ((cl.AccNumber == c.accNumber) && (!String.IsNullOrEmpty(c.CellPhone)) && cl.SMS) {
                                AddSMS(c, 2);
                                break;
                            }
                        }
                        break;

                    case "Disconnect Notice":
                        created = true;
                        c.setAgeing(c.ageing[0] + values.disconnectionNoticefee, 0);
                        fileName = wp.disconGen(c, letterDate, disconDate, values.disconnectionFee, values.disconnectionNoticefee, uName, uPhone, uFax, docStatement, building.Abbr);
                        amt = values.disconnectionNoticefee;
                        trustAcc = "1100/000";
                        foreach (CustomerList cl in cls) {
                            if ((cl.AccNumber == c.accNumber) && (!String.IsNullOrEmpty(c.CellPhone)) && cl.SMS) {
                                AddSMS(c, 3);
                                break;
                            }
                        }
                        break;

                    case "Summons Pending Letter":
                        created = true;
                        c.setAgeing(c.ageing[0] + values.summonsFee, 0);
                        fileName = wp.SummonsGen(c, letterDate, values.summonsFee, uName, uPhone, uFax, docStatement, building.Abbr, building.isHOA);
                        amt = values.summonsFee;
                        trustAcc = "1100/000";
                        foreach (CustomerList cl in cls) {
                            if ((cl.AccNumber == c.accNumber) && (!String.IsNullOrEmpty(c.CellPhone)) && cl.SMS) {
                                AddSMS(c, 4);
                                break;
                            }
                        }
                        break;
                }
                if (Controller.user.id == 1) {
                    MessageBox.Show(c.statPrintorEmail.ToString() + " - " + created.ToString());
                }

                //#region Upload Letter
                //String actFileTitle = Path.GetFileNameWithoutExtension(fileName);
                //String actFile = Path.GetFileName(fileName);
                //try {
                //    mysqlConn.InsertStatement(actFileTitle, "Customer Statements", actFile, c.accNumber, c.Email);
                //    ftpClient.Upload(fileName, actFile);
                //} catch { }
                //#endregion

                if ((c.statPrintorEmail <= 3) && created) {
                    String msgStatus = String.Empty;
                    //if (Controller.user.id != 1) {
                    String mailBody = "Dear Owner" + Environment.NewLine + Environment.NewLine;
                    mailBody += "Attached please find " + docType + " for your attention." + Environment.NewLine + Environment.NewLine;
                    mailBody += "Account #: " + c.accNumber + ". For any queries on your account, please contact " + uName + " on email: " + uEmail + ", tel: " + uPhone + "." + Environment.NewLine + Environment.NewLine;
                    mailBody += "Do not reply to this e-mail address" + Environment.NewLine + Environment.NewLine;
                    mailBody += "Regards" + Environment.NewLine;
                    mailBody += "Astrodon (Pty) Ltd" + Environment.NewLine;
                    mailBody += "You're in good hands";
                    try {
                        bool sentMail = dh.InsertLetter(uEmail, c.Email, docType + ": " + c.accNumber + " " + DateTime.Now.ToString(), mailBody, false, true, true, new String[] { Path.GetFileName(fileName) }, c.accNumber, out msgStatus);
                    } catch {
                        MessageBox.Show(uEmail + " - " + c.Email + " - " + docType + " - " + c.accNumber + " - " + DateTime.Now.ToString() + " - " + mailBody + " - " + Path.GetFileName(fileName) + " - " + c.accNumber);
                    }
                    if (Controller.user.id == 1) { MessageBox.Show(msgStatus); }
                    //}
                    if ((c.statPrintorEmail == 1 || c.statPrintorEmail == 3) || docType == "Disconnect Notice") {
                        try {
                            SendToPrinter(fileName);
                        } catch (Exception ex) {
                        }
                    }
                } else if (docType == "Disconnect Notice") {
                    SendToPrinter(fileName);
                }
                if (Controller.user.id != 1) {
                    pastelReturn = Controller.pastel.PostBatch(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, amt.ToString("#0.00"), trustAcc, "", out pastelString);
                    if (regPost != 0) { pastelReturn = Controller.pastel.PostBatch(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, "Registered Postage", "Registered Postage", regPost.ToString("#0.00"), "1090/000", "", out pastelString); }
                    if (values.reminderSplit != 0 && docIdx == 1) {
                        pastelReturn = Controller.pastel.PostCredit(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.reminderSplit.ToString("#0.00"), trustAcc, "", out pastelString);
                        pastelReturn = Controller.pastel.PostBatch(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, "9250/000", docType, docType, values.reminderSplit.ToString("#0.00"), trustAcc, "2900000", out pastelString);
                    } else if (values.finalSplit != 0 && docIdx == 2) {
                        pastelReturn = Controller.pastel.PostCredit(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.finalSplit.ToString("#0.00"), trustAcc, "", out pastelString);
                        pastelReturn = Controller.pastel.PostBatch(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, "9250/000", docType, docType, values.finalSplit.ToString("#0.00"), trustAcc, "2900000", out pastelString);
                    } else if (values.disconnectionNoticeSplit != 0 && docIdx == 3) {
                        pastelReturn = Controller.pastel.PostCredit(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.disconnectionNoticeSplit.ToString("#0.00"), trustAcc, "", out pastelString);
                        pastelReturn = Controller.pastel.PostBatch(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, "9250/000", docType, docType, values.disconnectionNoticeSplit.ToString("#0.00"), trustAcc, "2900000", out pastelString);
                    } else if (values.summonsSplit != 0 && docIdx == 5) {
                        pastelReturn = Controller.pastel.PostCredit(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.summonsSplit.ToString("#0.00"), trustAcc, "", out pastelString);
                        pastelReturn = Controller.pastel.PostBatch(letterDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, "9250/000", docType, docType, values.summonsSplit.ToString("#0.00"), trustAcc, "2900000", out pastelString);
                    }
                }
            }
            if (wp != null) {
                wp.killprocess("winword");
                wp = null;
            }
        }

        private void DisconnectCustomers(List<String> customers) {
            String docType = "Disconnect / Reconnect";
            DateTime trnDate = DateTime.Now;
            List<Customer> checkedCustomers = new List<Customer>();
            foreach (String acc in customers) { checkedCustomers.Add(customerDic[acc]); }
            BuildingValues values = new BuildingValues(building.ID, reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee);
            List<String> excludedBuildings = new List<string>();
            excludedBuildings.Add("LR");
            excludedBuildings.Add("TSM");
            foreach (Customer c in checkedCustomers) {
                AddSMS(c, 5);
                String pastelReturn, pastelString;
                if (!excludedBuildings.Contains(building.Abbr)) {
                    pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.disconnectionFee.ToString("#0.00"), "1105/000", "", out pastelString);
                } else {
                    if (building.Abbr == "LR") {
                        Controller.pastel.PostBuildBatch(trnDate, building.DataPath, building.Journal, building.Period, c.accNumber, "2901000", c.accNumber, docType, values.disconnectionFee.ToString("#0.00"), out pastelString);
                    } else if (building.Abbr == "TSM") {
                        Controller.pastel.PostBuildBatch(trnDate, building.DataPath, building.Journal, building.Period, c.accNumber, "2910000", c.accNumber, docType, values.disconnectionFee.ToString("#0.00"), out pastelString);
                    }
                }
                if (values.disconnectionSplit != 0) {
                    pastelReturn = Controller.pastel.PostCredit(trnDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.disconnectionSplit.ToString("#0.00"), "1105/000", "", out pastelString);
                    pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, "9250/000", docType, docType, values.disconnectionSplit.ToString("#0.00"), "", "2900000", out pastelString);
                }
                c.category = "04";
                String customerString = c.GetCustomer();
                Controller.pastel.UpdateCustomer(customerString, building.DataPath);
            }
        }

        private void HandOverCustomers(List<String> customers) {
            String docType = "Handover";
            BuildingValues values = new BuildingValues(building.ID, reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee);
            DateTime trnDate = DateTime.Now;
            List<Customer> checkedCustomers = new List<Customer>();
            foreach (String acc in customers) { checkedCustomers.Add(customerDic[acc]); }
            String pastelReturn, pastelString;
            foreach (Customer c in checkedCustomers) {
                AddSMS(c, 6);
                pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.handoverFee.ToString("#0.00"), "1110/000", "", out pastelString);
                if (values.disconnectionSplit != 0) {
                    pastelReturn = Controller.pastel.PostCredit(trnDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, building.Centrec_Building, docType, docType, values.handoverSplit.ToString("#0.00"), "1105/000", "", out pastelString);
                    pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, c.accNumber, building.Centrec_Building, "9250/000", docType, docType, values.handoverSplit.ToString("#0.00"), "", "2900000", out pastelString);
                }
                c.category = "05";
                String customerString = c.GetCustomer();
                Controller.pastel.UpdateCustomer(customerString, building.DataPath);
            }
        }

        public void SendSMS(List<MessageConstruct> msgs, out String status) {
            status = "";
            SMS sms = new SMS();
            foreach (MessageConstruct msg in msgs) {
                SMSMessage smsMsg = new SMSMessage();
                smsMsg.astStatus = msg.astStatus;
                smsMsg.batchID = msg.batchID;
                smsMsg.billable = msg.billable;
                smsMsg.building = msg.building;
                smsMsg.bulkbillable = msg.bulkbillable;
                smsMsg.cbal = msg.os;
                smsMsg.customer = msg.customer;
                smsMsg.message = msg.text;
                smsMsg.number = msg.number;
                smsMsg.pollCount = msg.pollCount;
                smsMsg.reference = msg.reference;
                smsMsg.sender = msg.sender;
                smsMsg.sent = msg.sent;
                smsMsg.smsType = msg.msgType;
                smsMsg.status = msg.status;
                bool immediate = (smsMsg.smsType == "Disconnection SMS");
                bool success = sms.SendMessage(smsMsg, immediate, out status);
            }
        }

        private void runJournals() {
            String pastelString = "";
            int completedJournals = cls.Count;
            foreach (CustomerList cl in cls) {
                if (cl.Journal && cl.JournalAmt != "" && cl.JournalAcc != "") {
                    String pastelReturn = Controller.pastel.PostBatch(DateTime.Now, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, cl.AccNumber, building.Centrec_Building, cl.JournalAcc, cl.AccNumber, cl.AccNumber, cl.JournalAmt, cl.AccNumber, "", out pastelString);
                }
            }
        }

        private void AddSMS(Customer c, int docType) {
            MessageConstruct m = new MessageConstruct();
            m.id = 0;
            m.building = building.Abbr;
            m.customer = c.accNumber;
            m.number = c.CellPhone;
            m.reference = string.Empty;
            m.os = c.ageing[0];
            switch (docType) {
                case 1:
                    m.text = "Astrodon: reminder - outstanding levies R" + c.ageing[0].ToString("#,##0.00");
                    m.msgType = "Reminder SMS";
                    break;

                case 2:
                    m.text = "Astrodon: final demand - levies overdue R" + c.ageing[0].ToString("#,##0.00");
                    m.msgType = "Final Demand SMS";
                    break;

                case 3:
                    m.text = "Astrodon: disconnection notice served for outstanding levies R" + c.ageing[0].ToString("#,##0.00");
                    m.msgType = "Disconnection Notice SMS";
                    break;

                case 4:
                    m.text = "Astrodon: summons pending letter issued for outstanding levies R" + c.ageing[0].ToString("#,##0.00");
                    m.msgType = "Summons Pending SMS";
                    break;

                case 5:
                    m.text = "Astrodon: confirmation of disconnection due to non payment as per notice - R" + c.ageing[0].ToString("#,##0.00");
                    m.msgType = "Disconnection SMS";
                    break;

                case 6:
                    m.text = "Astrodon: confirmation of legal hand over to appointed attorneys due to non payment of R" + c.ageing[0].ToString("#,##0.00");
                    m.msgType = "Legal Handover SMS";
                    break;
            }
            m.sent = DateTime.Now;
            m.sender = Controller.user.id.ToString();
            m.billable = true;
            m.bulkbillable = false;
            m.astStatus = "1";
            m.batchID = "";
            m.status = "-1";
            m.nextPolled = DateTime.Now.AddMinutes(5);
            m.pollCount = 0;
            messages.Add(m);
        }

        private void getDebtorEmail(String buildingName) {
            List<Building> testBuildings = new Buildings(false).buildings;
            foreach (Building b in testBuildings) {
                if (b.Name == buildingName) {
                    debtor = new Users().GetUserBuild(b.ID);
                    break;
                }
            }
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        private void SendToPrinter(String fileName) {
            try {
                ProcessStartInfo info = new ProcessStartInfo();
                info.Verb = "print";
                info.FileName = fileName;
                //info.CreateNoWindow = true;
                //info.WindowStyle = ProcessWindowStyle.Hidden;

                Process p = new Process();
                p.StartInfo = info;
                p.Start();

                //p.WaitForInputIdle();
                System.Threading.Thread.Sleep(3000);
                //if (false == p.CloseMainWindow()) { p.Kill(); }
            } catch {
                MessageBox.Show("Cannot print document.  Please print from the folder");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Astrodon
{

    public class Buildings
    {

        #region Variables

        public List<Building> buildings;
        private String status = String.Empty;
        private SqlDataHandler dh = new SqlDataHandler();

        #endregion Variables

        #region Queries

        private String buildQuery = "SELECT id, Building, Code, AccNumber, DataPath, Period, Contra, ownbank, cashbook3, payments, receipts, journals, bc, centrec, business, bank, pm, bankName, accName, bankAccNumber, branch, isBuilding,addy1, addy2, addy3, addy4, addy5, web, letterName, pid, hoa FROM tblBuildings ORDER BY Building";
        private String feeQuery = "SELECT reminderFee, reminderSplit, finalFee, finalSplit, disconnectionNoticefee, disconnectionNoticeSplit, summonsFee, summonsSplit, disconnectionFee, disconnectionSplit, handoverFee, handoverSplit, reminderTemplate, finalTemplate, diconnectionNoticeTemplate, summonsTemplate, reminderSMS, finalSMS, disconnectionNoticeSMS, summonsSMS, disconnectionSMS, handoverSMS FROM tblBuildingSettings WHERE (buildingID = @buildID)";
        private String buildUserQuery = "SELECT b.id, b.Building, b.Code, b.AccNumber, b.DataPath, b.Period, b.Contra, b.ownbank, b.cashbook3, b.payments, b.receipts, b.journals, b.bc, b.centrec, b.business, b.bank, b.pm, b.bankName, b.accName, b.bankAccNumber, b.branch, b.isBuilding, b.addy1, b.addy2, b.addy3, b.addy4, b.addy5, b.web, b.letterName, b.pid,b.hoa FROM tblBuildings b INNER JOIN tblUserBuildings u ON b.id = u.buildingid WHERE u.userid = @userid ORDER BY b.Building";

        #endregion Queries

        private void LoadBuildings(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Building b = new Building();
                    b.ID = int.Parse(dr["id"].ToString());
                    b.Name = dr["Building"].ToString();
                    b.Abbr = dr["Code"].ToString();
                    b.Trust = dr["AccNumber"].ToString();
                    b.DataPath = dr["DataPath"].ToString();
                    b.Period = int.Parse(dr["Period"].ToString());
                    b.Cash_Book = dr["Contra"].ToString();
                    b.OwnBank = dr["ownbank"].ToString();
                    b.Cashbook3 = dr["cashbook3"].ToString();
                    b.Payments = int.Parse(dr["payments"].ToString());
                    b.Receipts = int.Parse(dr["receipts"].ToString());
                    b.Journal = int.Parse(dr["journals"].ToString());
                    b.Centrec_Account = dr["bc"].ToString();
                    b.Centrec_Building = dr["centrec"].ToString();
                    b.Business_Account = dr["business"].ToString();
                    b.Bank = dr["bank"].ToString();
                    b.PM = dr["pm"].ToString();
                    b.Debtor = getDebtorEmail(b.ID);
                    b.Bank_Name = dr["bankName"].ToString();
                    b.Acc_Name = dr["accName"].ToString();
                    b.Bank_Acc_Number = dr["bankAccNumber"].ToString();
                    b.Branch_Code = dr["branch"].ToString();
                    b.Web_Building = bool.Parse(dr["isBuilding"].ToString());
                    b.webFolder = dr["web"].ToString();
                    b.letterName = dr["letterName"].ToString();
                    b.addy1 = dr["addy1"].ToString();
                    b.addy2 = dr["addy2"].ToString();
                    b.addy3 = dr["addy3"].ToString();
                    b.addy4 = dr["addy4"].ToString();
                    b.addy5 = dr["addy5"].ToString();
                    b.pid = dr["pid"].ToString();
                    b.isHOA = bool.Parse(dr["hoa"].ToString());
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@buildID", b.ID);
                    DataSet dsFee = dh.GetData(feeQuery, sqlParms, out status);
                    if (dsFee != null && dsFee.Tables.Count > 0 && dsFee.Tables[0].Rows.Count > 0)
                    {
                        DataRow dFee = dsFee.Tables[0].Rows[0];
                        b.reminderFee = double.Parse(dFee["reminderFee"].ToString());
                        b.reminderSplit = double.Parse(dFee["reminderSplit"].ToString());
                        b.finalFee = double.Parse(dFee["finalFee"].ToString());
                        b.finalSplit = double.Parse(dFee["finalSplit"].ToString());
                        b.disconnectionNoticefee = double.Parse(dFee["disconnectionNoticefee"].ToString());
                        b.disconnectionNoticeSplit = double.Parse(dFee["disconnectionNoticeSplit"].ToString());
                        b.summonsFee = double.Parse(dFee["summonsFee"].ToString());
                        b.summonsSplit = double.Parse(dFee["summonsSplit"].ToString());
                        b.disconnectionFee = double.Parse(dFee["disconnectionFee"].ToString());
                        b.disconnectionSplit = double.Parse(dFee["disconnectionSplit"].ToString());

                        b.handoverFee = double.Parse(dFee["handoverFee"].ToString());
                        b.handoverSplit = double.Parse(dFee["handoverSplit"].ToString());
                        b.reminderTemplate = dFee["reminderTemplate"].ToString();
                        b.finalTemplate = dFee["finalTemplate"].ToString();
                        b.diconnectionNoticeTemplate = dFee["diconnectionNoticeTemplate"].ToString();
                        b.summonsTemplate = dFee["summonsTemplate"].ToString();
                        b.reminderSMS = dFee["reminderSMS"].ToString();
                        b.finalSMS = dFee["finalSMS"].ToString();
                        b.disconnectionNoticeSMS = dFee["disconnectionNoticeSMS"].ToString();
                        b.summonsSMS = dFee["summonsSMS"].ToString();
                        b.disconnectionSMS = dFee["disconnectionSMS"].ToString();
                        b.handoverSMS = dFee["handoverSMS"].ToString();
                    }
                    else
                    {
                        b.reminderFee = 0;
                        b.reminderSplit = 0;
                        b.finalFee = 0;
                        b.finalSplit = 0;
                        b.disconnectionNoticefee = 0;
                        b.disconnectionNoticeSplit = 0;
                        b.summonsFee = 0;
                        b.summonsSplit = 0;
                        b.disconnectionFee = 0;
                        b.disconnectionSplit = 0;
                        b.handoverFee = 0;
                        b.handoverSplit = 0;

                        b.reminderTemplate = "";
                        b.finalTemplate = "";
                        b.diconnectionNoticeTemplate = "";
                        b.summonsTemplate = "";
                        b.reminderSMS = "";
                        b.finalSMS = "";
                        b.disconnectionNoticeSMS = "";
                        b.summonsSMS = "";
                        b.disconnectionSMS = "";
                        b.handoverSMS = "";
                    }

                    buildings.Add(b);
                }
            }
        }

        public Buildings(bool addNew)
        {
            buildings = new List<Building>();

            if (addNew)
            {
                Building b = new Building();
                b.ID = 0;
                b.Name = "Add new building";
                buildings.Add(b);
            }
            LoadBuildings(dh.GetData(buildQuery, null, out status));
        }

        public Buildings(bool addNew, String nameValue)
        {
            buildings = new List<Building>();
            if (addNew)
            {
                Building b = new Building();
                b.ID = 0;
                b.Name = nameValue;
                buildings.Add(b);
            }
            LoadBuildings(dh.GetData(buildQuery, null, out status));
        }

        public Buildings(int userID)
        {
            buildings = new List<Building>();
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@userid", userID);
            LoadBuildings(dh.GetData(buildUserQuery, sqlParms, out status));
        }

        public Building GetBuilding(int id)
        {
            String buildQuery = "SELECT id, Building, Code, AccNumber, DataPath, Period, Contra, ownbank, cashbook3, payments, receipts, journals, bc, centrec, business, bank, pm, bankName, accName, bankAccNumber, ";
            buildQuery += " branch, isBuilding,addy1, addy2, addy3, addy4, addy5, web, letterName, pid FROM tblBuildings ORDER BY Building";
            Building b = new Building();
            dh = new SqlDataHandler();
            DataSet ds = dh.GetData(buildQuery, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    b.ID = int.Parse(dr["id"].ToString());
                    b.Name = dr["Building"].ToString();
                    b.Abbr = dr["Code"].ToString();
                    b.Trust = dr["AccNumber"].ToString();
                    b.DataPath = dr["DataPath"].ToString();
                    b.Period = int.Parse(dr["Period"].ToString());
                    b.Cash_Book = dr["Contra"].ToString();
                    b.OwnBank = dr["ownbank"].ToString();
                    b.Cashbook3 = dr["cashbook3"].ToString();
                    b.Payments = int.Parse(dr["payments"].ToString());
                    b.Receipts = int.Parse(dr["receipts"].ToString());
                    b.Journal = int.Parse(dr["journals"].ToString());
                    b.Centrec_Account = dr["bc"].ToString();
                    b.Centrec_Building = dr["centrec"].ToString();
                    b.Business_Account = dr["business"].ToString();
                    b.Bank = dr["bank"].ToString();
                    b.PM = dr["pm"].ToString();
                    b.Debtor = getDebtorEmail(b.ID);
                    b.Bank_Name = dr["bankName"].ToString();
                    b.Acc_Name = dr["accName"].ToString();
                    b.Bank_Acc_Number = dr["bankAccNumber"].ToString();
                    b.Branch_Code = dr["branch"].ToString();
                    b.Web_Building = bool.Parse(dr["isBuilding"].ToString());
                    b.webFolder = dr["web"].ToString();
                    b.letterName = dr["letterName"].ToString();
                    b.addy1 = dr["addy1"].ToString();
                    b.addy2 = dr["addy2"].ToString();
                    b.addy3 = dr["addy3"].ToString();
                    b.addy4 = dr["addy4"].ToString();
                    b.addy5 = dr["addy5"].ToString();
                    b.pid = dr["pid"].ToString();
                }
            }
            return b;
        }

        public bool Update(int idx, bool remove, out String status)
        {
            dh = new SqlDataHandler();
            String updateQuery = String.Empty;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@Name", buildings[idx].Name);
            sqlParms.Add("@Abbr", buildings[idx].Abbr);
            sqlParms.Add("@Trust", buildings[idx].Trust);
            sqlParms.Add("@DataPath", buildings[idx].DataPath);
            sqlParms.Add("@Period", buildings[idx].Period);
            sqlParms.Add("@Cash", buildings[idx].Cash_Book);
            sqlParms.Add("@ownbank", buildings[idx].OwnBank);
            sqlParms.Add("@cashbook3", buildings[idx].Cashbook3);
            sqlParms.Add("@Payments", buildings[idx].Payments);
            sqlParms.Add("@Receipts", buildings[idx].Receipts);
            sqlParms.Add("@Journal", buildings[idx].Journal);
            sqlParms.Add("@centrec", buildings[idx].Centrec_Account);
            sqlParms.Add("@cbuild", buildings[idx].Centrec_Building);
            sqlParms.Add("@business", buildings[idx].Business_Account);
            sqlParms.Add("@Bank", buildings[idx].Bank);
            sqlParms.Add("@PM", buildings[idx].PM);
            sqlParms.Add("@BankName", buildings[idx].Bank_Name);
            sqlParms.Add("@AccName", buildings[idx].Acc_Name);
            sqlParms.Add("@BankAccNumber", buildings[idx].Bank_Acc_Number);
            sqlParms.Add("@Branch", buildings[idx].Branch_Code);
            sqlParms.Add("@web", buildings[idx].Web_Building);
            sqlParms.Add("@ln", buildings[idx].letterName);
            sqlParms.Add("@ID", buildings[idx].ID);

            sqlParms.Add("@rf", buildings[idx].reminderFee);
            sqlParms.Add("@rfs", buildings[idx].reminderSplit);
            sqlParms.Add("@ff", buildings[idx].finalFee);
            sqlParms.Add("@ffs", buildings[idx].finalSplit);
            sqlParms.Add("@dcf", buildings[idx].disconnectionNoticefee);
            sqlParms.Add("@dcfs", buildings[idx].disconnectionNoticeSplit);
            sqlParms.Add("@sf", buildings[idx].summonsFee);
            sqlParms.Add("@sfs", buildings[idx].summonsSplit);
            sqlParms.Add("@df", buildings[idx].disconnectionFee);
            sqlParms.Add("@dfs", buildings[idx].disconnectionSplit);
            sqlParms.Add("@hf", buildings[idx].handoverFee);
            sqlParms.Add("@hfs", buildings[idx].handoverSplit);
            sqlParms.Add("@rt", buildings[idx].reminderTemplate);
            sqlParms.Add("@ft", buildings[idx].finalTemplate);
            sqlParms.Add("@dct", buildings[idx].diconnectionNoticeTemplate);
            sqlParms.Add("@st", buildings[idx].summonsTemplate);
            sqlParms.Add("@rsms", buildings[idx].reminderSMS);
            sqlParms.Add("@fsms", buildings[idx].finalSMS);
            sqlParms.Add("@dcsms", buildings[idx].disconnectionNoticeSMS);
            sqlParms.Add("@ssms", buildings[idx].summonsSMS);
            sqlParms.Add("@dsms", buildings[idx].disconnectionSMS);
            sqlParms.Add("@hosms", buildings[idx].handoverSMS);
            sqlParms.Add("@addy1", buildings[idx].addy1);
            sqlParms.Add("@addy2", buildings[idx].addy2);
            sqlParms.Add("@addy3", buildings[idx].addy3);
            sqlParms.Add("@addy4", buildings[idx].addy4);
            sqlParms.Add("@addy5", buildings[idx].addy5);

            if (buildings[idx].ID == 0 && !remove)
            {
                updateQuery = "INSERT INTO tblBuildings(Building, Code, AccNumber, DataPath, Period, Acc, Contra, ownbank, cashbook3, payments, receipts, journals, bc, centrec, business, bank, pm, bankName, accName, ";
                updateQuery += " bankAccNumber, branch, isBuilding, addy1, addy2, addy3, addy4, addy5, letterName)";
                updateQuery += " VALUES(@Name, @Abbr, @Trust, @DataPath, @Period, @Trust, @Cash, @ownbank, @cashbook3, @Payments, @Receipts, @Journal, @centrec, @cbuild, @business, @Bank, @PM, @BankName, @AccName, ";
                updateQuery += " @BankAccNumber, @Branch, @web, @addy1, @addy2, @addy3, @addy4, @addy5, @ln)";
            }
            else if (!remove)
            {
                updateQuery = "UPDATE tblBuildings SET Building = @Name, Code = @Abbr, AccNumber = @Trust, DataPath = @DataPath, Period = @Period, Contra = @Cash, payments = @Payments, ownbank = @ownbank, cashbook3 = @cashbook3, ";
                updateQuery += " receipts = @Receipts, journals = @Journal, bc = @centrec, centrec = @cbuild, business = @business, bank = @Bank, pm = @PM, bankName = @BankName, accName = @AccName, ";
                updateQuery += " bankAccNumber = @BankAccNumber, branch = @Branch, isBuilding = @web, addy1 = @addy1, addy2 = @addy2, addy3 = @addy3, addy4 = @addy4, addy5 = @addy5, letterName = @ln ";
                updateQuery += " WHERE id = @ID";
            }
            else
            {
                updateQuery = "DELETE FROM tblBuildings WHERE id = @ID";
            }

            if (buildings[idx].ID == 0)
            {
                String newBuildPath = "Y:\\Buildings Managed\\" + buildings[idx].Name;
                if (!Directory.Exists(newBuildPath)) { try { Directory.CreateDirectory(newBuildPath); } catch { } }
                String idQuery = "SELECT MAX(id) as ID FROM tblBuildings";
                DataSet dsID = dh.GetData(idQuery, null, out status);
                if (dsID != null && dsID.Tables.Count > 0 && dsID.Tables[0].Rows.Count > 0)
                {
                    sqlParms["@ID"] = int.Parse(dsID.Tables[0].Rows[0]["ID"].ToString());
                }
            }

            if (dh.SetData(updateQuery, sqlParms, out status) > 0 && !remove)
            {
                String feeQuery = "IF EXISTS(SELECT id FROM tblBuildingSettings WHERE buildingID = @ID)";
                feeQuery += " UPDATE tblBuildingSettings SET reminderFee = @rf, reminderSplit = @rfs, finalFee = @ff, finalSplit = @ffs, disconnectionNoticefee = @dcf, disconnectionNoticeSplit = @dcfs, ";
                feeQuery += " summonsFee = @sf, summonsSplit = @sfs, disconnectionFee = @df, disconnectionSplit = @dfs, handoverFee = @hf, handoverSplit = @hfs, reminderTemplate = @rt, finalTemplate = @ft, ";
                feeQuery += " diconnectionNoticeTemplate = @dct, summonsTemplate = @st, reminderSMS = @rsms, finalSMS = @fsms, disconnectionNoticeSMS = @dcsms, summonsSMS = @ssms, disconnectionSMS = @dsms, ";
                feeQuery += " handoverSMS = @hosms WHERE (buildingID = @ID)";
                feeQuery += " ELSE ";
                feeQuery += " INSERT INTO tblBuildingSettings(buildingID, reminderFee, reminderSplit, finalFee, finalSplit, disconnectionNoticefee, disconnectionNoticeSplit, summonsFee, summonsSplit, ";
                feeQuery += " disconnectionFee, disconnectionSplit, handoverFee, handoverSplit, reminderTemplate, finalTemplate, diconnectionNoticeTemplate, summonsTemplate, reminderSMS, finalSMS, ";
                feeQuery += " disconnectionNoticeSMS, summonsSMS, disconnectionSMS, handoverSMS)";
                feeQuery += " VALUES(@ID, @rf, @rfs, @ff, @ffs, @dcf, @dcfs, @sf, @sfs, @df, @dfs, @hf, @hfs, @rt, @ft, @dct, @st, @rsms, @fsms, @dcsms, @ssms, @dsms, @hosms)";
                dh.SetData(feeQuery, sqlParms, out status);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(Building b, out String status)
        {
            dh = new SqlDataHandler();
            String updateQuery = String.Empty;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@Name", b.Name);
            sqlParms.Add("@Abbr", b.Abbr);
            sqlParms.Add("@Trust", b.Trust);
            sqlParms.Add("@DataPath", b.DataPath);
            sqlParms.Add("@Period", b.Period);
            sqlParms.Add("@Cash", b.Cash_Book);
            sqlParms.Add("@ownbank", b.OwnBank);
            sqlParms.Add("@cashbook3", b.Cashbook3);
            sqlParms.Add("@Payments", b.Payments);
            sqlParms.Add("@Receipts", b.Receipts);
            sqlParms.Add("@Journal", b.Journal);
            sqlParms.Add("@centrec", b.Centrec_Account);
            sqlParms.Add("@cbuild", b.Centrec_Building);
            sqlParms.Add("@business", b.Business_Account);
            sqlParms.Add("@Bank", b.Bank);
            sqlParms.Add("@PM", b.PM);
            sqlParms.Add("@BankName", b.Bank_Name);
            sqlParms.Add("@AccName", b.Acc_Name);
            sqlParms.Add("@BankAccNumber", b.Bank_Acc_Number);
            sqlParms.Add("@Branch", b.Branch_Code);
            sqlParms.Add("@web", b.Web_Building);
            sqlParms.Add("@webfolder", b.webFolder);
            sqlParms.Add("@ln", b.letterName);
            sqlParms.Add("@ID", b.ID);
            sqlParms.Add("@pid", b.pid);
            sqlParms.Add("@addy1", b.addy1);
            sqlParms.Add("@addy2", b.addy2);
            sqlParms.Add("@addy3", b.addy3);
            sqlParms.Add("@addy4", b.addy4);
            sqlParms.Add("@addy5", b.addy5);

            if (b.ID == 0)
            {
                updateQuery = "INSERT INTO tblBuildings(Building, Code, AccNumber, DataPath, Period, Acc, Contra, ownbank, cashbook3, payments, receipts, journals, bc, centrec, business, bank, pm, bankName, accName, ";
                updateQuery += " bankAccNumber, branch, isBuilding, addy1, addy2, addy3, addy4, addy5, letterName, web, pid)";
                updateQuery += " VALUES(@Name, @Abbr, @Trust, @DataPath, @Period, @Trust, @Cash, @ownbank, @cashbook3, @Payments, @Receipts, @Journal, @centrec, @cbuild, @business, @Bank, @PM, @BankName, @AccName, ";
                updateQuery += " @BankAccNumber, @Branch, @web, @addy1, @addy2, @addy3, @addy4, @addy5, @ln, @webfolder, @pid)";
            }
            else
            {
                updateQuery = "UPDATE tblBuildings SET Building = @Name, Code = @Abbr, AccNumber = @Trust, DataPath = @DataPath, Period = @Period, Contra = @Cash, payments = @Payments, pid = @pid,";
                updateQuery += " receipts = @Receipts, journals = @Journal, bc = @centrec, centrec = @cbuild, business = @business, bank = @Bank, pm = @PM, bankName = @BankName, accName = @AccName, ownbank = @ownbank, cashbook3 = @cashbook3,";
                updateQuery += " bankAccNumber = @BankAccNumber, branch = @Branch, isBuilding = @web, addy1 = @addy1, addy2 = @addy2, addy3 = @addy3, addy4 = @addy4, addy5 = @addy5, letterName = @ln, web = @webfolder ";
                updateQuery += " WHERE id = @ID";
            }

            if (dh.SetData(updateQuery, sqlParms, out status) > 0)
            {
                if (b.ID == 0)
                {
                    String newBuildPath = "Y:\\Buildings Managed\\" + b.Name;
                    if (!Directory.Exists(newBuildPath)) { try { Directory.CreateDirectory(newBuildPath); } catch { } }
                    String newBuildQuery = "SELECT id from tblBuildings WHERE Building = '" + b.Name + "'";
                    DataSet dsNew = dh.GetData(newBuildQuery, null, out status);
                    if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                    {
                        b.ID = int.Parse(dsNew.Tables[0].Rows[0]["id"].ToString());
                    }
                }

                String linkQuery = "IF NOT EXISTS(SELECT id FROM tblUserBuildings WHERE userid = " + Controller.user.id.ToString() + " AND buildingID = " + b.ID.ToString() + ")";
                linkQuery += " INSERT INTO tblUserBuildings(userid, buildingid) VALUES(" + Controller.user.id.ToString() + ", " + b.ID.ToString() + ")";
                dh.SetData(linkQuery, null, out status);
                return true;
            }
            else
            {
                return false;
            }
        }

        public String getDebtorName(int bid)
        {
            String query = "SELECT DISTINCT tblUsers.name FROM tblUserBuildings INNER JOIN tblUsers ON tblUserBuildings.userid = tblUsers.id WHERE (tblUserBuildings.buildingid = " + bid.ToString() + ") AND (tblUsers.usertype = 3) ";
            DataSet dsDebtor = dh.GetData(query, null, out status);
            if (dsDebtor != null && dsDebtor.Tables.Count > 0 && dsDebtor.Tables[0].Rows.Count > 0)
            {
                String debtorEmail = dsDebtor.Tables[0].Rows[0]["name"].ToString();
                return debtorEmail;
            }
            else
            {
                return "";
            }
        }

        public String getDebtorEmail(int bid)
        {
            String query = "SELECT DISTINCT tblUsers.email FROM tblUserBuildings INNER JOIN tblUsers ON tblUserBuildings.userid = tblUsers.id WHERE (tblUserBuildings.buildingid = " + bid.ToString() + ") AND (tblUsers.usertype = 3) ";
            DataSet dsDebtor = dh.GetData(query, null, out status);
            if (dsDebtor != null && dsDebtor.Tables.Count > 0 && dsDebtor.Tables[0].Rows.Count > 0)
            {
                String debtorEmail = dsDebtor.Tables[0].Rows[0]["email"].ToString();
                return debtorEmail;
            }
            else
            {
                return "";
            }
        }
    }

    public class Building
    {
        public int ID { get; set; }

        public String Name { get; set; }

        public String Abbr { get; set; }

        public String Trust { get; set; }

        public String DataPath { get; set; }

        public int Period { get; set; }

        public String Cash_Book { get; set; }

        public String OwnBank { get; set; }

        public String Cashbook3 { get; set; }

        public int Payments { get; set; }

        public int Receipts { get; set; }

        public int Journal { get; set; }

        public String Centrec_Account { get; set; }

        public String Centrec_Building { get; set; }

        public String Business_Account { get; set; }

        public String Bank { get; set; }

        public String PM { get; set; }

        public String Debtor { get; set; }

        public String Bank_Name { get; set; }

        public String Acc_Name { get; set; }

        public String Bank_Acc_Number { get; set; }

        public String Branch_Code { get; set; }

        public bool Web_Building { get; set; }

        public String letterName { get; set; }

        public String webFolder { get; set; }

        public String pid { get; set; }

        public double reminderFee { get; set; }

        public double reminderSplit { get; set; }

        public double finalFee { get; set; }

        public double finalSplit { get; set; }

        public double disconnectionNoticefee { get; set; }

        public double disconnectionNoticeSplit { get; set; }

        public double summonsFee { get; set; }

        public double summonsSplit { get; set; }

        public double disconnectionFee { get; set; }

        public double disconnectionSplit { get; set; }

        public double handoverFee { get; set; }

        public double handoverSplit { get; set; }

        public String reminderTemplate { get; set; }

        public String finalTemplate { get; set; }

        public String diconnectionNoticeTemplate { get; set; }

        public String summonsTemplate { get; set; }

        public String reminderSMS { get; set; }

        public String finalSMS { get; set; }

        public String disconnectionNoticeSMS { get; set; }

        public String summonsSMS { get; set; }

        public String disconnectionSMS { get; set; }

        public String handoverSMS { get; set; }

        public String addy1 { get; set; }

        public String addy2 { get; set; }

        public String addy3 { get; set; }

        public String addy4 { get; set; }

        public String addy5 { get; set; }

        public bool isHOA { get; set; }

        public Building()
        {
            reminderTemplate = "";
            finalTemplate = "";
            diconnectionNoticeTemplate = "";
            summonsTemplate = "";
            reminderSMS = "";
            finalSMS = "";
            disconnectionNoticeSMS = "";
            summonsSMS = "";
            disconnectionSMS = "";
            handoverSMS = "";
        }
    }

    public class Building2
    {
        private int _id;
        private String _building;
        private String _code;
        private String _path;
        private int _period;
        private int _journal;
        private String _acc;
        private String _contra;
        private List<Customer> _customers;

        public Account buildCentrec;
        public Customer centrecBuild;
        private String _bc;
        private String _business;
        private String _bank;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String BuildingName
        {
            get { return _building; }
            set { _building = value; }
        }

        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public String Acc
        {
            get { return _acc; }
            set { _acc = value; }
        }

        public String Contra
        {
            get { return _contra; }
            set { _contra = value; }
        }

        public String BC
        {
            get { return _bc; }
            set { _bc = value; }
        }

        public String Business
        {
            get { return _business; }
            set { _business = value; }
        }

        public int Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public int Journal
        {
            get { return _journal; }
            set { _journal = value; }
        }

        public String Bank
        {
            get { return _bank; }
            set { _bank = value; }
        }

        public bool isHOA { get; set; }

        public Building2(int __id, String __building, String __code, String __path, int __period, int __journal, String __acc, String __contra, String bc, String business, Account _centrec, Customer _build, String __bank)
        {
            Id = __id;
            BuildingName = __building;
            Code = __code;
            Path = __path;
            Acc = __acc;
            Contra = __contra;
            Period = __period;
            Journal = __journal;
            BC = bc;
            Business = business;
            _customers = new List<Customer>();
            buildCentrec = _centrec;
            centrecBuild = _build;
            Bank = __bank;
        }

        public Building2(int __id, String __building, String __code, String __path, int __period, int __journal, String __acc, String __contra, String bc, String business, String __bank)
        {
            Id = __id;
            BuildingName = __building;
            Code = __code;
            Path = __path;
            Acc = __acc;
            Contra = __contra;
            Period = __period;
            Journal = __journal;
            BC = bc;
            Business = business;
            _customers = new List<Customer>();
            Bank = __bank;
        }

        public List<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }
    }

    public class PMBuilding
    {
        public String Code { get; set; }

        public String Name { get; set; }

        public String Outstanding { get; set; }

        public String Bank_Balance { get; set; }

        public String Bank_Last_Transaction_Date { get; set; }

        public String Trust_Balance { get; set; }

        public String Trust_Last_Transaction_Date { get; set; }

        public String Own_Bank_Balance { get; set; }

        public String Own_Bank_Last_Transaction_Date { get; set; }

        public String Invest_Balance { get; set; }

        public String Invest_Last_Transaction_Date { get; set; }
    }

    public class BuildingList
    {
        public String Name { get; set; }

        public String Code { get; set; }

        public String Debtor { get; set; }
    }

    public class ListLetter : BuildingList
    {
        public bool Update { get; set; }

        public bool Age_Analysis { get; set; }

        private bool printemail = false;

        public ListLetter(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public void SetPrint(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public bool Print_Email { get { return printemail; } }

        public bool File { get; set; }
    }

    public class ListStmt : BuildingList
    {
        public bool Update { get; set; }

        public bool Interest { get; set; }

        private bool printemail = false;

        public ListStmt(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public void SetPrint(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public bool Print_Email { get { return printemail; } }

        public bool File { get; set; }
    }

    public class ListMonthEnd : BuildingList
    {
        public bool Update { get; set; }

        public bool Invest_Acc { get; set; }

        public bool _9990 { get; set; }

        public bool _4000 { get; set; }

        public bool Petty_Cash { get; set; }
    }

    public class ListDaily : BuildingList
    {
        public bool Trust { get; set; }

        public bool Own { get; set; }

        public bool File { get; set; }
    }

    public class ListReport
    {
        public String Name { get; set; }

        public String Code { get; set; }

        public String Debtor { get; set; }

        public String Daily_trust { get; set; }

        public String Daily_own { get; set; }

        public String Daily_file { get; set; }

        public String Letters_updated { get; set; }

        public String Letters_ageanalysis { get; set; }

        public String Letters_printed { get; set; }

        public String Letters_filed { get; set; }

        public String Statements_updated { get; set; }

        public String Statements_interest { get; set; }

        public String Statements_printed { get; set; }

        public String Statements_filed { get; set; }

        public String Month_end_updated { get; set; }

        public String Month_end_invest_account { get; set; }

        public String Month_end_9990 { get; set; }

        public String Month_end_4000 { get; set; }

        public String Month_end_petty_cash { get; set; }
    }

    public class MonthlyFinancial
    {

        public MonthlyFinancial(int buildingID, int period, int year)
        {
            DateTime startDate = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime endDate = new DateTime(year, 12, 31, 23, 59, 59);
            SqlDataHandler dh = new SqlDataHandler();
            String query = "SELECT id, completeDate, buildingID, finPeriod, levies, leviesReason, sewage, sewageNotes, electricity, electricityNotes, water, waterNotes, specialLevies, ";
            query += " specialLevyNotes, otherIncomeDescription, otherIncome, otherIncomeNotes, memberInterest, memberInterestNotes, bankInterest, bankInterestNotes, accountingFees, ";
            query += " accountingFeesNotes, bankCharges, bankChargesNotes, sewageExpense, sewageExpenseNotes, deliveries, deliveriesNotes, electricityExpense, ";
            query += " electricityExpenseNotes, gardens, gardensNotes, insurance, insuranceNotes, interestPaid, interestPaidNotes, managementFees, managementFeesNotes, ";
            query += " meterReading, meterReadingNotes, printing, printingNotes, post, postNotes, repairs, repairsNotes, refuse, refuseNotes, salaries, salariesNotes, security, ";
            query += " securityNotes, telephone, telephoneNotes, waterExpense, waterExpenseNotes, municipal, municipalReason, trust, trustNotes, own, ownNotes, investment, ";
            query += " investmentNotes, sundy, sundryNotes, assets, assetsNotes, debtors, debtorsNotes, municipalAccounts, municipalAccountsNotes, owners, ownersNotes, suppliers, ";
            query += " suppliersNotes, liabilities, liabilitiesNotes, electricityRecon, waterRecon";
            query += " FROM tblMonthFin WHERE buildingID = " + buildingID.ToString() + " AND finPeriod = " + period.ToString() + " AND completeDate >= '" + startDate.ToString() + "' AND completeDate <= '" + endDate.ToString() + "'";
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                id = int.Parse(dr["id"].ToString());
                bool result;
                completeDate = DateTime.Parse(dr["completeDate"].ToString());
                buildingID = int.Parse(dr["buildingID"].ToString());
                finPeriod = int.Parse(dr["finPeriod"].ToString());
                levies = (bool.TryParse(dr["levies"].ToString(), out result) ? result : false);
                leviesReason = dr["leviesReason"].ToString();
                sewage = (bool.TryParse(dr["sewage"].ToString(), out result) ? result : false);
                sewageNotes = dr["sewageNotes"].ToString();
                electricity = (bool.TryParse(dr["electricity"].ToString(), out result) ? result : false);
                electricityNotes = dr["electricityNotes"].ToString();
                water = (bool.TryParse(dr["water"].ToString(), out result) ? result : false);
                waterNotes = dr["waterNotes"].ToString();
                specialLevies = (bool.TryParse(dr["specialLevies"].ToString(), out result) ? result : false);
                specialLevyNotes = dr["specialLevyNotes"].ToString();
                otherIncomeDescription = dr["otherIncomeDescription"].ToString();
                otherIncome = (bool.TryParse(dr["otherIncome"].ToString(), out result) ? result : false);
                otherIncomeNotes = dr["otherIncomeNotes"].ToString();
                memberInterest = (bool.TryParse(dr["memberInterest"].ToString(), out result) ? result : false);
                memberInterestNotes = dr["memberInterestNotes"].ToString();
                bankInterest = (bool.TryParse(dr["bankInterest"].ToString(), out result) ? result : false);
                bankInterestNotes = dr["bankInterestNotes"].ToString();
                accountingFees = (bool.TryParse(dr["accountingFees"].ToString(), out result) ? result : false);
                accountingFeesNotes = dr["accountingFeesNotes"].ToString();
                bankCharges = (bool.TryParse(dr["bankCharges"].ToString(), out result) ? result : false);
                bankChargesNotes = dr["bankChargesNotes"].ToString();
                sewageExpense = (bool.TryParse(dr["sewageExpense"].ToString(), out result) ? result : false);
                sewageExpenseNotes = dr["sewageExpenseNotes"].ToString();
                deliveries = (bool.TryParse(dr["deliveries"].ToString(), out result) ? result : false);
                deliveriesNotes = dr["deliveriesNotes"].ToString();
                electricityExpense = (bool.TryParse(dr["electricityExpense"].ToString(), out result) ? result : false);
                electricityExpenseNotes = dr["electricityExpenseNotes"].ToString();
                gardens = (bool.TryParse(dr["gardens"].ToString(), out result) ? result : false);
                gardensNotes = dr["gardensNotes"].ToString();
                insurance = (bool.TryParse(dr["insurance"].ToString(), out result) ? result : false);
                insuranceNotes = dr["insuranceNotes"].ToString();
                interestPaid = (bool.TryParse(dr["interestPaid"].ToString(), out result) ? result : false);
                interestPaidNotes = dr["interestPaidNotes"].ToString();
                managementFees = (bool.TryParse(dr["managementFees"].ToString(), out result) ? result : false);
                managementFeesNotes = dr["managementFeesNotes"].ToString();
                meterReading = (bool.TryParse(dr["meterReading"].ToString(), out result) ? result : false);
                meterReadingNotes = dr["meterReadingNotes"].ToString();
                printing = (bool.TryParse(dr["printing"].ToString(), out result) ? result : false);
                printingNotes = dr["printingNotes"].ToString();
                post = (bool.TryParse(dr["post"].ToString(), out result) ? result : false);
                postNotes = dr["postNotes"].ToString();
                repairs = (bool.TryParse(dr["repairs"].ToString(), out result) ? result : false);
                repairsNotes = dr["repairsNotes"].ToString();
                refuse = (bool.TryParse(dr["refuse"].ToString(), out result) ? result : false);
                refuseNotes = dr["refuseNotes"].ToString();
                salaries = (bool.TryParse(dr["salaries"].ToString(), out result) ? result : false);
                salariesNotes = dr["salariesNotes"].ToString();
                security = (bool.TryParse(dr["security"].ToString(), out result) ? result : false);
                securityNotes = dr["securityNotes"].ToString();
                telephone = (bool.TryParse(dr["telephone"].ToString(), out result) ? result : false);
                telephoneNotes = dr["telephoneNotes"].ToString();
                waterExpense = (bool.TryParse(dr["waterExpense"].ToString(), out result) ? result : false);
                waterExpenseNotes = dr["waterExpenseNotes"].ToString();
                municipal = (bool.TryParse(dr["municipal"].ToString(), out result) ? result : false);
                municipalReason = dr["municipalReason"].ToString();
                trust = (bool.TryParse(dr["trust"].ToString(), out result) ? result : false);
                trustNotes = dr["trustNotes"].ToString();
                own = (bool.TryParse(dr["own"].ToString(), out result) ? result : false);
                ownNotes = dr["ownNotes"].ToString();
                investment = (bool.TryParse(dr["investment"].ToString(), out result) ? result : false);
                investmentNotes = dr["investmentNotes"].ToString();
                sundy = (bool.TryParse(dr["sundy"].ToString(), out result) ? result : false);
                sundryNotes = dr["sundryNotes"].ToString();
                assets = (bool.TryParse(dr["assets"].ToString(), out result) ? result : false);
                assetsNotes = dr["assetsNotes"].ToString();
                debtors = (bool.TryParse(dr["debtors"].ToString(), out result) ? result : false);
                debtorsNotes = dr["debtorsNotes"].ToString();
                municipalAccounts = (bool.TryParse(dr["municipalAccounts"].ToString(), out result) ? result : false);
                municipalAccountsNotes = dr["municipalAccountsNotes"].ToString();
                owners = (bool.TryParse(dr["owners"].ToString(), out result) ? result : false);
                ownersNotes = dr["ownersNotes"].ToString();
                suppliers = (bool.TryParse(dr["suppliers"].ToString(), out result) ? result : false);
                suppliersNotes = dr["suppliersNotes"].ToString();
                liabilities = (bool.TryParse(dr["liabilities"].ToString(), out result) ? result : false);
                liabilitiesNotes = dr["liabilitiesNotes"].ToString();
                electricityRecon = int.Parse(dr["electricityRecon"].ToString());
                waterRecon = int.Parse(dr["waterRecon"].ToString());
            }
            else
            {
                id = 0;
            }
        }

        public int id { get; set; }

        public DateTime completeDate { get; set; }

        public int buildingID { get; set; }

        public int finPeriod { get; set; }

        public bool levies { get; set; }

        public String leviesReason { get; set; }

        public bool sewage { get; set; }

        public String sewageNotes { get; set; }

        public bool electricity { get; set; }

        public String electricityNotes { get; set; }

        public bool water { get; set; }

        public String waterNotes { get; set; }

        public bool specialLevies { get; set; }

        public String specialLevyNotes { get; set; }

        public String otherIncomeDescription { get; set; }

        public bool otherIncome { get; set; }

        public String otherIncomeNotes { get; set; }

        public bool memberInterest { get; set; }

        public String memberInterestNotes { get; set; }

        public bool bankInterest { get; set; }

        public String bankInterestNotes { get; set; }

        public bool accountingFees { get; set; }

        public String accountingFeesNotes { get; set; }

        public bool bankCharges { get; set; }

        public String bankChargesNotes { get; set; }

        public bool sewageExpense { get; set; }

        public String sewageExpenseNotes { get; set; }

        public bool deliveries { get; set; }

        public String deliveriesNotes { get; set; }

        public bool electricityExpense { get; set; }

        public String electricityExpenseNotes { get; set; }

        public bool gardens { get; set; }

        public String gardensNotes { get; set; }

        public bool insurance { get; set; }

        public String insuranceNotes { get; set; }

        public bool interestPaid { get; set; }

        public String interestPaidNotes { get; set; }

        public bool managementFees { get; set; }

        public String managementFeesNotes { get; set; }

        public bool meterReading { get; set; }

        public String meterReadingNotes { get; set; }

        public bool printing { get; set; }

        public String printingNotes { get; set; }

        public bool post { get; set; }

        public String postNotes { get; set; }

        public bool repairs { get; set; }

        public String repairsNotes { get; set; }

        public bool refuse { get; set; }

        public String refuseNotes { get; set; }

        public bool salaries { get; set; }

        public String salariesNotes { get; set; }

        public bool security { get; set; }

        public String securityNotes { get; set; }

        public bool telephone { get; set; }

        public String telephoneNotes { get; set; }

        public bool waterExpense { get; set; }

        public String waterExpenseNotes { get; set; }

        public bool municipal { get; set; }

        public String municipalReason { get; set; }

        public bool trust { get; set; }

        public String trustNotes { get; set; }

        public bool own { get; set; }

        public String ownNotes { get; set; }

        public bool investment { get; set; }

        public String investmentNotes { get; set; }

        public bool sundy { get; set; }

        public String sundryNotes { get; set; }

        public bool assets { get; set; }

        public String assetsNotes { get; set; }

        public bool debtors { get; set; }

        public String debtorsNotes { get; set; }

        public bool municipalAccounts { get; set; }

        public String municipalAccountsNotes { get; set; }

        public bool owners { get; set; }

        public String ownersNotes { get; set; }

        public bool suppliers { get; set; }

        public String suppliersNotes { get; set; }

        public bool liabilities { get; set; }

        public String liabilitiesNotes { get; set; }

        public int electricityRecon { get; set; }

        public int waterRecon { get; set; }
    }
}
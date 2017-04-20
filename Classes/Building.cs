using Astro.Library.Entities;
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

        private String buildQuery
        {
            get
            {
                String query = "* FROM tblBuildings ORDER BY Building";
                return query;
            }
        }

        private String feeQuery
        {
            get
            {
                String query = "SELECT * FROM tblBuildingSettings ";
                query += " WHERE (buildingID = @buildID)";
                return query;
            }
        }

        private String buildUserQuery
        {
            get
            {
                String query = "SELECT b.* FROM tblBuildings b INNER JOIN tblUserBuildings u ON b.id = u.buildingid ";
                query += " WHERE u.userid = @userid ORDER BY b.Building";
                return query;
            }
        }

        #endregion Queries

        private void LoadBuildings(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Building b = new Building()
                    {
                        ID = int.Parse(dr["id"].ToString()),
                        Name = dr["Building"].ToString(),
                        Abbr = dr["Code"].ToString(),
                        Trust = dr["AccNumber"].ToString(),
                        DataPath = dr["DataPath"].ToString(),
                        Period = int.Parse(dr["Period"].ToString()),
                        Cash_Book = dr["Contra"].ToString(),
                        OwnBank = dr["ownbank"].ToString(),
                        Cashbook3 = dr["cashbook3"].ToString(),
                        Payments = int.Parse(dr["payments"].ToString()),
                        Receipts = int.Parse(dr["receipts"].ToString()),
                        Journal = int.Parse(dr["journals"].ToString()),
                        Centrec_Account = dr["bc"].ToString(),
                        Centrec_Building = dr["centrec"].ToString(),
                        Business_Account = dr["business"].ToString(),
                        Bank = dr["bank"].ToString(),
                        PM = dr["pm"].ToString(),
                        Debtor = getDebtorEmail(int.Parse(dr["id"].ToString())),
                        Bank_Name = dr["bankName"].ToString(),
                        Acc_Name = dr["accName"].ToString(),
                        Bank_Acc_Number = dr["bankAccNumber"].ToString(),
                        Branch_Code = dr["branch"].ToString(),
                        Web_Building = bool.Parse(dr["isBuilding"].ToString()),
                        webFolder = dr["web"].ToString(),
                        letterName = dr["letterName"].ToString(),
                        addy1 = dr["addy1"].ToString(),
                        addy2 = dr["addy2"].ToString(),
                        addy3 = dr["addy3"].ToString(),
                        addy4 = dr["addy4"].ToString(),
                        addy5 = dr["addy5"].ToString(),
                        pid = dr["pid"].ToString(),
                        isHOA = bool.Parse(dr["hoa"].ToString())
                    };
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
                        b.reminderFee = b.reminderSplit = b.finalFee = b.finalSplit = b.disconnectionNoticefee = b.disconnectionNoticeSplit = b.summonsFee = b.summonsSplit = b.disconnectionFee = b.disconnectionSplit = 0;
                        b.handoverFee = b.handoverSplit = 0;
                        b.reminderTemplate = b.finalTemplate = b.diconnectionNoticeTemplate = b.summonsTemplate = b.reminderSMS = b.finalSMS = b.disconnectionNoticeSMS = b.summonsSMS = b.disconnectionSMS = b.handoverSMS = "";
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
                Building b = new Building
                {
                    ID = 0,
                    Name = "Add new building"
                };
                buildings.Add(b);
            }
            LoadBuildings(dh.GetData(buildQuery, null, out status));
        }

        public Buildings(bool addNew, String nameValue)
        {
            buildings = new List<Building>();
            if (addNew)
            {
                Building b = new Building
                {
                    ID = 0,
                    Name = nameValue
                };
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
                if (dsID != null && dsID.Tables.Count > 0 && dsID.Tables[0].Rows.Count > 0) { sqlParms["@ID"] = int.Parse(dsID.Tables[0].Rows[0]["ID"].ToString()); }
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
                    if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0) { b.ID = int.Parse(dsNew.Tables[0].Rows[0]["id"].ToString()); }
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
            return (dsDebtor != null && dsDebtor.Tables.Count > 0 && dsDebtor.Tables[0].Rows.Count > 0) ? dsDebtor.Tables[0].Rows[0]["name"].ToString() : "";
        }

        public String getDebtorEmail(int bid)
        {
            String query = "SELECT DISTINCT tblUsers.email FROM tblUserBuildings INNER JOIN tblUsers ON tblUserBuildings.userid = tblUsers.id WHERE (tblUserBuildings.buildingid = " + bid.ToString() + ") AND (tblUsers.usertype = 3) ";
            DataSet dsDebtor = dh.GetData(query, null, out status);
            return (dsDebtor != null && dsDebtor.Tables.Count > 0 && dsDebtor.Tables[0].Rows.Count > 0) ? dsDebtor.Tables[0].Rows[0]["email"].ToString() : "";
        }
    }
}
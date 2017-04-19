using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;

namespace Astrodon.Controls
{
    public partial class usrDebtor : UserControl
    {
        private List<Building> buildings;
        private String status;

        private SqlDataHandler dataHandler = new SqlDataHandler();
        public List<ListDaily> dailyList = new List<ListDaily>();
        public List<ListLetter> letterList = new List<ListLetter>();
        public List<ListMonthEnd> monthList = new List<ListMonthEnd>();
        public List<ListStmt> stmtList = new List<ListStmt>();

        public usrDebtor()
        {
            InitializeComponent();
            LoadBuildings();
        }

        private void usrDebtor_Load(object sender, EventArgs e)
        {
            dgDaily.DataSource = dailyList;
            dgStmt.DataSource = stmtList;
            dgMonth.DataSource = monthList;
            dgLetters.DataSource = letterList;
            SetDaily();
            SetStmt();
            SetMonth();
            SetLetters();
        }

        private void LoadBuildings()
        {
            Buildings bManager = new Buildings(false);
            List<Building> allBuildings = bManager.buildings;
            buildings = new List<Building>();
            foreach (int bid in Controller.user.buildings)
            {
                foreach (Building b in allBuildings)
                {
                    if (bid == b.ID && !buildings.Contains(b))
                    {
                        buildings.Add(b);
                        break;
                    }
                }
            }
            buildings = buildings.OrderBy(c => c.Name).ToList();
            foreach (Building b in buildings)
            {
                ListDaily ld = new ListDaily();
                ListStmt ls = new ListStmt(false);
                ListMonthEnd lm = new ListMonthEnd();
                ListLetter ll = new ListLetter(false);
                ld.Name = ls.Name = lm.Name = ll.Name = b.Name;
                ld.Code = ls.Code = lm.Code = ll.Code = b.Abbr;
                ld.Debtor = ls.Debtor = lm.Debtor = ll.Debtor = bManager.getDebtorName(b.ID);

                ll.Update = false;
                ll.Age_Analysis = false;
                ll.File = false;

                ls.Update = false;
                ls.Interest = false;
                ls.File = false;

                lm.Update = false;
                lm.Invest_Acc = false;
                lm._9990 = false;
                lm._4000 = false;
                lm.Petty_Cash = false;

                ld.Trust = false;
                ld.Own = false;
                ld.File = false;

                dailyList.Add(ld);
                monthList.Add(lm);
                stmtList.Add(ls);
                letterList.Add(ll);
            }
        }

        private String GetBuildingID(String buildingCode)
        {
            String bID = "";
            foreach (Building b in buildings)
            {
                if (buildingCode == b.Abbr)
                {
                    bID = b.ID.ToString();
                    break;
                }
            }
            return bID;
        }

        #region Daily

        private void dailyPicker_ValueChanged(object sender, EventArgs e)
        {
            SetDaily();
        }

        private void SetDaily()
        {
            DateTime dailyDate = dailyPicker.Value;
            String dailyQuery = "SELECT dailytrust, dailyown, dailyfile FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            foreach (ListDaily ld in dailyList)
            {
                ld.Trust = false;
                ld.Own = false;
                ld.File = false;
                foreach (Building b in buildings)
                {
                    if (ld.Code == b.Abbr)
                    {
                        Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                        sqlParms.Add("@bid", b.ID);
                        sqlParms.Add("@cdate", dailyDate.ToString("yyyy/MM/dd"));
                        DataSet dsDaily = dataHandler.GetData(dailyQuery, sqlParms, out status);
                        if (dsDaily != null && dsDaily.Tables.Count > 0 && dsDaily.Tables[0].Rows.Count > 0)
                        {
                            DataRow drDaily = dsDaily.Tables[0].Rows[0];
                            try
                            {
                                ld.Trust = bool.Parse(drDaily["dailytrust"].ToString());
                                ld.Own = bool.Parse(drDaily["dailyown"].ToString());
                                ld.File = bool.Parse(drDaily["dailyfile"].ToString());
                            }
                            catch { }
                        }
                        break;
                    }
                }
            }
            dgDaily.Invalidate();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String query = "IF EXISTS(SELECT id FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate))";
            query += " UPDATE tblDebtors SET dailytrust = @dt, dailyown = @do, dailyfile = @df WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            query += " ELSE ";
            query += " INSERT INTO tblDebtors(buildingID, completeDate, dailytrust, dailyown, dailyfile)";
            query += " VALUES(@bid, @cdate, @dt, @do, @df)";
            foreach (ListDaily ld in dailyList)
            {
                if (ld.Trust || ld.Own || ld.File)
                {
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@bid", GetBuildingID(ld.Code));
                    sqlParms.Add("@cdate", dailyPicker.Value.ToString("yyyy/MM/dd"));
                    sqlParms.Add("@dt", ld.Trust);
                    sqlParms.Add("@do", ld.Own);
                    sqlParms.Add("@df", ld.File);
                    dataHandler.SetData(query, sqlParms, out status);
                }
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show("Submit complete");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SetDaily();
        }

        #endregion Daily

        #region Letters

        private void SetLetters()
        {
            DateTime dailyDate = letterDatePicker.Value;
            String dailyQuery = "SELECT lettersupdated, lettersageanalysis, lettersprintemail, lettersfiled FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            foreach (ListLetter ld in letterList)
            {
                ld.Update = false;
                ld.Age_Analysis = false;
                ld.SetPrint(false);
                ld.File = false;
                foreach (Building b in buildings)
                {
                    if (ld.Code == b.Abbr)
                    {
                        Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                        sqlParms.Add("@bid", b.ID);
                        sqlParms.Add("@cdate", dailyDate.ToString("yyyy/MM/dd"));
                        DataSet dsDaily = dataHandler.GetData(dailyQuery, sqlParms, out status);
                        if (dsDaily != null && dsDaily.Tables.Count > 0 && dsDaily.Tables[0].Rows.Count > 0)
                        {
                            DataRow drDaily = dsDaily.Tables[0].Rows[0];
                            try
                            {
                                ld.Update = bool.Parse(drDaily["lettersupdated"].ToString());
                                ld.Age_Analysis = bool.Parse(drDaily["lettersageanalysis"].ToString());
                                ld.SetPrint(bool.Parse(drDaily["lettersprintemail"].ToString()));
                                ld.File = bool.Parse(drDaily["lettersfiled"].ToString());
                            }
                            catch { }
                        }
                        break;
                    }
                }
            }
        }

        private void letterDatePicker_ValueChanged(object sender, EventArgs e)
        {
            SetLetters();
        }

        private void btnSubmitLetters_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String query = "IF EXISTS(SELECT id FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate))";
            query += " UPDATE tblDebtors SET lettersupdated = @lu, lettersageanalysis = @la, lettersprintemail = @lp, lettersfiled = @lf WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            query += " ELSE ";
            query += " INSERT INTO tblDebtors(buildingID, completeDate, lettersupdated, lettersageanalysis, lettersprintemail, lettersfiled)";
            query += " VALUES(@bid, @cdate, @lu, @la, @lp, @lf)";
            foreach (ListLetter ld in letterList)
            {
                if (ld.Update || ld.Age_Analysis || ld.Print_Email || ld.File)
                {
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@bid", GetBuildingID(ld.Code));
                    sqlParms.Add("@cdate", letterDatePicker.Value.ToString("yyyy/MM/dd"));
                    sqlParms.Add("@lu", ld.Update);
                    sqlParms.Add("@la", ld.Age_Analysis);
                    sqlParms.Add("@lp", ld.Print_Email);
                    sqlParms.Add("@lf", ld.File);
                    dataHandler.SetData(query, sqlParms, out status);
                }
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show("Submit complete");
        }

        private void btnResetLetters_Click(object sender, EventArgs e)
        {
            SetLetters();
        }

        #endregion Letters

        #region Month

        private void btnResetMonth_Click(object sender, EventArgs e)
        {
            SetMonth();
        }

        private void btnSubmitMonth_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String query = "IF EXISTS(SELECT id FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate))";
            query += " UPDATE tblDebtors SET meupdate = @mu, meinvest = @mi, me9990 = @mn, me4000 = @mf, mepettycash = @mp WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            query += " ELSE ";
            query += " INSERT INTO tblDebtors(buildingID, completeDate, meupdate, meinvest, me9990, me4000, mepettycash)";
            query += " VALUES(@bid, @cdate, @mu, @mi, @mn, @mf, @mp)";
            foreach (ListMonthEnd ld in monthList)
            {
                if (ld.Update || ld.Invest_Acc || ld._9990 || ld._4000)
                {
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@bid", GetBuildingID(ld.Code));
                    sqlParms.Add("@cdate", stmtPicker.Value.ToString("yyyy/MM/dd"));
                    sqlParms.Add("@mu", ld.Update);
                    sqlParms.Add("@mi", ld.Invest_Acc);
                    sqlParms.Add("@mn", ld._9990);
                    sqlParms.Add("@mf", ld._4000);
                    sqlParms.Add("@mp", ld.Petty_Cash);
                    dataHandler.SetData(query, sqlParms, out status);
                }
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show("Submit complete");
        }

        private void monthPicker_ValueChanged(object sender, EventArgs e)
        {
            SetMonth();
        }

        private void SetMonth()
        {
            //stmtupdated, stmtinterest, stmtprintemail, stmtfiled
            DateTime dailyDate = stmtPicker.Value;
            String dailyQuery = "SELECT  meupdate, meinvest, me9990, me4000, mepettycash FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            foreach (ListMonthEnd ld in monthList)
            {
                ld.Update = false;
                ld.Invest_Acc = false;
                ld._9990 = false;
                ld._4000 = false;
                ld.Petty_Cash = false;
                foreach (Building b in buildings)
                {
                    if (ld.Code == b.Abbr)
                    {
                        Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                        sqlParms.Add("@bid", b.ID);
                        sqlParms.Add("@cdate", dailyDate.ToString("yyyy/MM/dd"));
                        DataSet dsDaily = dataHandler.GetData(dailyQuery, sqlParms, out status);
                        if (dsDaily != null && dsDaily.Tables.Count > 0 && dsDaily.Tables[0].Rows.Count > 0)
                        {
                            DataRow drDaily = dsDaily.Tables[0].Rows[0];
                            try
                            {
                                ld.Update = bool.Parse(drDaily["meupdate"].ToString());
                                ld.Invest_Acc = bool.Parse(drDaily["meinvest"].ToString());
                                ld._9990 = bool.Parse(drDaily["me9990"].ToString());
                                ld._4000 = bool.Parse(drDaily["me4000"].ToString());
                                ld.Petty_Cash = bool.Parse(drDaily["mepettycash"].ToString());
                            }
                            catch { }
                        }
                        break;
                    }
                }
            }
        }

        #endregion Month

        #region Statements

        private void stmtPicker_ValueChanged(object sender, EventArgs e)
        {
            SetStmt();
        }

        private void SetStmt()
        {
            //stmtupdated, stmtinterest, stmtprintemail, stmtfiled
            DateTime dailyDate = stmtPicker.Value;
            String dailyQuery = "SELECT stmtupdated, stmtinterest, stmtprintemail, stmtfiled FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            foreach (ListStmt ld in stmtList)
            {
                ld.Update = false;
                ld.Interest = false;
                ld.SetPrint(false);
                ld.File = false;
                foreach (Building b in buildings)
                {
                    if (ld.Code == b.Abbr)
                    {
                        Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                        sqlParms.Add("@bid", b.ID);
                        sqlParms.Add("@cdate", dailyDate.ToString("yyyy/MM/dd"));
                        DataSet dsDaily = dataHandler.GetData(dailyQuery, sqlParms, out status);
                        if (dsDaily != null && dsDaily.Tables.Count > 0 && dsDaily.Tables[0].Rows.Count > 0)
                        {
                            DataRow drDaily = dsDaily.Tables[0].Rows[0];
                            try
                            {
                                ld.Update = bool.Parse(drDaily["stmtupdated"].ToString());
                                ld.Interest = bool.Parse(drDaily["stmtinterest"].ToString());
                                ld.SetPrint(bool.Parse(drDaily["stmtprintemail"].ToString()));
                                ld.File = bool.Parse(drDaily["stmtfiled"].ToString());
                            }
                            catch { }
                        }
                        break;
                    }
                }
            }
        }

        private void btnSubmitStmt_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String query = "IF EXISTS(SELECT id FROM tblDebtors WHERE (buildingID = @bid) AND (completeDate = @cdate))";
            query += " UPDATE tblDebtors SET stmtupdated = @su, stmtinterest = @si, stmtprintemail = @sp, stmtfiled = @sf WHERE (buildingID = @bid) AND (completeDate = @cdate)";
            query += " ELSE ";
            query += " INSERT INTO tblDebtors(buildingID, completeDate, stmtupdated, stmtinterest, stmtprintemail, stmtfiled)";
            query += " VALUES(@bid, @cdate, @su, @si, @sp, @sf)";
            foreach (ListStmt ld in stmtList)
            {
                if (ld.Update || ld.Interest || ld.Print_Email || ld.File)
                {
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@bid", GetBuildingID(ld.Code));
                    sqlParms.Add("@cdate", stmtPicker.Value.ToString("yyyy/MM/dd"));
                    sqlParms.Add("@su", ld.Update);
                    sqlParms.Add("@si", ld.Interest);
                    sqlParms.Add("@sp", ld.Print_Email);
                    sqlParms.Add("@sf", ld.File);
                    dataHandler.SetData(query, sqlParms, out status);
                }
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show("Submit complete");
        }

        private void btnResetStmt_Click(object sender, EventArgs e)
        {
            SetStmt();
        }

        #endregion Statements
    }
}
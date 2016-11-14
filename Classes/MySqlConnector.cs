using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon {

    public class SqlArgs : EventArgs {
        public String msgArgs;

        public SqlArgs(String args) {
            msgArgs = args;
        }
    }

    public class MySqlConnector {
        private MySqlConnection mysqlConn;
        private MySqlCommand mysqlCmd;
        private String sqlStatus;

        public String SqlStatus {
            get { return sqlStatus; }
            set {
                sqlStatus = value;
                if (MessageHandler != null) { MessageHandler(this, new SqlArgs(sqlStatus)); }
            }
        }

        public event EventHandler<SqlArgs> MessageHandler;

        private String ConnectionString { get; set; }

        public MySqlConnector() {
            try {
                String server = "10.0.1.252";
                String database = "astrodon_co_za";
                String uid = "root";
                String password = "66r94e77";
                String connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                ConnectionString = connectionString;
                KillSleepingConnections();
                mysqlConn = new MySqlConnection(connectionString);
            } catch (Exception ex) {
                String msg = ex.Message;
            }
        }

        private int KillSleepingConnections() {
            string strSQL = "show processlist";
            System.Collections.ArrayList m_ProcessesToKill = new ArrayList();
            String status;
            DataSet ds = GetData(strSQL, null, out status);

            try {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                    foreach (DataRow dr in ds.Tables[0].Rows) {
                        // Find all processes sleeping with a timeout value higher than our threshold.
                        int iPID = Convert.ToInt32(dr["Id"].ToString());
                        string strState = dr["Command"].ToString();
                        int iTime = Convert.ToInt32(dr["Time"].ToString());

                        if (strState == "Sleep" && iPID > 0) {
                            // This connection is sitting around doing nothing. Kill it.
                            m_ProcessesToKill.Add(iPID);
                        }
                    }
                }

                foreach (int aPID in m_ProcessesToKill) {
                    strSQL = "kill " + aPID;
                    SetData(strSQL, null, out status);
                }
            } catch (Exception excep) {
            } finally {
            }
            return m_ProcessesToKill.Count;
        }

        public bool ToggleConnection(bool open) {
            bool success = false;
            try {
                if (open) {
                    if (mysqlConn.State != System.Data.ConnectionState.Open) { mysqlConn.Open(); }
                    success = mysqlConn.State == System.Data.ConnectionState.Open;
                } else {
                    if (mysqlConn.State != System.Data.ConnectionState.Closed) { mysqlConn.Close(); }
                    success = mysqlConn.State == System.Data.ConnectionState.Closed;
                }
            } catch { }
            return success;
        }

        public bool InsertStatement(String title, String description, String file, String unitno, String[] emails) {
            description = "Customer Statements";
            String status = String.Empty;
            String cQuery = "SELECT * FROM tx_astro_docs WHERE file = @file";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@file", file);
            DataSet cDS = GetData(cQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = status; }
            if (cDS != null && cDS.Tables.Count > 0 && cDS.Tables[0].Rows.Count > 0) {
                return true;
            } else {
                String cruser_id = "0";
                if (emails.Length > 0) {
                    foreach (String email in emails) {
                        if (GetLogin(email, unitno, out cruser_id)) { break; }
                    }
                }
                sqlParms.Add("@cruser_id", cruser_id);
                sqlParms.Add("@title", title);
                sqlParms.Add("@description", description);
                sqlParms.Add("@unitno", unitno);
                String query = "INSERT INTO tx_astro_docs(pid, tstamp, crdate, cruser_id, title, description, file, unitno)";
                query += " VALUES(1, UNIX_TIMESTAMP(now()),UNIX_TIMESTAMP(now()),@cruser_id,@title,@description,@file,@unitno);";
                return SetData(query, sqlParms, out status);
            }
        }

        public bool InsertBuilding(Building b, out String status) {
            String testQuery = "SELECT uid FROM tx_astro_complex WHERE name = @name AND abbr = @abbr";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@name", b.Name);
            sqlParms.Add("@abbr", b.Abbr);
            sqlParms.Add("@pm", b.PM);
            sqlParms.Add("@debtor", b.Debtor);
            String address = "";
            bool success = false;
            if (!String.IsNullOrEmpty(b.addy1)) { address += b.addy1; }
            if (!String.IsNullOrEmpty(b.addy2)) { address += ", " + b.addy2; }
            if (!String.IsNullOrEmpty(b.addy3)) { address += ", " + b.addy3; }
            if (!String.IsNullOrEmpty(b.addy4)) { address += ", " + b.addy4; }
            if (!String.IsNullOrEmpty(b.addy5)) { address += ", " + b.addy5; }
            sqlParms.Add("@addy", address);
            DataSet dsTest = GetData(testQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = "Building - " + testQuery + status; }
            if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0) {
                sqlParms.Add("@uid", dsTest.Tables[0].Rows[0]["uid"].ToString());
                String update = "UPDATE tx_astro_complex SET name = @name, abbr = @abbr, exceptions_email = @debtor, debtors_email = @debtor, agent_email = @pm, address = @addy WHERE uid = @uid";
                success = SetData(update, sqlParms, out status);
                status = "OK";
                return success;
            } else if (status == "OK") {
                String query = "INSERT INTO tx_astro_complex(pid, tstamp, crdate, cruser_id, name, abbr, exceptions_email, debtors_email, agent_email, address)";
                query += " VALUES(1, UNIX_TIMESTAMP(now()), UNIX_TIMESTAMP(now()), 1, @name, @abbr, @debtor, @debtor, @pm, @addy)";
                success = SetData(query, sqlParms, out status);
                if (status != "OK") { SqlStatus = "Building - " + query + status; }
                return success;
            } else {
                return false;
            }
        }

        public bool InsertBuilding(Building b, String oldName, String oldAbbr, out String buildingID, out String status) {
            String testQuery = "SELECT uid FROM tx_astro_complex WHERE name = @oname AND abbr = @oabbr";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@oname", oldName);
            sqlParms.Add("@oabbr", oldAbbr);
            sqlParms.Add("@name", b.Name);
            sqlParms.Add("@abbr", b.Abbr);
            sqlParms.Add("@pm", b.PM);
            sqlParms.Add("@debtor", b.Debtor);
            String address = "";
            bool success = false;
            if (!String.IsNullOrEmpty(b.addy1)) { address += b.addy1; }
            if (!String.IsNullOrEmpty(b.addy2)) { address += ", " + b.addy2; }
            if (!String.IsNullOrEmpty(b.addy3)) { address += ", " + b.addy3; }
            if (!String.IsNullOrEmpty(b.addy4)) { address += ", " + b.addy4; }
            if (!String.IsNullOrEmpty(b.addy5)) { address += ", " + b.addy5; }
            sqlParms.Add("@addy", address);
            DataSet dsTest = GetData(testQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = "Building - " + testQuery + status; }
            if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0) {
                String mySqlID = dsTest.Tables[0].Rows[0]["uid"].ToString();
                sqlParms.Add("@uid", mySqlID);
                String update = "UPDATE tx_astro_complex SET name = @name, abbr = @abbr, exceptions_email = @debtor, debtors_email = @debtor, agent_email = @pm, address = @addy WHERE uid = @uid";
                success = SetData(update, sqlParms, out status);
                status = "OK";
                String pid = GetPID(b.Name);
                if (pid == "") {
                    b.pid = "0";
                    UpdatePages(b);
                    pid = GetPID(b.Name);
                    b.pid = pid;
                }
                buildingID = pid;
                UpdatePages(b);
                return success;
            } else if (status == "OK") {
                String query = "INSERT INTO tx_astro_complex(pid, tstamp, crdate, cruser_id, name, abbr, exceptions_email, debtors_email, agent_email, address)";
                query += " VALUES(1, UNIX_TIMESTAMP(now()), UNIX_TIMESTAMP(now()), 1, @name, @abbr, @debtor, @debtor, @pm, @addy)";
                success = SetData(query, sqlParms, out status);
                if (status != "OK") { SqlStatus = "Building - " + query + status; }
                sqlParms["@oname"] = b.Name;
                sqlParms["@oabbr"] = b.Abbr;
                dsTest = GetData(testQuery, sqlParms, out status);

                if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0) {
                    buildingID = dsTest.Tables[0].Rows[0]["uid"].ToString();
                    String pid = GetPID(b.Name);
                    if (pid == "") {
                        b.pid = "0";
                        UpdatePages(b);
                        pid = GetPID(b.Name);
                    }
                    buildingID = pid;
                    UpdatePages(b);
                } else {
                    buildingID = "";
                }
                return success;
            } else {
                buildingID = "";
                return false;
            }
        }

        public bool DeleteBuilding(Building b, String oldName, String oldAbbr, out String status) {
            String testQuery = "SELECT uid FROM tx_astro_complex WHERE name = @oname AND abbr = @oabbr";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@oname", oldName);
            sqlParms.Add("@oabbr", oldAbbr);
            sqlParms.Add("@name", b.Name);
            sqlParms.Add("@abbr", b.Abbr);
            sqlParms.Add("@pm", b.PM);
            sqlParms.Add("@debtor", b.Debtor);
            String address = "";
            bool success = false;
            if (!String.IsNullOrEmpty(b.addy1)) { address += b.addy1; }
            if (!String.IsNullOrEmpty(b.addy2)) { address += ", " + b.addy2; }
            if (!String.IsNullOrEmpty(b.addy3)) { address += ", " + b.addy3; }
            if (!String.IsNullOrEmpty(b.addy4)) { address += ", " + b.addy4; }
            if (!String.IsNullOrEmpty(b.addy5)) { address += ", " + b.addy5; }
            sqlParms.Add("@addy", address);
            DataSet dsTest = GetData(testQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = "Building - " + testQuery + status; }
            if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0) {
                String mySqlID = dsTest.Tables[0].Rows[0]["uid"].ToString();
                sqlParms.Add("@uid", mySqlID);
                String update = "UPDATE tx_astro_account_user_mapping SET cruser_id = 0 WHERE complex_id = @uid";
                success = SetData(update, sqlParms, out status);
                status = "OK";
                return success;
            } else {
                return true;
            }
        }

        public bool UpdateBuilding(Building b, String oldName, String oldAbbr, out String status) {
            String testQuery = "SELECT uid FROM tx_astro_complex WHERE name = @oname AND abbr = @oabbr";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@oname", oldName);
            sqlParms.Add("@oabbr", oldAbbr);
            sqlParms.Add("@pm", b.PM);
            sqlParms.Add("@debtor", b.Debtor);
            String address = "";
            bool success = false;
            if (!String.IsNullOrEmpty(b.addy1)) { address += b.addy1; }
            if (!String.IsNullOrEmpty(b.addy2)) { address += ", " + b.addy2; }
            if (!String.IsNullOrEmpty(b.addy3)) { address += ", " + b.addy3; }
            if (!String.IsNullOrEmpty(b.addy4)) { address += ", " + b.addy4; }
            if (!String.IsNullOrEmpty(b.addy5)) { address += ", " + b.addy5; }
            sqlParms.Add("@addy", address);
            DataSet dsTest = GetData(testQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = "Building - " + testQuery + status; }
            if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0) {
                String mySqlID = dsTest.Tables[0].Rows[0]["uid"].ToString();
                sqlParms.Add("@uid", mySqlID);
                String update = "UPDATE tx_astro_complex SET exceptions_email = @debtor, debtors_email = @debtor, agent_email = @pm ";
                update += " WHERE uid = @uid AND (exceptions_email <> @debtor OR debtors_email <> @debtor OR agent_email <> @pm)";
                success = SetData(update, sqlParms, out status);
            }
            return success;
        }

        public String GetPID(String title) {
            String query = "SELECT uid FROM pages WHERE title = '" + title + "'";
            String status = "";
            DataSet pidDS = GetData(query, null, out status);
            if (pidDS != null && pidDS.Tables.Count > 0 && pidDS.Tables[0].Rows.Count > 0) {
                return pidDS.Tables[0].Rows[0]["uid"].ToString();
            } else {
                return "";
            }
        }

        public void UpdatePages(Building b) {
            String query = "";
            if (b.pid == "0") {
                query = "INSERT INTO pages(pid, deleted, hidden, title, doktype, urltype)";
                query += " VALUES(@pid, @deleted, @hidden, @title, @doktype, @urltype)";
            } else {
                query = "UPDATE pages SET title = @title WHERE uid = @pid";
            }
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@pid", b.pid == "0" ? GetPID(b.Name.Substring(0, 1)) : b.pid);
            sqlParms.Add("@deleted", 0);
            sqlParms.Add("@hidden", 0);
            sqlParms.Add("@title", b.Name);
            sqlParms.Add("@doktype", 1);
            sqlParms.Add("@urltype", 1);
            String status;
            SetData(query, sqlParms, out status);
        }

        public bool UpdateWebCustomer(String buildingName, String acc, String[] emails) {
            bool success = false;
            try {
                String status;
                bool isCurrentCustomer = false;
                bool hasAccount = false;
                bool createNewUser = false;
                String uid = String.Empty;
                String cruser_id = String.Empty;
                String complex_id = String.Empty;
                String complex_name = String.Empty;
                foreach (String email in emails) {
                    String owner, tenant;
                    String currentQuery = "select uid, cruser_id, owner_email, tenant_email,complex_id,complex_name  FROM tx_astro_account_user_mapping WHERE account_no = '" + acc + "'";
                    DataSet dsCurrent = GetData(currentQuery, null, out status);
                    if (dsCurrent != null && dsCurrent.Tables.Count > 0 && dsCurrent.Tables[0].Rows.Count > 0) {
                        uid = dsCurrent.Tables[0].Rows[0]["uid"].ToString();
                        cruser_id = dsCurrent.Tables[0].Rows[0]["cruser_id"].ToString();
                        owner = dsCurrent.Tables[0].Rows[0]["owner_email"].ToString();
                        tenant = dsCurrent.Tables[0].Rows[0]["tenant_email"].ToString();
                        complex_id = dsCurrent.Tables[0].Rows[0]["complex_id"].ToString();
                        complex_name = dsCurrent.Tables[0].Rows[0]["complex_name"].ToString();
                        hasAccount = true;
                        if (owner == email) {
                            isCurrentCustomer = true;
                            break;
                        }
                    } else {
                        hasAccount = false;
                    }
                }
                if (String.IsNullOrEmpty(complex_id)) {
                    String buildingQuery = "SELECT uid FROM tx_astro_complex WHERE name = '" + buildingName + "'";
                    DataSet bDS = GetData(buildingQuery, null, out status);
                    if (bDS != null && bDS.Tables.Count > 0 && bDS.Tables[0].Rows.Count > 0) {
                        complex_id = bDS.Tables[0].Rows[0]["uid"].ToString();
                        complex_name = buildingName;
                    }
                }
                if (!isCurrentCustomer && hasAccount) {
                    String otherQuery = "SELECT * FROM tx_astro_account_user_mapping WHERE cruser_id = '" + cruser_id + "' AND account_no <> '" + acc + "'";
                    DataSet dsOther = GetData(otherQuery, null, out status);
                    if (dsOther != null && dsOther.Tables.Count > 0 && dsOther.Tables[0].Rows.Count > 0) { createNewUser = true; }
                } else if (isCurrentCustomer) {
                    String updateMappingQuery = "UPDATE tx_astro_docs SET cruser_id = '" + cruser_id + "' WHERE unitno = '" + acc + "' AND cruser_id <> '" + cruser_id + "'";
                    SetData(updateMappingQuery, null, out status);
                    if (status == "OK") { success = true; }
                }

                if (!isCurrentCustomer && !String.IsNullOrEmpty(complex_id) && !String.IsNullOrEmpty(complex_name)) {
                    if (!hasAccount) { //new fe & mapping
                        String newFe = "INSERT INTO fe_users(username, disable, email, tx_astro_accountno, tx_astro_complex_name)";
                        newFe += " VALUES('" + emails[0] + "', 1, '" + emails[0] + "', '" + acc + "', '" + buildingName + "')";
                        SetData(newFe, null, out status);
                        String feUser = "SELECT uid FROM fe_users WHERE email = '" + emails[0] + "'";
                        DataSet newFeDS = GetData(feUser, null, out status);
                        if (newFeDS != null && newFeDS.Tables.Count > 0 && newFeDS.Tables[0].Rows.Count > 0) {
                            cruser_id = newFeDS.Tables[0].Rows[0]["uid"].ToString();
                            UpdateUser(cruser_id);
                            String insertMapping = "INSERT INTO tx_astro_account_user_mapping(cruser_id, account_no, owner_email, tenant_email, complex_id, complex_name)";
                            insertMapping += " VALUES('" + cruser_id + "', '" + acc + "', '" + emails[0] + "', '" + emails[0] + "', '" + complex_id + "', '" + complex_name + "')";
                            SetData(insertMapping, null, out status);
                            String updateMappingQuery = "UPDATE tx_astro_docs SET cruser_id = '" + cruser_id + "' WHERE unitno = '" + acc + "' AND cruser_id <> '" + cruser_id + "'";
                            SetData(updateMappingQuery, null, out status);
                            if (status == "OK") { success = true; }
                        }
                    } else if (createNewUser) { //new fe, update mapping
                        String newFe = "INSERT INTO fe_users(username, disable, email, tx_astro_accountno, tx_astro_complex_name)";
                        newFe += " VALUES('" + emails[0] + "', 1, '" + emails[0] + "', '" + acc + "', '" + buildingName + "')";
                        SetData(newFe, null, out status);
                        String feUser = "SELECT uid FROM fe_users WHERE email = '" + emails[0] + "'";
                        DataSet newFeDS = GetData(feUser, null, out status);
                        if (newFeDS != null && newFeDS.Tables.Count > 0 && newFeDS.Tables[0].Rows.Count > 0) {
                            cruser_id = newFeDS.Tables[0].Rows[0]["uid"].ToString();
                            UpdateUser(cruser_id);
                            String insertMapping = "UPDATE tx_astro_account_user_mapping SET cruser_id = '" + cruser_id + "', owner_email = '" + emails[0] + "', ";
                            insertMapping += " tenant_email = '" + emails[0] + "' WHERE account_no = '" + acc + "'";
                            SetData(insertMapping, null, out status);
                            String updateMappingQuery = "UPDATE tx_astro_docs SET cruser_id = '" + cruser_id + "' WHERE unitno = '" + acc + "' AND cruser_id <> '" + cruser_id + "'";
                            SetData(updateMappingQuery, null, out status);
                            if (status == "OK") { success = true; }
                        }
                    } else { //update fe & mapping
                        String newFe = "UPDATE fe_users SET username = '" + emails[0] + "', disable = 1, password = '', email = '" + emails[0] + "' WHERE uid = '" + cruser_id + "'";
                        SetData(newFe, null, out status);
                        String insertMapping = "UPDATE tx_astro_account_user_mapping SET owner_email = '" + emails[0] + "', tenant_email = '" + emails[0] + "' WHERE account_no = '" + acc + "'";
                        SetData(insertMapping, null, out status);
                        if (status == "OK") { success = true; }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return success;
        }

        public void InsertCustomer(Building b, String acc, String[] emails, out String status) {
            String bQuery = "SELECT uid FROM tx_astro_complex WHERE name = '" + b.Name + "'";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@bname", b.Name);
            DataSet dsB = GetData(bQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = "Customer - " + bQuery + status; }
            List<String> emailAddresses = emails.ToList();
            if (dsB != null && dsB.Tables.Count > 0 && dsB.Tables[0].Rows.Count > 0) {
                DataRow drB = dsB.Tables[0].Rows[0];
                String bID = drB["uid"].ToString();
                bool newCustomer = false;
                CheckOldLogins(acc, emails);
                String validEmail = String.Empty;
                String cruser_id = CheckCustomer(acc, emails, out status, out newCustomer, out validEmail);
                MessageBox.Show(validEmail);
                if (status != "OK") { SqlStatus = "Customer - 129" + status; }
                if (!String.IsNullOrEmpty(cruser_id) && !String.IsNullOrEmpty(validEmail)) {
                    if (!String.IsNullOrEmpty(validEmail)) {
                        sqlParms.Add("@acc", acc);
                        sqlParms.Add("@cruser_id", cruser_id);
                        sqlParms.Add("@validEmail", validEmail);
                        sqlParms.Add("@bID", bID);

                        String checkMappingID = "SELECT * FROM tx_astro_account_user_mapping WHERE account_no = @acc";
                        DataSet ds = GetData(checkMappingID, sqlParms, out status);
                        if (status != "OK") { SqlStatus = "Customer - 146" + status; }
                        String updateMappingQuery = String.Empty;
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                            if (ds.Tables[0].Rows.Count > 1) {
                                updateMappingQuery = "DELETE FROM tx_astro_account_user_mapping WHERE account_no = @acc AND owner_email <> @validEmail";
                                SetData(updateMappingQuery, sqlParms, out status);
                                if (status != "OK") { SqlStatus = "Customer - 152" + status; }

                                bool hasAccount = false;

                                foreach (DataRow dr in ds.Tables[0].Rows) {
                                    if (dr["owner_email"].ToString() == validEmail && !hasAccount) {
                                        hasAccount = true;
                                    } else {
                                        updateMappingQuery = "DELETE FROM tx_astro_account_user_mapping WHERE account_no = '" + acc + "' AND owner_email <> '" + validEmail + "'";
                                        SetData(updateMappingQuery, null, out status);
                                        if (status != "OK") { SqlStatus = "Customer - 162" + status; }
                                    }
                                }
                                if (!hasAccount) {
                                    String updCQuery = "INSERT INTO tx_astro_account_user_mapping(pid, tstamp, crdate, cruser_id, account_no, owner_email, tenant_email, complex_id, complex_name)";
                                    updCQuery += " VALUES(1, UNIX_TIMESTAMP(now()), UNIX_TIMESTAMP(now()), @cruser_id, @acc, @validEmail, @validEmail, @bID, @bname)";
                                    SetData(updCQuery, sqlParms, out status);
                                    if (status != "OK") { SqlStatus = "Customer - 170" + status; }
                                }
                            }
                            UpdateUser(cruser_id);
                        } else {
                            String updCQuery = "INSERT INTO tx_astro_account_user_mapping(pid, tstamp, crdate, cruser_id, account_no, owner_email, tenant_email, complex_id, complex_name)";
                            updCQuery += " VALUES(1, UNIX_TIMESTAMP(now()), UNIX_TIMESTAMP(now()), @cruser_id, @acc, @validEmail, @validEmail, @bID, @bname)";
                            SetData(updCQuery, sqlParms, out status);
                            if (status != "OK") { SqlStatus = "Customer - 179" + status; }

                            if (status != "OK") { status += " - " + updCQuery; }
                        }
                        updateMappingQuery = "UPDATE tx_astro_docs SET cruser_id = @cruser_id WHERE unitno = @acc AND cruser_id <> @cruser_id";
                        SetData(updateMappingQuery, sqlParms, out status);
                        if (status != "OK") { SqlStatus = status; }
                    }
                } else {
                    SqlStatus = "No valid email address";
                }
                UpdateLogins();
            } else if (InsertBuilding(b, out status)) {
                InsertCustomer(b, acc, emails, out status);
            }
            status = SqlStatus;
        }

        private void UpdateLogins() {
            String query = "update fe_users f inner join tx_astro_account_user_mapping t on f.uid = t.cruser_id and f.username = t.owner_email ";
            query += " set f.disable = 0 where f.disable = 1 and f.password <> '' and f.password is not null";
            String status = String.Empty;
            SetData(query, null, out status);
        }

        private String UpdateUser(String cruser_id) {
            String updQuery = "update  fe_users f1, fe_users f2 set f1.pid= f2.pid,f1.tstamp = f2.tstamp,f1.starttime = f2.starttime,f1.endtime = f1.endtime,f1.crdate= f2.crdate,";
            updQuery += " f1.cruser_id= f2.cruser_id,f1.lockToDomain= f2.lockToDomain,f1.deleted= f2.deleted,f1.uc= f2.uc,f1.TSconfig= f2.TSconfig,f1.fe_cruser_id= f2.fe_cruser_id,";
            updQuery += " f1.tx_astro_usertype= f2.tx_astro_usertype,f1.tx_feuserloginsystem_redirectionafterlogin= f2.tx_feuserloginsystem_redirectionafterlogin,";
            updQuery += " f1.tx_feuserloginsystem_redirectionafterlogout= f2.tx_feuserloginsystem_redirectionafterlogout,f1.tx_astro_activation_code= f2.tx_astro_activation_code,";
            updQuery += " f1.tx_astro_istenant= f2.tx_astro_istenant,f1.felogin_redirectPid= f2.felogin_redirectPid,f1.felogin_forgotHash= f2.felogin_forgotHash";
            updQuery += " where f1.uid = '" + cruser_id + "' and f2.username = 'sheldon@astrodon.co.za'";
            String status = String.Empty;
            SetData(updQuery, null, out status);
            return status;
        }

        private void CheckOldLogins(String acc, String[] emails) {
            String query = "select f.uid from fe_users f inner join tx_astro_account_user_mapping t on f.uid = t.cruser_id where t.account_no = @accNo and f.username <> @email and f.disable <> 1";
            String status = String.Empty;
            foreach (String email in emails) {
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@accNo", acc);
                sqlParms.Add("@email", email);
                DataSet ds = GetData(query, sqlParms, out status);
                if (status != "OK") { SqlStatus = "Customer - 214" + status; }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                    foreach (DataRow dr in ds.Tables[0].Rows) {
                        String uid = dr["uid"].ToString();
                        String deleteQuery = "UPDATE fe_users SET disable = 1 WHERE uid = " + uid;
                        SetData(deleteQuery, null, out status);
                        if (status != "OK") { SqlStatus = "Customer - 220 - " + deleteQuery + "-" + status; }
                    }
                }
            }
        }

        public String CheckCustomer(String acc, String[] emails, out String status, out bool newCustomer, out String validEmail) {
            status = String.Empty;
            newCustomer = true;
            String cruser_id = String.Empty;
            validEmail = String.Empty;
            foreach (String email in emails) {
                if (!email.Contains("imp.ad-one.co.za") && GetLogin(email, acc, out cruser_id)) {
                    validEmail = email;
                    newCustomer = false;
                    break;
                } else if (!email.Contains("imp.ad-one.co.za")) {
                    validEmail = email;
                    CreateLogin(email, acc, out cruser_id);
                    newCustomer = true;
                }
            }
            status = "OK";
            return cruser_id;
        }

        private bool GetLogin(String emailAddress, String acc, out string uid) {
            String status = String.Empty;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@emailAddress", emailAddress);
            String feQuery = "SELECT uid FROM fe_users WHERE username = @emailAddress";
            DataSet dsFE = GetData(feQuery, sqlParms, out status);
            if (status != "OK") { SqlStatus = "Customer - 247" + status; }

            if (dsFE != null && dsFE.Tables.Count > 0 && dsFE.Tables[0].Rows.Count > 0) {
                uid = dsFE.Tables[0].Rows[0]["uid"].ToString();
                return true;
            } else {
                uid = "0";
                return false;
            }
        }

        private bool CreateLogin(String emailAddress, String acc, out string uid) {
            String status = String.Empty;
            String feIQ = "INSERT INTO fe_users(pid, tstamp, username, disable, email, crdate, tx_astro_accountno, starttime, endtime, cruser_id, lockToDomain, deleted, uc, ";
            feIQ += "TSconfig, fe_cruser_id, tx_astro_usertype, tx_feuserloginsystem_redirectionafterlogin, tx_feuserloginsystem_redirectionafterlogout, tx_astro_activation_code, ";
            feIQ += "tx_astro_istenant, felogin_redirectPid, felogin_forgotHash)";
            feIQ += " SELECT f2.pid, UNIX_TIMESTAMP(now()), '" + emailAddress + "', 1, '" + emailAddress + "', UNIX_TIMESTAMP(now()), '" + acc + "', f2.starttime, f2.endtime, f2.cruser_id, ";
            feIQ += " f2.lockToDomain, f2.deleted, f2.uc, f2.TSconfig, f2.fe_cruser_id, f2.tx_astro_usertype, f2.tx_feuserloginsystem_redirectionafterlogin, f2.tx_feuserloginsystem_redirectionafterlogout, ";
            feIQ += " f2.tx_astro_activation_code, f2.tx_astro_istenant, f2.felogin_redirectPid, f2.felogin_forgotHash FROM fe_users f2 WHERE f2.username = 'sheldon@astrodon.co.za'";
            SetData(feIQ, null, out status);
            if (status != "OK") { SqlStatus = "Customer - 266" + status; }
            return GetLogin(emailAddress, acc, out uid);
        }

        public DataSet GetData(String query, Dictionary<String, Object> sqlParms, out String status) {
            DataSet ds = new DataSet();

            using (MySqlConnection connect = new MySqlConnection(ConnectionString)) {
                using (MySqlCommand cmd = new MySqlCommand(query, connect)) {
                    try {
                        if (connect.State != ConnectionState.Open) { connect.Open(); }
                        if (sqlParms != null) {
                            foreach (KeyValuePair<String, Object> sqlParm in sqlParms) {
                                cmd.Parameters.AddWithValue(sqlParm.Key, sqlParm.Value);
                            }
                        }
                        MySql.Data.MySqlClient.MySqlDataAdapter da = new MySqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        status = "OK";
                        connect.Close();
                    } catch (Exception ex) {
                        ds = null;
                        status = ex.Message;
                        //MessageBox.Show(status);
                    } finally {
                        if (connect.State != ConnectionState.Closed) { connect.Close(); }
                    }
                }
            }
            return ds;
        }

        public DataSet GetFiles(String unitno) {
            String query = "SELECT tstamp, title, file FROM tx_astro_docs where unitno = '" + unitno + "' ORDER BY tstamp DESC";
            String status = "";
            return GetData(query, null, out status);
        }

        public DataSet GetCustomerDocs(String buildingName, out String status) {
            if (buildingName.ToUpper().StartsWith("VILLAGE GREEN")) { buildingName = "VILLAGE GREEN B/C"; }
            String query = "SELECT d.uid, d.tstamp, d.title, d.file, d.unitno FROM tx_astro_docs d inner join tx_astro_account_user_mapping m on d.unitno = m.account_no";
            query += " inner join tx_astro_complex c on m.complex_id = c.uid where c.name = '" + buildingName + "' order by d.tstamp";
            return GetData(query, null, out status);
        }

        public bool SetData(String query, Dictionary<String, Object> sqlParms, out String status) {
            bool success = false;
            using (MySqlConnection connect = new MySqlConnection(ConnectionString)) {
                using (MySqlCommand cmd = new MySqlCommand(query, connect)) {
                    try {
                        if (connect.State != ConnectionState.Open) { connect.Open(); }
                        if (sqlParms != null) {
                            foreach (KeyValuePair<String, Object> sqlParm in sqlParms) {
                                cmd.Parameters.AddWithValue(sqlParm.Key, sqlParm.Value);
                            }
                        }
                        cmd.ExecuteNonQuery();
                        success = true;
                        status = "OK";
                        connect.Close();
                    } catch (Exception ex) {
                        status = ex.Message;
                        //MessageBox.Show(status);
                    } finally {
                        if (connect.State != ConnectionState.Closed) { connect.Close(); }
                    }
                }
            }
            return success;
        }
    }
}
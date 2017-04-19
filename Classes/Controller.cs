using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace Astrodon
{
    public class Controller
    {
        public static frmLogin loginF;
        public static frmMain mainF;
        public static User user;
        public static Pastel pastel;
        public static Classes.CommClient commClient;
        private static SqlDataHandler dataHandler;

        public static void RunProgram()
        {
            try
            {
                pastel = new Pastel();
            }
            catch
            {
                MessageBox.Show("No pastel");
            }
            mainF = new frmMain();
            dataHandler = new SqlDataHandler();
            Login();
        }

        private static void Login()
        {
            bool loggedIn = false;
            loginF = new frmLogin(true);
            while (!loggedIn)
            {
                if (loginF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    String status = String.Empty;
                    loggedIn = Utilities.Login(loginF.username, loginF.password, out user, out status);
                    loginF = new frmLogin(loggedIn);
                }
                else
                {
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
            commClient = new Classes.CommClient();
            commClient.MessageReceived += commClient_MessageReceived;
            commClient.LoginOK += commClient_LoginOK;
            commClient.Login(user.username, user.password);
            if (user.id == 2)
            {
                pastel.pastelDirectory = "\\\\SERVER2\\Pastel11\\";
                //pastelDirectory = "C:\\Pastel12";
            }
            mainF.Show();
            DependencyInitialization();
        }

        private static void commClient_LoginOK(object sender, EventArgs e)
        {
            commClient.SendMessage("hello server");
        }

        private static System.Timers.Timer tmrDependency;
        private static DataSet dsDependency;
        private static DataSet newDSDependency;

        public static void DependencyInitialization()
        {
            tmrDependency = new System.Timers.Timer(60000);
            tmrDependency.Elapsed += tmrDependency_Elapsed;
            ThreadStart thrCheck = new ThreadStart(CheckStatus);
            Thread tCheck = new Thread(thrCheck);
            tCheck.Start();
            tmrDependency.Enabled = true;
        }

        private static void tmrDependency_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmrDependency.Enabled = false;
            if (!ShowingJobList)
            {
                ThreadStart thrCheck = new ThreadStart(CheckStatus);
                Thread tCheck = new Thread(thrCheck);
                tCheck.Start();
                while (tCheck.IsAlive) { }
            }
            tmrDependency.Enabled = true;
        }

        public static event EventHandler<EventArgs> JobUpdateEvent;

        private static void JobHandler()
        {
            SqlConnection connection = dataHandler.sqlConnection;
            try
            {
                if (connection.State != ConnectionState.Open) { connection.Open(); }
                SqlCommand command = new SqlCommand("SELECT COUNT(id) AS qCount, currentStatus FROM tblPMJob WHERE (status <> 'Complete' AND status <> 'APPROVED') GROUP BY status ORDER BY status", connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                newDSDependency = new DataSet();
                da.Fill(newDSDependency);

                if (!CompareDataSets(dsDependency, newDSDependency))
                {
                    dsDependency = newDSDependency;
                    if (dsDependency != null && dsDependency.Tables.Count > 0 && dsDependency.Tables[0].Rows.Count > 0)
                    {
                        String notify = "Current queue status:" + Environment.NewLine;
                        foreach (DataRow dr in dsDependency.Tables[0].Rows)
                        {
                            notify += dr["qCount"].ToString() + " " + dr["status"].ToString() + Environment.NewLine;
                        }
                        mainF.PopupNotification(notify);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) { connection.Close(); }
            }
        }

        public static bool ShowingJobList = false;
        private static DateTime lastCheckedDate = DateTime.Now.AddDays(-1);

        public static void CheckStatus()
        {
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            String query = "SELECT id FROM tblJobUpdate WHERE " + (user.usertype == 2 ? "pmLastUpdated " : "paLastUpdated") + " > @lastChecked AND " + (user.usertype == 2 ? "pmID " : "paID") + " = @id";
            sqlParms.Add("@lastChecked", lastCheckedDate);
            sqlParms.Add("@id", user.id);
            String status;
            DataSet ds = new SqlDataHandler().GetData(query, sqlParms, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lastCheckedDate = DateTime.Now;
                System.Media.SystemSounds.Exclamation.Play();
                if (!Controller.mainF.notifyIcon1.Visible) { Controller.mainF.notifyIcon1.Visible = true; }
                Controller.mainF.notifyIcon1.Text = "You have new jobs to action";
                Controller.mainF.notifyIcon1.BalloonTipText = "You have new jobs to action";
                Controller.mainF.notifyIcon1.ShowBalloonTip(10000);
            }
        }

        private static bool CompareDataSets(DataSet ds1, DataSet ds2)
        {
            bool match = true;
            if ((ds1 == null || ds2 == null) && (ds1 != ds2))
            {
                return false;
            }
            else if (ds1 != null && ds2 != null)
            {
                if ((ds1.Tables.Count != ds2.Tables.Count) && ds2.Tables.Count > 0)
                {
                    return false;
                }
                else if (ds1.Tables.Count == ds2.Tables.Count && ds2.Tables.Count > 0 && ds1.Tables[0].Rows.Count != ds2.Tables[0].Rows.Count)
                {
                    return false;
                }
                else if (ds1.Tables[0].Rows.Count == ds2.Tables[0].Rows.Count && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[i];
                        DataRow dr2 = ds2.Tables[0].Rows[i];
                        String d1Count = dr1["qCount"].ToString();
                        String d2Count = dr1["qCount"].ToString();
                        String d1Type = dr1["status"].ToString();
                        String d2Type = dr1["status"].ToString();
                        if (d1Count != d2Count || d1Type != d2Type)
                        {
                            match = false;
                            break;
                        }
                    }
                }
            }
            return match;
        }

        // Handler method
        private static void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            JobHandler();
        }

        public static void DependencyTermination()
        {
            // Release the dependency.
            SqlDependency.Stop(dataHandler.connString);
        }

        public static bool FiredUpdate = false;

        private static void commClient_MessageReceived(object sender, Classes.IMReceivedEventArgs e)
        {
            String myMessage = e.Message;
            if (myMessage.ToLower().StartsWith("hello"))
            {
                String user = myMessage.ToLower().Replace("hello ", "").ToUpper();
                mainF.SetNotifications(user + " connected to server");
            }
            if (!e.Message.ToUpper().Contains(commClient.UserName.ToUpper()))
            {
                mainF.PopupNotification(e.Message);
                FiredUpdate = false;
                if (JobUpdateEvent != null) { JobUpdateEvent(null, new EventArgs()); }
            }
            Thread.Sleep(5000);
            JobHandler();
        }

        private static void UpdateJobStatus(String status, String jobID, String userID)
        {
            String query = "INSERT INTO tblPMJobStatus(jobID, actioned, status) VALUES(@jobID, @actioned, @status)";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@jobID", jobID);
            sqlParms.Add("@actioned", userID);
            sqlParms.Add("@status", status);
            dataHandler.SetData(query, sqlParms, out status);
        }

        public static void AssignJob()
        {
            SqlDataHandler dm = new SqlDataHandler();
            String status;
            String qQuery = "SELECT * FROM tblPMJob WHERE (status = 'PENDING') ORDER BY id";
            String avPAQuery = "SELECT paID FROM tblPAStatus WHERE (paStatus = 'True') AND paID in (" + Controller.user.id.ToString() + ") ORDER BY availableSince";
            DataSet dsQ = dm.GetData(qQuery, null, out status);
            DataSet dsPA = dm.GetData(avPAQuery, null, out status);
            bool validQ = (dsQ != null && dsQ.Tables.Count > 0 && dsQ.Tables[0].Rows.Count > 0);
            bool validPA = (dsPA != null && dsPA.Tables.Count > 0 && dsPA.Tables[0].Rows.Count > 0);
            String updateQuery = "UPDATE tblPMJob SET status = 'ASSIGNED', processedBy = {0}, assigneddate = getdate() WHERE id = {1}";
            String updQ2 = "UPDATE tblPAStatus SET paStatus = 'False' WHERE paID = {0}";
            if (validQ && validPA)
            {
                int rowCount = 0;
                if (dsQ.Tables[0].Rows.Count == dsPA.Tables[0].Rows.Count)
                {
                    rowCount = dsQ.Tables[0].Rows.Count;
                }
                else if (dsQ.Tables[0].Rows.Count > dsPA.Tables[0].Rows.Count)
                {
                    rowCount = dsPA.Tables[0].Rows.Count;
                }
                else if (dsQ.Tables[0].Rows.Count < dsPA.Tables[0].Rows.Count)
                {
                    rowCount = dsQ.Tables[0].Rows.Count;
                }
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow drQ = dsQ.Tables[0].Rows[i];
                    DataRow drPA = dsPA.Tables[0].Rows[i];
                    String eQ = String.Format(updateQuery, drPA["paID"].ToString(), drQ["id"].ToString(), drPA["paID"].ToString());
                    UpdateJobStatus("ASSIGNED", drQ["id"].ToString(), drPA["paID"].ToString());
                    String jobMessage = "Job ID " + drQ["id"].ToString() + " has been assigned to " + Controller.user.name;
                    commClient.SendMessage(jobMessage);
                    String eQ2 = String.Format(updQ2, drPA["paID"].ToString());
                    dm.SetData(eQ, null, out status);
                    dm.SetData(eQ2, null, out status);
                }
            }
        }
    }
}
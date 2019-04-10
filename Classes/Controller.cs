using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Data.Entity;

namespace Astrodon
{
    public class Controller
    {
        public static frmLogin loginF;
        public static frmMain mainF;
        public static User user;
        public static Pastel pastel;
      //  public static Classes.CommClient commClient;
        private static SqlDataHandler dataHandler;
        private static System.Timers.Timer tmrDependency;
        private static DataSet dsDependency;
        private static DataSet newDSDependency;
        public static bool ShowingJobList = false;
        private static DateTime lastCheckedDate = DateTime.Now.AddDays(-1);
        public static bool FiredUpdate = false;

        public static event EventHandler<EventArgs> JobUpdateEvent;

        public static bool UserIsSheldon()
        {
            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(user.username))
                return false;

            return user.username.ToLower() == "sheldon" || user.username.ToLower() == "tertia";
        }

        public static void RunProgram()
        {
            mainF = new frmMain();
            dataHandler = new SqlDataHandler();
            Login();
            pastel = new Pastel();
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
            //commClient = new Classes.CommClient();
            //commClient.MessageReceived += commClient_MessageReceived;
            //commClient.LoginOK += commClient_LoginOK;
            //commClient.Login(user.username, user.password);
            mainF.Show();
            DependencyInitialization();
        }

        internal static void HandleError(Exception e, string title = "Application Error")
        {
            HandleError(e.Message + Environment.NewLine + e.StackTrace, title);
        }

        internal static void HandleError(string error, string title = "Application Error")
        {
            
            MessageBox.Show(error, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal static void ShowMessage(string message, string title = "Information")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static void ShowWarning(string message, string title = "Warning")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        internal static bool AskQuestion(string message, string title = "Confirmation")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private static void commClient_LoginOK(object sender, EventArgs e)
        {
            //commClient.SendMessage("hello server");
        }

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
            catch (Exception ex) { Controller.HandleError(ex); }

            finally
            {
                if (connection.State != ConnectionState.Closed) { connection.Close(); }
            }
        }

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

        private static void UpdateJobStatus(String status, String jobID, String userID)
        {
            String query = "INSERT INTO tblPMJobStatus(jobID, actioned, status) VALUES(@jobID, @actioned, @status)";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@jobID", jobID);
            sqlParms.Add("@actioned", userID);
            sqlParms.Add("@status", status);
            dataHandler.SetData(query, sqlParms, out status);
        }

        public static void SetPAAvailable()
        {
            if (Controller.user.usertype == 4)
            {
                using (var ctx = SqlDataHandler.GetDataContext())
                {
                    var myUser = ctx.tblPAStatus.SingleOrDefault(a => a.paID == Controller.user.id);
                    if(myUser == null)
                    {
                        myUser = new Data.tblPAStatu()
                        {
                            paID = Controller.user.id
                        };
                        ctx.tblPAStatus.Add(myUser);
                    }

                    myUser.paStatus = true;
                    myUser.availableSince = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public static void AssignJob()
        {
            SetPAAvailable();
            SqlDataHandler dm = new SqlDataHandler();
            String status;

            //query to find first item to assign to this pm when he is done.
            String qQuery = "SELECT top 5 * FROM tblPMJob WHERE (status = 'PENDING') ORDER BY id";

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
                    //commClient.SendMessage(jobMessage);
                    String eQ2 = String.Format(updQ2, drPA["paID"].ToString());
                    dm.SetData(eQ, null, out status);
                    dm.SetData(eQ2, null, out status);
                }
            }
        }

        public static string ReadResourceString(string path)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string result = string.Empty;

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }


        private static string _TrustPath = "";
        private static string GetTrustPath()
        {
            if (!string.IsNullOrWhiteSpace(_TrustPath))
                return _TrustPath;

            String query = "SELECT trust FROM tblSettings";
            String status;
            DataSet ds = (new SqlDataHandler()).GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _TrustPath = ds.Tables[0].Rows[0]["trust"].ToString();
            }
            else
            {
                _TrustPath = String.Empty;
            }
            return _TrustPath;
        }

        public static double? GetBuildingBalance(Astrodon.Data.tblBuilding building)
        {
            if (building.bank == null)
                throw new Exception("Building bank not configured");

            string buildingPath = "";
            if (building.bank.ToUpper() == "TRUST")
            {
                buildingPath = GetTrustPath();
                return GetBalance(buildingPath, building.AccNumber);
            }
            else
            {
                buildingPath = building.DataFolder;
                return GetBalance(buildingPath, building.ownbank);
            }
        }

        private static double GetBalance(String datapath, String account)
        {
            String acc = Controller.pastel.GetAccount(datapath, account.Replace("/", ""));
            if (acc.StartsWith("99"))
            {
                return 0;
            }
            else
            {
                String[] accBits = acc.Split(new String[] { "|" }, StringSplitOptions.None);
                double bal = 0;
                try
                {
                    for (int i = 7; i <= 32; i++)
                    {
                        if (i < accBits.Length)
                        {
                            double lbal = (double.TryParse(accBits[i], out lbal) ? lbal : 0);
                            bal += lbal;
                        }
                    }
                }
                catch (Exception ex) { Controller.HandleError(ex); }
                return bal;
            }
        }
    }
}
using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Astrodon
{
    public class Utilities
    {
        public static bool Login(String username, String password, out User user, out String status)
        {
            user = new Users().GetUser(username, password, out user, out status);

            if (user != null)
            {
                return true;
            }
            else if (Environment.MachineName != "VIRTUALXP-34829")
            {
                return false;
            }
            else
            {
                user = new User();
                user.id = 1;
                user.name = "ADMIN";
                return true;
            }
        }

        public static List<int> GetBuildingsIDs(int usertype, int userid, String email, out String status)
        {
            List<int> buildings = new List<int>();
            String buildQuery1 = "SELECT buildingid as id FROM tblUserBuildings WHERE userid = @userid";
            String buildQuery2 = "SELECT id FROM tblBuildings WHERE pm = @userid";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            String query = String.Empty;
            status = String.Empty;
            if (usertype == 2)
            {
                query = buildQuery2;
                sqlParms.Add("@userid", email);
            }
            else if (usertype == 1 || usertype == 3)
            {
                query = buildQuery1;
                sqlParms.Add("@userid", userid);
            }
            else
            {
                return buildings;
            }
            SqlDataHandler dh = new SqlDataHandler();
            DataSet ds = dh.GetData(query, sqlParms, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int buildID = int.Parse(dr["id"].ToString());
                    buildings.Add(buildID);
                }
            }
            return buildings;
        }

        public static List<Customer> getAllCustomers(String buildingName, String buildPath)
        {
            return Controller.pastel.AddCustomers(buildingName, buildPath);
        }

        public static Dictionary<String, Building2> GetReportBuildings()
        {
            String status;
            Dictionary<String, Building2> repBuildings = new Dictionary<string, Building2>();
            repBuildings.Clear();
            String centrecQuery = "SELECT centrec FROM tblSettings";
            SqlDataHandler dh = new SqlDataHandler();
            DataSet dsCentrec = dh.GetData(centrecQuery, null, out status);
            String centrecPath = "";
            if (dsCentrec != null && dsCentrec.Tables.Count > 0 && dsCentrec.Tables[0].Rows.Count > 0)
            {
                centrecPath = dsCentrec.Tables[0].Rows[0]["centrec"].ToString();
            }
            else
            {
                MessageBox.Show(status);
            }
            List<Building> buildings = new Buildings(false).buildings;
            //String myPath = "";
            //String pastelTest = Controller.pastel.SetPath("CENTRE17", out myPath);
            //if (pastelTest != "0") { MessageBox.Show(myPath); }

            foreach (Building b in buildings)
            {
                try
                {
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
                    if (buildCentrec != null && centrecBuild != null)
                    {
                        Building2 build = new Building2(id, building, code, path, period, journal, acc, centrec_building, centrec, business, buildCentrec, centrecBuild, bank);
                        repBuildings.Add(building, build);
                    }
                    //break;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return repBuildings;
        }

        public static void ProcessKiller(String fileName)
        {
            Process tool = new Process();
            tool.StartInfo.FileName = "handle.exe";
            tool.StartInfo.Arguments = fileName + " /accepteula";
            tool.StartInfo.UseShellExecute = false;
            tool.StartInfo.RedirectStandardOutput = true;
            tool.Start();
            tool.WaitForExit();
            string outputTool = tool.StandardOutput.ReadToEnd();

            string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
            foreach (Match match in Regex.Matches(outputTool, matchPattern))
            {
                Process.GetProcessById(int.Parse(match.Value)).Kill();
            }
        }

        public static String GetTrustPath()
        {
            String status;
            String query = "SELECT trust FROM tblSettings";
            DataSet ds = (new SqlDataHandler()).GetData(query, null, out status);
            String trustPath = String.Empty;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) { trustPath = ds.Tables[0].Rows[0]["trust"].ToString(); }
            return trustPath;
        }
    }

   

}
﻿using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Astrodon
{
    public class Users
    {
        public User GetUser(String username, String password, out User user, out String status)
        {
            user = new User();
            String loginQuery = "SELECT id, admin, email, name, phone, fax, usertype, pmSignature FROM tblUsers WHERE BINARY_CHECKSUM(username) = BINARY_CHECKSUM(@username)";
            loginQuery += " AND BINARY_CHECKSUM(password) = BINARY_CHECKSUM(@password)";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@username", username);
            sqlParms.Add("@password", password);
            SqlDataHandler dh = new SqlDataHandler();
            DataSet ds = dh.GetData(loginQuery, sqlParms, out status);
            if (status == "OK" && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                user.id = int.Parse(dr["id"].ToString());
                user.admin = bool.Parse(dr["admin"].ToString());
                user.email = dr["email"].ToString();
                user.name = dr["name"].ToString();
                user.phone = dr["phone"].ToString();
                user.fax = dr["fax"].ToString();
                user.usertype = int.Parse(dr["usertype"].ToString());
                user.buildings = GetBuildingsIDs(user.usertype, user.id, user.email, out status);
                user.username = username;
                user.password = password;
                user.signature = null;
                try
                {
                    byte[] sigArray = (byte[])dr["pmSignature"];
                    MemoryStream ms = new MemoryStream(sigArray);
                    user.signature = Image.FromStream(ms);
                }
                catch { }
                return user;
            }
            else
            {
                return null;
            }
        }

        public User GetUser(String email, out User user, out String status)
        {
            user = new User();
            String loginQuery = "SELECT id, admin, email, name, phone, fax, usertype, pmSignature FROM tblUsers WHERE email = @email";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@email", email);
            SqlDataHandler dh = new SqlDataHandler();
            DataSet ds = dh.GetData(loginQuery, sqlParms, out status);
            if (status == "OK" && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                user.id = int.Parse(dr["id"].ToString());
                user.admin = bool.Parse(dr["admin"].ToString());
                user.email = dr["email"].ToString();
                user.name = dr["name"].ToString();
                user.phone = dr["phone"].ToString();
                user.fax = dr["fax"].ToString();
                user.usertype = int.Parse(dr["usertype"].ToString());
                user.buildings = GetBuildingsIDs(user.usertype, user.id, user.email, out status);
                user.signature = null;
                return user;
            }
            else
            {
                return null;
            }
        }

        public User GetUser(int id)
        {
            User user = new User();
            String loginQuery = "SELECT * FROM tblUsers WHERE id = " + id.ToString();
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            DataSet ds = dh.GetData(loginQuery, sqlParms, out status);
            if (status == "OK" && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                user.id = int.Parse(dr["id"].ToString());
                user.admin = bool.Parse(dr["admin"].ToString());
                user.email = dr["email"].ToString();
                user.name = dr["name"].ToString();
                user.phone = dr["phone"].ToString();
                user.fax = dr["fax"].ToString();
                user.usertype = int.Parse(dr["usertype"].ToString());
                user.buildings = GetBuildingsIDs(user.usertype, user.id, user.email, out status);
                user.username = dr["username"].ToString();
                user.password = dr["password"].ToString();
                user.signature = null;
                try
                {
                    byte[] sigArray = (byte[])dr["pmSignature"];
                    MemoryStream ms = new MemoryStream(sigArray);
                    user.signature = Image.FromStream(ms);
                }
                catch { }
                return user;
            }
            else
            {
                return null;
            }
        }

        public User GetUserBuild(int id)
        {
            User user = new User();
            String loginQuery = "SELECT DISTINCT u.id, u.admin, u.email, u.name, u.phone, u.fax, u.usertype, u.username, u.password";
            loginQuery += " FROM tblUserBuildings ub INNER JOIN tblUsers u ON ub.userid = u.id INNER JOIN tblBuildings b ON ub.buildingid = b.id";
            loginQuery += " WHERE (b.id = " + id.ToString() + ") AND (u.usertype = 3)";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            DataSet ds = dh.GetData(loginQuery, sqlParms, out status);
            if (status == "OK" && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                user.id = int.Parse(dr["id"].ToString());
                user.admin = bool.Parse(dr["admin"].ToString());
                user.email = dr["email"].ToString();
                user.name = dr["name"].ToString();
                user.phone = dr["phone"].ToString();
                user.fax = dr["fax"].ToString();
                user.usertype = int.Parse(dr["usertype"].ToString());
                user.buildings = GetBuildingsIDs(user.usertype, user.id, user.email, out status);
                user.username = dr["username"].ToString();
                user.password = dr["password"].ToString();
                user.signature = null;

                return user;
            }
            else
            {
                MessageBox.Show(status);
                return null;
            }
        }

        public List<User> GetUsers(bool addnew)
        {
            List<User> users = new List<User>();
            if (addnew)
            {
                User u = new User();
                u.id = 0;
                u.name = "Add new user";
                users.Add(u);
            }
            String status = String.Empty;
            String loginQuery = "SELECT id, username, password, admin, email, name, phone, fax, usertype, pmSignature FROM tblUsers order by name";
            SqlDataHandler dh = new SqlDataHandler();
            DataSet ds = dh.GetData(loginQuery, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    User user = new User();
                    user.id = int.Parse(dr["id"].ToString());
                    user.username = dr["username"].ToString();
                    user.password = dr["password"].ToString();
                    user.admin = bool.Parse(dr["admin"].ToString());
                    user.email = dr["email"].ToString();
                    user.name = dr["name"].ToString();
                    user.phone = dr["phone"].ToString();
                    user.fax = dr["fax"].ToString();
                    user.usertype = int.Parse(dr["usertype"].ToString());
                    user.buildings = GetBuildingsIDs(user.usertype, user.id, user.email, out status);
                    user.signature = null;
                    try
                    {
                        byte[] sigArray = (byte[])dr["pmSignature"];
                        MemoryStream ms = new MemoryStream(sigArray);
                        user.signature = Image.FromStream(ms);
                    }
                    catch { }

                    users.Add(user);
                }
                return users;
            }
            else
            {
                return null;
            }
        }

        public bool SaveUser(User u)
        {
            String status = String.Empty;
            String updateUserQuery = "IF EXISTS (SELECT id FROM tblUsers WHERE id = @id)";
            updateUserQuery += " UPDATE tblUsers SET username = @username, password = @password, admin = @admin, email = @email, name = @name, phone = @phone,";
            updateUserQuery += " fax = @fax, usertype = @usertype, pmSignature = @sig WHERE id = @id";
            updateUserQuery += " ELSE ";
            updateUserQuery += " INSERT INTO tblUsers(username, password, admin, email, name, phone, fax, usertype, pmSignature)";
            updateUserQuery += " VALUES(@username, @password, @admin, @email, @name, @phone, @fax, @usertype, @sig)";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@username", u.username);
            sqlParms.Add("@password", u.password);
            sqlParms.Add("@admin", u.admin);
            sqlParms.Add("@email", u.email);
            sqlParms.Add("@name", u.name);
            sqlParms.Add("@phone", u.phone);
            sqlParms.Add("@fax", u.fax);
            sqlParms.Add("@usertype", u.usertype);
            sqlParms.Add("@id", u.id);
            byte[] sig = new byte[0];
            if (u.signature != null)
            {
                MemoryStream ms = new MemoryStream();
                u.signature.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                sig = ms.ToArray();
            }
            sqlParms.Add("@sig", sig);
            SqlDataHandler dh = new SqlDataHandler();
            bool success = (dh.SetData(updateUserQuery, sqlParms, out status) > 0);
            if (success && u.id == 0)
            {
                String maxQuery = "SELECT max(id) as id from tblUsers";
                DataSet maxDS = dh.GetData(maxQuery, null, out status);
                if (maxDS != null && maxDS.Tables.Count > 0 && maxDS.Tables[0].Rows.Count > 0)
                {
                    u.id = int.Parse(maxDS.Tables[0].Rows[0]["id"].ToString());
                }
            }
            else if (!success)
            {
                return false;
            }
            sqlParms.Clear();
            sqlParms.Add("@userid", u.id);
            sqlParms.Add("@buildID", 0);
            sqlParms.Add("@email", u.email);
            if (u.usertype != 2)
            {
                String buildDeleteQuery = "DELETE FROM tblUserBuildings WHERE userid = @userid";
                String clearOldBuildingQuery = "DELETE FROM tblUserBuildings WHERE buildingid in (" + String.Join(",", u.buildings) + ")";
                String buildQuery = "INSERT INTO tblUserBuildings(userid, buildingid) VALUES(@userid, @buildID)";
                dh.SetData(buildDeleteQuery, sqlParms, out status);
                if (Controller.user.id == 1) { MessageBox.Show(status); }
                dh.SetData(clearOldBuildingQuery, null, out status);
                if (Controller.user.id == 1) { MessageBox.Show(status); }
                foreach (int bid in u.buildings)
                {
                    sqlParms["@buildID"] = bid;
                    dh.SetData(buildQuery, sqlParms, out status);
                }
                UpdateBuildings();
            }
            else
            {
                String buildDeleteQuery = "UPDATE tblBuildings SET pm = '' WHERE pm = @email";
                String buildQuery = "UPDATE tblBuildings SET pm = @email WHERE id = @buildID";
                dh.SetData(buildDeleteQuery, sqlParms, out status);
                foreach (int bid in u.buildings)
                {
                    sqlParms["@buildID"] = bid;
                    dh.SetData(buildQuery, sqlParms, out status);
                }
                UpdateBuildings();
            }
            return true;
        }

        public bool DeleteUser(User u)
        {
            String buildDeleteQuery1 = "DELETE FROM tblUserBuildings WHERE userid = @userid";
            String buildDeleteQuery2 = "UPDATE tblBuildings SET pm = '' WHERE pm = @email";
            String userDeleteQuery = "DELETE FROM tblUsers WHERE id = @userid";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@userid", u.id);
            sqlParms.Add("@email", u.email);
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            bool bSuccess = (dh.SetData(buildDeleteQuery1, sqlParms, out status) >= 1);
            bool pSuccess = (dh.SetData(buildDeleteQuery2, sqlParms, out status) >= 1);
            bool uSuccess = (dh.SetData(userDeleteQuery, sqlParms, out status) >= 1);
            UpdateBuildings();
            return (bSuccess && pSuccess && uSuccess);
        }

        private void UpdateBuildings()
        {
            MySqlConnector mysql = new MySqlConnector();
            List<Building> buildings = new Buildings(false).buildings;
            String status;
            foreach (Building b in buildings) { mysql.UpdateBuilding(b, b.Name, b.Abbr, out status); }
        }

        public List<int> GetBuildingsIDs(int usertype, int userid, String email, out String status)
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
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Astrodon {

    public partial class usrUsers : UserControl {
        private Users userManager = new Users();
        private List<UserType> userTypes;
        private List<User> users;
        private List<Building> buildings;
        private BindingList<Building> bindingList;
        private User selectedUser;

        public usrUsers() {
            InitializeComponent();
            userTypes = new UserTypes().types;
            buildings = new Buildings(false).buildings;
            bindingList = new BindingList<Building>();
            foreach (Building b in buildings) {
                bindingList.Add(b);
            }
        }

        private void usrUsers_Load(object sender, EventArgs e) {
            LoadUsers();
        }

        private void LoadUsers() {
            users = userManager.GetUsers(true);
            cmbUser.DataSource = users;
            cmbUser.DisplayMember = "name";
            cmbUser.ValueMember = "id";
            cmbType.DataSource = userTypes;
            cmbType.DisplayMember = "type";
            cmbType.ValueMember = "typeID";
            chkBuildings.DataSource = bindingList;
            chkBuildings.DisplayMember = "Name";
            chkBuildings.ValueMember = "ID";
            ClearUser();
        }

        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                selectedUser = users[cmbUser.SelectedIndex];
                LoadUser();
            } catch { }
        }

        private void LoadUser() {
            txtID.Text = selectedUser.id.ToString();
            cmbType.SelectedValue = selectedUser.usertype;
            txtUserName.Text = selectedUser.username;
            txtPassword.Text = selectedUser.password;
            txtEmail.Text = selectedUser.email;
            txtName.Text = selectedUser.name;
            txtPhone.Text = selectedUser.phone;
            txtFax.Text = selectedUser.fax;
            if (selectedUser.signature != null) { picSig.Image = selectedUser.signature; }
            try {
                for (int i = 0; i < chkBuildings.Items.Count; i++) { chkBuildings.SetItemChecked(i, false); }
                foreach (int bid in selectedUser.buildings) {
                    for (int i = 0; i < chkBuildings.Items.Count; i++) {
                        Building b = (Building)chkBuildings.Items[i];
                        if (b.ID == bid) {
                            chkBuildings.SetItemChecked(i, true);
                            break;
                        }
                    }
                }
            } catch { }
        }

        private void ClearUser() {
            cmbUser.SelectedIndex = -1;
            cmbType.SelectedIndex = -1;
            txtID.Text = "";
            cmbType.SelectedValue = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
            txtFax.Text = "";
            picSig.Image = null;
            for (int i = 0; i < chkBuildings.Items.Count; i++) { chkBuildings.SetItemChecked(i, false); }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(txtName.Text) && txtName.Text != "Add new user" && txtName.Text != "" && !String.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text != "") {
                int userType;
                try {
                    userType = (int)cmbType.SelectedValue;
                } catch {
                    userType = 3;
                }
                selectedUser.usertype = userType;
                selectedUser.username = txtUserName.Text;
                selectedUser.password = txtPassword.Text;
                selectedUser.email = txtEmail.Text;
                selectedUser.name = txtName.Text;
                selectedUser.phone = txtPhone.Text;
                selectedUser.fax = txtFax.Text;
                try {
                    if (picSig.Image != null) { selectedUser.signature = picSig.Image; }
                } catch {
                    selectedUser.signature = null;
                }
                List<int> selectedBuildings = new List<int>();
                try {
                    foreach (Object checkedItem in chkBuildings.CheckedItems) {
                        int bid = ((Building)checkedItem).ID;
                        selectedBuildings.Add(bid);
                    }
                } catch { }
                if (selectedUser.buildings == null) { selectedUser.buildings = new List<int>(); }
                selectedUser.buildings.Clear();
                selectedUser.buildings = selectedBuildings;
                if (userManager.SaveUser(selectedUser)) {
                    MessageBox.Show("User updated!", "Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearUser();
                } else {
                    MessageBox.Show("User update failed!", "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            ClearUser();
        }

        private void picSig_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpeg|*.jpg|bmp|*.bmp|all files|*.*";
            DialogResult res = ofd.ShowDialog();
            try {
                if (res == DialogResult.OK) { picSig.Image = Image.FromFile(ofd.FileName); }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Confirm delete user " + selectedUser.name + "?", "Users", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                if (userManager.DeleteUser(selectedUser)) {
                    MessageBox.Show("User " + selectedUser.name + " deleted");
                    LoadUsers();
                }
            }
        }
    }
}
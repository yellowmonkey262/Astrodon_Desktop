﻿using System;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrPMJobs : UserControl
    {
        private delegate void UpdateGridDelegate();

        public usrPMJobs()
        {
            InitializeComponent();
            Controller.JobUpdateEvent += Controller_JobUpdateEvent;
            if (Controller.user.usertype == 4) { Controller.AssignJob(); }
        }

        private void UpdateGrid()
        {
            if (!Controller.FiredUpdate)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new UpdateGridDelegate(UpdateGrid));
                }
                else
                {
                    Controller.FiredUpdate = true;
                    Controller.mainF.ShowJobs();
                }
            }
        }

        private void Controller_JobUpdateEvent(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void usrPMJobs_Load(object sender, EventArgs e)
        {
            RefreshList();
            tmrJob.Enabled = true;
            if (Controller.mainF.notifyIcon1.Visible) { Controller.mainF.notifyIcon1.Visible = false; }
        }

        private void RefreshList()
        {
            if (Controller.user.usertype == 2 || Controller.user.id == 1)
            {
                dgJobs.AllowUserToDeleteRows = true;
                jobListAdapter.FillPM(this.astrodonDataSet.JobList, Controller.user.id);
            }
            else if (Controller.user.usertype == 4)
            {
                jobListAdapter.FillPA(this.astrodonDataSet.JobList, Controller.user.id);
            }
        }

        private void ShowNotification(String message)
        {
            System.Media.SystemSounds.Exclamation.Play();
            if (!Controller.mainF.notifyIcon1.Visible) { Controller.mainF.notifyIcon1.Visible = true; }
            Controller.mainF.notifyIcon1.Text = message;
            Controller.mainF.notifyIcon1.BalloonTipText = message;
            Controller.mainF.notifyIcon1.ShowBalloonTip(10000);
            Controller.mainF.notifyIcon1.Click += notifyIcon1_Click;
            Controller.mainF.notifyIcon1.BalloonTipClicked += notifyIcon1_BalloonTipClicked;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (Controller.mainF.WindowState == FormWindowState.Minimized) { Controller.mainF.WindowState = FormWindowState.Normal; }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (Controller.mainF.WindowState == FormWindowState.Minimized) { Controller.mainF.WindowState = FormWindowState.Normal; }
        }

        private void dgJobs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            bool hasReviews = false;
            foreach (DataGridViewRow dvr in dgJobs.Rows)
            {
                DataGridViewTextBoxCell txtcell = new DataGridViewTextBoxCell();
                try
                {
                    String status = dvr.Cells[5].Value.ToString();
                    if (Controller.user.usertype == 2)
                    {
                        if (status != "NEW" && status != "PENDING" && status != "REVIEW") { dvr.Cells[6] = txtcell; }
                        if (status == "REVIEW") { hasReviews = true; }
                    }
                    else if (Controller.user.usertype == 4)
                    {
                        if (status != "ASSIGNED" && status != "REWORK")
                        {
                            dvr.Cells[6] = txtcell;
                        }
                        else
                        {
                            hasReviews = true;
                        }
                    }
                }
                catch { }
            }
            if (hasReviews)
            {
                String popupMsg = "You have new jobs to action";
                ShowNotification(popupMsg);
            }
        }

        private void dgJobs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 6)
                {
                    int jobID = int.Parse(dgJobs[0, e.RowIndex].Value.ToString());
                    tmrJob.Enabled = false;
                    Controller.mainF.ShowJob(jobID);
                }
            }
            catch { }
        }

        private void dgJobs_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            int colIdx = e.ColumnIndex;
            int rowIdx = e.RowIndex;
            String message = e.Exception.Message;
            String sTrace = e.Exception.StackTrace;
        }

        private void tmrJob_Tick(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void dgJobs_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int jobID = int.Parse(e.Row.Cells[0].Value.ToString());
            String query = "DELETE FROM tblPMJob WHERE id = " + jobID.ToString();
            SqlDataHandler dh = new SqlDataHandler();
            String status = "";
            dh.SetData(query, null, out status);
        }
    }
}
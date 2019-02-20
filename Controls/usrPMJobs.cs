using System;
using System.Windows.Forms;
using System.Linq;

namespace Astrodon.Controls
{
    public partial class usrPMJobs : UserControl
    {
        private delegate void UpdateGridDelegate();

        public usrPMJobs()
        {
            InitializeComponent();
            Controller.JobUpdateEvent += Controller_JobUpdateEvent;
            if (Controller.user.usertype == 4)
            {
                Controller.AssignJob();
            }
            dgJobs.Columns[7].Visible = Controller.UserIsSheldon();
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
            if (Controller.user.usertype == 2 || Controller.user.id == 1 || Controller.user.SubmitLettersForReview)
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
                    if (Controller.user.usertype == 2 || Controller.user.SubmitLettersForReview)
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
                catch (Exception ex)
                {
                    Controller.HandleError(ex);
                }
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
                if (e.RowIndex >= 0)
                {
                    int jobID = int.Parse(dgJobs[0, e.RowIndex].Value.ToString());
                    if (e.ColumnIndex == 6)
                    {
                        tmrJob.Enabled = false;
                        Controller.mainF.ShowJob(jobID);
                    }else
                    {
                        DeleteJob(jobID);
                    }
                }
            }
            catch { }
        }

        private void DeleteJob(int jobID)
        {
            if (Controller.UserIsSheldon())
            {
                if(Controller.AskQuestion("Are you sure you want to delete job " + jobID.ToString()))
                {
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var job = context.tblJobs.Single(a => a.id == jobID);
                    
                        var jobAttachments = context.tblJobAttachments.Where(a => a.jobID == jobID).ToList();
                        if(jobAttachments.Count > 0)
                          context.tblJobAttachments.RemoveRange(jobAttachments);

                        var jobCustomers = context.tblJobCustomers.Where(a => a.jobID == jobID).ToList();
                        if (jobCustomers.Count > 0)
                            context.tblJobCustomers.RemoveRange(jobCustomers);

                        var jobStatus = context.tblJobStatus.Where(a => a.jobID == jobID).ToList();
                        if (jobCustomers.Count > 0)
                            context.tblJobStatus.RemoveRange(jobStatus);

                        context.tblJobs.Remove(job);

                        context.SaveChanges();
                        Controller.ShowMessage("Job deleted.");
                    }
                }
            }
            else
            {
                Controller.HandleError("You are not allowed to delete jobs");
            }
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Astrodon.Forms
{
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void AddMessage(string message)
        {
            tbMessage.Text =DateTime.Now.ToString()+ ": "+ message + Environment.NewLine + tbMessage.Text;
            Application.DoEvents();
        }

        public void ProcessComplete()
        {
            button1.Enabled = true;

            MessageBox.Show("Process Complete");
        }

        public static frmProgress ShowForm()
        {
            var frm = new frmProgress();
            frm.Show();
            return frm;
        }
    }
}

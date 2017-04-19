using Astro.Library.Entities;
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
    public partial class frmReqTrans : Form
    {
        private List<Trns> Transactions = null;

        public frmReqTrans()
        {
            InitializeComponent();
            Transactions = new List<Trns>();
        }

        public frmReqTrans(List<Trns> aTransactions)
        {
            InitializeComponent();
            Transactions = aTransactions;
        }

        private void frmReqTrans_Load(object sender, EventArgs e)
        {
            dgTrans.DataSource = Transactions;
        }
    }
}
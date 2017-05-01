using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using Astro.Library.Entities;

namespace Astrodon.Controls
{
    public partial class usrPaidRequisitions : UserControl
    {
        #region Variables

        private List<Building> allBuildings;
        private List<Building> rBuildings;
        private BindingList<RequisitionList> unProcessedRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> unPaidRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> paidRequisitions = new BindingList<RequisitionList>();
        private SqlDataHandler dh;
        private String status;

        #endregion Variables

        public usrPaidRequisitions()
        {
            InitializeComponent();
            dh = new SqlDataHandler();
        }

        private void usrPaidRequisitions_Load(object sender, EventArgs e)
        {
            dgUnprocessed.DataSource = unProcessedRequisitions;
            dgUnpaid.DataSource = unPaidRequisitions;
            dgPaid.DataSource = paidRequisitions;
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            rBuildings = new List<Building>();
            allBuildings = new Buildings(false).buildings;
            List<int> bids = getReqBuildings();
            foreach (int bid in bids)
            {
                foreach (Building b in allBuildings)
                {
                    if (bid == b.ID)
                    {
                        rBuildings.Add(b);
                        break;
                    }
                }
            }
            rBuildings = rBuildings.OrderByDescending(c => c.Name).ToList();
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.DataSource = rBuildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.SelectedItem = null;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private List<int> getReqBuildings()
        {
            String query = "SELECT DISTINCT b.id FROM tblRequisition AS r INNER JOIN tblBuildings AS b ON r.building = b.id";
            DataSet dsB = dh.GetData(query, null, out status);
            List<int> myBuildings = new List<int>();
            if (dsB != null && dsB.Tables.Count > 0 && dsB.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsB.Tables[0].Rows)
                {
                    myBuildings.Add(int.Parse(dr["id"].ToString()));
                }
            }
            return myBuildings;
        }

        private void LoadRequisitions()
        {
            String buildingID = rBuildings[cmbBuilding.SelectedIndex].ID.ToString();
            String query = "SELECT r.id, r.trnDate, b.Building, r.account, r.processed, r.paid, r.reference, r.contractor, r.payreference, r.amount, r.ledger, b.acc, b.ownbank, b.datapath";
            query += " FROM tblRequisition AS r INNER JOIN tblBuildings AS b ON r.building = b.id";
            query += " WHERE b.id = " + buildingID + " ORDER BY trnDate";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            DataSet dsRequisitions = dh.GetData(query, null, out status);
            unProcessedRequisitions.Clear();
            unPaidRequisitions.Clear();
            paidRequisitions.Clear();

            if (dsRequisitions != null && dsRequisitions.Tables.Count > 0 && dsRequisitions.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRequisitions.Tables[0].Rows)
                {
                    RequisitionList r = new RequisitionList()
                    {
                        ID = Convert.ToString(dr["id"]),
                        trnDate = Convert.ToDateTime(dr["trnDate"]),
                        building = Convert.ToString(dr["Building"]),
                        account = Convert.ToString(dr["account"]),
                        reference = Convert.ToString(dr["reference"]),
                        payreference = Convert.ToString(dr["payreference"]),
                        amount = Convert.ToDouble(dr["amount"]),
                        ledger = Convert.ToString(dr["ledger"]),
                        paid = Convert.ToBoolean(dr["paid"]),
                    };
                    bool paid = r.paid;
                    bool matched = false;
                    bool processed = Convert.ToBoolean(dr["processed"]);
                    //handled by a batch process now

                    //String ledger = r.ledger.Split(new String[] { ":" }, StringSplitOptions.None)[0];
                    //if (r.account.ToUpper() == "TRUST" && !paid)
                    //{
                    //    matched = GetTransactions(Utilities.GetTrustPath(), r.trnDate, dr["acc"].ToString(), ledger, r.amount);
                    //}
                    //else if (!paid)
                    //{
                    //    matched = GetTransactions(Convert.ToString(dr["datapath"]), r.trnDate, Convert.ToString(dr["ownbank"]), ledger, r.amount * -1);
                    //}
                    if (!processed)
                    {
                        unProcessedRequisitions.Add(r);
                    }
                    else if (!matched && !paid)
                    {
                        unPaidRequisitions.Add(r);
                    }
                  
                }
            }
        }

        private bool GetTransactions(String path, DateTime reqDate, String acc, String ledger, double amt)
        {
            bool matched = false;
            List<Trns> transactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc.Replace("/", "")).OrderByDescending(c => c.Date).ToList();
            reqDate = new DateTime(reqDate.Year, reqDate.Month, reqDate.Day, 0, 0, 0);
            foreach (Trns transaction in transactions)
            {
                DateTime trnDate = DateTime.Parse(transaction.Date);
                ledger = (transaction.Reference.Trim().Length > 0 && transaction.Reference.Trim().Length < ledger.Length ? ledger.Substring(0, transaction.Reference.Trim().Length) : ledger);
                if (trnDate >= reqDate.AddDays(-1) && trnDate <= reqDate.AddDays(2) && Convert.ToDouble(transaction.Amount) == amt)
                {
                    matched = true;
                    break;
                }
            }
            return matched;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedItem != null) { LoadRequisitions(); }
        }
    }
}
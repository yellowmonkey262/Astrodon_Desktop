using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class frmClearance : Form
    {
        private PDF pdf = new PDF();
        private SqlDataHandler dh = new SqlDataHandler();
        private ClearanceValues values = new ClearanceValues();
        private List<Building> buildings;

        public frmClearance()
        {
            InitializeComponent();
        }

        private void frmClearance_Load(object sender, EventArgs e)
        {
       
            buildings = new Buildings(false).buildings;
            ReloadGrids();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //0 = id; 4 = processed
            PDF pdf = new PDF();
            List<String> attachments = new List<string>();
            int proc = 0;
            foreach (DataGridViewRow dvr in dgUnprocessed.Rows)
            {
                if (bool.Parse(dvr.Cells[4].Value.ToString()))
                {
                    bool journal = bool.Parse(dvr.Cells[5].Value.ToString());
                    String fileName = String.Empty;
                    int id = int.Parse(dvr.Cells[0].Value.ToString());
                    if (Environment.MachineName != "STEPHEN-PC" && journal) { ProcessJournals(id); }
                    if (pdf.Create(id, out fileName))
                    {
                        attachments.Add(fileName);
                    }
                    proc++;
                }
            }
            MessageBox.Show(proc.ToString() + " clearances processed");
            if (attachments.Count > 0 && !String.IsNullOrEmpty(Controller.user.email))
            {
                String fromAddress = Controller.user.email;
                String toAddress = Controller.user.email;
                String status = String.Empty;
                if (!Email.EmailProvider.SendClearanceCertificate(fromAddress, toAddress, attachments.ToArray()))
                    MessageBox.Show("Error sending clearance certificate");
                else
                    MessageBox.Show("Certificates sent");
            }
            else if (attachments.Count == 0)
            {
                MessageBox.Show("No certificates to be sent!");
            }
            else
            {
                MessageBox.Show("No email address to send certificates to!");
            }
            tblClearancesTableAdapter1.Update(this.clearances);
            ReloadGrids();
        }

        private void ProcessJournals(int clearanceID)
        {
            ClearanceObject clr = GetClearance(clearanceID);
            //Recon split Seller/Buyer date reconciliation
            bool hasSplit = false;
            String splitDesc = String.Empty;
            double splitFee = 0;
            foreach (ClearanceObjectTrans clrT in clr.Trans)
            {
                if (clrT.description == "Recon split Seller/Buyer date reconciliation")
                {
                    hasSplit = true;
                    splitDesc = "Recon split Seller/Buyer date reconciliation";
                    splitFee = clrT.amount;
                    break;
                }
            }
            String docType = "Clearance " + clr.validTo.ToString("yyyy/MM/dd");
            Building building = null;
            foreach (Building b in buildings)
            {
                if (b.Abbr == clr.buildingCode)
                {
                    building = b;
                    break;
                }
            }
            if (building != null)
            {
                DateTime trnDate = clr.certDate;
                String pastelReturn, pastelString;

                pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, values.centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, clr.customerCode, building.Centrec_Building,
                    building.Centrec_Building, docType, docType, clr.clearanceFee.ToString(), "5500/000", "", out pastelString);
                Controller.pastel.PostBusGBatch(trnDate, 5, "5500000", clr.customerCode, docType, docType, clr.clearanceFee.ToString("#0.00"));
                if (hasSplit)
                {
                    pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, values.centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, clr.customerCode, building.Centrec_Building,
                        building.Centrec_Building, splitDesc, splitDesc, splitFee.ToString(), "5500/000", "", out pastelString);
                    Controller.pastel.PostBusGBatch(trnDate, 5, "5500000", clr.customerCode, splitDesc, splitDesc, splitFee.ToString("#0.00"));
                }
            }
        }

        private ClearanceObject GetClearance(int id)
        {
            String query1 = "SELECT id, buildingCode, customerCode, preparedBy, trfAttorneys, attReference, fax, certDate, complex, unitNo, seller, purchaser, purchaserAddress, purchaserTel, purchaserEmail, ";
            query1 += " regDate, notes, clearanceFee, astrodonTotal, validDate, processed, registered, extClearance FROM tblClearances WHERE (id = " + id.ToString() + ")";
            String status = String.Empty;
            DataSet ds = dh.GetData(query1, null, out status);
            DataRow dr = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) { dr = ds.Tables[0].Rows[0]; }
            String query2 = "SELECT description, amount FROM tblClearanceTransactions WHERE clearanceID = " + id.ToString();
            DataSet ds2 = dh.GetData(query2, null, out status);
            if (dr != null) { return new ClearanceObject(id, dr, ds2); } else { return null; }
        }

        private int clickedRow = -1;

        private void dgProcessed_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                clickedRow = e.RowIndex;
                ContextMenu m = new ContextMenu();
                MenuItem deleteMe = new MenuItem("Delete Clearance");
                MenuItem deleteMeNoJournal = new MenuItem("Delete Clearance without journal");
                MenuItem viewMe = new MenuItem("View Clearance");
                deleteMe.Click += new EventHandler(deleteMe_Click);
                deleteMeNoJournal.Click += new EventHandler(deleteMeNoJournal_Click);
                viewMe.Click += new EventHandler(viewMe_Click);
                m.MenuItems.Add(deleteMe);
                m.MenuItems.Add(deleteMeNoJournal);
                m.MenuItems.Add(viewMe);
                m.Show(dgProcessed, new Point(e.X, e.Y));
            }
        }

        private void viewMe_Click(object sender, EventArgs e)
        {
            if (clickedRow != -1)
            {
                PDF pdf = new PDF();
                DataGridViewRow dvr = dgProcessed.Rows[clickedRow];
                clickedRow = -1;
                String fileName = String.Empty;
                int id = int.Parse(dvr.Cells[0].Value.ToString());
                if (pdf.Create(id, out fileName)) { Process.Start(fileName); }
            }
        }

        private void deleteMeNoJournal_Click(object sender, EventArgs e)
        {
            DeleteRow(false);
        }

        private void deleteMe_Click(object sender, EventArgs e)
        {
            DeleteRow(true);
        }

        private void DeleteRow(bool journal)
        {
            DataGridViewRow dvr = dgProcessed.Rows[clickedRow];
            clickedRow = -1;
            if (Environment.MachineName != "STEPHEN-PC" && journal)
            {
                int id = int.Parse(dvr.Cells[0].Value.ToString());
                ClearanceObject clr = GetClearance(id);
                String docType = "Clearance " + clr.certDate.ToString("yyyy/MM/dd");
                Building building = null;
                foreach (Building b in buildings)
                {
                    if (b.Abbr == clr.buildingCode)
                    {
                        building = b;
                        break;
                    }
                }
                if (building != null)
                {
                    DateTime trnDate = clr.certDate;
                    String pastelReturn, pastelString;
                    if (clr.extClearance)
                    {
                        pastelReturn = Controller.pastel.PostReverseBatch(trnDate, building.Period, values.centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, clr.customerCode,
                            building.Centrec_Building, building.Centrec_Building, docType, docType, "-390.00", "5500/000", "", out pastelString);
                        Controller.pastel.PostBusGBatch(trnDate, 5, "5500000", clr.customerCode, docType, docType, "-390.00");
                    }
                    pastelReturn = Controller.pastel.PostReverseBatch(trnDate, building.Period, values.centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, clr.customerCode,
                        building.Centrec_Building, building.Centrec_Building, docType, docType, (clr.clearanceFee * -1).ToString(), "5500/000", "", out pastelString);
                    MessageBox.Show(pastelReturn);
                    Controller.pastel.PostBusGBatch(trnDate, 5, "5500000", clr.customerCode, docType, docType, (clr.clearanceFee * -1).ToString("#0.00"));
                }
                else
                {
                    MessageBox.Show("no building");
                }
            }
            dgProcessed.Rows.Remove(dvr);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (frmClearances clearances = new frmClearances(0))
            {
                clearances.ShowDialog();
                ReloadGrids();
            }
        }

        private void btnUpdateProcessed_Click(object sender, EventArgs e)
        {
            List<String> attachments = new List<string>();
            foreach (DataGridViewRow dvr in dgProcessed.Rows)
            {
                if (bool.Parse(dvr.Cells[6].Value.ToString()))
                {
                    int clearanceID = int.Parse(dvr.Cells[0].Value.ToString());
                    bool bc = bool.Parse(dvr.Cells[7].Value.ToString());
                    bool hoa = bool.Parse(dvr.Cells[8].Value.ToString());
                    String bcCert, bcLetter, hoaCert, hoaLetter;
                    bcCert = String.Empty;
                    bcLetter = String.Empty;
                    hoaCert = String.Empty;
                    hoaLetter = String.Empty;
                    if (bc)
                    {
                        pdf.CreateBCCertificate(clearanceID, out bcCert);
                        pdf.CreateBCLetter(clearanceID, out bcLetter);
                    }
                    if (hoa)
                    {
                        pdf.CreateHOACertificate(clearanceID, out hoaCert);
                        pdf.CreateHOALetter(clearanceID, out hoaLetter);
                    }
                    if (!String.IsNullOrEmpty(bcCert)) { attachments.Add(bcCert); }
                    if (!String.IsNullOrEmpty(bcLetter)) { attachments.Add(bcLetter); }
                    if (!String.IsNullOrEmpty(hoaCert)) { attachments.Add(hoaCert); }
                    if (!String.IsNullOrEmpty(hoaLetter)) { attachments.Add(hoaLetter); }
                }
            }
            if (attachments.Count > 0 && !String.IsNullOrEmpty(Controller.user.email))
            {
                String fromAddress = Controller.user.email;
                String toAddress = Controller.user.email;
                String status = String.Empty;
                if (!Email.EmailProvider.SendClearanceCertificate(fromAddress, toAddress, attachments.ToArray()))
                {
                    MessageBox.Show("Error sending email");
                }
                else
                  MessageBox.Show("Certificates sent");

              
            }
            else if (attachments.Count == 0)
            {
                MessageBox.Show("No documents to be sent!");
            }
            else
            {
                MessageBox.Show("No email address to send documents to!");
            }

            tblClearances1TableAdapter.Update(this.astrodonDataSet1);
            ReloadGrids();
        }

        private void ReloadGrids()
        {
            this.tblClearancesTableAdapter1.Fill(this.clearances.tblClearances, false);
            this.tblClearancesTableAdapter.Fill(this.astrodonDataSet1.tblClearances, false);
            this.tblClearances1TableAdapter.Fill(this.astrodonDataSet1.tblClearances1);
        }

        private void dgUnprocessed_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int clearanceID = int.Parse(dgUnprocessed.Rows[e.RowIndex].Cells[0].Value.ToString());
                using (frmClearances clearances = new frmClearances(clearanceID))
                {
                    clearances.ShowDialog();
                    ReloadGrids();
                }
            }
        }

        private void dgProcessed_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon {

    public partial class usrConfig : UserControl {
        private CashDepositFees fees;
        private String settingsQuery;
        private String updateSettingsQuery;
        private String status = String.Empty;
        private SqlDataHandler dh = new SqlDataHandler();

        public usrConfig() {
            InitializeComponent();
            fees = new CashDepositFees();
        }

        private void usrConfig_Load(object sender, EventArgs e) {
            settingsQuery = "SELECT minbal, reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee, clearance, ex_clearance, recon_split, debit_order, ret_debit_order, ";
            settingsQuery += " eft_fee, monthly_journal, trust, centrec, business, rental FROM tblSettings";
            updateSettingsQuery = "UPDATE tblSettings SET minbal = @minbal, reminder_fee = @rem, final_fee = @fd, summons_fee = @summons, discon_notice_fee = @dcn, discon_fee = @dc, ";
            updateSettingsQuery += " handover_fee = @ho, clearance = @cl, ex_clearance = @excl, recon_split = @crs, debit_order = @do, ret_debit_order = @rdo, eft_fee = @eff, ";
            updateSettingsQuery += " monthly_journal = @mbj, trust = @trust, centrec = @centrec, business = @business, rental = @rental";
            LoadSettings();
        }

        private void LoadSettings() {
            DataSet ds = dh.GetData(settingsQuery, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                DataRow dr = ds.Tables[0].Rows[0];
                txtMinBalance.Text = dr["minbal"].ToString();
                txtRemFee.Text = dr["reminder_fee"].ToString();
                txtFinalFee.Text = dr["final_fee"].ToString();
                txtSummonsFee.Text = dr["summons_fee"].ToString();
                txtDisconNoticeFee.Text = dr["discon_notice_fee"].ToString();
                txtDisconFee.Text = dr["discon_fee"].ToString();
                txtHandoverFee.Text = dr["handover_fee"].ToString();
                txtClearanceFee.Text = dr["clearance"].ToString();
                txtExClearanceFee.Text = dr["ex_clearance"].ToString();
                txtRecon.Text = dr["recon_split"].ToString();
                txtDebit.Text = dr["debit_order"].ToString();
                txtRetDebit.Text = dr["ret_debit_order"].ToString();
                txtEFT.Text = dr["eft_fee"].ToString();
                txtMonthBuild.Text = dr["monthly_journal"].ToString();
                txtTrust.Text = dr["trust"].ToString();
                txtCentrec.Text = dr["centrec"].ToString();
                txtBusiness.Text = dr["business"].ToString();
                txtRental.Text = dr["rental"].ToString();
            }
            dgCashDepositFee.DataSource = fees.fees;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@minbal", txtMinBalance.Text);
            sqlParms.Add("@rem", txtRemFee.Text);
            sqlParms.Add("@fd", txtFinalFee.Text);
            sqlParms.Add("@summons", txtSummonsFee.Text);
            sqlParms.Add("@dcn", txtDisconNoticeFee.Text);
            sqlParms.Add("@dc", txtDisconFee.Text);
            sqlParms.Add("@ho", txtHandoverFee.Text);
            sqlParms.Add("@cl", txtClearanceFee.Text);
            sqlParms.Add("@excl", txtExClearanceFee.Text);
            sqlParms.Add("@crs", txtRecon.Text);
            sqlParms.Add("@do", txtDebit.Text);
            sqlParms.Add("@rdo", txtRetDebit.Text);
            sqlParms.Add("@eff", txtEFT.Text);
            sqlParms.Add("@mbj", txtMonthBuild.Text);
            sqlParms.Add("@trust", txtTrust.Text);
            sqlParms.Add("@centrec", txtCentrec.Text);
            sqlParms.Add("@business", txtBusiness.Text);
            sqlParms.Add("@rental", txtRental.Text);
            dh.SetData(updateSettingsQuery, sqlParms, out status);
            fees.Update();
            fees = new CashDepositFees();
            if (status == "") {
                MessageBox.Show("Settings Updated!", "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSettings();
            } else {
                MessageBox.Show("Settings Update failed: " + status, "Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            LoadSettings();
        }
    }
}
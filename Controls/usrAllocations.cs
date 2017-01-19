using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon {

    public partial class usrAllocations : UserControl {
        private List<AllocatedStatements> allocatedStatements;
        private List<Building> buildings;
        private SqlDataHandler dh;

        private String status;

        public usrAllocations() {
            InitializeComponent();
            dh = new SqlDataHandler();
            buildings = new Buildings(false).buildings;
        }

        private void usrAllocations_Load(object sender, EventArgs e) {
            GetAllocated();
        }

        private void GetAllocated() {
            allocatedStatements = new List<AllocatedStatements>();
            String SelectCommand = "SELECT id, lid, trnDate, amount, building, code, description, reference, accnumber, contra, datapath FROM tblExport ORDER BY lid";
            DataSet dsAllocated = dh.GetData(SelectCommand, null, out status);
            if (dsAllocated != null && dsAllocated.Tables.Count > 0 && dsAllocated.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in dsAllocated.Tables[0].Rows) {
                    AllocatedStatements ast = new AllocatedStatements();
                    ast.id = dr["id"].ToString();
                    ast.lid = dr["lid"].ToString();
                    ast.trnDate = dr["trnDate"].ToString();
                    ast.amount = dr["amount"].ToString();
                    ast.building = dr["building"].ToString();//name
                    ast.code = dr["code"].ToString();//abbr
                    ast.description = dr["description"].ToString();
                    ast.reference = dr["reference"].ToString();
                    ast.accnumber = dr["accnumber"].ToString(); //trust
                    ast.contra = dr["contra"].ToString(); //cash book
                    ast.datapath = dr["datapath"].ToString();
                    if (!allocatedStatements.Contains(ast)) { allocatedStatements.Add(ast); }
                }
            }
            colBuilding.DisplayMember = "Name";
            colBuilding.ValueMember = "Name";
            colBuilding.DataSource = buildings;
            dgAllocated.DataSource = allocatedStatements;
        }

        private void btnUpdateAllocated_Click(object sender, EventArgs e) {
            String UpdateCommand = "UPDATE tblExport SET building = @building, code = @code, reference = @reference, accnumber = @accnumber, contra = @contra, datapath = @datapath WHERE id = @id";
            String loadExport1 = "UPDATE d SET d.building = b.id FROM tblDevision d JOIN tblBuildings b ON d.building = b.code or d.Building = b.building;";
            dh.SetData(loadExport1, null, out status);
            foreach (AllocatedStatements ast in allocatedStatements) {
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@building", ast.building);
                sqlParms.Add("@code", ast.code);
                sqlParms.Add("@reference", ast.reference);
                sqlParms.Add("@accnumber", ast.accnumber);
                sqlParms.Add("@contra", ast.contra);
                sqlParms.Add("@datapath", ast.datapath);
                sqlParms.Add("@id", ast.id);
                dh.SetData(UpdateCommand, sqlParms, out status);

                if (ast.code == "UNA") {
                    String sql = " INSERT INTO tblUnallocated(lid, trnDate, amount, building, code, description, reference, accnumber, contra, datapath, period) ";
                    sql += " SELECT lid, trnDate, amount, building, code, description, reference, accnumber, contra, datapath, period FROM tblExport WHERE id = " + ast.id + " AND lid NOT IN (SELECT lid FROM tblUnallocated);";
                    sql += " UPDATE tblLedgerTransactions SET allocate = 'False' WHERE id = (SELECT lid FROM tblExport WHERE id = " + ast.id + ");";
                    dh.SetData(sql, null, out status);
                }

                String str = " update div set div.Building = exo.building, div.AccNumber = exo.accnumber, div.Reference = exo.reference";
                str += " from tblDevision as div inner join tblExport exo on div.lid = exo.lid where exo.id = " + ast.id;
                dh.SetData(str, null, out status);
                dh.SetData(loadExport1, null, out status);
            }
        }

        private void dgAllocated_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            ComboBox cb = e.Control as ComboBox;
            if (cb != null) {
                cb.SelectedIndexChanged -= cb_SelectedIndexChanged;
                cb.SelectedIndexChanged += cb_SelectedIndexChanged;
            }
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                ComboBox cmb = (sender as ComboBox);
                int idx = cmb.SelectedIndex;
                DataGridViewComboBoxEditingControl comboBox = (DataGridViewComboBoxEditingControl)sender;
                int rowIndex = comboBox.EditingControlRowIndex;
                dgAllocated.Rows[rowIndex].Cells["colCode"].Value = buildings[idx].Abbr;
                dgAllocated.Rows[rowIndex].Cells["colTrust"].Value = buildings[idx].Trust;
                dgAllocated.Rows[rowIndex].Cells["colPath"].Value = buildings[idx].DataPath;
            } catch {
            }
        }

        private void dgAllocated_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            DataGridView senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0) {
                DeleteAllocated(allocatedStatements[e.RowIndex].id);
                allocatedStatements.RemoveAt(e.RowIndex);
                dgAllocated.Refresh();
            }
        }

        private void DeleteAllocated(String id) {
            String deleteQuery = "DELETE FROM tblExport WHERE id = " + id;
            dh.SetData(deleteQuery, null, out status);
        }
    }

    public class AllocatedStatements {
        public String id { get; set; }

        public String lid { get; set; }

        public String trnDate { get; set; }

        public String amount { get; set; }

        public String building { get; set; }

        public String code { get; set; }

        public String description { get; set; }

        public String reference { get; set; }

        public String accnumber { get; set; }

        public String contra { get; set; }

        public String datapath { get; set; }
    }
}
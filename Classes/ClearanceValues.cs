using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Classes
{
    public class ClearanceValues
    {
        public double clearanceFee { get; set; }

        public double exClearanceFee { get; set; }

        public double splitFee { get; set; }

        public String centrec { get; set; }

        public String business { get; set; }

        public ClearanceValues()
        {
            String query = "SELECT clearance, ex_clearance, recon_split, centrec, business FROM tblSettings";
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                clearanceFee = double.Parse(dr["clearance"].ToString());
                exClearanceFee = double.Parse(dr["ex_clearance"].ToString());
                splitFee = double.Parse(dr["recon_split"].ToString());
                centrec = dr["centrec"].ToString();
                business = dr["business"].ToString();
            }
        }
    }
}
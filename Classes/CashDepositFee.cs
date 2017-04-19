using System;
using System.Collections.Generic;
using System.Data;
using Astro.Library.Entities;

namespace Astrodon
{
    public class CashDepositFees
    {
        private String depQuery = "SELECT id, min, max, amount FROM tblCashDeposits ORDER BY min";
        private String depUpdateQuery = "UPDATE tblCashDeposits SET min = @min, max = @max, amount = @amt WHERE id = @id";
        private String status = String.Empty;
        public List<CashDepositFee> fees = new List<CashDepositFee>();

        public CashDepositFees()
        {
            SqlDataHandler dh = new SqlDataHandler();
            DataSet ds = dh.GetData(depQuery, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CashDepositFee cdf = new CashDepositFee();
                    cdf.ID = int.Parse(dr["id"].ToString());
                    cdf.Min = double.Parse(dr["min"].ToString());
                    cdf.Max = double.Parse(dr["max"].ToString());
                    cdf.Amt = double.Parse(dr["amount"].ToString());
                    fees.Add(cdf);
                }
            }
        }

        public void Update()
        {
            SqlDataHandler dh = new SqlDataHandler();
            foreach (CashDepositFee cdf in fees)
            {
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@min", cdf.Min);
                sqlParms.Add("@max", cdf.Max);
                sqlParms.Add("@amt", cdf.Amt);
                sqlParms.Add("@id", cdf.ID);
                dh.SetData(depUpdateQuery, sqlParms, out status);
            }
        }
    }
}
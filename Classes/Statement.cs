using System;
using System.Collections.Generic;

namespace Astrodon
{
    public class Statements
    {
        public List<Statement> statements { get; set; }
    }

    public class Statement
    {
        public DateTime StmtDate { get; set; }

        public String DebtorEmail { get; set; }

        public String[] email1 { get; set; }

        public String email2 { get; set; }

        public bool PrintMe { get; set; }

        public bool EmailMe { get; set; }

        public String[] Address { get; set; }

        public String AccNo { get; set; }

        public String BankDetails { get; set; }

        public String BuildingName { get; set; }

        public String LevyMessage1 { get; set; }

        public String LevyMessage2 { get; set; }

        public String Message { get; set; }

        public List<Transaction> Transactions { get; set; }

        public double totalDue { get; set; }

        public String bankName { get; set; }

        public String accName { get; set; }

        public String branch { get; set; }

        public String accNumber { get; set; }

        public bool isStd { get; set; }

        public String pm { get; set; }
    }

    public class Transaction
    {
        public DateTime TrnDate { get; set; }

        public String Reference { get; set; }

        public String Description { get; set; }

        public double TrnAmt { get; set; }

        public double AccAmt { get; set; }
    }
}
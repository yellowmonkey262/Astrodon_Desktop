using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class Statements
    {
        public List<Statement> statements { get; set; }
    }

    public class Statement
    {
        public int BuildingId { get; set; }

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

        public String BankAccountNumber { get; set; }

        public bool isStd { get; set; }

        public String pm { get; set; }

        public bool IsInTransfer { get; set; }

        public byte[] InTransferLetter { get; set; }


        public bool IsRental
        {
            get
            {
                if (String.IsNullOrWhiteSpace(BuildingName))
                    return false;
                return BuildingName.Trim().ToUpper() == "ASTRODON RENTALS";
            }
        }
    }
}
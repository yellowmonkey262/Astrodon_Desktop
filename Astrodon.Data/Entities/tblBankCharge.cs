namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBankCharge
    {
        [Key]
        public int BankChargesId { get; set; }

        public decimal? CashDeposit { get; set; }

        public decimal? DebitOrder { get; set; }
    }
}

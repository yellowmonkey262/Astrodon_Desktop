namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblLedgerTransaction
    {
        public int id { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        public string Description { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Balance { get; set; }

        public string AccNumber { get; set; }

        public string AccDescription { get; set; }

        public string StatementNr { get; set; }

        [StringLength(50)]
        public string Allocate { get; set; }
    }
}

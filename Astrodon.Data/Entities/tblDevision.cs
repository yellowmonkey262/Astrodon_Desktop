namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDevision")]
    public partial class tblDevision
    {
        public int id { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        public string Description { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Balance { get; set; }

        [StringLength(50)]
        public string FromAccNumber { get; set; }

        public string AccDescription { get; set; }

        [StringLength(50)]
        public string StatementNr { get; set; }

        [StringLength(10)]
        public string Allocate { get; set; }

        public string Reference { get; set; }

        public string Building { get; set; }

        [StringLength(50)]
        public string AccNumber { get; set; }

        public bool posted { get; set; }

        public int? period { get; set; }

        public int? lid { get; set; }
    }
}

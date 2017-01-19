namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblClearanceTransaction
    {
        public int id { get; set; }

        public int clearanceID { get; set; }

        [Required]
        public string description { get; set; }

        public decimal qty { get; set; }

        public decimal rate { get; set; }

        public decimal markup { get; set; }

        public decimal amount { get; set; }
    }
}

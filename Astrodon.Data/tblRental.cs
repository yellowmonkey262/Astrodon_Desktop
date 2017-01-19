namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRental
    {
        public int id { get; set; }

        public DateTime? trnDate { get; set; }

        [StringLength(255)]
        public string reference { get; set; }

        public string description { get; set; }

        public decimal? drValue { get; set; }

        public decimal? crValue { get; set; }

        public decimal? cumValue { get; set; }
    }
}

namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblExport")]
    public partial class tblExport
    {
        public int id { get; set; }

        public int lid { get; set; }

        [StringLength(255)]
        public string trnDate { get; set; }

        public decimal? amount { get; set; }

        [StringLength(255)]
        public string building { get; set; }

        [StringLength(255)]
        public string code { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        [StringLength(255)]
        public string reference { get; set; }

        [StringLength(255)]
        public string accnumber { get; set; }

        [StringLength(255)]
        public string contra { get; set; }

        [StringLength(255)]
        public string datapath { get; set; }

        public int? period { get; set; }

        public bool trustpost { get; set; }

        public bool? buspost { get; set; }

        public bool? buildpost { get; set; }

        public bool? una { get; set; }
    }
}

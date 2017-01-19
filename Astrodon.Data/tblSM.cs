namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSMS")]
    public partial class tblSM
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string building { get; set; }

        [Required]
        [StringLength(50)]
        public string customer { get; set; }

        public decimal? currentBalance { get; set; }

        [StringLength(50)]
        public string smsType { get; set; }

        [StringLength(50)]
        public string number { get; set; }

        [StringLength(255)]
        public string reference { get; set; }

        [Required]
        public string message { get; set; }

        public bool billable { get; set; }

        public bool bulkbillable { get; set; }

        public DateTime sent { get; set; }

        [Required]
        public string sender { get; set; }

        public string astStatus { get; set; }

        public string batchID { get; set; }

        [Required]
        public string status { get; set; }

        public DateTime nextPolled { get; set; }

        public int pollCount { get; set; }

        [StringLength(50)]
        public string bsStatus { get; set; }
    }
}

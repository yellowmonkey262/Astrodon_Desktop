namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPMQueue")]
    public partial class tblPMQueue
    {
        public int id { get; set; }

        public int pm { get; set; }

        public DateTime createDate { get; set; }

        public int building { get; set; }

        [StringLength(50)]
        public string customer { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string message { get; set; }

        public int assigned { get; set; }

        public int currentStatus { get; set; }

        public bool emailMe { get; set; }
    }
}

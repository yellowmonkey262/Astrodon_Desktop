namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCustomerNote
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string customer { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string notes { get; set; }

        public DateTime noteDate { get; set; }
    }
}

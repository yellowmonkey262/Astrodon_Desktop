namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRentalRecon")]
    public partial class tblRentalRecon
    {
        public int id { get; set; }

        public int rentalId { get; set; }

        public DateTime trnDate { get; set; }

        public decimal value { get; set; }

        [Required]
        [StringLength(50)]
        public string account { get; set; }

        [Required]
        [StringLength(50)]
        public string contra { get; set; }

        public bool posted { get; set; }
    }
}

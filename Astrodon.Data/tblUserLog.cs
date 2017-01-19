namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblUserLog")]
    public partial class tblUserLog
    {
        public int id { get; set; }

        public int userid { get; set; }

        public int buildingid { get; set; }

        [Required]
        [StringLength(50)]
        public string customercode { get; set; }

        public DateTime trnDate { get; set; }

        public decimal amount { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string description { get; set; }
    }
}

namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblReminder
    {
        public int id { get; set; }

        public int userID { get; set; }

        [Required]
        [StringLength(50)]
        public string customer { get; set; }

        [Required]
        [StringLength(50)]
        public string building { get; set; }

        public DateTime remDate { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string remNote { get; set; }

        public bool action { get; set; }

        public DateTime? actionDate { get; set; }
    }
}

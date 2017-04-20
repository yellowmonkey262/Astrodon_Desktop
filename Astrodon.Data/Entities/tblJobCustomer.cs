namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblJobCustomer")]
    public partial class tblJobCustomer
    {
        public int id { get; set; }

        public int jobID { get; set; }

        [Required]
        [StringLength(50)]
        public string customerID { get; set; }
    }
}

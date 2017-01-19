namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPMQCustomer
    {
        public int id { get; set; }

        public int pmQID { get; set; }

        [Required]
        [StringLength(50)]
        public string customerCode { get; set; }
    }
}

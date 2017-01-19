namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPMQStatu
    {
        public int id { get; set; }

        public int pmQID { get; set; }

        public DateTime statusDate { get; set; }

        [Required]
        public string status { get; set; }

        [Column(TypeName = "ntext")]
        public string notes { get; set; }
    }
}

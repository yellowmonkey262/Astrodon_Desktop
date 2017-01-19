namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPMJobStatu
    {
        public int id { get; set; }

        public int jobID { get; set; }

        public int actioned { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        public DateTime actionDate { get; set; }
    }
}

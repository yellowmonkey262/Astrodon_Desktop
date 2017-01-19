namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblStatement
    {
        public int id { get; set; }

        [Required]
        public string building { get; set; }

        public DateTime lastProcessed { get; set; }
    }
}

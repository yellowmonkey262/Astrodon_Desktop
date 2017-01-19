namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMatch")]
    public partial class tblMatch
    {
        public int id { get; set; }

        [Required]
        public string statementRef { get; set; }

        [Required]
        public string astroRef { get; set; }
    }
}

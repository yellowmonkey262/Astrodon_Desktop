namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblTemplate
    {
        public int id { get; set; }

        public int buildingID { get; set; }

        [Required]
        public string templateName { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string templateContent { get; set; }
    }
}

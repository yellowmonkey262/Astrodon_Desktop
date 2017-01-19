namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblEmbedType")]
    public partial class tblEmbedType
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string attType { get; set; }
    }
}

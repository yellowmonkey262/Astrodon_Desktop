namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblJobAttachment
    {
        public int id { get; set; }

        public int jobID { get; set; }

        public int fileType { get; set; }

        [Required]
        [StringLength(50)]
        public string fileName { get; set; }

        public byte[] fileContent { get; set; }

        public string fileString { get; set; }

        public int creator { get; set; }

        public DateTime createDate { get; set; }

        public int attType { get; set; }
    }
}

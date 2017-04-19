namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAttachment
    {
        public int id { get; set; }

        public int jobID { get; set; }

        [Required]
        public string fileName { get; set; }

        [Column(TypeName = "ntext")]
        public string fileRTF { get; set; }

        public byte[] fileContent { get; set; }

        public int attachmentType { get; set; }

        public bool justify { get; set; }

        public bool letterhead { get; set; }

        public bool customer { get; set; }
    }
}

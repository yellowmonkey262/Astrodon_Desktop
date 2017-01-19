namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblEmail
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string customer { get; set; }

        public bool shouldBeSent { get; set; }

        public bool sent { get; set; }

        [Required]
        public string fromEmail { get; set; }

        [Required]
        public string toEmail { get; set; }

        [Required]
        public string subject { get; set; }

        public bool hasAttachment { get; set; }

        public DateTime sentDate { get; set; }
    }
}

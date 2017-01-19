namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPMCustomer
    {
        public int id { get; set; }

        public int jobID { get; set; }

        [Required]
        [StringLength(50)]
        public string account { get; set; }

        public bool sendMail1 { get; set; }

        public bool sendMail2 { get; set; }

        public bool sendMail3 { get; set; }

        public bool sendMail4 { get; set; }

        public bool sendSMS { get; set; }
    }
}

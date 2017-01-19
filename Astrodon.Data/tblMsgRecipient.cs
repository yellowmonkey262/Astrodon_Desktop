namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblMsgRecipient
    {
        public int id { get; set; }

        public int msgID { get; set; }

        [Required]
        [StringLength(50)]
        public string recipient { get; set; }

        [Required]
        [StringLength(50)]
        public string accNo { get; set; }

        public DateTime queueDate { get; set; }

        public DateTime? sentDate { get; set; }

        public bool billCustomer { get; set; }
    }
}

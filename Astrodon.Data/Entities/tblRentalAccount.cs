namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRentalAccount
    {
        public int id { get; set; }

        public string description { get; set; }

        [StringLength(50)]
        public string drAccount { get; set; }

        [StringLength(50)]
        public string drContra { get; set; }

        [StringLength(50)]
        public string crAccount { get; set; }

        [StringLength(50)]
        public string crContra { get; set; }
    }
}

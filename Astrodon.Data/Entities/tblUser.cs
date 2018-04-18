namespace Astrodon.Data
{
    using Calendar;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblUser
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        public bool admin { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string phone { get; set; }

        [StringLength(50)]
        public string fax { get; set; }

        public int usertype { get; set; }

        [Column(TypeName = "image")]
        public byte[] pmSignature { get; set; }

        public bool ProcessCheckLists { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<BuildingCalendarEntry> BuildingCalendarEntries { get; set; }

    }
}

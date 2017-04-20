namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblJobUpdate")]
    public partial class tblJobUpdate
    {
        public int id { get; set; }

        public DateTime pmLastUpdated { get; set; }

        public int pmID { get; set; }

        public DateTime paLastUpdated { get; set; }

        public int paID { get; set; }
    }
}

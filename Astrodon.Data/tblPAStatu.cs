namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPAStatu
    {
        public int id { get; set; }

        public int paID { get; set; }

        public bool paStatus { get; set; }

        public DateTime availableSince { get; set; }
    }
}

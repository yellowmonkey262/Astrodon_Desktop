namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRunConfig")]
    public partial class tblRunConfig
    {
        public int id { get; set; }

        public bool stmtRunStatus { get; set; }

        public bool letterRunStatus { get; set; }
    }
}

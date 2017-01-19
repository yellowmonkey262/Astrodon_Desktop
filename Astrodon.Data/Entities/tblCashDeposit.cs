namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCashDeposit
    {
        public int id { get; set; }

        public decimal? min { get; set; }

        public decimal? max { get; set; }

        public decimal? amount { get; set; }
    }
}

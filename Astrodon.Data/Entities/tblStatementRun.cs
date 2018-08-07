namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblStatementRun")]
    public partial class tblStatementRun
    {
        public int id { get; set; }

        [Required]
        public string email1 { get; set; }

        public DateTime? sentDate1 { get; set; }

        public DateTime queueDate { get; set; }

        [Required]
        public string fileName { get; set; }

        public string debtorEmail { get; set; }

        [Required]
        public string unit { get; set; }

        public string attachment { get; set; }

        public string errorMessage { get; set; }

        public string subject { get; set; }

        public string status { get; set; }

      //  public string URL { get; set; }
    }
}

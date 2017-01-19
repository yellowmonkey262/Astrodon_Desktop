namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLetterRun")]
    public partial class tblLetterRun
    {
        public int id { get; set; }

        [Required]
        public string fromEmail { get; set; }

        [Required]
        public string toEmail { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string message { get; set; }

        public bool? html { get; set; }

        public bool? addcc { get; set; }

        public bool? readreceipt { get; set; }

        public string attachment { get; set; }

        public DateTime queueDate { get; set; }

        public DateTime? sentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string unitno { get; set; }

        public string errorMessage { get; set; }

        public string status { get; set; }

        public string cc { get; set; }

        public string bcc { get; set; }
    }
}

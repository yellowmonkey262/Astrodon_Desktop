namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblJob")]
    public partial class tblJob
    {
        public int id { get; set; }

        public int creator { get; set; }

        public int processor { get; set; }

        public DateTime createDate { get; set; }

        public int buildingID { get; set; }

        [Required]
        [StringLength(255)]
        public string subject { get; set; }

        [StringLength(255)]
        public string topic { get; set; }

        public string notes { get; set; }

        public string emailBody { get; set; }

        public bool upload { get; set; }

        public bool email { get; set; }

        [Required]
        [StringLength(50)]
        public string currentStatus { get; set; }

        public bool customerAction { get; set; }
    }
}

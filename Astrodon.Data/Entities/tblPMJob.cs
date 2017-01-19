namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPMJob")]
    public partial class tblPMJob
    {
        public int id { get; set; }

        public int creator { get; set; }

        public int? processedBy { get; set; }

        [Required]
        [StringLength(50)]
        public string buildingCode { get; set; }

        public bool buildingUpload { get; set; }

        public bool inboxUpload { get; set; }

        public string buildingFolder { get; set; }

        public string topic { get; set; }

        public string instructions { get; set; }

        public string notes { get; set; }

        public string cc { get; set; }

        public string bcc { get; set; }

        public string subject { get; set; }

        public string body { get; set; }

        public string sms { get; set; }

        public bool billcustomer { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        public DateTime createDate { get; set; }

        public DateTime? assignedDate { get; set; }

        public DateTime? completeDate { get; set; }
    }
}

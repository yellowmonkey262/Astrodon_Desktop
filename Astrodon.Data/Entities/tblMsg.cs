namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMsg")]
    public partial class tblMsg
    {
        public int id { get; set; }

        public int buildingID { get; set; }

        [Required]
        public string fromAddress { get; set; }

        public bool incBCC { get; set; }

        [StringLength(50)]
        public string bccAddy { get; set; }

        public string subject { get; set; }

        public string message { get; set; }

        public bool billBuilding { get; set; }

        public decimal billAmount { get; set; }

        public bool queue { get; set; }
    }
}

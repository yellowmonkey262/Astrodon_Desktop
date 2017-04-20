namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblClearance
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string buildingCode { get; set; }

        [Required]
        [StringLength(50)]
        public string customerCode { get; set; }

        public string preparedBy { get; set; }

        public string trfAttorneys { get; set; }

        public string attReference { get; set; }

        public string fax { get; set; }

        public DateTime? certDate { get; set; }

        public string complex { get; set; }

        public string unitNo { get; set; }

        public string seller { get; set; }

        public string purchaser { get; set; }

        public string purchaserAddress { get; set; }

        public string purchaserTel { get; set; }

        public string purchaserEmail { get; set; }

        public DateTime? regDate { get; set; }

        public string notes { get; set; }

        public decimal clearanceFee { get; set; }

        public decimal astrodonTotal { get; set; }

        public DateTime? validDate { get; set; }

        public bool processed { get; set; }

        public bool paid { get; set; }

        public bool bc { get; set; }

        public bool hoa { get; set; }

        public bool registered { get; set; }

        public bool? journal { get; set; }

        public bool? extClearance { get; set; }
    }
}

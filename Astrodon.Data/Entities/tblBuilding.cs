namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBuilding
    {
        public int id { get; set; }

        public string Building { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public string AccNumber { get; set; }

        public string DataPath { get; set; }

        [StringLength(50)]
        public string Period { get; set; }

        [StringLength(50)]
        public string Acc { get; set; }

        [StringLength(50)]
        public string Contra { get; set; }

        [StringLength(50)]
        public string ownbank { get; set; }

        [StringLength(50)]
        public string cashbook3 { get; set; }

        public int? payments { get; set; }

        public int? receipts { get; set; }

        public int? journals { get; set; }

        [StringLength(50)]
        public string bc { get; set; }

        [StringLength(50)]
        public string centrec { get; set; }

        [StringLength(50)]
        public string business { get; set; }

        [StringLength(50)]
        public string bank { get; set; }

        public string pm { get; set; }

        public string bankName { get; set; }

        public string accName { get; set; }

        public string bankAccNumber { get; set; }

        public string branch { get; set; }

        public bool isBuilding { get; set; }

        public string addy1 { get; set; }

        public string addy2 { get; set; }

        public string addy3 { get; set; }

        public string addy4 { get; set; }

        public string addy5 { get; set; }

        [StringLength(255)]
        public string web { get; set; }

        [StringLength(255)]
        public string letterName { get; set; }

        [StringLength(50)]
        public string pid { get; set; }

        public bool hoa { get; set; }
    }
}
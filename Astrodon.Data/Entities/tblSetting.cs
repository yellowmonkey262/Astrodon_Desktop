namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblSetting
    {
        [Key]
        [Column(Order = 0)]
        [MaxLength(200)]
        public string templatedir { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal minbal { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal rem_admin { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal sum_admin { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal recon_fee { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal discon_admin { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal fd_admin { get; set; }

        public decimal? clearance { get; set; }

        public decimal? ex_clearance { get; set; }

        public decimal? reminder_fee { get; set; }

        public decimal? final_fee { get; set; }

        public decimal? summons_fee { get; set; }

        public decimal? discon_notice_fee { get; set; }

        public decimal? discon_fee { get; set; }

        public decimal? handover_fee { get; set; }

        public decimal? recon_split { get; set; }

        public decimal? debit_order { get; set; }

        public decimal? ret_debit_order { get; set; }

        public decimal? eft_fee { get; set; }

        public decimal? monthly_journal { get; set; }

        public string trust { get; set; }

        public string centrec { get; set; }

        public string business { get; set; }

        public string rental { get; set; }
    }
}

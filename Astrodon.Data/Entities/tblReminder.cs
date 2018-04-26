namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblReminder
    {
        public int id { get; set; }

        [Index("UIDX_tblReminderUser", Order = 0)]
        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual tblUser User { get; set; }

        [Index("UIDX_tblReminderBuilding")]
        public virtual int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [Required]
        [StringLength(50)]
        public string customer { get; set; }

        [Index("UIDX_tblReminderUser",Order =1)]
        public DateTime remDate { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string remNote { get; set; }

        [Index("UIDX_tblReminderUser", Order = 2)]
        public bool action { get; set; }

        public DateTime? actionDate { get; set; }

        public string Contacts { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }
    }
}

using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.Calendar
{
    [Table("BuildingCalendarEntry")]
    public class BuildingCalendarEntry : DbEntity
    {
        [Index("IDX_BuildingCalendarEntry", Order = 0)]
        public CalendarEntryType CalendarEntryType { get; set; }

        [Index("IDX_BuildingCalendarEntry",  Order = 1)]
        public virtual int? BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [Index("IDX_BuildingCalendarEntry", Order = 2)]
        public virtual int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual tblUser User { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime EventToDate { get; set; }

        [MaxLength(50)]
        [Required]
        public string Event { get; set; }

        [MaxLength(200)]
        [Required]
        public string Venue { get; set; }

        public bool NotifyTrustees { get; set; }

        public string BCCEmailAddress { get; set; }

        public string InviteSubject { get; set; }

        public string InviteBody { get; set; }

        public bool TrusteesNotified { get; set; }

        public virtual ICollection<CalendarEntryAttachment> Attachments { get; set; }

        public virtual ICollection<CalendarUserInvite> UserInvites { get; set; }
    }
}

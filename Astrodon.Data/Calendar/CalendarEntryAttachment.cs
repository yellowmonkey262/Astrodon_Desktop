using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.Calendar
{
    [Table("CalendarEntryAttachment")]
    public class CalendarEntryAttachment : DbEntity
    {
        [Index("IDX_CalendarAttachment", Order = 0)]
        public virtual int BuildingCalendarEntryId { get; set; }

        [ForeignKey("BuildingCalendarEntryId")]
        public virtual BuildingCalendarEntry CalendarEntry { get; set; }

        public byte[] FileData { get; set; }

        [Required]
        public string FileName { get; set; }

    }
}

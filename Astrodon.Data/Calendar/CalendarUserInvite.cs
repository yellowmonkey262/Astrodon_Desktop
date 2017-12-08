using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.Calendar
{
    [Table("CalendarUserInvite")]
    public class CalendarUserInvite : DbEntity
    {
        public virtual int CalendarEntryId { get; set; }
        [ForeignKey("CalendarEntryId")]
        public virtual BuildingCalendarEntry CalendarEntry { get; set; }

        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual tblUser User { get; set; }
    }
}

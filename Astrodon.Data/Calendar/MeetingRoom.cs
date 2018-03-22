using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.Calendar
{
    [Table("MeetingRoom")]
    public class MeetingRoom : DbEntity
    {
        [Index("IDX_MeetingRoom", IsUnique = true)]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int NumberOfSeats { get; set; }

        public bool Active { get; set; }

        public override string ToString()
        {
            if (NumberOfSeats > 0)
                return Name + " ("+NumberOfSeats.ToString()+")";
            return Name;
        }

        public virtual ICollection<BuildingCalendarEntry> BuildingCalendarEntries { get; set; }
    }
}

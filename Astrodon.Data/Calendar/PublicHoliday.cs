using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.Calendar
{
    [Table("PublicHoliday")]
    public class PublicHoliday : DbEntity
    {
        [Index("IDX_PublicHoliday", IsUnique =true)]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(300)]
        public string HolidayName { get; set; }

    }
}

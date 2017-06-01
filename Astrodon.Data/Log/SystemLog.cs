using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Astrodon.Data.Log
{
    public class SystemLog : DbEntity
    {
        public DateTime EventTime { get; set; }

        [Required]
        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}

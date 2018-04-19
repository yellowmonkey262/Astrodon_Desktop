using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.InsuranceData
{
    [Table("InsuranceBroker")]
    public class InsuranceBroker : DbEntity
    {
        [MaxLength(200)]
        [Required]
        [Index("IDX_InsuarnceBroker",IsUnique =true)]
        public virtual string CompanyName { get; set; }

        [MaxLength(200)]
        public virtual string CompanyRegistration { get; set; }

        [MaxLength(200)]
        public virtual string VATNumber { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string ContactPerson { get; set; }

        public virtual string EmailAddress { get; set; }

        [MaxLength(200)]
        public virtual string ContactNumber { get; set; }
   
    }
}

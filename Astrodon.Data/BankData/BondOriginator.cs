using Astrodon.Data.Base;
using Astrodon.Data.SupplierData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace Astrodon.Data.BankData
{
    [Table("BondOriginator")]
    public class BondOriginator : DbEntity
    {
        [Index("IDX_BondOriginator", Order = 0, IsUnique = true)]
        [Required]
        [MaxLength(300)]
        public string CompanyName { get; set; }
    }
}

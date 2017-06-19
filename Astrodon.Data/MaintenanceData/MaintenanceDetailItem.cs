using Astrodon.Data.Base;
using Astrodon.Data.SupplierData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace Astrodon.Data.MaintenanceData
{
    [Table("MaintenanceDetailItem")]
    public class MaintenanceDetailItem : DbEntity
    {
        [Index("IDX_MaintenanceDetail", Order = 0, IsUnique = true)]
        public virtual int MaintenanceId { get; set; }
        [ForeignKey("MaintenanceId")]
        public virtual Maintenance Maintenance { get; set; }

        [MaxLength(50)]
        [Index("IDX_MaintenanceDetail", Order = 1, IsUnique = true)]
        [Required]
        public virtual string CustomerAccount { get; set; } //nullable as unit/customer account record in pastel

        public virtual bool IsForBodyCorporate { get; set; }

        public virtual decimal Amount { get; set; }

        public static string BodyCorporateAccountName { get; set; } = "BODY-CORPORATE";
    }
}

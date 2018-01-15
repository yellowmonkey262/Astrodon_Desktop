using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.ManagementPackData
{
    [Table("ManagementPack")]
    public class ManagementPack : DbEntity
    {
        [Index("UIDX_ManagementPack", IsUnique = true, Order = 0)]
        public virtual int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [Index("UIDX_ManagementPack", IsUnique = true, Order = 1)]
        public virtual DateTime Period { get; set; }

        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual tblUser UserCreated { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual DateTime DateUpdated { get; set; }

        public virtual byte[] ReportData { get; set; }

        public virtual bool Published { get; set; }

        public virtual string Commments { get; set; }

        public virtual ICollection<ManagementPackReportItem> Items { get; set; }
    }
}

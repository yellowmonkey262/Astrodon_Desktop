using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.RequisitionData
{
    [Table("RequisitionBatch")]
    public class RequisitionBatch : DbEntity
    {
        [Index("IDX_RequisitionBatch",IsUnique =true,Order =0)]
        public virtual int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [Index("IDX_RequisitionBatch", IsUnique = true, Order = 1)]
        public virtual int BatchNumber { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual tblUser UserCreated { get; set; }
    
        public virtual int Entries { get; set; }

        public virtual ICollection<tblRequisition> Requisitions { get; set; }

    }
}

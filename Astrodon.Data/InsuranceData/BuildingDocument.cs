using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.InsuranceData
{
    [Table("BuildingDocument")]
    public class BuildingDocument : DbEntity
    {
        [Index("IDX_BuildingDocumentBuildingId", Order =0, IsUnique = true)]
        public virtual int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [Index("IDX_BuildingDocumentBuildingId", Order = 1, IsUnique = true)]
        public virtual DocumentType DocumentType { get; set; }

        [Index("IDX_BuildingDocumentBuildingId", Order = 2, IsUnique = true)]
        public virtual DateTime DateUploaded { get; set; }

        [Required]
        public string FileName { get; set; }

        public byte[] FileData { get; set; }
    }
}

using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.ManagementPackData
{
    public class ManagementPackReportItem:DbEntity
    {
        [Index("IDX_ManagementPackReportItem")]
        public virtual int ManagementPackId { get; set; }
        [ForeignKey("ManagementPackId")]
        public virtual ManagementPack ManagementPack { get; set; }

        [Required]
        public virtual string Path { get; set; }

        [Required]
        public virtual string File { get; set; }

        public virtual string Description { get; set; }

        public virtual string Description2 { get; set; }

        public virtual int Pages { get; set; }

        public virtual int Position { get; set; }

        public virtual DateTime FileDate { get; set; }

        public virtual bool IsTempFile { get; set; }

        public virtual byte[] FileData { get; set; }
    }
}

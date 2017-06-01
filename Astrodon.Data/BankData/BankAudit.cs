using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Astrodon.Data.BankData
{
    [Table("BankAudit")]
    public class BankAudit : DbEntity
    {
        [Index("IDX_BankAudit", Order = 0)]
        public virtual int BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Bank Bank { get; set; }

        [Index("IDX_BankAudit", Order = 1)]
        public virtual DateTime AuditTimeStamp { get; set; }

      
        public virtual int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual tblUser User { get; set; }

      
        public virtual string FieldName { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string NewValue { get; set; }

        [NotMapped]
        public string AuditTimeStampStr { get { return AuditTimeStamp.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture); } }

        [NotMapped]
        public string UserName
        {
            get
            {
                return User == null ? string.Empty : User.name;
            }
        }

    }
}

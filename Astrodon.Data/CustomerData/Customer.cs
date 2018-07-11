using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.CustomerData
{
    [Table("Customer")]
    public class Customer : DbEntity
    {
        [Index("UIDX_CustomerUnit", IsUnique = true, Order = 0)]
        public virtual int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [MaxLength(200)]
        [Required]
        [Index("UIDX_CustomerUnit", IsUnique = true, Order = 1)]
        public virtual string AccountNumber { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsTrustee { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual bool SendBirthdayNotification { get; set; }

        public virtual string IDNumber { get; set; }

        [Index("IDX_CustomerDateOfBirth")]
        public virtual DateTime? DateOfBirth { get; set; }

        public virtual string CustomerFullName { get; set; }

        public virtual DateTime? LastBirthdayNotificationSent { get; set; }

        public virtual string CellNumber { get; set; }
        public virtual string BirthdaySMSText { get; set; }
        public virtual string BirthDaySMSStatus { get; set; }
        public virtual string BirthDaySMSBatch { get; set; }

        [MaxLength(200)]
        public virtual string EmailAddress1 { get; set; }

        [MaxLength(200)]
        public virtual string EmailAddress2 { get; set; }

        [MaxLength(200)]
        public virtual string EmailAddress3 { get; set; }

        [MaxLength(200)]
        public virtual string EmailAddress4 { get; set; }

        public virtual ICollection<CustomerDocument> Documents { get; set; }

        public void LoadEmails()
        {
            throw new NotImplementedException();
        }

        public void LoadEmails(string[] emailList)
        {

            if (emailList != null)
            {
                for (int x = 0; x < emailList.Length; x++)
                {
                    switch (x)
                    {
                        case 0:
                            if(!String.IsNullOrWhiteSpace(emailList[x]))
                            EmailAddress1 = emailList[x];
                            break;
                        case 1:
                            if (!String.IsNullOrWhiteSpace(emailList[x]))
                                EmailAddress2 = emailList[x];
                            break;
                        case 2:
                            if (!String.IsNullOrWhiteSpace(emailList[x]))
                                EmailAddress3 = emailList[x];
                            break;
                        case 3:
                            if (!String.IsNullOrWhiteSpace(emailList[x]))
                                EmailAddress4 = emailList[x];
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}

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
    [Table("Maintenance")]
    public class Maintenance : DbEntity
    {
        [Index("UIDX_Maintenance", IsUnique = true)]
        public virtual int RequisitionId { get; set; }
        [ForeignKey("RequisitionId")]
        public virtual tblRequisition Requisition { get; set; }

        public virtual int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public virtual int BuildingMaintenanceConfigurationId { get; set; }
        [ForeignKey("BuildingMaintenanceConfigurationId")]
        public virtual BuildingMaintenanceConfiguration BuildingMaintenanceConfiguration { get; set; }

        [MaxLength(10)]
        public virtual string CustomerAccount { get; set; } //nullable as unit/customer account record in pastel

        public virtual bool IsForBodyCorporate { get; set; }

        public virtual DateTime DateLogged { get; set; }

        public virtual DateTime InvoiceDate { get; set; }

        public virtual decimal TotalAmount { get; set; }

        public virtual string Description { get; set; }

        public virtual string InvoiceNumber { get; set; }

        #region Warranty

        public virtual int? WarrantyDuration { get; set; }

        public virtual DurationType? WarrantyDurationType { get; set; }

        public virtual DateTime? WarrentyExpires { get; set; }

        public virtual string WarrantySerialNumber { get; set; }

        public virtual string WarrantyNotes { get; set; }

        #endregion

        public virtual ICollection<MaintenanceDocument> MaintenanceDocuments { get; set; } 

    }
}

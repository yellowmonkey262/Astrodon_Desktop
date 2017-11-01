namespace Astrodon.Data
{
    using InsuranceData;
    using MaintenanceData;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.IO;

    public partial class tblBuilding
    {
        public int id { get; set; }

        public string Building { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public string AccNumber { get; set; }

        public string DataPath { get; set; }

        [StringLength(50)]
        public string Period { get; set; }

        [StringLength(50)]
        public string Acc { get; set; }

        [StringLength(50)]
        public string Contra { get; set; }

        [StringLength(50)]
        public string ownbank { get; set; }

        [StringLength(50)]
        public string cashbook3 { get; set; }

        public int? payments { get; set; }

        public int? receipts { get; set; }

        public int? journals { get; set; }

        [StringLength(50)]
        public string bc { get; set; }

        [StringLength(50)]
        public string centrec { get; set; }

        [StringLength(50)]
        public string business { get; set; }

        [StringLength(50)]
        public string bank { get; set; }

        public string pm { get; set; }

        public string bankName { get; set; }

        public string accName { get; set; }

        public string bankAccNumber { get; set; }

        public string branch { get; set; }

        public bool isBuilding { get; set; }

        public string addy1 { get; set; }

        public string addy2 { get; set; }

        public string addy3 { get; set; }

        public string addy4 { get; set; }

        public string addy5 { get; set; }

        [StringLength(255)]
        public string web { get; set; }

        [StringLength(255)]
        public string letterName { get; set; }

        [StringLength(50)]
        public string pid { get; set; }

        public bool hoa { get; set; }

        public decimal limitM { get; set; }

        public decimal limitW { get; set; }

        public decimal limitD { get; set; }

        public bool BuildingFinancialsEnabled { get; set; }

        #region Building Insurance

        public decimal CommonPropertyDimensions { get; set; }

        public decimal UnitPropertyDimensions { get; set; }

        public decimal UnitReplacementCost { get; set; }

        public decimal CommonPropertyReplacementCost { get; set; }

        public string PolicyNumber { get; set; }

        public bool InsuranceReplacementValueIncludesCommonProperty { get; set; }

        public bool BondHolderInterestNotedOnPolicy { get; set; }

        public string InsuranceBondHolder { get; set; }

        public int? InsuranceBrokerId { get; set; }
        [ForeignKey("InsuranceBrokerId")]
        public virtual InsuranceBroker InsuranceBroker { get; set; }

        #endregion

        [NotMapped]
        public string DataFolder
        {
            get
            {
                string root = @"Y:\Buildings Managed (Do not Remove)\";

                if (!Directory.Exists(root))
                    root = @"\\NAS01\Volume_1\USERS - DO NOT MOVE!!\Buildings Managed (Do not Remove)\";

                string basePath = root + this.Building.Replace("/", "").Replace(@"\", "") + @"\";


                return basePath;
            }
        }

        [NotMapped]
        public bool IsUsingNedbank
        {
            get
            {
                return !String.IsNullOrWhiteSpace(bank) && !String.IsNullOrWhiteSpace(bankAccNumber) && bank.Trim().ToLower() == "nedbank";// && bankAccNumber.Trim() == "1902226429";
            }
        }

        public bool IsDebitOrderFeeDisabled { get; set; }

        public bool CheckIfFolderExists()
        {
            try
            {
                if (!Directory.Exists(DataFolder))
                    Directory.CreateDirectory(DataFolder);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

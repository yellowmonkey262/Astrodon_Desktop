namespace Astrodon.Data
{
    using Calendar;
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

        public bool BuildingDisabled { get; set; }

        [StringLength(50)]
        [Index("IDX_BuildingCode",IsUnique =true)]
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

        public decimal AdditionalInsuredValueCost { get; set; }

        public string PolicyNumber { get; set; }

        public bool InsuranceReplacementValueIncludesCommonProperty { get; set; }

        public int? InsuranceBrokerId { get; set; }
        [ForeignKey("InsuranceBrokerId")]
        public virtual InsuranceBroker InsuranceBroker { get; set; }

        public decimal MonthlyInsurancePremium { get; set; }

        public DateTime? InsurancePolicyRenewalDate { get; set; }

        public string ExcessStructures { get; set; }

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

        #region Financials
        public int FinancialDayOfMonth { get; set; }

        public DateTime? FinancialStartDate { get; set; } //date financials should start at
        public DateTime? FinancialEndDate { get; set; } //stop allocating financials after the end date

        public bool FixedMonthyFinancialMeetingEnabled { get; set; }
        public int? FinancialMeetingDayOfMonth { get; set; }
        public string FinancialMeetingSubject { get; set; }
        public string FinancialMeetingBodyText { get; set; }
        public bool FinancialMeetingSendInviteToAllTrustees { get; set; }

        public string FinancialMeetingEvent { get; set; }
        public string FinancialMeetingVenue { get; set; }
        public string FinancialMeetingBCC { get; set; }

        public DateTime? FinancialMeetingStartTime { get; set; }
        public DateTime? FinancialMeetingEndTime { get; set; }


        public bool IsFixed { get; set; }

        public void CalculatePeriod()
        {
            if (!String.IsNullOrWhiteSpace(this.Period))
            {
                _PeriodCalculated = true;
                var intPeriod = Convert.ToInt32(this.Period);

                int month = 2; //feb
                for (int x = 0; x < intPeriod; x++)
                {
                    month++;
                    if (month > 12)
                        month = 1;
                }

                _FinancialPeriodEnd = new DateTime(DateTime.Now.Year + 1, month, 1);
                _FinancialPeriodStart = _FinancialPeriodEnd.AddMonths(-11);
                _FinancialPeriodEnd = _FinancialPeriodEnd.AddMonths(1).AddDays(-1);

                if (_FinancialPeriodStart > DateTime.Today)
                {
                    _FinancialPeriodStart = _FinancialPeriodStart.AddYears(-1);
                    _FinancialPeriodEnd = _FinancialPeriodEnd.AddYears(-1);
                }
            }
        }

        private bool _PeriodCalculated = false;
        private DateTime _FinancialPeriodStart;
        [NotMapped]
        public DateTime FinancialPeriodStart
        {
            get
            {
                if (!_PeriodCalculated)
                    CalculatePeriod();
                return _FinancialPeriodStart;
            }
            
        }

        private DateTime _FinancialPeriodEnd;
        [NotMapped]
        public DateTime FinancialPeriodEnd
        {
            get
            {
                if (!_PeriodCalculated)
                    CalculatePeriod();
                return _FinancialPeriodEnd;
            }

        }

        #endregion

        #region Calendar

        public virtual ICollection<BuildingCalendarEntry> BuildingCalendarEntries { get; set; }

        #endregion

        public ICollection<CustomerData.Customer> CustomerList { get; set; }
    }
}

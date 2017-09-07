namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMonthFin")]
    public partial class tblMonthFin
    {
        public int id { get; set; }

        public DateTime completeDate { get; set; }

        [Required]
        [StringLength(50)]
        [Index("IDX_tblMonthFinBuilding",Order =0)]
        public string buildingID { get; set; }

        public int userID { get; set; }

        public int? finPeriod { get; set; }

        public int? finMonth { get; set; }

        [Index("IDX_tblMonthFinBuilding", Order = 1)]
        public int year { get; set; }

        public DateTime findate { get; set; }

        public int levies { get; set; }

        public string leviesReason { get; set; }

        public int sewage { get; set; }

        public string sewageNotes { get; set; }

        public int electricity { get; set; }

        public string electricityNotes { get; set; }

        public int water { get; set; }

        public string waterNotes { get; set; }

        public int specialLevies { get; set; }

        public string specialLevyNotes { get; set; }

        public string otherIncomeDescription { get; set; }

        public int? otherIncome { get; set; }

        public string otherIncomeNotes { get; set; }

        public int memberInterest { get; set; }

        public string memberInterestNotes { get; set; }

        public int bankInterest { get; set; }

        public string bankInterestNotes { get; set; }

        public int accountingFees { get; set; }

        public string accountingFeesNotes { get; set; }

        public int? bankCharges { get; set; }

        public string bankChargesNotes { get; set; }

        public int? sewageExpense { get; set; }

        public string sewageExpenseNotes { get; set; }

        public int? deliveries { get; set; }

        public string deliveriesNotes { get; set; }

        public int? electricityExpense { get; set; }

        public string electricityExpenseNotes { get; set; }

        public int? gardens { get; set; }

        public string gardensNotes { get; set; }

        public int? insurance { get; set; }

        public string insuranceNotes { get; set; }

        public int? interestPaid { get; set; }

        public string interestPaidNotes { get; set; }

        public int? managementFees { get; set; }

        public string managementFeesNotes { get; set; }

        public int? meterReading { get; set; }

        public string meterReadingNotes { get; set; }

        public int? printing { get; set; }

        public string printingNotes { get; set; }

        public int? post { get; set; }

        public string postNotes { get; set; }

        public int? repairs { get; set; }

        public string repairsNotes { get; set; }

        public int? refuse { get; set; }

        public string refuseNotes { get; set; }

        public int? salaries { get; set; }

        public string salariesNotes { get; set; }

        public int? security { get; set; }

        public string securityNotes { get; set; }

        public int? telephone { get; set; }

        public string telephoneNotes { get; set; }

        public int? waterExpense { get; set; }

        public string waterExpenseNotes { get; set; }

        public int? municipal { get; set; }

        public string municipalReason { get; set; }

        public int? trust { get; set; }

        public string trustNotes { get; set; }

        public int? own { get; set; }

        public string ownNotes { get; set; }

        public int? investment { get; set; }

        public string investmentNotes { get; set; }

        public int? sundy { get; set; }

        public string sundryNotes { get; set; }

        public int? assets { get; set; }

        public string assetsNotes { get; set; }

        public int? debtors { get; set; }

        public string debtorsNotes { get; set; }

        public int? municipalAccounts { get; set; }

        public string municipalAccountsNotes { get; set; }

        public int? owners { get; set; }

        public string ownersNotes { get; set; }

        public int? suppliers { get; set; }

        public string suppliersNotes { get; set; }

        public int? liabilities { get; set; }

        public string liabilitiesNotes { get; set; }

        public int electricityRecon { get; set; }

        public int waterRecon { get; set; }

        public virtual byte[] CheckListPDF { get; set; }
    }
}

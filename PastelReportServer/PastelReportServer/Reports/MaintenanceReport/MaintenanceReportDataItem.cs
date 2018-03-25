using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data.MaintenanceData;
using Astradon.Data.Utility;

namespace Astrodon.Reports.MaintenanceReport
{
    public class MaintenanceReportDataItem : ReportDataBase
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Paid { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public MaintenanceClassificationType MaintenanceClassificationType { get; internal set; }
        public string Classification
        {
            get
            {
                return NameSplitting.SplitCamelCase(MaintenanceClassificationType);
            }
        }


        public string CompanyReg { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; }

        public string MaintenanceDisplayName { get { return MaintenanceType + " " + PastelAccountNumber; } }

        public string PastelAccountName { get; set; }
        public string PastelAccountNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Supplier { get; set; }
        public string Unit { get; set; }
        public string VatNumber { get; set; }
        public int? WarrantyDuration { get; set; }
        public DateTime? WarrantyExpires { get; set; }
        public string WarrantyNotes { get; set; }
        public DurationType? WarrantyType { get; set; }

        public string WarrantyDescription
        {
            get
            {
                if (WarrantyDuration == null)
                    return null;
                if (WarrantyType == null)
                    return null;

                return WarrantyDuration.Value.ToString() + " " + WarrantyType.Value.ToString();
            }
        }
        public decimal Budget { get; set; }
        public decimal Balance { get; set; }
        public decimal BudgetAvailable { get; set; }
        public int? LinkedPastelTransaction { get; set; }
        public string Ledger { get; set; }

        public string Month { get { return MaintenanceDate.ToString("MMM yyyy"); } }

        public string LedgerAccountNumber
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Ledger))
                    return Ledger;
                if (!Ledger.Contains(":"))
                    return Ledger;

                return Ledger.Substring(0, Ledger.IndexOf(":"));
            }
        }

        public DateTime PeriodMonth
        {
            get
            {
                return new DateTime(MaintenanceDate.Year, MaintenanceDate.Month, 1);
            }
        }

        public bool DetailLineVisible
        {
            get
            {
                return !String.IsNullOrWhiteSpace(Bank)
                    || !String.IsNullOrWhiteSpace(Branch)
                    || !String.IsNullOrWhiteSpace(BranchCode)
                    || !String.IsNullOrWhiteSpace(AccountNumber)
                    || WarrantyExpires != null
                    || !String.IsNullOrWhiteSpace(WarrantyNotes)
                    || !String.IsNullOrWhiteSpace(SerialNumber)
                    || !String.IsNullOrWhiteSpace(ContactPerson)
                    || !String.IsNullOrWhiteSpace(EmailAddress)
                    || !String.IsNullOrWhiteSpace(ContactNumber)
                    || !String.IsNullOrWhiteSpace(VatNumber)
                    || !String.IsNullOrWhiteSpace(CompanyReg);

            }
        }
    }
}

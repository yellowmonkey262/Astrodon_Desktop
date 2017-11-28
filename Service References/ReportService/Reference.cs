﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Astrodon.ReportService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MaintenanceReportType", Namespace="http://schemas.datacontract.org/2004/07/Astrodon.DataContracts")]
    public enum MaintenanceReportType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SummaryReport = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DetailedReport = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DetailedReportWithSupportingDocuments = 2,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PastelMaintenanceTransaction", Namespace="http://schemas.datacontract.org/2004/07/Astrodon.DataContracts.Maintenance")]
    [System.SerializableAttribute()]
    public partial class PastelMaintenanceTransaction : Astrodon.ReportService.PervasiveItem {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AutoNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DataPathField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReferenceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TransactionDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool TrustAccountField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Account {
            get {
                return this.AccountField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountField, value) != true)) {
                    this.AccountField = value;
                    this.RaisePropertyChanged("Account");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountName {
            get {
                return this.AccountNameField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountNameField, value) != true)) {
                    this.AccountNameField = value;
                    this.RaisePropertyChanged("AccountName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountType {
            get {
                return this.AccountTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountTypeField, value) != true)) {
                    this.AccountTypeField = value;
                    this.RaisePropertyChanged("AccountType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Amount {
            get {
                return this.AmountField;
            }
            set {
                if ((this.AmountField.Equals(value) != true)) {
                    this.AmountField = value;
                    this.RaisePropertyChanged("Amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AutoNumber {
            get {
                return this.AutoNumberField;
            }
            set {
                if ((this.AutoNumberField.Equals(value) != true)) {
                    this.AutoNumberField = value;
                    this.RaisePropertyChanged("AutoNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataPath {
            get {
                return this.DataPathField;
            }
            set {
                if ((object.ReferenceEquals(this.DataPathField, value) != true)) {
                    this.DataPathField = value;
                    this.RaisePropertyChanged("DataPath");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Reference {
            get {
                return this.ReferenceField;
            }
            set {
                if ((object.ReferenceEquals(this.ReferenceField, value) != true)) {
                    this.ReferenceField = value;
                    this.RaisePropertyChanged("Reference");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime TransactionDate {
            get {
                return this.TransactionDateField;
            }
            set {
                if ((this.TransactionDateField.Equals(value) != true)) {
                    this.TransactionDateField = value;
                    this.RaisePropertyChanged("TransactionDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool TrustAccount {
            get {
                return this.TrustAccountField;
            }
            set {
                if ((this.TrustAccountField.Equals(value) != true)) {
                    this.TrustAccountField = value;
                    this.RaisePropertyChanged("TrustAccount");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PervasiveItem", Namespace="http://schemas.datacontract.org/2004/07/Astrodon.DataContracts")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Astrodon.ReportService.PastelMaintenanceTransaction))]
    public partial class PervasiveItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TOCDataItem", Namespace="http://schemas.datacontract.org/2004/07/Astrodon.Reports.ManagementReportCoverPag" +
        "e")]
    [System.SerializableAttribute()]
    public partial class TOCDataItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ItemDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ItemNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PageNumberField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ItemDescription {
            get {
                return this.ItemDescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.ItemDescriptionField, value) != true)) {
                    this.ItemDescriptionField = value;
                    this.RaisePropertyChanged("ItemDescription");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ItemNumber {
            get {
                return this.ItemNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.ItemNumberField, value) != true)) {
                    this.ItemNumberField = value;
                    this.RaisePropertyChanged("ItemNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PageNumber {
            get {
                return this.PageNumberField;
            }
            set {
                if ((this.PageNumberField.Equals(value) != true)) {
                    this.PageNumberField = value;
                    this.RaisePropertyChanged("PageNumber");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DebitOrderItem", Namespace="http://schemas.datacontract.org/2004/07/Astrodon.DebitOrder")]
    [System.SerializableAttribute()]
    public partial class DebitOrderItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Astrodon.Data.DebitOrder.AccountTypeType AccountTypeIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmountDueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BranchCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int BuildingIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CollectionDayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> DebitOrderCancelDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool DebitOrderCancelledField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Astrodon.Data.DebitOrder.DebitOrderDayType DebitOrderCollectionDayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal DebitOrderFeeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool DebitOrderFeeDisabledField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsDebitOrderFeeDisabledOnBuildingField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal LevyRollDueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal MaxDebitOrderAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal PaymentsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountNumber {
            get {
                return this.AccountNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountNumberField, value) != true)) {
                    this.AccountNumberField = value;
                    this.RaisePropertyChanged("AccountNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Astrodon.Data.DebitOrder.AccountTypeType AccountTypeId {
            get {
                return this.AccountTypeIdField;
            }
            set {
                if ((this.AccountTypeIdField.Equals(value) != true)) {
                    this.AccountTypeIdField = value;
                    this.RaisePropertyChanged("AccountTypeId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal AmountDue {
            get {
                return this.AmountDueField;
            }
            set {
                if ((this.AmountDueField.Equals(value) != true)) {
                    this.AmountDueField = value;
                    this.RaisePropertyChanged("AmountDue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BranchCode {
            get {
                return this.BranchCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.BranchCodeField, value) != true)) {
                    this.BranchCodeField = value;
                    this.RaisePropertyChanged("BranchCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BuildingId {
            get {
                return this.BuildingIdField;
            }
            set {
                if ((this.BuildingIdField.Equals(value) != true)) {
                    this.BuildingIdField = value;
                    this.RaisePropertyChanged("BuildingId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CollectionDay {
            get {
                return this.CollectionDayField;
            }
            set {
                if ((this.CollectionDayField.Equals(value) != true)) {
                    this.CollectionDayField = value;
                    this.RaisePropertyChanged("CollectionDay");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CustomerCode {
            get {
                return this.CustomerCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.CustomerCodeField, value) != true)) {
                    this.CustomerCodeField = value;
                    this.RaisePropertyChanged("CustomerCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CustomerName {
            get {
                return this.CustomerNameField;
            }
            set {
                if ((object.ReferenceEquals(this.CustomerNameField, value) != true)) {
                    this.CustomerNameField = value;
                    this.RaisePropertyChanged("CustomerName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> DebitOrderCancelDate {
            get {
                return this.DebitOrderCancelDateField;
            }
            set {
                if ((this.DebitOrderCancelDateField.Equals(value) != true)) {
                    this.DebitOrderCancelDateField = value;
                    this.RaisePropertyChanged("DebitOrderCancelDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool DebitOrderCancelled {
            get {
                return this.DebitOrderCancelledField;
            }
            set {
                if ((this.DebitOrderCancelledField.Equals(value) != true)) {
                    this.DebitOrderCancelledField = value;
                    this.RaisePropertyChanged("DebitOrderCancelled");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Astrodon.Data.DebitOrder.DebitOrderDayType DebitOrderCollectionDay {
            get {
                return this.DebitOrderCollectionDayField;
            }
            set {
                if ((this.DebitOrderCollectionDayField.Equals(value) != true)) {
                    this.DebitOrderCollectionDayField = value;
                    this.RaisePropertyChanged("DebitOrderCollectionDay");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal DebitOrderFee {
            get {
                return this.DebitOrderFeeField;
            }
            set {
                if ((this.DebitOrderFeeField.Equals(value) != true)) {
                    this.DebitOrderFeeField = value;
                    this.RaisePropertyChanged("DebitOrderFee");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool DebitOrderFeeDisabled {
            get {
                return this.DebitOrderFeeDisabledField;
            }
            set {
                if ((this.DebitOrderFeeDisabledField.Equals(value) != true)) {
                    this.DebitOrderFeeDisabledField = value;
                    this.RaisePropertyChanged("DebitOrderFeeDisabled");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsDebitOrderFeeDisabledOnBuilding {
            get {
                return this.IsDebitOrderFeeDisabledOnBuildingField;
            }
            set {
                if ((this.IsDebitOrderFeeDisabledOnBuildingField.Equals(value) != true)) {
                    this.IsDebitOrderFeeDisabledOnBuildingField = value;
                    this.RaisePropertyChanged("IsDebitOrderFeeDisabledOnBuilding");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal LevyRollDue {
            get {
                return this.LevyRollDueField;
            }
            set {
                if ((this.LevyRollDueField.Equals(value) != true)) {
                    this.LevyRollDueField = value;
                    this.RaisePropertyChanged("LevyRollDue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal MaxDebitOrderAmount {
            get {
                return this.MaxDebitOrderAmountField;
            }
            set {
                if ((this.MaxDebitOrderAmountField.Equals(value) != true)) {
                    this.MaxDebitOrderAmountField = value;
                    this.RaisePropertyChanged("MaxDebitOrderAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Payments {
            get {
                return this.PaymentsField;
            }
            set {
                if ((this.PaymentsField.Equals(value) != true)) {
                    this.PaymentsField = value;
                    this.RaisePropertyChanged("Payments");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReportService.IReportService")]
    public interface IReportService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/LevyRollReport", ReplyAction="http://tempuri.org/IReportService/LevyRollReportResponse")]
        byte[] LevyRollReport(System.DateTime processMonth, string buildingName, string dataPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/LevyRollExcludeSundries", ReplyAction="http://tempuri.org/IReportService/LevyRollExcludeSundriesResponse")]
        byte[] LevyRollExcludeSundries(System.DateTime processMonth, string buildingName, string dataPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/SupplierReport", ReplyAction="http://tempuri.org/IReportService/SupplierReportResponse")]
        byte[] SupplierReport(string sqlConnectionString, System.DateTime fromDate, System.DateTime toDate, System.Nullable<int> buildingId, System.Nullable<int> supplierId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/MaintenanceReport", ReplyAction="http://tempuri.org/IReportService/MaintenanceReportResponse")]
        byte[] MaintenanceReport(string sqlConnectionString, Astrodon.ReportService.MaintenanceReportType reportType, System.DateTime fromDate, System.DateTime toDate, int buildingId, string buildingName, string dataPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/MissingMaintenanceRecordsGet", ReplyAction="http://tempuri.org/IReportService/MissingMaintenanceRecordsGetResponse")]
        Astrodon.ReportService.PastelMaintenanceTransaction[] MissingMaintenanceRecordsGet(string sqlConnectionString, int buildingId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/RequisitionBatchReport", ReplyAction="http://tempuri.org/IReportService/RequisitionBatchReportResponse")]
        byte[] RequisitionBatchReport(string sqlConnectionString, int requisitionBatchId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/ManagementPackCoverPage", ReplyAction="http://tempuri.org/IReportService/ManagementPackCoverPageResponse")]
        byte[] ManagementPackCoverPage(System.DateTime processMonth, string buildingName, string agent, Astrodon.ReportService.TOCDataItem[] tocDataItems);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/InsuranceSchedule", ReplyAction="http://tempuri.org/IReportService/InsuranceScheduleResponse")]
        byte[] InsuranceSchedule(string sqlConnectionString, int buildingId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/RunDebitOrderForBuilding", ReplyAction="http://tempuri.org/IReportService/RunDebitOrderForBuildingResponse")]
        Astrodon.ReportService.DebitOrderItem[] RunDebitOrderForBuilding(string sqlConnectionString, int buildingId, System.DateTime processMonth, bool showFeeBreakdown);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/MonthlyReport", ReplyAction="http://tempuri.org/IReportService/MonthlyReportResponse")]
        byte[] MonthlyReport(string sqlConnectionString, System.DateTime processMonth, bool completedItems, System.Nullable<int> userId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReportServiceChannel : Astrodon.ReportService.IReportService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReportServiceClient : System.ServiceModel.ClientBase<Astrodon.ReportService.IReportService>, Astrodon.ReportService.IReportService {
        
        public ReportServiceClient() {
        }
        
        public ReportServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReportServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReportServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReportServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public byte[] LevyRollReport(System.DateTime processMonth, string buildingName, string dataPath) {
            return base.Channel.LevyRollReport(processMonth, buildingName, dataPath);
        }
        
        public byte[] LevyRollExcludeSundries(System.DateTime processMonth, string buildingName, string dataPath) {
            return base.Channel.LevyRollExcludeSundries(processMonth, buildingName, dataPath);
        }
        
        public byte[] SupplierReport(string sqlConnectionString, System.DateTime fromDate, System.DateTime toDate, System.Nullable<int> buildingId, System.Nullable<int> supplierId) {
            return base.Channel.SupplierReport(sqlConnectionString, fromDate, toDate, buildingId, supplierId);
        }
        
        public byte[] MaintenanceReport(string sqlConnectionString, Astrodon.ReportService.MaintenanceReportType reportType, System.DateTime fromDate, System.DateTime toDate, int buildingId, string buildingName, string dataPath) {
            return base.Channel.MaintenanceReport(sqlConnectionString, reportType, fromDate, toDate, buildingId, buildingName, dataPath);
        }
        
        public Astrodon.ReportService.PastelMaintenanceTransaction[] MissingMaintenanceRecordsGet(string sqlConnectionString, int buildingId) {
            return base.Channel.MissingMaintenanceRecordsGet(sqlConnectionString, buildingId);
        }
        
        public byte[] RequisitionBatchReport(string sqlConnectionString, int requisitionBatchId) {
            return base.Channel.RequisitionBatchReport(sqlConnectionString, requisitionBatchId);
        }
        
        public byte[] ManagementPackCoverPage(System.DateTime processMonth, string buildingName, string agent, Astrodon.ReportService.TOCDataItem[] tocDataItems) {
            return base.Channel.ManagementPackCoverPage(processMonth, buildingName, agent, tocDataItems);
        }
        
        public byte[] InsuranceSchedule(string sqlConnectionString, int buildingId) {
            return base.Channel.InsuranceSchedule(sqlConnectionString, buildingId);
        }
        
        public Astrodon.ReportService.DebitOrderItem[] RunDebitOrderForBuilding(string sqlConnectionString, int buildingId, System.DateTime processMonth, bool showFeeBreakdown) {
            return base.Channel.RunDebitOrderForBuilding(sqlConnectionString, buildingId, processMonth, showFeeBreakdown);
        }
        
        public byte[] MonthlyReport(string sqlConnectionString, System.DateTime processMonth, bool completedItems, System.Nullable<int> userId) {
            return base.Channel.MonthlyReport(sqlConnectionString, processMonth, completedItems, userId);
        }
    }
}

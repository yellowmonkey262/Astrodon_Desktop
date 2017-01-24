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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReportService.IReportService")]
    public interface IReportService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/LevyRollReport", ReplyAction="http://tempuri.org/IReportService/LevyRollReportResponse")]
        byte[] LevyRollReport(System.DateTime processMonth, string buildingName, string dataPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/SupplierReport", ReplyAction="http://tempuri.org/IReportService/SupplierReportResponse")]
        byte[] SupplierReport(string sqlConnectionString, System.DateTime processMonth);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReportService/MaintenanceReport", ReplyAction="http://tempuri.org/IReportService/MaintenanceReportResponse")]
        byte[] MaintenanceReport(string sqlConnectionString, Astrodon.ReportService.MaintenanceReportType reportType, System.DateTime processMonth, string buildingName, string dataPath);
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
        
        public byte[] SupplierReport(string sqlConnectionString, System.DateTime processMonth) {
            return base.Channel.SupplierReport(sqlConnectionString, processMonth);
        }
        
        public byte[] MaintenanceReport(string sqlConnectionString, Astrodon.ReportService.MaintenanceReportType reportType, System.DateTime processMonth, string buildingName, string dataPath) {
            return base.Channel.MaintenanceReport(sqlConnectionString, reportType, processMonth, buildingName, dataPath);
        }
    }
}

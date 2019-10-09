﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using HermesNS.TC.Services.AuditTrail;

namespace TopCoder.Services.WCF.Audit.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// svcutil generated code
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditRecord))]
    [HermesNS.TC.CoverageExcludeAttribute]
    public partial class AuditRecordBaseOfHermesAuditRecordstringvzJn3tl2 :
        object, System.Runtime.Serialization.IExtensibleDataObject
    {
        /// <summary>
        /// svcutil generated code
        /// </summary>
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <value>svcutil generated code</value>
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
}

namespace HermesNS.TC.Services.ScheduleItem.Clients
{
    /// <summary>
    /// svcutil generated code
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "HermesAuditTrailSaveService")]
    [HermesNS.TC.CoverageExcludeAttribute]
    public interface HermesAuditTrailSaveService
    {
        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="records">svcutil generated code</param>
        [System.ServiceModel.OperationContractAttribute(
            Action = "http://tempuri.org/HermesAuditTrailSaveService/SaveAuditRecords",
            ReplyAction = "http://tempuri.org/HermesAuditTrailSaveService/SaveAuditRecordsResponse")]
        void SaveAuditRecords(IList<HermesAuditRecord> records);
    }

    /// <summary>
    /// svcutil generated code
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [HermesNS.TC.CoverageExcludeAttribute]
    public interface HermesAuditTrailSaveServiceChannel : HermesAuditTrailSaveService, System.ServiceModel.IClientChannel
    {
    }

    /// <summary>
    /// svcutil generated code
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [HermesNS.TC.CoverageExcludeAttribute]
    public partial class AuditServiceClient :
        System.ServiceModel.ClientBase<HermesAuditTrailSaveService>, HermesAuditTrailSaveService
    {
        /// <summary>
        /// svcutil generated code
        /// </summary>
        public AuditServiceClient()
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="endpointConfigurationName">svcutil generated code</param>
        public AuditServiceClient(string endpointConfigurationName)
            :
                base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="endpointConfigurationName">svcutil generated code</param>
        /// <param name="remoteAddress">svcutil generated code</param>
        public AuditServiceClient(string endpointConfigurationName, string remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="endpointConfigurationName">svcutil generated code</param>
        /// <param name="remoteAddress">svcutil generated code</param>
        public AuditServiceClient(string endpointConfigurationName,
            System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="binding">svcutil generated code</param>
        /// <param name="remoteAddress">svcutil generated code</param>
        public AuditServiceClient(System.ServiceModel.Channels.Binding binding,
            System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(binding, remoteAddress)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="records">svcutil generated code</param>
        public void SaveAuditRecords(IList<HermesAuditRecord> records)
        {
            base.Channel.SaveAuditRecords(records);
        }
    }

    /// <summary>
    /// svcutil generated code
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "HermesAuditTrailRetrieveService")]
    [HermesNS.TC.CoverageExcludeAttribute]
    public interface HermesAuditTrailRetrieveService
    {
        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="recordSearch">svcutil generated code</param>
        /// <returns>svcutil generated code</returns>
        [System.ServiceModel.OperationContractAttribute(
            Action = "http://tempuri.org/HermesAuditTrailRetrieveService/GetAuditRecords",
            ReplyAction = "http://tempuri.org/HermesAuditTrailRetrieveService/GetAuditRecordsResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditRecord[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditRecord))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(
            TopCoder.Services.WCF.Audit.Entities.AuditRecordBaseOfHermesAuditRecordstringvzJn3tl2))]
        IList<HermesAuditRecord> GetAuditRecords(object recordSearch);
    }

    /// <summary>
    /// svcutil generated code
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [HermesNS.TC.CoverageExcludeAttribute]
    public interface HermesAuditTrailRetrieveServiceChannel :
        HermesAuditTrailRetrieveService, System.ServiceModel.IClientChannel
    {
    }

    /// <summary>
    /// svcutil generated code
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [HermesNS.TC.CoverageExcludeAttribute]
    public partial class HermesAuditTrailRetrieveServiceClient :
        System.ServiceModel.ClientBase<HermesAuditTrailRetrieveService>, HermesAuditTrailRetrieveService
    {

        /// <summary>
        /// svcutil generated code
        /// </summary>
        public HermesAuditTrailRetrieveServiceClient()
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="endpointConfigurationName">svcutil generated code</param>
        public HermesAuditTrailRetrieveServiceClient(string endpointConfigurationName)
            :
                base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="endpointConfigurationName">svcutil generated code</param>
        /// <param name="remoteAddress">svcutil generated code</param>
        public HermesAuditTrailRetrieveServiceClient(string endpointConfigurationName, string remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="endpointConfigurationName">svcutil generated code</param>
        /// <param name="remoteAddress">svcutil generated code</param>
        public HermesAuditTrailRetrieveServiceClient(string endpointConfigurationName,
            System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="binding">svcutil generated code</param>
        /// <param name="remoteAddress">svcutil generated code</param>
        public HermesAuditTrailRetrieveServiceClient(System.ServiceModel.Channels.Binding binding,
            System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(binding, remoteAddress)
        {
        }

        /// <summary>
        /// svcutil generated code
        /// </summary>
        /// <param name="recordSearch">svcutil generated code</param>
        /// <returns>svcutil generated code</returns>
        public IList<HermesAuditRecord> GetAuditRecords(object recordSearch)
        {
            return base.Channel.GetAuditRecords(recordSearch);
        }
    }
}
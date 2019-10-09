using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using TopCoder.Services.WCF;
using TopCoder.Service.WCF.Audit;
using TopCoder.Services.WCF.Audit.Entities;
using TopCoder.Services.WCF.Audit.Searching;

namespace TopCoder.Service.WCF.Audit
{
    public class AuditTrailSaveServiceBase<TAuditRecord, K> : TCWcfServiceBase, IDisposable
        where TAuditRecord : AuditRecordBase<TAuditRecord, K>, new()
    {
        /// <summary>
        /// <para>Does nothing</para>
        /// </summary>
        protected override void HostUpdated()
        {
        }

        /// <summary>
        /// <para>This method is called when the service is disposed.</para>
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// <para>Initialize an instance of AuditTrailSaveServiceBase</para>
        /// </summary>
        /// <param name="nameSpace">
        /// The namespace to load configuration values from.
        /// </param>
        /// <exception cref="SelfDocumentingException">
        /// If the given <paramref name="nameSpace"/> parameter is null.
        /// If the given <paramref name="nameSpace"/> parameter is an empty string.
        /// If errors occur while reading in the configuration, either from missing or erroneous values.
        /// </exception>
        public AuditTrailSaveServiceBase(string nameSpace)
        {
        }
    }

    public class AuditTrailRetrieveServiceBase<TAuditRecord, K> : TCWcfServiceBase
        where TAuditRecord : AuditRecordBase<TAuditRecord, K>, new()
    {
        public AuditTrailRetrieveServiceBase(string nameSpace)
        {
        }

        protected override void HostUpdated()
        {
        }
    }
}

namespace HermesNS.TC.Services.AuditTrail
{
    [ServiceContract]
    public class HermesAuditTrailSaveService : AuditTrailSaveServiceBase<HermesAuditRecord, string>
    {
        public HermesAuditTrailSaveService()
            : base(typeof(HermesAuditTrailSaveService).FullName)
        {
        }

        public HermesAuditTrailSaveService(string nameSpace)
            : base(nameSpace)
        {
        }

        [OperationContract]
        public virtual void SaveAuditRecords(IList<HermesAuditRecord> records)
        {
            AuditRecordsWrapper<HermesAuditRecord, string>.AuditRecords.Clear();

            //A mock save of the audit records
            foreach (HermesAuditRecord record in records)
            {
                AuditRecordsWrapper<HermesAuditRecord, string>.AuditRecords.Add(record);
            }
        }
    }

    [ServiceContract]
    public class HermesAuditTrailRetrieveService : AuditTrailRetrieveServiceBase<HermesAuditRecord, string>
    {
        public HermesAuditTrailRetrieveService()
            : base(typeof(HermesAuditTrailRetrieveService).FullName)
        {
        }

        public HermesAuditTrailRetrieveService(string nameSpace)
            : base(nameSpace)
        {
        }

        [OperationContract]
        public virtual IList<HermesAuditRecord> GetAuditRecords(IRecordSearch<HermesAuditRecord, string> recordSearch)
        {
            return AuditRecordsWrapper<HermesAuditRecord, string>.AuditRecords;
        }
    }

    internal static class AuditRecordsWrapper<TAuditRecord, K>
        where TAuditRecord : AuditRecordBase<TAuditRecord, K>, new()
    {
        private static IList<TAuditRecord> auditRecords = new List<TAuditRecord>();

        public static IList<TAuditRecord> AuditRecords
        {
            get
            {
                return auditRecords;
            }
            set
            {
                auditRecords = value;
            }
        }
    }
}

namespace TopCoder.Services.WCF.Audit.Searching
{
    public interface IRecordSearch<TAuditRecord, K>
    {
    }
}

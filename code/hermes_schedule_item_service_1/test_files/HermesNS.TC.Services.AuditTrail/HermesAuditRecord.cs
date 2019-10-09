// HermesAuditRecord.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;
using TopCoder.Services.WCF.Audit.Entities;

namespace HermesNS.TC.Services.AuditTrail
{
    /// <summary>A mock implementation of the HermesAuditRecord class</summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesAuditRecord :
        AuditRecordBase<HermesAuditRecord, string>
    {
        /// <summary>
        /// Stores the id of the entity which was audited.
        /// </summary>
        string entityId;

        /// <summary>
        /// Creates a new HermesAuditRecord
        /// </summary>
        public HermesAuditRecord()
        {
        }

        /// <summary>
        /// Gets or sets the id of the entity which was audited.
        /// </summary>
        [DataMember]
        public string EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }
    }
}

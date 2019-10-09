// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER
using System;
using System.Runtime.Serialization;

namespace TopCoder.Services.WCF.Audit.Entities
{
    /// <summary>
    /// A mock implementation of the AuditRecordBase class
    /// </summary>
    /// <typeparam name="TAuditRecord">Thetype of entity to be audited.</typeparam>
    /// <typeparam name="K">The type of id of the entity.</typeparam>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public abstract class AuditRecordBase<TAuditRecord, K>
        where TAuditRecord : AuditRecordBase<TAuditRecord, K>
    {
        /// <summary>
        /// The string representation of the old property value.
        /// </summary>
        string text1;

        /// <summary>
        /// The string representation of the new property value.
        /// </summary>
        string text2;

        /// <summary>
        /// The numeric representation of the old property value.
        /// </summary>
        decimal numericValue1;

        /// <summary>
        /// The numeric representation of the new property value.
        /// </summary>
        decimal numericValue2;

        /// <summary>
        /// The message for the audit.
        /// </summary>
        string message;

        /// <summary>
        /// Gets or sets the message for the audit.
        /// </summary>
        /// <value>The message for the audit.</value>
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Gets or sets the string representation of the old property value.
        /// </summary>
        /// <value>The string representation of the old property value.</value>
        [DataMember]
        public string TextValue1
        {
            get
            {
                return text1;
            }
            set
            {
                text1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the string representation of the new property value.
        /// </summary>
        /// <value>The string representation of the new property value.</value>
        [DataMember]
        public string TextValue2
        {
            get
            {
                return text2;
            }
            set
            {
                text2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the numeric representation of the old property value.
        /// </summary>
        /// <value>The numeric representation of the old property value.</value>
        [DataMember]
        public decimal NumericValue1
        {
            get
            {
                return numericValue1;
            }
            set
            {
                numericValue1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the numeric representation of the new property value.
        /// </summary>
        /// <value>The numeric representation of the new property value.</value>
        [DataMember]
        public decimal NumericValue2
        {
            get
            {
                return numericValue2;
            }
            set
            {
                numericValue2 = value;
            }
        }

        /// <summary>
        /// The current application
        /// </summary>
        private string currentApplication;

        /// <summary>
        /// The current application
        /// </summary>
        [DataMember]
        public string CurrentApplication
        {
            get { return currentApplication; }
            set { currentApplication = value; }
        }

        /// <summary>
        /// CallerIdentity
        /// </summary>
        private string callerIdentity;

        /// <summary>
        /// CallerIdentity
        /// </summary>
        [DataMember]
        public string CallerIdentity
        {
            get { return callerIdentity; }
            set { callerIdentity = value; }
        }

        /// <summary>
        /// CallerIdentityDomain
        /// </summary>
        private string callerIdentityDomain;

        /// <summary>
        /// CallerIdentityDomain
        /// </summary>
        [DataMember]
        public string CallerIdentityDomain
        {
            get { return callerIdentityDomain; }
            set { callerIdentityDomain = value; }
        }

        /// <summary>
        /// CreatedTimeStamp
        /// </summary>
        private DateTime createdTimeStamp;

        /// <summary>
        /// CreatedTimeStamp
        /// </summary>
        [DataMember]
        public DateTime CreatedTimeStamp
        {
            get { return createdTimeStamp; }
            set { createdTimeStamp = value; }
        }

        /// <summary>
        /// Category
        /// </summary>
        private AuditEventCategory category;

        /// <summary>
        /// Category
        /// </summary>
        [DataMember]
        public AuditEventCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// TransactionId
        /// </summary>
        private string transactionId;

        /// <summary>
        /// TransactionId
        /// </summary>
        [DataMember]
        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }

        /// <summary>
        /// Guid
        /// </summary>
        private Guid guid;

        /// <summary>
        /// Guid
        /// </summary>
        [DataMember]
        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        /// <summary>
        /// EventOutcomeCode
        /// </summary>
        private EventOutcomeCode eventOutcomeCode;

        /// <summary>
        /// EventOutcomeCode
        /// </summary>
        [DataMember]
        public EventOutcomeCode EventOutcomeCode
        {
            get { return eventOutcomeCode; }
            set { eventOutcomeCode = value; }
        }
    }

    /// <summary>
    /// AuditEventCategory enum
    /// </summary>
    [DataContract]
    public enum AuditEventCategory
    {
        /// <summary>
        /// Information
        /// </summary>
        [EnumMember]
        Information
    }

    /// <summary>
    /// EventOutcomeCode enum
    /// </summary>
    [DataContract]
    public enum EventOutcomeCode
    {
        /// <summary>
        /// ObjectCreated
        /// </summary>
        [EnumMember]
        ObjectCreated,

        /// <summary>
        /// DataModified
        /// </summary>
        [EnumMember]
        DataModified,

        /// <summary>
        /// ObjectDeleted
        /// </summary>
        [EnumMember]
        ObjectDeleted
    }
}

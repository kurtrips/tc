// ITradeActivityItem.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace Calypso.RDTP.Entity.Job
{
    /// <summary>
    /// Mock ITradeActivityItem class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ITradeActivityItem
    {
        /// <summary>
        /// Mock Status property.
        /// </summary>
        /// <value>Status</value>
        JobStatus Status
        {
            get;
        }

        /// <summary>
        /// Mock ServiceID property.
        /// </summary>
        /// <value>ServiceID</value>
        long ServiceID
        {
            get;
        }

        /// <summary>
        /// Mock JobPriority property.
        /// </summary>
        /// <value>JobPriority</value>
        int JobPriority
        {
            get;
        }

        /// <summary>
        /// Mock AccountingTreatment property.
        /// </summary>
        /// <value>AccountingTreatment</value>
        string AccountingTreatment
        {
            get;
        }

        /// <summary>
        /// Mock CreationDate property.
        /// </summary>
        /// <value>CreationDate</value>
        DateTime CreationDate
        {
            get;
        }

        /// <summary>
        /// Mock QueueID property.
        /// </summary>
        /// <value>QueueID</value>
        long QueueID
        {
            get;
        }

        /// <summary>
        /// Mock StatusChanged event.
        /// </summary>
        event EventHandler<StatusEventArgs> StatusChanged;

        /// <summary>
        /// Mock SetStatus method.
        /// </summary>
        void SetStatus(JobStatus statusCode);
    }
}

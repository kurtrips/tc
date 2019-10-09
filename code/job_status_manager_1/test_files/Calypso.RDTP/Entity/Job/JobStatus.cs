// JobStatus.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace Calypso.RDTP.Entity.Job
{
    /// <summary>
    /// This enumeration defines the job statuses that can be associated with a ITradeActivityItem.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public enum JobStatus
    {
        /// <summary>
        /// Indicates that the job is queued.
        /// </summary>
        Queued = 1,

        /// <summary>
        /// Indicates that the job is in the process of being dispatched.
        /// </summary>
        Dispatching = 2,

        /// <summary>
        /// Indicates that the job is being calculated.
        /// </summary>
        InCalculation = 3,

        /// <summary>
        /// Indicates that results have been calcuated for the job.
        /// </summary>
        ResultNoticeRecieved = 4,

        /// <summary>
        /// Indicates that the database has been updated for the job calculation.
        /// </summary>
        DBUpdateNoticeRecieved = 5,

        /// <summary>
        /// Indicates that the job is successfully complete.
        /// </summary>
        Complete = 6,

        /// <summary>
        /// Indicates that the job is hung.
        /// </summary>
        Hung = 7,

        /// <summary>
        /// Indicates that the job is in an error state.
        /// </summary>
        Error = 8,

        /// <summary>
        /// Indicates that there was an error dispatching the job.
        /// </summary>
        DispatchError = 9,

        /// <summary>
        /// Indicates that the max retry count for the job was exceeded.
        /// </summary>
        RetryCountExceededError = 10,

        /// <summary>
        /// Indicates that there was an error in the calculation for the job.
        /// </summary>
        CalculationServiceError = 11,

        /// <summary>
        /// Indicates that there was an error updating Prism.
        /// </summary>
        PrismUpdateError = 12
    }
}

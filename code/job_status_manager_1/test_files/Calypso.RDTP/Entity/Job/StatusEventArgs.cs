// StatusEventArgs.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace Calypso.RDTP.Entity.Job
{
    /// <summary>
    /// Mock StatusEventArgs class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class StatusEventArgs : EventArgs
    {
        /// <summary>
        /// The old status
        /// </summary>
        JobStatus oldStatus;

        /// <summary>
        /// The new status.
        /// </summary>
        JobStatus newStatus;

        /// <summary>
        /// The status message.
        /// </summary>
        string statusMessage;

        /// <summary>
        /// The status time.
        /// </summary>
        DateTime statusTime;

        /// <summary>
        /// Old status.
        /// </summary>
        /// <value>Old status</value>
        public JobStatus OldStatus
        {
            get
            {
                return oldStatus;
            }
        }

        /// <summary>
        /// New status.
        /// </summary>
        /// <value>New status</value>
        public JobStatus NewStatus
        {
            get
            {
                return newStatus;
            }
        }

        /// <summary>
        /// Creates a new StatusEventArgs for given parameters.
        /// </summary>
        /// <param name="oldStatus">old status.</param>
        /// <param name="newStatus">new status.</param>
        /// <param name="statusMessage">the status message.</param>
        /// <param name="statusTime">the status time.</param>
        public StatusEventArgs(JobStatus oldStatus, JobStatus newStatus, string statusMessage, DateTime statusTime)
        {
            this.oldStatus = oldStatus;
            this.newStatus = newStatus;
            this.statusMessage = statusMessage;
            this.statusTime = statusTime;
        }
    }
}

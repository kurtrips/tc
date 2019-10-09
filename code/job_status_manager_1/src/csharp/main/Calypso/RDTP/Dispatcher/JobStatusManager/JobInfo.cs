/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using Calypso.RDTP.Entity.Job;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// This class is used as key to order jobs by <see cref="JobStatus"/> and ServiceID.
    /// It implements IEquatable&lt;<see cref="JobInfo"/>&gt; interface to provide custom
    /// Equals method for key comparison.
    /// This class is used by main class (<see cref="JobStatusManager"/>) of component.
    /// </summary>
    /// <threadsafety>
    /// This class is immutable and thread safe.
    /// </threadsafety>
    /// <author>dfn</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class JobInfo : IEquatable<JobInfo>
    {
        /// <summary>
        /// <para>Job service ID. This variable is set be constructor and never be changed after it.
        /// It is immutable and there is no restriction for it. </para>
        /// </summary>
        private readonly long serviceID;

        /// <summary>
        /// <para>Job status. This variable is set be constructor and never be changed after it.
        /// It is immutable and it should be valid <see cref="JobStatus"/> value. </para>
        /// </summary>
        private readonly JobStatus status;

        /// <summary>
        /// <para>Gets the service ID of the job.</para>
        /// </summary>
        /// <value>The service ID of the job.</value>
        public long ServiceID
        {
            get
            {
                return serviceID;
            }
        }

        /// <summary>
        /// <para>Gets the status of the job.</para>
        /// </summary>
        /// <value>The status of the job.</value>
        public JobStatus Status
        {
            get
            {
                return status;
            }
        }


        /// <summary>
        /// <para>Constructs class instance with given parameters</para>
        /// </summary>
        /// <exception cref="ArgumentException">if status is not valid <see cref="JobStatus"/> value</exception>
        /// <param name="serviceID">service ID of the job.</param>
        /// <param name="status">job status of the job.</param>
        public JobInfo(long serviceID, JobStatus status)
        {
            Helper.ValidateJobStatus(status, "status");

            this.serviceID = serviceID;
            this.status = status;
        }

        /// <summary>
        /// <para>Compares two <see cref="JobInfo"/> instances.</para>
        /// <para>
        /// If other is null the method returns false.
        /// Two non-null instances are equal if both their serviceIDs and their statuses are equal."
        /// </para>
        /// </summary>
        /// <param name="other">other class instance to compare</param>
        /// <returns>true if this and other instances are equals, false otherwise.</returns>
        public bool Equals(JobInfo other)
        {
            //Return false if other is null
            if (other == null)
            {
                return false;
            }

            return (this.serviceID == other.serviceID) && (this.status == other.Status);
        }

        /// <summary>
        /// <para>Compares object with this instance.</para>
        /// <para>
        /// If other is not of type <see cref="JobInfo"/>, returns false.
        /// If other is null the method, returns false.
        /// Two non-null instances are equal if both their serviceIDs and their statuses are equal."
        /// </para>
        /// </summary>
        /// <param name="other">other object to compare with this</param>
        /// <returns>true if other is equals to this, false else</returns>
        public override bool Equals(object other)
        {
            return Equals(other as JobInfo);
        }

        /// <summary>
        /// <para>Gets the hash code for this object.</para>
        /// </summary>
        /// <returns>hash code for this object.</returns>
        public override int GetHashCode()
        {
            return status.GetHashCode() ^ serviceID.GetHashCode();
        }

    }
}

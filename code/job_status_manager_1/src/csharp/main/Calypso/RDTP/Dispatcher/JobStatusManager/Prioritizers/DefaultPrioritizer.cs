/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using Calypso.RDTP.Entity.Job;

namespace Calypso.RDTP.Dispatcher.JobStatusManager.Prioritizers
{
    /// <summary>
    /// This class is default implementation of <see cref="IPrioritizer"/> interface.
    /// It is used to compare two <see cref="ITradeActivityItem"/> instances.
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is immutable and thread safe.
    /// </threadsafety>
    ///
    /// <author>dfn</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class DefaultPrioritizer : IPrioritizer
    {
        /// <summary>
        /// The value of MTM accounting treatment
        /// </summary>
        private const string MTM = "MTM";

        /// <summary>
        /// The value of ACC accounting treatment
        /// </summary>
        private const string ACC = "ACC";

        /// <summary>
        /// The value for <c>job</c> equal-to <c>otherJob</c>.
        /// Used in <c>Compare</c> method.
        /// </summary>
        private const int EQUAL = 0;

        /// <summary>
        /// The value for <c>job</c> greater-than other <c>otherJob</c>.
        /// Used in <c>Compare</c> method.
        /// </summary>
        private const int GREATER = 1;

        /// <summary>
        /// The value for <c>job</c> less-than <c>otherJob</c>.
        /// Used in <c>Compare</c> method.
        /// </summary>
        private const int LESS = -1;

        /// <summary>
        /// <para>Constructs <see cref="DefaultPrioritizer"/> instance.</para>
        /// </summary>
        public DefaultPrioritizer()
        {
        }

        /// <summary>
        /// <para>Compares two <see cref="ITradeActivityItem"/> instances.</para>
        /// <para>
        /// The comparer uses following criteria -
        ///  Null or not-null (Non-null job is greater than null)
        ///  ServiceID (greater job has greater value)
        ///  Status as integer value (greater job has greater value)
        ///  Priority  (greater job has greater value)
        ///  AccountingTreatment ( 'MTM' is greater than 'ACC' )
        ///  CreationDate (greater job has greater value)
        /// If these parameters are not equal it returns result of comparison.
        /// If they are equal next criteria is compared. If all criteria are equal the method returns 0.
        /// </para>
        /// </summary>
        /// <param name="job">job to compare</param>
        /// <param name="otherJob">other job to campare</param>
        /// <returns>
        /// negative number if  job is less than otherJob,
        /// zero if they are equal,
        /// positive number if job is greater than otherJob
        /// </returns>
        public int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        {
            //Compare based on null.
            if ((job == null) && (otherJob == null))
            {
                return EQUAL;
            }
            if ((job != null) && (otherJob == null))
            {
                return GREATER;
            }
            if ((job == null) && (otherJob != null))
            {
                return LESS;
            }

            //Compare based on ServiceID
            if (job.ServiceID != otherJob.ServiceID)
            {
                return job.ServiceID > otherJob.ServiceID ? GREATER : LESS;
            }

            //Compare based on Status
            if (job.Status != otherJob.Status)
            {
                return job.Status > otherJob.Status ? GREATER : LESS;
            }

            //Compare based on JobPriority
            if (job.JobPriority != otherJob.JobPriority)
            {
                return job.JobPriority > otherJob.JobPriority ? GREATER : LESS;
            }

            //Any other value of AccountingTreatment except for "MTM" can be taken as "ACC"
            string accountingTreatment1 = MTM.Equals(job.AccountingTreatment) ? MTM : ACC;
            string accountingTreatment2 = MTM.Equals(otherJob.AccountingTreatment) ? MTM : ACC;
            //Compare based on AccountingTreatment
            if (!accountingTreatment1.Equals(accountingTreatment2))
            {
                return accountingTreatment1.CompareTo(accountingTreatment2);
            }

            //Compare based on CreationDate
            if (job.CreationDate != otherJob.CreationDate)
            {
                return job.CreationDate > otherJob.CreationDate ? GREATER : LESS;
            }

            return EQUAL;
        }
    }
}

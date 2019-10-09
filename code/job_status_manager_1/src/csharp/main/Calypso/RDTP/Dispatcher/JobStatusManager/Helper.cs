/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using Calypso.RDTP.Entity.Job;
using System.Collections.Generic;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// <para>
    /// This class exposes static helper functions which helps improve code readability and reduces code redundancy.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>This class is stateless and thread-safe.</threadsafety>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class Helper
    {
        /// <summary>
        /// Checks whether an object is null or not
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="name">The name of the object</param>
        /// <exception cref="ArgumentNullException">If object is null</exception>
        internal static void ValidateNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name, name + " must not be null.");
            }
        }

        /// <summary>
        /// Validates if a <see cref="JobStatus"/> instance is a valid <see cref="JobStatus"/> enum value.
        /// </summary>
        /// <param name="status">The <see cref="JobStatus"/> instance to check</param>
        /// <param name="paramName">The parameter name of the <see cref="JobStatus"/> instance</param>
        /// <exception cref="ArgumentException">If status is not a valid <see cref="JobStatus"/> enum.</exception>
        internal static void ValidateJobStatus(JobStatus status, string paramName)
        {
            if (!Enum.IsDefined(typeof(JobStatus), status))
            {
                throw new ArgumentException("Invalid JobStatus enum value.", paramName);
            }
        }

        /// <summary>
        /// Validates a collection of <see cref="ITradeActivityItem"/> instances.
        /// The collection must be not null and must not contain null elements.
        /// </summary>
        /// <param name="jobs">The collection of <see cref="ITradeActivityItem"/> instances.</param>
        /// <param name="name">The name of the collection</param>
        /// <exception cref="ArgumentException">If jobs contains null elements</exception>
        /// <exception cref="ArgumentNullException">If jobs is null</exception>
        internal static void ValidateJobCollection(IEnumerable<ITradeActivityItem> jobs, string name)
        {
            ValidateNotNull(jobs, name);

            //Check array elements for null and empty.
            IEnumerator<ITradeActivityItem> en = jobs.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current == null)
                {
                    throw new ArgumentException(name + " must not contain null elements.", name);
                }
            }
        }
    }
}

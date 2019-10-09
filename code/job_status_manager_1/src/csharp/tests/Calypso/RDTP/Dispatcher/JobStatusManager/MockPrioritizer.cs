/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Reflection;
using Calypso.RDTP.Dispatcher.JobStatusManager;
using Calypso.RDTP.Entity.Job;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// A mock implementation of IPrioritizer interface.
    /// This class comapres 2 jobs only on the basis of their creation dates.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class MockPrioritizer : IPrioritizer
    {
        /// <summary>
        /// Creates an instance of the MockPrioritizer class.
        /// </summary>
        public MockPrioritizer()
        {
        }

        /// <summary>
        /// Compares 2 jobs only on the basis of their creation dates.
        /// </summary>
        /// <param name="job1">The first job to compare</param>
        /// <param name="job2">The second job to compare</param>
        /// <returns>
        /// Greater than 0 if job1 has CreationDate greater than job2.
        /// Less than 0 if job1 has CreationDate lesser than job2.
        /// 0 otherwise.
        /// </returns>
        public int Compare(ITradeActivityItem job1, ITradeActivityItem job2)
        {
            //Compares 2 jobs only on the basis of their creation dates.
            if (job1.CreationDate != job2.CreationDate)
            {
                return job1.CreationDate > job2.CreationDate ? 1 : -1;
            }

            return 0;
        }
    }
}

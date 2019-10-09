/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Calypso.RDTP.Entity.Job;
using Calypso.RDTP.Entity.Job.Impl;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Demo for the component.
    /// </summary>
    /// <author>dfn</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class DemoTests
    {
        /// <summary>
        /// A sample job.
        /// </summary>
        ITradeActivityItem job1;

        /// <summary>
        /// Another sample job.
        /// </summary>
        ITradeActivityItem job2;

        /// <summary>
        /// A sample list of jobs.
        /// </summary>
        IList<ITradeActivityItem> list;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Create some jobs
            job1 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 2, 3, JobStatus.Queued);
            job2 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 3, 3, JobStatus.Queued);

            //Create the list of jobs
            list = new List<ITradeActivityItem>();
            list.Add(job1);
            list.Add(job2);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            job1 = null;
            job2 = null;
            list = null;
        }

        /// <summary>
        /// Demo for the component.
        /// </summary>
        [Test]
        public void TestDemo()
        {
            //Base operations

            // create JobStatusManager
            JobStatusManager jobManager = new JobStatusManager();

            // add job
            jobManager.AddJob(job1);

            // Remove job
            jobManager.RemoveJob(job1.QueueID);

            // add a list of jobs
            jobManager.AddJobs(list);

            // Remove a list of jobs
            jobManager.RemoveJobs(list);


            //Get data for various criteria

            // get concrete job (QueueID is unique job identifier)
            ITradeActivityItem item1 = jobManager.GetJobByQueueID(2);

            // Get count of jobs with given status
            int jobsCount1 = jobManager.GetJobCountByStatus(JobStatus.Queued);

            // get jobs with given status
            ReadOnlyCollection<ITradeActivityItem> jobs2 = jobManager.GetJobsByStatus(JobStatus.Queued);

            // get jobs for given service
            ReadOnlyCollection<ITradeActivityItem> jobs3 = jobManager.GetJobsByServiceID(3);

            // Get count of jobs with given status for given service
            int jobCount2 = jobManager.GetJobCountByStatusServiceID(JobStatus.Queued, 3);

            // get jobs for given service and with given status
            ReadOnlyCollection<ITradeActivityItem> jobs4 = jobManager.GetJobsByStatusAndServiceID(JobStatus.Queued, 3);

            // get jobs for given service and with JobStatus.Queued status
            ReadOnlyCollection<ITradeActivityItem> jobs5 = jobManager.GetJobsForDispatch(3);

            // we can get exclusive access to JobStatusManager to execute not thread safe operations
            lock (jobManager.SyncRoot)
            {
                foreach (ITradeActivityItem job in jobs4)
                {
                    Console.WriteLine(job.QueueID);
                }
            }


            //Monitor jobs for given criteria
            //The application can get read only wrapper for given condition once and uses it many times.
            //The wrapper provides read only access to collection of jobs for given criteria.

            // get jobs for dispatch once
            ReadOnlyCollection<ITradeActivityItem> jobs = jobManager.GetJobsForDispatch(3);

            // call by timer for example
            foreach (ITradeActivityItem job in jobs)
            {
                // process job
            }
        }
    }
}

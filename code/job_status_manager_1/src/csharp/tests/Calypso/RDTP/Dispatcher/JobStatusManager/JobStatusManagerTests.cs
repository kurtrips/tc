/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Calypso.RDTP.Dispatcher.JobStatusManager.Prioritizers;
using Calypso.RDTP.Entity.Job.Impl;
using Calypso.RDTP.Entity.Job;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Unit tests for the JobStatusManager class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class JobStatusManagerTests
    {
        /// <summary>
        /// The JobStatusManager instance to use for the tests.
        /// </summary>
        JobStatusManager jsm;

        /// <summary>
        /// A MockTradeActivityItem instance to use for adding/removing.
        /// </summary>
        MockTradeActivityItem ta1;

        /// <summary>
        /// Another MockTradeActivityItem instance to use for adding/removing.
        /// </summary>
        MockTradeActivityItem ta2;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            jsm = new JobStatusManager();
            ta1 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 2, 3, JobStatus.Queued);
            ta2 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 3, 3, JobStatus.Queued);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            jsm = null;
            ta1 = null;
            ta2 = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// JobStatusManager()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(jsm, "jobsByQueueID"),
                "jobsByQueueID must initially be non-null.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(jsm, "jobsByStatus"),
                "jobsByStatus must initially be non-null.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(jsm, "jobsByServiceId"),
                "jobsByServiceId must initially be non-null.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(jsm, "jobsByStatusAndServiceID"),
                "jobsByStatusAndServiceID must initially be non-null.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(jsm, "syncRoot"),
                "syncRoot must initially be non-null.");

            //jobPrioritizer must be set to an instance of DefaultPrioritizer
            object jobPrioritizer = UnitTestHelper.GetPrivateField(jsm, "jobPrioritizer");
            Assert.IsNotNull(jobPrioritizer, "jobPrioritizer must be non-null.");
            Assert.AreEqual(typeof(DefaultPrioritizer), jobPrioritizer.GetType(),
                "Default constructor must be set jobPrioritizer to an instance of DefaultPrioritizer.");

        }

        /// <summary>
        /// Tests the constructor.
        /// JobStatusManager(IPrioritizer prioritizer)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            jsm = new JobStatusManager(new MockPrioritizer());

            //jobPrioritizer must be set to an instance of MockPrioritizer
            object jobPrioritizer = UnitTestHelper.GetPrivateField(jsm, "jobPrioritizer");
            Assert.IsNotNull(jobPrioritizer, "jobPrioritizer must be non-null.");
            Assert.AreEqual(typeof(MockPrioritizer), jobPrioritizer.GetType(),
                "jobPrioritizer must be set to an instance of MockPrioritizer.");
        }

        /// <summary>
        /// Tests the SyncRoot property.
        /// Object SyncRoot()
        /// </summary>
        [Test]
        public void TestSyncRoot()
        {
            object syncRoot = UnitTestHelper.GetPrivateField(jsm, "syncRoot");
            Assert.IsTrue(object.ReferenceEquals(syncRoot, jsm.SyncRoot),
                "Incorrect SyncRoot property implementation.");
        }

        /// <summary>
        /// Tests the AddJob method.
        /// void AddJob(ITradeActivityItem job)
        /// </summary>
        [Test]
        public void TestAddJob1()
        {
            //Add
            jsm.AddJob(ta1);

            //Check if item was added to all indices correctly.
            Assert.AreEqual(ta1, jsm.GetJobByQueueID(2), "Wrong AddJob implementation.");
            Assert.AreEqual(ta1, jsm.GetJobsByServiceID(3)[0], "Wrong AddJob implementation.");
            Assert.AreEqual(ta1, jsm.GetJobsByStatus(JobStatus.Queued)[0], "Wrong AddJob implementation.");
            Assert.AreEqual(
                ta1, jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3)[0], "Wrong AddJob implementation.");
        }

        /// <summary>
        /// Tests the AddJob method when another item is added.
        /// void AddJob(ITradeActivityItem job)
        /// </summary>
        [Test]
        public void TestAddJob2()
        {
            //Add ta1
            TestAddJob1();

            //Add ta2
            jsm.AddJob(ta2);

            //Check if item was added to all indices correctly.
            Assert.AreEqual(ta2, jsm.GetJobByQueueID(3), "Wrong AddJob implementation.");
            Assert.AreEqual(ta2, jsm.GetJobsByServiceID(3)[1], "Wrong AddJob implementation.");
            Assert.AreEqual(ta2, jsm.GetJobsByStatus(JobStatus.Queued)[1], "Wrong AddJob implementation.");
            Assert.AreEqual(
                ta2, jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3)[1], "Wrong AddJob implementation.");
        }

        /// <summary>
        /// Tests the AddJob method for failure when job is null.
        /// void AddJob(ITradeActivityItem job)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddJobFail1()
        {
            jsm.AddJob(null);
        }

        /// <summary>
        /// Tests the AddJob method for failure when job.Status is invalid JobStatus enum value.
        /// void AddJob(ITradeActivityItem job)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddJobFail2()
        {
            ta1 = new MockTradeActivityItem("A", DateTime.Today, 1, 2, 3, (JobStatus)609);
            jsm.AddJob(ta1);
        }

        /// <summary>
        /// Tests the AddJob method for failure when job with given queueId has already been added.
        /// void AddJob(ITradeActivityItem job)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddJobFail3()
        {
            //Add ta1
            jsm.AddJob(ta1);

            //Add again
            jsm.AddJob(ta1);
        }

        /// <summary>
        /// Tests the AddJobs method.
        /// void AddJobs(IEnumerable&lt;ITradeActivityItem&gt; jobs)
        /// </summary>
        [Test]
        public void TestAddJobs()
        {
            //Create IEnumerable
            IList<ITradeActivityItem> list = new List<ITradeActivityItem>();
            list.Add(ta1);
            list.Add(ta2);

            jsm.AddJobs(list);
            //Check if items were added to all indices correctly.
            Assert.AreEqual(ta1, jsm.GetJobByQueueID(2), "Wrong AddJobs implementation.");
            Assert.AreEqual(ta2, jsm.GetJobByQueueID(3), "Wrong AddJobs implementation.");
            Assert.AreEqual(ta1, jsm.GetJobsByServiceID(3)[0], "Wrong AddJobs implementation.");
            Assert.AreEqual(ta2, jsm.GetJobsByServiceID(3)[1], "Wrong AddJobs implementation.");
            Assert.AreEqual(ta1, jsm.GetJobsByStatus(JobStatus.Queued)[0], "Wrong AddJobs implementation.");
            Assert.AreEqual(ta2, jsm.GetJobsByStatus(JobStatus.Queued)[1], "Wrong AddJobs implementation.");
            Assert.AreEqual(ta1, jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3)[0],
                "Wrong AddJobs implementation.");
            Assert.AreEqual(ta2, jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3)[1],
                "Wrong AddJobs implementation.");
        }

        /// <summary>
        /// Tests the AddJobs method when jobs is null.
        /// void AddJobs(IEnumerable&lt;ITradeActivityItem&gt; jobs)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddJobsFail1()
        {
            jsm.AddJobs(null);
        }

        /// <summary>
        /// Tests the AddJobs method when jobs contains null elements..
        /// void AddJobs(IEnumerable&lt;ITradeActivityItem&gt; jobs)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddJobsFail2()
        {
            //Create IEnumerable
            IList<ITradeActivityItem> list = new List<ITradeActivityItem>();
            list.Add(ta1);
            list.Add(null);

            jsm.AddJobs(list);
        }

        /// <summary>
        /// Tests the GetJobByQueueID method.
        /// ITradeActivityItem GetJobByQueueID(long queueID)
        /// </summary>
        [Test]
        public void TestGetJobByQueueID1()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            Assert.AreEqual(ta1, jsm.GetJobByQueueID(2), "Wrong GetJobByQueueID implementation.");
            Assert.AreEqual(ta2, jsm.GetJobByQueueID(3), "Wrong GetJobByQueueID implementation.");
        }

        /// <summary>
        /// Tests the GetJobByQueueID method when ITradeActivityItem with given queueId has not been added before.
        /// Must return null.
        /// ITradeActivityItem GetJobByQueueID(long queueID)
        /// </summary>
        [Test]
        public void TestGetJobByQueueID2()
        {
            Assert.IsNull(jsm.GetJobByQueueID(54), "Wrong GetJobByQueueID implementation.");
        }

        /// <summary>
        /// Tests the GetJobCountByStatus method.
        /// int GetJobCountByStatus(JobStatus status)
        /// </summary>
        [Test]
        public void TestGetJobCountByStatus1()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            Assert.AreEqual(2, jsm.GetJobCountByStatus(JobStatus.Queued),
                "There must be 2 elements with status JobStatus.Queued");
        }

        /// <summary>
        /// Tests the GetJobCountByStatus method when there is no job found with given status.
        /// int GetJobCountByStatus(JobStatus status)
        /// </summary>
        [Test]
        public void TestGetJobCountByStatus2()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            Assert.AreEqual(0, jsm.GetJobCountByStatus(JobStatus.Hung),
                "There must be 0 elements with status JobStatus.Hung");
        }

        /// <summary>
        /// Tests the GetJobCountByStatus method when status is invalid JobStatus enum value.
        /// int GetJobCountByStatus(JobStatus status)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGetJobCountByStatusFail1()
        {
            jsm.GetJobCountByStatus((JobStatus)876);
        }

        /// <summary>
        /// Tests the GetJobCountByStatusServiceID method.
        /// int GetJobCountByStatusServiceID(JobStatus status, long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobCountByStatusServiceID1()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            Assert.AreEqual(2, jsm.GetJobCountByStatusServiceID(JobStatus.Queued, 3),
                "There must be 2 elements with status as JobStatus.Queued and serviceId as 3.");
        }

        /// <summary>
        /// Tests the GetJobCountByStatusServiceID method when there is no record found for the
        /// given combination of the JobStatus and serviceID.
        /// Must return 0.
        /// int GetJobCountByStatusServiceID(JobStatus status, long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobCountByStatusServiceID2()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            Assert.AreEqual(0, jsm.GetJobCountByStatusServiceID(JobStatus.Queued, 2),
                "There must be 0 elements with status as JobStatus.Queued and serviceId as 2.");
        }

        /// <summary>
        /// Tests the GetJobCountByStatusServiceID method when the status is invalid JobStatus enum value.
        /// int GetJobCountByStatusServiceID(JobStatus status, long serviceID)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGetJobCountByStatusServiceIDFail1()
        {
            jsm.GetJobCountByStatusServiceID((JobStatus)34, 3);
        }

        /// <summary>
        /// Tests the GetJobsByServiceID method when jobs with given serviceId are present.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByServiceID(long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobsByServiceID1()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsByServiceID(3);

            Assert.AreEqual(2, coll.Count, "Count must be 2.");
            Assert.AreEqual(ta1, coll[0], "Incorrect GetJobsByServiceID implementation.");
            Assert.AreEqual(ta2, coll[1], "Incorrect GetJobsByServiceID implementation.");
        }

        /// <summary>
        /// Tests the GetJobsByServiceID method when no jobs with given serviceId are present.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByServiceID(long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobsByServiceID2()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsByServiceID(4);

            Assert.IsNotNull(coll, "Null must not be returned even if no jobs were found.");
            Assert.AreEqual(0, coll.Count, "Empty collection must be returned.");
        }

        /// <summary>
        /// Tests the GetJobsByStatus method when jobs with given status are present.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByStatus(JobStatus status)
        /// </summary>
        [Test]
        public void TestGetJobsByStatus1()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsByStatus(JobStatus.Queued);

            Assert.AreEqual(2, coll.Count, "Count must be 2.");
            Assert.AreEqual(ta1, coll[0], "Incorrect GetJobsByServiceID implementation.");
            Assert.AreEqual(ta2, coll[1], "Incorrect GetJobsByServiceID implementation.");
        }

        /// <summary>
        /// Tests the GetJobsByStatus method when no jobs with given status are present.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByStatus(JobStatus status)
        /// </summary>
        [Test]
        public void TestGetJobsByStatus2()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsByStatus(JobStatus.PrismUpdateError);

            Assert.IsNotNull(coll, "Null must not be returned even if no jobs were found.");
            Assert.AreEqual(0, coll.Count, "Empty collection must be returned.");
        }

        /// <summary>
        /// Tests the GetJobsByStatus method status is invalid JobStatus enum value.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByStatus(JobStatus status)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGetJobsByStatusFail1()
        {
            jsm.GetJobsByStatus((JobStatus)4345);
        }

        /// <summary>
        /// Tests the GetJobsByStatusAndServiceID method when jobs with given status and serviceId are present.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByStatusAndServiceID(JobStatus status, long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobsByStatusAndServiceID1()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3);

            Assert.AreEqual(2, coll.Count, "Count must be 2.");
            Assert.AreEqual(ta1, coll[0], "Incorrect GetJobsByServiceID implementation.");
            Assert.AreEqual(ta2, coll[1], "Incorrect GetJobsByServiceID implementation.");
        }

        /// <summary>
        /// Tests the GetJobsByStatusAndServiceID method when jobs with given status and serviceId are not present.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByStatusAndServiceID(JobStatus status, long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobsByStatusAndServiceID2()
        {
            //Add items ta1 and ta2
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsByStatusAndServiceID(JobStatus.Hung, 3);

            Assert.IsNotNull(coll, "Null must not be returned even if no jobs were found.");
            Assert.AreEqual(0, coll.Count, "Empty collection must be returned.");
        }

        /// <summary>
        /// Tests the GetJobsByStatusAndServiceID method status is invalid JobStatus enum value.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsByStatusAndServiceID(JobStatus status, long serviceID)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGetJobsByStatusAndServiceIDFail1()
        {
            jsm.GetJobsByStatusAndServiceID((JobStatus)4345, 3);
        }

        /// <summary>
        /// Tests the GetJobsForDispatch method when there are jobs meant for dispatch.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsForDispatch(long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobsForDispatch1()
        {
            //Add items ta1 and ta2 (both are meant for dispatch)
            jsm.AddJob(ta1);
            jsm.AddJob(ta2);

            //Add another item. It's not meant for dispatch.
            jsm.AddJob(new MockTradeActivityItem("ACC", DateTime.Today, 1, 4, 3, JobStatus.Dispatching));

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsForDispatch(3);

            Assert.AreEqual(2, coll.Count, "Count must be 2.");
            Assert.AreEqual(ta1, coll[0], "Incorrect GetJobsByServiceID implementation.");
            Assert.AreEqual(ta2, coll[1], "Incorrect GetJobsByServiceID implementation.");
        }

        /// <summary>
        /// Tests the GetJobsForDispatch method when there are no jobs meant for dispatch.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; GetJobsForDispatch(long serviceID)
        /// </summary>
        [Test]
        public void TestGetJobsForDispatch2()
        {
            //Add item. It's not meant for dispatch.
            jsm.AddJob(new MockTradeActivityItem("ACC", DateTime.Today, 1, 4, 3, JobStatus.Dispatching));

            ReadOnlyCollection<ITradeActivityItem> coll = jsm.GetJobsForDispatch(3);

            Assert.IsNotNull(coll, "Null must not be returned even if no jobs were found.");
            Assert.AreEqual(0, coll.Count, "Empty collection must be returned.");
        }


        /// <summary>
        /// Tests the RemoveJob method.
        /// ITradeActivityItem RemoveJob(long queueID)
        /// </summary>
        [Test]
        public void TestRemoveJob()
        {
            //Add ta1 and ta2
            TestAddJob2();

            //Remove ta1
            ITradeActivityItem item = jsm.RemoveJob(2);
            Assert.IsTrue(object.ReferenceEquals(item, ta1), "The removed instance must be same as ta1");
            //Check if item was removed from all indices correctly.
            Assert.IsNull(jsm.GetJobByQueueID(2), "Wrong RemoveJob implementation.");
            Assert.AreEqual(1, jsm.GetJobsByServiceID(3).Count, "Wrong RemoveJob implementation.");
            Assert.AreEqual(1, jsm.GetJobsByStatus(JobStatus.Queued).Count, "Wrong RemoveJob implementation.");
            Assert.AreEqual(
                1, jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3).Count, "Wrong RemoveJob implementation.");

            //Try to Remove ta1 again
            item = jsm.RemoveJob(2);
            Assert.IsNull(item, "Must return null if job with given queueId is not present.");

            //Remove ta2
            item = jsm.RemoveJob(3);
            Assert.IsTrue(object.ReferenceEquals(item, ta2), "The removed instance must be same as ta1");
            //Check if item was removed from all indices correctly.
            Assert.IsNull(jsm.GetJobByQueueID(3), "Wrong RemoveJob implementation.");
            Assert.AreEqual(0, jsm.GetJobsByServiceID(3).Count, "Wrong RemoveJob implementation.");
            Assert.AreEqual(0, jsm.GetJobsByStatus(JobStatus.Queued).Count, "Wrong RemoveJob implementation.");
            Assert.AreEqual(
                0, jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3).Count, "Wrong RemoveJob implementation.");

            //Try to Remove ta2 again
            item = jsm.RemoveJob(3);
            Assert.IsNull(item, "Must return null if job with given queueId is not present.");
        }

        /// <summary>
        /// Tests the RemoveJobs method.
        /// void RemoveJobs(IEnumerable&lt;ITradeActivityItem&gt; jobs)
        /// </summary>
        [Test]
        public void TestRemoveJobs()
        {
            //Add ta1 and ta2
            TestAddJob2();

            IList<ITradeActivityItem> list = new List<ITradeActivityItem>();
            list.Add(ta1);
            list.Add(ta2);

            jsm.RemoveJobs(list);

            //All indices must now be empty.
            Assert.IsNull(jsm.GetJobByQueueID(2), "Wrong RemoveJob implementation.");
            Assert.IsNull(jsm.GetJobByQueueID(3), "Wrong RemoveJob implementation.");
            Assert.AreEqual(0, jsm.GetJobsByServiceID(3).Count, "Wrong RemoveJob implementation.");
            Assert.AreEqual(0, jsm.GetJobsByStatus(JobStatus.Queued).Count, "Wrong RemoveJob implementation.");
            Assert.AreEqual(
                0, jsm.GetJobsByStatusAndServiceID(JobStatus.Queued, 3).Count, "Wrong RemoveJob implementation.");
        }

        /// <summary>
        /// Tests the RemoveJobs method when jobs is null.
        /// void RemoveJobs(IEnumerable&lt;ITradeActivityItem&gt; jobs)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException (typeof(ArgumentNullException))]
        public void TestRemoveJobsFail1()
        {
            jsm.RemoveJobs(null);
        }

        /// <summary>
        /// Tests the RemoveJobs method when jobs contains null elements.
        /// void RemoveJobs(IEnumerable&lt;<see cref="ITradeActivityItem"/>&gt; jobs)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestRemoveJobsFail2()
        {
            IList<ITradeActivityItem> list = new List<ITradeActivityItem>();
            list.Add(ta1);
            list.Add(null);
            jsm.RemoveJobs(list);
        }

        /// <summary>
        /// Tests the HandleStatusChange method.
        /// void HandleStatusChange(object sender, StatusEventArgs e)
        /// </summary>
        [Test]
        public void TestHandleStatusChange()
        {
            //Add an item
            jsm.AddJob(ta1);

            Assert.AreEqual(1, jsm.GetJobsByStatus(JobStatus.Queued).Count,
                "There must be 1 entry for JobStatus.Queued status.");
            Assert.AreEqual(0, jsm.GetJobsByStatus(JobStatus.InCalculation).Count,
                "There must be no entry for JobStatus.InCalculation status.");

            //Change the status of ta1. This invokes the HandleStatusChange event handler.
            ta1.SetStatus(JobStatus.InCalculation);

            Assert.AreEqual(0, jsm.GetJobsByStatus(JobStatus.Queued).Count,
                "After status change, there must be no entry for JobStatus.Queued status.");
            Assert.AreEqual(1, jsm.GetJobsByStatus(JobStatus.InCalculation).Count,
                "After status change, there must be 1 entry for JobStatus.InCalculation status.");
        }

        /// <summary>
        /// Tests the HandleStatusChange method for failure when sender is null.
        /// void HandleStatusChange(object sender, StatusEventArgs e)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestHandleStatusChangeFail1()
        {
            //Add an item to register handler
            jsm.AddJob(ta1);

            ta1.SetStatusFail1();
        }

        /// <summary>
        /// Tests the HandleStatusChange method for failure when e is null.
        /// void HandleStatusChange(object sender, StatusEventArgs e)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestHandleStatusChangeFail2()
        {
            //Add an item to register handler
            jsm.AddJob(ta1);

            ta1.SetStatusFail2();
        }

        /// <summary>
        /// Tests the HandleStatusChange method for failure sender not of type <see cref="ITradeActivityItem"/>
        /// void HandleStatusChange(object sender, StatusEventArgs e)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestHandleStatusChangeFail3()
        {
            //Add an item to register handler
            jsm.AddJob(ta1);

            ta1.SetStatusFail3();
        }
    }
}

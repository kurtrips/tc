/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Reflection;
using Calypso.RDTP.Dispatcher.JobStatusManager.Prioritizers;
using Calypso.RDTP.Entity.Job.Impl;
using Calypso.RDTP.Entity.Job;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Unit tests for the JobComparer class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class JobComparerTests
    {
        /// <summary>
        /// The JobComparer instance to use for the tests.
        /// </summary>
        IPrioritizer jc;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            jc = UnitTestHelper.GetJobComparerInstance(new DefaultPrioritizer());
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            jc = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// JobComparer(IPrioritizer prioritizer)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            //Check type
            Assert.IsTrue(jc is IPrioritizer, "Wrong type of JobComparer class.");

            //prioritizer field must not be null
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(jc, "prioritizer"),
                "prioritizer field must not be null.");
        }

        /// <summary>
        /// Tests the constructor when prioritizer is null.
        /// JobComparer(IPrioritizer prioritizer)
        /// ArgumentNullException is expected as inner exception of TargetInvocationException
        /// </summary>
        [Test]
        public void TestConstructorFail1()
        {
            try
            {
                jc = UnitTestHelper.GetJobComparerInstance(null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is TargetInvocationException, "TargetInvocationException must be thrown.");
                Assert.IsTrue(e.InnerException is ArgumentNullException,
                    "Inner exception must be ArgumentNullException.");
            }
        }

        /// <summary>
        /// Tests the Compare method when the jobs are different because of their creation date.
        /// This test uses a JobComparer based on the MockPrioritizer.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare1()
        {
            MockTradeActivityItem ta1 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 2, 3, JobStatus.Queued);
            MockTradeActivityItem ta2 = new MockTradeActivityItem(
                "MTM", DateTime.Today.AddDays(-1), 1, 2, 3, JobStatus.Queued);

            jc = UnitTestHelper.GetJobComparerInstance(new MockPrioritizer());

            Assert.IsTrue(jc.Compare(ta1, ta2) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when the jobs are different because of their queueIds.
        /// This test uses a JobComparer based on the MockPrioritizer.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare2()
        {
            MockTradeActivityItem ta1 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 200, 3, JobStatus.Queued);
            MockTradeActivityItem ta2 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 100, 3, JobStatus.Queued);

            jc = UnitTestHelper.GetJobComparerInstance(new MockPrioritizer());

            Assert.IsTrue(jc.Compare(ta1, ta2) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when both CreationDate and QueueId of jobs are same.
        /// This test uses a JobComparer based on the MockPrioritizer.
        /// Therefore for the jobs to be considered same, the CreationDate and QueueId of jobs must be same.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare3()
        {
            MockTradeActivityItem ta1 = new MockTradeActivityItem(
                "ACC", DateTime.Today, 2341, 100, 3, JobStatus.Queued);
            MockTradeActivityItem ta2 = new MockTradeActivityItem(
                "MTM", DateTime.Today, 1, 100, 324, JobStatus.Error);

            jc = UnitTestHelper.GetJobComparerInstance(new MockPrioritizer());

            Assert.IsTrue(jc.Compare(ta1, ta2) == 0, "Wrong Compare method implementation.");
        }
    }
}

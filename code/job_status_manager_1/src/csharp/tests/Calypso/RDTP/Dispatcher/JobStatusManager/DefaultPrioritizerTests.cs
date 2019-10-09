/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using Calypso.RDTP.Entity.Job;
using Calypso.RDTP.Entity.Job.Impl;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager.Prioritizers
{
    /// <summary>
    /// Unit tests for the DefaultPrioritizer class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class DefaultPrioritizerTests
    {
        /// <summary>
        /// The DefaultPrioritizer instance to use for the tests.
        /// </summary>
        DefaultPrioritizer dp;

        /// <summary>
        /// The first parameter to be passed to the Compare function of the DefaultPrioritizer class.
        /// </summary>
        MockTradeActivityItem job1;

        /// <summary>
        /// The second parameter to be passed to the Compare function of the DefaultPrioritizer class.
        /// </summary>
        MockTradeActivityItem job2;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            dp = new DefaultPrioritizer();
            job1 = new MockTradeActivityItem("MTM", DateTime.Today.AddDays(1), 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 2, 1002, 2002, JobStatus.InCalculation);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            dp = null;
            job1 = null;
            job2 = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// DefaultPrioritizer()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsTrue(dp is IPrioritizer, "Wrong type of DefaultPrioritizer.");
        }

        /// <summary>
        /// Tests the Compare method when job and otherJob are null.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare1()
        {
            Assert.AreEqual(0, dp.Compare(null, null), "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job is null and otherJob is non-null.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare2()
        {
            Assert.IsTrue(dp.Compare(null, job2) < 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job is non-null and otherJob is null.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare3()
        {
            Assert.IsTrue(dp.Compare(job1, null) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has ServiceID greater than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare4()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2002, JobStatus.InCalculation);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.InCalculation);

            Assert.IsTrue(dp.Compare(job1, job2) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has ServiceID lesser than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare5()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.InCalculation);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2002, JobStatus.InCalculation);

            Assert.IsTrue(dp.Compare(job1, job2) < 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has Status (as integer) greater than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare6()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.InCalculation);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has Status (as integer) lesser than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare7()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.InCalculation);

            Assert.IsTrue(dp.Compare(job1, job2) < 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has Priority greater than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare8()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 2, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has Priority lesser than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare9()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 2, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) < 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has AccountingTreatment greater than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare10()
        {
            job1 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has AccountingTreatment lesser than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare11()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("MTM", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) < 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has CreationDate greater than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare12()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today.AddDays(1), 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) > 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job has CreationDate lesser than otherJob.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare13()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today.AddDays(1), 1, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) < 0, "Wrong Compare method implementation.");
        }

        /// <summary>
        /// Tests the Compare method when job and otherJob are equal.
        /// int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
        /// </summary>
        [Test]
        public void TestCompare14()
        {
            job1 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);
            job2 = new MockTradeActivityItem("ACC", DateTime.Today, 1, 1001, 2001, JobStatus.Queued);

            Assert.IsTrue(dp.Compare(job1, job2) == 0, "Wrong Compare method implementation.");
        }
    }
}

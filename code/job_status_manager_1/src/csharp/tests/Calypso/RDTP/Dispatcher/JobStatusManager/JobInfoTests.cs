/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using Calypso.RDTP.Entity.Job;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Unit tests for the JobInfo class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class JobInfoTests
    {
        /// <summary>
        /// The JobInfo instance to use for the tests.
        /// </summary>
        JobInfo ji;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ji = new JobInfo(1001, JobStatus.Queued);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ji = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// JobInfo(long serviceID, JobStatus status)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsTrue(ji is IEquatable<JobInfo>, "Wrong type of JobInfo class.");

            //Status and serviceId must be set properly.
            Assert.AreEqual(1001, ji.ServiceID, "Wrong ServiceID property implementation.");
            Assert.AreEqual(JobStatus.Queued, ji.Status, "Wrong Status property implementation.");
        }

        /// <summary>
        /// Tests the constructor for failure when status is not a valid JobStatus enum.
        /// JobInfo(long serviceID, JobStatus status)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail1()
        {
            ji = new JobInfo(101, (JobStatus)102);
        }

        /// <summary>
        /// Tests the ServiceID property.
        /// long ServiceID()
        /// </summary>
        [Test]
        public void TestServiceID()
        {
            Assert.AreEqual(1001, ji.ServiceID, "Wrong ServiceID property implementation.");
        }

        /// <summary>
        /// Tests the Status property.
        /// JobStatus Status()
        /// </summary>
        [Test]
        public void TestStatus()
        {
            Assert.AreEqual(JobStatus.Queued, ji.Status, "Wrong Status property implementation.");
        }

        /// <summary>
        /// Tests the Equals method when other is null. Must return false.
        /// bool Equals(JobInfo other)
        /// </summary>
        [Test]
        public void TestEquals1()
        {
            Assert.IsFalse(ji.Equals((JobInfo)null), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when serviceID of other is not same as serviceID of current JobInfo.
        /// Must return false.
        /// bool Equals(JobInfo other)
        /// </summary>
        [Test]
        public void TestEquals2()
        {
            JobInfo ji2 = new JobInfo(1002, JobStatus.Queued);
            Assert.IsFalse(ji.Equals(ji2), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when status of other is not same as status of current JobInfo.
        /// Must return false.
        /// bool Equals(JobInfo other)
        /// </summary>
        [Test]
        public void TestEquals3()
        {
            JobInfo ji2 = new JobInfo(1001, JobStatus.ResultNoticeRecieved);
            Assert.IsFalse(ji.Equals(ji2), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when status and serviceId of other is same as that of current JobInfo.
        /// Must return false.
        /// bool Equals(JobInfo other)
        /// </summary>
        [Test]
        public void TestEquals4()
        {
            JobInfo ji2 = new JobInfo(1001, JobStatus.Queued);
            Assert.IsTrue(ji.Equals(ji2), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when other is null. Must return false
        /// bool Equals(Object other)
        /// </summary>
        [Test]
        public void TestEquals5()
        {
            Assert.IsFalse(ji.Equals(null), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when other is not of type JobInfo. Must return false
        /// bool Equals(Object other)
        /// </summary>
        [Test]
        public void TestEquals6()
        {
            object a = new object();
            Assert.IsFalse(ji.Equals(a), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when serviceID of other is not same as serviceID of current JobInfo.
        /// Must return false.
        /// bool Equals(object other)
        /// </summary>
        [Test]
        public void TestEquals7()
        {
            JobInfo ji2 = new JobInfo(1002, JobStatus.Queued);
            Assert.IsFalse(ji.Equals((object)ji2), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when status of other is not same as status of current JobInfo.
        /// Must return false.
        /// bool Equals(object other)
        /// </summary>
        [Test]
        public void TestEquals8()
        {
            JobInfo ji2 = new JobInfo(1001, JobStatus.ResultNoticeRecieved);
            Assert.IsFalse(ji.Equals((object)ji2), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the Equals method when status and serviceId of other is same as that of current JobInfo.
        /// Must return false.
        /// bool Equals(JobInfo other)
        /// </summary>
        [Test]
        public void TestEquals9()
        {
            JobInfo ji2 = new JobInfo(1001, JobStatus.Queued);
            Assert.IsTrue(ji.Equals((object)ji2), "Wrong Equals method implementation.");
        }

        /// <summary>
        /// Tests the GetHashCode method.
        /// int GetHashCode()
        /// </summary>
        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(ji.ServiceID.GetHashCode() ^ ji.Status.GetHashCode(), ji.GetHashCode(),
                "Wrong GetHashCode implementation.");
        }
    }
}

/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using Calypso.RDTP.Entity.Job;
using Calypso.RDTP.Entity.Job.Impl;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Unit tests for the Helper class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class HelperTests
    {
        /// <summary>
        /// Tests the ValidateNotNull when object is not null.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestValidateNotNull1()
        {
            Helper.ValidateNotNull(new object(), "a");
        }

        /// <summary>
        /// Tests the ValidateNotNull when object is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNull2()
        {
            Helper.ValidateNotNull(null, "a");
        }

        /// <summary>
        /// Tests the ValidateJobStatus when status is a valid JobStatus
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestValidateJobStatus1()
        {
            Helper.ValidateJobStatus((JobStatus)2, "a");
        }

        /// <summary>
        /// Tests the ValidateJobStatus when status is a invalid JobStatus
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateJobStatus2()
        {
            Helper.ValidateJobStatus((JobStatus)1234, "a");
        }

        /// <summary>
        /// Tests the ValidateJobCollection when jobs is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateJobCollection1()
        {
            IList<ITradeActivityItem> list = null;
            Helper.ValidateJobCollection(list, "As");
        }

        /// <summary>
        /// Tests the ValidateJobCollection when jobs has null elements.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateJobCollection2()
        {
            IList<ITradeActivityItem> list = new List<ITradeActivityItem>();
            list.Add(new MockTradeActivityItem("A", DateTime.Now, 1, 2, 3, JobStatus.Queued));
            list.Add(null);
            Helper.ValidateJobCollection(list, "AS");
        }

        /// <summary>
        /// Tests the ValidateJobCollection when jobs is not null and has no null elements.
        /// </summary>
        [Test]
        public void TestValidateJobCollection3()
        {
            IList<ITradeActivityItem> list = new List<ITradeActivityItem>();
            list.Add(new MockTradeActivityItem("A", DateTime.Now, 1, 2, 3, JobStatus.Queued));
            Helper.ValidateJobCollection(list, "sdh");
        }
    }
}

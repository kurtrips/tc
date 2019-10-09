/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Calypso.RDTP.Dispatcher.JobStatusManager.Prioritizers;
using Calypso.RDTP.Entity.Job;
using Calypso.RDTP.Entity.Job.Impl;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Unit tests for the JobSortedCollection class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class JobSortedCollectionTests
    {
        /// <summary>
        /// The JobSortedCollection instance to use for the tests.
        /// </summary>
        JobSortedCollection jsc;

        /// <summary>
        /// A test ITradeActivityItem item.
        /// </summary>
        ITradeActivityItem ta1;

        /// <summary>
        /// A test ITradeActivityItem item.
        /// </summary>
        ITradeActivityItem ta2;

        /// <summary>
        /// A test ITradeActivityItem item.
        /// </summary>
        ITradeActivityItem ta3;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            jsc = new JobSortedCollection(new DefaultPrioritizer());

            ta1 = new MockTradeActivityItem("AS", DateTime.Today, 1, 2, 3, JobStatus.Queued);
            ta2 = new MockTradeActivityItem("AS", DateTime.Today.AddDays(-2), 1, 2, 3, JobStatus.Queued);
            ta3 = new MockTradeActivityItem("AS", DateTime.Today.AddDays(-1), 1, 2, 3, JobStatus.Queued);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            jsc = null;
            ta2 = null;
            ta1 = null;
            ta3 = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// JobSortedCollection(IPrioritizer prioritizer)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            //Check type
            Assert.IsTrue(jsc is ICollection<ITradeActivityItem>, "Wrong type of JobSortedCollection class.");

            //items field must not be null initially.
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(jsc, "items"), "items field must not be null initially.");
        }

        /// <summary>
        /// Tests the Count property.
        /// int Count()
        /// </summary>
        [Test]
        public void TestCount()
        {
            Assert.AreEqual(0, jsc.Count, "Initially count must be zero.");

            jsc.Add(ta1);
            Assert.AreEqual(1, jsc.Count, "Count must now be one.");

            jsc.Add(ta2);
            Assert.AreEqual(2, jsc.Count, "Count must now be two.");
        }

        /// <summary>
        /// Tests the IsReadOnly property.
        /// bool IsReadOnly()
        /// </summary>
        [Test]
        public void TestIsReadOnly()
        {
            Assert.IsFalse(jsc.IsReadOnly, "Must always be false.");
        }

        /// <summary>
        /// Tests the Add method. The items must be added into appropriate locations.
        /// void Add(ITradeActivityItem item)
        /// </summary>
        [Test]
        public void TestAdd()
        {
            Assert.AreEqual(0, jsc.Count, "Initially count must be zero.");

            jsc.Add(ta1);
            Assert.AreEqual(1, jsc.Count, "Count must now be one.");

            jsc.Add(ta2);
            Assert.AreEqual(2, jsc.Count, "Count must now be two.");
            //This item must be added at index 0 as its creation date is lesser than that of ta1
            List<ITradeActivityItem> items = UnitTestHelper.GetPrivateField(jsc, "items") as List<ITradeActivityItem>;
            Assert.AreEqual(0, items.IndexOf(ta2), "Item was inserted at wrong position.");

            jsc.Add(ta3);
            Assert.AreEqual(3, jsc.Count, "Count must now be three.");
            //This item must be added at index 1 as its creation date is lesser than that of ta1 but greater than ta2
            Assert.AreEqual(1, items.IndexOf(ta3), "Item was inserted at wrong position.");
        }

        /// <summary>
        /// Tests the Add method when item is null.
        /// void Add(ITradeActivityItem item)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddFail1()
        {
            jsc.Add(null);
        }

        /// <summary>
        /// Tests the Add method when item.Status is not a valid JobStatus enum.
        /// void Add(ITradeActivityItem item)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddFail2()
        {
            MockTradeActivityItem ta = new MockTradeActivityItem("A", DateTime.Today, 1, 2, 3, (JobStatus)100);
            jsc.Add(ta);
        }

        /// <summary>
        /// Tests the Clear method.
        /// void Clear()
        /// </summary>
        [Test]
        public void TestClear()
        {
            //Add a few items
            TestAdd();
            Assert.AreEqual(3, jsc.Count, "Incorrect Add implementation.");

            //Clear
            jsc.Clear();
            Assert.AreEqual(0, jsc.Count, "Incorrect Clear implementation.");
        }

        /// <summary>
        /// Tests the Contains method.
        /// bool Contains(ITradeActivityItem item)
        /// </summary>
        [Test]
        public void TestContains()
        {
            //Add an item
            jsc.Add(ta1);
            Assert.IsTrue(jsc.Contains(ta1), "Must return true.");

            Assert.IsFalse(jsc.Contains(ta2), "Must return false.");

            //Add another item
            jsc.Add(ta2);
            Assert.IsTrue(jsc.Contains(ta2), "Must return true.");
        }

        /// <summary>
        /// Tests the Contains method when item is null.
        /// bool Contains(ITradeActivityItem item)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestContainsFail1()
        {
            jsc.Contains(null);
        }

        /// <summary>
        /// Tests the CopyTo method.
        /// void CopyTo(ITradeActivityItem[] array, int index)
        /// </summary>
        [Test]
        public void TestCopyTo()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);
            jsc.Add(ta3);

            ITradeActivityItem[] array = new ITradeActivityItem[6];
            jsc.CopyTo(array, 2);
            Assert.IsNull(array[0], "Copying must be from index 2.");
            Assert.IsNull(array[1], "Copying must be from index 2.");

            //Note the order is ta2, ta3, ta1
            Assert.AreEqual(ta2, array[2], "Incorrect CopyTo implementation.");
            Assert.AreEqual(ta3, array[3], "Incorrect CopyTo implementation.");
            Assert.AreEqual(ta1, array[4], "Incorrect CopyTo implementation.");

            Assert.IsNull(array[5], "Copying must be from index 2.");
        }

        /// <summary>
        /// Tests the CopyTo method when array is null.
        /// void CopyTo(ITradeActivityItem[] array, int index)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCopyToFail1()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);
            jsc.Add(ta3);

            jsc.CopyTo(null, 0);
        }

        /// <summary>
        /// Tests the CopyTo method when index is less than 0.
        /// void CopyTo(ITradeActivityItem[] array, int index)
        /// ArgumentOutOfRangeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestCopyToFail2()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);
            jsc.Add(ta3);

            ITradeActivityItem[] array = new ITradeActivityItem[6];
            jsc.CopyTo(array, -1);
        }

        /// <summary>
        /// Tests the CopyTo method when index is equal to the length of array.
        /// void CopyTo(ITradeActivityItem[] array, int index)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestCopyToFail3()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);
            jsc.Add(ta3);

            ITradeActivityItem[] array = new ITradeActivityItem[6];
            jsc.CopyTo(array, 6);
        }

        /// <summary>
        /// Tests the CopyTo method when the number of elements in the source collection is greater than
        /// the available space from index to the end of the destination array.
        /// void CopyTo(ITradeActivityItem[] array, int index)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestCopyToFail4()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);
            jsc.Add(ta3);

            ITradeActivityItem[] array = new ITradeActivityItem[6];
            jsc.CopyTo(array, 4);
        }

        /// <summary>
        /// Tests the Remove method.
        /// bool Remove(ITradeActivityItem item)
        /// </summary>
        [Test]
        public void TestRemove()
        {
            //Add a few items. ta3 is not added
            jsc.Add(ta1);
            jsc.Add(ta2);

            Assert.AreEqual(2, jsc.Count, "Count must be 2.");

            Assert.IsTrue(jsc.Remove(ta1), "Incorrect Remove implementation.");
            Assert.AreEqual(1, jsc.Count, "Count must now be 1.");

            Assert.IsFalse(jsc.Remove(ta1), "Incorrect Remove implementation.");
            Assert.AreEqual(1, jsc.Count, "Count must still be 1.");

            Assert.IsTrue(jsc.Remove(ta2), "Incorrect Remove implementation.");
            Assert.AreEqual(0, jsc.Count, "Count must now be 0.");
        }

        /// <summary>
        /// Tests the Remove method for failure when item is null.
        /// bool Remove(ITradeActivityItem item)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveFail1()
        {
            jsc.Remove(null);
        }

        /// <summary>
        /// Tests the GetEnumerator method.
        /// IEnumerator&lt;ITradeActivityItem&gt; GetEnumerator()
        /// </summary>
        [Test]
        public void TestGetEnumerator1()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);
            jsc.Add(ta3);

            IEnumerator<ITradeActivityItem> en = jsc.GetEnumerator();
            List<ITradeActivityItem> list = new List<ITradeActivityItem>();
            while (en.MoveNext())
            {
                list.Add(en.Current);
            }

            Assert.AreEqual(ta2, list[0], "Incorrect GetEnumerator implementation.");
            Assert.AreEqual(ta3, list[1], "Incorrect GetEnumerator implementation.");
            Assert.AreEqual(ta1, list[2], "Incorrect GetEnumerator implementation.");
        }

        /// <summary>
        /// Tests the GetEnumerator method.
        /// IEnumerator System.Collections.IEnumerable.GetEnumerator()
        /// </summary>
        [Test]
        public void TestGetEnumerator2()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);

            IEnumerator en = (jsc as IEnumerable).GetEnumerator();
            List<ITradeActivityItem> list = new List<ITradeActivityItem>();
            while (en.MoveNext())
            {
                list.Add(en.Current as ITradeActivityItem);
            }

            Assert.AreEqual(ta2, list[0], "Incorrect GetEnumerator implementation.");
            Assert.AreEqual(ta1, list[1], "Incorrect GetEnumerator implementation.");
        }

        /// <summary>
        /// Tests the op_Implicit method.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; op_Implicit(JobSortedCollection collection)
        /// </summary>
        [Test]
        public void Testop_Implicit()
        {
            //Add a few items
            jsc.Add(ta1);
            jsc.Add(ta2);
            jsc.Add(ta3);

            ReadOnlyCollection<ITradeActivityItem> roc = jsc;
            Assert.AreEqual(ta2, roc[0], "Wrong implicit operator implementation.");
            Assert.AreEqual(ta3, roc[1], "Wrong implicit operator implementation.");
            Assert.AreEqual(ta1, roc[2], "Wrong implicit operator implementation.");
        }

        /// <summary>
        /// Tests the op_Implicit method when input collection is null.
        /// ReadOnlyCollection&lt;ITradeActivityItem&gt; op_Implicit(JobSortedCollection collection)
        /// </summary>
        [Test]
        public void Testop_Implicit2()
        {
            ReadOnlyCollection<ITradeActivityItem> roc = (JobSortedCollection)null;
            Assert.IsNull(roc, "Wrong implicit operator implementation for input null.");
        }
    }
}

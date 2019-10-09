// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.SecurityDataCombiners
{
    /// <summary>
    /// Unit tests for the DefaultSecurityDataCombiner class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DefaultSecurityDataCombinerTests
    {
        /// <summary>
        /// The DefaultSecurityDataCombiner instance to use for the tests.
        /// </summary>
        DefaultSecurityDataCombiner dsdc;

        /// <summary>
        /// The first SecurityData instance to use
        /// </summary>
        SecurityData first;

        /// <summary>
        /// The second SecurityData instance to use
        /// </summary>
        SecurityData second;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            dsdc = new DefaultSecurityDataCombiner();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            dsdc = null;
            first = null;
            second = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// DefaultSecurityDataCombiner()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsTrue(dsdc is ISecurityDataCombiner, "Wrong type of DefaultSecurityDataCombiner.");
        }

        /// <summary>
        /// Tests the Combine method.
        /// SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// </summary>
        [Test]
        public void TestCombineAccuracy1()
        {
            first = new SecurityData("A", "Name", new string[] { "B", "C" });
            second = new SecurityData("B", "Name", new string[] { "D", "C" });

            SecurityData combined = dsdc.Combine(first, second);

            Assert.AreEqual(combined.Id, "A", "Wrong Combine implementation.");
            Assert.AreEqual(combined.CompanyName, "Name", "Wrong Combine implementation.");
            Assert.IsTrue(
                UnitTestHelper.AreReferenceIdsEqual(combined.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Combine implementation.");
        }

        /// <summary>
        /// Tests the Combine method. Here
        /// A -> A,B
        /// B -> A,C
        /// SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// </summary>
        [Test]
        public void TestCombineAccuracy2()
        {
            first = new SecurityData("A", "Name", new string[] { "B", "A" });
            second = new SecurityData("B", "Name", new string[] { "A", "C" });

            SecurityData combined = dsdc.Combine(first, second);

            Assert.AreEqual(combined.Id, "A", "Wrong Combine implementation.");
            Assert.AreEqual(combined.CompanyName, "Name", "Wrong Combine implementation.");
            Assert.IsTrue(
                UnitTestHelper.AreReferenceIdsEqual(combined.ReferenceIds, new string[] { "A", "B", "C" }),
                "Wrong Combine implementation.");
        }

        /// <summary>
        /// Tests the Combine method. Here
        /// A -> B,B,A,C
        /// B -> A,C,D
        /// The resultant referenceIds must have unique entries.
        /// SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// </summary>
        [Test]
        public void TestCombineAccuracy3()
        {
            first = new SecurityData("A", "Name", new string[] { "B", "B", "A", "C" });
            second = new SecurityData("B", "Name", new string[] { "A", "C", "D" });

            SecurityData combined = dsdc.Combine(first, second);

            Assert.AreEqual(combined.Id, "A", "Wrong Combine implementation.");
            Assert.AreEqual(combined.CompanyName, "Name", "Wrong Combine implementation.");
            Assert.IsTrue(
                UnitTestHelper.AreReferenceIdsEqual(combined.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Combine implementation.");
        }

        /// <summary>
        /// Tests the Combine method. Here
        /// A -> B
        /// C -> D
        /// Note that even if securityIds are not related still they are combined. It is upto the using
        /// code to not combine when securities are not related.
        /// SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// </summary>
        [Test]
        public void TestCombineAccuracy4()
        {
            first = new SecurityData("A", "Name", new string[] { "B" });
            second = new SecurityData("C", "Name", new string[] { "D" });

            SecurityData combined = dsdc.Combine(first, second);

            Assert.AreEqual(combined.Id, "A", "Wrong Combine implementation.");
            Assert.AreEqual(combined.CompanyName, "Name", "Wrong Combine implementation.");
            Assert.IsTrue(
                UnitTestHelper.AreReferenceIdsEqual(combined.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Combine implementation.");
        }

        /// <summary>
        /// Tests the Combine method for failure when firstSecurityData is null.
        /// SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCombineFailure1()
        {
            first = null;
            second = new SecurityData("C", "Name", new string[] { "D" });
            dsdc.Combine(first, second);
        }

        /// <summary>
        /// Tests the Combine method for failure when secondSecurityData is null.
        /// SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCombineFailure2()
        {
            second = null;
            first = new SecurityData("C", "Name", new string[] { "D" });
            dsdc.Combine(first, second);
        }
    }
}

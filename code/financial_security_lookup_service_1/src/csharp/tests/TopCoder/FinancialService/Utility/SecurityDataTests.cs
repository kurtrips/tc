// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the SecurityData class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SecurityDataTests
    {
        /// <summary>
        /// The SecurityData instance to use for the tests.
        /// </summary>
        SecurityData sd;

        /// <summary>
        /// The referenceIds to use for the SecurityData
        /// </summary>
        string[] refIds;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            refIds = new string[] { "C", "B" };
            sd = new SecurityData("A", "Name", refIds);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sd = null;
            refIds = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// SecurityData(string id, string companyName)
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            sd = new SecurityData("A", "Name");

            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(sd, "id"), "A", "Wrong constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(sd, "companyName"), "Name",
                "Wrong constructor implementation.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateFieldValue(sd, "referenceIds"),
                "Wrong constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor.
        /// SecurityData(string id, string companyName, String[] referenceIds)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(sd, "id"), "A", "Wrong constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(sd, "companyName"), "Name",
                "Wrong constructor implementation.");

            //References of the 2 arrays must not be same as a shallow copy is made.
            Assert.IsFalse(object.ReferenceEquals(UnitTestHelper.GetPrivateFieldValue(sd, "referenceIds"), refIds),
                "Wrong constructor implementation.");

            //But the contents must be same
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(refIds,
                UnitTestHelper.GetPrivateFieldValue(sd, "referenceIds") as string[]),
                "Wrong constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor for failure when id is null.
        /// SecurityData(string id, string companyName)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail11()
        {
            sd = new SecurityData(null, "Name");
        }

        /// <summary>
        /// Tests the constructor for failure when id is empty.
        /// SecurityData(string id, string companyName)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail12()
        {
            sd = new SecurityData("  ", "Name");
        }

        /// <summary>
        /// Tests the constructor for failure when companyName is null.
        /// SecurityData(string id, string companyName)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail13()
        {
            sd = new SecurityData("A", null);
        }

        /// <summary>
        /// Tests the constructor for failure when companyName is empty.
        /// SecurityData(string id, string companyName)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail14()
        {
            sd = new SecurityData("A", "  ");
        }

        /// <summary>
        /// Tests the constructor when referenceIds is null.
        /// SecurityData(string id, string companyName, String[] referenceIds)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail5()
        {
            sd = new SecurityData("A", "as", null);
        }

        /// <summary>
        /// Tests the constructor when referenceIds has null element
        /// SecurityData(string id, string companyName, String[] referenceIds)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail6()
        {
            string[] refIds = new string[] { "a", null };
            sd = new SecurityData("A", "as", refIds);
        }

        /// <summary>
        /// Tests the constructor when referenceIds has empty element
        /// SecurityData(string id, string companyName, String[] referenceIds)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail7()
        {
            string[] refIds = new string[] { "a", "      " };
            sd = new SecurityData("A", "as", refIds);
        }

        /// <summary>
        /// Tests the Id getter.
        /// </summary>
        [Test]
        public void TestId()
        {
            Assert.AreEqual(sd.Id, "A", "Wrong getter implementation.");
        }

        /// <summary>
        /// Tests the CompanyName getter.
        /// </summary>
        [Test]
        public void TestCompanyName()
        {
            Assert.AreEqual(sd.CompanyName, "Name", "Wrong getter implementation.");
        }

        /// <summary>
        /// Tests the ReferenceIds getter.
        /// </summary>
        [Test]
        public void TestReferenceIds()
        {
            //References of the 2 arrays must not be same as a shallow copy is made.
            Assert.IsFalse(object.ReferenceEquals(UnitTestHelper.GetPrivateFieldValue(sd, "referenceIds"),
                sd.ReferenceIds), "Wrong getter implementation.");

            //But the contents must be same
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(sd.ReferenceIds,
                UnitTestHelper.GetPrivateFieldValue(sd, "referenceIds") as string[]),
                "Wrong getter implementation.");
        }
    }
}

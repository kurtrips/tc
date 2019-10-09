/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SecurityDataAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SecurityData</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>SecurityData</c> class.
    /// </summary>
    ///
    /// <author>
    /// icyriver
    /// </author>
    ///
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    ///
    /// <version>
    /// 1.0
    /// </version>
    [TestFixture]
    public class SecurityDataAccuracyTests
    {
        /// <summary>
        /// Represents the <c>SecurityData</c> instance used in the test.
        /// </summary>
        SecurityData test = null;

        /// <summary>
        /// A <c>string</c> array represents the reference ids used in the test.
        /// </summary>
        string[] referenceIds = null;

        /// <summary>
        /// Set Up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            referenceIds = new string[] {"1", "2", "3"};
            test = new SecurityData("1", "TopCoder", referenceIds);
        }

        /// <summary>
        /// Cleans up the test environment. The test instance is disposed.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            referenceIds = null;
            test = null;
        }

        /// <summary>
        /// Accuracy Test of the <c>Id</c> property of this class, it test the property-getter.
        /// </summary>
        [Test]
        public void SecurityData_Property_Id()
        {
            Assert.AreEqual("1", test.Id, "The Id property should be set to '1'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>CompanyName</c> property of this class, it test the property-getter.
        /// </summary>
        [Test]
        public void SecurityData_Property_CompanyName()
        {
            Assert.AreEqual("TopCoder", test.CompanyName, "The CompanyName property should be set to 'TopCoder'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>ReferenceIds</c> property of this class, it test the property-getter.
        /// </summary>
        [Test]
        public void SecurityData_Property_ReferenceIds()
        {
            string[] ids = test.ReferenceIds;
            // we check the length of the array first.
            Assert.AreEqual(3, ids.Length, "The length of the ids array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual("1", ids[0], "The element in the ids array should be equal.");
            Assert.AreEqual("2", ids[1], "The element in the ids array should be equal.");
            Assert.AreEqual("3", ids[2], "The element in the ids array should be equal.");
        }

        /// <summary>
        /// Accuracy Test of the <c>ReferenceIds</c> property of this class, it test the
        /// given array is cloned.
        /// </summary>
        [Test]
        public void SecurityData_Property_ReferenceIds_Cloned()
        {
            // first we change the given array outside.
            referenceIds = new string[] { "4", "5", "6", "7"};

            // the property value should not be changed.
            string[] ids = test.ReferenceIds;
            // we check the length of the array first.
            Assert.AreEqual(3, ids.Length, "The length of the ids array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual("1", ids[0], "The element in the ids array should be equal.");
            Assert.AreEqual("2", ids[1], "The element in the ids array should be equal.");
            Assert.AreEqual("3", ids[2], "The element in the ids array should be equal.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityData()</c> ctor.
        /// </summary>
        [Test]
        public void SecurityData_Ctor1()
        {
            test = new SecurityData("2", "IBM");
            Assert.IsNotNull(test, "The ctor should work well.");

            // get the property to test the ctor.
            Assert.AreEqual("2", test.Id, "The Id property should be set to '2'.");
            Assert.AreEqual("IBM", test.CompanyName, "The CompanyName property should be set to 'IBM'.");

            string[] ids = test.ReferenceIds;
            // we check the length of the array first.
            Assert.IsNotNull(ids, "The ids array should be not null.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityData()</c> ctor.
        /// </summary>
        [Test]
        public void SecurityData_Ctor2()
        {
            Assert.IsNotNull(test, "The ctor should work well.");

            // get the property to test the ctor.
            Assert.AreEqual("1", test.Id, "The Id property should be set to '1'.");
            Assert.AreEqual("TopCoder", test.CompanyName, "The CompanyName property should be set to 'TopCoder'.");

            string[] ids = test.ReferenceIds;
            // we check the length of the array first.
            Assert.AreEqual(3, ids.Length, "The length of the ids array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual("1", ids[0], "The element in the ids array should be equal.");
            Assert.AreEqual("2", ids[1], "The element in the ids array should be equal.");
            Assert.AreEqual("3", ids[2], "The element in the ids array should be equal.");
        }
    }
}
/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * DefaultSecurityDataCombinerAccuracyTests.cs
 */
using System;
using System.Collections;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>DefaultSecurityDataCombiner</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>DefaultSecurityDataCombiner</c> class.
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
    public class DefaultSecurityDataCombinerAccuracyTests
    {
        /// <summary>
        /// Represents the <c>DefaultSecurityDataCombiner</c> instance used in the test.
        /// </summary>
        DefaultSecurityDataCombiner test = null;

        /// <summary>
        /// Set Up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            test = new DefaultSecurityDataCombiner();
        }

        /// <summary>
        /// Cleans up the test environment. The test instance is disposed.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            test = null;
        }

        /// <summary>
        /// Accuracy Test of the <c>DefaultSecurityDataCombiner()</c> ctor.
        /// </summary>
        [Test]
        public void DefaultSecurityDataCombiner_Ctor()
        {
            Assert.IsNotNull(test, "The ctor should work well.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Combine()</c> method, it test when the reference ids
        /// have some overlap.
        /// </summary>
        [Test]
        public void DefaultSecurityDataCombiner_Combine_Overlap1()
        {
            SecurityData data1 = new SecurityData("1", "TopCoder", new string[] { "1", "2", "3" });
            SecurityData data2 = new SecurityData("2", "IBM", new string[] { "4", "2", "3" });

            SecurityData result = test.Combine(data1, data2);
            // get the property to test the method.
            Assert.AreEqual("1", result.Id, "The Id property should be set to '1'.");
            Assert.AreEqual("TopCoder", result.CompanyName, "The CompanyName property should be set to 'TopCoder'.");

            ArrayList ids = new ArrayList(result.ReferenceIds);
            // we check the length of the array first.
            Assert.AreEqual(4, ids.Count, "The number of the ids should be equal.");
            Assert.IsTrue(ids.Contains("1"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("2"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("3"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("4"), "The result should contain this reference id");
        }

        /// <summary>
        /// Accuracy Test of the <c>Combine()</c> method, it test when the reference ids
        /// have some overlap.
        /// </summary>
        [Test]
        public void DefaultSecurityDataCombiner_Combine_Overlap2()
        {
            SecurityData data1 = new SecurityData("1", "TopCoder", new string[] { "1", "2", "3" });
            SecurityData data2 = new SecurityData("2", "IBM", new string[] { "3", "4", "5" });

            SecurityData result = test.Combine(data2, data1);
            // get the property to test the method.
            Assert.AreEqual("2", result.Id, "The Id property should be set to '2'.");
            Assert.AreEqual("IBM", result.CompanyName, "The CompanyName property should be set to 'IBM'.");

            ArrayList ids = new ArrayList(result.ReferenceIds);
            // we check the length of the array first.
            Assert.AreEqual(5, ids.Count, "The number of the ids should be equal.");
            Assert.IsTrue(ids.Contains("1"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("2"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("3"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("4"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("5"), "The result should contain this reference id");
        }

        /// <summary>
        /// Accuracy Test of the <c>Combine()</c> method, it test when the reference ids
        /// have no overlap.
        /// </summary>
        [Test]
        public void DefaultSecurityDataCombiner_Combine()
        {
            SecurityData data1 = new SecurityData("1", "TopCoder", new string[] { "1", "2", "3" });
            SecurityData data2 = new SecurityData("5", "IBM", new string[] { "5", "6" });

            SecurityData result = test.Combine(data2, data1);
            // get the property to test the method.
            Assert.AreEqual("5", result.Id, "The Id property should be set to '2'.");
            Assert.AreEqual("IBM", result.CompanyName, "The CompanyName property should be set to 'IBM'.");

            ArrayList ids = new ArrayList(result.ReferenceIds);
            // we check the length of the array first.
            Assert.AreEqual(5, ids.Count, "The number of the ids should be equal.");
            Assert.IsTrue(ids.Contains("1"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("2"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("3"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("5"), "The result should contain this reference id");
            Assert.IsTrue(ids.Contains("6"), "The result should contain this reference id");
        }
    }
}
/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SecurityIdDetailsAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SecurityIdDetails</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>SecurityIdDetails</c> class.
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
    public class SecurityIdDetailsAccuracyTests
    {
        /// <summary>
        /// Represents the <c>SecurityIdDetails</c> instance used in the test.
        /// </summary>
        SecurityIdDetails test = null;

        /// <summary>
        /// Set Up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            test = new SecurityIdDetails("123", "type");
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
        /// Accuracy Test of the <c>Id</c> property of this class, it test the property-getter.
        /// </summary>
        [Test]
        public void SecurityIdDetails_Property_Id()
        {
            Assert.AreEqual("123", test.Id, "The Id property should be set to '123'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Type</c> property of this class, it test the property-getter.
        /// </summary>
        [Test]
        public void SecurityIdDetails_Property_Type()
        {
            Assert.AreEqual("type", test.Type, "The Type property should be set to 'type'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityIdDetails()</c> ctor.
        /// </summary>
        [Test]
        public void SecurityIdDetails_Ctor()
        {
            Assert.IsNotNull(test, "The ctor should work well.");

            // get the property to test the ctor.
            Assert.AreEqual("123", test.Id, "The Id property should be set to '123'.");
            Assert.AreEqual("type", test.Type, "The Type property should be set to 'type'.");
        }
    }
}
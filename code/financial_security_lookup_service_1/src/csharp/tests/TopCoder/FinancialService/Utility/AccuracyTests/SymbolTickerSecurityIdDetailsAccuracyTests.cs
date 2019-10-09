/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SymbolTickerSecurityIdDetailsAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SymbolTickerSecurityIdDetails</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>SymbolTickerSecurityIdDetails</c> class.
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
    public class SymbolTickerSecurityIdDetailsAccuracyTests
    {
        /// <summary>
        /// Represents the <c>SymbolTickerSecurityIdDetails</c> instance used in the test.
        /// </summary>
        SymbolTickerSecurityIdDetails test = null;

        /// <summary>
        /// A <c>string</c> array represents the financialMarkets used in the test.
        /// </summary>
        string[] financialMarkets = null;

        /// <summary>
        /// Set Up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            financialMarkets = new string[] { "NYSE", "NASDAQ", "AMEX" };
            test = new SymbolTickerSecurityIdDetails("1", "old", financialMarkets, "Big");
        }

        /// <summary>
        /// Cleans up the test environment. The test instance is disposed.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            financialMarkets = null;
            test = null;
        }

        /// <summary>
        /// Accuracy Test of the <c>SymbolTickerSecurityIdDetails</c>'s inheritance.
        /// </summary>
        [Test]
        public void InheritanceTest()
        {
            // check the inheritance here.
            Assert.IsTrue(typeof(SecurityIdDetails).IsAssignableFrom(typeof(SymbolTickerSecurityIdDetails)),
                "The SymbolTickerSecurityIdDetails should extend from SecurityIdDetails.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SpecialCode</c> property of this class, it test the property-getter.
        /// </summary>
        [Test]
        public void SymbolTickerSecurityIdDetails_Property_SpecialCode()
        {
            Assert.AreEqual("Big", test.SpecialCode, "The SpecialCode property should be set to 'Big'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>FinancialMarkets</c> property of this class, it test the property-getter.
        /// </summary>
        [Test]
        public void SymbolTickerSecurityIdDetails_Property_FinancialMarkets()
        {
            string[] markets = test.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(3, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual("NYSE", markets[0], "The element in the markets array should be equal.");
            Assert.AreEqual("NASDAQ", markets[1], "The element in the markets array should be equal.");
            Assert.AreEqual("AMEX", markets[2], "The element in the markets array should be equal.");
        }

        /// <summary>
        /// Accuracy Test of the <c>FinancialMarkets</c> property of this class, it test the
        /// given array is cloned.
        /// </summary>
        [Test]
        public void SymbolTickerSecurityIdDetails_Property_FinancialMarkets_Cloned()
        {
            // first we change the given array outside.
            financialMarkets = new string[] { "4", "5", "6", "7" };

            // the property value should not be changed.
            string[] markets = test.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(3, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual("NYSE", markets[0], "The element in the markets array should be equal.");
            Assert.AreEqual("NASDAQ", markets[1], "The element in the markets array should be equal.");
            Assert.AreEqual("AMEX", markets[2], "The element in the markets array should be equal.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SymbolTickerSecurityIdDetails()</c> ctor.
        /// </summary>
        [Test]
        public void SymbolTickerSecurityIdDetails_Ctor1()
        {
            test = new SymbolTickerSecurityIdDetails("2", "new", financialMarkets);
            Assert.IsNotNull(test, "The ctor should work well.");

            // get the property to test the ctor.
            Assert.AreEqual("2", test.Id, "The Id property should be set to '2'.");
            Assert.AreEqual("new", test.Type, "The Type property should be set to 'new'.");

            string[] markets = test.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(3, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual("NYSE", markets[0], "The element in the markets array should be equal.");
            Assert.AreEqual("NASDAQ", markets[1], "The element in the markets array should be equal.");
            Assert.AreEqual("AMEX", markets[2], "The element in the markets array should be equal.");

            Assert.IsNull(test.SpecialCode, "The SpecialCode property should be set to 'null'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SymbolTickerSecurityIdDetails()</c> ctor.
        /// </summary>
        [Test]
        public void SymbolTickerSecurityIdDetails_Ctor2()
        {
            Assert.IsNotNull(test, "The ctor should work well.");

            // get the property to test the ctor.
            Assert.AreEqual("1", test.Id, "The Id property should be set to '1'.");
            Assert.AreEqual("old", test.Type, "The Type property should be set to 'old'.");

            string[] markets = test.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(3, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual("NYSE", markets[0], "The element in the markets array should be equal.");
            Assert.AreEqual("NASDAQ", markets[1], "The element in the markets array should be equal.");
            Assert.AreEqual("AMEX", markets[2], "The element in the markets array should be equal.");

            Assert.AreEqual("Big", test.SpecialCode, "The SpecialCode property should be set to 'Big'.");
        }
    }
}
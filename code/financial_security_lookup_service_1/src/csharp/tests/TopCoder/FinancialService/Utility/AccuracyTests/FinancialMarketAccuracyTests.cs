/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * FinancialMarketAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>FinancialMarket</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>FinancialMarket</c> class.
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
    public class FinancialMarketAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>NYSE</c> value.
        /// </summary>
        [Test]
        public void FinancialMarket_NYSE()
        {
            Assert.AreEqual(FinancialMarket.NYSE, "NYSE", "The FinancialMarket should contain this value 'NYSE'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>NASDAQ</c> value.
        /// </summary>
        [Test]
        public void FinancialMarket_NASDAQ()
        {
            Assert.AreEqual(FinancialMarket.NASDAQ, "NASDAQ", "The FinancialMarket should contain this value 'NASDAQ'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>AMEX</c> value.
        /// </summary>
        [Test]
        public void FinancialMarket_AMEX()
        {
            Assert.AreEqual(FinancialMarket.AMEX, "AMEX", "The FinancialMarket should contain this value 'AMEX'.");
        }
    }
}
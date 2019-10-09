/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SecurityIdTypeAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SecurityIdType</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>SecurityIdType</c> class.
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
    public class SecurityIdTypeAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>CUSIP</c> value.
        /// </summary>
        [Test]
        public void SecurityIdType_CUSIP()
        {
            Assert.AreEqual(SecurityIdType.CUSIP, "CUSIP", "The SecurityIdType should contain this value 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>ISIN</c> value.
        /// </summary>
        [Test]
        public void SecurityIdType_ISIN()
        {
            Assert.AreEqual(SecurityIdType.ISIN, "ISIN", "The SecurityIdType should contain this value 'ISIN'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SymbolTicker</c> value.
        /// </summary>
        [Test]
        public void SecurityIdType_SymbolTicker()
        {
            Assert.AreEqual(SecurityIdType.SymbolTicker, "SymbolTicker",
                "The SecurityIdType should contain this value 'SymbolTicker'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SEDOL</c> value.
        /// </summary>
        [Test]
        public void SecurityIdType_SEDOL()
        {
            Assert.AreEqual(SecurityIdType.SEDOL, "SEDOL", "The SecurityIdType should contain this value 'SEDOL'.");
        }
    }
}
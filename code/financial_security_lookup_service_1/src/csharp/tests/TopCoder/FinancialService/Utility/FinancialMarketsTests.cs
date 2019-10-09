// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the FinancialMarkets class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class FinancialMarketsTests
    {
        /// <summary>
        /// Tests the constants of the class.
        /// </summary>
        [Test]
        public void Test1()
        {
            Assert.AreEqual(FinancialMarket.AMEX, "AMEX", "Wrong value of constant.");
            Assert.AreEqual(FinancialMarket.NASDAQ, "NASDAQ", "Wrong value of constant.");
            Assert.AreEqual(FinancialMarket.NYSE, "NYSE", "Wrong value of constant.");
        }
    }
}

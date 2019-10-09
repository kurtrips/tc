// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the SecurityIdType class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SecurityIdTypeTests
    {
        /// <summary>
        /// Tests the constants of the class.
        /// </summary>
        [Test]
        public void Test1()
        {
            Assert.AreEqual(SecurityIdType.CUSIP, "CUSIP", "Wrong value of constant.");
            Assert.AreEqual(SecurityIdType.ISIN, "ISIN", "Wrong value of constant.");
            Assert.AreEqual(SecurityIdType.SEDOL, "SEDOL", "Wrong value of constant.");
            Assert.AreEqual(SecurityIdType.SymbolTicker, "SymbolTicker", "Wrong value of constant.");
        }
    }
}

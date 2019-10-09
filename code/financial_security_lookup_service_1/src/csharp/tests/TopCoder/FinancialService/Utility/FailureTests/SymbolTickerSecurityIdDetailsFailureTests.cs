/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Failure tests for <c>SymbolTickerSecurityIdDetails</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SymbolTickerSecurityIdDetailsFailureTests
    {
        /// <summary>
        /// The financial markets the security id is in.
        /// </summary>
        string[] financialMarkets = new string[] { "value1", "value2" };

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with null id.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSymbolTickerSecurityIdDetails_1_Null_id()
        {
            new SymbolTickerSecurityIdDetails(null, "type", financialMarkets);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with empty id.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_1_empty_id()
        {
            new SymbolTickerSecurityIdDetails("          ", "type", financialMarkets);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with null type.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSymbolTickerSecurityIdDetails_1_Null_type()
        {
            new SymbolTickerSecurityIdDetails("id", null, financialMarkets);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with empty type.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_1_empty_type()
        {
            new SymbolTickerSecurityIdDetails("id", "      ", financialMarkets);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with null financialMarkets.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSymbolTickerSecurityIdDetails_1_Null_financialMarkets()
        {
            new SymbolTickerSecurityIdDetails("id", "type", null);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with empty financialMarkets.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_1_empty_financialMarkets()
        {
            new SymbolTickerSecurityIdDetails("id", "type", new string[] { });
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with empty item financialMarkets.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_1_empty_item_financialMarkets()
        {
            financialMarkets[1] = "     ";
            new SymbolTickerSecurityIdDetails("id", "type", financialMarkets);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
        /// </c> constructor with null item financialMarkets.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_1_null_item_financialMarkets()
        {
            financialMarkets[1] = null;
            new SymbolTickerSecurityIdDetails("id", "type", financialMarkets);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with null id.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSymbolTickerSecurityIdDetails_2_Null_id()
        {
            new SymbolTickerSecurityIdDetails(null, "type", financialMarkets);
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with empty id.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_2_empty_id()
        {
            new SymbolTickerSecurityIdDetails("          ", "type", financialMarkets, "specialCode");
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with null type.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSymbolTickerSecurityIdDetails_2_Null_type()
        {
            new SymbolTickerSecurityIdDetails("id", null, financialMarkets, "specialCode");
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with empty type.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_2_empty_type()
        {
            new SymbolTickerSecurityIdDetails("id", "      ", financialMarkets, "specialCode");
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with null financialMarkets.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSymbolTickerSecurityIdDetails_2_Null_financialMarkets()
        {
            new SymbolTickerSecurityIdDetails("id", "type", null, "specialCode");
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with empty financialMarkets.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_2_empty_financialMarkets()
        {
            new SymbolTickerSecurityIdDetails("id", "type", new string[] { }, "specialCode");
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with empty item financialMarkets.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_2_empty_item_financialMarkets()
        {
            financialMarkets[1] = "     ";
            new SymbolTickerSecurityIdDetails("id", "type", financialMarkets, "specialCode");
        }

        /// <summary>
        /// Tests the failure of the <c>SymbolTickerSecurityIdDetails(string id, string type,
        /// string[] financialMarkets, string specialCode)</c> constructor with null item financialMarkets.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSymbolTickerSecurityIdDetails_2_null_item_financialMarkets()
        {
            financialMarkets[1] = null;
            new SymbolTickerSecurityIdDetails("id", "type", financialMarkets, "specialCode");
        }
    }
}
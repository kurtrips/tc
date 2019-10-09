// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the SymbolTickerSecurityIdDetails class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SymbolTickerSecurityIdDetailsTests
    {
        /// <summary>
        /// The SymbolTickerSecurityIdDetails instance to use for the tests
        /// </summary>
        SymbolTickerSecurityIdDetails stsid;

        /// <summary>
        /// The Financial markets array to create the SymbolTickerSecurityIdDetails instance.
        /// </summary>
        string[] finMarkets;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            finMarkets = new string[] { "NYSE", "NASDAQ" };
            stsid = new SymbolTickerSecurityIdDetails("A", SecurityIdType.SymbolTicker, finMarkets);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            finMarkets = null;
            stsid = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// SymbolTickerSecurityIdDetails(string id, string type, String[] financialMarkets)
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsTrue(stsid is SecurityIdDetails, "Wrong type of SymbolTickerSecurityIdDetails");
            Assert.AreEqual(stsid.Id, "A", "Wrong constructor implementation.");
            Assert.AreEqual(stsid.Type, SecurityIdType.SymbolTicker, "Wrong constructor implementation.");

            //References of the arrays must not be same
            Assert.IsFalse(object.ReferenceEquals(UnitTestHelper.GetPrivateFieldValue(stsid, "financialMarkets"),
                finMarkets), "Wrong constructor implementation.");

            //But contents must be same
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                UnitTestHelper.GetPrivateFieldValue(stsid, "financialMarkets") as string[], finMarkets),
                "Wrong constructor implementation.");

        }

        /// <summary>
        /// Tests the constructor.
        /// SymbolTickerSecurityIdDetails(string id, string type, String[] financialMarkets, string specialCode)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            stsid = new SymbolTickerSecurityIdDetails("A", SecurityIdType.SymbolTicker, finMarkets, "SuperSpecial");

            Assert.IsTrue(stsid is SecurityIdDetails, "Wrong type of SymbolTickerSecurityIdDetails");
            Assert.AreEqual(stsid.Id, "A", "Wrong constructor implementation.");
            Assert.AreEqual(stsid.Type, SecurityIdType.SymbolTicker, "Wrong constructor implementation.");
            Assert.AreEqual(stsid.SpecialCode, "SuperSpecial", "Wrong constructor implementation.");

            //References of the arrays must not be same
            Assert.IsFalse(object.ReferenceEquals(UnitTestHelper.GetPrivateFieldValue(stsid, "financialMarkets"),
                finMarkets), "Wrong constructor implementation.");

            //But contents must be same
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                UnitTestHelper.GetPrivateFieldValue(stsid, "financialMarkets") as string[], finMarkets),
                "Wrong constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor when financialMarkets is null
        /// SymbolTickerSecurityIdDetails(string id, string type, String[] financialMarkets, string specialCode)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            stsid = new SymbolTickerSecurityIdDetails("A", "B", null, "SuperSpecial");
        }

        /// <summary>
        /// Tests the constructor when financialMarkets is empty
        /// SymbolTickerSecurityIdDetails(string id, string type, String[] financialMarkets, string specialCode)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail2()
        {
            stsid = new SymbolTickerSecurityIdDetails("A", "B", new string[0], "SuperSpecial");
        }

        /// <summary>
        /// Tests the constructor when financialMarkets has null element.
        /// SymbolTickerSecurityIdDetails(string id, string type, String[] financialMarkets, string specialCode)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail3()
        {
            stsid = new SymbolTickerSecurityIdDetails("A", "B", new string[] { null }, "SuperSpecial");
        }

        /// <summary>
        /// Tests the constructor when financialMarkets has empty element.
        /// SymbolTickerSecurityIdDetails(string id, string type, String[] financialMarkets, string specialCode)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail4()
        {
            stsid = new SymbolTickerSecurityIdDetails("A", "B", new string[] { "   " }, "SuperSpecial");
        }

        /// <summary>
        /// Tests the constructor when specialCode is empty
        /// SymbolTickerSecurityIdDetails(string id, string type, String[] financialMarkets, string specialCode)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail5()
        {
            stsid = new SymbolTickerSecurityIdDetails("A", "B", finMarkets, "  ");
        }

        /// <summary>
        /// Tests the FinancialMarkets getter.
        /// </summary>
        [Test]
        public void TestFinancialMarkets()
        {
            //References of the arrays must not be same
            Assert.IsFalse(object.ReferenceEquals(
                UnitTestHelper.GetPrivateFieldValue(stsid, "financialMarkets") as string[], stsid.FinancialMarkets),
                "Wrong getter implementation.");

            //But contents must be same
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                UnitTestHelper.GetPrivateFieldValue(stsid, "financialMarkets") as string[], stsid.FinancialMarkets),
                "Wrong getter implementation.");
        }

        /// <summary>
        /// Tests the SpecialCode getter.
        /// </summary>
        [Test]
        public void TestSpecialCode()
        {
            stsid = new SymbolTickerSecurityIdDetails("A", SecurityIdType.SymbolTicker, finMarkets, "SuperSpecial");
            Assert.AreEqual(stsid.SpecialCode, "SuperSpecial", "Wrong getter implementation.");
        }
    }
}

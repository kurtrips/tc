/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * DefaultSecurityIdParserAccuracyTests.cs
 */
using System;
using System.Collections;
using TopCoder.FinancialService.Utility.SecurityIdParsers;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>DefaultSecurityIdParser</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>DefaultSecurityIdParser</c> class.
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
    public class DefaultSecurityIdParserAccuracyTests
    {
        /// <summary>
        /// Represents the <c>DefaultSecurityIdParser</c> instance used in the test.
        /// </summary>
        DefaultSecurityIdParser test = null;

        /// <summary>
        /// Set Up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            test = new DefaultSecurityIdParser();
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
        /// Accuracy Test of the <c>DefaultSecurityIdParser()</c> ctor.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_Ctor()
        {
            Assert.IsNotNull(test, "The ctor should work well.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the PM's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_CUSIP_Letters1()
        {
            SecurityIdDetails details = test.Parse("J0176K103");
            // get the property to test the method.
            Assert.AreEqual("J0176K103", details.Id, "The Id property should be set to 'J0176K103'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the PM's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_CUSIP_Letters1_LowerCase()
        {
            SecurityIdDetails details = test.Parse("j0176k103");
            // get the property to test the method.
            Assert.AreEqual("J0176K103", details.Id, "The Id property should be set to 'J0176K103'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the PM's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_CUSIP_Letters2()
        {
            SecurityIdDetails details = test.Parse("G4770P115");
            // get the property to test the method.
            Assert.AreEqual("G4770P115", details.Id, "The Id property should be set to 'G4770P115'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the PM's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_CUSIP_Letters2_LowerCase()
        {
            SecurityIdDetails details = test.Parse("g4770p115");
            // get the property to test the method.
            Assert.AreEqual("G4770P115", details.Id, "The Id property should be set to 'G4770P115'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }
        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_CUSIP_Number1()
        {
            SecurityIdDetails details = test.Parse("037833100");
            // get the property to test the method.
            Assert.AreEqual("037833100", details.Id, "The Id property should be set to '037833100'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the example of Wal-Mart.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_CUSIP_Number2()
        {
            SecurityIdDetails details = test.Parse("931142103");
            // get the property to test the method.
            Assert.AreEqual("931142103", details.Id, "The Id property should be set to '931142103'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_ISIN_Number()
        {
            SecurityIdDetails details = test.Parse("US0378331005");
            // get the property to test the method.
            Assert.AreEqual("US0378331005", details.Id, "The Id property should be set to 'US0378331005'.");
            Assert.AreEqual(SecurityIdType.ISIN, details.Type, "The Type property should be set to 'ISIN'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests with the designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_SEDOL_Number()
        {
            SecurityIdDetails details = test.Parse("0263494");
            // get the property to test the method.
            Assert.AreEqual("0263494", details.Id, "The Id property should be set to '0263494'.");
            Assert.AreEqual(SecurityIdType.SEDOL, details.Type, "The Type property should be set to 'SEDOL'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests when the securityId ends with a dot 
        /// plus an letter and starts with one to three letters before the dot.
        /// It is a designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_SymbolTicker_NYSE1()
        {
            // The details should be an instance of SymbolTickerSecurityIdDetails
            SymbolTickerSecurityIdDetails details = (SymbolTickerSecurityIdDetails)test.Parse("ABC.L");

            // get the property to test the method.
            Assert.AreEqual("ABC.L", details.Id, "The Id property should be set to 'ABC.L'.");
            Assert.AreEqual(SecurityIdType.SymbolTicker, details.Type, "The Type property should be set to 'SymbolTicker'.");

            string[] markets = details.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(2, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual(FinancialMarket.NYSE, markets[0], "The element in the markets array should be equal.");
            Assert.AreEqual(FinancialMarket.AMEX, markets[1], "The element in the markets array should be equal.");

            Assert.AreEqual("Miscellaneous", details.SpecialCode, "The SpecialCode property should be set to 'Miscellaneous'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests when the securityId is 1-letter string.
        /// It is a designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_SymbolTicker_NYSE2()
        {
            // The details should be an instance of SymbolTickerSecurityIdDetails
            SymbolTickerSecurityIdDetails details = (SymbolTickerSecurityIdDetails)test.Parse("A");
            // get the property to test the method.
            Assert.AreEqual("A", details.Id, "The Id property should be set to 'A'.");
            Assert.AreEqual(SecurityIdType.SymbolTicker, details.Type, "The Type property should be set to 'SymbolTicker'.");

            string[] markets = details.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(1, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual(FinancialMarket.NYSE, markets[0], "The element in the markets array should be equal.");

            Assert.IsNull(details.SpecialCode, "The SpecialCode property should be set to 'null'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests when the securityId is 2-letter string.
        /// It is a designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_SymbolTicker_AMEX1()
        {
            // The details should be an instance of SymbolTickerSecurityIdDetails
            SymbolTickerSecurityIdDetails details = (SymbolTickerSecurityIdDetails)test.Parse("AB");
            // get the property to test the method.
            Assert.AreEqual("AB", details.Id, "The Id property should be set to 'AB'.");
            Assert.AreEqual(SecurityIdType.SymbolTicker, details.Type, "The Type property should be set to 'SymbolTicker'.");

            ArrayList markets = new ArrayList(details.FinancialMarkets);
            // we check the length of the array first.
            Assert.AreEqual(2, markets.Count, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.IsTrue(markets.Contains(FinancialMarket.AMEX), "The element in the markets array should be equal.");
            Assert.IsTrue(markets.Contains(FinancialMarket.NYSE), "The element in the markets array should be equal.");

            Assert.IsNull(details.SpecialCode, "The SpecialCode property should be set to 'null'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests when the securityId is 3-letter string.
        /// It is a designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_SymbolTicker_AMEX2()
        {
            // The details should be an instance of SymbolTickerSecurityIdDetails
            SymbolTickerSecurityIdDetails details = (SymbolTickerSecurityIdDetails)test.Parse("ABC");
            // get the property to test the method.
            Assert.AreEqual("ABC", details.Id, "The Id property should be set to 'ABC'.");
            Assert.AreEqual(SecurityIdType.SymbolTicker, details.Type, "The Type property should be set to 'SymbolTicker'.");

            ArrayList markets = new ArrayList(details.FinancialMarkets);
            // we check the length of the array first.
            Assert.AreEqual(2, markets.Count, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.IsTrue(markets.Contains(FinancialMarket.AMEX), "The element in the markets array should be equal.");
            Assert.IsTrue(markets.Contains(FinancialMarket.NYSE), "The element in the markets array should be equal.");

            Assert.IsNull(details.SpecialCode, "The SpecialCode property should be set to 'null'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests when the securityId is 4-letter string.
        /// It is a designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_SymbolTicker_NASDAQ1()
        {
            // The details should be an instance of SymbolTickerSecurityIdDetails
            SymbolTickerSecurityIdDetails details = (SymbolTickerSecurityIdDetails)test.Parse("ABCD");
            // get the property to test the method.
            Assert.AreEqual("ABCD", details.Id, "The Id property should be set to 'ABCD'.");
            Assert.AreEqual(SecurityIdType.SymbolTicker, details.Type, "The Type property should be set to 'SymbolTicker'.");

            string[] markets = details.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(2, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual(FinancialMarket.NYSE, markets[0], "The element in the markets array should be equal.");
            Assert.AreEqual(FinancialMarket.NASDAQ, markets[1], "The element in the markets array should be equal.");

            Assert.IsNull(details.SpecialCode, "The SpecialCode property should be set to 'null'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Parse()</c> method, it tests when the securityId is 5-letter string.
        /// It is a designer's example.
        /// </summary>
        [Test]
        public void DefaultSecurityIdParser_SymbolTicker_NASDAQ2()
        {
            // The details should be an instance of SymbolTickerSecurityIdDetails
            SymbolTickerSecurityIdDetails details = (SymbolTickerSecurityIdDetails)test.Parse("ABCDY");
            // get the property to test the method.
            Assert.AreEqual("ABCDY", details.Id, "The Id property should be set to 'ABCDY'.");
            Assert.AreEqual(SecurityIdType.SymbolTicker, details.Type, "The Type property should be set to 'SymbolTicker'.");

            string[] markets = details.FinancialMarkets;
            // we check the length of the array first.
            Assert.AreEqual(2, markets.Length, "The length of the markets array should be equal.");
            // then we check the element in the array.
            Assert.AreEqual(FinancialMarket.NYSE, markets[0], "The element in the markets array should be equal.");
            Assert.AreEqual(FinancialMarket.NASDAQ, markets[1], "The element in the markets array should be equal.");

            Assert.AreEqual("American Depositary Receipt (ADR)", details.SpecialCode, "The SpecialCode property should be equal.");
        }

    }
}
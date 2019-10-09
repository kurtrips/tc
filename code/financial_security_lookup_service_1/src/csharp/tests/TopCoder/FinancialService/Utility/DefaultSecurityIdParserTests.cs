// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.SecurityIdParsers
{
    /// <summary>
    /// Unit tests for the DefaultSecurityIdParser class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DefaultSecurityIdParserTests
    {
        /// <summary>
        /// The DefaultSecurityIdParser instance to use for the tests.
        /// </summary>
        DefaultSecurityIdParser dsip;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            dsip = new DefaultSecurityIdParser();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            dsip = null;
        }

        #region Accuracy Tests

        /// <summary>
        /// Tests the constructor.
        /// DefaultSecurityIdParser()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsTrue(dsip is ISecurityIdParser, "Wrong type of DefaultSecurityIdParser");
        }

        /// <summary>
        /// Tests the Parse method when securityId is valid ISIN.
        /// This securityId is of W.R. Grace and Co. shares
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseIsinAccuracy1()
        {
            SecurityIdDetails sid = dsip.Parse("US3838831051");
            Assert.AreEqual(sid.Type, SecurityIdType.ISIN, "Wrong parsing of ISIN.");
        }

        /// <summary>
        /// Tests the Parse method when securityId is valid ISIN but characters are in lowercase
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseIsinAccuracy2()
        {
            SecurityIdDetails sid = dsip.Parse("us3838831051");
            Assert.AreEqual(sid.Type, SecurityIdType.ISIN, "Wrong parsing of ISIN.");
        }

        /// <summary>
        /// Another test for the Parse method when securityId is valid ISIN.
        /// This securityId is that of International Bank for Reconstruction and Development (Worldbank)
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseIsinAccuracy3()
        {
            SecurityIdDetails sid = dsip.Parse("US459056DG91");
            Assert.AreEqual(sid.Type, SecurityIdType.ISIN, "Wrong parsing of ISIN.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid SEDOL.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseSedolAccuracy1()
        {
            SecurityIdDetails sid = dsip.Parse("B1F3M59");
            Assert.AreEqual(sid.Type, SecurityIdType.SEDOL, "Wrong parsing of SEDOL.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid SEDOL but its characters are lowercase.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseSedolAccuracy2()
        {
            SecurityIdDetails sid = dsip.Parse("b1f3m59");
            Assert.AreEqual(sid.Type, SecurityIdType.SEDOL, "Wrong parsing of SEDOL.");
        }

        /// <summary>
        /// Another test for the Parse method when securityId is valid SEDOL.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseSedolAccuracy3()
        {
            SecurityIdDetails sid = dsip.Parse("B1H54P7");
            Assert.AreEqual(sid.Type, SecurityIdType.SEDOL, "Wrong parsing of SEDOL.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid CUSIP.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseCusipAccuracy1()
        {
            SecurityIdDetails sid = dsip.Parse("J0176K103");
            Assert.AreEqual(sid.Type, SecurityIdType.CUSIP, "Wrong parsing of CUSIP.");
        }

        /// <summary>
        /// Another test for the Parse method when securityId is valid CUSIP.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseCusipAccuracy2()
        {
            SecurityIdDetails sid = dsip.Parse("G4770P115");
            Assert.AreEqual(sid.Type, SecurityIdType.CUSIP, "Wrong parsing of CUSIP.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid CUSIP but its chacracters are lowercase.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseCusipAccuracy3()
        {
            SecurityIdDetails sid = dsip.Parse("g4770p115");
            Assert.AreEqual(sid.Type, SecurityIdType.CUSIP, "Wrong parsing of CUSIP.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid NYSE symbol and returned SymbolTickerSecurityIdDetails
        /// instance contains only NYSE.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseTickerAccuracy1()
        {
            SecurityIdDetails sid = dsip.Parse("A");

            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets.Length, 1,
                "FinancialMarkets array must contain only 1 entry.");
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets[0],
                FinancialMarket.NYSE, "Wrong parsing of NYSE symbol.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid NYSE or AMEX symbol
        /// and returned SymbolTickerSecurityIdDetails instance contains NYSE and AMEX.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseTickerAccuracy2()
        {
            SecurityIdDetails sid = dsip.Parse("AB");

            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets.Length, 2,
                "FinancialMarkets array must contain 2 entries.");
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets[0],
                FinancialMarket.NYSE, "Wrong parsing of NYSE symbol.");
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets[1],
                FinancialMarket.AMEX, "Wrong parsing of AMEX symbol.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid NYSE or NASDAQ symbol
        /// and returned SymbolTickerSecurityIdDetails instance contains NYSE and NASDAQ.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseTickerAccuracy3()
        {
            SecurityIdDetails sid = dsip.Parse("ABCDX");

            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets.Length, 2,
                "FinancialMarkets array must contain 2 entries.");

            //A valid NYSE symbol
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets[0],
                FinancialMarket.NYSE, "Wrong parsing of NYSE symbol.");

            //A valid NASDAQ symbol with special code as Mutual Fund
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets[1],
                FinancialMarket.NASDAQ, "Wrong parsing of NASDAQ symbol.");
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).SpecialCode,
                "Mutual fund", "Wrong parsing of NASDAQ symbol.");
        }

        /// <summary>
        /// Test for the Parse method when securityId is valid NYSE or AMEX symbol
        /// and returned SymbolTickerSecurityIdDetails instance contains NYSE and AMEX.
        /// SecurityIdDetails Parse(string securityId)
        /// </summary>
        [Test]
        public void TestParseTickerAccuracy4()
        {
            SecurityIdDetails sid = dsip.Parse("ABC.X");

            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets.Length, 2,
                "FinancialMarkets array must contain 2 entries.");

            //A valid NYSE symbol
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets[0],
                FinancialMarket.NYSE, "Wrong parsing of NYSE symbol.");

            //A valid AMEX symbol
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).FinancialMarkets[1],
                FinancialMarket.AMEX, "Wrong parsing of AMEX symbol.");

            //Special code must be Mutual fund
            Assert.AreEqual((sid as SymbolTickerSecurityIdDetails).SpecialCode,
                "Mutual fund", "Wrong parsing of special code.");
        }

        #endregion

        #region Failure Tests

        /// <summary>
        /// Test for the Parse method when securityId is invalid ISIN because countryCode is not existent.
        /// SecurityIdDetails Parse(string securityId)
        /// InvalidSecurityIdFormatException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestParseIsinFail1()
        {
            dsip.Parse("ZX459056DG91");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid ISIN because check digit is wrong.
        /// SecurityIdDetails Parse(string securityId)
        /// InvalidSecurityIdFormatException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestParseIsinFail2()
        {
            dsip.Parse("US459056DG95");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid ISIN because it contains non-alphanumeric characters.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(SecurityIdParsingException))]
        public void TestParseIsinFail3()
        {
            dsip.Parse("US459+56DG91");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid ISIN because check digit is not a digit.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestParseIsinFail4()
        {
            dsip.Parse("US459056DG9A");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid SEDOL because its last character is not digit.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestParseSedolFail2()
        {
            dsip.Parse("B1F3M5B");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid SEDOL because it contains a vowel.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(SecurityIdParsingException))]
        public void TestParseSedolFail3()
        {
            dsip.Parse("B1A3M59");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid SEDOL because it contains non alphanumeric character.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(SecurityIdParsingException))]
        public void TestParseSedolFail4()
        {
            dsip.Parse("B1+3M59");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid SEDOL because its check digit is incorrect.
        /// SecurityIdDetails Parse(string securityId)
        /// InvalidSecurityIdFormatException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestParseSedolFail5()
        {
            dsip.Parse("B1F3M50");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid CUSIP because the last character is not a digit.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestParseCusipFail1()
        {
            dsip.Parse("J0176K10D");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid CUSIP because 3rd, 4th or 5th character is not a digit.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(SecurityIdParsingException))]
        public void TestParseCusipFail2()
        {
            dsip.Parse("J01D6K103");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid CUSIP because it contains non alphanumeric character.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(SecurityIdParsingException))]
        public void TestParseCusipFail3()
        {
            dsip.Parse("J01+6K103");
        }

        /// <summary>
        /// Test for the Parse method when securityId is invalid CUSIP because its check digit is wrong.
        /// SecurityIdDetails Parse(string securityId)
        /// InvalidSecurityIdFormatException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestParseCusipFail4()
        {
            dsip.Parse("J0176K102");
        }

        /// <summary>
        /// Test for the Parse method when securityId is neither NYSE, NASDAQ, AMEX, CUSIP, SEDOL or ISIN
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestParseFail1()
        {
            dsip.Parse("KJDFHKSKDJHLKSKLJSAJDH");
        }

        /// <summary>
        /// Test for the Parse method when securityId is neither NYSE, NASDAQ, AMEX.
        /// The special code cannot be digit
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestParseFail2()
        {
            dsip.Parse("ABC.1");
        }

        /// <summary>
        /// Test for the Parse method when securityId is neither NYSE, NASDAQ, AMEX.
        /// Dot must always be second last character.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestParseFail3()
        {
            dsip.Parse("AB.CD");
        }

        /// <summary>
        /// Test for the Parse method when securityId is neither NYSE, NASDAQ, AMEX.
        /// Only 1 Dot can be present.
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestParseFail4()
        {
            dsip.Parse(".B.C");
        }

        /// <summary>
        /// Test for the Parse method when securityId is neither NYSE, NASDAQ, AMEX.
        /// No special characters are allowed
        /// SecurityIdDetails Parse(string securityId)
        /// UnknownSecurityIdTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestParseFail5()
        {
            dsip.Parse("A+B");
        }

        /// <summary>
        /// Test for the Parse method when securityId is null.
        /// SecurityIdDetails Parse(string securityId)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestParseFailANE()
        {
            dsip.Parse(null);
        }

        /// <summary>
        /// Test for the Parse method when securityId is empty.
        /// SecurityIdDetails Parse(string securityId)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestParseFailAE()
        {
            dsip.Parse("     ");
        }

        #endregion
    }
}

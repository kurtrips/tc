// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using NUnit.Framework;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Cache;
using TopCoder.Configuration;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;
using TopCoder.FinancialService.Utility.SecurityIdParsers;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Demo for this component.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DemoTests
    {
        /// <summary>
        /// The FinancialSecurityManager instance to use for the tests.
        /// </summary>
        FinancialSecurityManager fsm;

        /// <summary>
        /// A dictionary holding the security id types to ISecurityLookupService
        /// objects mappings.
        /// </summary>
        IDictionary<string, ISecurityLookupService> securityLookupServices;

        /// <summary>
        /// The DefaultSecurityDataCombiner instance to use for creating the FinancialSecurityManager instance.
        /// </summary>
        DefaultSecurityDataCombiner combiner;

        /// <summary>
        /// The SimpleCache instance to use for creating the FinancialSecurityManager instance.
        /// </summary>
        SimpleCache cache;

        /// <summary>
        /// The DefaultSecurityIdParser instance to use for creating the FinancialSecurityManager instance.
        /// </summary>
        DefaultSecurityIdParser parser;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // create a dictionary holding the security id types to ISecurityLookupService
            // objects mappings
            securityLookupServices = new Dictionary<string, ISecurityLookupService>();
            securityLookupServices[SecurityIdType.CUSIP] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.ISIN] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.SEDOL] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.SymbolTicker] = new CustomSecurityLookupService();

            //Create DefaultSecurityDataCombiner
            combiner = new DefaultSecurityDataCombiner();

            //Create cache
            cache = new SimpleCache();

            //Create DefaultSecurityIdParser
            parser = new DefaultSecurityIdParser();

            fsm = new FinancialSecurityManager(parser, securityLookupServices, combiner,
                false, true, cache);

        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            fsm = null;
            securityLookupServices = null;
            combiner = null;
            parser = null;
            cache = null;
        }

        /// <summary>
        /// Demo of different types of lookup functionality for this component.
        /// The securityId -> referenceIds relationship here is:
        /// A -> B C
        /// B -> D
        /// C ->
        /// D -> B
        /// </summary>
        [Test]
        public void DemoLookup()
        {
            //////////////REFERENCE LOOKUP DEMO
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, false, true, cache);

            //Lookup using A. Returns A,B,C
            SecurityData securityData = fsm.Lookup("A");

            //Returned data must have reference ids A,B,C
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C" }),
                "Wrong Lookup implementation.");

            //Now lookup using B
            securityData = fsm.Lookup("B");

            //Returned data must have reference ids A,B,C,D
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Lookup implementation.");



            //////////////SIMPLE LOOKUP DEMO
            cache = new SimpleCache();
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, false, false, cache);

            //Lookup using A. Returns A,B,C
            securityData = fsm.Lookup("A");

            //Returned data must have reference ids A,B,C
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C" }),
                "Wrong Lookup implementation.");

            //Now lookup using B
            securityData = fsm.Lookup("B");

            //Returned data must have reference ids A,B,C
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C" }),
                "Wrong Lookup implementation.");



            //////////////RECURSIVE LOOKUP DEMO
            cache = new SimpleCache();
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, true, false, cache);

            //Lookup using A. Returns A,B,C,D
            securityData = fsm.Lookup("A");

            //Returned data must have reference ids A,B,C,D
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Lookup implementation.");

            //Now lookup using B
            securityData = fsm.Lookup("B");

            //Returned data must have reference ids A,B,C,D
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Lookup implementation.");
        }

        /// <summary>
        /// Demo of different ConvertXXX functionality for this component.
        /// </summary>
        [Test]
        public void DemoConvertIds()
        {
            //Convert from CUSIP to ISIN
            string isin = fsm.ConvertFromCUSIPToISIN("459056DG9");
            Assert.AreEqual(isin, "US459056DG91", "Wrong conversion of Cusip to ISIN");

            //Convert from ISIN to CUSIP
            string cusip = fsm.ConvertFromISINToCUSIP("US459056DG91");
            Assert.AreEqual(cusip, "459056DG9", "Wrong ConvertFromISINToCUSIP implementation.");
        }

        /// <summary>
        /// Demo of usage of FinancialSecurityManagerBuilder.
        /// </summary>
        [Test]
        public void DemoFinancialSecurityManagerBuilder()
        {
            //Get IConfiguration object either by preparing it manually or
            //by using File Based Configuration TCS component
            IConfiguration config = UnitTestHelper.GetConfigObject();

            //Create the FinancialSecurityManager instance from IConfiguration
            FinancialSecurityManager fsm = FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
        }
    }
}

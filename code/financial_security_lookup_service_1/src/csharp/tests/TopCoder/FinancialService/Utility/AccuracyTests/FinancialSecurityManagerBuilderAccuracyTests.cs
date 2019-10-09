/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * FinancialSecurityManagerBuilderAccuracyTests.cs
 */
using System;
using TopCoder.Configuration;
using System.Collections.Generic;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>FinancialSecurityManagerBuilder</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>FinancialSecurityManagerBuilder</c> class.
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
    public class FinancialSecurityManagerBuilderAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>BuildFinancialSecurityManager()</c> method.
        /// It test with the full config, the value of security_id_parser_key is also
        /// given.
        /// </summary>
        [Test]
        public void BuildFinancialSecurityManager_FullConfig()
        {
            IConfiguration config = AccuracyTestsTestHelper.GetConfigObject();
            FinancialSecurityManager manager = FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
            Assert.IsNotNull(manager, "The ctor should work well.");

            // 1 get the idParser to test the builder.
            ISecurityIdParser idParser = (ISecurityIdParser)AccuracyTestsTestHelper.getPrivateField(manager, "securityIdParser");
            // this idParser should be DefaultSecurityIdParser.
            SecurityIdDetails details = idParser.Parse("J0176K103");
            // get the property to test the method.
            Assert.AreEqual("J0176K103", details.Id, "The Id property should be set to 'J0176K103'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");

            // 2 get the lookupServices to test the builder.
            IDictionary<string, ISecurityLookupService> lookupServices = (IDictionary<string, ISecurityLookupService>)
                AccuracyTestsTestHelper.getPrivateField(manager, "securityLookupServices");
            Assert.AreEqual(2, lookupServices.Count, "Should have 2 element");
            Assert.IsTrue(lookupServices.ContainsKey(SecurityIdType.CUSIP), "Should have this lookupService name");
            Assert.IsTrue(lookupServices.ContainsKey(SecurityIdType.ISIN), "Should have this lookupService name");

            // 3 get the recursive to test the builder.
            bool recursive = (bool)AccuracyTestsTestHelper.getPrivateField(manager, "recursiveLookup");
            Assert.IsFalse(recursive, "Should set to false");

            // 4 get the recursive to test the builder.
            bool reference = (bool)AccuracyTestsTestHelper.getPrivateField(manager, "referenceLookup");
            Assert.IsTrue(reference, "Should set to true");

            // 5 get the combiner to test the builder.
            ISecurityDataCombiner combiner = (ISecurityDataCombiner)
                AccuracyTestsTestHelper.getPrivateField(manager, "securityDataCombiner");

            SecurityData data1 = new SecurityData("1", "TopCoder", new string[] { "1", "2", "3" });
            SecurityData data2 = new SecurityData("2", "IBM", new string[] { "4", "2", "3" });

            SecurityData result = combiner.Combine(data1, data2);
            // get the property to test the method.
            Assert.AreEqual("1", result.Id, "The Id property should be set to '1'.");
            Assert.AreEqual("TopCoder", result.CompanyName, "The CompanyName property should be set to 'TopCoder'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>BuildFinancialSecurityManager()</c> method.
        /// It test with the option config, the value of security_id_parser_key is not
        /// given.
        /// </summary>
        [Test]
        public void BuildFinancialSecurityManager_OptionConfig()
        {
            IConfiguration config = AccuracyTestsTestHelper.GetConfigObject();

            // the security_id_parser_key value is optional.
            config.RemoveAttribute("security_id_parser_key");
            FinancialSecurityManager manager = FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
            Assert.IsNotNull(manager, "The build method should work well.");

            // 1 get the idParser to test the builder.
            ISecurityIdParser idParser = (ISecurityIdParser)AccuracyTestsTestHelper.getPrivateField(manager, "securityIdParser");
            // this idParser should be not null.
            Assert.IsNotNull(idParser, "The build method should work well.");

            // 2 get the lookupServices to test the builder.
            IDictionary<string, ISecurityLookupService> lookupServices = (IDictionary<string, ISecurityLookupService>)
                AccuracyTestsTestHelper.getPrivateField(manager, "securityLookupServices");
            Assert.AreEqual(2, lookupServices.Count, "Should have 2 element");
            Assert.IsTrue(lookupServices.ContainsKey(SecurityIdType.CUSIP), "Should have this lookupService name");
            Assert.IsTrue(lookupServices.ContainsKey(SecurityIdType.ISIN), "Should have this lookupService name");

            // 3 get the recursive to test the builder.
            bool recursive = (bool)AccuracyTestsTestHelper.getPrivateField(manager, "recursiveLookup");
            Assert.IsFalse(recursive, "Should set to false");

            // 4 get the recursive to test the builder.
            bool reference = (bool)AccuracyTestsTestHelper.getPrivateField(manager, "referenceLookup");
            Assert.IsTrue(reference, "Should set to true");

            // 5 get the combiner to test the builder.
            ISecurityDataCombiner combiner = (ISecurityDataCombiner)
                AccuracyTestsTestHelper.getPrivateField(manager, "securityDataCombiner");

            SecurityData data1 = new SecurityData("1", "TopCoder", new string[] { "1", "2", "3" });
            SecurityData data2 = new SecurityData("2", "IBM", new string[] { "4", "2", "3" });

            SecurityData result = combiner.Combine(data1, data2);
            // get the property to test the method.
            Assert.AreEqual("1", result.Id, "The Id property should be set to '1'.");
            Assert.AreEqual("TopCoder", result.CompanyName, "The CompanyName property should be set to 'TopCoder'.");
        }
    }
}
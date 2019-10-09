/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * FinancialSecurityManagerAccuracyTests.cs
 */
using System;
using System.Collections;
using System.Collections.Generic;
using TopCoder.Cache;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;
using TopCoder.FinancialService.Utility.SecurityIdParsers;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>FinancialSecurityManager</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>FinancialSecurityManager</c> class.
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
    public class FinancialSecurityManagerAccuracyTests
    {
        /// <summary>
        /// Represents the <c>FinancialSecurityManager</c> instance used in the test.
        /// </summary>
        FinancialSecurityManager test = null;

        /// <summary>
        /// An instance of <c>ISecurityIdParser</c> used in the test.
        /// </summary>
        ISecurityIdParser idParser = null;

        /// <summary>
        /// An instance of <c>IDictionary</c> used in the test.
        /// </summary>
        IDictionary<string, ISecurityLookupService> lookupServices = null;

        /// <summary>
        /// An instance of <c>ISecurityDataCombiner</c> used in the test.
        /// </summary>
        ISecurityDataCombiner combiner = null;

        /// <summary>
        /// An instance of <c>ICache</c> used in the test.
        /// </summary>
        ICache cache = null;

        /// <summary>
        /// Set Up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            idParser = new DefaultSecurityIdParser();
            lookupServices = new Dictionary<string, ISecurityLookupService>();
            lookupServices.Add(SecurityIdType.SymbolTicker, new SimpleSecurityLookupService());
            combiner = new DefaultSecurityDataCombiner();
            cache = new SimpleCache();
            test = new FinancialSecurityManager(idParser, lookupServices, combiner, false, true, cache);
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
        /// Accuracy Test of the <c>FinancialSecurityManager()</c> constrcutor.
        /// </summary>
        [Test]
        public void FinancialSecurityManager_Ctor1()
        {
            test = new FinancialSecurityManager(lookupServices, combiner, true, false, cache);
            Assert.IsNotNull(test, "The ctor should work well.");

            SecurityIdDetails details = test.Parse("037833100");
            // get the property to test the method.
            Assert.AreEqual("037833100", details.Id, "The Id property should be set to '037833100'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>FinancialSecurityManager()</c> ctor.
        /// </summary>
        [Test]
        public void FinancialSecurityManager_Ctor2()
        {
            Assert.IsNotNull(test, "The ctor should work well.");

            SecurityIdDetails details = test.Parse("037833100");
            // get the property to test the method.
            Assert.AreEqual("037833100", details.Id, "The Id property should be set to '037833100'.");
            Assert.AreEqual(SecurityIdType.CUSIP, details.Type, "The Type property should be set to 'CUSIP'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>ConvertFromCUSIPToISIN()</c> method.
        /// </summary>
        [Test]
        public void ConvertFromCUSIPToISIN()
        {
            string result = test.ConvertFromCUSIPToISIN("037833100");
            // get the property to test the method.
            Assert.AreEqual("US0378331005", result, "The ISIN should be set to 'US0378331005'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>ConvertFromISINToCUSIP()</c> method.
        /// </summary>
        [Test]
        public void ConvertFromISINToCUSIP()
        {
            string result = test.ConvertFromISINToCUSIP("US0378331005");
            // get the property to test the method.
            Assert.AreEqual("037833100", result, "The CUSIP should be set to '037833100'.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = false, ReferenceLookup = true
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker1_LookA()
        {
            // lookup security data by security id 
            // (securityId = "A", so it is a NYSE)
            SecurityData data = test.Lookup("A");
            CheckData_SymbolTicker1_LookA(data, "A");

            // first we check the number of elements in  the cache.
            Assert.AreEqual(3, cache.GetCache().Count, "The cache should only have 3 element.");

            // check the cache key : A
            Object record = cache["A"];
            bool isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            SecurityData recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookA(recordData, "A");

            // check the cache key : B
            record = cache["B"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsFalse(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookA(recordData, "B");

            // check the cache key : C
            record = cache["C"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsFalse(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookA(recordData, "C");
        }

        /// <summary>
        /// Helper method use for check security data get from look up method or in cache.
        /// Lookup with RecursiveLookup = false, ReferenceLookup = true
        /// </summary>
        ///
        /// <param name='data'>the security data to check.</param>
        /// <param name='id'>the excepted id of given security data.</param>
        public void CheckData_SymbolTicker1_LookA(SecurityData data, String id)
        {
            // since the recursiveLookup is false, so the return data should be: 
            // SecurityData(id, "company1", new string[] {"A" "B", "C" })

            // get the property to test the ctor.
            Assert.AreEqual(id, data.Id, "The Id property should be set to " + id);
            Assert.AreEqual("company1", data.CompanyName, "The CompanyName property should be set to 'company1'.");

            ArrayList ids = new ArrayList(data.ReferenceIds);
            // we check the length of the array first.
            Assert.AreEqual(3, ids.Count, "The length of the ids array should be equal.");
            // then we check the element in the array.
            Assert.IsTrue(ids.Contains("A"), "The element in the ids array should be equal.");
            Assert.IsTrue(ids.Contains("B"), "The element in the ids array should be equal.");
            Assert.IsTrue(ids.Contains("C"), "The element in the ids array should be equal.");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = false, ReferenceLookup = true
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker1_LookAAndB()
        {
            // lookup security data by security id 
            // (securityId = "A", so it is a NYSE)
            test.Lookup("A");
            SecurityData data = test.Lookup("B");
            CheckData_SymbolTicker1_LookAAndB(data, "B");

            // first we check the number of elements in  the cache.
            Assert.AreEqual(4, cache.GetCache().Count, "The cache should only have 4 element.");

            // check the cache key : A
            Object record = cache["A"];
            bool isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            SecurityData recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "A");

            // check the cache key : B
            record = cache["B"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "B");

            // check the cache key : C
            record = cache["C"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsFalse(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "C");

            //  check the cache key : D
            record = cache["D"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsFalse(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "D");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = false, ReferenceLookup = true.
        /// It lookups security data by SecurityIdDetails object
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker1_IdDetails()
        {
            test.Lookup("A");
            test.Lookup("B");
            SecurityData data = test.Lookup(test.Parse("A"));
            CheckData_SymbolTicker1_LookAAndB(data, "A");
        }

        /// <summary>
        /// Helper method use for check security data get from look up method or in cache.
        /// </summary>
        ///
        /// <param name='data'>the security data to check.</param>
        /// <param name='id'>the excepted id of given security data.</param>
        public void CheckData_SymbolTicker1_LookAAndB(SecurityData data, String id)
        {
            // since the recursiveLookup is false, so the return data should be: 
            // SecurityData(id, "company1", new string[] {"A" "B", "C" "D"})

            // get the property to test the ctor.
            Assert.AreEqual(id, data.Id, "The Id property should be set to " + id);
            Assert.AreEqual("company1", data.CompanyName, "The CompanyName property should be set to 'company1'.");

            ArrayList ids = new ArrayList(data.ReferenceIds);
            // we check the length of the array first.
            Assert.AreEqual(4, ids.Count, "The length of the ids array should be equal.");
            // then we check the element in the array.
            Assert.IsTrue(ids.Contains("A"), "The element in the ids array should be equal.");
            Assert.IsTrue(ids.Contains("B"), "The element in the ids array should be equal.");
            Assert.IsTrue(ids.Contains("C"), "The element in the ids array should be equal.");
            Assert.IsTrue(ids.Contains("D"), "The element in the ids array should be equal.");
        }


        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = true.
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker2_LookA_True()
        {
            test = new FinancialSecurityManager(idParser, lookupServices, combiner, true, true, cache);
            // lookup security data by security id 
            // (securityId = "A", so it is a NYSE)
            SecurityData data = test.Lookup("A");
            CheckData_SymbolTicker1_LookAAndB(data, "A");

            // first we check the number of elements in  the cache.
            Assert.AreEqual(4, cache.GetCache().Count, "The cache should only have 4 element.");

            // check the cache key : A
            Object record = cache["A"];
            bool isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            SecurityData recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "A");

            // check the cache key : B
            record = cache["B"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "B");

            // check the cache key : C
            record = cache["C"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "C");

            //  check the cache key : D
            record = cache["D"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "D");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = true.
        /// It lookups security data by SecurityIdDetails object
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker2_IdDetails_True()
        {
            test = new FinancialSecurityManager(idParser, lookupServices, combiner, true, true, cache);
            test.Lookup("A");
            SecurityData data = test.Lookup(test.Parse("B"));
            CheckData_SymbolTicker1_LookAAndB(data, "B");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = true.
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker2_LookA_False()
        {
            test = new FinancialSecurityManager(idParser, lookupServices, combiner, true, false, cache);
            // lookup security data by security id 
            // (securityId = "A", so it is a NYSE)
            SecurityData data = test.Lookup("A");
            CheckData_SymbolTicker1_LookAAndB(data, "A");

            // first we check the number of elements in  the cache.
            Assert.AreEqual(4, cache.GetCache().Count, "The cache should only have 4 element.");

            // check the cache key : A
            Object record = cache["A"];
            bool isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            SecurityData recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "A");

            // check the cache key : B
            record = cache["B"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "B");

            // check the cache key : C
            record = cache["C"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "C");

            //  check the cache key : D
            record = cache["D"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookAAndB(recordData, "D");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = true.
        /// It lookups security data by SecurityIdDetails object
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker2_IdDetails_False()
        {
            test = new FinancialSecurityManager(idParser, lookupServices, combiner, true, false, cache);
            test.Lookup("A");
            SecurityData data = test.Lookup(test.Parse("B"));
            CheckData_SymbolTicker1_LookAAndB(data, "B");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = false, ReferenceLookup = false.
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker3_LookA()
        {
            test = new FinancialSecurityManager(idParser, lookupServices, combiner, false, false, cache);
            // lookup security data by security id 
            // (securityId = "A", so it is a NYSE)
            SecurityData data = test.Lookup("A");
            CheckData_SymbolTicker1_LookA(data, "A");

            // first we check the number of elements in  the cache.
            Assert.AreEqual(3, cache.GetCache().Count, "The cache should only have 3 element.");

            // check the cache key : A
            Object record = cache["A"];
            bool isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsTrue(isLookedUp, "The IsLookedUp tag should be set well");
            SecurityData recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookA(recordData, "A");

            // check the cache key : B
            record = cache["B"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsFalse(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookA(recordData, "B");

            // check the cache key : C
            record = cache["C"];
            isLookedUp = (bool)AccuracyTestsTestHelper.getPropertyField(record, "IsLookedUp");
            Assert.IsFalse(isLookedUp, "The IsLookedUp tag should be set well");
            recordData = (SecurityData)AccuracyTestsTestHelper.getPropertyField(record, "SecurityData");
            CheckData_SymbolTicker1_LookA(recordData, "C");
        }

        /// <summary>
        /// Accuracy Test of the <c>Lookup()</c> method.
        /// Lookup with RecursiveLookup = false, ReferenceLookup = false.
        /// </summary>
        [Test]
        public void Lookup_SymbolTicker3_LookAAndB()
        {
            test = new FinancialSecurityManager(idParser, lookupServices, combiner, false, false, cache);
            test.Lookup("A");
            SecurityData data = test.Lookup("B");
            CheckData_SymbolTicker1_LookA(data, "B");
        }
    }
}